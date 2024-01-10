EXEC tSQLt.NewTestClass 'Tests';
GO

CREATE OR ALTER FUNCTION Tests.GetConfigurationFake 
(
	@ConfigurationId NVARCHAR(50)
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	RETURN 'Test Configuration'
END
GO

CREATE OR ALTER FUNCTION Tests.GetChildrenConfigurationsFake 
(
	@ParentConfigId NVARCHAR(50)
)
RETURNS NVARCHAR(MAX)
AS
BEGIN
	RETURN 'Test Children'
END
GO

CREATE OR ALTER PROC Tests.testFindConfigurations
AS
BEGIN
	DECLARE @ApPart AS NVARCHAR(50) = 'Calendar'
	DECLARE	@ConfigurationsExpected AS NVARCHAR(MAX) = '[{"configuration":Test Configuration,"children":Test Children}]'
	DECLARE	@ConfigurationsActual AS NVARCHAR(MAX) 
	DECLARE @CountExpected AS INT = 1
	DECLARE @CountActual AS INT

	EXEC tSQLt.FakeTable @TableName='dbo.Configuration'
	INSERT INTO dbo.Configuration (Id, ParentConfigurationId,  ApplicationPart) VALUES('111', NULL, 'Calendar')
	INSERT INTO dbo.Configuration (Id, ParentConfigurationId,  ApplicationPart) VALUES('222', '111', 'Calendar')

	EXEC tSQLt.FakeFunction 'dbo.GetConfiguration' ,'Tests.GetConfigurationFake'
	EXEC tSQLt.FakeFunction 'dbo.GetChildrenConfigurations' ,'Tests.GetChildrenConfigurationsFake'

	EXEC dbo.FindConfigurations @ApPart, NULL, NULL, NULL, @ReturnCount = @CountActual OUTPUT, @JsonResult = @ConfigurationsActual OUTPUT
	EXEC tSQLt.AssertEqualsString @ConfigurationsExpected, @ConfigurationsActual
	EXEC tSQLt.AssertEquals @CountExpected, @CountActual
 
END

GO
EXEC tSQLt.Run 'Tests.testFindConfigurations'