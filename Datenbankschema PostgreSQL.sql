-- DROP INDEX object_tree_indx_name;
-- DROP INDEX object_tree_indx_parent;
-- DROP INDEX object_tree_indx_path;
-- DROP INDEX object_tree_indx_type;
-- DROP TABLE datapoint_values;
-- DROP TABLE object_tree;
-- DROP DATABASE seminarkurs2014;

-- 1. Schritt: Erstellen der Datenbank

CREATE DATABASE seminarkurs2014
	WITH OWNER = postgres
		ENCODING = 'UTF8'
		TABLESPACE = pg_default
		LC_COLLATE = 'de_DE.UTF-8'
		LC_CTYPE = 'de_DE.UTF-8'
		CONNECTION LIMIT = -1;

-- 2. Schritt: Erstellen der Tabellen und Indizes

CREATE TABLE object_tree (
	object_id bigserial NOT NULL,
	object_parent_id bigint,
	object_type integer NOT NULL,
	object_path character varying(240),
	object_name character varying(128) NOT NULL,
	object_description character varying(255),
	object_last_updated timestamp without time zone,
	sensor_ip_address character varying(15),
	sensor_port integer,
	sensor_last_connection timestamp without time zone,
	datapoint_type integer,
	datapoint_unit character varying(20),
	datapoint_calculation character varying(240),
	CONSTRAINT object_tree_pkey PRIMARY KEY (object_id)
) WITH (
	OIDS=FALSE
);

ALTER TABLE object_tree
	OWNER TO postgres;

CREATE TABLE datapoint_values (
	time_stamp timestamp without time zone NOT NULL,
	datapoint_id bigint NOT NULL,
	datatype integer NOT NULL,
	int_value integer,
	decimal_value double precision,
	string_value character varying(128),
	CONSTRAINT datapoint_values_pkey PRIMARY KEY (datapoint_id, time_stamp),
	CONSTRAINT datapoint_values_fkey FOREIGN KEY (datapoint_id)
		REFERENCES object_tree (object_id) MATCH SIMPLE
		ON UPDATE NO ACTION ON DELETE NO ACTION
) WITH (
  OIDS=FALSE
);

ALTER TABLE datapoint_values
	OWNER TO postgres;

CREATE INDEX object_tree_indx_name
	ON object_tree
	USING btree
	(object_name COLLATE pg_catalog."default");

CREATE INDEX object_tree_indx_parent
	ON object_tree
	USING btree
	(object_parent_id);

CREATE INDEX object_tree_indx_path
	ON object_tree
	USING btree
	(object_path COLLATE pg_catalog."default" varchar_pattern_ops);

CREATE INDEX object_tree_indx_type
	ON object_tree
	USING btree
	(object_type);
