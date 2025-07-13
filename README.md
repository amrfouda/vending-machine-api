
# 🥤 Vending Machine API

A **RESTful Web API** built in **C# / ASP.NET Core** that simulates a vending machine.  
Users can register as either **Buyer** or **Seller**:

- **Buyers**: Deposit coins, buy products, get change, reset deposit.  
- **Sellers**: Add, update, or delete products they own.

---

## 🚀 Features

- 🔐 **JWT Authentication** with role-based access: `buyer`, `seller`
- 👤 **User management** (register & profile)
- 📦 **Product management** (CRUD) by seller
- 💰 **Buyer deposit**: accepts `5`, `10`, `20`, `50`, `100` cent coins
- 🛒 **Purchase products** and receive change in coins
- 🔄 **Reset buyer deposit**
- ✅ **Role-based access control**
- 🌐 **JSON-based REST API**  
- 🔍 **Swagger UI** for easy API testing

---

## 📦 Tech Stack

- **C# / ASP.NET Core 8**
- **Entity Framework Core** (SQLite or In-Memory for dev)
- **JWT Authentication**
- **Swagger / OpenAPI**

---

## 🧱 Models

### 🔹 User

| Field      | Type   | Description                 |
|------------|--------|-----------------------------|
| Id         | int    | Auto-generated ID           |
| Username   | string | Unique username             |
| Password   | string | Stored as hashed password   |
| Deposit    | int    | Buyer balance (in cents)    |
| Role       | string | `buyer` or `seller`         |

---

### 🔹 Product

| Field           | Type   | Description                        |
|-----------------|--------|------------------------------------|
| Id              | int    | Auto-generated ID                  |
| ProductName     | string | Name of the product                |
| AmountAvailable | int    | Product stock                      |
| Cost            | int    | In cents (divisible by 5)          |
| SellerId        | int    | Reference to seller user           |

---

## 🔐 Authentication

- Uses **JWT Bearer Token** for authentication.
- Each request (except registration and login) must include:

```
Authorization: Bearer <your-token>
```

---

## 📘 API Endpoints

### 🔐 **Auth Endpoints**

| Method | Endpoint            | Access  | Description             |
|---------|--------------------|---------|-------------------------|
| POST    | `/api/auth/login`   | Public  | Login & get JWT token    |

---

### 👤 **User Endpoints**

| Method | Endpoint            | Access        | Description                |
|---------|--------------------|----------------|----------------------------|
| POST    | `/api/users/register` | Public    | Register a new user         |
| GET     | `/api/users/me`     | Buyer/Seller | Get current user info       |
| POST    | `/api/users/deposit` | Buyer      | Deposit coins (5,10,20,50,100) |
| POST    | `/api/users/reset`  | Buyer       | Reset deposit to zero       |

---

### 📦 **Product Endpoints**

| Method | Endpoint              | Access       | Description                   |
|---------|----------------------|--------------|-------------------------------|
| GET     | `/api/products`       | Public       | View all products              |
| POST    | `/api/products`       | Seller       | Add a new product              |
| PUT     | `/api/products/{id}`  | Seller       | Update owned product           |
| DELETE  | `/api/products/{id}`  | Seller       | Delete owned product           |

---

### 🛒 **Buy Endpoint**

| Method | Endpoint    | Access | Description                   |
|---------|-------------|--------|-------------------------------|
| POST    | `/api/buy`  | Buyer  | Buy product(s) & get change    |

---

## 💰 Change Calculation Example

Change is returned in coins `[100, 50, 20, 10, 5]`:

**Example response:**

```json
{
  "totalSpent": 200,
  "productName": "Cola",
  "quantity": 2,
  "change": [50, 20, 20, 10]
}
```

---

## 🧪 Testing with Postman

### 1️⃣ **Login & Get Token**

- **POST** `/api/auth/login`
```json
{
  "username": "buyer1",
  "password": "1234"
}
```
- **Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

---

### 2️⃣ **Send Authorized Requests**

In Postman, for any **protected endpoint**:

- Go to **Authorization Tab**
- Set **Type** to `Bearer Token`
- Paste the **token** from login

Or in Headers manually:

```
Authorization: Bearer <your-token>
```

---

## ⚙️ **Run the Project**

```bash
dotnet restore
dotnet build
dotnet run
```

Swagger UI available at:

```
https://localhost:5209/swagger
```

---

## 🚀 **Project Setup Notes**

- Configure your **JWT Secret** in `appsettings.json`:

```json
"Jwt": {
  "Key": "Your32CharSecretKeyHere",
  "ExpireHours": 1
}
```

---

## 📂 **Project Structure**

```
/Controllers
  - AuthController.cs
  - UserController.cs
  - ProductController.cs

/Models
  - User.cs
  - Product.cs
  - LoginRequest.cs

/Helpers
  - PasswordHasher.cs

Program.cs
```

---

## 🛡️ **Security Notes**

- Passwords are hashed with **SHA256** (consider upgrading to **BCrypt** in production).
- JWT keys should be stored securely (e.g., `dotnet user-secrets`).
