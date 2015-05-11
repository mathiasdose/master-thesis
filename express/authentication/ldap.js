/// <reference path="../typings/tsd.d.ts" />

'use strict';

var ldap = require('ldapjs');
var fs = require('fs');

var server = ldap.createServer();

function authorize(req, res, next) {
  if (!req.connection.ldap.bindDN.equals('cn=root'))
    return next(new ldap.InsufficientAccessRightsError());

  return next();
}

function loadPasswdFile(req, res, next) {
  req.users = {
	  'root': {
		  dn: 'cn=root, ou=users, o=myhost',
		  cn: 'root',
      uid: 0,
      gid: 0,
      description: 'System Administrator',
      homedirectory: '/var/root',
      shell: '/bin/sh',
      objectclass: 'unixUser'
	  },
    'notroot': {
		  dn: 'cn=root, ou=users, o=myhost',
		  cn: 'root',
      uid: 0,
      gid: 0,
      description: 'System Administrator',
      homedirectory: '/var/root',
      shell: '/bin/sh',
      objectclass: 'unixUser'
	  }
  };
  return next();

}

var pre = [authorize, loadPasswdFile];

server.search('o=myhost', pre, function(req, res, next) {

  Object.keys(req.users).forEach(function(k) {
      console.log(k);
      if (req.filter.matches(req.users[k].attributes))
        console.log('matched');
//    if (req.filter.matches(req.users[k].attributes))
//      res.send(req.users[k]);
  });

  res.end();
  return next();
});

server.bind('cn=root', function(req, res, next) {
  if (req.dn.toString() !== 'cn=root' || req.credentials !== 'secret')
    return next(new ldap.InvalidCredentialsError());

  res.end();
  return next();
});

server.listen(1389, function() {
  console.log('ldapjs listening at ' + server.url);
});