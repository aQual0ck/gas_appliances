CREATE TABLE ToolCategory(
Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
ToolCategoryName NVARCHAR(50) NOT NULL
);

CREATE TABLE ToolManufacturerType(
Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
ManufacturerTypeName NVARCHAR(100) NOT NULL
);

CREATE TABLE ToolManufacturer(
Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
ManufacturerName NVARCHAR(100) NOT NULL,
ManufacturerTypeId INT NOT NULL FOREIGN KEY REFERENCES ToolManufacturerType(Id),
ContactInfo NVARCHAR(100),
RepresentativeName NVARCHAR(100)
);

CREATE TABLE Tool(
Id INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
CategoryId INT NOT NULL FOREIGN KEY REFERENCES ToolCategory(Id),
ModelName NVARCHAR(100) NOT NULL,
OperatingSince DATE,
NextExamination DATE,
DecomissionedSince DATE,
ManufacturerId INT NOT NULL FOREIGN KEY REFERENCES ToolManufacturer(Id),
SerialNumber NVARCHAR(100),
Notes NVARCHAR(255)
);

CREATE TABLE ToolCheck(
UserId INT FOREIGN KEY REFERENCES Users(Id),
ToolId INT FOREIGN KEY REFERENCES Tool(Id),
CheckDate DATE NOT NULL,
PRIMARY KEY(UserId, ToolId)
);

INSERT INTO Users (Username, Password, FullName, RoleId) VALUES 
('user2', '12345', 'Сидоров Алексей Павлович', 1),
('user3', '12345', 'Тарасова Марина Юрьевна', 2),
('user4', '12345', 'Волков Дмитрий Николаевич', 1),
('user5', '12345', 'Кузнецова Ольга Александровна', 2),
('user6', '12345', 'Захаров Иван Сергеевич', 1),
('user7', '12345', 'Воронова Дарья Романовна', 2),
('user8', '12345', 'Орлов Сергей Игоревич', 1),
('user9', '12345', 'Павлова Анна Дмитриевна', 2),
('user10', '12345', 'Никитин Роман Андреевич', 1),
('user11', '12345', 'Егорова Татьяна Константиновна', 2),
('user12', '12345', 'Фёдоров Михаил Васильевич', 1),
('user13', '12345', 'Лебедева Анастасия Олеговна', 2),
('user14', '12345', 'Макаров Николай Евгеньевич', 1);

INSERT INTO ToolCategory (ToolCategoryName) VALUES 
('Газоанализатор'),
('Манометр показывающий'),
('Манометр технический'),
('Индикатор утечки газа'),
('Электрический счетчик');

INSERT INTO ToolManufacturerType (ManufacturerTypeName) VALUES 
('ООО'),
('АО'),
('ЗАО');

INSERT INTO ToolManufacturer (ManufacturerName, ManufacturerTypeId, ContactInfo, RepresentativeName) VALUES 
('ГазПромПрибор', 1, '+7 916 123-45-67', 'Романов Сергей Петрович'),
('ТехСервис', 2, '+7 917 234-56-78', 'Кириллова Анна Владимировна'),
('ИнженерГаз', 3, '+7 918 345-67-89', 'Гаврилов Михаил Олегович'),
('ЭнергоПром', 1, '+7 919 456-78-90', 'Смирнова Елена Юрьевна'),
('ГазКонтроль', 2, '+7 920 567-89-01', 'Тихонов Артем Викторович'),
('ПромИзмер', 3, '+7 921 678-90-12', 'Васильева Татьяна Алексеевна'),
('СтройМетр', 1, '+7 922 789-01-23', 'Жуков Илья Михайлович'),
('СигналГаз', 2, '+7 923 890-12-34', 'Соловьева Мария Сергеевна'),
('НижПрибор', 3, '+7 924 901-23-45', 'Фомин Алексей Николаевич'),
('МегаГазПром', 1, '+7 925 012-34-56', 'Павлова Инна Дмитриевна'),
('СибПрибор', 2, '+7 926 123-45-67', 'Данилов Виктор Андреевич'),
('ТехИнструмент', 3, '+7 927 234-56-78', 'Козлова Ирина Павловна'),
('АльфаГаз', 1, '+7 928 345-67-89', 'Куликов Андрей Романович'),
('ВолгаПрибор', 2, '+7 929 456-78-90', 'Семенова Алёна Владиславовна'),
('РегионГаз', 3, '+7 930 567-89-01', 'Мельников Николай Егорович');

INSERT INTO Tool (CategoryId, ModelName, OperatingSince, NextExamination, ManufacturerId, SerialNumber) VALUES 
(1, 'GAZ-100', '2022-01-10', '2023-06-04', 1, 'TSN2001'),
(2, 'MANO-200', '2021-06-18', '2023-06-18', 2, 'TSN2002'),
(3, 'TECH-300', '2022-09-25', '2023-07-02', 3, 'TSN2003'),
(4, 'GASLEAK-400', '2023-03-11', '2023-07-15', 4, 'TSN2004'),
(5, 'ELEC-500', '2021-11-05', '2023-08-01', 5, 'TSN2005'),
(1, 'GAZ-101', '2022-02-14', '2023-08-19', 6, 'TSN2006'),
(2, 'MANO-201', '2023-01-22', '2023-09-07', 7, 'TSN2007'),
(3, 'TECH-301', '2022-08-19', '2023-09-21', 8, 'TSN2008'),
(4, 'GASLEAK-401', '2021-04-08', '2023-10-05', 9, 'TSN2009'),
(5, 'ELEC-501', '2023-05-30', '2023-10-20', 10,'TSN2010'),
(1, 'GAZ-102', '2022-07-07', '2023-11-08', 11,'TSN2011'),
(2, 'MANO-202', '2022-10-13', '2023-11-22', 12,'TSN2012'),
(3, 'TECH-302', '2021-12-01', '2023-12-06', 13,'TSN2013'),
(4, 'GASLEAK-402', '2023-02-16', '2023-12-21', 14,'TSN2014'),
(5, 'ELEC-502', '2022-06-09', '2024-01-09', 15,'TSN2015');

INSERT INTO ToolCheck (UserId, ToolId, CheckDate) VALUES 
(1, 12, '2023-03-01'),
(2, 3, '2022-05-12'),
(3, 9, '2021-11-20'),
(4, 14, '2023-01-18'),
(5, 6, '2022-03-30'),
(6, 1, '2023-02-22'),
(7, 11, '2021-07-07'),
(8, 5, '2022-12-15'),
(9, 8, '2022-04-05'),
(10, 15, '2021-09-17'),
(11, 7, '2022-10-25'),
(12, 2, '2023-06-01'),
(13, 10, '2021-08-23'),
(14, 13, '2023-03-10'),
(15, 4, '2022-11-19');