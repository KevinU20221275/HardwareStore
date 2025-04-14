CREATE DATABASE HardwareStoreDB
GO

USE HardwareStoreDB
GO


-- Category table
CREATE TABLE Category (
    Id INT PRIMARY KEY IDENTITY(1,1),
    CategoryName VARCHAR(50) NOT NULL,
    Description VARCHAR(250)
);
GO

-- Role table
CREATE TABLE Role (
	Id INT PRIMARY KEY IDENTITY (1,1)NOT NULL,
	Name VARCHAR(50)NOT NULL
);
GO

-- Customer table
CREATE TABLE Customer (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Email VARCHAR(50),
    Phone VARCHAR(20) NOT NULL,
    Address VARCHAR(250),
    City VARCHAR(50)
);
GO

-- Supplier table
CREATE TABLE Supplier (
    Id INT PRIMARY KEY IDENTITY(1,1),
    SupplierName VARCHAR(100) NOT NULL,
	Phone VARCHAR(20) NOT NULL,
	Email VARCHAR(50),
    Address VARCHAR(250),
    City VARCHAR(50)
);
GO

--  Employee table
CREATE TABLE Employee (
    Id INT PRIMARY KEY IDENTITY(1,1),
    FirstName VARCHAR(50) NOT NULL,
    LastName VARCHAR(50) NOT NULL,
    Username VARCHAR(50) NOT NULL UNIQUE,
    Password VARCHAR(250) NOT NULL,
    RoleID INT NOT NULL,
	Phone VARCHAR(20),
	Email VARCHAR(50),
	Salary DECIMAL(10,2),
    Address VARCHAR(255),
    City VARCHAR(50),
    CONSTRAINT FK_RolId FOREIGN KEY (RoleId) REFERENCES Role(Id)
);
GO

-- Product table
CREATE TABLE Product (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ProductName VARCHAR(100) NOT NULL,
    CategoryID INT NOT NULL,
    SupplierID INT NOT NULL,
    UnitPrice DECIMAL(10, 2) NOT NULL CHECK(UnitPrice > 0),
    UnitsInStock INT NOT NULL CHECK(UnitsInStock >= 0),
    CONSTRAINT FK_Product_Category FOREIGN KEY (CategoryID) REFERENCES Category(Id),
    CONSTRAINT Fk_Product_Supplier FOREIGN KEY (SupplierID) REFERENCES Supplier(Id)
);
GO

-- Sale table
CREATE TABLE Sale (
    Id INT PRIMARY KEY IDENTITY(1,1),
    CustomerID INT NOT NULL,
    EmployeeID INT NOT NULL,
    SaleDate DATETIME DEFAULT GETDATE(),
    Total DECIMAL(10, 2),
    CONSTRAINT FK_Sale_Customer FOREIGN KEY (CustomerID) REFERENCES Customer(Id),
    CONSTRAINT FK_Sale_Employee  FOREIGN KEY (EmployeeID) REFERENCES Employee(Id)
);
GO

-- SaleDetails table
CREATE TABLE SaleDetails (
    Id INT PRIMARY KEY IDENTITY(1,1),
    SaleID INT NOT NULL,
    ProductID INT NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(10, 2) NOT NULL,
	Total AS Quantity * UnitPrice,
    CONSTRAINT FK_SaleDetails_Sale FOREIGN KEY (SaleID) REFERENCES Sale(Id),
    CONSTRAINT FK_SaleDetails_Product FOREIGN KEY (ProductID) REFERENCES Product(Id)
);
GO

-- ============  STORED PROCEDURES ============== ---

-- ========  Role ===========

-- Stored Procedure to Get All Role
CREATE OR ALTER PROCEDURE dbo.spRole_GetAll
AS
BEGIN
	SELECT Id, Name FROM Role
END;
GO

-- Stored Procedure to Get an Role by ID
CREATE OR ALTER PROCEDURE dbo.spRole_GetById(@Id INT)
AS
BEGIN
	SELECT Id, Name
	FROM Role
    WHERE Id = @Id
END;
GO

-- Stored Procedure to Insert an Role
CREATE OR ALTER PROCEDURE dbo.spRole_Insert
(
	@Name VARCHAR(50)
)
AS
BEGIN
	INSERT INTO Role
	VALUES(@Name)
END;
GO

