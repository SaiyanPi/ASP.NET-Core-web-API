# About this project

Hello. This is a project i am engaging in currently. I am having a fun building this process. Though I've not implemented any specific WEB API Design patterns, it's great. The main reason why it's great for me is that i can play a lot. Instead of sticking to just one practice for implementing feature or refining it, i can explore different patterns. So codes are a bit
inconsistent in some area but again my main objective building this project is to learn.


# Multi-Tenant Task Manager

A multi-tenant task management application built with ASP.NET Core and Entity Framework.


## Features

- Multi-tenant architecture
- Role and policy based authorization
- Task and Project management
- Activity logging
- EF Core with soft delete


 ## Tech Stack
 
- ASP.NET Core 8
- Entity Framework Core
- JWT Authentication
- SQL Server
- FluentValidation


## Design Overview 

### User roles :
- SuperAdmin
- Admin
- Manager
- Member
- SpecialMember
  
### Permissions per role
#### 1. SuperAdmin
- lives outside tenant scope
- manages tenants(CREATE, DELETE, UPDATE Tenants)

#### 2. Admin
- lives inside Tenant
- manages User(UPDATE, DELETE Users from Tenant)
- manage Projects(CREATE, DELETE, UPDATE Projects)

#### 3. Manager
- lives inside Tenant
- manages Projects(CREATE, DELETE, UPDATE, Assign Manager, SpecialMember, and Member Users, Update Project status)
- manages Tasks(CREATE, DELETE, UPDATE, Assign User, Update Task status)

#### 4. SpecialMember
- lives inside Tenant
- manages Tasks(CREATE, DELETE, UPDATE, Assign Member User, Update Task status)

#### 5. Member
- lives inside Tenant
- view Tasks
