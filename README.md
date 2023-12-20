# Store-Product backend with FastEndpoints in ASP.NET Core

## For each entity, 4 endpoints are available:
### - Create
### - Get
### - Update
### - Delete
#### Each endpoint can be secured by adding 
```
Policies("AdminsOnly")

```
#### in the overriden "Configure" method, as this is the registered authorization policy.

## Identity is implemented using ASP.NET Core Identity
### - 2 endpoints are avaialble for register and login.
### - JWT is used for authorization.

## SqlServer is used for the DB connection with TrustedWindowsConnection
