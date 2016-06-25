-- --------------------------------------------------------
-- Host:                         london
-- Server version:               10.0.17-MariaDB-0ubuntu1-log - (Ubuntu)
-- Server OS:                    debian-linux-gnu
-- HeidiSQL Version:             9.2.0.4947
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping database structure for recus_dev
DROP DATABASE IF EXISTS `recus_dev`;
CREATE DATABASE IF NOT EXISTS `recus_dev` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `recus_dev`;


-- Dumping structure for table recus_dev.categories
DROP TABLE IF EXISTS `categories`;
CREATE TABLE IF NOT EXISTS `categories` (
  `cID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `cName` varchar(45) NOT NULL,
  PRIMARY KEY (`cID`),
  UNIQUE KEY `cID_UNIQUE` (`cID`),
  UNIQUE KEY `cName_UNIQUE` (`cName`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Dumping data for table recus_dev.categories: ~0 rows (approximately)
DELETE FROM `categories`;
/*!40000 ALTER TABLE `categories` DISABLE KEYS */;
/*!40000 ALTER TABLE `categories` ENABLE KEYS */;


-- Dumping structure for table recus_dev.entrees
DROP TABLE IF EXISTS `entrees`;
CREATE TABLE IF NOT EXISTS `entrees` (
  `eID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `eBillID` varchar(20) DEFAULT NULL,
  `eItem` int(10) unsigned NOT NULL DEFAULT '1',
  `eAmount` decimal(10,2) NOT NULL,
  `eEffectiveAmount` decimal(10,2) DEFAULT NULL,
  `EDateInvoiced` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `eDateEntered` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`eID`),
  UNIQUE KEY `eID` (`eID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Dumping data for table recus_dev.entrees: ~0 rows (approximately)
DELETE FROM `entrees`;
/*!40000 ALTER TABLE `entrees` DISABLE KEYS */;
/*!40000 ALTER TABLE `entrees` ENABLE KEYS */;


-- Dumping structure for table recus_dev.usagers
DROP TABLE IF EXISTS `usagers`;
CREATE TABLE IF NOT EXISTS `usagers` (
  `cID` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `uName` varchar(45) NOT NULL,
  `uLogin` varchar(16) NOT NULL,
  `uPassword` text,
  `uLastLogin` datetime DEFAULT NULL,
  `uLastIP` varchar(15) DEFAULT '"127.0.0.1"',
  `uIsAdmin` enum('0','1') DEFAULT '0',
  PRIMARY KEY (`cID`),
  UNIQUE KEY `cID` (`cID`),
  UNIQUE KEY `uLogin` (`uLogin`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Dumping data for table recus_dev.usagers: ~0 rows (approximately)
DELETE FROM `usagers`;
/*!40000 ALTER TABLE `usagers` DISABLE KEYS */;
/*!40000 ALTER TABLE `usagers` ENABLE KEYS */;
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
