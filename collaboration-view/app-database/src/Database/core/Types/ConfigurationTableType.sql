-- <copyright file="ConfigurationTableType.sql" company="Duly Health and Care">
-- Copyright (c) Duly Health and Care. All rights reserved.
-- </copyright>

CREATE TYPE [core].[ConfigurationTableType] AS TABLE
(
	Id                    NVARCHAR (50)  NOT NULL,
    ParentConfigurationId NVARCHAR (50)  NULL,
    ParentTargetId        NVARCHAR (50)  NULL,
    SiteId                NVARCHAR (50)  NULL,
    PatientId             NVARCHAR (50)  NULL,
    ApplicationPart       NVARCHAR (50)  NOT NULL,
    ConfigType            NVARCHAR (50)  NOT NULL,
    TargetAreaType        NVARCHAR (50)  NOT NULL,
    TargetType            NVARCHAR (50)  NOT NULL,
    Tags                  NVARCHAR (500)  NULL,
    Details               NVARCHAR (MAX) NULL,
    PRIMARY KEY CLUSTERED (Id ASC)
)