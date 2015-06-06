import urllib2
import time
from multiprocessing import Process
import socket
import sys

def fastCalls(count=1000):
    print "fast calls"
    for i in range(count):
        urllib2.urlopen("http://"+hostIP+"/?by=fr&value=FR1-"+str(i)).read()
        time.sleep(0.2)
    print str(count)+" fast calls done"

def longCalls(count=1000):
    print "long calls"
    for j in range(count) :
        time.sleep(1)
        urllib2.urlopen("http://"+hostIP+"/?by=tms&value=CATHE-"+str(j)).read()
        
        for i in range(count):
            urllib2.urlopen("http://"+hostIP+"/?by=fr&value=FR1-"+str(i)).read()
            time.sleep(0.2)
    print str(count * count)+" long calls done"

def multiProcessCalls(processCount=3):
    processes = []
    for i in range(processCount):
        p = Process(target=fastCalls)
        processes.append(p)
        p.start()

    for p in processes:
        p.join();
    
    print "processes with " + processCount + " processes done"

if __name__ == '__main__':
    hostName = socket.gethostname()
    hostIP = (socket.gethostbyname(hostName))
    print "Host:"+hostName
    print "IP:"+hostIP
    
    fastCalls();
    multiProcessCalls
    longCalls();
    
    
    

