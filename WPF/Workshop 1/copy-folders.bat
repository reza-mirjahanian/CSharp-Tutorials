@echo off
setlocal enabledelayedexpansion

REM =========================
REM Batch Folder Copier
REM =========================

REM Source folder name
set "SOURCE_FOLDER=1 -"

REM How many copies to create
set COPY_COUNT=30

REM =========================

REM Check if source folder exists
if not exist "%SOURCE_FOLDER%\" (
    echo WARNING: Source folder not found: %SOURCE_FOLDER%
    pause
    exit /b
)

REM Extract starting number
for /f "tokens=1" %%A in ("%SOURCE_FOLDER%") do (
    set /a START_NUMBER=%%A + 1
)

REM Create copies
for /L %%I in (0,1,%COPY_COUNT%) do (

    set /a FOLDER_NUMBER=!START_NUMBER! + %%I - 1
    set "NEW_FOLDER=!FOLDER_NUMBER! - "

    REM Skip existing folders
    if exist "!NEW_FOLDER!\" (
        echo WARNING: Skipping existing folder: !NEW_FOLDER!
    ) else (
        xcopy "%SOURCE_FOLDER%" "!NEW_FOLDER!\" /E /I /Q /Y >nul

        if errorlevel 1 (
            echo WARNING: Failed to create: !NEW_FOLDER!
        ) else (
            echo Created: !NEW_FOLDER!
        )
    )
)

echo.
echo Done.
pause
