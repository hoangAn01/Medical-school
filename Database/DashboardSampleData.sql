-- Sample Data for Dashboard

-- Add sample students
INSERT INTO [dbo].[Student] (FullName, Gender, DateOfBirth, ParentID, ClassID)
VALUES 
    ('Nguyen Van A', 'M', '2010-01-15', 1, 1),
    ('Tran Thi B', 'F', '2010-03-20', 2, 1),
    ('Le Van C', 'M', '2010-05-25', 3, 2),
    ('Pham Thi D', 'F', '2010-07-30', 4, 2),
    ('Hoang Van E', 'M', '2010-09-10', 5, 3)
GO

-- Add sample health profiles
INSERT INTO [dbo].[HealthProfile] (StudentID, ChronicDisease, VisionTest, Allergy, Weight, Height, LastCheckupDate)
VALUES 
    (1, 'None', '20/20', 'None', 45.5, 150.0, '2023-05-15'),
    (2, 'Asthma', '20/20', 'Peanuts', 47.2, 152.0, '2023-05-15'),
    (3, 'None', '20/20', 'None', 46.8, 151.0, '2023-05-15'),
    (4, 'None', '20/20', 'Dust', 48.5, 153.0, '2023-05-15'),
    (5, 'None', '20/20', 'None', 49.0, 154.0, '2023-05-15')
GO

-- Add sample medical events (last month)
INSERT INTO [dbo].[MedicalEvent] (StudentID, EventType, Description, EventTime, NurseID, Status)
VALUES 
    (1, 'Sick Leave', 'Fever and cough', '2023-05-10 09:00:00', 1, 'Active'),
    (2, 'First Aid', 'Minor cut on finger', '2023-05-15 11:30:00', 1, 'Resolved'),
    (3, 'Sick Leave', 'Stomachache', '2023-05-20 10:15:00', 2, 'Active'),
    (4, 'First Aid', 'Sprained ankle', '2023-05-22 14:45:00', 2, 'Resolved'),
    (5, 'Sick Leave', 'Headache', '2023-05-25 13:30:00', 1, 'Active')
GO

-- Add sample medicine requests
INSERT INTO [dbo].[MedicineRequest] (Date, RequestStatus, StudentID, ParentID, Note, ApprovedBy, ApprovalDate)
VALUES 
    ('2023-05-10', 'Approved', 1, 1, 'Paracetamol for fever', 1, '2023-05-10'),
    ('2023-05-15', 'Pending', 2, 2, 'Bandage for cut', NULL, NULL),
    ('2023-05-20', 'Approved', 3, 3, 'Antacid for stomachache', 1, '2023-05-20'),
    ('2023-05-22', 'Approved', 4, 4, 'Ice pack for sprain', 2, '2023-05-22'),
    ('2023-05-25', 'Pending', 5, 5, 'Painkiller for headache', NULL, NULL)
GO

-- Add sample vaccine records
INSERT INTO [dbo].[VaccineRecord] (StudentID, VaccinationEventID, NurseID, VaccineName, InjectionDate, Reaction, FollowUpStatus, InjectionSite, NextDoseDate)
VALUES 
    (1, 1, 1, 'Flu Vaccine', '2023-05-10', 'None', 'Completed', 'Left Arm', '2024-05-10'),
    (2, 1, 1, 'Flu Vaccine', '2023-05-10', 'None', 'Completed', 'Right Arm', '2024-05-10'),
    (3, 2, 2, 'Measles Vaccine', '2023-05-15', 'Mild fever', 'Completed', 'Left Arm', '2024-05-15'),
    (4, 2, 2, 'Measles Vaccine', '2023-05-15', 'None', 'Completed', 'Right Arm', '2024-05-15'),
    (5, 3, 1, 'Hepatitis B', '2023-05-20', 'None', 'Completed', 'Left Arm', '2024-05-20')
GO

-- Add sample dashboard notifications
INSERT INTO [dbo].[DashboardNotification] (UserID, Title, Message, Type, Priority, IsRead, CreatedDate)
VALUES 
    (1, 'New Medical Event', 'Student Nguyen Van A reported fever and cough', 'Medical', 2, 0, '2023-05-10'),
    (1, 'Vaccination Update', 'Flu vaccine campaign completed successfully', 'Vaccination', 1, 0, '2023-05-10'),
    (2, 'Health Checkup Reminder', 'Health checkups for Class 1 will be held next week', 'Reminder', 1, 0, '2023-05-15'),
    (3, 'Medicine Request', 'Medicine request for Student Pham Thi D approved', 'Medical', 2, 0, '2023-05-22'),
    (2, 'Vaccination Reminder', 'Measles vaccine campaign starts tomorrow', 'Vaccination', 3, 0, '2023-05-15')
GO

-- Add sample dashboard preferences
INSERT INTO [dbo].[DashboardPreferences] (UserID, PreferredWidgets, Theme, RefreshInterval)
VALUES 
    (1, 'overview,medical-events,trends,notifications', 'light', 300),
    (2, 'overview,vaccinations,health-checkups,notifications', 'dark', 600),
    (3, 'overview,medicine-requests,medical-events,notifications', 'light', 300)
GO

-- Add sample medical inventory
INSERT INTO [dbo].[MedicalInventory] (ItemName, Category, Quantity, Unit, Description)
VALUES 
    ('Paracetamol', 'Medicine', 100, 'Tablet', 'Fever and pain relief'),
    ('Bandages', 'First Aid', 50, 'Pack', 'Wound dressing'),
    ('Antacid', 'Medicine', 75, 'Tablet', 'Stomach relief'),
    ('Ice Packs', 'First Aid', 20, 'Pack', 'Cold therapy'),
    ('Painkillers', 'Medicine', 90, 'Tablet', 'Pain relief')
GO

-- Add sample vaccination events
INSERT INTO [dbo].[VaccinationEvent] (EventName, Date, Location, ManagerID, ClassID)
VALUES 
    ('Flu Vaccine Campaign', '2023-05-10', 'School Gym', 1, NULL),
    ('Measles Vaccine Campaign', '2023-05-15', 'School Hall', 1, NULL),
    ('Hepatitis B Campaign', '2023-05-20', 'School Gym', 1, NULL)
GO

-- Add sample parental consents
INSERT INTO [dbo].[ParentalConsent] (StudentID, VaccinationEventID, ParentID, ConsentStatus, ConsentDate, Note)
VALUES 
    (1, 1, 1, 'Approved', '2023-05-09', 'No objections'),
    (2, 1, 2, 'Approved', '2023-05-09', 'No objections'),
    (3, 2, 3, 'Approved', '2023-05-14', 'No objections'),
    (4, 2, 4, 'Approved', '2023-05-14', 'No objections'),
    (5, 3, 5, 'Approved', '2023-05-19', 'No objections')
GO

-- Add sample blog posts
INSERT INTO [dbo].[Blog] (Title, Content, ImageUrl, AuthorID, CreatedDate, UpdatedDate, IsPublished)
VALUES 
    ('Summer Health Tips', 'Tips for staying healthy during summer...', 'summer-health.jpg', 1, '2023-05-01', '2023-05-01', 1),
    ('Vaccination Guide', 'Guide to school vaccination programs...', 'vaccination-guide.jpg', 1, '2023-05-05', '2023-05-05', 1),
    ('First Aid Basics', 'Basic first aid procedures for common injuries...', 'first-aid.jpg', 2, '2023-05-10', '2023-05-10', 1)
GO
