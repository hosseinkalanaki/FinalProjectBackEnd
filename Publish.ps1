
cd .\SignalRWebpack\

if (Test-Path ".\bin\Publish") {
    Remove-Item -Path ".\bin\Publish" -Recurse -Force
    Write-Host "Previous publish directory removed."
}
Write-Host "Clear Publish Director"


Write-Host "
>>Start publish with this configuration:
                                            --configuration Release `
                                            --framework net7.0 `
                                            --output bin\Publish `
                                            --self-contained false
"

dotnet publish `
    --configuration Release `
    --framework net7.0 `
    --output bin\Publish `
    --self-contained false
	
	
cd ..

Write-Host "Publishing verison $currentVersionInCsproj in date $currentDate was Completed!"
