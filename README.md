# Inventory & Order Management System

A simple ASP.NET Core MVC application to manage products and orders.

---

## Features

* Product management (create, list, search, edit, delete)
* Order management (create, list, details)
* Dynamic order items (add/remove rows)
* Auto calculation of totals (client-side)
* AJAX product search

---

## Technologies

* ASP.NET Core MVC
* Dapper/ADO.NET(Command,Query)
* Entity Framework Core(Identity)
* jQuery / JavaScript
* Razor, Bootstrap

---

## Setup Instructions

### 1. Clone the repository

```bash
git clone https://github.com/aburayhan98/Inventory-Order-Erp.git
cd Inventory-Order-Erp
```

---

### 2. Configure database

Open `appsettings.json` and update connection string:

```json
"ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=InventoryDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}

---

### 3. Apply migrations
```bash
dotnet ef migrations add AddIdentityTables --project Inventory.Infrastructure --startup-project Inventory.Web
```bash
dotnet ef database update --project Inventory.Infrastructure --startup-project Inventory.Web
```

---

### 4. Run the project

```bash
dotnet run
```

---

### 5. Open in browser

```
https://localhost:7227/
```

---

## Notes

* Order total is calculated on client side and saved on server
* JavaScript for order page is in:

```
wwwroot/js/order.js
```

---

## Author

Abu Rayhan
