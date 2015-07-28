import urllib2
import time
from multiprocessing import Process
from multiprocessing import Pool
import socket
import sys
 
def fastCalls(hostIP, itrange=1000):
    print "fast calls"
    for i in range(itrange):
        urllib2.urlopen("http://"+hostIP+"/?by=fr&value=FR1-"+str(i)).read()
        time.sleep(0.3)
    print str(itrange)+" fast calls done"
 
def longCalls(hostIP, count=1000):
    print "long calls"
    for j in range(count) :
        time.sleep(1)
        urllib2.urlopen("http://"+hostIP+"/?by=tms&value=CATHE-"+str(j)).read()
       
        for i in range(count):
            urllib2.urlopen("http://"+hostIP+"/?by=fr&value=FR1-"+str(i)).read()
            time.sleep(0.2)
    print str(count * count)+" long calls done"
 
def multiProcessCalls(hostIP, processCount=3):
    processes = []
    print "multiProcessCalls"
    for i in range(processCount):
        p = Process(target=fastCalls, args=(hostIP,))
        p.start()
        processes.append(p)
 
    for proc in processes:
        proc.join();
   
    print "processes with " + str(processCount) + " processes done"
 
if __name__ == '__main__':
    hostName = socket.gethostname()
    hostIP = socket.gethostbyname(hostName)
    print "Host:"+hostName
    print "IP:"+hostIP
 
    multiProcessCalls(hostIP, 5)