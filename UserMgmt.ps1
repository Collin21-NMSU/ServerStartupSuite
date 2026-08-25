#Collin Milligan UserMGMT file
#Created 4/13/2026
#Last Modified 4/14/2026

param (
	[string]$TargetName,
	[string]$Password,
	[int]$IsGroup
)

#Ensuring AD module is avaliable
Import-Module ActiveDirectory -ErrorAction SilentlyContinue

try{
	if($IsGroup -eq 1) {
		$GroupArgs = @{
			Name = $TargetName
			SamAccountName = $TargetName
			GroupCategory = "Security"
			GroupScope = "Global"
			ErrorAction = "Stop"
		}
		#GroupCreation
		New-ADGroup @GroupArgs
	}

	
	else {
		#Creation of user account and password
		$SecurePass = ConvertTo-SecureString $Password -AsPlainText -Force
		$UserArgs = @{
			Name = $TargetName
			SamAccountName = $TargetName
			AccountPassword = $SecurePass
			Enabled = $true
			ErrorAction = "Stop"
		}
		New-ADUser @UserArgs
	}
}
catch{
	#Error logs can still be handled regardless of closing window speed
	
	Write-Error "Error Occured: $($_.Exception.Message)"
	Read-Host "Press Enter to exit"
}