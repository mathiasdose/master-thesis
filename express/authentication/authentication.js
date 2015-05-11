/// <reference path="../typings/tsd.d.ts" />

'use strict';

var express = require('express');
var http = require('http');
var _ = require('lodash');
var GoogleStrategy = require( 'passport-google-oauth2' ).Strategy;

var app = module.exports = express();
var server = http.createServer(app);



server.listen(9000, function () {
    console.log('Express server listening on %d', 9000);
});