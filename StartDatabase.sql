USE master
GO

-- BORRAR BASE DE DATOS SI EXISTE
IF EXISTS(SELECT * FROM sys.databases WHERE name = 'N5Solucion')
BEGIN 
    DROP DATABASE [N5Solucion]
END 
GO

-- CREAR BASE DE DATOS Y USARLA
CREATE DATABASE [N5Solucion]
GO
USE [N5Solucion]
GO

-- CREACION DE ESQUEMA
CREATE SCHEMA n5Schema;
GO

-- CREACION DE Permissions 
CREATE TABLE n5Schema.[Permissions](
    Id INT IDENTITY(1,1) NOT NULL,
    EmployeeForename TEXT NOT NULL,
    EmployeeSurname TEXT NOT NULL,
    PermissionType INT NOT NULL,
    PermissionDate DATE NOT NULL,
    PRIMARY KEY(Id)
)
GO

-- CREACION DE PermissionsTypes
CREATE TABLE n5Schema.PermissionsTypes(
    Id INT IDENTITY(1,1) NOT NULL,
    [Description] TEXT NOT NULL,
    PRIMARY KEY(Id)
)
GO

-- CREAR RELACION ENTRE Permissions - PermissionsTypes
ALTER TABLE n5Schema.[Permissions]
ADD CONSTRAINT FK_Permissions_PermissionsTypes
FOREIGN KEY (PermissionType) REFERENCES n5Schema.PermissionsTypes(Id)
GO

-- DATA DE EJEMPLO
INSERT INTO n5Schema.PermissionsTypes VALUES
('Regular User'), -- 1
('Medium User'), -- 2
('Dev User'), -- 3
('IT User'), -- 4
('Sales User'), -- 5
('Bussines User'), -- 6
('Admin User') -- 7

INSERT INTO n5Schema.[Permissions] VALUES
('ELENA','ROSALES',1,'2022-01-01'),
('MARTA','SANDOVAL',1,'2022-01-02'),
('EVA','CEBALLOS',2,'2022-01-03'),
('LUCIA','CANO',2,'2022-01-04'),
('MARIA','PACHECO',3,'2022-01-05'),
('RAQUEL','BURGOS',3,'2022-01-06'),
('SARA','PRIETO',4,'2022-01-07'),
('DIEGO','BARRERA',4,'2022-01-08'),
('SERGIO','ZAPATA',5,'2022-01-09'),
('JORGE','ZARATE',5,'2022-01-10'),
('JAVIER','RUSSO',6,'2022-01-11'),
('KEVIN','ASTROZ',6,'2022-01-12'),
('ALEJANDRO','BAEZ',7,'2022-01-13'),
('CARLOS','CORONEL',7,'2022-01-14')
GO