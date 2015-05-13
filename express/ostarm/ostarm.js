/// <reference path="../typings/tsd.d.ts" />

'use strict';

var express = require('express');
var http = require('http');
var Chance = require('chance');
var Sequelize = require('sequelize');
var cluster = require('cluster');
var numCPUs = require('os').cpus().length;


var chance = new Chance();

var sequelize = new Sequelize('test', 'mathiasdose', null, {
		host: 'localhost',
		port: 3306,
		dialect: 'mysql',
		logging: false,
		protocol: 'tcp'
	});

	var world = sequelize.define('world', {
		id: {
			type: Sequelize.INTEGER,
			primaryKey: true,
			field: 'id',
			autoIncrement: true
		},
		randomInteger: {
			type: Sequelize.INTEGER,
			allowNull: false
		},
		randomString: {
			type: Sequelize.STRING,
			allowNull: false
		},
		randomDecimal: {
			type: Sequelize.DECIMAL,
			allowNull: false
		},
		randomDate: {
			type: Sequelize.DATE,
			allowNull: false
		}
	}, {
			freezeTableName: true,
			timestamps: false
		});


if (cluster.isMaster) {
  // Fork workers.
  for (var i = 0; i < numCPUs; i++) {
    cluster.fork();
  }
	
	// Seed data
	sequelize.dropSchema('world')
		.then(function () {
		return world.sync();
	})
		.then(function () {
		seedData();
	});

  cluster.on('exit', function (worker, code, signal) {
    console.log('worker ' + worker.pid + ' died');
  });
} else {

	var app = module.exports = express();
	var server = http.createServer(app);

	app.get('/ostarm/read', function (req, res) {
		var identifier = chance.integer({ min: 1, max: 100 });
		world.find(identifier)
			.then(function (result) {
			res.send();
		});
	});

	app.get('/ostarm/readAll', function (req, res) {
		world.findAll()
			.then(function (result) {
			res.send();
		});
	});

	app.get('/ostarm/update', function (req, res) {
		var identifier = chance.integer({ min: 1, max: 100 });
		world.update(getRandomifiedWorldObject(), { where: { id: identifier } })
			.then(function (result) {
			res.send();
		});
	});

	app.get('/ostarm/create', function (req, res) {
		world.create(getRandomifiedWorldObject())
			.then(function (data) {
			res.send();
		});
	});

	server.listen(9000, function () {
		console.log('Express server listening on %d', 9000);
	});
}



function seedData() {
	var worlds = getRandomifiedWorldObjects();
	return world.bulkCreate(worlds)
		.then(function () {
			console.log('data was seeded...');
		});
}

function getRandomifiedWorldObjects() {
	var worlds = [];

	for (var i = 0; i < 100; i++) {
		worlds.push(getRandomifiedWorldObject());
	}
	return worlds;
}

function getRandomifiedWorldObject() {
	return getWorldObject();
	return {
		randomInteger: chance.integer({ min: 0, max: 10000 }),
		randomString: chance.string({ length: 10 }),
		randomDecimal: chance.floating({ min: 0, max: 100 }),
		randomDate: chance.date()
	};
}

var date = new Date();

function getWorldObject() {
	return {
		randomInteger: 10,
		randomString: 'Hello world',
		randomDecimal: 0.5,
		randomDate: date
	};
}


