# RQS and WebQA

RQS (requirements search) is a program developed to quick search the requirements that is used in ISD company.

There are 2 different implementations:
 1. master branch contains standard RQS implementation - standalone desktop application that is used to parse Excel files and find the requirements;
 2. webqa branch contains web-oriented implementation called WebQA - console application that parse Excel files with requirements and create SQLite database for future use. Web server part of the WebQA provide ability to interact with users throw http protocol. Users navigate to WebQA http address using browser and performs searching using web.
 
# Screenshots

## RQS
![Desktop1](https://github.com/kungfux/rqs/blob/wiki/MainWindow.jpg)
![Desktop2](https://github.com/kungfux/rqs/blob/wiki/SearchOptions.jpg)

## WebQA
![Web1](https://github.com/kungfux/rqs/blob/wiki/Screenshot-web1.png)
![Web2](https://github.com/kungfux/rqs/blob/wiki/Screenshot-web2.png)
