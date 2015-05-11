var cluster = require('cluster');
var numCPUs = require('os').cpus().length;
var express = require('express');
var bodyParser = require('body-parser');
var methodOverride = require('method-override');
var http = require('http');
var program = require('commander');
var uuid = require('node-uuid');
var _ = require('lodash');


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

  for(var i = 0; i < Math.ceil(routes/methods.length); i++) {
    var url = '';
    if (i === Math.floor(routes/(methods.length*2))) {
      url = '/request-routing/hello/:id/world';
    } else {
      url = '/request-routing/' + uuid.v4() + '/:id/' + uuid.v4();
    }
    _.forEach(methods, function(method) {
      app[method](url, function (req, res) {
        res.send();
      });
    });
  }

  server.listen(9000, function () {
    console.log('Express server listening on %d', 9000);
  });

}


