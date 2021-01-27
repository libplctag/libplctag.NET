
# This script is used to make it easier to copy native libraries into the libplctag.NativeImport project without error.


# Step 1: Download the releases and put in a folder called "releases" next to this script. The folder structure should look like this:
#   .\releases
#   .\releases\libplctag_2.2.0_macos_x64.zip
#   .\releases\libplctag_2.2.0_ubuntu_x64.zip
#   .\releases\libplctag_2.2.0_ubuntu_x86.zip
#   .\releases\libplctag_2.2.0_windows_x64.zip
#   .\releases\libplctag_2.2.0_windows_x86.zip

# Step 2: Run the script

# Step 3: Verify that the files have been correctly copied

# Step 4: Make relevant modifications to libplctag.NativeImport such as modifying the method signatures (if required)

# Step 5: Increment version number of libplctag.NativeImport project

# Step 6: Upload to nuget
#         Note there is a github action which automatically builds libplctag.NativeImport and uploads to nuget











$releasesFolder = ".\releases\"
$destinationFolder = ".\src\libplctag.NativeImport\runtime"



Push-Location
Set-Location $releasesFolder

Get-Item libplctag_*_macos_x64.zip   | Expand-Archive
Get-Item libplctag_*_ubuntu_x64.zip  | Expand-Archive
Get-Item libplctag_*_ubuntu_x86.zip  | Expand-Archive
Get-Item libplctag_*_windows_x64.zip | Expand-Archive
Get-Item libplctag_*_windows_x86.zip | Expand-Archive

Pop-Location



Copy-Item $releasesFolder\libplctag_*_macos_x64\libplctag.dylib         $destinationFolder\osx_x64\libplctag.dylib
Copy-Item $releasesFolder\libplctag_*_ubuntu_x64\libplctag.so           $destinationFolder\linux_x64\libplctag.so
Copy-Item $releasesFolder\libplctag_*_ubuntu_x86\libplctag.so           $destinationFolder\linux_x86\libplctag.so
Copy-Item $releasesFolder\libplctag_*_windows_x64\Release\plctag.dll    $destinationFolder\win_x64\plctag.dll
Copy-Item $releasesFolder\libplctag_*_windows_x86\Release\plctag.dll    $destinationFolder\win_x86\plctag.dll








Get-FileHash $releasesFolder\libplctag_*_macos_x64\libplctag.dylib
Get-FileHash $destinationFolder\osx_x64\libplctag.dylib

Get-FileHash $releasesFolder\libplctag_*_ubuntu_x64\libplctag.so
Get-FileHash $destinationFolder\linux_x64\libplctag.so

Get-FileHash $releasesFolder\libplctag_*_ubuntu_x86\libplctag.so
Get-FileHash $destinationFolder\linux_x86\libplctag.so

Get-FileHash $releasesFolder\libplctag_*_windows_x64\Release\plctag.dll
Get-FileHash $destinationFolder\win_x64\plctag.dll

Get-FileHash $releasesFolder\libplctag_*_windows_x86\Release\plctag.dll
Get-FileHash $destinationFolder\win_x86\plctag.dll