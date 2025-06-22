USE [master]
GO

-- Create Database
CREATE DATABASE [SchoolMedicalDB]
GO

USE [SchoolMedicalDB]
GO

-- Enable FullText Search if needed
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [SchoolMedicalDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

-- Enable Query Store
ALTER DATABASE [SchoolMedicalDB] SET QUERY_STORE = ON
GO

-- Create Tables

-- Base Account Table
CREATE TABLE [dbo].[Account] (
    [UserID] INT IDENTITY(1,1) PRIMARY KEY,
    [Username] NVARCHAR(100) NOT NULL,
    [PasswordHash] NVARCHAR(255) NOT NULL,
    [Role] NVARCHAR(50) NOT NULL,
    [Active] BIT DEFAULT 1
)
GO

-- Student Table
CREATE TABLE [dbo].[Student] (
    [StudentID] INT IDENTITY(1,1) PRIMARY KEY,
    [FullName] NVARCHAR(100),
    [Gender] CHAR(1),
    [DateOfBirth] DATE,
    [ParentID] INT,
    [UserID] INT,
    [ClassID] INT,
    CONSTRAINT FK_Student_Parent FOREIGN KEY (ParentID) REFERENCES [dbo].[Parent](ParentID),
    CONSTRAINT FK_Student_Account FOREIGN KEY (UserID) REFERENCES [dbo].[Account](UserID),
    CONSTRAINT FK_Student_Class FOREIGN KEY (ClassID) REFERENCES [dbo].[Class](ClassID)
)
GO

-- Class Table
CREATE TABLE [dbo].[Class] (
    [ClassID] INT IDENTITY(1,1) PRIMARY KEY,
    [ClassName] NVARCHAR(50) NOT NULL,
    [SchoolYear] NVARCHAR(20),
    [TeacherName] NVARCHAR(100),
    [Description] NVARCHAR(255),
    [TeacherID] INT,
    CONSTRAINT FK_Class_Teacher FOREIGN KEY (TeacherID) REFERENCES [dbo].[Teacher](TeacherID)
)
GO

-- Medical Event Table
CREATE TABLE [dbo].[MedicalEvent] (
    [EventID] INT IDENTITY(1,1) PRIMARY KEY,
    [StudentID] INT,
    [EventType] NVARCHAR(100),
    [Description] NVARCHAR(MAX),
    [EventTime] DATETIME,
    [NurseID] INT,
    [Status] NVARCHAR(50),
    CONSTRAINT FK_MedicalEvent_Student FOREIGN KEY (StudentID) REFERENCES [dbo].[Student](StudentID),
    CONSTRAINT FK_MedicalEvent_Nurse FOREIGN KEY (NurseID) REFERENCES [dbo].[Nurse](NurseID)
)
GO

-- Health Profile Table
CREATE TABLE [dbo].[HealthProfile] (
    [ProfileID] INT IDENTITY(1,1) PRIMARY KEY,
    [StudentID] INT,
    [ChronicDisease] NVARCHAR(255),
    [VisionTest] NVARCHAR(50),
    [Allergy] NVARCHAR(255),
    [Weight] DECIMAL(5,2),
    [Height] DECIMAL(5,2),
    [LastCheckupDate] DATE,
    CONSTRAINT FK_HealthProfile_Student FOREIGN KEY (StudentID) REFERENCES [dbo].[Student](StudentID)
)
GO

-- Medicine Request Table
CREATE TABLE [dbo].[MedicineRequest] (
    [RequestID] INT IDENTITY(1,1) PRIMARY KEY,
    [Date] DATE,
    [RequestStatus] NVARCHAR(50),
    [StudentID] INT,
    [ParentID] INT,
    [Note] NVARCHAR(255),
    [ApprovedBy] INT,
    [ApprovalDate] DATE,
    CONSTRAINT FK_MedicineRequest_Student FOREIGN KEY (StudentID) REFERENCES [dbo].[Student](StudentID),
    CONSTRAINT FK_MedicineRequest_Parent FOREIGN KEY (ParentID) REFERENCES [dbo].[Parent](ParentID)
)
GO

-- Medicine Request Detail Table
CREATE TABLE [dbo].[MedicineRequestDetail] (
    [RequestDetailID] INT IDENTITY(1,1) PRIMARY KEY,
    [RequestID] INT,
    [RequestItemID] INT,
    [Quantity] INT,
    [DosageInstructions] NVARCHAR(255),
    [Time] NVARCHAR(50),
    CONSTRAINT FK_MedicineRequestDetail_Request FOREIGN KEY (RequestID) REFERENCES [dbo].[MedicineRequest](RequestID),
    CONSTRAINT FK_MedicineRequestDetail_Item FOREIGN KEY (RequestItemID) REFERENCES [dbo].[RequestItemList](RequestItemID)
)
GO

-- Request Item List Table
CREATE TABLE [dbo].[RequestItemList] (
    [RequestItemID] INT IDENTITY(1,1) PRIMARY KEY,
    [RequestItemName] NVARCHAR(255) NOT NULL,
    [Description] NVARCHAR(500)
)
GO

-- Medical Inventory Table
CREATE TABLE [dbo].[MedicalInventory] (
    [ItemID] INT IDENTITY(1,1) PRIMARY KEY,
    [ItemName] NVARCHAR(100) NOT NULL,
    [Category] NVARCHAR(50),
    [Quantity] INT,
    [Unit] NVARCHAR(20),
    [Description] NVARCHAR(255)
)
GO

-- Vaccination Event Table
CREATE TABLE [dbo].[VaccinationEvent] (
    [EventID] INT IDENTITY(1,1) PRIMARY KEY,
    [EventName] NVARCHAR(100) NOT NULL,
    [Date] DATE,
    [Location] NVARCHAR(255),
    [ManagerID] INT,
    [ClassID] INT,
    CONSTRAINT FK_VaccinationEvent_Manager FOREIGN KEY (ManagerID) REFERENCES [dbo].[ManagerAdmin](ManagerID),
    CONSTRAINT FK_VaccinationEvent_Class FOREIGN KEY (ClassID) REFERENCES [dbo].[Class](ClassID)
)
GO

-- Vaccine Record Table
CREATE TABLE [dbo].[VaccineRecord] (
    [VaccineRecordID] INT IDENTITY(1,1) PRIMARY KEY,
    [StudentID] INT,
    [VaccinationEventID] INT,
    [NurseID] INT,
    [VaccineName] NVARCHAR(100) NOT NULL,
    [InjectionDate] DATE,
    [Reaction] NVARCHAR(255),
    [FollowUpStatus] NVARCHAR(50),
    [InjectionSite] NVARCHAR(50),
    [NextDoseDate] DATE,
    CONSTRAINT FK_VaccineRecord_Student FOREIGN KEY (StudentID) REFERENCES [dbo].[Student](StudentID),
    CONSTRAINT FK_VaccineRecord_Event FOREIGN KEY (VaccinationEventID) REFERENCES [dbo].[VaccinationEvent](EventID),
    CONSTRAINT FK_VaccineRecord_Nurse FOREIGN KEY (NurseID) REFERENCES [dbo].[Nurse](NurseID)
)
GO

-- Parental Consent Table
CREATE TABLE [dbo].[ParentalConsent] (
    [ConsentID] INT IDENTITY(1,1) PRIMARY KEY,
    [StudentID] INT,
    [VaccinationEventID] INT,
    [ParentID] INT,
    [ConsentStatus] NVARCHAR(50) NOT NULL,
    [ConsentDate] DATE,
    [Note] NVARCHAR(255),
    CONSTRAINT FK_ParentalConsent_Student FOREIGN KEY (StudentID) REFERENCES [dbo].[Student](StudentID),
    CONSTRAINT FK_ParentalConsent_Event FOREIGN KEY (VaccinationEventID) REFERENCES [dbo].[VaccinationEvent](EventID),
    CONSTRAINT FK_ParentalConsent_Parent FOREIGN KEY (ParentID) REFERENCES [dbo].[Parent](ParentID)
)
GO

-- Notification Table
CREATE TABLE [dbo].[Notification] (
    [NotificationID] INT IDENTITY(1,1) PRIMARY KEY,
    [Title] NVARCHAR(100) NOT NULL,
    [Content] NVARCHAR(MAX) NOT NULL,
    [SentDate] DATETIME,
    [Status] NVARCHAR(50),
    [NotificationType] NVARCHAR(50),
    [VaccinationEventID] INT,
    [MedicalEventID] INT,
    CONSTRAINT FK_Notification_Event FOREIGN KEY (VaccinationEventID) REFERENCES [dbo].[VaccinationEvent](EventID),
    CONSTRAINT FK_Notification_MedicalEvent FOREIGN KEY (MedicalEventID) REFERENCES [dbo].[MedicalEvent](EventID)
)
GO

-- Parent Notification Table
CREATE TABLE [dbo].[ParentNotification] (
    [NotificationID] INT,
    [ParentID] INT,
    [IndividualSentDate] DATETIME,
    [IndividualStatus] NVARCHAR(50),
    PRIMARY KEY (NotificationID, ParentID),
    CONSTRAINT FK_ParentNotification_Notification FOREIGN KEY (NotificationID) REFERENCES [dbo].[Notification](NotificationID),
    CONSTRAINT FK_ParentNotification_Parent FOREIGN KEY (ParentID) REFERENCES [dbo].[Parent](ParentID)
)
GO

-- Dashboard Preferences Table
CREATE TABLE [dbo].[DashboardPreferences] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [UserID] INT,
    [PreferredWidgets] NVARCHAR(MAX),
    [Theme] NVARCHAR(50),
    [RefreshInterval] INT,
    CONSTRAINT FK_DashboardPreferences_User FOREIGN KEY (UserID) REFERENCES [dbo].[Account](UserID)
)
GO

