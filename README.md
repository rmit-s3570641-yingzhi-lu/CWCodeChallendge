## Frontend
1. Bootstrap v5
2. ASP.NET MVC Razer View (.NET Core 3.1)

## Backend
Integrated Cosmos DB using Factory and Repository Pattern

## How to Run
Go to appsettings and specify the Cosmos DB configuration
```json
  "ConnectionStrings": {
    "ServiceEndpoint": "https://cw-code-challendge-db.documents.azure.com:443/",
    "AuthKey": ""
  },
  "CosmosDb": {
    "DatabaseName": "cw",
    "CollectionNames": [
      {
        "Name": "products",
        "PartitionKey": "/id" // This has to be id for now
      }
    ]
  },
```
 Then go to the web project and start it. As long as the config is correct, all DB and container will be created for you.
 
 ## Demo
 ![alt text](https://i.imgur.com/Tb1pYl0.gif)

## Try by yourself
https://cwcodechallendge20210801233047.azurewebsites.net

## What Lefts
Due to the time limitation, these are the items remaing in progress
1. UNIT TESTS! 
2. Sorting and Pagination
3. Implementing actual js based frontend. (Currently I am using Ravor View Tag Helper ect to do very simple validation, still covered frontend + backend)
