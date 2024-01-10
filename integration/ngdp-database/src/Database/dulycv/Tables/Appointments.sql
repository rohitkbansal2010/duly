-- <copyright file="Appointments.sql" company="Duly Health and Care">
-- Copyright (c) Duly Health and Care. All rights reserved.
-- </copyright>

CREATE TABLE [dulycv].[Appointments] (
    [pat_id]                  VARCHAR (18)   NOT NULL,
    [csn_id]                  NUMERIC (18)   NOT NULL,
    [department_id]           NUMERIC (18)   NULL,
    [department_name]         VARCHAR (254)  NULL,
    [location_id]             NUMERIC (18)   NULL,
    [location_name]           VARCHAR (80)   NULL,
    [appt_status_id]          INT            NULL,
    [appt_status_name]        VARCHAR (254)  NULL,
    [visit_type_id]           VARCHAR (18)   NULL,
    [visit_type]              VARCHAR (200)  NULL,
    [visit_type_display_name] VARCHAR (200)  NULL,
    [appt_time]               DATETIME       NULL,
    [appt_length]             INT            NULL,
    [appt_note]               VARCHAR (1000) NULL,
    [visit_prov_id]           VARCHAR (18)   NULL,
    [provider_name]           VARCHAR (254)  NULL,
    [NPI]                     VARCHAR (10)   NULL,
    [telehealth_enc]          VARCHAR (1)    NOT NULL,
    [btg_flag]                VARCHAR (1)    NOT NULL,
    [enc_type_id]             VARCHAR (66)   NULL,
    [enc_type_name]           VARCHAR (254)  NULL
);