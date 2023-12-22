
use [Dentistry]
go


insert into [Clinic] ( [Addres_Clinic])
values ('г. Москва, ул. Родниковая, д. 6, к.1')
go
select * from [Clinic]

insert into [Role] ([Name_Role])
values ('Пациент'),('Врач'),('Менеджер по услугам')
go
select * from [Role]

insert into [Employee] ([Surname_Employee], [First_Name_Employee], [Middle_Employee], [Email_Employee], [Password_Employee], [Experience], [Role_ID], [Clinic_ID])
values ('Олегов','Игнат','Адреевич','olegov@mail.ru','oleg1', 10, 2,1 ),
		('Сухова','Екатерина','Алексеевна','syxv@mail.ru','syx1', 20, 2,1 ),
		('Санпасова','Дарья','Макаровна','kis@mail.ru','kis1', 2, 2,1 ),
		('Фадеева','Анастасия','Александровна','fad@mail.ru','fad1', 3, 3,1 )
go
select * from [Employee]


insert into [Service] ([Name_Service], [Price_Service])
values ('Осмотр', 1000),
		('Удаление кариеса', 7000),
		('Чистка зубов', 4000)
go
select * from [Service]

insert into [Patient] ([Surname_Patient], [First_Name_Patient], [Middle_Patient], [Email_Patient], [Password_Patient], [Sex],[Age], [Role_ID])
values ('Юров','Юрий','Юрьевич','yur@mail.ru','yur1', 'Мужчина', 22, 1 ),
('Максимов','Максим','Максимович','max@mail.ru','max1', 'Мужчина', 32, 1 ),
('Евгеньев','Евгений','Евгеньевич','evg@mail.ru','evg1', 'Другое', 18, 1 )
go
select * from [Patient]

insert into [Date_Appointment] ([Date], [Time], [Free])
values ('25-09-04', '15:00', 1),
		('25-09-04', '16:00', 1),
		('25-09-04', '19:00', 1),
		('25-12-04', '19:00', 1)
go
select * from [Date_Appointment]

insert into [Appointment] ([Number], [Complaint], [Treatment], [Employee_ID],[Patient_ID], [Service_ID], [Date_Appointment_ID])
values ('1111', 'Боль в деснах','Хороший уход и обезболивающий гель', 1, 1, 1, 1),
('2111', 'Боль в зубе','Паста с кальцием и новая щетка', 2, 2, 2, 2 ),
('3111','Налёт на зубах','Регулярная чистка раз в полгода', 3, 3, 3, 3 )
go
select * from [Appointment]

--Триггеры

CREATE OR ALTER TRIGGER Update_Free_Status
ON [dbo].[Appointment]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE da
    SET da.[Free] = 0 
    FROM [dbo].[Date_Appointment] da
    INNER JOIN inserted i ON da.[ID_Date_Appointment] = i.[Date_Appointment_ID];

END;

CREATE OR ALTER TRIGGER Delete_Appointment
ON [dbo].[Appointment]
AFTER DELETE
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE da
    SET da.[Free] = 1
    FROM [dbo].[Date_Appointment] da
    INNER JOIN inserted i ON da.[ID_Date_Appointment] = i.[Date_Appointment_ID];

END;


--Виртуальные таблицы

CREATE OR ALTER VIEW [History] AS
SELECT 
		Appointment.Complaint AS 'Жалоба', 
		Appointment.Treatment AS 'Лечение', 
		Date_Appointment.Date AS 'Дата', 
        Patient.Surname_Patient+' '+Patient.First_Name_Patient+' '+Patient.Middle_Patient AS 'Пациент'
FROM Appointment INNER JOIN Patient ON Appointment.Patient_ID = Patient.ID_Patient
INNER JOIN Date_Appointment ON Appointment.Date_Appointment_ID = Date_Appointment.ID_Date_Appointment

select * from History

CREATE OR ALTER VIEW Price_Appointment AS
SELECT Date_Appointment.Date AS 'Дата', 
		Appointment.Number AS 'Номер записи', 
        Service.Price_Service AS 'Цена'
