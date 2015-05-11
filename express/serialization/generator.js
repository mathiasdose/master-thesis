'use strict';
var _ = require('lodash');
var program = require('commander');
var Promise = require('bluebird');
var fs = Promise.promisifyAll(require("fs"));
program
  .version('0.0.1')
  .usage('[options]')
  .option('-s, --size <n>', 'Size of JSON file to generate', parseInt)
  .parse(process.argv);


var DIRECTORY = './data/';
var FILETYPE = '.json';

var sizeInKB;
if (program.size) {
  sizeInKB = program.size;
} else {
  console.log('Did not provide a size, see --help');
  process.exit(1);
}

writeJsonToFile(sizeInKB);


function getFilesizeInBytes(filename) {
  var stats = fs.statSync(DIRECTORY + filename + FILETYPE);
  return stats.size
}

function writeJsonToFile(filename) {
  var path = DIRECTORY + filename + FILETYPE;
  var json = [];

  var size = 0;
  var counter = 0;
  for (var i = 0; i < sizeInKB*1000/205; i++) {
    json.push(getJSON(i));
  }
  var fd = fs.openSync(path, 'w');
  fs.writeSync(fd, JSON.stringify(json));

 /* while (size < sizeInKB * 1000) {
    json.push(getJSON(counter));
    var fd = fs.openSync(path, 'w');
    fs.writeSync(fd, JSON.stringify(json));
    counter++;
    size = getFilesizeInBytes(filename);
  }*/
  fs.closeSync(fd);
}


function getJSON(index) {
  return {
    Id: index,
    aNull: null,
    aBoolean: true,
    anInteger: 68523,
    aFloat: 6.8523015e+5,
    aString: 'Testing s',
    anArray: [null, true, 123, "any"],
    anObject: { aNull: null, aBoolean: true, anInteger: 123, aString: 'any' }
  };
}

console.log('Wrote ' + getFilesizeInBytes(sizeInKB) + 'bytes to ' + DIRECTORY + sizeInKB + FILETYPE);
  

