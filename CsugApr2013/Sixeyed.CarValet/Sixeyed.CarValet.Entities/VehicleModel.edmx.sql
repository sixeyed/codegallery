
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, and Azure
-- --------------------------------------------------
-- Date Created: 04/07/2013 19:27:06
-- Generated from EDMX file: C:\TFS\Incubation\CsugApril13\Source\CarValet\Sixeyed.CarValet\Sixeyed.CarValet.Entities\VehicleModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [CarValet];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_MakeModel]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Models] DROP CONSTRAINT [FK_MakeModel];
GO
IF OBJECT_ID(N'[dbo].[FK_ModelVehicle]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Vehicles] DROP CONSTRAINT [FK_ModelVehicle];
GO
IF OBJECT_ID(N'[dbo].[FK_VehicleTypeVehicle]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Vehicles] DROP CONSTRAINT [FK_VehicleTypeVehicle];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Makes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Makes];
GO
IF OBJECT_ID(N'[dbo].[Models]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Models];
GO
IF OBJECT_ID(N'[dbo].[Vehicles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Vehicles];
GO
IF OBJECT_ID(N'[dbo].[VehicleTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VehicleTypes];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Makes'
CREATE TABLE [dbo].[Makes] (
    [MakeCode] nvarchar(50)  NOT NULL,
    [MakeName] nvarchar(100)  NOT NULL,
    [MakeNameLower] varchar(100)  NULL
);
GO

-- Creating table 'Models'
CREATE TABLE [dbo].[Models] (
    [MakeCode] nvarchar(50)  NOT NULL,
    [ModelCode] nvarchar(50)  NOT NULL,
    [ModelName] nvarchar(100)  NOT NULL,
    [ModelNameLower] varchar(100)  NULL
);
GO

-- Creating table 'Vehicles'
CREATE TABLE [dbo].[Vehicles] (
    [VehicleId] int IDENTITY(1,1) NOT NULL,
    [ProducedFromUtc] datetime  NOT NULL,
    [ProducedToUtc] datetime  NULL,
    [Name] nvarchar(max)  NOT NULL,
    [ImageUrl] nvarchar(max)  NOT NULL,
    [Model_MakeCode] nvarchar(50)  NOT NULL,
    [Model_ModelCode] nvarchar(50)  NOT NULL,
    [VehicleType_TypeCode] nvarchar(10)  NOT NULL
);
GO

-- Creating table 'VehicleTypes'
CREATE TABLE [dbo].[VehicleTypes] (
    [TypeCode] nvarchar(10)  NOT NULL,
    [TypeDescription] nvarchar(max)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [MakeCode] in table 'Makes'
ALTER TABLE [dbo].[Makes]
ADD CONSTRAINT [PK_Makes]
    PRIMARY KEY CLUSTERED ([MakeCode] ASC);
GO

-- Creating primary key on [MakeCode], [ModelCode] in table 'Models'
ALTER TABLE [dbo].[Models]
ADD CONSTRAINT [PK_Models]
    PRIMARY KEY CLUSTERED ([MakeCode], [ModelCode] ASC);
GO

-- Creating primary key on [VehicleId] in table 'Vehicles'
ALTER TABLE [dbo].[Vehicles]
ADD CONSTRAINT [PK_Vehicles]
    PRIMARY KEY CLUSTERED ([VehicleId] ASC);
GO

-- Creating primary key on [TypeCode] in table 'VehicleTypes'
ALTER TABLE [dbo].[VehicleTypes]
ADD CONSTRAINT [PK_VehicleTypes]
    PRIMARY KEY CLUSTERED ([TypeCode] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [MakeCode] in table 'Models'
ALTER TABLE [dbo].[Models]
ADD CONSTRAINT [FK_MakeModel]
    FOREIGN KEY ([MakeCode])
    REFERENCES [dbo].[Makes]
        ([MakeCode])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Model_MakeCode], [Model_ModelCode] in table 'Vehicles'
ALTER TABLE [dbo].[Vehicles]
ADD CONSTRAINT [FK_ModelVehicle]
    FOREIGN KEY ([Model_MakeCode], [Model_ModelCode])
    REFERENCES [dbo].[Models]
        ([MakeCode], [ModelCode])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ModelVehicle'
CREATE INDEX [IX_FK_ModelVehicle]
ON [dbo].[Vehicles]
    ([Model_MakeCode], [Model_ModelCode]);
GO

-- Creating foreign key on [VehicleType_TypeCode] in table 'Vehicles'
ALTER TABLE [dbo].[Vehicles]
ADD CONSTRAINT [FK_VehicleTypeVehicle]
    FOREIGN KEY ([VehicleType_TypeCode])
    REFERENCES [dbo].[VehicleTypes]
        ([TypeCode])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_VehicleTypeVehicle'
CREATE INDEX [IX_FK_VehicleTypeVehicle]
ON [dbo].[Vehicles]
    ([VehicleType_TypeCode]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------