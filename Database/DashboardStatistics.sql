CREATE TABLE [dbo].[DashboardStatistics] (
    [StatisticID] INT IDENTITY(1,1) PRIMARY KEY,
    [StatisticDate] DATE NOT NULL,
    [TotalStudents] INT,
    [ActiveCases] INT,
    [NewCases] INT,
    [VaccinationRate] DECIMAL(5,2),
    [HealthCheckups] INT,
    [MedicalEvents] INT,
    [MedicineRequests] INT,
    [LastUpdated] DATETIME2 DEFAULT GETDATE(),
    CONSTRAINT UQ_StatisticDate UNIQUE (StatisticDate)
)

-- Add indexes for better performance
CREATE INDEX IX_DashboardStatistics_StatisticDate ON [dbo].[DashboardStatistics] (StatisticDate)
CREATE INDEX IX_DashboardStatistics_LastUpdated ON [dbo].[DashboardStatistics] (LastUpdated)

-- Add dashboard preferences table
CREATE TABLE [dbo].[DashboardPreferences] (
    [UserID] INT PRIMARY KEY,
    [PreferredWidgets] NVARCHAR(MAX),
    [Theme] NVARCHAR(50),
    [RefreshInterval] INT,
    [LastUpdated] DATETIME2 DEFAULT GETDATE(),
    CONSTRAINT FK_DashboardPreferences_User FOREIGN KEY (UserID) REFERENCES [dbo].[Account](UserID)
)

-- Add dashboard notifications table
CREATE TABLE [dbo].[DashboardNotifications] (
    [NotificationID] INT IDENTITY(1,1) PRIMARY KEY,
    [UserID] INT,
    [Title] NVARCHAR(255),
    [Message] NVARCHAR(MAX),
    [Type] NVARCHAR(50),
    [Priority] INT,
    [IsRead] BIT DEFAULT 0,
    [CreatedDate] DATETIME2 DEFAULT GETDATE(),
    CONSTRAINT FK_DashboardNotifications_User FOREIGN KEY (UserID) REFERENCES [dbo].[Account](UserID)
)

-- Add indexes
CREATE INDEX IX_DashboardNotifications_UserID ON [dbo].[DashboardNotifications] (UserID)
CREATE INDEX IX_DashboardNotifications_CreatedDate ON [dbo].[DashboardNotifications] (CreatedDate)
CREATE INDEX IX_DashboardNotifications_IsRead ON [dbo].[DashboardNotifications] (IsRead)
