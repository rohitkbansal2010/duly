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

CREATE OR ALTER PROC Tests.testGetChildrenConfigurations
AS
BEGIN
	DECLARE @ParentConfigId AS NVARCHAR(50) = '111'
	DECLARE	@ChildrenExpected AS NVARCHAR(MAX) = '[Test Configuration,Test Configuration]'
	DECLARE	@ChildrenActual AS NVARCHAR(MAX) 

	EXEC tSQLt.FakeTable @TableName='dbo.Configuration'
	INSERT INTO dbo.Configuration (Id, ParentConfigurationId,  ApplicationPart) VALUES('111', NULL, 'Calendar')
	INSERT INTO dbo.Configuration (Id, ParentConfigurationId,  ApplicationPart) VALUES('222', '111', 'Calendar')
	INSERT INTO dbo.Configuration (Id, ParentConfigurationId,  ApplicationPart) VALUES('333', '111', 'Calendar')

	EXEC tSQLt.FakeFunction 'dbo.GetConfiguration' ,'Tests.GetConfigurationFake'

	SET @ChildrenActual = dbo.GetChildrenConfigurations(@ParentConfigId)
	EXEC tSQLt.AssertEqualsString @ChildrenExpected, @ChildrenActual

END
GO

EXEC tSQLt.Run 'Tests.testGetChildrenConfigurations'