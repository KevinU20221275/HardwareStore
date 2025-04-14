# Hardware Store Sales Management Application

This is a sales management application created for a hardware store. The system allows you to manage products, perform sales, and visualize charts that show the best-selling products and monthly sales. The project is built using the **MVC architecture**.

It uses **Dapper** for data access, **AJAX** and **jQuery** for frontend interactions, **DataTables** to enhance table functionality, and **FluentValidation** for model validation. Stored procedures are used in the database to calculate total sales and update product stock.

## Technologies Used

- **Dapper**: Micro ORM for interacting with the database.
- **jQuery**: JavaScript library for DOM manipulation and AJAX.
- **Bootstrap**: CSS framework for UI components.
- **DataTables**: jQuery plugin to enhance HTML tables with pagination, search, and sort.
- **MailKit**: Library for sending emails.
- **MailTrap**: Service for testing email delivery.
- **Chart.js**: Library to render interactive charts.
- **Select2**: jQuery plugin to enhance select boxes.
- **FluentValidation**: Library for validating .NET models.
- **SQL**: Stored procedures to calculate sales totals and update product stock.

## Key Features

### Product Management:
- View available products with quantities and prices.
- Stock is automatically updated after each sale.

### Sales and Details:
- Products can be added to a sale, showing unit price and total by quantity.
- Stock validation to prevent sales of insufficient quantities.
- Ability to remove products from the sale before confirmation.

### Sales Charts:
- Most Sold Products chart.
- Monthly Sales chart.

### Form Validation:
- Validates that products in the sale have sufficient stock.
- Prevents duplicate products in the same sale.
- Validates customer and product selection.
- Uses **FluentValidation** for backend model validation.

### Email Integration:
- **MailKit** is used to send email receipts to customers.
- **MailTrap** is used for testing the email functionality.

### Enhanced Tables:
- Uses **DataTables** to display and interact with product and sales data:
  - Pagination
  - Search
  - Sorting

## Installation Instructions

### Clone the Repository:

```bash
git clone https://github.com/KevinU20221275/HardwareStore.git
cd HardwareStore
```

### Install Dependencies:
Make sure you have .NET Core and SQL Server installed.
```bash
dotnet restore
```

### Configure the Database:
- Execute the SQL script included in the project (`HardwareStore.sql`).  
  This script will create the database, tables, stored procedures, triggers, and insert some initial data.
- execute this query to generate a user and you can login in sistem
![Admin User](images/credentials.png)
![Login](images/login.png)
- Make sure your connection string is correctly configured in `appsettings.json` to match your SQL Server instance.

### Run the Application:
```bash
dotnet run
```

## Project Structure
- **Controllers**: Handle product and sales-related actions.
- **Models**: Contain product, sales, and customer classes.
- **Views**: Razor views with forms and tables to manage data.
- **Scripts**: JavaScript and jQuery files for frontend logic.

## Technical Details
### Frontend:
- **Bootstrap** for layout and UI.
- **jQuery**, **Select2**, and **DataTables** for enhanced interactions.
- **Chart.js** for rendering dynamic charts.
<img src="images/graphics.png" alt="dynamic charts" width="600"/>

### Backend:
- **Dapper** for executing SQL queries and managing data logic.
- **Stored procedures** for sales calculations and stock updates.
- **FluentValidation** for robust model validations.

### Email Functionality:
- **MailKit** for sending confirmation emails.
- **MailTrap** for development and testing of email delivery.

## Relevant jQuery Functions
### Select2 Integration:
```javascript
$('#customersSearch').select2({
    width: '70%',
    placeholder: "Select a Customer"
});
```

### Manage Sale Products:
```javascript
$('#add-row').click(function () { ... });
$('#details-table').on('click', '.remove-row', function () { ... });
```

### Calculate Total Sale:
```javascript
function calculateTotalSale() {
    var totalSale = 0;
    $('#details-table tbody tr').each(function () {
        var total = parseFloat($(this).find('.totalDetail').text()) || 0;
        totalSale += total;
    });
    $('#total').text(totalSale.toFixed(2));
}
```

### Form Validation:
```javascript
$('#saleForm').submit(function (event) {
    // Validate stock and duplicate products
    ...
});
```

## Contributions
If you'd like to contribute to this project, please follow these steps:

1. Fork the repository.
2. Create a new branch for your change.
3. Make your changes and submit a pull request.

## :information_source: Important Notes

### 🗣️ 1. User Interface Language

The application interface is in **Spanish**, as it was developed for a university project. This includes form labels, buttons, menus, and tables.

<img src="images/saleModule.png" alt="UI in Spanish" width="600"/>

> 📌 You can change the language by editing the Razor views in the `/Views` folder.

---

### 🍪 2. Cookie-Based Login

User authentication is handled using cookies. When a user logs in, a cookie is created to keep the session active.

> 🔐 Make sure to configure authentication correctly in `Program.cs` and review the cookie expiration settings to match your needs.

---

### 🔐 3. User Roles and Access Permissions

The project includes role-based access control implemented in the controllers. These roles restrict access to specific features and sections of the application:

- **Administrador | Administrator**: Full access.
- **Inventario | Inventory**: Access to product and stock management.
- **SoloLectura | ReadOnly**: View-only access.
- **Vendedor | Sales**: Access to create and view sales.
- **Marketing | Marketing**: Access to reports and analytics.

<img src="images/sidebarView.png" alt="Sidebar by Role" width="600"/>

> 🛠️ To use the system properly, you must adapt role names to your preferred language and check access logic in the sidebar, controllers, and authorization filters.
<img src="images/403.png" alt="unauthorized" width="600"/>
<img src="images/Roles.png" alt="roles in controllers" width="600"/>
---


### 📝 Recommendation

If you plan to modify this project for personal or production use, make sure to:

- Translate all interface text to your desired language.
- Configure user roles properly in your database.
- Review access permissions in the controllers.
