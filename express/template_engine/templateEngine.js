'use strict';

var cluster = require('cluster');
var numCPUs = require('os').cpus().length;
var express = require('express');
var http = require('http');
var program = require('commander');
var _ = require('lodash');
var fs = require('fs');
var Handlebars = require('handlebars');



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

  var users = JSON.parse(fs.readFileSync('./data/filtered.json'));

  var variables = {
    '10': users.slice(0, 10),
    '100': users,
    '1000': times(users, 10),
    '10000': times(users, 100)
  };
  var data = {};

  var preCompiled = compileHandlebar();

  app.get('/template-engine/:dataLength', function (req, res) {
    data.users = variables[req.params.dataLength];
    var html = preCompiled(data);
    res.send(html);
  });


  server.listen(9000, '0.0.0.0', function () {
    console.log('Express server listening on %d', 9000);
  });

}

function registerPartials() {
  var userStr = fs.readFileSync('./templates/partial.handlebars', 'utf8');
  Handlebars.registerPartial('object', userStr);
}

function compileHandlebar() {
  registerPartials();
  var str = fs.readFileSync('./templates/main.handlebars', 'utf8');
  return Handlebars.compile(str);
}

function times(array ,n) {
  var ret = [];
  for (var i = 0; i < n; i++) {
    ret = ret.concat(array)
  }
  return ret;
}

