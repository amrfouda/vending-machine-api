# 🥤 Vending Machine API

A RESTful Web API built in **C# / ASP.NET Core** that simulates a vending machine.  
Users can register as either a **buyer** or a **seller**. Sellers manage products, and buyers can deposit coins, buy products, and reset their deposit.

---

## 🚀 Features

- 🔐 User authentication with roles: `buyer`, `seller`
- 👤 User management (CRUD)
- 📦 Product management (CRUD) by seller
- 💰 Buyer deposit in accepted coins: 5, 10, 20, 50, 100 cents
- 🛒 Purchase products and receive change in coins
- 🔄 Reset buyer deposit
- ✅ Role-based access control
- 🌐 JSON-based API with RESTful design

---

## 📦 Tech Stack

- C#
- ASP.NET Core Web API
- Entity Framework Core (In-Memory DB for development)
- JWT (optional authentication layer)
- Swagger (API testing interface)

---

## 🧱 Models

### 🔹 User

| Field     | Type   | Description         |
|-----------|--------|---------------------|
| Id        | int    | Auto-generated ID   |
| Username  | string | Unique username     |
| Password  | string | Hashed password     |
| Deposit   | int    | Total in cents      |
| Role      | enum   | Buyer or Seller     |

### 🔹 Product

| Field           | Type   | Description                        |
|-----------------|--------|------------------------------------|
| Id              | int    | Auto-generated ID                  |
| ProductName     | string | Name of the product                |
| AmountAvailable | int    | Stock count                        |
| Cost            | int    | In cents (must be divisible by 5) |
| SellerId        | int    | Reference to the seller user       |

---

## 📘 API Endpoints

### 🔐 User Endpoints

| Method | Endpoint      | Access         | Description              |
|--------|---------------|----------------|--------------------------|
| POST   | `/api/users`  | Public         | Register a new user      |
| GET    | `/api/users`  | Authenticated  | Get all users            |
| PUT    | `/api/users`  | Self or Admin  | Update user              |
| DELETE | `/api/users`  | Self or Admin  | Delete user              |

### 📦 Product Endpoints

| Method | Endpoint         | Access         | Description                     |
|--------|------------------|----------------|---------------------------------|
| GET    | `/api/products`  | Public         | View all products               |
| POST   | `/api/products`  | Seller only    | Add new product                 |
| PUT    | `/api/products`  | Seller only    | Update owned product            |
| DELETE | `/api/products`  | Seller only    | Remove owned product            |

### 💰 Buyer Endpoints

| Method | Endpoint         | Access         | Description                                      |
|--------|------------------|----------------|--------------------------------------------------|
| POST   | `/api/deposit`   | Buyer only     | Deposit 5, 10, 20, 50, or 100 cent coins         |
| POST   | `/api/buy`       | Buyer only     | Buy product(s) and get change                    |
| POST   | `/api/reset`     | Buyer only     | Reset deposit to 0                               |

---

## 🔁 Change Calculation

The change is always returned in descending coin values using:  
`[100, 50, 20, 10, 5]`

Example:
```json
{
  "totalSpent": 200,
  "product": "Cola",
  "change": [50, 20, 20, 10]
}
