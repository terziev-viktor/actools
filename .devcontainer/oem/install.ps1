# One-time toolchain setup for building, testing and running Content Manager.
# Log: C:\OEM\install.log. Safe to re-run manually if a step failed.
$ErrorActionPreference = 'Stop'
$ProgressPreference = 'SilentlyContinue'
[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
$tmp = 'C:\OEM\downloads'
New-Item -ItemType Directory -Force $tmp | Out-Null

function Step($name, [scriptblock]$body) {
    Write-Host "== $name"
    & $body
    Write-Host "== ${name}: done"
}

Step 'Visual Studio 2022 Build Tools (.NET desktop + WPF)' {
    $exe = "$tmp\vs_buildtools.exe"
    Invoke-WebRequest 'https://aka.ms/vs/17/release/vs_buildtools.exe' -OutFile $exe
    $p = Start-Process $exe -Wait -PassThru -ArgumentList @(
        '--quiet', '--wait', '--norestart', '--nocache',
        '--add', 'Microsoft.VisualStudio.Workload.ManagedDesktopBuildTools',
        '--includeRecommended')
    # 3010 = success, reboot required.
    if ($p.ExitCode -notin 0, 3010) { throw "vs_buildtools exited with $($p.ExitCode)" }
}

# VS 2022 no longer ships the 4.5.2 targeting pack; the projects target v4.5.2,
# so install the reference assemblies from the official NuGet package.
Step '.NET Framework 4.5.2 reference assemblies' {
    $dest = "${env:ProgramFiles(x86)}\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5.2"
    $zip = "$tmp\net452.zip"
    Invoke-WebRequest 'https://www.nuget.org/api/v2/package/Microsoft.NETFramework.ReferenceAssemblies.net452/1.0.3' -OutFile $zip
    Expand-Archive $zip "$tmp\net452" -Force
    New-Item -ItemType Directory -Force $dest | Out-Null
    Copy-Item "$tmp\net452\build\.NETFramework\v4.5.2\*" $dest -Recurse -Force
}

# The test projects reference NUnit 3.x but the repo has no runner.
Step 'NUnit console runner' {
    $zip = "$tmp\nunit.zip"
    Invoke-WebRequest 'https://www.nuget.org/api/v2/package/NUnit.ConsoleRunner/3.16.3' -OutFile $zip
    Expand-Archive $zip 'C:\Tools\NUnit.ConsoleRunner' -Force
}

# Runtime for CefSharp/SlimDX native parts (CM is x86).
Step 'Visual C++ runtime (x86)' {
    $exe = "$tmp\vc_redist.x86.exe"
    Invoke-WebRequest 'https://aka.ms/vs/17/release/vc_redist.x86.exe' -OutFile $exe
    $p = Start-Process $exe -Wait -PassThru -ArgumentList '/install', '/quiet', '/norestart'
    if ($p.ExitCode -notin 0, 1638, 3010) { throw "vc_redist exited with $($p.ExitCode)" }
}

Step 'OpenSSH server' {
    Add-WindowsCapability -Online -Name 'OpenSSH.Server~~~~0.0.1.0' | Out-Null
    Set-Service sshd -StartupType Automatic
    Start-Service sshd
    if (-not (Get-NetFirewallRule -Name 'OpenSSH-Server-In-TCP' -ErrorAction SilentlyContinue)) {
        New-NetFirewallRule -Name 'OpenSSH-Server-In-TCP' -DisplayName 'OpenSSH Server (sshd)' `
            -Enabled True -Direction Inbound -Protocol TCP -Action Allow -LocalPort 22 | Out-Null
    }
}

Step 'Desktop shortcuts' {
    $desktop = [Environment]::GetFolderPath('CommonDesktopDirectory')
    $shell = New-Object -ComObject WScript.Shell
    $s = $shell.CreateShortcut("$desktop\Content Manager (dev).lnk")
    $s.TargetPath = 'C:\actools\Output\x86\Debug\Content Manager.exe'
    $s.WorkingDirectory = 'C:\actools\Output\x86\Debug'
    $s.Save()
    $s = $shell.CreateShortcut("$desktop\Build CM.lnk")
    $s.TargetPath = 'powershell.exe'
    $s.Arguments = '-NoExit -NoProfile -ExecutionPolicy Bypass -File \\host.lan\Data\.devcontainer\cm.ps1 build'
    $s.Save()
}

Remove-Item $tmp -Recurse -Force
Write-Host 'ALL DONE'
