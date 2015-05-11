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

var privateRSAKey = fs.readFileSync(__dirname + '/private.pem').toString();
var publicRSAKey = fs.readFileSync(__dirname + '/public.pem').toString();
var privateECKey = fs.readFileSync(__dirname + '/privateec.pem').toString();
var publicECKey = fs.readFileSync(__dirname + '/cert.pem').toString();
var secretKey = '0rtfaE3N58pPkQ7UURL6H4D4Ostht0N1';
    
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
    var token = jwt.sign(PAYLOAD, secretKey, { algorithm: 'HS256'});
    jwt.verify(token, secretKey, { algorithms: ['HS256'] }, function (err, decoded) {
      res.send(decoded);  
    });
  });
  
  app.get('/jwt/rs256', function (req, res) {
    var token = jwt.sign(PAYLOAD, privateRSAKey, { algorithm: 'RS256' });
    jwt.verify(token, publicRSAKey, { algorithms: ['RS256'] }, function (err, decoded) {
      res.send(decoded);
    });    
  });
  
  app.get('/jwt/es256', function (req, res) {
    var token = jwt.sign(PAYLOAD, privateECKey, { algorithm: 'ES256' });
    jwt.verify(token, publicECKey, { algorithms: ['ES256'] }, function (err, decoded) {
      res.send(decoded);
    });
  });
  
  app.get('/jwt/none', function (req, res) {
    var token = jwt.sign(PAYLOAD, null, { algorithm: 'none' });
    jwt.verify(token, null, { algorithms: ['none'] }, function (err, decoded) {
      res.send(decoded);
    });
  });
  
  server.listen(9000, function () {
    console.log('Express server listening on %d', 9000);
  });
    
}    