-- DROP TABLE IF EXISTS `datapoint_values`;
-- DROP TABLE IF EXISTS `object_tree`;
-- DROP DATABASE IF EXISTS `seminarkurs2014`

-- 1. Schritt: Erstellen der Datenbank

CREATE DATABASE `seminarkurs2014`
DEFAULT CHARSET=utf8 COLLATE=utf8_general_mysql500_ci

-- 2. Schritt: Erstellen der Tabellen und Indizes

SET SQL_MODE="NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";

CREATE TABLE `datapoint_values` (
	`time_stamp` timestamp NOT NULL DEFAULT '0000-00-00 00:00:00',
	`datapoint_id` bigint(20) NOT NULL,
	`datatype` int(11) NOT NULL,
	`int_value` int(11) DEFAULT NULL,
	`decimal_value` double DEFAULT NULL,
	`string_value` varchar(128) COLLATE utf8_general_mysql500_ci DEFAULT NULL,
	PRIMARY KEY (`datapoint_id`,`time_stamp`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_general_mysql500_ci;

CREATE TABLE `object_tree` (
	`object_id` bigint(20) NOT NULL AUTO_INCREMENT,
	`object_parent_id` bigint(20) DEFAULT NULL,
	`object_type` int(11) NOT NULL,
	`object_path` varchar(240) COLLATE ucs2_general_mysql500_ci DEFAULT NULL,
	`object_name` varchar(128) COLLATE ucs2_general_mysql500_ci NOT NULL,
	`object_description` varchar(240) COLLATE ucs2_general_mysql500_ci DEFAULT NULL,
	`object_last_updated` timestamp NULL DEFAULT NULL,
	`sensor_ip_address` varchar(15) COLLATE ucs2_general_mysql500_ci DEFAULT NULL,
	`sensor_port` int(11) DEFAULT NULL,
	`sensor_last_connection` timestamp NULL DEFAULT NULL,
	`datapoint_type` int(11) DEFAULT NULL,
	`datapoint_unit` varchar(20) COLLATE ucs2_general_mysql500_ci DEFAULT NULL,
	`datapoint_calculation` varchar(240) COLLATE ucs2_general_mysql500_ci DEFAULT NULL,
	PRIMARY KEY (`object_id`),
	KEY `object_tree_indx_name` (`object_name`),
	KEY `object_tree_indx_parent` (`object_parent_id`),
	KEY `object_tree_indx_path` (`object_path`),
	KEY `object_tree_indx_type` (`object_type`)
) ENGINE=InnoDB DEFAULT CHARSET=ucs2 COLLATE=ucs2_general_mysql500_ci AUTO_INCREMENT=1 ;

ALTER TABLE `datapoint_values`
	ADD CONSTRAINT `datapoint_values_fkey` FOREIGN KEY (`datapoint_id`) REFERENCES `object_tree` (`object_id`) ON DELETE NO ACTION ON UPDATE NO ACTION;
	