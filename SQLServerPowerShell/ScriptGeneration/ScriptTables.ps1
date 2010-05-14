param ($serverInstance, $databaseName, $scriptOutputPath)

function CheckParam([string] $param, [string] $message)
{
	if ($param -eq $null -or $param.Length -eq 0) 
    { 
        write-output $message 
    }
}

CheckParam($serverInstance, "A server and instance must be provided")
CheckParam($serverInstance, "A database must be specified")

#load the SQL Server management assesmbly:
[System.Reflection.Assembly]::LoadWithPartialName('Microsoft.SqlServer.SMO') 

#connect to server instance (e.g. "LOCAL\DEV01") and database (e.g. "AdventureWorks"):
$server = new-object ('Microsoft.SqlServer.Management.Smo.Server') $serverInstance 
$database = $server.Databases[$databaseName]

#create the scripter and populate options:
$scripter = new-object ("Microsoft.SqlServer.Management.Smo.Scripter") ($server)
$scripter.Options.DriAll = $True
$scripter.Options.IncludeHeaders = $False
$scripter.Options.ToFileOnly = $True
$scripter.Options.WithDependencies = $False

#the scripter takes a URN collection of objects to script:
$urns = new-object ('Microsoft.SqlServer.Management.Smo.UrnCollection')

#but we generate them one at a time to use the table for the filename:
foreach ($item in $database.Tables) 
{
    $urns.Clear()
    $urns.Add($item.Urn)
    $filename = $item.Schema + "_" + $item.Name + ".sql"
    $scripter.Options.FileName = [IO.Path]::Combine($scriptOutputPath, $filename)
    $scripter.Options.AppendToFile = $False
    $scripter.Options.ScriptSchema = $True
    $scripter.Options.IncludeIfNotExists = $True
    $scripter.EnumScript($urns)
} 