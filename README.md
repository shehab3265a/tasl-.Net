# Advanced Entity Framework Core

## Project Overview

This project demonstrates advanced Entity Framework Core features using ASP.NET Core Web API and SQL Server.

## Technologies Used

- C#
- ASP.NET Core Web API
- Entity Framework Core 8
- SQL Server LocalDB
- Entity Framework Core Migrations

## Features

### 1. Product Listing and Pagination

- Retrieves products through `GET /api/products`.
- Uses `AsNoTracking()` for read-only queries.
- Uses `Select()` to return only the required fields.
- Implements pagination using `Skip()` and `Take()`.
- Returns the category name with each product without executing a separate query for every product.

Example request:

`GET /api/products?page=1&pageSize=10`

### 2. Category Name Without N+1 Queries

The product query projects the category name using `Select()`:

`CategoryName = p.Category.Name`

Entity Framework Core translates this projection into SQL, allowing the product and category data to be retrieved without a separate query for each product.

### 3. Database Index

An index is configured on the `Product.Name` property.

```csharp
modelBuilder.Entity<Product>()
    .HasIndex(p => p.Name);
```

**Reason for choosing this field:** Product names are a natural field for searching and filtering products. An index can improve queries that search by name, depending on the query pattern and database execution plan. Indexes also require additional storage and can add overhead to insert and update operations.

### 4. Soft Delete

Products have an `IsDeleted` property.

The global query filter excludes deleted products:

```csharp
modelBuilder.Entity<Product>()
    .HasQueryFilter(p => !p.IsDeleted);
```

Instead of deleting a product record permanently, the delete endpoint sets `IsDeleted` to `true`. Normal product queries then exclude that record.

### 5. Controlled Product Updates

The `PUT /api/products/{id}` endpoint updates only the fields accepted by `UpdateProductDto`.

The endpoint retrieves the tracked product, updates the permitted properties, and saves the changes through Entity Framework Core.

### 6. Optimistic Concurrency

The `RowVersion` property is configured as a SQL Server rowversion concurrency token:

```csharp
modelBuilder.Entity<Product>()
    .Property(p => p.RowVersion)
    .IsRowVersion();
```

The update endpoint uses the client's original rowversion value. If the record has changed since that version was read, Entity Framework Core can throw `DbUpdateConcurrencyException`, which the API handles by returning `409 Conflict`.

## Database Configuration

The project uses SQL Server LocalDB. The connection string is configured in `appsettings.json`.

The database name is `AdvancedEFCoreDb`.

## How to Run

1. Open the project folder in Visual Studio Code.
2. Ensure the .NET 8 SDK and SQL Server LocalDB are installed.
3. Restore the NuGet packages:

   `dotnet restore`

4. Apply the database migrations:

   `dotnet ef database update`

5. Run the API:

   `dotnet run`

6. Open the API endpoint using the local address and port shown in the terminal:

   `/api/products?page=1&pageSize=10`

## API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| GET | `/api/products?page=1&pageSize=10` | Retrieves products with pagination |
| PUT | `/api/products/{id}` | Updates permitted product fields |
| DELETE | `/api/products/{id}` | Soft-deletes a product |

## Testing

The project was tested for product listing, pagination, category name projection, product updates, concurrency handling, and soft deletion.

## Author

Advanced EF Core Practice Project