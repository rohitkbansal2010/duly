-- =============================================
-- Description: Insert the initial data in the table core.UIResource
-- =============================================

merge into [core].[UIResource] as Target
using (VALUES
(N'28a11797-05d5-44ee-9b8c-04b788452939', N'ModuleResults', N'Module', NULL),
(N'2f319c4c-238e-448c-8441-4cf08ca8edb1', N'WidgetAppointments', N'Widget', NULL),
(N'45ef1146-13c0-4fd4-aa56-af7fba75e531', N'WidgetVitals', N'Widget', NULL),
(N'4895ca3a-f778-44fb-ae66-ba0f2af41cfe', N'ModuleCarePlan', N'Module', NULL),
(N'6278d7ef-1631-4fb0-9f30-ddf64fe4ad45', N'ModuleCalendar', N'Module', NULL),
(N'71c31320-1755-481a-9b0f-e24894e86187', N'ModuleEducation', N'Module', NULL),
(N'8369136a-8693-46e4-a638-e27a97c571e5', N'WidgetGoals', N'Widget', NULL),
(N'84c98f0a-cba4-4947-a109-cbbe9436d36e', N'WidgetAllergies', N'Widget', NULL),
(N'851f0793-c3cd-4b8d-8083-f1b13f3c6711', N'WidgetImmunizations', N'Widget', NULL),
(N'b344c08d-eb5d-407b-9194-77ef802784f1', N'WidgetTodayAppointments', N'Widget', NULL),
(N'b459226f-834b-4f25-8762-04542abc627b', N'ModuleOverview', N'Module', NULL),
(N'bc325a84-4060-4124-ac00-b3bd588ac0a4', N'WidgetConditions', N'Widget', NULL),
(N'be5c35c9-07b5-4ed6-b2d7-7c90ce3ff799', N'WidgetMedications', N'Widget', NULL),
(N'd531af75-c6ec-4aaf-9849-318231d0b633', N'WidgetQuestions', N'Widget', NULL),
(N'fb683963-b80d-4261-a718-261c468b1a56', N'ModuleTelehealth', N'Module', NULL)
) as Source ([Id], [Alias], [ResourceType], [Details])
on (Target.Id = Source.Id)
when matched then
update set
        Target.[Alias] = Source.[Alias], 
        Target.[ResourceType] = Source.[ResourceType], 
        Target.[Details] = Source.[Details]
when not matched by Target then
    insert ([Id], [Alias], [ResourceType], [Details]) 
    values (Source.[Id], Source.[Alias], Source.[ResourceType], Source.[Details])
when not matched by Source then
delete;