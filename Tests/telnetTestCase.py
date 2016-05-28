# Telnet TestCase
# Test there are no problems while processing requests
#	over telnet connection

# Expectations:
#	Fuse server should not fail while processing non standard
# 	requests. Expects that server will return some default data
#	and close the connection

import unittest
import sys
import telnetlib
import socket

# server location
fuseHostName = socket.gethostname()
fuseIpAddress = socket.gethostbyname(fuseHostName)
fusePort = 80

# telnet config
telnetTimeOut = 5
  
def test(x):
	telnet = telnetlib.Telnet(fuseIpAddress, fusePort, telnetTimeOut)
	telnet.write(x)
	telnet.read_all()
	telnet.close
	return True
		
class TelnetConnectionTestCase(unittest.TestCase):	
	def testNoDataSentByClient(self):
		self.assertTrue(test(''))
	def testNewLineSentByClient(self):
		self.assertTrue(test('\n'))
	def testLsCommandSentByClient(self):
		self.assertTrue(test('ln\n'))
	def testHttpOverTelnet(self):
		self.assertTrue(test('GET /index.html HTTP/1.1'))
	def testHttpWithParameterOverTelnet(self):
		self.assertTrue(test('GET /?by=fr&value=FR1-100 HTTP/1.1'))
	def testHttpHeadOverTelnet(self):
		self.assertTrue(test('HEAD /help.html HTTP/1.1'))
