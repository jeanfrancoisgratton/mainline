#!/bin/bash

pushd .
clear

case "$1" in
	ubuntu)
		DISTRO="deb";
		;;

	fedora)
		DISTRO="rh";
		;;

	*)
		echo $0 "{ubuntu|fedora} [destination_dir]"
		exit 0
		;;
esac

echo
echo "Saving with command: docker save -o "$2"jfgratton_"$DISTRO"_devtools.tar jfgratton/"$DISTRO"_devtools"
docker save -o "$2"jfgratton_"$DISTRO"_devtools.tar jfgratton/"$DISTRO"_devtools

echo "Compressing tarball"
bzip2 -9v "$2"jfgratton_"$DISTRO"_devtools.tar

popd