FROM Appointment INNER JOIN Service ON Appointment.Service_ID = Service.ID_Service
INNER JOIN Date_Appointment ON Appointment.Date_Appointment_ID = Date_Appointment.ID_Date_Appointment

select * from Price_Appointment

--Транзакции

BEGIN TRY

   BEGIN TRANSACTION

   UPDATE Service SET Price_Service = 2000
   WHERE ID_Service = 1;

   UPDATE Service SET Price_Service = 9000
   WHERE ID_Service = 2;

   END TRY
   BEGIN CATCH
      --Откат транзакции
      ROLLBACK TRANSACTION

      SELECT ERROR_NUMBER() AS [Номер ошибки],
             ERROR_MESSAGE() AS [Описание ошибки]

   RETURN

   END CATCH

   COMMIT TRANSACTION

   GO

   SELECT ID_Service, Name_Service, Price_Service
   FROM Service;
--
BEGIN TRY
BEGIN TRANSACTION

UPDATE Appointment SET Number = 1212
WHERE Patient_ID = 1;

UPDATE Appointment SET Number = 1133
WHERE Patient_ID=2;

END TRY

BEGIN CATCH
	ROLLBACK TRANSACTION
	SELECT ERROR_NUMBER() AS [Номер ошибки],
		   ERROR_MESSAGE() AS [Описание ошибки]
RETURN
END CATCH

COMMIT TRANSACTION
GO

SELECT Number AS 'Номер записи', [Surname_Patient]+' '+[First_Name_Patient]+' '+[Middle_Patient] AS 'Пациент'
FROM Appointment INNER JOIN Patient ON Appointment.Patient_ID = Patient.ID_Patient;


--Процедуры

CREATE OR ALTER PROCEDURE Date_App AS
BEGIN
	SELECT [Date] AS 'Дата', [Time] AS 'Время', [Surname_Patient]+' '+[First_Name_Patient]+' '+[Middle_Patient] AS 'Пациент'
	FROM Appointment INNER JOIN Patient ON Appointment.Patient_ID = Patient.ID_Patient
	INNER JOIN Date_Appointment ON Appointment.Date_Appointment_ID = Date_Appointment.ID_Date_Appointment
END;

EXEC Date_App

CREATE OR ALTER PROCEDURE Experience_Employee AS
BEGIN
	SELECT [Experience] AS 'Опыт работы, г', [Addres_Clinic] AS 'Адрес клиники', [Surname_Employee]+' '+[First_Name_Employee]+' '+[Middle_Employee] AS 'Сотрудник'
	FROM Employee INNER JOIN Clinic ON Employee.Clinic_ID = Clinic.ID_Clinic
END;

EXEC Experience_Employee

--Функции

CREATE OR ALTER FUNCTION Info_Appointment()
RETURNS TABLE
AS
 RETURN 
 (
  SELECT Name_Service AS 'Название услуги', Price_Service AS 'Цена', [Surname_Employee]+' '+[First_Name_Employee]+' '+[Middle_Employee] AS 'Врач', [Date] AS 'Дата', [Time] AS 'Время', [Addres_Clinic] AS 'Адрес клиники'
  FROM Appointment INNER JOIN Service ON Appointment.Service_ID = Service.ID_Service
  INNER JOIN Employee ON Appointment.Employee_ID = Employee.ID_Employee
  INNER JOIN Clinic ON Employee.Clinic_ID = Clinic.ID_Clinic
  INNER JOIN Date_Appointment ON Appointment.Date_Appointment_ID = Date_Appointment.ID_Date_Appointment
 )

 SELECT * FROM Info_Appointment()

CREATE OR ALTER FUNCTION NDFL_Price
 (@name varchar(50))
RETURNS decimal(10,2)
 BEGIN
  DECLARE @Summ decimal(10,2)
  SELECT @Summ = Price_Service-(Price_Service*0.13)
  FROM Service
  WHERE Name_Service=@name
  RETURN @Summ
 END

SELECT dbo.NDFL_Price('Осмотр')