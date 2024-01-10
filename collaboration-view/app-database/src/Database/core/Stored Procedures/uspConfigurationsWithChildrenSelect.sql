-- =============================================
-- Description: Returns configurations with children that match the search parameters ConfigType, SiteId, PatientId.
-- Parameters:
--   @Configurations - Table with filtered configurations.
--   @SiteId - Id of a specific site. Accepts nulls.
--   @PatientId - Id of a specific Patient. Accepts nulls.
--   @ConfigType - Type of the congiguration.
--   @ReturnCount - Returns the number of parent configurations found.
--   @JsonResult - Returns a JSON containing an array of the parent configurations found (including their items) 
--                 and their child configurations (including their items).
-- =============================================
CREATE PROCEDURE [core].[uspConfigurationsWithChildrenSelect]
	@Configurations ConfigurationTableType READONLY, 
	@SiteId NVARCHAR(50), 
	@PatientId NVARCHAR(50), 
    @ConfigType NVARCHAR(50), 
	@ReturnCount INT OUTPUT,						
	@JsonResult NVARCHAR(MAX) OUTPUT				
AS
BEGIN
	 SELECT @JsonResult = ISNULL(@JsonResult + ',','') + '{"configuration":'+ [core].fnGetConfigurationById(Id) + ',"children":' + [core].fnGetChildrenConfigurationsByParentId(Id)+'}' 
	 FROM @Configurations
	 WHERE ConfigType = @ConfigType  
		   AND CASE WHEN @SiteId IS NULL THEN 'a' ELSE SiteId END = CASE WHEN @SiteId IS NULL THEN 'a' ELSE @SiteId END
		   AND CASE WHEN @PatientId IS NULL THEN 'a' ELSE PatientId END = CASE WHEN @PatientId IS NULL THEN 'a' ELSE @PatientId END
	 SET @ReturnCount = @@ROWCOUNT 
END