-- Stored Procedure to Update an Role
CREATE OR ALTER PROCEDURE dbo.spRole_Update
(
	@Name VARCHAR(50),
	@Id INT
)
AS
BEGIN
	UPDATE Role
	SET Name = @Name
    WHERE Id = @Id
END;
GO

-- Stored Procedure to Delete an Role
CREATE OR ALTER PROCEDURE dbo.spRole_Delete
(@Id int)
AS
BEGIN
	DELETE FROM Role WHERE Id = @Id
END;
GO

-- ========= Category ==========

-- Stored Procedure to Insert a Category
CREATE OR ALTER PROCEDURE spCategory_Insert
    @CategoryName VARCHAR(50),
    @Description VARCHAR(250)
AS
BEGIN
    INSERT INTO Category (CategoryName, Description)
    VALUES (@CategoryName, @Description);
END;
GO

-- Stored Procedure to Get All Categories
CREATE OR ALTER PROCEDURE spCategory_GetAll
AS
BEGIN
    SELECT Id, CategoryName, Description FROM Category;
END;
GO

-- Stored Procedure to Get a Category by ID
CREATE OR ALTER PROCEDURE spCategory_GetById
    @Id INT
AS
BEGIN
    SELECT Id, CategoryName, Description FROM Category WHERE Id = @Id;
END;
GO

-- Stored Procedure to Delete a Category
CREATE OR ALTER PROCEDURE spCategory_Delete
    @Id INT
AS
BEGIN
    DELETE FROM Category WHERE Id = @Id;
END;
GO

-- Stored Procedure to Update a Category
CREATE OR ALTER PROCEDURE spCategory_Update
    @Id INT,
    @CategoryName VARCHAR(50),
    @Description VARCHAR(250)
AS
BEGIN
    UPDATE Category
    SET 
        CategoryName = @CategoryName,
        Description = @Description
    WHERE Id = @Id;
END;
GO

-- sp to get category by name
CREATE OR ALTER PROCEDURE spCategory_GetByName
    @CategoryName VARCHAR(100)
AS
BEGIN
    SELECT * FROM Category
    WHERE CategoryName = @CategoryName;
END;
GO


-- ========= Customer ==============

-- Stored Procedure to Insert a Customer
CREATE OR ALTER PROCEDURE spCustomer_Insert
    @FirstName VARCHAR(50),
    @LastName VARCHAR(50),
    @Email VARCHAR(100),
    @Phone VARCHAR(20),
    @Address VARCHAR(255),
    @City VARCHAR(50)
AS
BEGIN
    INSERT INTO Customer (FirstName, LastName, Email, Phone, Address, City)
    VALUES (@FirstName, @LastName, @Email, @Phone, @Address, @City);
END;
GO

-- Stored Procedure to Insert a Customer from sales
CREATE OR ALTER PROCEDURE spCustomer_InsertFromSales
    @FirstName VARCHAR(50),
    @LastName VARCHAR(50),
    @Email VARCHAR(100),
    @Phone VARCHAR(20),
    @Address VARCHAR(255),
    @City VARCHAR(50),
	@CustomerID INT OUTPUT
AS
BEGIN
    INSERT INTO Customer (FirstName, LastName, Email, Phone, Address, City)
    VALUES (@FirstName, @LastName, @Email, @Phone, @Address, @City);

	SET @CustomerID = SCOPE_IDENTITY();
END;
GO

-- Stored Procedure to Get All Customer
CREATE OR ALTER PROCEDURE spCustomer_GetAll
AS
BEGIN
    SELECT Id, FirstName, LastName, Email, Phone, Address, City FROM Customer;
END;
GO

-- Stored Procedure to Get a Customer by ID
CREATE OR ALTER PROCEDURE spCustomer_GetById
    @Id INT
AS
BEGIN
    SELECT Id, FirstName, LastName, Email, Phone, Address, City  FROM Customer WHERE Id = @Id;
END;
GO

-- Stored Procedure to Delete a Customer
CREATE OR ALTER PROCEDURE spCustomer_Delete
    @Id INT
AS
BEGIN
    DELETE FROM Customer WHERE Id = @Id;
END;
GO

-- Stored Procedure to Update a Customer
CREATE OR ALTER PROCEDURE spCustomer_Update
    @Id INT,
    @FirstName VARCHAR(50),
    @LastName VARCHAR(50),
    @Email VARCHAR(100),
    @Phone VARCHAR(20),
    @Address VARCHAR(255),
    @City VARCHAR(50)
