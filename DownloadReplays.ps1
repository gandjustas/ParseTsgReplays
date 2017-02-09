    #FTP Server Information - SET VARIABLES
    $ftp = "ftp://37.204.226.59:21021" 
    $user = 'armaplayer' 
    $pass = '1'
    $folder = 'MPMissions/Replay'
    $target = "D:\Arma\"


    $existing = Get-ChildItem -Path $target | % { $_.Name }

    #SET CREDENTIALS
    $credentials = new-object System.Net.NetworkCredential($user, $pass)

    function Get-FtpDir ($url,$credentials) {
        $request = [Net.WebRequest]::Create($url)
        $request.Method = [System.Net.WebRequestMethods+FTP]::ListDirectory
        if ($credentials) { $request.Credentials = $credentials }
        $response = $request.GetResponse()
        $reader = New-Object IO.StreamReader $response.GetResponseStream() 
        $reader.ReadToEnd()
        $reader.Close()
        $response.Close()
    }


    function Download-FtpDirRecursive($url,$credentials,$target, $existing)
    {
        $Allfiles=Get-FTPDir -url $url -credentials $credentials
        $files = ($Allfiles -split "`r`n")

        $files 



        $webclient = New-Object System.Net.WebClient 
        $webclient.Credentials = $credentials
        $counter = 0
        foreach ($file in ($files | where {$_ -like "*.*"})){
            if($existing -contains $file) { continue; }
            $source=$url + $file  
            $destination = $target + $file 
            $webclient.DownloadFile($source, $target+$file)

            #PRINT FILE NAME AND COUNTER
            $counter++
            $counter
            $source
        }


        foreach ($dir in ($files | where {$_ -notlike "*.*" -and $_ -ne ""})){
            $url+$dir+"/"
            Download-FtpDirRecursive -url ($url+$dir+"/") -credentials $credentials -target $target -existing $existing
        }
    }

    $folderPath = $ftp + "/" + $folder + "/"
    
    Download-FtpDirRecursive -url $folderPath -credentials $credentials -target $target -existing $existing


    #Extract 7zips
    $pbos = new-object 'System.Collections.Generic.HashSet[string]'

    Get-ChildItem -Path "Unpack\*.pbo" | % { $pbos.Add($_.Name.Substring(0,2)+"-"+$_.Name.Substring(3,19)) } | out-null 

    $7zips = (Get-ChildItem -Path "*.7z" | where { -not $pbos.Contains($_.Name.Substring(0,2)+"-"+$_.Name.Substring(3,19))  })
    foreach($archive in $7zips) {
        & 'C:\Program Files\7-Zip\7z.exe' e $archive.Name -oUnpack -aos
    }

    #Extract pbos

    $folders = new-object 'System.Collections.Generic.HashSet[string]'
    Get-ChildItem -Path "Unpack\*" -Directory | % { $folders.Add($_.Name) } | out-null 
    $pbos1 = (Get-ChildItem -Path "Unpack\*.pbo" | where { -not $folders.Contains($_.BaseName) })
    foreach($pbo in $pbos1) {
        & ExtractPboDos.exe -P -N $pbo.FullName 
    }



