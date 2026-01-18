rem Build dll
set msbuild_path=
rem Check if Rider MSBuild exists
if exist "%LOCALAPPDATA%\Programs\Rider\tools\MSBuild\Current\Bin\amd64\MSBuild.exe" (
    echo Found Rider MSBuild
    set msbuild_path="%LOCALAPPDATA%\Programs\Rider\tools\MSBuild\Current\Bin\amd64\MSBuild.exe"
) else (
    rem Try to find MSBuild via PATH
    where msbuild.exe >nul 2>&1
    if %errorlevel% equ 0 (
        echo Found MSBuild in PATH
        set msbuild_path=msbuild
    ) else (
        echo Loading Visual Studio Tools
        call "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\Tools\VsDevCmd.bat"
        set msbuild_path=msbuild
    )
)

echo Compiling Dll
%msbuild_path% ..\KeeTheme.sln /p:Configuration=Release /p:LangVersion=3

echo Releasing Dll
copy ..\KeeTheme\bin\Release\KeeTheme.dll .\KeeTheme.dll

rem Build plgx
echo Deleting existing PlgX folder
rmdir /s /q "PlgX"

echo Creating PlgX folder
mkdir "PlgX"

echo Copying files
xcopy "..\KeeTheme\*" "PlgX" /s /e /exclude:PlgXExclude.txt

echo Compiling PlgX
"C:\Program Files\KeePass Password Safe 2\KeePass.exe" /plgx-create "%cd%\PlgX" --debug --plgx-prereq-net:3.5

echo Releasing PlgX
move /y "PlgX.plgx" "KeeTheme.plgx"

echo Cleaning up
rmdir /s /q "PlgX"

pause