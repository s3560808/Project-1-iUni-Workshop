For migrate to a new version of database:
	1.Delete data in folder "iUni Workshop"
	2.Delete database "project1" in Mariadb
	3.Continue to "For set up a new database" tutorial
For set up a new database:
	1. Install Mariadb 10.3 and set up user & key
	2. Find "appsettings.Development.json" in "iUni Workshop" Folder (if cannot find create one)
	3. Add following code block into "appsettings.Development.json", PLEASE REMEMBER TO CHANGE ID and PASSWORD!!!!
		{
  			"ConnectionStrings": {
  			  "DefaultConnection": "Server=localhost;User Id=XXXXX;Password=XXXXXX;Database=project1"
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
	5.You will see five botton in a column on left bottom of your IDE, Click the second button (When you hover on the button, it will show "Refresh NuGet Packages")
	6.Open PowerShell and go to your corresponding folder, e.g. 
	cd "C:\Users\kzmai\Dropbox\_COSC_2408_Project_1\Repository\iUni Workshop"
	7.Migrate and update your database, enter command:
	dotnet ef migrations add Initial --context ApplicationDbContext
	dotnet ef database update --context ApplicationDbContext
	8. Use HeidiSQL (already installed with Mariadb) to login your MariaDB System, if you can see "project 1" database and corresponding Database it means you already connect ASP.NET Core with MySQL!


Seed Data:
  Program.cs seeding:
    https://www.learnentityframeworkcore.com/migrations/seeding
    https://dotnetthoughts.net/seed-database-in-aspnet-core/
  Commands seeding:
    https://docs.microsoft.com/en-us/ef/core/modeling/data-seeding
  LINQ where()
    https://stackoverflow.com/questions/9410321/using-select-and-where-in-a-single-linq-statement
  Unique constraint
    https://stackoverflow.com/questions/49526370/is-there-a-data-annotation-for-unique-constraint-in-ef-core-code-first
  Foreign Key
    https://msdn.microsoft.com/en-us/library/jj713564(v=vs.113).aspx
    https://stackoverflow.com/questions/25884399/aspnetusers-id-as-foreign-key-in-separate-table-one-to-one-relationship
  Composite key
    https://stackoverflow.com/questions/40898365/asp-net-add-migration-composite-primary-key-error-how-to-use-fluent-api  
  Add Identity
    https://docs.microsoft.com/en-us/aspnet/identity/overview/getting-started/introduction-to-aspnet-identity
  Save & add object (EF Core)
    https://docs.microsoft.com/en-us/ef/core/saving/basic
  Migrate database (EF Core)
    https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/adding-model?view=aspnetcore-2.1
    https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/
  Role Base
    https://docs.microsoft.com/en-us/aspnet/core/security/authorization/roles?view=aspnetcore-2.1
  Add identity
    https://docs.microsoft.com/en-us/aspnet/identity/overview/getting-started/adding-aspnet-identity-to-an-empty-or-existing-web-forms-project
  Add MySQL
    https://medium.com/@balramchavan/setup-entity-framework-core-for-mysql-in-asp-net-core-2-5b40a5a3af94
    https://www.jerriepelser.com/blog/using-mariadb-with-aspnet-core/
JavaScript
    Is integer
      https://stackoverflow.com/questions/28193973/checking-if-variable-is-an-integer-in-javascript
    Ajax
      https://www.w3schools.com/asp/asp_ajax.asp
      https://www.w3schools.com/xml/ajax_intro.asp
Select + input type
      https://www.w3schools.com/tags/tag_datalist.asp
      https://stackoverflow.com/questions/30022728/perform-action-when-clicking-html5-datalist-option

https://forums.asp.net/t/1480118.aspx?Check+ModelState+errors

validation
  https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation?view=aspnetcore-2.1
  https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-aspnet-mvc4/adding-validation-to-the-model