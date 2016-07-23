# Headers TestCase
# Test that appropriate response status is given
#	based on requested target

# Expectations:
#	Fuse server should process standard HTTP requests correctly
# 	and should not allow read files not from www directory.
#	Appropriate HTTP response status should be given.

"""
Codes list:
	200 - OK
	301 - Moved Permanently
	400 - Bad Request
	403 - Forbidden
	404 - Not found
	500 - Internal Server Error
	501 - Not Implemented
"""

import unittest
import httplib
import socket

# Server location
fuseHostName = socket.gethostname()
fuseIpAddress = socket.gethostbyname(fuseHostName)

# Request method types
GET = 'GET'
HEAD = 'HEAD'
OPTIONS = 'OPTIONS'
TRACE = 'TRACE'
SEARCH = 'SEARCH'

def getRequestStatus(method, uri):
	conn = httplib.HTTPConnection(fuseIpAddress)
	conn.request(method, uri)
	return conn.getresponse().status

class ResponceHeadersTestCase(unittest.TestCase):

	# GET requests
	#def testGET_EmptyUrlRequestReturns301(self):
	#	self.assertEqual(getRequestStatus(GET, ''), 301)
	def testGET_RootRequestReturns200(self):
		self.assertEqual(getRequestStatus(GET, '/'), 200)
	def testGET_IndexFileRequestReturns200(self):
		self.assertEqual(getRequestStatus(GET, '/index.html'), 200)
	# GET requests Directory Traversal Vulnerability
	def testGET_DTV_Request1Returns403(self):
		self.assertEqual(getRequestStatus(GET, '/..%2f'), 403)
	def testGET_DTV_Request2Returns403(self):
		self.assertEqual(getRequestStatus(GET, '/..%5c'), 403)
	def testGET_DTV_Request3Returns403(self):
		self.assertEqual(getRequestStatus(GET, '/..\\'), 403)
	def testGET_DTV_Request4Returns403(self):
		self.assertEqual(getRequestStatus(GET, '/..\\\\'), 403)
	def testGET_DTV_Request5Returns403(self):
		self.assertEqual(getRequestStatus(GET, '/..\\/'), 403)
	def testGET_DTV_Request6Returns403(self):
		self.assertEqual(getRequestStatus(GET, '/.\..\\'), 403)
	def testGET_DTV_Request7Returns404(self):
		self.assertEqual(getRequestStatus(GET, '/\\127.0.0.1\\'), 404)
	# GET requests Handling Characters
	#def testGET_RequestWithCharacters1Returns301(self):
	#	self.assertEqual(getRequestStatus(GET, '/?.'), 301)
	#def testGET_RequestWithCharacters2Returns301(self):
	#	self.assertEqual(getRequestStatus(GET, '/<.'), 301)
	#def testGET_RequestWithCharacters3Returns301(self):
	#	self.assertEqual(getRequestStatus(GET, '/$.'), 301)
	#def testGET_RequestWithCharacters4Returns301(self):
	#	self.assertEqual(getRequestStatus(GET, '/cc.'), 301)
	# GET requests Commands
	def testGET_SystemDriveRequestReturns404(self):
		self.assertEqual(getRequestStatus(GET, 'C:\\'), 404)
	def testGET_CmdCommandRequestReturns404(self):
		self.assertEqual(getRequestStatus(GET, 'echo%20"Hi"'), 404)

	# HEAD requests
	#def testHEAD_EmptyRequestReturns301(self):
	#	self.assertEqual(getRequestStatus('HEAD', ''), 301)
	def testHEAD_RootRequestReturns200(self):
		self.assertEqual(getRequestStatus('HEAD', '/'), 200)
	def testHEAD_IndexFileRequestReturns200(self):
		self.assertEqual(getRequestStatus('HEAD', '/index.html'), 200)
	def testHEAD_IndexFileWithParameterRequestReturns200(self):
		self.assertEqual(getRequestStatus('HEAD', '/index.html?param=value'), 200)

	# OPTIONS request
	#def testOPTIONS_EmptyRequestReturns301(self):
	#	self.assertEqual(getRequestStatus(OPTIONS, ''), 301)
	def testOPTIONS_RootRequestReturns200(self):
		self.assertEqual(getRequestStatus(OPTIONS, '/'), 200)

	# TRACE request (not implemented)
	def testTRACE_RequestReturns501(self):
		self.assertEqual(getRequestStatus(TRACE, '/index.html'), 501)

	# SEARCH request (not implemented, not known)
	def testSEARCH_RequestReturns400(self):
		self.assertEqual(getRequestStatus(SEARCH, '/index.html'), 400)

	# Incorrect request
	def testEMPTY_RequestReturns400(self):
		self.assertEqual(getRequestStatus('', '/index.html'), 400)
