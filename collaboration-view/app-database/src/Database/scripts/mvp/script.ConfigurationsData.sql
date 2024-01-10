-- =============================================
-- Description: Deletes all rows from the tables core.Configuration, core.UIResource, core.ConfigurationItem and Inserts initial data. 
-- =============================================

IF OBJECT_ID(N'[core].UIResource', N'U') IS NOT NULL   
	AND OBJECT_ID(N'[core].Configuration', N'U') IS NOT NULL
	AND OBJECT_ID(N'[core].ConfigurationItem', N'U') IS NOT NULL
	BEGIN
		DELETE FROM [core].[ConfigurationItem]
	
		DELETE FROM [core].[Configuration]
	
		DELETE FROM [core].[UIResource]
	
		INSERT [core].[UIResource] ([Id], [Alias], [ResourceType], [Details]) VALUES (N'28a11797-05d5-44ee-9b8c-04b788452939', N'ModuleResults', N'Module', NULL)
		INSERT [core].[UIResource] ([Id], [Alias], [ResourceType], [Details]) VALUES (N'2f319c4c-238e-448c-8441-4cf08ca8edb1', N'WidgetAppointments', N'Widget', NULL)
		INSERT [core].[UIResource] ([Id], [Alias], [ResourceType], [Details]) VALUES (N'45ef1146-13c0-4fd4-aa56-af7fba75e531', N'WidgetVitals', N'Widget', NULL)
		INSERT [core].[UIResource] ([Id], [Alias], [ResourceType], [Details]) VALUES (N'4895ca3a-f778-44fb-ae66-ba0f2af41cfe', N'ModuleCarePlan', N'Module', NULL)
		INSERT [core].[UIResource] ([Id], [Alias], [ResourceType], [Details]) VALUES (N'6278d7ef-1631-4fb0-9f30-ddf64fe4ad45', N'ModuleCalendar', N'Module', NULL)
		INSERT [core].[UIResource] ([Id], [Alias], [ResourceType], [Details]) VALUES (N'71c31320-1755-481a-9b0f-e24894e86187', N'ModuleEducation', N'Module', NULL)
		INSERT [core].[UIResource] ([Id], [Alias], [ResourceType], [Details]) VALUES (N'8369136a-8693-46e4-a638-e27a97c571e5', N'WidgetGoals', N'Widget', NULL)
		INSERT [core].[UIResource] ([Id], [Alias], [ResourceType], [Details]) VALUES (N'84c98f0a-cba4-4947-a109-cbbe9436d36e', N'WidgetAllergies', N'Widget', NULL)
		INSERT [core].[UIResource] ([Id], [Alias], [ResourceType], [Details]) VALUES (N'851f0793-c3cd-4b8d-8083-f1b13f3c6711', N'WidgetImmunizations', N'Widget', NULL)
		INSERT [core].[UIResource] ([Id], [Alias], [ResourceType], [Details]) VALUES (N'b344c08d-eb5d-407b-9194-77ef802784f1', N'WidgetTodayAppointments', N'Widget', NULL)
		INSERT [core].[UIResource] ([Id], [Alias], [ResourceType], [Details]) VALUES (N'b459226f-834b-4f25-8762-04542abc627b', N'ModuleOverview', N'Module', NULL)
		INSERT [core].[UIResource] ([Id], [Alias], [ResourceType], [Details]) VALUES (N'bc325a84-4060-4124-ac00-b3bd588ac0a4', N'WidgetConditions', N'Widget', NULL)
		INSERT [core].[UIResource] ([Id], [Alias], [ResourceType], [Details]) VALUES (N'be5c35c9-07b5-4ed6-b2d7-7c90ce3ff799', N'WidgetMedications', N'Widget', NULL)
		INSERT [core].[UIResource] ([Id], [Alias], [ResourceType], [Details]) VALUES (N'd531af75-c6ec-4aaf-9849-318231d0b633', N'WidgetQuestions', N'Widget', NULL)
		INSERT [core].[UIResource] ([Id], [Alias], [ResourceType], [Details]) VALUES (N'fb683963-b80d-4261-a718-261c468b1a56', N'ModuleTelehealth', N'Module', NULL)
	
		INSERT [core].[Configuration] ([Id], [ParentConfigurationId], [ParentTargetId], [SiteId], [PatientId], [ApplicationPart], [ConfigType], [TargetAreaType], [TargetType], [Tags], [Details]) VALUES (N'43532904-9dd7-4926-a3b6-7ce50ff5bc71', NULL, NULL, NULL, NULL, N'Calendar', N'GlobalConfiguration', N'Navigation', N'Modules', NULL, NULL)
		INSERT [core].[Configuration] ([Id], [ParentConfigurationId], [ParentTargetId], [SiteId], [PatientId], [ApplicationPart], [ConfigType], [TargetAreaType], [TargetType], [Tags], [Details]) VALUES (N'a22547f9-4f4a-4dd0-9309-5ddac9d0b798', N'a569bc60-d72c-45bf-a2fb-ba2501336b6d', N'b459226f-834b-4f25-8762-04542abc627b', NULL, NULL, N'CurrentAppointment', N'GlobalConfiguration', N'Navigation', N'Widgets', NULL, NULL)
		INSERT [core].[Configuration] ([Id], [ParentConfigurationId], [ParentTargetId], [SiteId], [PatientId], [ApplicationPart], [ConfigType], [TargetAreaType], [TargetType], [Tags], [Details]) VALUES (N'a569bc60-d72c-45bf-a2fb-ba2501336b6d', NULL, NULL, NULL, NULL, N'CurrentAppointment', N'GlobalConfiguration', N'Navigation', N'Modules', NULL, NULL)
		INSERT [core].[Configuration] ([Id], [ParentConfigurationId], [ParentTargetId], [SiteId], [PatientId], [ApplicationPart], [ConfigType], [TargetAreaType], [TargetType], [Tags], [Details]) VALUES (N'cd87c96e-6251-489e-be85-61dc30bd61a2', N'43532904-9dd7-4926-a3b6-7ce50ff5bc71', N'6278d7ef-1631-4fb0-9f30-ddf64fe4ad45', NULL, NULL, N'Calendar', N'GlobalConfiguration', N'Navigation', N'Widgets', NULL, NULL)
	
		INSERT [core].[ConfigurationItem] ([ConfigurationId], [ItemTargetId], [ItemTargetType], [Index], [Details]) VALUES (N'a569bc60-d72c-45bf-a2fb-ba2501336b6d', N'28a11797-05d5-44ee-9b8c-04b788452939', N'Module', 4, NULL)
		INSERT [core].[ConfigurationItem] ([ConfigurationId], [ItemTargetId], [ItemTargetType], [Index], [Details]) VALUES (N'a22547f9-4f4a-4dd0-9309-5ddac9d0b798', N'2f319c4c-238e-448c-8441-4cf08ca8edb1', N'Widget', 5, NULL)
		INSERT [core].[ConfigurationItem] ([ConfigurationId], [ItemTargetId], [ItemTargetType], [Index], [Details]) VALUES (N'a22547f9-4f4a-4dd0-9309-5ddac9d0b798', N'45ef1146-13c0-4fd4-aa56-af7fba75e531', N'Widget', 2, NULL)
		INSERT [core].[ConfigurationItem] ([ConfigurationId], [ItemTargetId], [ItemTargetType], [Index], [Details]) VALUES (N'43532904-9dd7-4926-a3b6-7ce50ff5bc71', N'6278d7ef-1631-4fb0-9f30-ddf64fe4ad45', N'Module', 1, NULL)
		INSERT [core].[ConfigurationItem] ([ConfigurationId], [ItemTargetId], [ItemTargetType], [Index], [Details]) VALUES (N'a22547f9-4f4a-4dd0-9309-5ddac9d0b798', N'84c98f0a-cba4-4947-a109-cbbe9436d36e', N'Widget', 7, NULL)
		INSERT [core].[ConfigurationItem] ([ConfigurationId], [ItemTargetId], [ItemTargetType], [Index], [Details]) VALUES (N'a22547f9-4f4a-4dd0-9309-5ddac9d0b798', N'851f0793-c3cd-4b8d-8083-f1b13f3c6711', N'Widget', 8, NULL)
		INSERT [core].[ConfigurationItem] ([ConfigurationId], [ItemTargetId], [ItemTargetType], [Index], [Details]) VALUES (N'cd87c96e-6251-489e-be85-61dc30bd61a2', N'b344c08d-eb5d-407b-9194-77ef802784f1', N'Widget', 1, NULL)
		INSERT [core].[ConfigurationItem] ([ConfigurationId], [ItemTargetId], [ItemTargetType], [Index], [Details]) VALUES (N'a569bc60-d72c-45bf-a2fb-ba2501336b6d', N'b459226f-834b-4f25-8762-04542abc627b', N'Module', 1, NULL)
		INSERT [core].[ConfigurationItem] ([ConfigurationId], [ItemTargetId], [ItemTargetType], [Index], [Details]) VALUES (N'a22547f9-4f4a-4dd0-9309-5ddac9d0b798', N'bc325a84-4060-4124-ac00-b3bd588ac0a4', N'Widget', 4, NULL)
		INSERT [core].[ConfigurationItem] ([ConfigurationId], [ItemTargetId], [ItemTargetType], [Index], [Details]) VALUES (N'a22547f9-4f4a-4dd0-9309-5ddac9d0b798', N'be5c35c9-07b5-4ed6-b2d7-7c90ce3ff799', N'Widget', 6, NULL)
		INSERT [core].[ConfigurationItem] ([ConfigurationId], [ItemTargetId], [ItemTargetType], [Index], [Details]) VALUES (N'a569bc60-d72c-45bf-a2fb-ba2501336b6d', N'fb683963-b80d-4261-a718-261c468b1a56', N'Module', 5, NULL)
	END
ELSE
	PRINT N'Warning! No data was touched. Some tables are missing.'
GO