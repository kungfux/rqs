import urllib2
import time
from multiprocessing import Process
from multiprocessing import Pool
import socket
import sys
 
def fastCalls(hostIP, itrange=1000):
    for i in range(itrange):
        urllib2.urlopen("http://"+hostIP+"/?value="+str(i)).read()
        time.sleep(0.3)
	print ".",
 
def longCalls(hostIP, count=1000):
    for j in range(count) :
        time.sleep(1)
        urllib2.urlopen("http://"+hostIP+"/?value="+str(j)).read()
       
        for i in range(count):
            urllib2.urlopen("http://"+hostIP+"/?value="+str(i)).read()
            time.sleep(0.2)
 
def multiProcessCalls(hostIP, processCount=50):
    processes = []
    print "50's processes will perform 1000 requests each"
    for i in range(processCount):
        p = Process(target=fastCalls, args=(hostIP, 1000))
        p.start()
        processes.append(p)
 
    for proc in processes:
        proc.join();
   
    print "done"
 
if __name__ == '__main__':
    hostName = socket.gethostname()
    hostIP = socket.gethostbyname(hostName)
    print "Host:"+hostName
    print "IP:"+hostIP
 
    multiProcessCalls(hostIP, 50)