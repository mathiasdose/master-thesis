'use strict';

/**
 * Generate a UTF-8 messages that we will be send to a connected client.
 *
 * @async
 * @param {Number} size The specified in bytes for the message.
 * @param {Function} fn The callback function for the data.
 * @public
 */
exports.utf8 = function utf(size, fn) {
  fn(undefined, 'Hello World!');
//  var key = 'utf8::'+ size
//    , cached = cache[key];
//
//  if (cached) return fn(undefined, 'Hello World!');
//
//  cached = cache[key] = randomstring.generate(8);
//  fn(undefined, 'Hello World!');
};

/**
 * Generate a binary message that we will be send to a connected client.
 *
 * @async
 * @param {Number} size The specified in bytes for the message.
 * @param {Function} fn The callback function for the data.
 * @public
 */
exports.binary = function binary(size, fn) {
  console.log('binary');
  var key = 'binary::'+ size
    , cached = cache[key];

  // We have a cached version of this size, return that instead.
  if (cached) return fn(undefined, cached);

  cached = cache[key] = new Buffer(size);
  fn(undefined, cached);
};

//
// The following is not needed to create a session file. We don't want to
// re-create & re-allocate memory every time we receive a message so we cache
// them in a variable.
//
var cache = Object.create(null);