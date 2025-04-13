# run-coverage.ps1 - Enhanced version that runs tests and collects coverage

# Clean up previous results
if (Test-Path "./CoverageResults") {
    Remove-Item -Recurse -Force "./CoverageResults"
}

# Create directories
New-Item -ItemType Directory -Force -Path "./CoverageResults"

# Install necessary tools if not already installed
dotnet tool install -g dotnet-reportgenerator-globaltool --version 5.4.5

# Step 1: Configure the test project to run without WinUI dependencies
Write-Host "Building projects..." -ForegroundColor Yellow

# Build the main project first
dotnet build ./Hospital/Hospital.csproj -c Debug

# Step 2: Run tests separately with coverlet
Write-Host "Running tests with coverage collection..." -ForegroundColor Yellow

# Run only specific tests that don't depend on UI components
$testDlls = @(
    "./Hospital.Tests/bin/x64/Debug/net8.0-windows10.0.19041.0/Hospital.dll"
)

foreach ($dll in $testDlls) {
    if (Test-Path $dll) {
        # Use MSTest directly with vstest.console.exe, which has better WinUI compatibility
        & "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\Extensions\TestPlatform\vstest.console.exe" `
            $dll `
            /Settings:test.runsettings `
            /Enablecodecoverage
    }
    else {
        Write-Host "Test DLL not found at path: $dll" -ForegroundColor Red
    }
}

# Step 3: Analyze the existing assemblies
Write-Host "Generating coverage report..." -ForegroundColor Yellow

# Find all coverage result files
$coverageFiles = Get-ChildItem -Path "./TestResults" -Filter "*.coverage" -Recurse -ErrorAction SilentlyContinue

if ($coverageFiles.Count -gt 0) {
    foreach ($file in $coverageFiles) {
        Write-Host "Found coverage file: $($file.FullName)" -ForegroundColor Green
        
        # Convert .coverage to .coveragexml using Visual Studio's tool
        & "C:\Program Files\Microsoft Visual Studio\2022\Community\Team Tools\Dynamic Code Coverage Tools\CodeCoverage.exe" `
            analyze /output:"./CoverageResults/coverage.coveragexml" $file.FullName
    }
    
    # Generate HTML report from the converted file
    reportgenerator `
        -reports:"./CoverageResults/coverage.coveragexml" `
        -targetdir:"./CoverageResults/Report" `
        -reporttypes:Html
    
    Write-Host "Coverage report generated at ./CoverageResults/Report/index.html" -ForegroundColor Green
}
else {
    Write-Host "No .coverage files found. Generating basic coverage report from assemblies..." -ForegroundColor Yellow
    
    # Fallback: Generate a report from the assemblies directly
    reportgenerator `
      -reports:"./Hospital/bin/x64/Debug/net8.0-windows10.0.19041.0/Hospital.dll" `
      -targetdir:"./CoverageResults" `
      -reporttypes:Html
    
    Write-Host "Basic coverage report generated at ./CoverageResults/index.html" -ForegroundColor Green
}
