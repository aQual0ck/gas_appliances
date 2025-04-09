INSERT INTO Roles(RoleName) 
VALUES ('Администратор'), 
('Пользователь');

INSERT INTO Users(Username, Password, FullName, RoleId)
VALUES ('admin', 'admin', 'Алексей Иванович Смирнов', 1),
('user', '12345', 'Дмитрий Сергеевич Кузнецов', 2)

INSERT INTO Category(CategoryName)
VALUES ('Плита'),
('Панель'),
('Духовка'),
('Водонагреватель'),
('Обогреватель'),
('Котел');

INSERT INTO Statuses(StatusName)
VALUES ('В эксплуатации'),
('Требуется проверка'),
('Требуется ремонт'),
('Списан');

INSERT INTO Owners(OwnerName, ContactInfo)
VALUES ('Александров Сергей Николаевич', '+7 901 123-45-67'),
('Михайлова Дарья Владимировна', '+7 902 234-56-78'),
('Иванов Роман Алексеевич', '+7 903 345-67-89'),
('Петрова Елена Викторовна', '+7 904 456-78-90'),
('Сидоров Андрей Павлович', '+7 905 567-89-01'),
('Кузнецова Марина Игоревна', '+7 906 678-90-12');

INSERT INTO Appliance(CategoryId, StatusId, ApplianceName, ApplianceAddress, ApplianceOwnerId, InstalledSince, NextExamination, SerialNumber)
VALUES (1, 1, 'Пламя 500 Lux', 'ул. Лесная, д. 15, кв. 42', 1, '2020-01-15', NULL, 'SN12345AB'),
(2, 2, 'Кухмастер G4 Turbo', 'пр. Энгельса, д. 50, кв. 12', 2, '2019-07-20', '2023-03-01', 'SN67890CD'),
(3, 3, 'ТеплоВест 600 Classic', 'ул. Баумана, д. 8, кв. 21', 3, '2021-05-10', NULL, 'SN11223EF'),
(4, 4, 'АкваГаз Mini 10', 'ул. Красный проспект, д. 120, кв. 9', 4, '2018-11-25', '2022-12-31', 'SN44556GH'),
(5, 3, 'Термогаз IR Comfort', 'ул. Малышева, д. 33, кв. 56', 5, '2022-03-30', NULL, 'SN77889IJ'),
(6, 1, 'ГАЗТЕХ ProHeat 24', 'ул. Родионова, д. 187, кв. 18', 6, '2017-09-05', '2023-01-15', 'SN99001KL');

INSERT INTO ApplianceCheck(UserId, ApplianceId, CheckDate)
VALUES (2, 1, '2021-02-13');