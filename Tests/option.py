# Test that Fuse web server correctly handle requests
# These tests checks OPTIONS HTTP request method

import httplib
import socket

fuseHostName = socket.gethostname()
fuseIpAddress = socket.gethostbyname(fuseHostName)

def get_status_code(uri):
	try:
		conn = httplib.HTTPConnection(fuseIpAddress)
		conn.request("OPTIONS", uri)
		return conn.getresponse().status
	except StandardError:
		print "Exception: Connection error. Is Fuse web server started?"
		exit()
		
def do_test(uri, expected_code):
	result = expected_code == get_status_code(uri)
	print "Test is " + ("PASSED" if result else "FAILED") + " for URI: " + ("EMPTY" if uri == "" else uri)
	
"""
Codes list:
	200 - OK
	404 - NOT_FOUND
	500 - INTERNAL_SERVER_ERROR
"""

# Tests
do_test("", 200)