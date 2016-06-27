# Load Test
# Test that server can process requests normally on high load

# Expectations:
#	Fuse server should not fail while processing tons of requests

import unittest
import httplib
import socket
import urllib2
import time
from multiprocessing import Process
from multiprocessing import Pool
import sys

# server location
fuseHostName = socket.gethostname()
fuseIpAddress = socket.gethostbyname(fuseHostName)
fuseWebAddress = 'http://{0}/?value='.format(fuseIpAddress)

# load parameters
repeatRequestCount = 1000
processesCount = 50

def doFastCalls():
    for i in range(repeatRequestCount):
        urllib2.urlopen(fuseWebAddress + str(i)).read()
        time.sleep(0.3)
		
def doLongCalls():
    for j in range(repeatRequestCount):
        time.sleep(1)
        urllib2.urlopen(fuseWebAddress + str(j)).read()
       
        for i in range(repeatRequestCount):
            urllib2.urlopen(fuseWebAddress + str(i)).read()
            time.sleep(0.2)
			
def doMultiProcessCalls():
    processes = []
    
    for i in range(processesCount):
        p = Process(target=doFastCalls)
        p.start()
        processes.append(p)
 
    for proc in processes:
        proc.join();
		
class LoadTestCase(unittest.TestCase):
	def testHighLoad(self):
		self.assertFalse(doMultiProcessCalls())