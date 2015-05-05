'use strict';

var cluster = require('cluster');
var numCPUs = require('os').cpus().length;
var express = require('express');
var http = require('http');
var program = require('commander');
var _ = require('lodash');
var fs = require('fs');

var DIRECTORY = './data/';
var FILETYPE = '.json';

if (cluster.isMaster) {
  // Fork workers.
  for (var i = 0; i < numCPUs; i++) {
    cluster.fork();
  }

  cluster.on('exit', function(worker, code, signal) {
    console.log('worker ' + worker.pid + ' died');
  });
} else {
  var app = module.exports = express();
  var server = http.createServer(app);
  
  var variables = ['10', '50', '100', '500', '1000', '5000', '10000'];
  var jsonInMemory = {};


  _.forEach(variables, function(variable) {
    var path = DIRECTORY + variable + FILETYPE;
    jsonInMemory[variable] = fs.readFileSync(path, 'utf8');
  });

  app.get('/serialization/:size', function (req, res) {
    var size = req.params.size;
    var deSerialized = JSON.parse(jsonInMemory[size]);
    var serialized = JSON.stringify(deSerialized);
    res.send();
  });



  server.listen(9000, function () {
    console.log('Express server listening on %d', 9000);
  });

}

