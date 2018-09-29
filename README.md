.NETCore WebApi project

This project contains only one webapi method, which returning paginated list of shows with casts ordered by birthday descending.

1. Build
Run following commands in the powershell command line:
cd src/
dotnet build

2. Running
If you want to run project without publishing it, do following actions:
- First, edit src/WebApi/appsettings.json and set up paths in sections "Logging" and "Database" properly.
- Second, run following commands:
cd src/
dotnet run -p webapi

Otherwise, publish project, edit appsettings.json and run command "dotnet webapi.dll" from published folder.

3. Getting data
Just type in browser http://localhost:5000/api/tvshows?page=0&pageSize=10.
Project also contains a simple log viewer: http://localhost:5000/logs