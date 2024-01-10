-- =============================================
-- Description: Insert the initial data in the table core.Configuration
-- =============================================

merge into [core].[Configuration] as Target
using (VALUES 
(N'43532904-9dd7-4926-a3b6-7ce50ff5bc71', NULL, NULL, NULL, NULL, N'Calendar', N'GlobalConfiguration', N'Navigation', N'Modules', NULL, NULL),
(N'a22547f9-4f4a-4dd0-9309-5ddac9d0b798', N'a569bc60-d72c-45bf-a2fb-ba2501336b6d', N'b459226f-834b-4f25-8762-04542abc627b', NULL, NULL, N'CurrentAppointment', N'GlobalConfiguration', N'Navigation', N'Widgets', NULL, NULL),
(N'a569bc60-d72c-45bf-a2fb-ba2501336b6d', NULL, NULL, NULL, NULL, N'CurrentAppointment', N'GlobalConfiguration', N'Navigation', N'Modules', NULL, NULL),
(N'cd87c96e-6251-489e-be85-61dc30bd61a2', N'43532904-9dd7-4926-a3b6-7ce50ff5bc71', N'6278d7ef-1631-4fb0-9f30-ddf64fe4ad45', NULL, NULL, N'Calendar', N'GlobalConfiguration', N'Navigation', N'Widgets', NULL, NULL)
) as Source ([Id], [ParentConfigurationId], [ParentTargetId], [SiteId], [PatientId], [ApplicationPart], [ConfigType], [TargetAreaType], [TargetType], [Tags], [Details])
on (Target.Id = Source.Id)
when matched then
    update set 
        Target.[ParentConfigurationId] = Source.[ParentConfigurationId], 
        Target.[ParentTargetId] = Source.[ParentTargetId],
        Target.[SiteId] = Source.[SiteId], 
        Target.[PatientId] = Source.[PatientId],
        Target.[ApplicationPart] = Source.[ApplicationPart], 
        Target.[ConfigType] = Source.[ConfigType],
        Target.[TargetAreaType] = Source.[TargetAreaType], 
        Target.[TargetType] = Source.[TargetType], 
        Target.[Tags] = Source.[Tags], 
        Target.[Details] = Source.[Details] 
when not matched by Target then
    insert ([Id], [ParentConfigurationId], [ParentTargetId], [SiteId], [PatientId], [ApplicationPart], [ConfigType], [TargetAreaType], [TargetType], [Tags], [Details]) 
    values (Source.[Id], Source.[ParentConfigurationId], Source.[ParentTargetId], Source.[SiteId], Source.[PatientId], Source.[ApplicationPart], Source.[ConfigType], Source.[TargetAreaType], Source.[TargetType], Source.[Tags], Source.[Details])
when not matched by Source then
delete;
