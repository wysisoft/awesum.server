-- This script was generated by the ERD tool in pgAdmin 4.
-- Please log an issue at https://github.com/pgadmin-org/pgadmin4/issues/new/choose if you find any bugs, including reproduction steps.
BEGIN;


CREATE TABLE IF NOT EXISTS public.apps
(
    id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    "manualId" text COLLATE pg_catalog."default",
    email text COLLATE pg_catalog."default",
    loginid text COLLATE pg_catalog."default",
    name text COLLATE pg_catalog."default",
    "lastModified" timestamp without time zone,
    "homePageIcon" text COLLATE pg_catalog."default",
    "uniqueId" uuid,
    deleted boolean,
    CONSTRAINT apps_pkey PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS public."databaseItems"
(
    id integer NOT NULL,
    letters text COLLATE pg_catalog."default",
    "order" integer,
    image text COLLATE pg_catalog."default",
    sound text COLLATE pg_catalog."default",
    type integer,
    "unitId" integer,
    "rewardType" integer,
    reward text COLLATE pg_catalog."default",
    "grouping" integer,
    "lastModified" timestamp without time zone,
    text text COLLATE pg_catalog."default",
    "databaseId" integer,
    "appId" integer,
    "uniqueId" uuid,
    deleted boolean,
    CONSTRAINT "databaseItems_pkey" PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS public."databaseUnits"
(
    id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    type integer,
    name text COLLATE pg_catalog."default",
    "order" integer,
    "lastModified" timestamp without time zone,
    "databaseId" integer,
    "appId" integer,
    "uniqueId" uuid,
    deleted boolean,
    CONSTRAINT "databaseProperties_pkey" PRIMARY KEY (id)
);

CREATE TABLE IF NOT EXISTS public.databases
(
    id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    "appId" integer,
    name text COLLATE pg_catalog."default",
    "default" timestamp without time zone,
    "lastModified" timestamp without time zone,
    "manualId" text COLLATE pg_catalog."default",
    "uniqueId" uuid,
    deleted boolean
);

CREATE TABLE IF NOT EXISTS public.followers
(
    id integer NOT NULL GENERATED ALWAYS AS IDENTITY ( INCREMENT 1 START 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 1 ),
    "leaderAppId" integer,
    "followerAppId" integer,
    "followerName" text COLLATE pg_catalog."default",
    "followerEmail" text COLLATE pg_catalog."default",
    "leaderName" text COLLATE pg_catalog."default",
    "leaderEmail" text COLLATE pg_catalog."default",
    "lastModified" timestamp without time zone,
    "leaderAccepted" boolean,
    "leaderRemoved" boolean,
    "uniqueId" uuid,
    deleted boolean,
    CONSTRAINT followrequests_pkey PRIMARY KEY (id)
);
END;