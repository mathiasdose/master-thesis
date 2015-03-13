var express = require('express');
var app = module.exports = express();
var bodyParser = require('body-parser');
var methodOverride = require('method-override');
var server = require('http').createServer(app);



//app.use(bodyParser.urlencoded({ extended: false }));
//app.use(bodyParser.json());
//app.use(methodOverride());


app.get('/json', function (req, res) {
  res.send({ message: 'Hello, World!' })
});

server.listen(9000, function () {
  console.log('Express server listening on %d', 9000);
});