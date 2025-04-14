go
use [HospitalApp]
go
-------------------------------------
-- Clean up existing tables (if any)
-------------------------------------
IF OBJECT_ID('dbo.Patients', 'U') IS NOT NULL
    DROP TABLE dbo.Patients;

IF OBJECT_ID('dbo.Doctors', 'U') IS NOT NULL
    DROP TABLE dbo.Doctors;

IF OBJECT_ID('dbo.Admins', 'U') IS NOT NULL
    DROP TABLE dbo.Admins;

IF OBJECT_ID('dbo.Users', 'U') IS NOT NULL
    DROP TABLE dbo.Users;

IF OBJECT_ID('dbo.Logs', 'U') IS NOT NULL
   DROP TABLE dbo.Logs

IF OBJECT_ID('dbo.Departments', 'U') IS NOT NULL
    DROP TABLE dbo.Departments;


-------------------------------------
-- Create Departments
-------------------------------------
CREATE TABLE Departments (
    DepartmentId INT IDENTITY(1,1) PRIMARY KEY,
    DepartmentName VARCHAR(100) NOT NULL
);


-------------------------------------
-- Create Users
-------------------------------------
CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY, -- Auto-incrementing primary key
    Username NVARCHAR(50) NOT NULL UNIQUE, -- Unique username
    Password NVARCHAR(255) NOT NULL, -- Store hashed passwords, so length is higher
    Mail NVARCHAR(100) NOT NULL UNIQUE, -- Unique email
    Role NVARCHAR(50) NOT NULL DEFAULT 'User', -- Default role
    Name NVARCHAR(100) NOT NULL,
    BirthDate DATE NOT NULL, -- 'DateOnly' maps to DATE in SQL
    Cnp NVARCHAR(20) NOT NULL UNIQUE CHECK(LEN(Cnp) = 13), -- Unique identifier
    Address NVARCHAR(255) NULL,
    PhoneNumber NVARCHAR(20) NULL CHECK(LEN(PhoneNumber) = 10 OR LEN(PhoneNumber) = 10),
    RegistrationDate DATETIME NOT NULL DEFAULT GETDATE() -- Automatically set on insert

);
ALTER TABLE Users ADD AvatarUrl NVARCHAR(255) NULL;

-------------------------------------
-- Create Doctors
-------------------------------------
CREATE TABLE Doctors (
    DoctorId INT IDENTITY(1,1) PRIMARY KEY,
	UserId INT,
    DepartmentId INT NOT NULL,
	DoctorRating FLOAT NOT NULL DEFAULT 0.0 CHECK(DoctorRating BETWEEN 0.0 AND 5.0),
    LicenseNumber NVARCHAR(50) NOT NULL,
    CONSTRAINT FK_Doctors_Departments
        FOREIGN KEY (DepartmentId) REFERENCES Departments(DepartmentId),
	CONSTRAINT FK_Doctors_Users
		FOREIGN KEY (UserId) REFERENCES Users(UserId)
);

ALTER TABLE Doctors ADD CareerInfo NVARCHAR(MAX) NULL;



-------------------------------------
-- Create Patients
-------------------------------------
CREATE TABLE Patients (
    UserId INT NOT NULL, -- Foreign key reference to Users table
    PatientId INT IDENTITY(1,1) PRIMARY KEY, -- Auto-increment primary key
    BloodType NVARCHAR(3) NOT NULL CHECK (BloodType IN ('A+', 'A-', 'B+', 'B-', 'AB+', 'AB-', 'O+', 'O-')), -- Enum-like constraint
    EmergencyContact NVARCHAR(20) NOT NULL CHECK(LEN(EmergencyContact) = 10), -- Phone number for emergency contact
    Allergies NVARCHAR(255) NULL, -- Can be NULL if no allergies
    Weight FLOAT NOT NULL CHECK (Weight > 0), -- Prevent invalid weight values
    Height INT NOT NULL CHECK (Height > 0), -- Height in cm, must be positive

    CONSTRAINT FK_Patients_Users FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE
);


-------------------------------------
-- Create Admins
-------------------------------------
CREATE TABLE Admins (
	AdminId INT PRIMARY KEY IDENTITY(1, 1),
	UserId INT FOREIGN KEY REFERENCES Users(UserId) ON DELETE CASCADE,
);


-------------------------------------
-- Create Logs
-------------------------------------
CREATE TABLE Logs (
    LogId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NULL, 
    ActionType NVARCHAR(50) NOT NULL CHECK (ActionType IN ('LOGIN', 'LOGOUT', 'UPDATE_PROFILE', 'CHANGE_PASSWORD', 'DELETE_ACCOUNT', 'CREATE_ACCOUNT')),  
    Timestamp DATETIME NOT NULL DEFAULT GETDATE(),  -- Auto-set when action occurs

    CONSTRAINT FK_Logs_Users FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE SET NULL
);



-------------------------------------
-- Insert Departments
-------------------------------------
INSERT INTO Departments (DepartmentName)
VALUES
    ('Cardiology'),      -- DepartmentId = 1
    ('Neurology'),       -- DepartmentId = 2
    ('Pediatrics');      -- DepartmentId = 3
	INSERT INTO Departments (DepartmentName)