AS
BEGIN
    UPDATE Customer
    SET 
        FirstName = @FirstName,
        LastName = @LastName,
        Email = @Email,
        Phone = @Phone,
        Address = @Address,
        City = @City
    WHERE Id = @Id;
END;
GO


-- ========== Employee ==========

-- Stored Procedure to Insert an Employee
CREATE OR ALTER PROCEDURE spEmployee_Insert
    @FirstName VARCHAR(50),
    @LastName VARCHAR(50),
	@Username VARCHAR(50),
	@Password VARCHAR(250),
	@RoleID INT,
    @Phone VARCHAR(20),
    @Email VARCHAR(50),
    @Salary DECIMAL(10,2),
    @Address VARCHAR(255),
    @City VARCHAR(50)
AS
BEGIN
    INSERT INTO Employee (FirstName, LastName, Username, Password, RoleID, Phone, Email, Salary, Address, City)
    VALUES (@FirstName, @LastName, @Username, @Password, @RoleID, @Phone, @Email, @Salary, @Address, @City);
END;
GO

-- Stored Procedure to Get All Employees
CREATE OR ALTER PROCEDURE spEmployee_GetAll
AS
BEGIN
    SELECT A.Id, FirstName, LastName, Username, Password, RoleID, B.Name AS RoleName, Phone, Email, Salary, Address, City 
	FROM Employee AS A 
	INNER JOIN Role AS B
	ON A.RoleID = B.Id;
END;
GO

-- Stored Procedure to Get an Employee by ID
CREATE OR ALTER PROCEDURE spEmployee_GetById
    @Id INT
AS
BEGIN
    SELECT A.Id, FirstName, LastName, Username, Password, RoleID, B.Name AS RoleName, Phone, Email, Salary, Address, City 
	FROM Employee AS A
	INNER JOIN Role AS B
	ON A.RoleID = B.Id
	WHERE A.Id = @Id;
END;
GO

-- Stored Procedure to Delete an Employee
CREATE OR ALTER PROCEDURE spEmployee_Delete
    @Id INT
AS
BEGIN
    DELETE FROM Employee WHERE Id = @Id;
END;
GO

-- Stored Procedure to Update an Employee
CREATE OR ALTER PROCEDURE spEmployee_Update
	@Id INT,
    @FirstName VARCHAR(50),
    @LastName VARCHAR(50),
	@Username VARCHAR(50),
	@RoleID INT,
    @Phone VARCHAR(20),
    @Email VARCHAR(50),
    @Salary DECIMAL(10,2),
    @Address VARCHAR(255),
    @City VARCHAR(50)
AS
BEGIN
    UPDATE Employee
    SET 
        FirstName = @FirstName,
        LastName = @LastName,
		Username = @Username,
		RoleID = @RoleID,
		Phone = @Phone,
		Email = @Email,
		Salary = @Salary,
		Address = @Address,
		City = @City
	WHERE Id = @Id	
END
GO

-- Stored Procere to update Employee Password
CREATE OR ALTER PROCEDURE spEmployee_UpdatePassword
	@Id INT,
    @Password VARCHAR(250)
AS
BEGIN
    UPDATE Employee
    SET 
		Password = @Password
	WHERE Id = @Id	
END
GO

-- sp to get employee by username
CREATE OR ALTER PROCEDURE spEmployee_GetByUserName
	@Username VARCHAR(50)
AS
BEGIN
	SELECT A.Id, FirstName, LastName, Username, Password, RoleID, B.Name AS RoleName, Email, Phone, Address, City
	FROM Employee AS A
	INNER JOIN Role AS B
	ON A.RoleID = B.Id
	WHERE Username = @Username
END;
GO


-- ========= Supplier ==============

-- Stored Procedure to Insert a Supplier
CREATE OR ALTER PROCEDURE spSupplier_Insert
    @SupplierName VARCHAR(100),
    @Phone VARCHAR(20),
	@Email VARCHAR(50),
    @Address VARCHAR(250),
    @City VARCHAR(50)
AS
BEGIN
    INSERT INTO Supplier (SupplierName, Phone, Email,Address, City)
    VALUES (@SupplierName, @Phone, @Email, @Address, @City);
END;
GO

-- Stored Procedure to Get All Suppliers
CREATE OR ALTER PROCEDURE spSupplier_GetAll
AS
BEGIN
    SELECT Id, SupplierName, Phone, Email, Address, City FROM Supplier;
