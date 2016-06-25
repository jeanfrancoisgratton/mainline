#!/bin/bash

pushd .
cd build

case "$1" in

	ubuntu)
		cd ubuntu
		DISTRO="deb";
		;;
	fedora)
		cd fedora
		DISTRO="rh";
		;;
	*)
		echo $0 {ubuntu|fedora}
		exit 0
		;;
esac


docker build -t jfgratton/$DISTRO_devtools .
popd
