use [HospitalDB]
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
    Cnp NVARCHAR(20) NOT NULL UNIQUE, -- Unique identifier
    Address NVARCHAR(255) NULL,
    PhoneNumber NVARCHAR(20) NULL,
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
	DoctorRating FLOAT NULL DEFAULT 0.0,
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
    EmergencyContact NVARCHAR(20) NOT NULL, -- Phone number for emergency contact
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
    ActionType NVARCHAR(50) NOT NULL CHECK (ActionType IN ('LOGIN', 'LOGOUT', 'UPDATE_PROFILE', 'CHANGE_PASSWORD', 'DELETE_ACCOUNT')),  
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

-------------------------------------
-- Insert Users (Doctors and Patients)
-------------------------------------
INSERT INTO Users (Username, Password, Mail, Role, Name, BirthDate, Cnp, Address, PhoneNumber)
VALUES 
('john_doe', 'hashed_password_1', 'john@example.com', 'Doctor', 'John Doe', '1990-05-15', '1234567890123', '123 Main St', '123-456-7890'),
('jane_doe', 'hashed_password_2', 'jane@example.com', 'Doctor', 'Jane Doe', '1995-08-20', '2345678901234', '456 Elm St', '321-654-0987'),
('alice_smith', 'hashed_password_3', 'alice@example.com', 'Doctor', 'Alice Smith', '1988-12-10', '3456789012345', '789 Oak St', '987-123-4567'),
('bob_johnson', 'hashed_password_4', 'bob@example.com', 'Doctor', 'Bob Johnson', '1985-07-25', '4567890123456', '147 Pine St', '654-987-3210');

INSERT INTO Users (Username, Password, Mail, Role, Name, BirthDate, Cnp, Address, PhoneNumber)
VALUES 
('jane_do', 'hashed_password_5', 'janedo@example.com', 'Patient', 'Jane Doe', '1992-03-10', '1334567890123', '123 Main St', '123-456-7890'),
('mike_davis', 'hashed_password_6', 'mike@example.com', 'Patient', 'Mike Davis', '1988-07-15', '2445678901234', '456 Oak St', '321-654-0987'),
('sarah_miller', 'hashed_password_7', 'sarah@example.com', 'Patient', 'Sarah Miller', '1995-12-05', '3756789012345', '789 Pine St', '987-123-4567');

INSERT INTO Users (Username, Password, Mail, Role, Name, BirthDate, Cnp, Address, PhoneNumber)
VALUES 
('emma_smith', 'hashed_password_99', 'emma.smith@example.com', 'Admin', 'Emma Smith', '1992-04-15', '5678901234567', '456 Maple Rd', '321-654-9870'),
('david_jones', 'hashed_password_23', 'david.jones@example.com', 'Admin', 'David Jones', '1985-09-30', '6789012345678', '890 Birch Ln', '210-987-6543');


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
(5, 'A+', '111-222-3333', 'Peanuts', 60.5, 165),  -- Jane Doe
(6, 'O-', '222-333-4444', 'None', 80.0, 175),     -- Mike Davis
(7, 'B+', '333-444-5555', 'Pollen', 70.2, 170);   -- Sarah Miller

-- Insert new users for the doctors
INSERT INTO Users (Username, Password, Mail, Role, Name, BirthDate, Cnp, Address, PhoneNumber)
VALUES 
('michael_brown', 'hashed_password_5', 'michael.brown@example.com', 'Doctor', 'Michael Brown', '1980-11-22', '5678901234567', '123 Cedar St', '123-456-7891'),
('linda_white', 'hashed_password_6', 'linda.white@example.com', 'Doctor', 'Linda White', '1983-03-14', '6789012345678', '456 Spruce St', '321-654-0988'),
('robert_green', 'hashed_password_7', 'robert.green@example.com', 'Doctor', 'Robert Green', '1975-07-30', '7890123456789', '789 Birch St', '987-123-4568');

