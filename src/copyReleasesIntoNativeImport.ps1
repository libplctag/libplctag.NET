
# This script is used to make it easier to copy native libraries into the libplctag.NativeImport project without error.


# Step 1: Download the releases and put in a folder called "releases" next to this script. The folder structure should look like this:
#   .\releases
#   .\releases\libplctag_[version]_[os1]_[architecture1].zip
#   .\releases\libplctag_[version]_[os2]_[architecture2].zip
#   .\releases\libplctag_[version]_[os3]_[architecture3].zip
#   ...

# Step 2: Run the script

# Step 3: Verify that the files have been correctly copied

# Step 4: Make relevant modifications to libplctag.NativeImport such as modifying the method signatures (if required)

# Step 5: Increment version number of libplctag.NativeImport project

# Step 6: Upload to nuget
#         Note there is a github action which automatically builds libplctag.NativeImport and uploads to nuget






$macos_x64 = @{     zip = "libplctag_*_macos_x64.zip";                  source = "libplctag_*_macos_x64\libplctag.dylib";                       destination = "osx_x64\libplctag.dylib" }
$linux_x64 = @{     zip = "libplctag_*_ubuntu_x64.zip";                 source = "libplctag_*_ubuntu_x64\libplctag.so";                         destination = "linux_x64\libplctag.so" }
$linux_x86 = @{     zip = "libplctag_*_ubuntu_x86.zip";                 source = "libplctag_*_ubuntu_x86\libplctag.so";                         destination = "linux_x86\libplctag.so" }
$linux_ARM = @{     zip = "libplctag_*_linux_arm7l_EXPERIMENTAL.zip";   source = "libplctag_*_linux_arm7l_EXPERIMENTAL\libplctag.so";           destination = "linux_ARM\libplctag.so" }
$linux_ARM64 = @{   zip = "libplctag_*_linux_aarch64_EXPERIMENTAL.zip"; source = "libplctag_*_linux_aarch64_EXPERIMENTAL\libplctag.so";         destination = "linux_ARM64\libplctag.so" }
$windows_x64 = @{   zip = "libplctag_*_windows_x64.zip";                source = "libplctag_*_windows_x64\Release\plctag.dll";                  destination = "win_x64\plctag.dll" }
$windows_x86 = @{   zip = "libplctag_*_windows_x86.zip";                source = "libplctag_*_windows_x86\Release\plctag.dll";                  destination = "win_x86\plctag.dll" }
$windows_ARM = @{   zip = "libplctag_*_windows_Arm.zip";                source = "libplctag_*_windows_Arm\Release\plctag.dll";                  destination = "win_ARM\plctag.dll" }
$windows_ARM64 = @{ zip = "libplctag_*_windows_Arm64_EXPERIMENTAL.zip"; source = "libplctag_*_windows_Arm64_EXPERIMENTAL\Release\plctag.dll";   destination = "win_ARM64\plctag.dll" }




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



$sourceFolder = ".\releases"
$destinationFolder = ".\libplctag.NativeImport\runtime"






# Unzip the build files
Push-Location
Set-Location $sourceFolder

foreach ($build in $builds) {
	Get-Item $build.zip      | Expand-Archive
}

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
