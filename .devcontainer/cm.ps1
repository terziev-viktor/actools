# Runs inside the Windows VM. Usage: cm.ps1 sync|build|test [extra nunit3-console args]
# Mirrors the read-only repo share into C:\actools, then builds/tests the Debug|x86 solution there.
param(
    [Parameter(Mandatory, Position = 0)][ValidateSet('sync', 'build', 'test')][string]$Command,
    [Parameter(ValueFromRemainingArguments)][string[]]$TestArgs = @()
)
$ErrorActionPreference = 'Stop'
$Source = '\\host.lan\Data'
$Work = 'C:\actools'

function Sync {
    # /MIR mirrors deletions too; excluded dirs (build outputs, extracted packages) are neither copied nor purged.
    robocopy $Source $Work /MIR /XD .git .vs bin obj Output packages /R:2 /W:1 /NFL /NDL /NJH /NP | Out-Null
    if ($LASTEXITCODE -ge 8) { throw "robocopy failed with $LASTEXITCODE" }
    $zip = Join-Path $Work 'packages.zip'
    $stamp = Join-Path $Work 'packages\.extracted'
    if (-not (Test-Path $stamp) -or (Get-Item $zip).LastWriteTimeUtc -gt (Get-Item $stamp).LastWriteTimeUtc) {
        Write-Host 'Extracting packages.zip...'
        tar -xf $zip -C $Work
        if ($LASTEXITCODE -ne 0) { throw 'Extracting packages.zip failed' }
        New-Item -ItemType File -Force $stamp | Out-Null
    }
}

function Build {
    $vswhere = "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe"
    $msbuild = & $vswhere -latest -products * -requires Microsoft.Component.MSBuild -find 'MSBuild\**\Bin\MSBuild.exe' | Select-Object -First 1
    if (-not $msbuild) { throw 'MSBuild not found; did C:\OEM\install.ps1 finish? See C:\OEM\install.log' }
    # AcTools.Render.Tests references MSTest (QualityTools.UnitTestFramework) without a hint path;
    # full VS resolves it implicitly, Build Tools only ship it under TestPlatform. ReferencePath is
    # searched before HintPath, so expose a folder holding only that DLL (TestPlatform also has its
    # own Newtonsoft.Json, which would shadow the repo's copy).
    $refDir = 'C:\Tools\MSTestRef'
    $dll = 'Microsoft.VisualStudio.QualityTools.UnitTestFramework.dll'
    if (-not (Test-Path "$refDir\$dll")) {
        $vs = & $vswhere -latest -products * -property installationPath
        New-Item -ItemType Directory -Force $refDir | Out-Null
        Copy-Item "$vs\Common7\IDE\Extensions\TestPlatform\$dll" $refDir
    }
    & $msbuild (Join-Path $Work 'AcManager.sln') /m /nologo /v:minimal /p:Configuration=Debug /p:Platform=x86 "/p:ReferencePath=$refDir"
    if ($LASTEXITCODE -ne 0) { throw "Build failed ($LASTEXITCODE)" }
    Write-Host "Built: $Work\Output\x86\Debug\Content Manager.exe"
}

function Test {
    $tests = @(
        'Output\x86\Debug\AcManager.Tools.Tests.dll',
        'Output\x86\Debug\FirstFloor.ModernUI.Tests.dll',
        'Output\x86\Debug\StringBasedFilter.Tests.dll',
        'Output\x86\Debug\AcTools.Render.Tests.dll',
        'AcTools.Tests\bin\x86\Debug\AcTools.Tests.dll',
        'AcTools.NeuralTyres.Tests\bin\x86\Debug\AcTools.NeuralTyres.Tests.dll'
    ) | ForEach-Object { Join-Path $Work $_ }
    & 'C:\Tools\NUnit.ConsoleRunner\tools\nunit3-console.exe' $tests --x86 --noheader "--work=$Work" @TestArgs
    if ($LASTEXITCODE -ne 0) { throw "Tests failed ($LASTEXITCODE)" }
}

Sync
if ($Command -in 'build', 'test') { Build }
if ($Command -eq 'test') { Test }
