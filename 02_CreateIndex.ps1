# credenciales
$root = 'http://localhost:9200/'
$username = "elastic"
$pass= "n5elasticsearch"

# creacion 
$auth = $username + ':' + $pass
$Encoded = [System.Text.Encoding]::UTF8.GetBytes($auth)
$authorizationInfo = [System.Convert]::ToBase64String($Encoded)
$headers = @{"Authorization"="Basic $($authorizationInfo)"}
$headers.Add('Content-Type','application/json')

try {

    # json de configuracion de indice
    $JSON = @{
        "settings"=@{
            "number_of_shards"=1
            "number_of_replicas"=0
        }
    } | ConvertTo-Json

    # creacion del indice n5permissions
    $uriCreateIndex = $root + "n5permissions"
    $response = Invoke-WebRequest -Uri $uriCreateIndex -Method PUT -Headers $headers -Body $JSON

    if($response.StatusCode -eq 200) {

        Write-Host "[INFO] : Indice creado" 

        # creacion de json de mapping
        $JSON = @{
            "properties"=@{
                "Id"=@{"type"="integer"} 
                "EmployeeForename"=@{"type"="text"} 
                "EmployeeSurname"=@{"type"="text"} 
                "PermissionType"=@{"type"="integer"} 
                "PermissionDate"=@{"type"="date"} 
            }
        } | ConvertTo-Json

        # creacion del mapping del indice 
        $uriCreateIndex = $root + "n5permissions/_mapping"
        $response = Invoke-WebRequest -Uri $uriCreateIndex -Method PUT -Headers $headers -Body $JSON

        if($response.StatusCode -eq 200) {
            Write-Host "[INFO] : Mapping creado y asociado al indice" 
        }
    }    
}
catch {
    Write-Host "[ERROR] : Ocurrio un error en la creacion del indice"  
}
