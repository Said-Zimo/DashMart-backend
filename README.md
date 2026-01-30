# 🚀 Project Overview

This project is a **RESTful Web API** built with **ASP.NET Core**, following **Clean Architecture principles**.  
It demonstrates how to design a **secure, scalable, and maintainable backend system** using modern .NET practices.

---

## 🧱 Architecture

- 🧠 **Domain** – Rich domain models, aggregates, entities, and business rules  
- ⚙️ **Application** – CQRS with MediatR and FluentValidation  
- 🗄️ **Infrastructure** – EF Core + SQL Server  
- 🌐 **Presentation** – RESTful Web API

---

## 🧩 Architecture, Patterns & Principles

- Clean Architecture  
- CQRS  
- DDD
- Result
- Unit Of Work
- Rich Domain Model 
- SOLID
- Dependency Injection  

---

## 🛠️ Technologies

- ASP.NET Core Web API  
- Entity Framework Core  
- SQL Server  
- MediatR  
- FluentValidation  
- JWT Authentication  

---

## 🔐 Security

- JWT Authentication with custom configuration
- Role & Permission-based Authorization
- Aggregate ownership validation
- Two-key strategy for aggregate entities
- Named CORS policies
- Current user context abstraction

---

## 🚨 Error Handling

- Global Exception Handling
- Consistent API error responses
- Problem Details (RFC 7807) compliant error responses

---

## 🗄️ Database (MS SQL)

- Normalized schema  
- Indexes & constraints for performance  
- Data integrity enforced  

---

## 🔁 API Style

- RESTful endpoints  
- Proper HTTP verbs  

---

## 📘 API Documentation

- Postman / Swagger integration for API exploration and testing  
- JWT Bearer authentication support in Swagger UI

---

## ⚙️ Configuration

- Strongly-typed JWT settings using the Options pattern  
- Environment-based configuration (appsettings, secrets)
- Configured named CORS policies for controlled cross-origin access

---

## 🎯 Purpose