VALUES
    ('Ophthalmology'),   -- DepartmentId = 4
    ('Gastroenterology'),-- DepartmentId = 5
    ('Orthopedics'),     -- DepartmentId = 6
    ('Dermatology');     -- DepartmentId = 7


-------------------------------------
-- Insert Users (Doctors and Patients)
-------------------------------------
INSERT INTO Users (Username, Password, Mail, Role, Name, BirthDate, Cnp, Address, PhoneNumber)
VALUES 
('john_doe', 'hashed_password_1', 'john@example.com', 'Doctor', 'John Doe', '1990-05-15', '1234567890123', '123 Main St', '1234567890'),
('jane_doe', 'hashed_password_2', 'jane@example.com', 'Doctor', 'Jane Doe', '1995-08-20', '2345678901234', '456 Elm St', '3216540987'),
('alice_smith', 'hashed_password_3', 'alice@example.com', 'Doctor', 'Alice Smith', '1988-12-10', '3456789012345', '789 Oak St', '9871234567'),
('bob_johnson', 'hashed_password_4', 'bob@example.com', 'Doctor', 'Bob Johnson', '1985-07-25', '4567890123456', '147 Pine St', '6549873210');

INSERT INTO Users (Username, Password, Mail, Role, Name, BirthDate, Cnp, Address, PhoneNumber)
VALUES 
('michael_brown', 'hashed_password_5', 'michael@example.com', 'Doctor', 'Michael Brown', '1982-11-05', '5678944434567', '159 Maple St', '7894561230'),
('sarah_wilson', 'hashed_password_6', 'sarahh@example.com', 'Doctor', 'Sarah Wilson', '1991-03-14', '6744412345678', '753 Birch St', '8529637410'),
('david_martinez', 'hashed_password_7', 'david@example.com', 'Doctor', 'David Martinez', '1987-09-21', '7555523456789', '369 Cedar St', '9632581470'),
('emily_davis', 'hashed_password_8', 'emily@example.com', 'Doctor', 'Emily Davis', '1994-06-30', '8901234544490', '951 Redwood St', '7418529630');

INSERT INTO Users (Username, Password, Mail, Role, Name, BirthDate, Cnp, Address, PhoneNumber)
VALUES 
('jane_do', 'hashed_password_5', 'janedo@example.com', 'Patient', 'Jane Doe', '1992-03-10', '1334567890123', '123 Main St', '1234567890'),
('mike_davis', 'hashed_password_6', 'mike@example.com', 'Patient', 'Mike Davis', '1988-07-15', '2445678901234', '456 Oak St', '3216540987'),
('sarah_miller', 'hashed_password_7', 'sarah@example.com', 'Patient', 'Sarah Miller', '1995-12-05', '3756789012345', '789 Pine St', '9871234567');

INSERT INTO Users (Username, Password, Mail, Role, Name, BirthDate, Cnp, Address, PhoneNumber)
VALUES 
('emma_smith', 'hashed_password_99', 'emma.smith@example.com', 'Admin', 'Emma Smith', '1992-04-15', '5678901234567', '456 Maple Rd', '3216549870'),
('david_jones', 'hashed_password_23', 'david.jones@example.com', 'Admin', 'David Jones', '1985-09-30', '6789012345678', '890 Birch Ln', '2109876543');


-------------------------------------
-- Insert Admins
-------------------------------------
INSERT INTO Admins (UserId)
VALUES
(8),  --AdminId = 1
(9);  --AdminId = 2



-------------------------------------
-- Insert Doctors
-------------------------------------
INSERT INTO Doctors (UserId, DepartmentId, LicenseNumber)
VALUES
    (1, 1, '696969'),   -- DoctorId = 1, Dept = Cardiology
    (2, 1, '3222'),  -- DoctorId = 2, Dept = Cardiology
    (3, 2, '231231'), -- DoctorId = 3, Dept = Neurology
    (4, 3, '124211');   -- DoctorId = 4, Dept = Pediatrics


-------------------------------------
-- Insert Patients
-------------------------------------
INSERT INTO Patients (UserId, BloodType, EmergencyContact, Allergies, Weight, Height)
VALUES 
(5, 'A+', '1112223333', 'Peanuts', 60.5, 165),  -- Jane Doe
(6, 'O-', '2223334444', 'None', 80.0, 175),     -- Mike Davis
(7, 'B+', '3334445555', 'Pollen', 70.2, 170);   -- Sarah Miller

-------------------------------------
-- Insert Logs
-------------------------------------
INSERT INTO Logs (UserId, ActionType)
VALUES 
(3, 'LOGIN'),
(5, 'UPDATE_PROFILE'),
(7, 'LOGOUT');

-------------------------------------
-- Verify Data
-------------------------------------
-- All Departments
SELECT * FROM Departments;

-- All Users
SELECT * FROM Users;

-- All Doctors
SELECT * FROM Doctors;

-- All Patients
SELECT * FROM Patients;

-- All Admins
SELECT * FROM Admins;

SELECT * FROM Logs;

------------------------------------
