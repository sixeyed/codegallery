USE [AdventureWorks]
GO
/****** Object:  StoredProcedure [Production].[uspInsertProduct]    Script Date: 11/10/2009 08:46:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [Production].[uspInsertProduct]
    @Name nvarchar(50),
    @ProductNumber nvarchar(25),
    @SafetyStockLevel smallint,
	@ReorderPoint smallint,
	@StandardCost money,
	@ListPrice money,
	@DaysToManufacture int,
	@SellStartDate datetime
WITH EXECUTE AS CALLER
AS
BEGIN
  INSERT INTO [Production].[Product]
    ([Name],ProductNumber,SafetyStockLevel,
     ReorderPoint,StandardCost,ListPrice,
	 DaysToManufacture,SellStartDate)
  VALUES
	(@Name,@ProductNumber,@SafetyStockLevel,
	 @ReorderPoint,@StandardCost,@ListPrice,
	 @DaysToManufacture,@SellStartDate)
END;
GO;