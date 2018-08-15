1. Install Mariadb 10.3 and set up user & key
2. Find "appsettings.Development.json" in "iUni Workshop" Folder (if cannot find create one)
3. Add following code block into "appsettings.Development.json", PLEASE REMEMBER TO CHANGE ID and PASSWORD!!!!
		{
  			"ConnectionStrings": {
  			  "DefaultConnection": "Server=localhost;User Id=XXXXX;Password=XXXXXX;Database=Project1"
  			},
  			"Logging": {
  			  "IncludeScopes": false,
  			  "LogLevel": {
  			    "Default": "Debug",
  			    "System": "Information",
  			    "Microsoft": "Information"
  			  }
  			}
		}
	
4.Open project with Rider, Go to "Boom Job Matching System"->"WebApplication1"->right click->"Manage NuGet Packages"
5.You will see five botton in a column on left bottom of your IDE, Click the second botton (When you hover on the button, it will show "Refresh NuGet Packages")
6.Open PowerShell and go to your corresponding folder, e.g. 
	cd "C:\Users\kzmai\Dropbox\_COSC_2408_Project_1\Repository\iUni Workshop"
7.Migrate and update your database, enter command:
	dotnet ef migrations add Initial --context ApplicationDbContext
	dotnet ef database update --context ApplicationDbContext
8. Use HeidiSQL (already installed with Mariadb) to login your MariaDB System, if you can see "project 1" database and corresponding Database it means you already connect ASP.NET Core with MySQL!