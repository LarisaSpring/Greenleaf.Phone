param(
	[int]$buildNumber = 0,
	[string]$task = "default"
)

"Build number $buildNumber"

src\.nuget\nuget.exe install src\.nuget\packages.config -o src\packages

Import-Module .\src\packages\psake.4.5.0\tools\psake.psm1

Invoke-Psake .\default.ps1 $task -framework "4.6x86" -properties @{ buildNumber=$buildNumber }

Remove-Module psake
