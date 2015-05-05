'use strict';

var _ = require('lodash');
var fs = require('fs');
var loremIpsum = require('lorem-ipsum');
var program = require('commander');

var loremConfigEscaped = {
  count: 4,
  units: 'paragraphs',
  sentenceLowerBound: 1,
  sentenceUpperBound: 3,
  paragraphLowerBound: 1,
  paragraphUpperBound: 3,
  format: 'html'
};

var loremConfigUnescaped = {
  count: 1,
  units: 'sentences',
  sentenceLowerBound: 10,
  sentenceUpperBound: 10,
  paragraphLowerBound: 1,
  paragraphUpperBound: 1,
  format: 'plain'
};

//var str = fs.readFileSync('./data/unfiltered.json');
var str = fs.readFileSync(__dirname + '/data/filtered.json');
var json = JSON.parse(str);

//console.log(json.results.length);
String.prototype.capitalizeFirstLetter = function () {
  return this.charAt(0).toUpperCase() + this.slice(1);
};

/*var filtered = _.map(json.results, function (result) {
  return {
    email: result.user.email,
    name: result.user.name.first.capitalizeFirstLetter() + ' ' + result.user.name.last.capitalizeFirstLetter(),
    username: '<bold>' + result.user.username + '</bold>',
    description: loremIpsum(loremConfig)
  }
});*/

var filtered = _.map(json, function (user) {
  return {
    unescaped: loremIpsum(loremConfigUnescaped),
    escaped: loremIpsum(loremConfigEscaped)
  }
});


fs.writeFileSync(__dirname + '/data/filtered.json', JSON.stringify(filtered));