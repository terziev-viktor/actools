# Windows build VM for Content Manager

Content Manager is a WPF app on .NET Framework 4.5.2 (x86), so it can only be built and run
on Windows. This folder runs a Windows 11 VM inside Docker
([dockur/windows](https://github.com/dockur/windows), KVM-backed). You edit code on the
Linux host as usual; the VM builds and runs the app.

Requirements: `/dev/kvm` access, ~70 GB free disk, 8 GB RAM for the VM (adjust in `compose.yml`).

## Quick start

All commands run **on the host**, from the repo root:

```sh
.devcontainer/cm up       # start the VM
.devcontainer/cm build    # build the whole solution (Debug|x86)
```

`cm build` connects to the VM over SSH by itself — there is nothing to connect to first.
When asked for a password, type `admin`.

Output: `C:\actools\Output\x86\Debug\Content Manager.exe` (inside the VM).

## First start (one-time)

The first `cm up` downloads and installs Windows unattended, then runs `oem/install.ps1`,
which installs VS 2022 Build Tools (.NET desktop/WPF), the .NET 4.5.2 reference assemblies,
the x86 VC++ runtime and OpenSSH. It took ~11 minutes here; watch it at
http://127.0.0.1:8006. It's finished when `cm ssh` works and `C:\OEM\install.log` ends
with `ALL DONE`. Windows runs unactivated, which is fine for development.

## Commands

| Command                     | What it does                                       |
|-----------------------------|----------------------------------------------------|
| `.devcontainer/cm up`       | Start the VM (`docker ps` shows `actools-windows-windows-1`) |
| `.devcontainer/cm build`    | Copy the repo into the VM and build `AcManager.sln` |
| `.devcontainer/cm sync`     | Only copy the repo into the VM, no build           |
| `.devcontainer/cm ssh`      | Open a shell (cmd) inside the VM                   |
| `.devcontainer/cm logs`     | Follow the VM container's logs                     |
| `.devcontainer/cm down`     | Stop the VM (its disk is kept)                     |

## Getting inside the VM

Credentials everywhere: user `Docker`, password `admin`.

- **Browser:** http://127.0.0.1:8006 — the Windows desktop in a web page.
- **RDP:** `xfreerdp /v:127.0.0.1 /u:Docker /p:admin /dynamic-resolution` (port 3389).
- **SSH:** `.devcontainer/cm ssh` (port 2222). To build from that shell:
  ```
  powershell -ExecutionPolicy Bypass -File \\host.lan\Data\.devcontainer\cm.ps1 build
  ```

On the desktop there are two shortcuts:

- **Build CM** — same as `cm build`.
- **Content Manager (dev)** — launches the last build.

## Running the app

Open the desktop (browser or RDP) and double-click **Content Manager (dev)**. On first launch
CM asks for the Assetto Corsa folder, so copy an AC install into the VM (e.g. via the
`Shared`/`Z:` folder — see below — or download it inside the VM). The VM has no GPU, so 3D
showroom features may not work.

## How it works

- The repo is mounted **read-only** into the VM as `\\host.lan\Data` (also `Z:` / `Shared`
  on the desktop).
- `cm.ps1` mirrors it to `C:\actools` (skipping `.git`, `bin`, `obj`, `Output`, `packages`)
  and builds there, so build outputs stay inside the VM and your host tree stays clean.
  **Edit on the host** — changes made in `C:\actools` are overwritten on the next sync.
- `packages.zip` is extracted into `C:\actools\packages` whenever it changes.
- `AcTools.Render.Tests` references MSTest without a hint path (full VS finds it implicitly);
  `cm.ps1` copies that one DLL to `C:\Tools\MSTestRef` and passes it as `ReferencePath`.
- Ports 8006 (web), 3389 (RDP) and 2222 (SSH) are bound to 127.0.0.1 only.

## Files

| File               | Purpose                                              |
|--------------------|------------------------------------------------------|
| `compose.yml`      | The VM definition (Windows version, CPU/RAM/disk, ports, mounts) |
| `oem/install.ps1`  | One-time toolchain setup, run at the end of Windows install |
| `cm.ps1`           | Sync + build script; runs inside the VM              |
| `cm`               | Host wrapper (`up`, `build`, `ssh`, ...)             |

## Troubleshooting

- **`cm build` hangs or "connection refused":** the VM isn't up yet. Run `cm up` and check
  http://127.0.0.1:8006.
- **"MSBuild not found":** the one-time setup didn't finish — check `C:\OEM\install.log`;
  `C:\OEM\install.ps1` can be re-run from an admin PowerShell.
- **Host key warning after recreating the VM:** `ssh-keygen -R actools-windows`.
- **Start over completely:** `docker compose -f .devcontainer/compose.yml down -v`
  (deletes the VM disk).
