INSERT INTO Roles(RoleName) 
VALUES ('�������������'), 
('������������');

INSERT INTO Users(Username, Password, FullName, RoleId)
VALUES ('admin', 'admin', '������� �������� �������', 1),
('user', '12345', '������� ��������� ��������', 2)

INSERT INTO Category(CategoryName)
VALUES ('�����'),
('������'),
('�������'),
('���������������'),
('������������'),
('�����');

INSERT INTO Statuses(StatusName)
VALUES ('� ������������'),
('��������� ��������'),
('��������� ������'),
('������');

INSERT INTO Owners(OwnerName, ContactInfo)
VALUES ('����������� ������ ����������', '+7 901 123-45-67'),
('��������� ����� ������������', '+7 902 234-56-78'),
('������ ����� ����������', '+7 903 345-67-89'),
('������� ����� ����������', '+7 904 456-78-90'),
('������� ������ ��������', '+7 905 567-89-01'),
('��������� ������ ��������', '+7 906 678-90-12');

INSERT INTO Appliance(CategoryId, StatusId, ApplianceName, ApplianceAddress, ApplianceOwnerId, InstalledSince, NextExamination, SerialNumber)
VALUES (1, 1, '����� 500 Lux', '��. ������, �. 15, ��. 42', 1, '2020-01-15', NULL, 'SN12345AB'),
(2, 2, '��������� G4 Turbo', '��. ��������, �. 50, ��. 12', 2, '2019-07-20', '2023-03-01', 'SN67890CD'),
(3, 3, '��������� 600 Classic', '��. �������, �. 8, ��. 21', 3, '2021-05-10', NULL, 'SN11223EF'),
(4, 4, '������� Mini 10', '��. ������� ��������, �. 120, ��. 9', 4, '2018-11-25', '2022-12-31', 'SN44556GH'),
(5, 3, '�������� IR Comfort', '��. ��������, �. 33, ��. 56', 5, '2022-03-30', NULL, 'SN77889IJ'),
(6, 1, '������ ProHeat 24', '��. ���������, �. 187, ��. 18', 6, '2017-09-05', '2023-01-15', 'SN99001KL');

INSERT INTO ApplianceCheck(UserId, ApplianceId, CheckDate)
VALUES (2, 1, '2021-02-13');