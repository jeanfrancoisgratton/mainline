#!/bin/bash

if [ ! -f /var/famillegratton.net/ansible/deploy/v3monitoring/grafana-plugins.deployed ]; then
  for i in briangann-gauge-panel digiapulssi-breadcrumb-panel digrich-bubblechart-panel grafana-piechart-panel natel-discrete-panel petrslavotinek-carpetplot-panel satellogic-3d-globe-panel vonage-status-panel grafana-worldmap-panel;do
    /usr/sbin/grafana-cli plugins install $i;done

  sudo touch /var/famillegratton.net/ansible/deploy/v3monitoring/grafana-plugins.deployed
  sudo chmod 755 /var/famillegratton.net/ansible/deploy/v3monitoring/grafana-plugins.deployed
fi
