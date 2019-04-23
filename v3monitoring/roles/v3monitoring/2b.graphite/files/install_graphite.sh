#!/bin/bash

if [ ! -f /var/famillegratton.net/ansible/deploy/v3monitoring/install_graphite.deployed ]; then
  useradd -d /opt/graphite graphite
  python /var/famillegratton.net/ansible/files/v3monitoring/get-pip.py
  yum install -y node npm httpd net-snmp perl python-devel git gcc-c++ pycairo mod_wsgi libffi-devel
  pip install django django-tagging pytz Twisted==16.4.1
  export PYTHONPATH="/opt/graphite/lib/:/opt/graphite/webapp/"
  pip install --no-binary=:all: https://github.com/graphite-project/whisper/tarball/master
  pip install --no-binary=:all: https://github.com/graphite-project/carbon/tarball/master
  pip install --no-binary=:all: https://github.com/graphite-project/graphite-web/tarball/master

  cp /opt/graphite/conf/storage-schemas.conf.example /opt/graphite/conf/storage-schemas.conf
  cp /opt/graphite/conf/storage-aggregation.conf.example /opt/graphite/conf/storage-aggregation.conf  
  cp /opt/graphite/conf/graphTemplates.conf.example /opt/graphite/conf/graphTemplates.conf 
  cp /opt/graphite/conf/graphite.wsgi.example /opt/graphite/conf/graphite.wsgi  
  cp /var/famillegratton.net/ansible/files/v3monitoring/local_settings.py /opt/graphite/webapp/graphite/
  cp /opt/graphite/conf/carbon.conf.example /opt/graphite/conf/carbon.conf

  pip install gunicorn
  cp /opt/graphite/conf/graphite.wsgi.example /opt/graphite/webapp/wsgi.py

  PYTHONPATH=/opt/graphite/webapp django-admin.py migrate --settings=graphite.settings --run-syncdb
  PYTHONPATH=/opt/graphite/webapp django-admin.py collectstatic --noinput --settings=graphite.settings
  chown -R graphite:graphite /opt/graphite

  systemctl daemon-reload ; systemctl enable carbon ; systemctl start carbon ; systemctl enable graphite ; systemctl start graphite

  sudo touch /var/famillegratton.net/ansible/deploy/v3monitoring/install_graphite.deployed
  sudo chmod 755 /var/famillegratton.net/ansible/deploy/v3monitoring/install_graphite.deployed
fi
