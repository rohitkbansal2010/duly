-- <copyright file="UIResource.sql" company="Duly Health and Care">
-- Copyright (c) Duly Health and Care. All rights reserved.
-- </copyright>

CREATE TABLE [core].[UIResource] (
    [Id]           NVARCHAR (50)  NOT NULL,
    [Alias]        NVARCHAR (50)  NOT NULL,
    [ResourceType] NVARCHAR (50)  NOT NULL,
    [Details]      NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_UIResource] PRIMARY KEY CLUSTERED ([Id] ASC)
);

