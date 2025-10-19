# User Microservice

## Create and publish package

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
