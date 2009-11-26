@ECHO OFF

SET Path=C:\Program Files\Microsoft Visual Studio 8\SDK\v2.0\Bin;C:\WINDOWS\Microsoft.NET\Framework\v2.0.50727;C:\Program Files\Microsoft Visual Studio 8\VC\bin;C:\Program Files\Microsoft Visual Studio 8\Common7\IDE;C:\Program Files\Microsoft Visual Studio 8\VC\vcpackages;C:\Program Files\Microsoft Platform SDK\Bin;%PATH%
SET /P _SQL=SQL Server instance:
IF "%_SQL%"=="" GOTO :error
msbuild deploy.proj /t:Deploy /p:SQLServerName=%_SQL%
GOTO :end

:error
ECHO SQL Server instance name required. No work done.

:end