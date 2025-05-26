# 🧪 Developer Evaluation Project - AMBEV

Welcome to the Developer Evaluation Challenge. This project implements a complete Sales API with full CRUD operations, following DDD (Domain-Driven Design) principles and respecting specific business rules.

---

## 📋 Instructions

- You have **7 calendar days** from the receipt of this challenge to deliver the solution.
- The code must be versioned in a **public GitHub repository**.
- Please upload this template to your repository and work from it.
- Make sure all requirements are implemented before submission.
- This `README` includes instructions on how to **configure**, **run**, and **test** the project.
- Code quality, organization and documentation are part of the evaluation criteria.

---

## 🚀 Use Case

You are a developer on the **DeveloperStore** team. Your mission is to implement an API to manage **Sales Records**, supporting full **CRUD operations** with the following data:

### 📦 Sale Details

- Sale Number
- Sale Date
- Customer
- Total Amount
- Branch
- Products:
   - Quantity
   - Unit Price
   - Discount
   - Total per Item
   - Cancelled / Not Cancelled

Additionally, it’s a plus to **log domain events** such as:

- `SaleCreated`
- `SaleModified`
- `SaleCancelled`
- `ItemCancelled`

No actual message broker is required — logging to console or file is acceptable.

---

## 📏 Business Rules

These rules apply to **discounts by quantity per product**:

| Quantity Range       | Discount |
|----------------------|----------|
| 1 - 3 items          | ❌ No discount |
| 4 - 9 items          | ✅ 10%     |
| 10 - 20 items        | ✅ 20%     |
| Above 20 items       | ❌ Not allowed |

> ❗ Sales with more than 20 identical items must be **rejected**.

---

## 🛠️ Tech Stack

| Layer         | Technology                                   |
|---------------|----------------------------------------------|
| Language      | C# (.NET 8)                                  |
| Architecture  | Clean Architecture + DDD                     |
| API           | ASP.NET Core Web API                         |
| Persistence   | Entity Framework Core (In-Memory or Postgres) |
| Testing       | xUnit                                        |
| Logging       | Serilog (Console/File)                       |

---

## 📂 Project Structure

```
src/
├── DeveloperStore.Api           # Entry point (Web API)
├── DeveloperStore.Application   # Use Cases, DTOs, Interfaces
├── DeveloperStore.Domain        # Entities, Value Objects, Domain Events
├── DeveloperStore.Infrastructure# EF, Repositories, Mappings
└── DeveloperStore.Tests         # Unit and Integration Tests
```

---

## ⚙️ How to Run

### 📌 Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Git](https://git-scm.com/)

### ▶️ Run the project

```bash
# Clone the repo
git clone https://github.com/leandro-oliveira83/challenge-ambev.git
cd challenge-ambev

# Restore dependencies
dotnet restore

# Run the API
dotnet run --project src/Ambev.DeveloperEvaluation.WebApi
```

API should now be available at: `http://localhost:5119/swagger/index.html`.

---

## 🧪 Running Tests

```bash
dotnet test
```

---

## 📌 Observations

- This solution uses **GitFlow** to manage development lifecycle.
- Commit messages follow **Conventional Commits** for readability and automation.
- Domain events are logged when actions are performed (e.g., `SaleCreated`).

---

## 📞 Contact

If you have any questions about the implementation or reasoning behind design decisions, feel free to reach out through GitHub.

---


---

## 🗄️ Running Migrations

### 🔧 Step 1: Configure Connection String

Update the connection string in the `appsettings.json` file of the **Application** project:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=DeveloperStoreDb;User Id=your_user;Password=your_password;"
  }
}
```

Make sure the database and credentials are correct and accessible.

### ▶️ Step 2: Run Migrations

To add or apply EF Core migrations, use the following command pattern **from the solution root**:

```bash
dotnet ef migrations add InitialCreate --project src/DeveloperStore.Infrastructure --startup-project src/DeveloperStore.Api --context SalesDbContext
```

To apply migrations to the database:

```bash
dotnet ef database update --project src/DeveloperStore.Infrastructure --startup-project src/DeveloperStore.Api --context SalesDbContext
```

> These commands ensure EF Core can locate both the DbContext (in `Infrastructure`) and the app entry point (in `Api`).

---