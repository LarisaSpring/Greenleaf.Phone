properties {
    $projectName            = "Greenleaf.Phone"
    $buildNumber            = 0
    $rootDir                = Resolve-Path .\
    $buildOutputDir         = "$rootDir\build"
    $mergedDir              = "$buildOutputDir\merged"
    $reportsDir             = "$buildOutputDir\reports"
    $srcDir                 = "$rootDir\src"
    $packagesDir            = "$srcDir\packages"
    $solutionFilePath       = "$srcDir\$projectName.sln"
    $assemblyInfoFilePath   = "$srcDir\SharedAssemblyInfo.cs"
    $ilmergePath            = FindTool "ILMerge.*\tools\ilmerge.exe" "$packagesDir"
    $nugetPath              = "$srcDir\.nuget\nuget.exe"
}

task default -depends Clean, UpdateVersion, CreateNuGetPackages

task Clean {
    Remove-Item $buildOutputDir -Force -Recurse -ErrorAction SilentlyContinue
    exec { msbuild /nologo /verbosity:quiet $solutionFilePath /t:Clean /p:platform="Any CPU"}
}

task RestoreNuget {
    Get-PackageConfigs |% {
        "Restoring " + $_
        &$nugetPath install $_ -o "$srcDir\packages" -configfile $_
    }
}

task UpdateVersion {
    $version = Get-Version $assemblyInfoFilePath
    $oldVersion = New-Object Version $version
    $newVersion = New-Object Version ($oldVersion.Major, $oldVersion.Minor, $oldVersion.Build, $buildNumber)
    Update-Version $newVersion $assemblyInfoFilePath
}

task Compile -depends RestoreNuget {
    exec { msbuild /nologo /verbosity:quiet $solutionFilePath /p:Configuration=Release /p:platform="Any CPU"}
}

task PreparePackagge -depends Compile {
    New-Item $mergedDir -Type Directory -ErrorAction SilentlyContinue

    $mainDllName = "Greenleaf.MVVM"
    $dllDir = "$srcDir\$mainDllName\Bin\Release"
    $inputDlls = "$dllDir\Greenleaf*.dll"
    $inputPdbs = "$dllDir\Greenleaf*.pdb"
    Copy-Item -Path $inputDlls -Destination $mergedDir
    Copy-Item -Path $inputPdbs -Destination $mergedDir
    
    $mainDllName = "Greenleaf.Phone"
    $dllDir = "$srcDir\$mainDllName\Bin\Release"
    $inputDlls = "$dllDir\Greenleaf*.dll"
    $inputPdbs = "$dllDir\Greenleaf*.pdb"
    Copy-Item -Path $inputDlls -Destination $mergedDir
    Copy-Item -Path $inputPdbs -Destination $mergedDir
    
    $mainDllName = "Greenleaf.UWP"
    $dllDir = "$srcDir\$mainDllName\Bin\Release"
    $inputDlls = "$dllDir\Greenleaf*.dll"
    $inputPdbs = "$dllDir\Greenleaf*.pdb"
    Copy-Item -Path $inputDlls -Destination $mergedDir
    Copy-Item -Path $inputPdbs -Destination $mergedDir
}

task ILMerge -depends Compile {
    New-Item $mergedDir -Type Directory -ErrorAction SilentlyContinue

    $mainDllName = "Greenleaf.Phone"
    $dllDir = "$srcDir\$mainDllName\Bin\Release"
    $inputDlls = "$dllDir\Greenleaf*.dll"
    Invoke-Expression "$ilmergePath /targetplatform:v4 /internalize /wildcards /allowDup /target:library /log /out:$mergedDir\$mainDllName.dll $inputDlls"
}

task CreateNuGetPackages -depends PreparePackagge {
    $versionString = Get-Version $assemblyInfoFilePath
    $version = New-Object Version $versionString
    $packageVersion = $version.Major.ToString() + "." + $version.Minor.ToString() + "." + $version.Build.ToString() + "." + $buildNumber.ToString()
    $packageVersion
    gci $srcDir -Recurse -Include *.nuspec | % {
        exec { .$srcDir\.nuget\nuget.exe pack $_ -o $buildOutputDir -version $packageVersion }
    }
}

function FindTool {
	param (
		[string]$name,
		[string]$packageDir
	)

	$result = Get-ChildItem "$packageDir\$name" | Select-Object -First 1
	return $result.FullName
}

function Get-PackageConfigs {
    return gci $srcDir -Recurse "packages.config" -ea SilentlyContinue | foreach-object { $_.FullName }
}

function EnsureDirectory {
    param (
		[string]$directory
	)

    if(!(test-path $directory))	{
        mkdir $directory
    }
}

function Get-Version {
	param (
		[string]$assemblyInfoFilePath
	)
	
	Write-Host "path $assemblyInfoFilePath"
	$pattern = '(?<=^\[assembly\: AssemblyVersion\(\")(?<versionString>\d+\.\d+\.\d+\.\d+)(?=\"\))'
	$assmblyInfoContent = Get-Content $assemblyInfoFilePath
	return $assmblyInfoContent | Select-String -Pattern $pattern | Select -expand Matches |% {$_.Groups['versionString'].Value}
}

function Update-Version {
	param (
		[string]$version,
		[string]$assemblyInfoFilePath
	)

	$newVersion = 'AssemblyVersion("' + $version + '")';
	$newFileVersion = 'AssemblyFileVersion("' + $version + '")';
	$tmpFile = $assemblyInfoFilePath + ".tmp"

	Get-Content $assemblyInfoFilePath |
		%{$_ -replace 'AssemblyFileVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)', $newFileVersion }  | Out-File -Encoding UTF8 $tmpFile

	Move-Item $tmpFile $assemblyInfoFilePath -force
}
