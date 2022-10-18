Add-Type -Path "$($args[0])\Neos_Data\Managed\PostX.dll"
Set-Location  ".\bin\$($args[1])"
[PostX.NeosAssemblyPostProcessor].GetMethod("Process").Invoke($null, @("$PWD\NEOSPlus.dll", "$PWD"))