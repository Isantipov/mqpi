dotnet build -c Release -r centos.7-x64
dotnet publish -c Release -r centos.7-x64
rm mqpi/bin/Release/netcoreapp2.0/centos.7-x64/publish.zip
cd mqpi/bin/Release/netcoreapp2.0/centos.7-x64/publish
zip -r ../publish.zip *