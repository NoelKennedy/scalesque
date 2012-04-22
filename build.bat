msbuild scalesque.sln /t:Clean;Rebuild /p:Configuration=Release

copy Scalesque.net35\bin\Release\*.* nuget\lib\net35-client
copy Scalesque.net40\bin\Release\*.* nuget\lib\net40-client
cd nuget
nuget pack scalesque.nuspec
cd ..
