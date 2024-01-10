-- <copyright file="JsonParse.sql" company="Duly Health and Care">
-- Copyright (c) Duly Health and Care. All rights reserved.
-- </copyright>

-- =============================================
-- Description: Inserts the data from files in JSON format in the tables core.Configuration, core.ConfiguratonItem
-- Filename(s) should be in the variable @Filenm
-- The table core.UIResource must be populated before by the script ConfigurationsData.sql
-- =============================================

DECLARE @i INT = 1;
DECLARE @Filenm NVARCHAR(100)
DECLARE @Command NVARCHAR(500)
DECLARE @JSON NVARCHAR(MAX)
delete from core.ConfigurationItem
delete from core.configuration
WHILE @i < 19
BEGIN

SET @Filenm='C:\Dpg\'+ CONVERT(NVARCHAR, @i)+'.json'
SET @Command =
N'SELECT @JSON = BulkColumn
FROM OPENROWSET 
(BULK '''+ @Filenm +''', SINGLE_CLOB)
AS j'
EXEC sp_executesql @Command , N'@JSON NVARCHAR(MAX) OUTPUT', @JSON = @JSON OUTPUT 

INSERT INTO core.Configuration(Id, ParentConfigurationId, ParentTargetId, SiteId, PatientId, ApplicationPart, ConfigType, TargetAreaType, TargetType, Tags, Details)
SELECT ConfigurationTable.*
FROM (
SELECT   Config.[Id]
       , Config.[ParentConfigurationId]
       , Config.[ParentTargetId]
       , Config.[SiteId]
       , Config.[PatientId]
       , Config.[ApplicationPart]
       , Config.[ConfigType]
	   , Config.[TargetAreaType]
       , Config.[TargetType]
       , Config.[Tags]
       , Config.[Details]
FROM OPENJSON(@JSON)
  WITH (
    [Id] NVARCHAR(50) '$.configuration.id',
    [ParentConfigurationId] NVARCHAR(50) '$.configuration.parentConfigurationId',
    [ParentTargetId] NVARCHAR(50) '$.configuration.parentTargetId',
    [SiteId] NVARCHAR(50) '$.configuration.siteId',
    [PatientId] NVARCHAR(50) '$.configuration.patientId',
    [ApplicationPart] NVARCHAR(50) '$.configuration.applicationPart',
    [ConfigType] NVARCHAR(50) '$.configuration.configType',
    [TargetAreaType] NVARCHAR(50) '$.configuration.targetAreaType',
    [TargetType] NVARCHAR(50) '$.configuration.targetType',
    [Tags] NVARCHAR(250) '$.configuration.tags',
    [Details] NVARCHAR(MAX) '$.configuration.Details'
  ) As Config
  ) As ConfigurationTable;

INSERT INTO core.ConfigurationItem(ConfigurationId, ItemTargetId, ItemTargetType, [Index], Details)
SELECT ConfigurationItemTable.*
FROM (
SELECT   Config.[Id]
       , ConfigItem.[ItemTargetId]
       , ConfigItem.[ItemTargetType]
       , ConfigItem.[Index]
       , ConfigItem.[ConfigItemDetails]
FROM OPENJSON(@JSON)
  WITH (
    [Id] NVARCHAR(50) '$.configuration.id',
	[Items] NVARCHAR(MAX) '$.configuration.items' as JSON
  ) As Config
  CROSS APPLY OPENJSON(Config.Items)
  WITH (
    [ItemTargetId] NVARCHAR(50) '$.itemTarget.id',
    [ItemTargetType] NVARCHAR(50) '$.itemTargetType',
    [Index] INT '$.index',
    [ConfigItemDetails] NVARCHAR(MAX) '$.details'
  ) As ConfigItem
) As ConfigurationItemTable;

INSERT INTO core.Configuration(Id, ParentConfigurationId, ParentTargetId, SiteId, PatientId, ApplicationPart, ConfigType, TargetAreaType, TargetType, Tags, Details)
SELECT ChildrenConfigurationTable.*
FROM (
SELECT   Config.[ChildConfigurationId]
       , Config.[ParentConfigurationId]
       , Config.[ParentTargetId]
       , Config.[SiteId]
       , Config.[PatientId]
       , Config.[ApplicationPart]
       , Config.[ConfigType]
	   , Config.[TargetAreaType]
       , Config.[TargetType]
       , Config.[Tags]
       , Config.[Details]
FROM OPENJSON(@JSON)
  WITH (
	[Children] NVARCHAR(MAX) '$.children' as JSON
  ) As ChildrenConfigs
  CROSS APPLY OPENJSON(ChildrenConfigs.Children)
  WITH (
    [ChildConfigurationId] NVARCHAR(50) '$.id',
    [ParentConfigurationId] NVARCHAR(50) '$.parentConfigurationId',
    [ParentTargetId] NVARCHAR(50) '$.parentTargetId',
    [SiteId] NVARCHAR(50) '$.siteId',
    [PatientId] NVARCHAR(50) '$.patientId',
    [ApplicationPart] NVARCHAR(50) '$.applicationPart',
    [ConfigType] NVARCHAR(50) '$.configType',
    [TargetAreaType] NVARCHAR(50) '$.targetAreaType',
    [TargetType] NVARCHAR(50) '$.targetType',
    [Tags] NVARCHAR(250) '$.tags',
    [Details] NVARCHAR(MAX) '$.Details',
	[Items] NVARCHAR(MAX) '$.items' as JSON
  ) As Config
) As ChildrenConfigurationTable;

INSERT INTO core.ConfigurationItem(ConfigurationId, ItemTargetId, ItemTargetType, [Index], Details)
SELECT ChildrenItemTable.*
FROM (
SELECT   Config.[ChildConfigurationId]
       , ConfigItem.[ItemTargetId]
       , ConfigItem.[ItemTargetType]
       , ConfigItem.[Index]
       , ConfigItem.[ConfigItemDetails]
FROM OPENJSON(@JSON)
  WITH (
	[Children] NVARCHAR(MAX) '$.children' as JSON
  ) As ChildrenConfigs
  CROSS APPLY OPENJSON(ChildrenConfigs.Children)
  WITH (
    [ChildConfigurationId] NVARCHAR(50) '$.id',
	[Items] NVARCHAR(MAX) '$.items' as JSON
  ) As Config
  CROSS APPLY OPENJSON(Config.Items)
  WITH (
    [ItemTargetId] NVARCHAR(50) '$.itemTarget.id',
    [ItemTargetType] NVARCHAR(50) '$.itemTargetType',
    [Index] INT '$.index',
    [ConfigItemDetails] NVARCHAR(MAX) '$.details'
  ) As ConfigItem
) As ChildrenItemTable;

    SET @i = @i + 1
	
END