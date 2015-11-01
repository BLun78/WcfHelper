@echo off
set config=%1
if "%config%" == "" (
   set config=Debug
)
set version=%2
if "%version%" == "" (
   set version=9.9.9.9
)

SET prog=%PROGRAMFILES(x86)%
if "%prog%" == "" (
   set prog=%PROGRAMFILES%
)

"%prog%"\MSBuild\14.0\Bin\msbuild.exe Build.proj /p:Configuration="%config%" /p:build_number="%version%" /p:Platform="Any CPU" 
