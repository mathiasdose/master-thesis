/// <reference path="../typings/tsd.d.ts" />

'use strict';

var express = require('express');
var http = require('http');
var _ = require('lodash');
var jwt = require('jsonwebtoken');
var fs = require('fs');
var cluster = require('cluster');
var numCPUs = require('os').cpus().length;

var PAYLOAD = 'John Doe';
var SECRET = 'SECRET';
var privateKey = fs.readFileSync(__dirname + '/private.pem').toString();
var publicKey = fs.readFileSync(__dirname + '/public.pem').toString();

if (cluster.isMaster) {

  for (var i = 0; i < numCPUs; i++) {
    cluster.fork();
  }

  cluster.on('exit', function(worker, code, signal) {
    console.log('worker ' + worker.pid + ' died');
  });
} else {

  var app = module.exports = express();
  var server = http.createServer(app);
  
  app.get('/jwt/hs256', function (req, res) {
    var token = jwt.sign(PAYLOAD, SECRET, { algorithm: 'HS256'});
    jwt.verify(token, SECRET, { algorithms: ['HS256'] }, function (err, decoded) {
      res.send();  
    });
  });
  
  app.get('/jwt/rs256', function (req, res) {
    var token = jwt.sign(PAYLOAD, privateKey, { algorithm: 'RS256' });
    jwt.verify(token, publicKey, { algorithms: ['RS256'] }, function (err, decoded) {
      res.send();
    });    
  });
  
  app.get('/jwt/es256', function (req, res) {
    var token = jwt.sign(PAYLOAD, privateKey, { algorithm: 'ES256' });
    jwt.verify(token, publicKey, { algorithms: ['ES256'] }, function (err, decoded) {
      res.send();
    });
  });
  
  app.get('/jwt/none', function (req, res) {
    var token = jwt.sign(PAYLOAD, null, { algorithm: 'none' });
    jwt.verify(token, null, { algorithms: ['none'] }, function (err, decoded) {
      res.send();
    });
  });
  
  server.listen(9000, function () {
    console.log('Express server listening on %d', 9000);
  });
    
}    