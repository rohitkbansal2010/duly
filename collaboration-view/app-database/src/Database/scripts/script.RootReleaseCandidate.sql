if ('$(EnvironmentType)' = 'SANDBOX' or '$(EnvironmentType)' = 'DEV')
begin    

    print 'script.PopulateConfigurations.sql'
    :r ".\script.PopulateConfigurations.sql"

    print 'script.PopulateUIResources.sql'
    :r ".\script.PopulateUIResources.sql"

    print 'script.PopulateConfigurationItems.sql'
    :r ".\script.PopulateConfigurationItems.sql"

end;
