# MovieApp Backend

MovieApp is a backend application built with **ASP.NET Core** for managing movies.

## Technologies

- **ASP.NET Core 8.0+**
- **Entity Framework Core**
- **MariaDB**
- **JWT Authentication**

## Installation

1. **Clone the repository**:

   ```bash
   git clone git@github.com:joanbier/movieApp_backend.git
   cd movieApp_backend
   
2. **Restore dependencies:**:

dotnet restore

3. **Set up the database connection

-in launchSettings.json set your envs db e.g.

    "MovieApp.Api": {
      "commandName": "Project",
      "environmentVariables": {
        "DB_SERVER": "localhost",
        "DB_PORT": "3306",
        "DB_USER": "root",
        "DB_PASSWORD": "root",
        "DB_NAME": "MovieDB"
      }
    }
    
-in appsettings.json set connection string like this:
  "ConnectionStrings": {
    "MovieAppCS": "Server=${DB_SERVER};Port=${DB_PORT};Database=${DB_NAME};User=${DB_USER};Password=${DB_PASSWORD};"
  },    
  
  4. **Run database migrations:**
  
  dotnet ef database update
  
  5. **Run the application**
  
  dotnet run
  
  
  The application will be available at http://localhost:5157/swagger/index.html
  
  6. **Example of Endpoints**
  
  GET /api/movies -get movies
  POST /api/movies -add a movie
  UPDATE /api/movies -update a movie
  GET /api/movies/{id} -get a movie by id
  DELETE /api/movies -remove a movie
  
