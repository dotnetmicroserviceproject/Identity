# User Microservice

## Create and publish the NuGet package

```powershell
$version="1.0.7"
$owner="dotnetmicroserviceproject"
$gh_pat="[PAT HERE]"

dotnet pack src\User.Contracts\ --configuration Release -p:PackageVersion=$version -p:RepositoryUrl=https://github.com/$owner/user -o ..\packages

dotnet nuget push ..\packages\User.Contracts.$version.nupkg --api-key $gh_pat --source "github"

```
## Build the docker image 
```powershell 
$env:GH_OWNER="dotnetmicroserviceproject" 
$env:GH_PAT="[PAT HERE]" 

docker build --secret id=GH_OWNER --secret id=GH_PAT -t user.service:$version . 
``` 

## Run the docker image 
```powershell 
$adminPass="[PASSWORD HERE]" 
docker run -it --rm -p 5002:5002 --name identity -e MongoDbSettings__Host=mongo -e 
RabbitMQSettings__Host=rabbitmq -e IdentitySettings__AdminUserPassword=$adminPass --network 
playinfra_default play.identity:$version 

``` 


  ## Run the docker image 
```powershell 
$adminPass="[Password Here]" 
$connectionstring="[connectionstring Here]" 

docker run -it --rm -p 5002:5002 --name user-service --network infra_backend -e ConnectionStrings__DefaultConnectionString=$connectionstring  -e RabbitMQSettings__Host=rabbitmq -e IdentitySettings__AdminUserPassword=$adminPass user.service:latest
``` 