END;
GO

-- Stored Procedure to Get a Supplier by ID
CREATE OR ALTER PROCEDURE spSupplier_GetById
    @Id INT
AS
BEGIN
    SELECT Id, SupplierName, Phone, Email, Address, City FROM Supplier WHERE Id = @Id;
END;
GO

-- Stored Procedure to Delete a Supplier
CREATE OR ALTER PROCEDURE spSupplier_Delete
    @Id INT
AS
BEGIN
    DELETE FROM Supplier WHERE Id = @Id;
END;
GO

-- Stored Procedure to Update a Supplier
CREATE OR ALTER PROCEDURE spSupplier_Update
    @Id INT,
    @SupplierName VARCHAR(100),
    @Phone VARCHAR(20),
	@Email VARCHAR(50),
    @Address VARCHAR(250),
    @City VARCHAR(50)
AS
BEGIN
    UPDATE Supplier
    SET 
        SupplierName = @SupplierName,
        Phone = @Phone,
		Email = @Email,
        Address = @Address,
        City = @City
    WHERE Id = @Id;
END;
GO


-- ========  Product ===========

-- Stored Procedure to Insert a Product
CREATE OR ALTER PROCEDURE spProduct_Insert
    @ProductName VARCHAR(100),
    @CategoryID INT,
    @SupplierID INT,
    @UnitPrice DECIMAL(10, 2),
    @UnitsInStock INT
AS
BEGIN
    INSERT INTO Product (ProductName, CategoryID, SupplierID, UnitPrice, UnitsInStock)
    VALUES (@ProductName, @CategoryID, @SupplierID, @UnitPrice, @UnitsInStock);
END;
GO

-- Stored Procedure to Get All Products
CREATE OR ALTER PROCEDURE spProduct_GetAll
AS
BEGIN
    SELECT A.Id, ProductName, UnitPrice, UnitsInStock, CategoryID, SupplierID, CategoryName, SupplierName FROM Product AS A
	INNER JOIN Category AS B ON A.CategoryID = B.Id
	INNER JOIN Supplier AS C ON A.SupplierID = C.Id;
END;
GO

-- Stored Procedure to Get a Product by ID
CREATE OR ALTER PROCEDURE spProduct_GetById
    @Id INT
AS
BEGIN
    SELECT A.Id, ProductName, UnitPrice, UnitsInStock, CategoryID, SupplierID, CategoryName, SupplierName FROM Product AS A
	INNER JOIN Category AS B ON A.CategoryID = B.Id
	INNER JOIN Supplier AS C ON A.SupplierID = C.Id
	WHERE A.Id = @Id;
END;
GO

-- Stored Procedure to Delete a Product
CREATE OR ALTER PROCEDURE spProduct_Delete
    @Id INT
AS
BEGIN
    DELETE FROM Product WHERE Id = @Id;
END;
GO


-- Stored Procedure to Update a Product
CREATE OR ALTER PROCEDURE spProduct_Update
    @Id INT,
    @ProductName VARCHAR(100),
    @CategoryID INT,
    @SupplierID INT,
    @UnitPrice DECIMAL(10, 2),
    @UnitsInStock INT
AS
BEGIN
    UPDATE Product
    SET 
        ProductName = @ProductName,
        CategoryID = @CategoryID,
        SupplierID = @SupplierID,
        UnitPrice = @UnitPrice,
        UnitsInStock = @UnitsInStock
    WHERE Id = @Id;
END;
GO

-- sp to get product by name
CREATE OR ALTER PROCEDURE spProduct_GetByName
    @ProductName VARCHAR(100)
AS
BEGIN
    SELECT * FROM Product
    WHERE ProductName = @ProductName;
END;
GO



-- ========== Sale ==============
-- Stored Procedure to Insert a Sale
CREATE OR ALTER PROCEDURE spSale_Insert
    @CustomerID INT,
    @EmployeeID INT,
    @SaleID INT OUTPUT
AS
BEGIN
    INSERT INTO Sale (CustomerID, EmployeeID)
    VALUES (@CustomerID, @EmployeeID);

    SET @SaleID = SCOPE_IDENTITY();
END
GO

