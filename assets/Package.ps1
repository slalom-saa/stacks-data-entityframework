<#
.SYNOPSIS
    Packages the Stacks Entity Framework NuGet packages.
#>
param (
    $Configuration = "DEBUG",
    $IncrementVersion = $false
)

function Increment-Version() {
    $jsonpath = 'project.json'
    $json = Get-Content -Raw -Path $jsonpath | ConvertFrom-Json
    $versionString = $json.version
    $patchInt = [convert]::ToInt32($versionString.Split(".")[2].Split("-")[0], 10)
    [int]$incPatch = $patchInt + 1
    $patchUpdate = $versionString.Split(".")[0] + "." + $versionString.Split(".")[1] + "." + ($incPatch -as [string]) + "-*"
    $json.version = $patchUpdate
    $json = ConvertTo-Json $json -Depth 100


    $json = Format-Json $json    
    $json | Out-File  -FilePath $jsonpath
}


function Format-Json([Parameter(Mandatory, ValueFromPipeline)][String] $json) {
  $indent = 0;
  ($json -Split '\n' |
    % {
      if ($_ -match '[\}\]]') {
        # This line contains  ] or }, decrement the indentation level
        $indent--
      }
      $line = (' ' * $indent * 2) + $_.TrimStart().Replace(':  ', ': ')
      if ($_ -match '[\{\[]') {
        # This line contains [ or {, increment the indentation level
        $indent++
      }
      $line
  }) -Join "`n"
}

function Clear-LocalCache() {
    $paths = nuget locals all -list
    foreach($path in $paths) {
        $path = $path.Substring($path.IndexOf(' ')).Trim()

        if (Test-Path $path) {

            Push-Location $path

            foreach($item in Get-ChildItem -Filter "*Slalom.Stacks.EntityFramework*" -Recurse) {
                  if (Test-Path $item.FullName) {
                    Remove-Item $item.FullName -Recurse -Force
                    Write-Host "Removing $item"
                }
            }


            Pop-Location
    
        }
    }
}

function Go ($Path) {
    Push-Location $Path

    Remove-Item .\Bin -Force -Recurse
    if ($IncrementVersion) {
        Increment-Version
    }
    else{
        Clear-LocalCache
    }
    dotnet build
    dotnet pack --no-build --configuration $Configuration
    copy .\bin\$Configuration\*.nupkg c:\nuget\

    Pop-Location
}

Push-Location $PSScriptRoot

Go ..\src\Slalom.Stacks.EntityFramework

Pop-Location



