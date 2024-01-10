-- =============================================
-- Description: Returns configurations with children for the TargetAreaType = 'Navigation'
-- Parameters:
--   @Configurations - Table with filtered configurations.
--   @SiteId - Id of a specific site. Accepts nulls.
--   @PatientId - Id of a specific Patient. Accepts nulls.
--   @ReturnCount - Returns the number of parent configurations found.
--   @JsonResult - Returns a JSON containing an array of the parent configurations found (including their items) 
--                 and their child configurations (including their items).
-- =============================================

CREATE PROCEDURE [core].[uspNavigationConfigurationsSelect]
	@Configurations ConfigurationTableType READONLY,
	@SiteId NVARCHAR(50), 
	@PatientId NVARCHAR(50),  
	@ReturnCount INT OUTPUT,							
	@JsonResult NVARCHAR(MAX) OUTPUT					
AS
BEGIN
	SET @ReturnCount = 0
	IF @PatientId IS NOT NULL AND @SiteId IS NOT NULL
		EXEC [core].[uspConfigurationsWithChildrenSelect] @Configurations, @SiteId, @PatientId, 'PatientConfiguration', @ReturnCount OUTPUT, @JsonResult OUTPUT

	IF (@PatientId IS NULL AND @SiteId IS NOT NULL) OR (@ReturnCount = 0 AND @SiteId IS NOT NULL) 
		EXEC [core].[uspConfigurationsWithChildrenSelect] @Configurations, @SiteId, NULL, 'ClinicConfiguration', @ReturnCount OUTPUT, @JsonResult OUTPUT
		
	IF @SiteId IS NULL OR @ReturnCount = 0 
		EXEC [core].[uspConfigurationsWithChildrenSelect] @Configurations, NULL, NULL, 'GlobalConfiguration', @ReturnCount OUTPUT, @JsonResult OUTPUT
END