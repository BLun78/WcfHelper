.\.nuget\nuget.exe push .\PackageOutputDir\Blun.WcfHelper.*.*.*.*.nupkg
.\.nuget\nuget.exe push .\PackageOutputDir\Blun.WcfHelper.Mock.*.*.*.*.nupkg

.\.nuget\nuget.exe push .\PackageOutputDir\Blun.WcfHelper.*.*.*.*.symbols.nupkg -Source http://nuget.gw.symbolsource.org/Public/NuGet
.\.nuget\nuget.exe push .\PackageOutputDir\Blun.WcfHelper.Mock.*.*.*.*.symbols.nupkg -Source http://nuget.gw.symbolsource.org/Public/NuGet
pause