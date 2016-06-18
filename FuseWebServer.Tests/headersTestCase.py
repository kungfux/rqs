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
	404 - NOT_FOUND
	500 - INTERNAL_SERVER_ERROR
"""

import unittest
import httplib
import socket

# server location
fuseHostName = socket.gethostname()
fuseIpAddress = socket.gethostbyname(fuseHostName)

# Headers
GET = 'GET'
HEAD = 'HEAD'
OPTIONS = 'OPTIONS'
TRACE = 'TRACE'

def doTest(method, uri):
	conn = httplib.HTTPConnection(fuseIpAddress)
	conn.request(method, uri)
	return conn.getresponse().status
	
class ResponceHeadersTestCase(unittest.TestCase):
	# Incorrect request
	def testIncorrectRequest(self):
		self.assertEqual(doTest('', '/index.html'), 404)
		
	# GET request
	def testGetEmptyRequest(self):
		self.assertEqual(doTest(GET, ''), 200)
	def testGetRootRequest(self):
		self.assertEqual(doTest(GET, '/'), 200)
	def testGetIndexFileRequest(self):
		self.assertEqual(doTest(GET, '/index.html'), 200)
	def testGetFolderUpLevelRequest(self):
		self.assertEqual(doTest(GET, '/../'), 403)
	def testGetSystemDriveRequest(self):
		self.assertEqual(doTest(GET, 'C:\\'), 404)
	def testGetCmdCommandRequest(self):
		self.assertEqual(doTest(GET, 'echo "Hi"'), 404)
	
	# HEAD requests
	def testHeadEmptyRequest(self):
		self.assertEqual(doTest('HEAD', ''), 200)
	def testHeadRootRequest(self):
		self.assertEqual(doTest('HEAD', '/'), 200)
	def testHeadIndexFileRequest(self):
		self.assertEqual(doTest('HEAD', '/index.html'), 200)
	def testHeadIndexWithParameterRequest(self):
		self.assertEqual(doTest('HEAD', '/index.html?param=value'), 200)
	def testHeadIndexWithTwoParametersRequest(self):
		self.assertEqual(doTest('HEAD', '/index.html?param1=&param2='), 200)
	def testHeadReadFolderUpRequest(self):
		self.assertEqual(doTest('HEAD', '/../'), 403)
	def testHeadReadSystemDriveRequest(self):
		self.assertEqual(doTest('HEAD', 'C:\\'), 404)
	def testHeadReadFileFromSystemDriveRequest(self):
		self.assertEqual(doTest('HEAD', 'C:\\pagefile.sys'), 404)
	def testHeadReadFileFromWindowsDirectoryRequest(self):
		self.assertEqual(doTest('HEAD', '%WINDIR%\\notepad.exe'), 404)
		
	# OPTIONS request
	def testOptionsMethod(self):
		self.assertEqual(doTest(OPTIONS, ''), 200)
		
	# TRACE request (not suitable for now)
	#def testTraceMethod(self):
	#	self.assertEqual(doTest(TRACE, '/index.html'), 405)