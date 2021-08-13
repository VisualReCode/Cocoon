# Sets the application pool to loadUserProfile so it can boot localdb and create files.
Import-Module WebAdministration;
Set-ItemProperty "IIS:\AppPools\DefaultAppPool" -Name "processmodel.loadUserProfile" -Value "True"