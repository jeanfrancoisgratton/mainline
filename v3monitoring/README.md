#How to setup the playbook + roles

Graphite + nginX will need a self-signed SSL cert. To create the certificate,  run the following:<br>
`openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout nginx-selfsigned.key -out nginx-selfsigned.crt`<br>

Be sure to move the above files in `/etc/nginx/ssl/` (as per the  `2b.graphite/templates/graphite.conf.j2` templated file)<br>

Influxdb, nginx, graphite and grafana target hosts are defined in monitoring-server.txt.
This file points at the path and variable values that need to be customized to your tastes<br>

Actually, most values *need* to be changed in the various defaults/main.yaml files !<br><br>

Some grafana templates are provided in /var/famillegratton/ansible/files/v3monitoring; look for files matching the pattern `^db*\.json$`. A quick way to import the dashboard is to locally copy the json file and import it through Grafana's UI.

Dashboards with `tig` in the filename are from the telegraf->influxdb->grafana pipeline.
Dashboards with `cgg` in the filename are from the collectd->graphite->grafana pipeline.
Dashboards with `tic` in the filename are from the telegraf->influxdb->chronograf pipeline (none, so far... maybe later !)

**Please note**:

roles/3.grafana/files/certs* are zero-byte files. You will need to create your own certs, or disable https in nginx/grafana.

++*TODO*++ 
- Automate Grafana and Chronograf dashboard imports
- Automate Chronograf configuration (right now, refer to https://docs.influxdata.com/chronograf/v1.7/administration/configuration/)
