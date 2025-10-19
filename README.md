# User Microservice

## Create and publish package

```powershell
$version="1.0.7"
$owner="dotnetmicroserviceproject"
$gh_pat="[PAT HERE]"

dotnet pack src\User.Contracts\ --configuration Release -p:PackageVersion=$version -p:RepositoryUrl=https://github.com/$owner/user -o ..\packages

dotnet nuget push ..\packages\User.Contracts.$version.nupkg --api-key $gh_pat --source "github"

```


