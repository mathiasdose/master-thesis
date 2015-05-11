/// <reference path="../typings/tsd.d.ts" />

'use strict';


var http = require('http');
var express = require('express');
var cluster = require('cluster');
var numCPUs = require('os').cpus().length;

//numCPUs = 3;
if (cluster.isMaster) {
  // Fork workers.
  for (var i = 0; i < numCPUs; i++) {
    cluster.fork();
  }
} else {
  var WebSocketServer = require('websocket').server;
  var app = express();
  var server = http.createServer(app);


  server.listen(9000, function () {
    console.log((new Date()) + ' Server is listening on port 9000');
  });

  var wsServer = new WebSocketServer({
    httpServer: server,
    autoAcceptConnections: true
  });

  wsServer.on('connect', function (connection) {
    connection.on('message', function (message) {
      if (message.type === 'utf8') {
        console.log('Received Message: ' + message.utf8Data);
        connection.sendUTF(message.utf8Data);
      }
    });
    //  connection.on('close', function(reasonCode, description) {
    //    console.log('Peer ' + connection.remoteAddress + ' disconnected.');
    //  });
  });

}

