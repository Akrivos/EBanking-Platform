# MellonBank

MellonBank is an ASP.NET Core MVC e-banking application. The project follows a layered **Clean Architecture** approach and provides two separate environments:

- **Staff Panel** for bank employees
- **Customer Panel** for bank customers

Authentication and authorization are implemented with **ASP.NET Core Identity** and role-based access control.

---

## Project Overview

The application simulates a simplified e-banking platform where:

- staff users manage customers, staff users, and bank accounts
- customer users access their own accounts only
- customers can check balances, transfer money, and change their password
- exchange rate conversion is used to display balance in **EUR** and **USD**

The solution is structured with the following layers:

- **Web** – MVC UI, Razor views, controllers, areas, and view models
- **Application** – use cases, DTOs, validators, interfaces, exceptions, and business services
- **Domain** – entities, enums, domain rules, and exceptions
- **Infrastructure** – EF Core persistence, Identity integration, external API services, and repositories

---

## Main Features

## 1. Staff Area

Only users with the **Staff** role can access the staff area.

### Staff capabilities

- Create new **Customer** users
- Create new **Staff** users
- View all customers
- Search customer by **AFM**
- View customer details
- Edit customer information
- Delete customer
- Create bank account
- View all bank accounts
- View bank account details
- Edit bank account information
- Delete bank account (implemented as soft delete)

### Staff notes

- Public user registration is not used as the main flow
- New users are created by staff through staff-only pages
- Customers and staff are separated by role

---

## 2. Customer Area

Only users with the **Customer** role can access the customer area.

### Customer capabilities

- View their own accounts only
- Select one account when they have more than one account
- View account details
- Check account balance in **EUR** and **USD**
- Transfer money between their own accounts
- Transfer money to a third-party account
- Change password

### Customer notes

- Account selection is implemented through the **My Accounts** page
- Each account card provides direct actions for:
  - View Details
  - Check Balance
  - Transfer to My Account
  - Transfer to Third Party

---

## Business Rules and Important Behaviors

### Account deletion

The application implements **soft delete** for bank accounts.

### Current behavior

- deleting an account does **not physically remove** the record from the database
- instead, the account is marked as inactive
- inactive accounts are excluded from normal account queries and UI lists

### User deletion restriction

A user **cannot be deleted** when they still own one or more bank accounts.

This restriction is intentional and prevents orphaned account data (i.e., accounts without an owner).

---

## Authentication and Authorization

The project uses **ASP.NET Core Identity**.

### Authentication

- Login is implemented through Identity
- Logout is implemented through Identity
- Password change is available for customers

### Authorization

Role-based access is enforced:

- **Staff** users can access only staff pages
- **Customer** users can access only customer pages

### Login redirect

After login, users are redirected based on their role:

- Staff -> `Staff/Dashboard`
- Customer -> `Customer/Dashboard`

---

## Architecture

The solution follows Clean Architecture principles.

### Web Layer

Contains:

- MVC controllers
- Razor views
- Areas (`Staff`, `Customer`, `Identity`)
- ViewModels

### Application Layer

Contains:

- service interfaces
- service implementations
- DTOs
- FluentValidation validators
- use case logic

### Domain Layer

Contains:

- entities
- enums
- domain methods
- domain exceptions

### Infrastructure Layer

Contains:

- Entity Framework Core
- DbContext
- repositories
- Identity integration
- seeders
- external exchange rate provider
- persistence configurations

---

## Technologies Used

- ASP.NET Core MVC
- Razor Views
- ASP.NET Core Identity
- Entity Framework Core
- SQL Server
- FluentValidation
- Serilog
- Bootstrap
- ExchangeRateProvider API

---

## Exchange Rate Integration

The application uses an external exchange rate API to convert account balances from **EUR** to **USD**.

### Current implementation

- balances are stored in the database in **EUR only**
- conversion to **USD** is performed on demand
- the application calls the external API using `HttpClient`

### Provider service

Exchange rate conversion is handled through an infrastructure service.

### Notes

- the API key must be configured before running the application
- balance conversion is customer-facing and used in the balance view

---

 ## Optional Currency API Endpoint

As an additional optional feature, the application includes a public Currency API endpoint.

### Purpose

This endpoint returns the supported exchange currencies used by MellonBank for EUR conversions.

Supported currencies

- AUD (Australian Dollar)
- CHF (Swiss Franc)
- GBP (British Pound)
- USD (US Dollar)

Implementation details

- Currency records are seeded automatically during application startup.
- Default exchange rate values are inserted during initial seeding.
- The endpoint can be consumed by any user (authenticated or not), depending on controller configuration.

