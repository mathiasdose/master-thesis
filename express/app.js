var cluster = require('cluster');
var numCPUs = require('os').cpus().length;
var express = require('express');
var bodyParser = require('body-parser');
var methodOverride = require('method-override');
var http = require('http');



//app.use(bodyParser.urlencoded({ extended: false }));
//app.use(bodyParser.json());
//app.use(methodOverride());

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
  app.get('/json', function (req, res) {
    res.send({ message: 'Hello, World!' })
  });

  server.listen(9000, function () {
    console.log('Express server listening on %d', 9000);
  });

}