-- Dashboard Notification Table
CREATE TABLE [dbo].[DashboardNotification] (
    [NotificationID] INT IDENTITY(1,1) PRIMARY KEY,
    [UserID] INT,
    [Title] NVARCHAR(100) NOT NULL,
    [Message] NVARCHAR(MAX) NOT NULL,
    [Type] NVARCHAR(50),
    [Priority] INT,
    [IsRead] BIT DEFAULT 0,
    [CreatedDate] DATETIME,
    CONSTRAINT FK_DashboardNotification_User FOREIGN KEY (UserID) REFERENCES [dbo].[Account](UserID)
)
GO

-- Audit Log Table
CREATE TABLE [dbo].[AuditLog] (
    [LogID] INT IDENTITY(1,1) PRIMARY KEY,
    [TableName] NVARCHAR(255),
    [Action] NVARCHAR(50),
    [UserID] INT,
    [ActionDate] DATETIME,
    [OldValue] NVARCHAR(MAX),
    [NewValue] NVARCHAR(MAX),
    CONSTRAINT FK_AuditLog_User FOREIGN KEY (UserID) REFERENCES [dbo].[Account](UserID)
)
GO

-- Blog Table
CREATE TABLE [dbo].[Blog] (
    [BlogID] INT IDENTITY(1,1) PRIMARY KEY,
    [Title] NVARCHAR(255) NOT NULL,
    [Content] NVARCHAR(MAX) NOT NULL,
    [ImageUrl] NVARCHAR(500),
    [AuthorID] INT,
    [CreatedDate] DATETIME,
    [UpdatedDate] DATETIME,
    [IsPublished] BIT,
    CONSTRAINT FK_Blog_Author FOREIGN KEY (AuthorID) REFERENCES [dbo].[Account](UserID)
)
GO

