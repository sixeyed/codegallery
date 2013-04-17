INSERT [dbo].[Vehicles] ([ProducedFromUtc], [Name], [ImageUrl], [Model_MakeCode], [Model_ModelCode], 
[VehicleType_TypeCode])
 VALUES (CAST(0x00009CF100000000 AS DateTime), N'Audi A3', 
 N'http://upload.wikimedia.org/wikipedia/commons/thumb/0/09/2009_Audi_A3_%288PA%29_2.0_TDI_5-door_Sportback_%282011-04-02%29_01.jpg/320px-2009_Audi_A3_%288PA%29_2.0_TDI_5-door_Sportback_%282011-04-02%29_01.jpg',
  N'audi', N'a3', N'MDN');

INSERT [dbo].[Vehicles] ([ProducedFromUtc], ProducedToUtc, [Name], [ImageUrl], [Model_MakeCode], [Model_ModelCode], 
[VehicleType_TypeCode])
 VALUES ('1964-01-01', '1968-01-01', N'123GT', 
 N'http://upload.wikimedia.org/wikipedia/commons/thumb/0/09/2009_Audi_A3_%288PA%29_2.0_TDI_5-door_Sportback_%282011-04-02%29_01.jpg/320px-2009_Audi_A3_%288PA%29_2.0_TDI_5-door_Sportback_%282011-04-02%29_01.jpg',
  N'volvo', N'amazon', N'CLASS');

  INSERT [dbo].[Vehicles] ([ProducedFromUtc], ProducedToUtc, [Name], [ImageUrl], [Model_MakeCode], [Model_ModelCode], 
[VehicleType_TypeCode])
 VALUES ('1964-01-01', '1970-01-01', N'122', 
 N'http://upload.wikimedia.org/wikipedia/commons/thumb/0/09/2009_Audi_A3_%288PA%29_2.0_TDI_5-door_Sportback_%282011-04-02%29_01.jpg/320px-2009_Audi_A3_%288PA%29_2.0_TDI_5-door_Sportback_%282011-04-02%29_01.jpg',
  N'volvo', N'amazon', N'CLASS');