-- Stored Procedure to Get All Sales
CREATE OR ALTER PROCEDURE spSale_GetAll
AS
BEGIN
    SELECT A.Id, CustomerID, B.FirstName + ' ' + B.LastName AS CustomerName, EmployeeID, C.FirstName + ' ' + C.LastName AS EmployeeName, SaleDate, Total 
	FROM Sale AS A
	INNER JOIN Customer AS B ON A.CustomerID = B.Id
	INNER JOIN Employee AS C ON A.EmployeeID = C.Id
	ORDER BY A.Id DESC;
END;
GO

-- Stored Procedure to Get a Sale by ID
CREATE OR ALTER PROCEDURE spSale_GetById
    @Id INT
AS
BEGIN
    SELECT A.Id, CustomerID, B.FirstName + ' ' + B.LastName AS CustomerName, B.Address + ', ' + B.City AS ClientAddress,
    B.Email AS ClientEmail,
	EmployeeID, C.FirstName + ' ' + C.LastName AS EmployeeName, SaleDate, Total 
	FROM Sale AS A
	INNER JOIN Customer AS B ON A.CustomerID = B.Id
	INNER JOIN Employee AS C ON A.EmployeeID = C.Id
	WHERE A.Id = @Id;
END;
GO

-- ============ Sale Details ==============
-- Stored Procedure to Insert a Sale Detail
CREATE OR ALTER PROCEDURE spSalesDetail_Insert
    @SaleId INT,
    @ProductID INT,
    @Quantity INT
AS
BEGIN

	DECLARE @UnitPrice DECIMAL(10,2);

    -- Obtener el precio del producto de la tabla Producto
    SELECT @UnitPrice = UnitPrice
    FROM Product
    WHERE Id = @ProductId;

    INSERT INTO SaleDetails (SaleID, ProductID, Quantity, UnitPrice)
    VALUES (@SaleId, @ProductID, @Quantity, @UnitPrice);
END;
GO

-- Stored Procedure to Get a Sale Detail by ID
CREATE OR ALTER PROCEDURE spSalesDetail_GetById
    @Id INT
AS
BEGIN
    SELECT A.Id, Quantity, A.UnitPrice, ProductName,A.Total AS DetailTotal
	FROM SaleDetails AS A
	INNER JOIN Product AS B ON A.ProductID = B.Id 
	INNER JOIN Sale AS C ON A.SaleID = C.Id
	WHERE A.SaleID = @Id
END;
GO

-- ========================== TRIGGERS ============================
-- trigger en la tabla SaleDetails
CREATE OR ALTER TRIGGER trg_UpdateStockOnSale
ON SaleDetails
AFTER INSERT
AS
BEGIN
    DECLARE @ProductID INT;
    DECLARE @Quantity INT;

    SELECT @ProductID = i.ProductID, @Quantity = i.Quantity
    FROM inserted i;

    -- Verificar si hay suficiente stock
    IF EXISTS (SELECT 1 FROM Product WHERE Id = @ProductID AND UnitsInStock >= @Quantity)
    BEGIN
        -- Actualizar el stock del producto
        UPDATE Product
        SET UnitsInStock = UnitsInStock - @Quantity
        WHERE Id = @ProductID;
    END
    ELSE
    BEGIN
        -- Si no mostrar error
        ROLLBACK;
        RAISERROR ('La cantidad solicitada excede el stock disponible.', 16, 1);
    END
END;
GO

-- trigger to update Sale Total
CREATE OR ALTER TRIGGER trg_UpdateSaleTotal
ON SaleDetails
AFTER INSERT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM inserted)
    BEGIN
        UPDATE Sale
        SET Total = (
            SELECT SUM(Quantity * UnitPrice)
            FROM SaleDetails
            WHERE SaleID = inserted.SaleID
        )
        FROM Sale
        INNER JOIN inserted ON Sale.Id = inserted.SaleID;
    END
END;
GO



-- sp for viewbag lists

-- stored procedure to get product data for sale
CREATE OR ALTER PROCEDURE spProduct_GetInfoList
AS
BEGIN
    SELECT Id, ProductName,UnitPrice, UnitsInStock FROM Product;
END;
GO

CREATE OR ALTER PROCEDURE spProduct_GetInfoForSaleById
    @Id INT
AS
BEGIN
    SELECT Id, ProductName,UnitPrice, UnitsInStock FROM Product 
	WHERE Id = @Id;
END;
GO

