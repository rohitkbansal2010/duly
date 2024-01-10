-- =============================================
-- Description: Returns an array of the child configurations for the specific parent configuration
-- Parameters:
--   @ConfigurationId - Id of the parent configuration.
-- =============================================

CREATE FUNCTION [core].[fnGetChildrenConfigurationsByParentId] 
(
	@ParentConfigId NVARCHAR(50)			
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	DECLARE @JsonChildren NVARCHAR(MAX)
    SELECT @JsonChildren = ISNULL(@JsonChildren + ',','') + [core].fnGetConfigurationById(Id)
	FROM [core].Configuration
	WHERE ParentConfigurationId = @ParentConfigId

	RETURN (SELECT '[' + @JsonChildren +']')
END	