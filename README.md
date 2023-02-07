# APIFunNoEntity
A .net API without Entity

#### Note: to pull and use this API you will need to create the MySQL tables needed and add this line of code to your appsettings.json file.
```json
 "ConnectionStrings": {
    "Default": "Server=YourServer;User ID=some_app_user;Password=aGoodPassWd;Database=yourDatabase"
  }
```
### Tables
```sql
CREATE TABLE `people` (
  `FirstName` char(50) NOT NULL,
  `LastName` char(50) NOT NULL,
  `DOB` date NOT NULL,
  `Email` char(50) DEFAULT NULL
);
```
