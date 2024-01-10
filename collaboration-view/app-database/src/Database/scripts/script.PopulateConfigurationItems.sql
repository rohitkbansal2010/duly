-- =============================================
-- Description: Insert the initial data in the table core.ConfigurationItem
-- =============================================

merge into [core].[ConfigurationItem] as Target
using (VALUES
(N'a569bc60-d72c-45bf-a2fb-ba2501336b6d', N'28a11797-05d5-44ee-9b8c-04b788452939', N'Module', 4, NULL),
(N'a22547f9-4f4a-4dd0-9309-5ddac9d0b798', N'2f319c4c-238e-448c-8441-4cf08ca8edb1', N'Widget', 5, NULL),
(N'a22547f9-4f4a-4dd0-9309-5ddac9d0b798', N'45ef1146-13c0-4fd4-aa56-af7fba75e531', N'Widget', 2, NULL),
(N'a569bc60-d72c-45bf-a2fb-ba2501336b6d', N'4895ca3a-f778-44fb-ae66-ba0f2af41cfe', N'Module', 2, NULL),
(N'43532904-9dd7-4926-a3b6-7ce50ff5bc71', N'6278d7ef-1631-4fb0-9f30-ddf64fe4ad45', N'Module', 1, NULL),
(N'a569bc60-d72c-45bf-a2fb-ba2501336b6d', N'71c31320-1755-481a-9b0f-e24894e86187', N'Module', 3, NULL),
(N'a22547f9-4f4a-4dd0-9309-5ddac9d0b798', N'8369136a-8693-46e4-a638-e27a97c571e5', N'Widget', 3, NULL),
(N'a22547f9-4f4a-4dd0-9309-5ddac9d0b798', N'84c98f0a-cba4-4947-a109-cbbe9436d36e', N'Widget', 7, NULL),
(N'a22547f9-4f4a-4dd0-9309-5ddac9d0b798', N'851f0793-c3cd-4b8d-8083-f1b13f3c6711', N'Widget', 8, NULL),
(N'cd87c96e-6251-489e-be85-61dc30bd61a2', N'b344c08d-eb5d-407b-9194-77ef802784f1', N'Widget', 1, NULL),
(N'a569bc60-d72c-45bf-a2fb-ba2501336b6d', N'b459226f-834b-4f25-8762-04542abc627b', N'Module', 1, NULL),
(N'a22547f9-4f4a-4dd0-9309-5ddac9d0b798', N'bc325a84-4060-4124-ac00-b3bd588ac0a4', N'Widget', 4, NULL),
(N'a22547f9-4f4a-4dd0-9309-5ddac9d0b798', N'be5c35c9-07b5-4ed6-b2d7-7c90ce3ff799', N'Widget', 6, NULL),
(N'a22547f9-4f4a-4dd0-9309-5ddac9d0b798', N'd531af75-c6ec-4aaf-9849-318231d0b633', N'Widget', 1, NULL),
(N'a569bc60-d72c-45bf-a2fb-ba2501336b6d', N'fb683963-b80d-4261-a718-261c468b1a56', N'Module', 5, NULL)
) as Source ([ConfigurationId], [ItemTargetId], [ItemTargetType], [Index], [Details])
on (Target.[ConfigurationId] = Source.[ConfigurationId] and Target.[ItemTargetId] = Source.[ItemTargetId])
when matched then
update set
        Target.[ItemTargetType] = Source.[ItemTargetType],
        Target.[Index] = Source.[Index],
        Target.[Details] = Source.[Details]

when not matched by Target then
    insert ([ConfigurationId], [ItemTargetId], [ItemTargetType], [Index], [Details]) 
    values (Source.[ConfigurationId], Source.[ItemTargetId], Source.[ItemTargetType], Source.[Index], Source.[Details])
when not matched by Source then
delete;