raiserror(N'PostDeployment script processing', 0, 1) with nowait;
go


-- ============================================
-- =====    go to release candidate       =====
    :r ".\script.RootReleaseCandidate.sql"

-- ============================================
go

raiserror(N'==> End of post-deploy script', 0, 1) with nowait;
go
