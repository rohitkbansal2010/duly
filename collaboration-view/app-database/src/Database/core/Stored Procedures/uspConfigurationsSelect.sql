-- =============================================
-- Description: Finds all parent configurations and their child configurations that match the search parameters.
-- Parameters:
--   @AppPart - An application part.
--   @SiteId - Id of a specific site. Accepts nulls.
--   @PatientId - Id of a specific Patient. Accepts nulls.
--   @ReturnCount - Returns the number of parent configurations found.
--   @JsonResult - Returns a JSON containing an array of the parent configurations found (including their items) 
--                 and their child configurations (including their items).
-- =============================================

CREATE PROCEDURE [core].[uspConfigurationsSelect]
	@AppPart NVARCHAR(50),				
	@SiteId NVARCHAR(50), 
	@PatientId NVARCHAR(50), 
    @TargetAreaType NVARCHAR(50), 
	@ReturnCount INT OUTPUT,			
	@JsonResult NVARCHAR(MAX) OUTPUT	
AS
BEGIN
	DECLARE @FilteredConfigurations AS ConfigurationTableType
	DECLARE	@Count INT
	DECLARE @Json NVARCHAR(MAX)

	INSERT INTO @FilteredConfigurations
	SELECT    Id   
			, ParentConfigurationId
			, ParentTargetId       
			, SiteId               
			, PatientId            
			, ApplicationPart   
			, ConfigType    
			, TargetAreaType  
			, TargetType  
			, Tags 
			, Details  
	FROM [core].[Configuration]
	WHERE ParentConfigurationId IS NULL AND ApplicationPart = @AppPart 
		  AND CASE WHEN @TargetAreaType IS NULL THEN 'a' ELSE TargetAreaType END = CASE WHEN @TargetAreaType IS NULL THEN 'a' ELSE @TargetAreaType END
	
	SET @ReturnCount = 0
	IF @TargetAreaType = 'Navigation' OR @TargetAreaType IS NULL
	BEGIN
		EXEC [core].[uspNavigationConfigurationsSelect] @FilteredConfigurations, @SiteId, @PatientId, @Count OUTPUT, @Json OUTPUT
		SET @JsonResult = ISNULL(@JsonResult + ',','') + @Json
		SET @ReturnCount = @ReturnCount + @Count
	END
	
	IF @TargetAreaType = 'Layout' OR @TargetAreaType IS NULL
	BEGIN

		------- To be done 

		SET @JsonResult = @JsonResult + '' 
		SET @ReturnCount = @ReturnCount + 0
	END

	SET @JsonResult = '[' + ISNULL(@JsonResult, '') + ']' 
END