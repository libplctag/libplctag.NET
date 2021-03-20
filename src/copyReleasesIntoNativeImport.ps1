
# This script is used to make it easier to copy native libraries into the libplctag.NativeImport project without error.



# Step 1: Run the script with the selected version
if($args.Count -ne 1)
{
  Write-Host "Please specify version to be downloaded (e.g. '2.3.6')"
  Exit
}
$version = $args[0]

# Step 2: Verify that the files have been correctly copied

# Step 3: Make relevant modifications to libplctag.NativeImport such as modifying the method signatures (if required)

# Step 4: Increment version number of libplctag.NativeImport project

# Step 5: Upload to nuget
#         Note there is a github action which automatically builds libplctag.NativeImport and uploads to nuget




$macos_x64 = @{     zip = "libplctag_$($version)_macos_x64.zip";                  source = "libplctag_$($version)_macos_x64\libplctag.dylib";                       destination = "osx_x64\libplctag.dylib" }
$linux_x64 = @{     zip = "libplctag_$($version)_ubuntu_x64.zip";                 source = "libplctag_$($version)_ubuntu_x64\libplctag.so";                         destination = "linux_x64\libplctag.so" }
$linux_x86 = @{     zip = "libplctag_$($version)_ubuntu_x86.zip";                 source = "libplctag_$($version)_ubuntu_x86\libplctag.so";                         destination = "linux_x86\libplctag.so" }
$linux_ARM = @{     zip = "libplctag_$($version)_linux_arm7l_EXPERIMENTAL.zip";   source = "libplctag_$($version)_linux_arm7l_EXPERIMENTAL\libplctag.so";           destination = "linux_ARM\libplctag.so" }
$linux_ARM64 = @{   zip = "libplctag_$($version)_linux_aarch64_EXPERIMENTAL.zip"; source = "libplctag_$($version)_linux_aarch64_EXPERIMENTAL\libplctag.so";         destination = "linux_ARM64\libplctag.so" }
$windows_x64 = @{   zip = "libplctag_$($version)_windows_x64.zip";                source = "libplctag_$($version)_windows_x64\Release\plctag.dll";                  destination = "win_x64\plctag.dll" }
$windows_x86 = @{   zip = "libplctag_$($version)_windows_x86.zip";                source = "libplctag_$($version)_windows_x86\Release\plctag.dll";                  destination = "win_x86\plctag.dll" }
$windows_ARM = @{   zip = "libplctag_$($version)_windows_Arm.zip";                source = "libplctag_$($version)_windows_Arm\Release\plctag.dll";                  destination = "win_ARM\plctag.dll" }
$windows_ARM64 = @{ zip = "libplctag_$($version)_windows_Arm64_EXPERIMENTAL.zip"; source = "libplctag_$($version)_windows_Arm64_EXPERIMENTAL\Release\plctag.dll";   destination = "win_ARM64\plctag.dll" }




$builds = @(
    $macos_x64,
    $linux_x64,
    $linux_x86,
    $linux_ARM,
    $linux_ARM64,
    $windows_x64,
    $windows_x86,
    $windows_ARM,
    $windows_ARM64
)


$downloadUriRoot = "https://github.com/libplctag/libplctag/releases/download/v$($version)"
$sourceFolder = ".\releases"
$destinationFolder = ".\libplctag.NativeImport\runtime"





# Create a folder to put the builds and enter into it
Remove-Item $sourceFolder -Recurse -Force
New-Item -ItemType "directory" -Name $sourceFolder
Push-Location
Set-Location $sourceFolder




# Download the builds
foreach ($build in $builds) {
	$uri = "$downloadUriRoot/$($build.zip)"
    Invoke-WebRequest -Uri $uri -OutFile $build.zip
}




# Unzip the build files
foreach ($build in $builds) {
	Get-Item $build.zip      | Expand-Archive
}




# Exit out of the builds folder
Pop-Location




# Copy each file into libplctag.NET NativeImport project
foreach ($build in $builds) {
    Copy-Item "$sourceFolder\$($build.source)" "$destinationFolder\$($build.destination)"
}




# Help the user to confirm that each file has been copied into the correct folder
foreach ($build in $builds) {
    Write-Host "$($build.zip)"
    Get-FileHash "$sourceFolder\$($build.source)"
    Get-FileHash "$destinationFolder\$($build.destination)"
    Write-Host
}
