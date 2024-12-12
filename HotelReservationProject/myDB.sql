
create table Users (Username int, Userpass VARCHAR (50));
INSERT INTO Users (Username, Userpass) VALUES ('123','123');

DROP TABLE Users;



CREATE TABLE Customers (
    CustomerID INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Gender NVARCHAR(10),
    Age INT,
    Phone NVARCHAR(15),
    Address NVARCHAR(255),
    CheckIn DATETIME NOT NULL,
    CheckOut DATETIME NULL,
    RoomID INT
);



CREATE TABLE Rooms (
    RoomID INT IDENTITY(1,1) PRIMARY KEY,
    RoomNumber NVARCHAR(50) NOT NULL,
    RoomType NVARCHAR(100) NOT NULL,
    Bed NVARCHAR(50) NOT NULL,
    Price DECIMAL(18, 2) NOT NULL,
    Status NVARCHAR(50) NOT NULL
);





SELECT * FROM Customers;

SELECT * FROM Rooms;







DROP TABLE Rooms;

DROP TABLE Customers;


INSERT INTO Rooms (RoomNumber, RoomType, Bed, Price, Status)
VALUES 
('101', 'AC', 'Single', 120.00, 'Available'),
('102', 'NON', 'Double', 200.00, 'Occupied'),
('103', 'Suite', 'King', 300.00, 'Available');


SELECT Customers.CustomerID, Customers.FullName, Customers.RoomID, Rooms.RoomNumber
FROM Customers
INNER JOIN Rooms ON Customers.RoomID = Rooms.RoomID
WHERE Customers.CheckOut IS NULL;

