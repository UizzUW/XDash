Param(
    [string]$dir 
)


function ZipFiles( $zipfilename, $sourcedir )
{
   Add-Type -Assembly System.IO.Compression.FileSystem
   $compressionLevel = [System.IO.Compression.CompressionLevel]::Optimal
   [System.IO.Compression.ZipFile]::CreateFromDirectory($sourcedir,
        $zipfilename, $compressionLevel, $false)
}

$folderName = Get-ChildItem "$dir\AppxPackages\" | Select-Object -first 1
$FileName = "$($dir)\xdash.zip"
if (Test-Path $FileName) {
  Remove-Item $FileName
}
Write-Host "creating zip of folder $dir\AppxPackages\"
ZipFiles $FileName "$dir\AppxPackages\";
Write-Host "created zip file with name $($FileName)"