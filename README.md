Code Churn Loader
=================

This is a simple utitlity to pull down number of lines added, changed, deleted (aka code churn) from public or private repos
hosted on Github or Bitbucket.

Code churn is stored in database where it can be reported.

For example,

![Weekly churn](/../screenshots/ExcelReport.png?raw=true "Weekly churn")

or 

```
-- Files with most changes
SELECT TOP 5
f.FileName, TotalChurn = SUM(TotalChurn)
FROM DimCommit c 
JOIN DimCommitFile dcf ON dcf.CommitId = c.CommitId
JOIN DimFile f ON f.FileId = dcf.FileId
JOIN FactCodeChurn fc ON fc.FileId = f.FileId
JOIN DimDate d ON d.DateId = fc.DateId
WHERE FileExtension = '.cs'
--AND d.DayOfMonth BETWEEN 20 AND 24
GROUP BY f.FileName
ORDER BY TotalChurn desc
```

Here's a fragment of PowerShell that can be used to automate running the utility

![Powershell](/../screenshots/PowershellCodeChurn.png?raw=true "Powershell")

The database is fairly straightforward

![Database](/../screenshots/CodeChurnDB.png?raw=true "Database")

#Sample Config file

```
<configuration>
  <configSections>        
    <section name="RepoCredentials" type="CodeChurnLoader.RepoCredentials, CodeChurnLoader" />
  </configSections>  
  <LoaderConfiguration>
        <Providers>
            <add Type="Github" Owner="stanbpublic" UserName="secret" Password="secret"></add>
            <add Type="Bitbucket" Owner="stanbpublic" UserName="" Password=""></add>
        </Providers>
  </LoaderConfiguration>
  <connectionStrings>
    <add name="CodeChurnLoaderWarehouse" 
        providerName="System.Data.SqlClient"
	connectionString="Data Source=sql01;Initial Catalog=IntegrationsCodeStats;Integrated Security=yes;" />
  </connectionStrings>
</configuration>
```

User name and password are only required for private repos.

#Command line parameters
--p Provider (Github | Bitbucket)
--r Repository name
--d Date for which to collect churn(from midnight to midnight)

Alternatively, from and to dates can be specified. In which case "--d"  is not
needed

--f From date / time
--t To date / time
 
