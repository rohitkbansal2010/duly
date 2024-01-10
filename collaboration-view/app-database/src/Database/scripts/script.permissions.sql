-- <copyright file="permissions.sql" company="Duly Health and Care">
-- Copyright (c) Duly Health and Care. All rights reserved.
-- </copyright>

-- =============================================
-- Description: Grant permissions for the groups EPAM-DEV-GRP and COLLAB-VIEW-DEV-DB-OWNER.
-- =============================================

CREATE USER [EPAM-DEV-GRP]
	FROM EXTERNAL PROVIDER
	WITH DEFAULT_SCHEMA =  dbo
GO

ALTER ROLE db_datareader
ADD MEMBER [EPAM-DEV-GRP]
GO

ALTER ROLE db_datawriter
ADD MEMBER [EPAM-DEV-GRP]
GO

CREATE ROLE dbo_executor
GO
GRANT EXECUTE ON SCHEMA::[core] to core_executor
GO

ALTER ROLE core_executor
ADD MEMBER [EPAM-DEV-GRP]
GO



CREATE USER [COLLAB-VIEW-DEV-DB-OWNER]
	FROM EXTERNAL PROVIDER
	WITH DEFAULT_SCHEMA =  dbo
GO

ALTER ROLE db_owner
ADD MEMBER [COLLAB-VIEW-DEV-DB-OWNER]
GO