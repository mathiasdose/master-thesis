'use strict';
var _ = require('lodash');
var program = require('commander');
var fs = require("fs");

program
  .version('0.0.1')
  .usage('[options]')
  .option('-s, --size <n>', 'Size of JSON file to generate', parseInt)
  .parse(process.argv);


var DIRECTORY = './data/';
var FILETYPE = '.json';

var jsonToWrite = [];

console.log(getFilesizeInBytes('10'));

var sizeInKB;
if (program.size) {
  sizeInKB = program.size;
} else {
  console.log('Did not provide a size, see --help');
  process.exit(1);
}

initiateFile(program.size, jsonToWrite);

var index = 0;
/*
while(true) {
  var jsonIter = getJSON(index);
  index++;
  break;
}*/



function getFilesizeInBytes(filename) {
  var stats = fs.statSync(DIRECTORY + filename + FILETYPE);
  return stats.size
}

function initiateFile(filename) {
  //var emptyArray = JSON.stringify([]);
  var path = DIRECTORY + filename + FILETYPE;
  console.log(path);
  fs.writeSync(path, "YOLO");
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

  