-- Create Indexes for better performance
CREATE INDEX IX_Student_ParentID ON [dbo].[Student] (ParentID)
CREATE INDEX IX_Student_ClassID ON [dbo].[Student] (ClassID)
CREATE INDEX IX_MedicalEvent_StudentID ON [dbo].[MedicalEvent] (StudentID)
CREATE INDEX IX_HealthProfile_StudentID ON [dbo].[HealthProfile] (StudentID)
CREATE INDEX IX_MedicineRequest_StudentID ON [dbo].[MedicineRequest] (StudentID)
CREATE INDEX IX_VaccineRecord_StudentID ON [dbo].[VaccineRecord] (StudentID)
CREATE INDEX IX_ParentalConsent_StudentID ON [dbo].[ParentalConsent] (StudentID)
CREATE INDEX IX_Notification_VaccinationEventID ON [dbo].[Notification] (VaccinationEventID)
CREATE INDEX IX_DashboardPreferences_UserID ON [dbo].[DashboardPreferences] (UserID)

-- Create Views for common queries
CREATE VIEW [dbo].[vw_StudentHealthStatus] AS
SELECT 
    s.StudentID,
    s.FullName,
    hp.ChronicDisease,
    hp.VisionTest,
    hp.Allergy,
    hp.Weight,
    hp.Height,
    hp.LastCheckupDate,
    COUNT(me.EventID) as MedicalEventsCount
