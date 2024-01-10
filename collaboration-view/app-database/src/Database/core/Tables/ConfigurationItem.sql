-- <copyright file="ConfigurationItem.sql" company="Duly Health and Care">
-- Copyright (c) Duly Health and Care. All rights reserved.
-- </copyright>

CREATE TABLE [core].[ConfigurationItem] (
    [ConfigurationId] NVARCHAR (50)  NOT NULL,
    [ItemTargetId]    NVARCHAR (50)  NOT NULL,
    [ItemTargetType]  NVARCHAR (50)  NOT NULL,
    [Index]           INT            NOT NULL,
    [Details]         NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_ConfigurationItem] PRIMARY KEY CLUSTERED (
	[ConfigurationId] ASC,
	[ItemTargetId] ASC ),
    CONSTRAINT [FK_ConfigurationItem_Configuration] FOREIGN KEY ([ConfigurationId]) REFERENCES [core].[Configuration] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_ConfigurationItem_UIResource] FOREIGN KEY([ItemTargetId]) REFERENCES [core].[UIResource] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

