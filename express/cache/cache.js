/// <reference path="../typings/tsd.d.ts" />

'use strict';
var _ = require('lodash');
var express = require('express');
var http = require('http');
var cluster = require('cluster');
var numCPUs = require('os').cpus().length;
var Chance = require('chance');
var chance = new Chance();
var Promise = require('bluebird');
Promise.promisifyAll(require("redis"));
var redis = require('redis')

var keys = [];
var client = redis.createClient();

client.on("error", function (err) {
	console.log("Error " + err);
});
	
if (cluster.isMaster) {
  // Fork workers.
  for (var i = 0; i < numCPUs; i++) {
    cluster.fork();
  }
	client.flushdbAsync()
		.then(seedData);

} else {

	var app = module.exports = express();
	var server = http.createServer(app);


	app.get('/cache/:amount', function (req, res) {
		var promises = buildPromises(req.params.amount);
		Promise.all(promises)
			.then(function (resolved) {
				res.send();
		});
	});

	server.listen(9000, '0.0.0.0', function () {
		console.log('Express server listening on %d', 9000);
	});

}

function buildPromises(amount) {
	var promises = [];
	for (var i = 0; i < amount; i++) {
		promises.push(
			client.hgetallAsync(keys[chance.integer({ min: 0, max: 100 })]));
	}
	return promises;	
}


function seedData() {
	for (var i = 0; i < 100; i++) {
		var key = 'Hash:' + i;
		keys.push(key);
		client.hmset(key, {
			field1: 'Hello',
			field2: 'World' + i
		});
	}
}



