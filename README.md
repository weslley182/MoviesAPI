# MoviesAPI

## On Windows and Linux:

 - Visual Studio 2022 Community Or VS Code
 - Sqlite3
 - SDK .Net 6
 
## Run commands:
  ```bash
  
 $ dotnet clean
 $ dotnet build
 $ dotnet ef database update
 
 ```
 
## To seed, On MoviesAPI:
 ```bash
 
 $ dotnet run seeddata
 
 ``` 
 
## Running the app:
 - https://localhost:7250/swagger/index.html
 
## Test, on MoviesAPI.Tests

 ```bash

 $ dotnet test
 or Test/Run All Tests on Visual Studio
 
 - On Linux, update Path To:
 - "CSVPath": "../../../Data/movielist.csv"



 ``` 