CREATE OR ALTER PROCEDURE spCustomer_GetInfoList
AS
BEGIN
	SELECT Id, FirstName + ' ' + LastName AS CustomerName FROM Customer
END
GO

CREATE OR ALTER PROCEDURE spEmployee_GetInfoList
AS
BEGIN
	SELECT Id, FirstName + ' ' + LastName AS EmployeeName FROM Employee
END
GO

CREATE OR ALTER PROCEDURE spSupplier_GetInfoList
AS
BEGIN
	SELECT Id, SupplierName FROM Supplier
END
GO

CREATE OR ALTER PROCEDURE spCategory_GetInfoList
AS
BEGIN
	SELECT Id, CategoryName FROM Category
END
GO

CREATE OR ALTER PROCEDURE spRole_GetInfoList
AS
BEGIN
	SELECT Id, Name FROM Role
END
GO


-- sp for graphics 
-- sale graph
GO
CREATE OR ALTER PROCEDURE spSale_InfoForGraph
AS
BEGIN
	DECLARE @fecha_maxima DATETIME;
    DECLARE @fecha_minima DATETIME;

    SET @fecha_maxima = (SELECT MAX(SaleDate) FROM Sale);
    SET @fecha_minima = DATEADD(MONTH, -3, DATEADD(DAY, 1 - DAY(@fecha_maxima), @fecha_maxima));

    SELECT 
        YEAR(SaleDate) AS [Year], 
        MONTH(SaleDate) AS [Month_Number], 
        DATENAME(MONTH, SaleDate) AS [MonthName], 
        COUNT(*) AS [Quantity]
    FROM Sale
    WHERE SaleDate BETWEEN @fecha_minima AND @fecha_maxima
    GROUP BY YEAR(SaleDate), MONTH(SaleDate), DATENAME(MONTH, SaleDate)
    ORDER BY YEAR(SaleDate), MONTH(SaleDate);
END
GO

-- producst graph
CREATE OR ALTER PROCEDURE spProduct_InfoForGraph
AS
BEGIN
    DECLARE @fecha_maxima DATETIME;
    DECLARE @fecha_minima DATETIME;

    SET @fecha_maxima = (SELECT MAX(SaleDate) FROM Sale);
    SET @fecha_minima = DATEADD(MONTH, -2, DATEADD(DAY, 1 - DAY(@fecha_maxima), @fecha_maxima));

    SELECT TOP 4 
        c.ProductName,
        SUM(a.Quantity) AS QuantitySold
    FROM SaleDetails a
    INNER JOIN Sale b ON a.SaleID = b.Id
    INNER JOIN Product c ON a.ProductID = c.Id
    WHERE b.SaleDate BETWEEN @fecha_minima AND @fecha_maxima
    GROUP BY c.ProductName
    ORDER BY QuantitySold DESC;
END
GO

-- /* =================== NOTA: EXECUTE ALL QUERYS IN ORDEN ==================  */

-- role
INSERT INTO Role (Name) VALUES ('Administrador');
GO

--================ DEFAULT DATA ======================--
-- CTEGORIES 
EXEC spCategory_Insert 'Demoledores', 'Herramientas para demoler estructuras y materiales';
EXEC spCategory_Insert 'Sierras manuales', 'Sierras operadas manualmente para cortar diversos materiales';
EXEC spCategory_Insert 'Tenazas', 'Herramientas para sujetar y cortar alambres y otros materiales';
EXEC spCategory_Insert 'Destornilladores', 'Herramientas para atornillar y desatornillar tornillos';
EXEC spCategory_Insert 'Llaves', 'Herramientas para ajustar o aflojar tuercas y pernos';
EXEC spCategory_Insert 'Remachadoras', 'Herramientas para colocar remaches en diversos materiales';
EXEC spCategory_Insert 'Cintas Metricas', 'Instrumentos de medici�n flexibles y port�tiles';
EXEC spCategory_Insert 'Niveles', 'Herramientas para verificar la horizontalidad o verticalidad';
EXEC spCategory_Insert 'Medidores', 'Instrumentos para medir diferentes par�metros y dimensiones';
EXEC spCategory_Insert 'Almadana', 'Martillos grandes para trabajos pesados y de demolici�n';
EXEC spCategory_Insert 'Martillos', 'Herramientas manuales para golpear y clavar';
EXEC spCategory_Insert 'Cinceles', 'Herramientas para cortar o esculpir materiales s�lidos';
EXEC spCategory_Insert 'Carretillas', 'Veh�culos manuales para transportar materiales';
EXEC spCategory_Insert 'Palas', 'Herramientas para cavar y mover tierra u otros materiales';
EXEC spCategory_Insert 'Sierras Electricas', 'Sierras el�ctricas para cortar materiales con mayor eficiencia';
EXEC spCategory_Insert 'Cepillos Electricos', 'Herramientas el�ctricas para alisar y dar forma a la madera';
EXEC spCategory_Insert 'Lijadoras electricas', 'Herramientas el�ctricas para lijar superficies';
EXEC spCategory_Insert 'Taladros', 'Herramientas el�ctricas para perforar agujeros en diversos materiales';
EXEC spCategory_Insert 'Rotomartillos', 'Herramientas el�ctricas para perforar y demoler materiales duros';
EXEC spCategory_Insert 'Esmeriles', 'Herramientas para afilar y cortar materiales con precisi�n';


