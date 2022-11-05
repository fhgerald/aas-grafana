= Sample of Asset Administration Shell Grafana Connector

This connector used [Simple JSON](https://grafana.com/grafana/plugins/grafana-simple-json-datasource/) Data Source of Grafana. It connects to to an AAS server and makes all properties that uses numbers as data types available.

== Run Grafana using a package manager

See [here](https://grafana.com/docs/grafana/latest/setup-grafana/installation/) for details.

* Windows
  - [Install chocolatey](https://chocolatey.org/install) 
  - Run in shell (with admin rights): ``choco install grafana``

* [Linux Ubuntu 20.04](https://linuxhostsupport.com/blog/how-to-install-grafana-on-ubuntu-20-04/)

* [MacOS](https://grafana.com/docs/grafana/latest/setup-grafana/installation/mac/)


== Getting started with sample code

By default, the AAS Registry is expected at ```http://localhost:4000/registry```. If you want to change this, set environment variable ``GRAF_CONNECTOROPTIONS__REGISTRYURI`` or command line arg ``--CONNECTOROPTIONS:REGISTRYURI={value}``. 

Start the project, check if page https://localhost:7240/swagger/index.html (Swagger Page) is available. 

Access Grafana and install Simple Json using the Plug-Ins page

Add a new data source and configure the IP address if the grafana connector

Create a dashboard.




