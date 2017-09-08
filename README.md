## Introduction

Please follow [this](https://github.com/kungfux/rqs/wiki/Introduction-of-jRQS) link to see more screenshots on the Wiki.

![jRQS main page](https://user-images.githubusercontent.com/10548881/30138071-14bef154-936f-11e7-8ff8-757ab3b80241.png)

## How to build

The project may be built using NetBeans. Open the `jrqs-app` project to build and app, `jrqs-tests` to perform Selenium tests.

## How to run

The application may be run using Tomcat. Deploy jRQS right from the NetBeans or build and deploy .war release. 
To work properly, jRQS requires a SQLite database file to be located in the `$CATALINA_HOME/data/webqa.sqlite`. 
The database may be prepared using parsing utility from WebQA branch.