### Example endpoint

`/api/Currency/GetRates`

Returned data includes the current conversion rates used internally by the application.

---

## Important Pages and Paths

## Public / Shared

- `/` -> Home page
- `/Identity/Account/Login` -> Login page

## Staff Area

- `/Staff/Dashboard` -> Staff dashboard
- `/Staff/Users/CreateCustomer` -> Create customer
- `/Staff/Users/CreateStaff` -> Create staff
- `/Staff/Customers` -> Customer list
- `/Staff/Customers/Details?afm={afm}` -> Customer details
- `/Staff/Customers/Edit?afm={afm}` -> Edit customer
- `/Staff/Customers/Delete?afm={afm}` -> Delete customer (only if no bank accounts exist for the specified customer)
- `/Staff/BankAccounts` -> Bank account list
- `/Staff/BankAccounts/Create` -> Create bank account
- `/Staff/BankAccounts/Details?accountNumber={accountNumber}` -> Bank account details
- `/Staff/BankAccounts/Edit?accountNumber={accountNumber}` -> Edit bank account
- `/Staff/BankAccounts/Delete?accountNumber={accountNumber}` -> Delete bank account (soft delete)

## Customer Area

- `/Customer/Dashboard` -> Customer dashboard
- `/Customer/Banking/MyAccounts` -> My accounts
- `/Customer/Banking/AccountDetails?accountNumber={accountNumber}` -> Account details
- `/Customer/Banking/Balance?accountNumber={accountNumber}` -> Balance in EUR and USD
- `/Customer/Banking/TransferToOwn?accountNumber={accountNumber}` -> Transfer between own accounts
- `/Customer/Banking/TransferToThirdParty?accountNumber={accountNumber}` -> Transfer to third-party account
- `/Customer/Profile/ChangePassword` -> Change password

---

## Initial Seed Data

The application supports startup seeding for identity roles and an initial staff user.

### Seeded roles

- `Staff`
- `Customer`

### Seeded initial staff user

A default staff user can be created during development startup.

## Default Seeded Staff Account

Credentials:

| Field | Value |
|------|------|
| Username | staffadmin |
| Email | staff@staff.com |
| Password | Admin123! |

### Notes

- migrations are applied on startup

---

## Setup Instructions

## 1. Prerequisites

Make sure the following are installed:

- .NET SDK
- SQL Server
- Visual Studio

---

## 2. Configure the database connection

Update the connection string in:

- `appsettings.json`
- or `appsettings.Development.json`

Example:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=MellonBankDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
}
```

---

## 3. Configure the exchange rate API

Add your API settings to configuration:

```json
"ExchangeRateApi": {
  "ApiKey": "YOUR_API_KEY",
  "BaseUrl": "https://v6.exchangerate-api.com/v6/"
}
```

Without a valid API key, the balance conversion feature will not work.

---

## 4. Apply migrations

You can either let the application apply migrations automatically at startup, or apply them manually.

### Manual EF Core migration commands

```bash
dotnet ef migrations add InitialCreate --project MellonBank.Infrastructure --startup-project MellonBank.Web --output-dir Migrations
dotnet ef database update --project MellonBank.Infrastructure --startup-project MellonBank.Web
```

If migrations already exist, you only need:

```bash
dotnet ef database update --project MellonBank.Infrastructure --startup-project MellonBank.Web
```

---

## 5. Run the application

```bash
dotnet run --project MellonBank.Web
```

Launch the application and navigate to `http://localhost:5176` to access the home page.

---

## 6. Login

Use the seeded staff account if development seeding is enabled, or log in with an existing user.

---

## Development Notes

### Validation

Validation is implemented in multiple layers:

- **ViewModel validation** for UI feedback
- **Application validation** with FluentValidation
- **Domain validation** for core invariants

### Exception handling

The project uses:

- local validation handling in controllers where needed
- gloal exception handling for unexpected failures

### Logging

The project uses **Serilog** for request and file logging.

---

## Example Demo Flow

### Staff demo flow

1. Log in as staff
2. Open Staff Dashboard
3. Create a customer
4. Create a second customer or a second staff user
5. Create one or more bank accounts for a customer
6. View customer list
7. Search customer by AFM
8. Edit customer
9. View accounts list
10. Edit or delete an account

### Customer demo flow

1. Log in as customer
2. Open Customer Dashboard
3. Open My Accounts
4. Select an account
5. View account details
6. Check balance in EUR and USD
7. Transfer money between own accounts
8. Transfer money to a third-party account
9. Change password
