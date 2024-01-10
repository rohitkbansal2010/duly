CREATE ROLE [core_executor]
    AUTHORIZATION [dbo];


GO
ALTER ROLE [core_executor] ADD MEMBER [EPAM-DEV-GRP];

GO
GRANT EXECUTE
    ON SCHEMA::[core] TO [core_executor];
