set ansi_nulls on
go
set ansi_padding on
go
set quoted_identifier on
go

create database [Dentistry]
go


use [Dentistry]
go

create table [dbo].[Clinic]
(
	[ID_Clinic] [int] not null identity(1,1),
	[Addres_Clinic] [varchar] (100) not null,
	constraint [PK_Clinic] primary key clustered 
	([ID_Clinic] ASC) on [PRIMARY]
)
go


create table [Role]
(
	[ID_Role] [int] not null identity(1,1),
	[Name_Role] [varchar] (50) not null,
	constraint [PK_Role] primary key clustered
	([ID_Role] ASC) on [PRIMARY]
)
go


create table [dbo].[Employee]
(
	[ID_Employee] [int] not null identity(1,1),
	[Surname_Employee] [varchar] (50) not null,
	[First_Name_Employee] [varchar] (50) not null,
	[Middle_Employee] [varchar] (50) not null,
	[Email_Employee] [varchar] (50) not null,
	[Password_Employee] [varchar] (10) not null,
	[Experience] [int]  not null,
	[Role_ID] [int] not null,
	[Clinic_ID] [int] not null,
	constraint [FK_Role] foreign key ([Role_ID])
	references [dbo].[Role] ([ID_Role]),
	constraint [FK_Clinic] foreign key ([Clinic_ID])
	references [dbo].[Clinic] ([ID_Clinic]),
	constraint [PK_Employee] primary key clustered 
	([ID_Employee] ASC) on [PRIMARY]
)
go

create table [dbo].[Service]
(
	[ID_Service] [int] not null identity(1,1),
	[Name_Service] [varchar] (50) not null,
	[Price_Service] [int] not null,
	constraint [PK_Service] primary key clustered 
	([ID_Service] ASC) on [PRIMARY]
)
go


create table [dbo].[Patient]
(
	[ID_Patient] [int] not null identity(1,1),
	[Surname_Patient] [varchar] (50) not null,
	[First_Name_Patient] [varchar] (50) not null,
	[Middle_Patient] [varchar] (50) not null,
	[Email_Patient] [varchar] (50) not null,
	[Password_Patient] [varchar] (10) not null,
	[Sex] [varchar] (50)  not null,
	[Role_ID] [int] not null,
	[Age] [varchar] (50) not null,
	constraint [FK_Role_Patient] foreign key ([Role_ID])
	references [dbo].[Role] ([ID_Role]),
	constraint [PK_Patient] primary key clustered 
	([ID_Patient] ASC) on [PRIMARY]
)
go

create table [dbo].[Date_Appointment]
(
	[ID_Date_Appointment] [int] not null identity(1,1),
	[Date] [date] not null,
	[Time] [time] not null,
	[Free] [bit] not null,
	constraint [PK_Date_Appointment] primary key clustered 
	([ID_Date_Appointment] ASC) on [PRIMARY]
)
go

create table [dbo].[Appointment]
(
	[ID_Appointment] [int] not null identity(1,1),
	[Number] [varchar] (5) not null,
	[Complaint] [varchar] (200) not null,
	[Treatment] [varchar] (200) not null,
	[Employee_ID] [int]  not null,
	[Patient_ID] [int] not null,
	[Service_ID] [int] not null,
	[Date_Appointment_ID] [int] not null,
	constraint [FK_Employee] foreign key ([Employee_ID])
	references [dbo].[Employee] ([ID_Employee]),
	constraint [FK_Patient] foreign key ([Patient_ID])
	references [dbo].[Patient] ([ID_Patient]),
	constraint [FK_Service] foreign key ([Service_ID])
	references [dbo].[Service] ([ID_Service]),
	constraint [FK_Date_Appointment] foreign key ([Date_Appointment_ID])
	references [dbo].[Date_Appointment] ([ID_Date_Appointment]),
	constraint [PK_Appointment] primary key clustered 
	([ID_Appointment] ASC) on [PRIMARY]
)
go


