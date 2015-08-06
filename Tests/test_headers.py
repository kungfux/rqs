# Test that Fuse web server correctly handle requests
# These tests checks HTTP headers level responses

import httplib

fuse_host = "localhost"

def get_status_code(uri):
	try:
		conn = httplib.HTTPConnection(fuse_host)
		conn.request("HEAD", uri)
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
do_test("/", 200)
do_test("/index.html", 200)
do_test("/index.html?param=value", 200)
do_test("/index.html?param1=&param2=", 200)

do_test("/../", 404)
do_test("C:\\", 404)
do_test("C:\\pagefile.sys", 404)
do_test("%WINDIR%\\notepad.exe", 404)