FROM [dbo].[Student] s
LEFT JOIN [dbo].[HealthProfile] hp ON s.StudentID = hp.StudentID
LEFT JOIN [dbo].[MedicalEvent] me ON s.StudentID = me.StudentID
GROUP BY s.StudentID, s.FullName, hp.ChronicDisease, hp.VisionTest, hp.Allergy, hp.Weight, hp.Height, hp.LastCheckupDate
GO

-- Create Stored Procedures
CREATE PROCEDURE [dbo].[usp_GetStudentMedicalHistory]
    @StudentID INT
AS
BEGIN
    SELECT 
        me.EventID,
        me.EventType,
        me.Description,
        me.EventTime,
        me.Status,
        n.FullName as NurseName
    FROM [dbo].[MedicalEvent] me
    LEFT JOIN [dbo].[Nurse] n ON me.NurseID = n.NurseID
    WHERE me.StudentID = @StudentID
    ORDER BY me.EventTime DESC
END
GO

-- Create Functions
CREATE FUNCTION [dbo].[fn_GetVaccinationStatus] (@StudentID INT)
RETURNS TABLE
AS
RETURN
(
    SELECT 
        v.VaccineName,
        v.InjectionDate,
        v.Reaction,
        v.FollowUpStatus
    FROM [dbo].[VaccineRecord] v
    WHERE v.StudentID = @StudentID
)
GO

-- Create Triggers for audit logging
CREATE TRIGGER [dbo].[trg_Student_Audit] 
ON [dbo].[Student]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @Action CHAR(1)
    SET @Action = CASE 
        WHEN EXISTS(SELECT * FROM inserted) AND EXISTS(SELECT * FROM deleted) THEN 'U'
        WHEN EXISTS(SELECT * FROM inserted) THEN 'I'
        ELSE 'D'
    END

    INSERT INTO [dbo].[AuditLog] (TableName, Action, UserID, ActionDate, OldValue, NewValue)
    SELECT 
        'Student',
        @Action,
        COALESCE(i.UserID, d.UserID),
        GETDATE(),
        (SELECT * FROM deleted FOR JSON AUTO),
        (SELECT * FROM inserted FOR JSON AUTO)
    FROM inserted i
    FULL OUTER JOIN deleted d ON i.StudentID = d.StudentID
END
GO

-- Add default data
INSERT INTO [dbo].[Account] (Username, PasswordHash, Role, Active)
VALUES 
    ('admin', 'admin_hash', 'Admin', 1),
    ('nurse1', 'nurse_hash', 'Nurse', 1),
    ('parent1', 'parent_hash', 'Parent', 1)
GO

INSERT INTO [dbo].[Nurse] (UserID, FullName, Gender, DateOfBirth, Phone)
VALUES 
    (2, 'Nurse One', 'F', '1980-01-01', '1234567890')
GO

INSERT INTO [dbo].[Parent] (UserID, FullName, Gender, DateOfBirth, Address, Phone)
VALUES 
    (3, 'Parent One', 'M', '1975-01-01', '123 Street', '0987654321')
GO

