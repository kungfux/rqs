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

def doTest(x):
	telnet = telnetlib.Telnet(fuseIpAddress, fusePort, telnetTimeOut)
	telnet.write(x)
	telnet.read_all()
	telnet.close
	return True

class TelnetConnectionTestCase(unittest.TestCase):	
	def testNoDataSentByClient(self):
		self.assertTrue(doTest(''))
	def testNewLineSentByClient(self):
		self.assertTrue(doTest('\n'))
	def testLsCommandSentByClient(self):
		self.assertTrue(doTest('ln\n'))
	def testHttpOverTelnet(self):
		self.assertTrue(doTest('GET /index.html HTTP/1.1'))
	def testHttpWithParameterOverTelnet(self):
		self.assertTrue(doTest('GET /?by=fr&value=FR1-100 HTTP/1.1'))
	def testHttpHeadOverTelnet(self):
		self.assertTrue(doTest('HEAD /help.html HTTP/1.1'))
