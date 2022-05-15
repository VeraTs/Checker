-- CREATING A NEW DB LOCALLY --
in visual studio, work according to the pdf guide:
1) open View > SQL Server Object View
2) in view: Right Click SQL Server> (LocalDB)\MSSQLLocalDB > Databases
3) in selection opened, choose "Add New Database"
4) Give the DB the name "CheckerDataBase" (location can be whatever is most convenient to you)
5) Click OK
6) SUPER IMPORTANT: creating the tables in the DB:
	note that in the solution, you have a migrations folder 
	that is not empty.
	To do the migration:
	a) open the POWERSHELL (you can search for it in the search bar in VS)
	b) In powershell: enter the following:
		cd ./CheckerServer/
		dotnet ef
	c) if you get an error, it means you have to download the entity framework tool, but that shouldn't happen
	d) now enter:
		dotnet ef database update
	   it will run the migration and create the tables.
	