-- Add foreign key constraints for tables that reference Account
ALTER TABLE [dbo].[Nurse] ADD CONSTRAINT FK_Nurse_Account FOREIGN KEY (UserID) REFERENCES [dbo].[Account](UserID)
ALTER TABLE [dbo].[Parent] ADD CONSTRAINT FK_Parent_Account FOREIGN KEY (UserID) REFERENCES [dbo].[Account](UserID)
ALTER TABLE [dbo].[ManagerAdmin] ADD CONSTRAINT FK_ManagerAdmin_Account FOREIGN KEY (UserID) REFERENCES [dbo].[Account](UserID)
GO

-- Add indexes for foreign key columns
CREATE INDEX IX_Nurse_UserID ON [dbo].[Nurse] (UserID)
CREATE INDEX IX_Parent_UserID ON [dbo].[Parent] (UserID)
CREATE INDEX IX_ManagerAdmin_UserID ON [dbo].[ManagerAdmin] (UserID)
GO

-- Add constraints for required fields
ALTER TABLE [dbo].[Student] ADD CONSTRAINT CK_Student_Gender CHECK (Gender IN ('M', 'F'))
ALTER TABLE [dbo].[Nurse] ADD CONSTRAINT CK_Nurse_Gender CHECK (Gender IN ('M', 'F'))
ALTER TABLE [dbo].[Parent] ADD CONSTRAINT CK_Parent_Gender CHECK (Gender IN ('M', 'F'))
GO

-- Add extended properties for documentation
EXEC sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = 'Main user account table', 
    @level0type = N'SCHEMA', @level0name = 'dbo', 
    @level1type = N'TABLE', @level1name = 'Account'
GO

EXEC sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = 'Stores student medical records', 
    @level0type = N'SCHEMA', @level0name = 'dbo', 
    @level1type = N'TABLE', @level1name = 'MedicalEvent'
GO

-- Create partitioning for large tables if needed
CREATE PARTITION FUNCTION pf_MedicalEvent (DATE)
    AS RANGE RIGHT FOR VALUES ('2023-01-01', '2024-01-01', '2025-01-01')
GO

CREATE PARTITION SCHEME ps_MedicalEvent
    AS PARTITION pf_MedicalEvent
    TO ([PRIMARY], [PRIMARY], [PRIMARY], [PRIMARY])
GO

-- Add partitioning to MedicalEvent table
ALTER TABLE [dbo].[MedicalEvent]
    ADD CONSTRAINT PK_MedicalEvent PRIMARY KEY NONCLUSTERED (EventID)
GO

CREATE CLUSTERED INDEX IX_MedicalEvent_Partition
    ON [dbo].[MedicalEvent] (EventTime)
    ON ps_MedicalEvent(EventTime)
GO

-- Add filegroups for better performance
ALTER DATABASE [SchoolMedicalDB] ADD FILEGROUP [MEDICAL_DATA]
ALTER DATABASE [SchoolMedicalDB] ADD FILEGROUP [INDEX_DATA]
GO

-- Add files to filegroups
ALTER DATABASE [SchoolMedicalDB] ADD FILE
(
    NAME = N'MedicalDataFile',
    FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\SchoolMedicalDB_MedicalData.ndf',
    SIZE = 100MB,
    MAXSIZE = 500MB,
    FILEGROWTH = 50MB
)
TO FILEGROUP [MEDICAL_DATA]
GO

ALTER DATABASE [SchoolMedicalDB] ADD FILE
(
    NAME = N'IndexDataFile',
    FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\SchoolMedicalDB_IndexData.ndf',
    SIZE = 50MB,
    MAXSIZE = 200MB,
    FILEGROWTH = 25MB
)
TO FILEGROUP [INDEX_DATA]
GO

-- Add indexes to filegroups
CREATE INDEX IX_MedicalEvent_StudentID ON [dbo].[MedicalEvent] (StudentID)
    ON [MEDICAL_DATA]
GO

CREATE INDEX IX_MedicalEvent_EventTime ON [dbo].[MedicalEvent] (EventTime)
    ON [INDEX_DATA]
GO
