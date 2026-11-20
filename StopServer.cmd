rem working directory
set workdir=%~dp0
cd %workdir%

rem Stop only THIS project's node gateway (nodejs/app.js), never another app's
rem node.exe (WorkBuddy, Playwright etc. also run node.exe). Fall back to the
rem owner of port 9001 so a gateway started with a different command line is
rem still stopped. PowerShell single-quoted literals avoid cmd escaping.
@powershell -NoProfile -ExecutionPolicy Bypass -Command "$ids = @(Get-CimInstance Win32_Process | Where-Object { $_.Name -eq 'node.exe' -and $_.CommandLine -like '*nodejs*app.js*' } | ForEach-Object { $_.ProcessId }); $port = @(Get-NetTCPConnection -LocalPort 9001 -State Listen -ErrorAction SilentlyContinue | ForEach-Object { $_.OwningProcess }); foreach ($p in @($ids + $port | Sort-Object -Unique)) { $pr = Get-Process -Id $p -ErrorAction SilentlyContinue; if ($pr -and $pr.ProcessName -eq 'node') { Write-Host ('Stopping project node gateway PID ' + $p); Stop-Process -Id $p -Force -ErrorAction SilentlyContinue } }"
@taskkill /im RoomServer.exe /f
@taskkill /im UserServer.exe /f
@taskkill /im Lobby.exe /f
@taskkill /im DataCache.exe /f
@taskkill /im ServerCenter.exe /f
@taskkill /im ServerCenter.exe /f

