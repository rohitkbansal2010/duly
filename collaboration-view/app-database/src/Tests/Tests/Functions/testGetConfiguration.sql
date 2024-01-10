EXEC tSQLt.NewTestClass 'Tests';
GO

CREATE OR ALTER PROC Tests.testGetConfiguration
AS
BEGIN
	DECLARE @Id AS NVARCHAR(50) = '111'
	DECLARE	@ConfigurationExpected AS NVARCHAR(MAX) = '{"id":"111","parentConfigurationId":null,"parentTargetId":null,"siteId":null,"patientId":null,"applicationPart":null,"configType":null,"targetAreaType":null,"targetType":null,"tags":null,"details":null,"items":[{"itemTarget":{"id":"111-11","alias":"TestAlias1","resourceType":null,"details":null},"index":1,"itemTargetType":null,"details":null},{"itemTarget":{"id":"111-22","alias":"TestAlias2","resourceType":null,"details":null},"index":2,"itemTargetType":null,"details":null}]}'
	DECLARE	@ConfigurationActual AS NVARCHAR(MAX) 

	EXEC tSQLt.FakeTable @TableName='dbo.Configuration'
	INSERT INTO dbo.Configuration (Id, ParentConfigurationId) VALUES('111', NULL)
	INSERT INTO dbo.Configuration (Id, ParentConfigurationId) VALUES('222', '111')

	EXEC tSQLt.FakeTable @TableName='dbo.ConfigurationItem'
	INSERT INTO dbo.ConfigurationItem (ConfigurationId, ItemTargetId,  [Index]) VALUES('111', '111-11', 1)
	INSERT INTO dbo.ConfigurationItem (ConfigurationId, ItemTargetId,  [Index]) VALUES('111', '111-22', 2)

	EXEC tSQLt.FakeTable @TableName='dbo.UIResource'
	INSERT INTO dbo.UIResource (Id, Alias) VALUES('111-11', 'TestAlias1')
	INSERT INTO dbo.UIResource (Id, Alias) VALUES('111-22', 'TestAlias2')

	SET @ConfigurationActual = dbo.GetConfiguration(@Id)
	EXEC tSQLt.AssertEqualsString @ConfigurationExpected, @ConfigurationActual
 
END
GO

EXEC tSQLt.Run 'Tests.testGetConfiguration'