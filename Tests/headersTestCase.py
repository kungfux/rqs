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

def test(method, uri):
	conn = httplib.HTTPConnection(fuseIpAddress)
	conn.request(method, uri)
	return conn.getresponse().status

def testHead(uri):
	return test('HEAD', uri)
	
def testOptions():
	return test('OPTIONS', '')
		
class HeadersTestCase(unittest.TestCase):
	def testEmptyRequest(self):
		self.assertEqual(testHead(''), 200)
	def testRootRequest(self):
		self.assertEqual(testHead('/'), 200)
	def testIndexFileRequest(self):
		self.assertEqual(testHead('/index.html'), 200)
	def testIndexWithParameterRequest(self):
		self.assertEqual(testHead('/index.html?param=value'), 200)
	def testIndexWithTwoParametersRequest(self):
		self.assertEqual(testHead('/index.html?param1=&param2='), 200)
	def testReadFolderUpRequest(self):
		self.assertEqual(testHead('/../'), 400)
	def testReadSystemDriveRequest(self):
		self.assertEqual(testHead('C:\\'), 404)
	def testReadFileFromSystemDriveRequest(self):
		self.assertEqual(testHead('C:\\pagefile.sys'), 404)
	def testReadFileFromWindowsDirectoryRequest(self):
		self.assertEqual(testHead('%WINDIR%\\notepad.exe'), 404)
	def testOptionsMethod(self):
		self.assertEqual(testOptions(), 200)