-- SUPPLIERS
EXEC spSupplier_Insert 'INGCO', '2222-3333', 'contacto@ingco.com', 'Calle Principal, San Salvador', 'San Salvador';
EXEC spSupplier_Insert 'DEWALT', '2233-4444', 'contacto@dewalt.com', 'Avenida Central, Santa Ana', 'Santa Ana';
EXEC spSupplier_Insert 'BLACK & DECKER', '2244-5555', 'contacto@blackdecker.com', 'Boulevard El Hip�dromo, San Miguel', 'San Miguel';
EXEC spSupplier_Insert 'IRWIN', '2255-6666', 'contacto@irwin.com', 'Calle El Progreso, Soyapango', 'Soyapango';
EXEC spSupplier_Insert 'Stanley', '2266-7777', 'contacto@stanley.com', 'Avenida La Paz, La Libertad', 'La Libertad';
EXEC spSupplier_Insert 'IMACASA', '2277-8888', 'contacto@imacasa.com', 'Calle Los Heroes, San Vicente', 'San Vicente';
EXEC spSupplier_Insert 'BOSCH', '2288-9999', 'contacto@bosch.com', 'Calle Nueva, San Salvador', 'San Salvador';


-- =============== NEED THIS DATA FOR VIEW GRAPHICS ================================== --
-- PRODUCTS
EXEC spProduct_Insert 'Martillo de Garra',11,7, 12.99, 50;
EXEC spProduct_Insert 'Destornillador Philips',4,7, 3.49, 100;
EXEC spProduct_Insert 'Llave Inglesa', 5,7,7.99, 75;
EXEC spProduct_Insert 'Taladro Inal�mbrico',18,7, 49.99, 30;
EXEC spProduct_Insert 'Lijadora Orbital',17,7, 29.99, 40;

-- Customer
EXEC spCustomer_Insert 'Juan Armando', 'Perez','juan@gmail.com','7272-0000','Barrio el Centro', 'San Salvador';

-- Employee credentials username: Admin password: 12345678
EXEC spEmployee_Insert 'Jhon', 'Doe', 'Admin', 'AQAAAAEAACcQAAAAEGYU6ls8Xr8BbNzT2pA6pDBdOogLh1AWaDfx3hSXS2C1fkd3JMI4LfnOIZYJPazoRg==', 1,'7276-0000', 'jhon@gmail.com',400,'Calle a la Plaza', 'San Miguel';

-- sales
INSERT INTO Sale (CustomerID, EmployeeID, SaleDate) VALUES (1,1,'12-25-2024')
INSERT INTO Sale (CustomerID, EmployeeID, SaleDate) VALUES (1,1,'01-22-2025')
INSERT INTO Sale (CustomerID, EmployeeID, SaleDate) VALUES (1,1,'03-24-2025')
INSERT INTO Sale (CustomerID, EmployeeID, SaleDate) VALUES (1,1,'03-25-2025')
INSERT INTO Sale (CustomerID, EmployeeID, SaleDate) VALUES (1,1,'04-10-2025')


-- sale details
EXEC spSalesDetail_Insert 1,1,3
EXEC spSalesDetail_Insert 1,4,2
EXEC spSalesDetail_Insert 2,2,1
EXEC spSalesDetail_Insert 3,3,2
EXEC spSalesDetail_Insert 4,4,4
EXEC spSalesDetail_Insert 4,5,2
EXEC spSalesDetail_Insert 5,5,1
go






