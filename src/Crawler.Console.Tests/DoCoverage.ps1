nuget install OpenCover -Version 4.6.519 -OutputDirectory ../packages
nuget install ReportGenerator -Version 2.4.5 -OutputDirectory ../packages
..\packages\OpenCover.4.6.519\tools\OpenCover.Console.exe "-target:C:\Program Files\dotnet\dotnet.exe" -targetargs:test -register:user -filter:"+[*]* -[xunit*]*" -output:coverage\coverage.xml -oldStyle
..\packages\ReportGenerator.2.4.5.0\tools\ReportGenerator.exe -reports:"coverage\coverage.xml" -targetdir:"coverage\CoverageReport"
start .\coverage\CoverageReport\index.htm