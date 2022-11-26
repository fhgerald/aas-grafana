# Sample of Asset Administration Shell Grafana Connector

This connector used [SimpleJSON](https://grafana.com/grafana/plugins/grafana-simple-json-datasource/) Data Source of Grafana. It connects to an AAS server and makes all properties that uses numbers as data types available.

## Install Grafana OSS

See [here](https://grafana.com/docs/grafana/latest/setup-grafana/installation/) for further details to install Grafana (OSS). 

* *Windows*
  - Use the [Windows Installer](https://grafana.com/grafana/download?edition=oss&pg=get&platform=windows&plcmt=selfmanaged-box1-cta1) which describes the Windows installer package or a standalone Windows binary file.

  or alternatively you can use a package manager for your installation process 
  - [Install chocolatey](https://chocolatey.org/install) 
  - Run in shell (with admin rights): ``choco install grafana``
  
* *Linux* 
   - Follow Grafanas [Install Documentation](https://grafana.com/docs/grafana/latest/setup-grafana/installation/) which explains how to install Grafana dependencies, download and install Grafana, get the service up and running on your Linux system, and also describes the installation package details.

   or if you prefer to communicate via Reverse Proxy with the Grafana server in your Ubuntu 20.04
   -  Follow [How to Install Grafana on Ubuntu 20.04](https://linuxhostsupport.com/blog/how-to-install-grafana-on-ubuntu-20-04/)

* *MacOS*
  - [MacOS](https://grafana.com/docs/grafana/latest/setup-grafana/installation/mac/) explains how to install Grafana and get the service running on your macOS.

After installation start and check your Grafana instance via http://localhost:3000, username ``admin`` and password ``admin``. 

## Prepare Grafana 

Because we need a generic JSON backend datasource for our project, install the SimpleJSON Plugin via [Plug-Ins page](http://localhost:3000/plugins?filterBy=all&filterByType=all&q=JSON)

By default, the AAS Registry is expected at ```http://localhost:4000/registry```. If you want to change this, set environment variable ``GRAF_CONNECTOROPTIONS__REGISTRYURI`` or command line arg ``--CONNECTOROPTIONS:REGISTRYURI={value}``. 

## Getting started with sample code

[_TODO_: Complete the following description]

Before we start with our project, check if page https://localhost:7240/swagger/index.html (Swagger Page) is available.

Add a new data source and configure the IP address if the grafana connector...???? 

Create a dashboard.