-- Insert new doctors with career information
INSERT INTO Doctors (UserId, DepartmentId, LicenseNumber, CareerInfo)
VALUES
    ((SELECT UserId FROM Users WHERE Username = 'michael_brown'), 1, '987654', 'Dr. Michael Brown has over 20 years of experience in Cardiology. He specializes in heart surgeries and has published numerous research papers.'),
    ((SELECT UserId FROM Users WHERE Username = 'linda_white'), 2, '876543', 'Dr. Linda White is a renowned Neurologist with a focus on neurodegenerative diseases. She has been awarded multiple times for her contributions to medical science.'),
    ((SELECT UserId FROM Users WHERE Username = 'robert_green'), 3, '765432', 'Dr. Robert Green is a Pediatrician with a passion for child healthcare. He has been working in the field for over 25 years and is known for his compassionate approach.');


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


-------------------------------------
-- Queries (includes models for implementation in C#)
-------------------------------------

-- Users --

-- Get users by username
SELECT * FROM Users
WHERE Username = @input_username

-- Patients --

-- Get all patients
SELECT 
    p.PatientId,
    p.UserId,
    u.Username,
    u.Name,
    u.BirthDate,
    u.Mail,
    u.PhoneNumber,
    u.Address,
    u.Cnp,
    u.RegistrationDate,
    u.AvatarUrl,
    p.BloodType,
    p.EmergencyContact,
    p.Allergies,
    p.Weight,
    p.Height
FROM Patients p
INNER JOIN Users u ON p.UserId = u.UserId;

-- Update password
UPDATE Users 
SET Password = @password 
WHERE UserId = @userId;

-- Update mail
UPDATE Users 
SET Mail = @newMail 
WHERE UserId = @userId;

-- Update phone
UPDATE Users 
SET PhoneNumber = @phoneNumber 
WHERE UserId = @userId;

-- Update picture
UPDATE Users 
SET AvatarUrl = @url 
WHERE UserId = @userId;


-- Doctors -- 

-- Get all doctors
SELECT 
    d.DoctorId,
    d.UserId,
    u.Username,
    u.Name,
    u.BirthDate,
    u.Mail,
    u.PhoneNumber,
    u.Address,
    u.Cnp,
    u.RegistrationDate,
    u.AvatarUrl, 
    d.DepartmentId,
    d.DoctorRating,
    d.LicenseNumber
FROM Doctors d
INNER JOIN Users u ON d.UserId = u.UserId;


-- Get doctors by department
SELECT 
    d.DoctorId,
    d.UserId,
    u.Username,
    u.Name,
    u.Mail,
    d.DepartmentId,
    d.DoctorRating,
    d.LicenseNumber
FROM Doctors d
INNER JOIN Users u ON d.UserId = u.UserId
WHERE d.DepartmentId = @departmentId;


-- Get doctors by name
SELECT 
    d.DoctorId,
    d.UserId,
    u.Username,
    u.Name,
    u.Mail,
    d.DepartmentId,
    d.DoctorRating,
    d.LicenseNumber
FROM Doctors d
INNER JOIN Users u ON d.UserId = u.UserId
WHERE u.Name LIKE '%' + @name + '%';


-- Updates are exactly the same, except we have carrer info
UPDATE Doctors 
SET CareerInfo = @careerInfo 
WHERE DoctorId = @doctorId;

-- Departments --

-- Get all departments
SELECT * FROM Departments

-- Logs --

-- Get all logs
SELECT * FROM Logs 
ORDER BY Timestamp DESC;

-- Get logs by user id
SELECT * FROM Logs 
WHERE UserId = @UserId;

-- Get logs before a Timestamp
SELECT * FROM Logs 
WHERE Timestamp < @BeforeTimestamp;  -- input example: 2024-03-26

-- Get logs by ActionType
SELECT * FROM Logs 
WHERE ActionType = @ActionType;

