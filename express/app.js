var cluster = require('cluster');
var numCPUs = require('os').cpus().length;
var express = require('express');
var bodyParser = require('body-parser');
var methodOverride = require('method-override');
var http = require('http');
var program = require('commander');
var uuid = require('node-uuid');


program
  .version('0.0.1')
  .option('-r, --routes <n>', 'Number of host to deploy', parseInt)
  .parse(process.argv);

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


  var routes = program.routes === undefined ? 1: program.routes;
  var methods = ['get', 'post', 'put', 'delete'];
  var div = routes/methods.length;
  console.log(methods[Math.floor(100000 / div)]);
  for(var i = 0; i < routes; i++) {
    if (i === routes/2) {
      app.get('/json/hello/:id/world', function (req, res) {
        res.send();
      });
    } else {
      var method = methods[Math.floor(i / div)];
      app[method]('/json/' + uuid.v4() + '/:id/' + uuid.v4(), function (req, res) {
        res.send();
      });
    }
  }

  server.listen(9000, function () {
    console.log('Express server listening on %d', 9000);
  });

}


