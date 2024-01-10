-- <copyright file="GetConfiguration.sql" company="Duly Health and Care">
-- Copyright (c) Duly Health and Care. All rights reserved.
-- </copyright>

-- =============================================
-- Description: Returns one configuration by Id in JSON format.
-- Parameters:
--   @ConfigurationId - Id of a specific configuration.
-- =============================================

CREATE FUNCTION [core].[fnGetConfigurationById] 
(
	@ConfigurationId NVARCHAR(50)	
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	RETURN 
	( SELECT 
		  Id AS id
		, ParentConfigurationId AS parentConfigurationId
		, ParentTargetId AS parentTargetId
		, SiteId AS siteId
		, PatientId AS patientId
		, ApplicationPart AS applicationPart
		, ConfigType AS configType
		, TargetAreaType AS targetAreaType
		, TargetType AS targetType
		, Tags AS tags
		, Details AS details
		, (SELECT
			  UR.Id AS [itemTarget.id]
			, UR.Alias AS [itemTarget.alias]
			, UR.ResourceType AS [itemTarget.resourceType]
			, UR.Details AS [itemTarget.details]
			, CI.[Index] AS [index]
			, CI.ItemTargetType AS itemTargetType
			, CI.Details AS details
			FROM [core].[ConfigurationItem] CI JOIN [core].[UIResource] UR
				 ON CI.ItemTargetId = UR.Id
		    WHERE CI.ConfigurationId = @ConfigurationId
			ORDER BY CI.[Index]
			FOR JSON PATH, INCLUDE_NULL_VALUES
		) AS items

		FROM [core].[Configuration]
		WHERE Id = @ConfigurationId
		FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER )
		
	END	