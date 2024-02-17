# .NET Authentication Example
.NET Core Microsoft Identity Role Based Authorization example.

# Requirements
- .NET 8

# Endpoints
- api/v1/users
	- GET
		- GetUsers
		- ("{id}")
			- GetById(string id)
	- POST
		- Create(CreateUserModel user)
	- /connect/token
		- Login(UserLoginModel userLogin)