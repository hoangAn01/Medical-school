USE [master]
GO
/****** Object:  Database [MyWebAppDB]    Script Date: 6/22/2025 12:23:17 AM ******/
CREATE DATABASE [MyWebAppDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'MyWebAppDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\MyWebAppDB.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'MyWebAppDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\MyWebAppDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [MyWebAppDB] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [MyWebAppDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [MyWebAppDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [MyWebAppDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [MyWebAppDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [MyWebAppDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [MyWebAppDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [MyWebAppDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [MyWebAppDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [MyWebAppDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [MyWebAppDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [MyWebAppDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [MyWebAppDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [MyWebAppDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [MyWebAppDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [MyWebAppDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [MyWebAppDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [MyWebAppDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [MyWebAppDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [MyWebAppDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [MyWebAppDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [MyWebAppDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [MyWebAppDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [MyWebAppDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [MyWebAppDB] SET RECOVERY FULL 
GO
ALTER DATABASE [MyWebAppDB] SET  MULTI_USER 
GO
ALTER DATABASE [MyWebAppDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [MyWebAppDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [MyWebAppDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [MyWebAppDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [MyWebAppDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [MyWebAppDB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'MyWebAppDB', N'ON'
GO
ALTER DATABASE [MyWebAppDB] SET QUERY_STORE = ON
GO
ALTER DATABASE [MyWebAppDB] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [MyWebAppDB]
GO
/****** Object:  Table [dbo].[Account]    Script Date: 6/22/2025 12:23:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Account](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](100) NOT NULL,
	[PasswordHash] [nvarchar](255) NOT NULL,
	[Role] [nvarchar](50) NOT NULL,
	[Active] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Allergen]    Script Date: 6/22/2025 12:23:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Allergen](
	[AllergenID] [int] IDENTITY(1,1) NOT NULL,
	[AllergenName] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[AllergenID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AuditLogs]    Script Date: 6/22/2025 12:23:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AuditLogs](
	[LogID] [int] IDENTITY(1,1) NOT NULL,
	[TableName] [nvarchar](255) NULL,
	[Action] [nvarchar](50) NULL,
	[UserID] [int] NULL,
	[ActionDate] [datetime2](7) NOT NULL,
	[OldValue] [nvarchar](max) NULL,
	[NewValue] [nvarchar](max) NULL,
 CONSTRAINT [PK_AuditLogs] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Blog]    Script Date: 6/22/2025 12:23:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Blog](
	[BlogID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](255) NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[ImageUrl] [nvarchar](500) NULL,
	[CreatedDate] [datetime] NULL,
	[UpdatedDate] [datetime] NULL,
	[IsPublished] [bit] NULL,
	[AuthorID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[BlogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Class]    Script Date: 6/22/2025 12:23:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Class](
	[ClassID] [int] IDENTITY(1,1) NOT NULL,
	[ClassName] [nvarchar](50) NOT NULL,
	[SchoolYear] [nvarchar](20) NULL,
	[TeacherID] [int] NULL,
	[Description] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[ClassID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HealthProfile]    Script Date: 6/22/2025 12:23:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HealthProfile](
	[ProfileID] [int] IDENTITY(1,1) NOT NULL,
	[StudentID] [int] NOT NULL,
	[ChronicDisease] [nvarchar](255) NULL,
	[VisionTest] [nvarchar](50) NULL,
	[Allergy] [nvarchar](255) NULL,
	[Weight] [decimal](5, 2) NULL,
	[Height] [decimal](5, 2) NULL,
	[LastCheckupDate] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProfileID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[HealthReport]    Script Date: 6/22/2025 12:23:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[HealthReport](
	[ReportID] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Description] [nvarchar](255) NULL,
	[StudentID] [int] NOT NULL,
	[NurseID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ReportID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ManagerAdmin]    Script Date: 6/22/2025 12:23:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ManagerAdmin](
	[ManagerID] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Gender] [char](1) NULL,
	[DateOfBirth] [date] NULL,
	[Address] [nvarchar](255) NULL,
	[UserID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ManagerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MedicalEvent]    Script Date: 6/22/2025 12:23:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MedicalEvent](
	[EventID] [int] IDENTITY(1,1) NOT NULL,
	[StudentID] [int] NOT NULL,
	[EventType] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](255) NULL,
	[EventTime] [datetime] NOT NULL,
	[NurseID] [int] NULL,
	[Status] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[EventID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MedicalEventInventory]    Script Date: 6/22/2025 12:23:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MedicalEventInventory](
	[EventInventoryID] [int] IDENTITY(1,1) NOT NULL,
	[EventID] [int] NULL,
	[ItemID] [int] NULL,
	[QuantityUsed] [int] NOT NULL,
	[UsedTime] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[EventInventoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MedicalInventory]    Script Date: 6/22/2025 12:23:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MedicalInventory](
	[ItemID] [int] IDENTITY(1,1) NOT NULL,
	[ItemName] [nvarchar](100) NOT NULL,
	[Category] [nvarchar](50) NULL,
	[Quantity] [int] NULL,
	[Unit] [nvarchar](20) NULL,
	[Description] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[ItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MedicalUsage]    Script Date: 6/22/2025 12:23:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MedicalUsage](
	[UsageID] [int] IDENTITY(1,1) NOT NULL,
	[ItemID] [int] NOT NULL,
	[StudentID] [int] NOT NULL,
	[NurseID] [int] NULL,
	[UsageDate] [datetime] NOT NULL,
	[QuantityUsed] [int] NOT NULL,
	[Purpose] [nvarchar](255) NULL,
	[RequestID] [int] NULL,
	[RequestDetailID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[UsageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MedicineRequest]    Script Date: 6/22/2025 12:23:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MedicineRequest](
	[RequestID] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime] NOT NULL,
	[RequestStatus] [nvarchar](50) NULL,
	[StudentID] [int] NOT NULL,
	[ParentID] [int] NULL,
	[Note] [nvarchar](255) NULL,
	[ApprovedBy] [int] NULL,
	[ApprovalDate] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[RequestID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MedicineRequestDetail]    Script Date: 6/22/2025 12:23:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MedicineRequestDetail](
	[RequestDetailID] [int] IDENTITY(1,1) NOT NULL,
	[RequestID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[DosageInstructions] [nvarchar](255) NULL,
	[Time] [nvarchar](10) NULL,
	[RequestItemID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[RequestDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Notification]    Script Date: 6/22/2025 12:23:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notification](
	[NotificationID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[SentDate] [datetime] NOT NULL,
	[Status] [nvarchar](50) NULL,
	[NotificationType] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[NotificationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Nurse]    Script Date: 6/22/2025 12:23:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Nurse](
	[NurseID] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Gender] [char](1) NULL,
	[DateOfBirth] [date] NULL,
	[Phone] [nvarchar](20) NULL,
	[UserID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[NurseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Parent]    Script Date: 6/22/2025 12:23:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Parent](
	[ParentID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Gender] [char](1) NULL,
	[DateOfBirth] [date] NULL,
	[Address] [nvarchar](255) NULL,
	[Phone] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[ParentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ParentalConsent]    Script Date: 6/22/2025 12:23:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ParentalConsent](
	[ConsentID] [int] IDENTITY(1,1) NOT NULL,
	[StudentID] [int] NOT NULL,
	[VaccinationEventID] [int] NOT NULL,
	[ParentID] [int] NOT NULL,
	[ConsentStatus] [nvarchar](50) NOT NULL,
	[ConsentDate] [datetime] NULL,
	[Note] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[ConsentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ParentNotification]    Script Date: 6/22/2025 12:23:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ParentNotification](
	[NotificationID] [int] NOT NULL,
	[ParentID] [int] NOT NULL,
	[IndividualSentDate] [datetime] NOT NULL,
	[IndividualStatus] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[NotificationID] ASC,
	[ParentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RequestItemList]    Script Date: 6/22/2025 12:23:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestItemList](
	[RequestItemID] [int] IDENTITY(1,1) NOT NULL,
	[RequestItemName] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[RequestItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SchoolCheckup]    Script Date: 6/22/2025 12:23:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SchoolCheckup](
	[CheckupID] [int] IDENTITY(1,1) NOT NULL,
	[StudentID] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[Result] [nvarchar](255) NULL,
	[Weight] [decimal](5, 2) NULL,
	[Height] [decimal](5, 2) NULL,
	[BloodPressure] [nvarchar](20) NULL,
	[VisionLeft] [nvarchar](20) NULL,
	[VisionRight] [nvarchar](20) NULL,
PRIMARY KEY CLUSTERED 
(
	[CheckupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Student]    Script Date: 6/22/2025 12:23:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Student](
	[StudentID] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Gender] [char](1) NULL,
	[DateOfBirth] [date] NULL,
	[ClassID] [int] NULL,
	[ParentID] [int] NULL,
	[UserID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[StudentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StudentAllergy]    Script Date: 6/22/2025 12:23:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StudentAllergy](
	[StudentID] [int] NOT NULL,
	[AllergenID] [int] NOT NULL,
	[Reaction] [nvarchar](255) NULL,
	[Severity] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[StudentID] ASC,
	[AllergenID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Teacher]    Script Date: 6/22/2025 12:23:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teacher](
	[TeacherID] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](100) NOT NULL,
	[Gender] [char](1) NULL,
	[DateOfBirth] [date] NULL,
	[Phone] [nvarchar](20) NULL,
	[Email] [nvarchar](100) NULL,
	[Address] [nvarchar](255) NULL,
	[HireDate] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[TeacherID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VaccinationEvent]    Script Date: 6/22/2025 12:23:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VaccinationEvent](
	[EventID] [int] IDENTITY(1,1) NOT NULL,
	[EventName] [nvarchar](100) NOT NULL,
	[Date] [datetime] NOT NULL,
	[Location] [nvarchar](255) NULL,
	[ManagerID] [int] NULL,
	[ClassID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[EventID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VaccineRecord]    Script Date: 6/22/2025 12:23:17 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VaccineRecord](
	[VaccineRecordID] [int] IDENTITY(1,1) NOT NULL,
	[StudentID] [int] NOT NULL,
	[VaccinationEventID] [int] NULL,
	[NurseID] [int] NULL,
	[VaccineName] [nvarchar](100) NOT NULL,
	[InjectionDate] [datetime] NOT NULL,
	[Reaction] [nvarchar](255) NULL,
	[FollowUpStatus] [nvarchar](50) NULL,
	[InjectionSite] [nvarchar](50) NULL,
	[NextDoseDate] [date] NULL,
PRIMARY KEY CLUSTERED 
(
	[VaccineRecordID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Account] ON 

INSERT [dbo].[Account] ([UserID], [Username], [PasswordHash], [Role], [Active]) VALUES (1, N'admin1', N'$2a$12$pQcYYIDYV05MKGslMDzYz.I8M5pb.0XF5z1.RTWR66NYaM65GiMI6', N'Admin', 1)
INSERT [dbo].[Account] ([UserID], [Username], [PasswordHash], [Role], [Active]) VALUES (2, N'nurse1', N'$2a$11$wTMUNdVk4jHWPVrnTU3Fi.uIqG34.aYXdLnNMGYVvMia9IoWr4tPW', N'Nurse', 1)
INSERT [dbo].[Account] ([UserID], [Username], [PasswordHash], [Role], [Active]) VALUES (3, N'nurse2', N'$2a$12$pQcYYIDYV05MKGslMDzYz.I8M5pb.0XF5z1.RTWR66NYaM65GiMI6', N'Nurse', 1)
INSERT [dbo].[Account] ([UserID], [Username], [PasswordHash], [Role], [Active]) VALUES (4, N'parent1', N'$2a$12$vnc.2yY2j5lxhAYtRe8V.u1EB3VYJ7tKfnJbccOYhrcU6K9z2rPYa', N'Parent', 1)
INSERT [dbo].[Account] ([UserID], [Username], [PasswordHash], [Role], [Active]) VALUES (5, N'parent2', N'hash_parent2', N'Parent', 0)
INSERT [dbo].[Account] ([UserID], [Username], [PasswordHash], [Role], [Active]) VALUES (6, N'hola133', N'$2a$11$FVtForl9Bft9HsZzrqkWL.MoTLd3jddfh8KRSuQ.clr2E66brZXYu', N'parent', 1)
INSERT [dbo].[Account] ([UserID], [Username], [PasswordHash], [Role], [Active]) VALUES (7, N'uia1', N'$2a$11$JLbY5L9HzjbTVdHV72k0mOzUnEICukSk95okKKY2WRxTEC5bD1OWG', N'string', 0)
SET IDENTITY_INSERT [dbo].[Account] OFF
GO
SET IDENTITY_INSERT [dbo].[Allergen] ON 

INSERT [dbo].[Allergen] ([AllergenID], [AllergenName], [Description]) VALUES (1, N'Phấn hoa', N'Dị ứng phấn hoa')
INSERT [dbo].[Allergen] ([AllergenID], [AllergenName], [Description]) VALUES (2, N'Hải sản', N'Dị ứng hải sản')
INSERT [dbo].[Allergen] ([AllergenID], [AllergenName], [Description]) VALUES (3, N'Sữa', N'Dị ứng sữa')
INSERT [dbo].[Allergen] ([AllergenID], [AllergenName], [Description]) VALUES (4, N'Bụi nhà', N'Dị ứng bụi nhà')
INSERT [dbo].[Allergen] ([AllergenID], [AllergenName], [Description]) VALUES (5, N'Kháng sinh', N'Dị ứng kháng sinh')
SET IDENTITY_INSERT [dbo].[Allergen] OFF
GO
SET IDENTITY_INSERT [dbo].[AuditLogs] ON 

INSERT [dbo].[AuditLogs] ([LogID], [TableName], [Action], [UserID], [ActionDate], [OldValue], [NewValue]) VALUES (1, N'Account', N'Modified', NULL, CAST(N'2025-06-21T19:50:57.3755862' AS DateTime2), N'{"UserID":2,"Active":true,"PasswordHash":"$2a$11$l7PcuHnB2JWqJIl90YPJPecOlGpFi1MgBXX9.uznm.ePv44LhgVfy","Role":"string","Username":"nurse1"}', N'{"UserID":2,"Active":true,"PasswordHash":"$2a$11$wTMUNdVk4jHWPVrnTU3Fi.uIqG34.aYXdLnNMGYVvMia9IoWr4tPW","Role":"Nurse","Username":"nurse1"}')
INSERT [dbo].[AuditLogs] ([LogID], [TableName], [Action], [UserID], [ActionDate], [OldValue], [NewValue]) VALUES (2, N'HealthProfile', N'Modified', 1, CAST(N'2025-06-21T19:57:56.9953701' AS DateTime2), N'{"ProfileID":1,"Allergy":"Phấn hoa","ChronicDisease":"Không","Height":130.00,"LastCheckupDate":"2023-09-01T00:00:00","StudentID":1,"VisionTest":"10/10","Weight":30.50}', N'{"ProfileID":1,"Allergy":"ko","ChronicDisease":"strong","Height":10.0,"LastCheckupDate":"2025-06-21T12:57:20.114Z","StudentID":1,"VisionTest":"100d","Weight":10.0}')
INSERT [dbo].[AuditLogs] ([LogID], [TableName], [Action], [UserID], [ActionDate], [OldValue], [NewValue]) VALUES (3, N'MedicalEvent', N'Added', 1, CAST(N'2025-06-21T20:06:55.5911544' AS DateTime2), NULL, N'{"EventID":-2147482647,"Description":"xước dầu","EventTime":"2025-06-21T13:06:24.698Z","EventType":"tai nan","NurseID":1,"Status":"Đang xử lý","StudentID":1}')
INSERT [dbo].[AuditLogs] ([LogID], [TableName], [Action], [UserID], [ActionDate], [OldValue], [NewValue]) VALUES (4, N'Notification', N'Added', 1, CAST(N'2025-06-21T20:06:56.1597780' AS DateTime2), NULL, N'{"NotificationID":-2147482647,"Content":"Con bạn Nguyễn Minh K gặp sự cố: tai nan - xước dầu vào lúc 21/06/2025 13:06","NotificationType":"MEDICAL_EVENT","SentDate":"2025-06-21T20:06:56.0684541+07:00","Status":"Published","Title":"Thông báo sự cố y tế"}')
INSERT [dbo].[AuditLogs] ([LogID], [TableName], [Action], [UserID], [ActionDate], [OldValue], [NewValue]) VALUES (5, N'ParentNotification', N'Added', 1, CAST(N'2025-06-21T20:06:56.2675415' AS DateTime2), NULL, N'{"NotificationID":7,"ParentID":1,"IndividualSentDate":"2025-06-21T20:06:56.2166037+07:00","IndividualStatus":"Sent"}')
INSERT [dbo].[AuditLogs] ([LogID], [TableName], [Action], [UserID], [ActionDate], [OldValue], [NewValue]) VALUES (7, N'Notification', N'Deleted', 1, CAST(N'2025-06-21T20:08:40.9948663' AS DateTime2), N'{"NotificationID":5,"Content":"chung tay diệt bãi gửi xe","NotificationType":"GENERAL","SentDate":"2025-06-19T22:51:01.097","Status":"Published","Title":"thong điệp 4k"}', NULL)
INSERT [dbo].[AuditLogs] ([LogID], [TableName], [Action], [UserID], [ActionDate], [OldValue], [NewValue]) VALUES (8, N'Account', N'Added', 1, CAST(N'2025-06-21T20:09:47.1112047' AS DateTime2), NULL, N'{"UserID":-2147482647,"Active":true,"PasswordHash":"$2a$11$e1/rr5wnairF7VQQrG3IMe64VC.Z6Ty/2GsYObla.KhNF4yiywODm","Role":"Parent","Username":"uia1"}')
INSERT [dbo].[AuditLogs] ([LogID], [TableName], [Action], [UserID], [ActionDate], [OldValue], [NewValue]) VALUES (9, N'Account', N'Modified', 1, CAST(N'2025-06-21T20:10:07.4278855' AS DateTime2), N'{"UserID":7,"Active":true,"PasswordHash":"$2a$11$e1/rr5wnairF7VQQrG3IMe64VC.Z6Ty/2GsYObla.KhNF4yiywODm","Role":"Parent","Username":"uia1"}', N'{"UserID":7,"Active":true,"PasswordHash":"$2a$11$JLbY5L9HzjbTVdHV72k0mOzUnEICukSk95okKKY2WRxTEC5bD1OWG","Role":"string","Username":"uia1"}')
INSERT [dbo].[AuditLogs] ([LogID], [TableName], [Action], [UserID], [ActionDate], [OldValue], [NewValue]) VALUES (10, N'Account', N'Modified', 1, CAST(N'2025-06-21T20:10:14.9925730' AS DateTime2), N'{"UserID":7,"Active":true,"PasswordHash":"$2a$11$JLbY5L9HzjbTVdHV72k0mOzUnEICukSk95okKKY2WRxTEC5bD1OWG","Role":"string","Username":"uia1"}', N'{"UserID":7,"Active":false,"PasswordHash":"$2a$11$JLbY5L9HzjbTVdHV72k0mOzUnEICukSk95okKKY2WRxTEC5bD1OWG","Role":"string","Username":"uia1"}')
INSERT [dbo].[AuditLogs] ([LogID], [TableName], [Action], [UserID], [ActionDate], [OldValue], [NewValue]) VALUES (11, N'HealthProfile', N'Modified', 1, CAST(N'2025-06-21T20:18:16.1485281' AS DateTime2), N'{"ProfileID":5,"Allergy":"Kháng sinh","ChronicDisease":"Không","Height":132.00,"LastCheckupDate":"2023-09-01T00:00:00","StudentID":5,"VisionTest":"10/10","Weight":31.00}', N'{"ProfileID":5,"Allergy":"ko co","ChronicDisease":"sting vàng","Height":101.0,"LastCheckupDate":"2025-06-21T13:17:19.269Z","StudentID":5,"VisionTest":"100","Weight":40.0}')
SET IDENTITY_INSERT [dbo].[AuditLogs] OFF
GO
SET IDENTITY_INSERT [dbo].[Blog] ON 

INSERT [dbo].[Blog] ([BlogID], [Title], [Content], [ImageUrl], [CreatedDate], [UpdatedDate], [IsPublished], [AuthorID]) VALUES (1, N'Phòng chống cúm học đường', N'Nội dung bài viết 1', NULL, CAST(N'2024-06-01T08:00:00.000' AS DateTime), NULL, 1, 1)
INSERT [dbo].[Blog] ([BlogID], [Title], [Content], [ImageUrl], [CreatedDate], [UpdatedDate], [IsPublished], [AuthorID]) VALUES (2, N'Tầm quan trọng của tiêm chủng', N'Nội dung bài viết 2', NULL, CAST(N'2024-06-02T08:00:00.000' AS DateTime), NULL, 1, 1)
INSERT [dbo].[Blog] ([BlogID], [Title], [Content], [ImageUrl], [CreatedDate], [UpdatedDate], [IsPublished], [AuthorID]) VALUES (3, N'Chăm sóc sức khỏe học sinh', N'Nội dung bài viết 3', NULL, CAST(N'2024-06-03T08:00:00.000' AS DateTime), NULL, 1, 1)
INSERT [dbo].[Blog] ([BlogID], [Title], [Content], [ImageUrl], [CreatedDate], [UpdatedDate], [IsPublished], [AuthorID]) VALUES (4, N'Phòng tránh dị ứng', N'Nội dung bài viết 4', NULL, CAST(N'2024-06-04T08:00:00.000' AS DateTime), NULL, 1, 1)
INSERT [dbo].[Blog] ([BlogID], [Title], [Content], [ImageUrl], [CreatedDate], [UpdatedDate], [IsPublished], [AuthorID]) VALUES (5, N'Bổ sung vitamin cho trẻ', N'Nội dung bài viết 5', NULL, CAST(N'2024-06-05T08:00:00.000' AS DateTime), NULL, 1, 1)
INSERT [dbo].[Blog] ([BlogID], [Title], [Content], [ImageUrl], [CreatedDate], [UpdatedDate], [IsPublished], [AuthorID]) VALUES (6, N'tiem mao', N'sui mao ga', N'string', CAST(N'2025-06-19T22:13:15.613' AS DateTime), NULL, 0, 1)
SET IDENTITY_INSERT [dbo].[Blog] OFF
GO
SET IDENTITY_INSERT [dbo].[Class] ON 

INSERT [dbo].[Class] ([ClassID], [ClassName], [SchoolYear], [TeacherID], [Description]) VALUES (1, N'1A', N'2023-2024', 1, N'Lớp 1A')
INSERT [dbo].[Class] ([ClassID], [ClassName], [SchoolYear], [TeacherID], [Description]) VALUES (2, N'1B', N'2023-2024', 2, N'Lớp 1B')
INSERT [dbo].[Class] ([ClassID], [ClassName], [SchoolYear], [TeacherID], [Description]) VALUES (3, N'2A', N'2023-2024', 3, N'Lớp 2A')
INSERT [dbo].[Class] ([ClassID], [ClassName], [SchoolYear], [TeacherID], [Description]) VALUES (4, N'2B', N'2023-2024', 4, N'Lớp 2B')
INSERT [dbo].[Class] ([ClassID], [ClassName], [SchoolYear], [TeacherID], [Description]) VALUES (5, N'3A', N'2023-2024', 5, N'Lớp 3A')
SET IDENTITY_INSERT [dbo].[Class] OFF
GO
SET IDENTITY_INSERT [dbo].[HealthProfile] ON 

INSERT [dbo].[HealthProfile] ([ProfileID], [StudentID], [ChronicDisease], [VisionTest], [Allergy], [Weight], [Height], [LastCheckupDate]) VALUES (1, 1, N'strong', N'100d', N'ko', CAST(10.00 AS Decimal(5, 2)), CAST(10.00 AS Decimal(5, 2)), CAST(N'2025-06-21' AS Date))
INSERT [dbo].[HealthProfile] ([ProfileID], [StudentID], [ChronicDisease], [VisionTest], [Allergy], [Weight], [Height], [LastCheckupDate]) VALUES (2, 2, N'Hen suyễn', N'9/10', N'Hải sản', CAST(28.00 AS Decimal(5, 2)), CAST(128.00 AS Decimal(5, 2)), CAST(N'2023-09-01' AS Date))
INSERT [dbo].[HealthProfile] ([ProfileID], [StudentID], [ChronicDisease], [VisionTest], [Allergy], [Weight], [Height], [LastCheckupDate]) VALUES (3, 3, N'Không', N'10/10', N'Sữa', CAST(32.00 AS Decimal(5, 2)), CAST(135.00 AS Decimal(5, 2)), CAST(N'2023-09-01' AS Date))
INSERT [dbo].[HealthProfile] ([ProfileID], [StudentID], [ChronicDisease], [VisionTest], [Allergy], [Weight], [Height], [LastCheckupDate]) VALUES (4, 4, N'Viêm mũi dị ứng', N'8/10', N'Bụi nhà', CAST(29.50 AS Decimal(5, 2)), CAST(129.00 AS Decimal(5, 2)), CAST(N'2023-09-01' AS Date))
INSERT [dbo].[HealthProfile] ([ProfileID], [StudentID], [ChronicDisease], [VisionTest], [Allergy], [Weight], [Height], [LastCheckupDate]) VALUES (5, 5, N'sting vàng', N'100', N'ko co', CAST(40.00 AS Decimal(5, 2)), CAST(101.00 AS Decimal(5, 2)), CAST(N'2025-06-21' AS Date))
SET IDENTITY_INSERT [dbo].[HealthProfile] OFF
GO
SET IDENTITY_INSERT [dbo].[HealthReport] ON 

INSERT [dbo].[HealthReport] ([ReportID], [Date], [Description], [StudentID], [NurseID]) VALUES (1, CAST(N'2024-06-11T08:00:00.000' AS DateTime), N'Khám sức khỏe định kỳ', 1, 1)
INSERT [dbo].[HealthReport] ([ReportID], [Date], [Description], [StudentID], [NurseID]) VALUES (2, CAST(N'2024-06-12T08:00:00.000' AS DateTime), N'Khám sức khỏe định kỳ', 2, 2)
INSERT [dbo].[HealthReport] ([ReportID], [Date], [Description], [StudentID], [NurseID]) VALUES (3, CAST(N'2024-06-13T08:00:00.000' AS DateTime), N'Khám sức khỏe định kỳ', 3, 1)
INSERT [dbo].[HealthReport] ([ReportID], [Date], [Description], [StudentID], [NurseID]) VALUES (4, CAST(N'2024-06-14T08:00:00.000' AS DateTime), N'Khám sức khỏe định kỳ', 4, 2)
INSERT [dbo].[HealthReport] ([ReportID], [Date], [Description], [StudentID], [NurseID]) VALUES (5, CAST(N'2024-06-15T08:00:00.000' AS DateTime), N'Khám sức khỏe định kỳ', 5, 1)
SET IDENTITY_INSERT [dbo].[HealthReport] OFF
GO
SET IDENTITY_INSERT [dbo].[ManagerAdmin] ON 

INSERT [dbo].[ManagerAdmin] ([ManagerID], [FullName], [Gender], [DateOfBirth], [Address], [UserID]) VALUES (1, N'Admin AnHNT', N'M', CAST(N'1970-01-01' AS Date), N'Hà Nội', 1)
SET IDENTITY_INSERT [dbo].[ManagerAdmin] OFF
GO
SET IDENTITY_INSERT [dbo].[MedicalEvent] ON 

INSERT [dbo].[MedicalEvent] ([EventID], [StudentID], [EventType], [Description], [EventTime], [NurseID], [Status]) VALUES (1, 2, N'tai nạn', N'xước đầu', CAST(N'2025-06-20T09:55:00.270' AS DateTime), 1, N'Đã xử lý')
INSERT [dbo].[MedicalEvent] ([EventID], [StudentID], [EventType], [Description], [EventTime], [NurseID], [Status]) VALUES (2, 2, N'Tai nạn', N'Té ngã', CAST(N'2024-06-02T09:00:00.000' AS DateTime), 2, N'Đã xử lý')
INSERT [dbo].[MedicalEvent] ([EventID], [StudentID], [EventType], [Description], [EventTime], [NurseID], [Status]) VALUES (3, 3, N'Tai nạn', N'Gãy răng', CAST(N'2024-06-03T10:00:00.000' AS DateTime), 1, N'Đang xử lý')
INSERT [dbo].[MedicalEvent] ([EventID], [StudentID], [EventType], [Description], [EventTime], [NurseID], [Status]) VALUES (4, 4, N'Khác', N'Đau bụng', CAST(N'2024-06-04T11:00:00.000' AS DateTime), 2, N'Đang xử lý')
INSERT [dbo].[MedicalEvent] ([EventID], [StudentID], [EventType], [Description], [EventTime], [NurseID], [Status]) VALUES (5, 5, N'Ốm', N'Cảm cúm', CAST(N'2024-06-05T12:00:00.000' AS DateTime), 1, N'Đang xử lý')
INSERT [dbo].[MedicalEvent] ([EventID], [StudentID], [EventType], [Description], [EventTime], [NurseID], [Status]) VALUES (6, 5, N'Tai nạn', N'Gãy Tay', CAST(N'2025-06-20T06:02:31.587' AS DateTime), 1, N'Chuyển sang viện')
INSERT [dbo].[MedicalEvent] ([EventID], [StudentID], [EventType], [Description], [EventTime], [NurseID], [Status]) VALUES (7, 5, N'string', N'string', CAST(N'2025-06-20T09:54:25.947' AS DateTime), 1, NULL)
INSERT [dbo].[MedicalEvent] ([EventID], [StudentID], [EventType], [Description], [EventTime], [NurseID], [Status]) VALUES (8, 5, N'string', N'string', CAST(N'2025-06-20T09:54:25.947' AS DateTime), 1, N'khong sao roi da co put')
INSERT [dbo].[MedicalEvent] ([EventID], [StudentID], [EventType], [Description], [EventTime], [NurseID], [Status]) VALUES (9, 2, N'Tai nan', N'ngã xe', CAST(N'2025-06-20T15:30:19.257' AS DateTime), 2, N'Đang xử lý')
INSERT [dbo].[MedicalEvent] ([EventID], [StudentID], [EventType], [Description], [EventTime], [NurseID], [Status]) VALUES (10, 1, N'tai nan', N'xước dầu', CAST(N'2025-06-21T13:06:24.697' AS DateTime), 1, N'Đang xử lý')
SET IDENTITY_INSERT [dbo].[MedicalEvent] OFF
GO
SET IDENTITY_INSERT [dbo].[MedicalEventInventory] ON 

INSERT [dbo].[MedicalEventInventory] ([EventInventoryID], [EventID], [ItemID], [QuantityUsed], [UsedTime]) VALUES (1, 2, 3, 1, CAST(N'2024-06-02T09:15:00.000' AS DateTime))
INSERT [dbo].[MedicalEventInventory] ([EventInventoryID], [EventID], [ItemID], [QuantityUsed], [UsedTime]) VALUES (2, 2, 4, 1, CAST(N'2024-06-02T09:15:00.000' AS DateTime))
INSERT [dbo].[MedicalEventInventory] ([EventInventoryID], [EventID], [ItemID], [QuantityUsed], [UsedTime]) VALUES (3, 1, 1, 2, CAST(N'2024-06-01T08:10:00.000' AS DateTime))
INSERT [dbo].[MedicalEventInventory] ([EventInventoryID], [EventID], [ItemID], [QuantityUsed], [UsedTime]) VALUES (4, 4, 3, 1, CAST(N'2024-06-04T11:10:00.000' AS DateTime))
INSERT [dbo].[MedicalEventInventory] ([EventInventoryID], [EventID], [ItemID], [QuantityUsed], [UsedTime]) VALUES (5, 4, 1, 1, CAST(N'2024-06-04T11:15:00.000' AS DateTime))
INSERT [dbo].[MedicalEventInventory] ([EventInventoryID], [EventID], [ItemID], [QuantityUsed], [UsedTime]) VALUES (6, 8, 1, 1, CAST(N'2025-06-20T21:21:26.747' AS DateTime))
SET IDENTITY_INSERT [dbo].[MedicalEventInventory] OFF
GO
SET IDENTITY_INSERT [dbo].[MedicalInventory] ON 

INSERT [dbo].[MedicalInventory] ([ItemID], [ItemName], [Category], [Quantity], [Unit], [Description]) VALUES (1, N'Paracetamol', N'Thuốc', 97, N'viên', N'Hạ sốt')
INSERT [dbo].[MedicalInventory] ([ItemID], [ItemName], [Category], [Quantity], [Unit], [Description]) VALUES (2, N'Vitamin C', N'Thuốc', 200, N'viên', N'Tăng sức đề kháng')
INSERT [dbo].[MedicalInventory] ([ItemID], [ItemName], [Category], [Quantity], [Unit], [Description]) VALUES (3, N'Băng gạc', N'Vật tư', 48, N'cuộn', N'Băng bó vết thương')
INSERT [dbo].[MedicalInventory] ([ItemID], [ItemName], [Category], [Quantity], [Unit], [Description]) VALUES (4, N'Cồn y tế', N'Vật tư', 19, N'chai', N'Sát trùng')
INSERT [dbo].[MedicalInventory] ([ItemID], [ItemName], [Category], [Quantity], [Unit], [Description]) VALUES (5, N'Khẩu trang', N'Vật tư', 500, N'cái', N'Phòng dịch')
SET IDENTITY_INSERT [dbo].[MedicalInventory] OFF
GO
SET IDENTITY_INSERT [dbo].[MedicalUsage] ON 

INSERT [dbo].[MedicalUsage] ([UsageID], [ItemID], [StudentID], [NurseID], [UsageDate], [QuantityUsed], [Purpose], [RequestID], [RequestDetailID]) VALUES (1, 1, 1, 1, CAST(N'2024-06-01T08:30:00.000' AS DateTime), 2, N'Hạ sốt', 1, 1)
INSERT [dbo].[MedicalUsage] ([UsageID], [ItemID], [StudentID], [NurseID], [UsageDate], [QuantityUsed], [Purpose], [RequestID], [RequestDetailID]) VALUES (2, 2, 2, 2, CAST(N'2024-06-02T09:30:00.000' AS DateTime), 1, N'Tăng sức đề kháng', 2, 2)
INSERT [dbo].[MedicalUsage] ([UsageID], [ItemID], [StudentID], [NurseID], [UsageDate], [QuantityUsed], [Purpose], [RequestID], [RequestDetailID]) VALUES (3, 3, 3, 1, CAST(N'2024-06-03T10:30:00.000' AS DateTime), 1, N'Băng bó', 3, 3)
INSERT [dbo].[MedicalUsage] ([UsageID], [ItemID], [StudentID], [NurseID], [UsageDate], [QuantityUsed], [Purpose], [RequestID], [RequestDetailID]) VALUES (4, 4, 4, 2, CAST(N'2024-06-04T11:30:00.000' AS DateTime), 1, N'Sát trùng', 4, 4)
INSERT [dbo].[MedicalUsage] ([UsageID], [ItemID], [StudentID], [NurseID], [UsageDate], [QuantityUsed], [Purpose], [RequestID], [RequestDetailID]) VALUES (5, 5, 5, 1, CAST(N'2024-06-05T12:30:00.000' AS DateTime), 2, N'Phòng dịch', 5, 5)
SET IDENTITY_INSERT [dbo].[MedicalUsage] OFF
GO
SET IDENTITY_INSERT [dbo].[MedicineRequest] ON 

INSERT [dbo].[MedicineRequest] ([RequestID], [Date], [RequestStatus], [StudentID], [ParentID], [Note], [ApprovedBy], [ApprovalDate]) VALUES (1, CAST(N'2024-06-01T08:00:00.000' AS DateTime), N'Chờ duyệt', 1, 1, N'Xin cấp thuốc hạ sốt', NULL, NULL)
INSERT [dbo].[MedicineRequest] ([RequestID], [Date], [RequestStatus], [StudentID], [ParentID], [Note], [ApprovedBy], [ApprovalDate]) VALUES (2, CAST(N'2024-06-02T09:00:00.000' AS DateTime), N'Đã duyệt', 2, 2, N'Xin cấp vitamin', 1, CAST(N'2024-06-03' AS Date))
INSERT [dbo].[MedicineRequest] ([RequestID], [Date], [RequestStatus], [StudentID], [ParentID], [Note], [ApprovedBy], [ApprovalDate]) VALUES (3, CAST(N'2024-06-03T10:00:00.000' AS DateTime), N'Chờ duyệt', 3, 4, N'Xin băng gạc', NULL, NULL)
INSERT [dbo].[MedicineRequest] ([RequestID], [Date], [RequestStatus], [StudentID], [ParentID], [Note], [ApprovedBy], [ApprovalDate]) VALUES (4, CAST(N'2024-06-04T11:00:00.000' AS DateTime), N'Đã duyệt', 4, 4, N'Xin cồn y tế', 2, CAST(N'2024-06-05' AS Date))
INSERT [dbo].[MedicineRequest] ([RequestID], [Date], [RequestStatus], [StudentID], [ParentID], [Note], [ApprovedBy], [ApprovalDate]) VALUES (5, CAST(N'2024-06-05T12:00:00.000' AS DateTime), N'Chờ duyệt', 5, 5, N'Xin khẩu trang', NULL, NULL)
INSERT [dbo].[MedicineRequest] ([RequestID], [Date], [RequestStatus], [StudentID], [ParentID], [Note], [ApprovedBy], [ApprovalDate]) VALUES (6, CAST(N'2025-06-21T00:00:00.000' AS DateTime), N'Pending', 4, 4, N'alo', NULL, NULL)
SET IDENTITY_INSERT [dbo].[MedicineRequest] OFF
GO
SET IDENTITY_INSERT [dbo].[MedicineRequestDetail] ON 

INSERT [dbo].[MedicineRequestDetail] ([RequestDetailID], [RequestID], [Quantity], [DosageInstructions], [Time], [RequestItemID]) VALUES (1, 1, 10, N'Uống 2 viên/ngày', N'Morning', 1)
INSERT [dbo].[MedicineRequestDetail] ([RequestDetailID], [RequestID], [Quantity], [DosageInstructions], [Time], [RequestItemID]) VALUES (2, 2, 20, N'Uống 1 viên/ngày', N'Morning', 4)
INSERT [dbo].[MedicineRequestDetail] ([RequestDetailID], [RequestID], [Quantity], [DosageInstructions], [Time], [RequestItemID]) VALUES (3, 3, 2, N'Dùng khi cần', N'Noon', 5)
INSERT [dbo].[MedicineRequestDetail] ([RequestDetailID], [RequestID], [Quantity], [DosageInstructions], [Time], [RequestItemID]) VALUES (4, 4, 1, N'Sát trùng vết thương', N'Noon', 7)
INSERT [dbo].[MedicineRequestDetail] ([RequestDetailID], [RequestID], [Quantity], [DosageInstructions], [Time], [RequestItemID]) VALUES (5, 5, 10, N'Dùng hàng ngày', N'Noon', 2)
INSERT [dbo].[MedicineRequestDetail] ([RequestDetailID], [RequestID], [Quantity], [DosageInstructions], [Time], [RequestItemID]) VALUES (6, 6, 123, N'2 lần 1 ngày', N'morning', 3)
SET IDENTITY_INSERT [dbo].[MedicineRequestDetail] OFF
GO
SET IDENTITY_INSERT [dbo].[Notification] ON 

INSERT [dbo].[Notification] ([NotificationID], [Title], [Content], [SentDate], [Status], [NotificationType]) VALUES (1, N'Thông báo tiêm chủng cúm', N'Kính gửi quý phụ huynh, trường học sẽ tổ chức tiêm chủng cúm vào ngày 10/06/2024. Vui lòng phản hồi trước ngày 05/06/2024.', CAST(N'2024-06-01T09:00:00.000' AS DateTime), N'Published', N'VACCINATION')
INSERT [dbo].[Notification] ([NotificationID], [Title], [Content], [SentDate], [Status], [NotificationType]) VALUES (2, N'Thông báo kết quả khám sức khỏe', N'Kính gửi quý phụ huynh, kết quả khám sức khỏe định kỳ của học sinh đã có. Vui lòng đăng nhập để xem chi tiết.', CAST(N'2024-06-06T14:00:00.000' AS DateTime), N'Published', N'CHECKUP')
INSERT [dbo].[Notification] ([NotificationID], [Title], [Content], [SentDate], [Status], [NotificationType]) VALUES (3, N'Nhắc nhở tiêm chủng bổ sung', N'Kính gửi quý phụ huynh, học sinh cần tiêm mũi bổ sung vào ngày 15/07/2024.', CAST(N'2024-07-01T10:00:00.000' AS DateTime), N'Published', N'CHECKUP')
INSERT [dbo].[Notification] ([NotificationID], [Title], [Content], [SentDate], [Status], [NotificationType]) VALUES (4, N'Sự kiện tiêm chủng toàn trường', N'Tiêm phong x', CAST(N'2025-06-17T17:00:18.663' AS DateTime), N'Published', N'VACCINATION')
INSERT [dbo].[Notification] ([NotificationID], [Title], [Content], [SentDate], [Status], [NotificationType]) VALUES (6, N'Thông báo sự cố y tế', N'Con bạn Lê Thị L gặp sự cố: Tai nan - ngã xe vào lúc 20/06/2025 15:30', CAST(N'2025-06-20T22:34:06.873' AS DateTime), N'Published', N'MEDICAL_EVENT')
INSERT [dbo].[Notification] ([NotificationID], [Title], [Content], [SentDate], [Status], [NotificationType]) VALUES (7, N'Thông báo sự cố y tế', N'Con bạn Nguyễn Minh K gặp sự cố: tai nan - xước dầu vào lúc 21/06/2025 13:06', CAST(N'2025-06-21T20:06:56.070' AS DateTime), N'Published', N'MEDICAL_EVENT')
SET IDENTITY_INSERT [dbo].[Notification] OFF
GO
SET IDENTITY_INSERT [dbo].[Nurse] ON 

INSERT [dbo].[Nurse] ([NurseID], [FullName], [Gender], [DateOfBirth], [Phone], [UserID]) VALUES (1, N'Nguyễn Thị Y Tá 1', N'F', CAST(N'1980-06-06' AS Date), N'0921111111', 2)
INSERT [dbo].[Nurse] ([NurseID], [FullName], [Gender], [DateOfBirth], [Phone], [UserID]) VALUES (2, N'Lê Văn Y Tá 2', N'M', CAST(N'1981-07-07' AS Date), N'0922222222', 3)
SET IDENTITY_INSERT [dbo].[Nurse] OFF
GO
SET IDENTITY_INSERT [dbo].[Parent] ON 

INSERT [dbo].[Parent] ([ParentID], [UserID], [FullName], [Gender], [DateOfBirth], [Address], [Phone]) VALUES (1, 4, N'Nguyễn Thị X', N'F', CAST(N'1975-01-01' AS Date), N'Hà Nội', N'0911111111')
INSERT [dbo].[Parent] ([ParentID], [UserID], [FullName], [Gender], [DateOfBirth], [Address], [Phone]) VALUES (2, 5, N'Lê Văn Y', N'M', CAST(N'1976-02-02' AS Date), N'Hồ Chí Minh', N'0912222222')
INSERT [dbo].[Parent] ([ParentID], [UserID], [FullName], [Gender], [DateOfBirth], [Address], [Phone]) VALUES (3, 4, N'Trần Thị Z', N'F', CAST(N'1977-03-03' AS Date), N'Đà Nẵng', N'0913333333')
INSERT [dbo].[Parent] ([ParentID], [UserID], [FullName], [Gender], [DateOfBirth], [Address], [Phone]) VALUES (4, 5, N'Phạm Văn W', N'M', CAST(N'1978-04-04' AS Date), N'Hải Phòng', N'0914444444')
INSERT [dbo].[Parent] ([ParentID], [UserID], [FullName], [Gender], [DateOfBirth], [Address], [Phone]) VALUES (5, 4, N'Hoàng Thị V', N'F', CAST(N'1979-05-05' AS Date), N'Cần Thơ', N'0915555555')
INSERT [dbo].[Parent] ([ParentID], [UserID], [FullName], [Gender], [DateOfBirth], [Address], [Phone]) VALUES (6, 6, N'nguyen ho lang', N'F', CAST(N'1994-06-15' AS Date), N'hòa lạc', N'0999999998')
SET IDENTITY_INSERT [dbo].[Parent] OFF
GO
SET IDENTITY_INSERT [dbo].[ParentalConsent] ON 

INSERT [dbo].[ParentalConsent] ([ConsentID], [StudentID], [VaccinationEventID], [ParentID], [ConsentStatus], [ConsentDate], [Note]) VALUES (1, 1, 1, 1, N'Đã đồng ý', CAST(N'2024-06-08T10:00:00.000' AS DateTime), N'Không có ghi chú')
INSERT [dbo].[ParentalConsent] ([ConsentID], [StudentID], [VaccinationEventID], [ParentID], [ConsentStatus], [ConsentDate], [Note]) VALUES (2, 2, 1, 2, N'Chờ phản hồi', NULL, NULL)
INSERT [dbo].[ParentalConsent] ([ConsentID], [StudentID], [VaccinationEventID], [ParentID], [ConsentStatus], [ConsentDate], [Note]) VALUES (3, 3, 1, 3, N'Từ chối', CAST(N'2024-06-09T11:30:00.000' AS DateTime), N'Con đã tiêm ở ngoài')
INSERT [dbo].[ParentalConsent] ([ConsentID], [StudentID], [VaccinationEventID], [ParentID], [ConsentStatus], [ConsentDate], [Note]) VALUES (4, 4, 1, 4, N'Đã đồng ý', CAST(N'2024-06-08T15:00:00.000' AS DateTime), NULL)
INSERT [dbo].[ParentalConsent] ([ConsentID], [StudentID], [VaccinationEventID], [ParentID], [ConsentStatus], [ConsentDate], [Note]) VALUES (5, 5, 1, 5, N'Chờ phản hồi', NULL, N'Phụ huynh sẽ phản hồi sau')
SET IDENTITY_INSERT [dbo].[ParentalConsent] OFF
GO
INSERT [dbo].[ParentNotification] ([NotificationID], [ParentID], [IndividualSentDate], [IndividualStatus]) VALUES (1, 1, CAST(N'2024-06-01T09:00:00.000' AS DateTime), N'Sent')
INSERT [dbo].[ParentNotification] ([NotificationID], [ParentID], [IndividualSentDate], [IndividualStatus]) VALUES (1, 2, CAST(N'2024-06-01T09:00:00.000' AS DateTime), N'Sent')
INSERT [dbo].[ParentNotification] ([NotificationID], [ParentID], [IndividualSentDate], [IndividualStatus]) VALUES (1, 3, CAST(N'2024-06-01T09:00:00.000' AS DateTime), N'Sent')
INSERT [dbo].[ParentNotification] ([NotificationID], [ParentID], [IndividualSentDate], [IndividualStatus]) VALUES (2, 1, CAST(N'2024-06-06T14:00:00.000' AS DateTime), N'Delivered')
INSERT [dbo].[ParentNotification] ([NotificationID], [ParentID], [IndividualSentDate], [IndividualStatus]) VALUES (2, 2, CAST(N'2024-06-06T14:00:00.000' AS DateTime), N'Sent')
INSERT [dbo].[ParentNotification] ([NotificationID], [ParentID], [IndividualSentDate], [IndividualStatus]) VALUES (3, 1, CAST(N'2024-07-01T10:00:00.000' AS DateTime), N'Sent')
INSERT [dbo].[ParentNotification] ([NotificationID], [ParentID], [IndividualSentDate], [IndividualStatus]) VALUES (6, 2, CAST(N'2025-06-20T22:34:07.047' AS DateTime), N'Sent')
INSERT [dbo].[ParentNotification] ([NotificationID], [ParentID], [IndividualSentDate], [IndividualStatus]) VALUES (7, 1, CAST(N'2025-06-21T20:06:56.217' AS DateTime), N'Sent')
GO
SET IDENTITY_INSERT [dbo].[RequestItemList] ON 

INSERT [dbo].[RequestItemList] ([RequestItemID], [RequestItemName], [Description]) VALUES (1, N'Paracetamol', N'Hạ sốt, giảm đau nhẹ đến vừa.
')
INSERT [dbo].[RequestItemList] ([RequestItemID], [RequestItemName], [Description]) VALUES (2, N'Oresol', N'Bù nước và điện giải khi tiêu chảy nhẹ hoặc sốc.')
INSERT [dbo].[RequestItemList] ([RequestItemID], [RequestItemName], [Description]) VALUES (3, N'Red iodine', N'Sát khuẩn vết thương ngoài da.
')
INSERT [dbo].[RequestItemList] ([RequestItemID], [RequestItemName], [Description]) VALUES (4, N'Dầu gió', N'Giảm ho, cảm cúm, sổ mũi nhẹ.
')
INSERT [dbo].[RequestItemList] ([RequestItemID], [RequestItemName], [Description]) VALUES (5, N'Chlorpheniramin', N'Giảm dị ứng, viêm mũi dị ứng, nổi mề đay.')
INSERT [dbo].[RequestItemList] ([RequestItemID], [RequestItemName], [Description]) VALUES (6, N'Thuốc nhỏ mắt', N'Rửa mắt.
')
INSERT [dbo].[RequestItemList] ([RequestItemID], [RequestItemName], [Description]) VALUES (7, N'Nước muối sinh lý', N'Rửa mũi, vết thương.
')
SET IDENTITY_INSERT [dbo].[RequestItemList] OFF
GO
SET IDENTITY_INSERT [dbo].[SchoolCheckup] ON 

INSERT [dbo].[SchoolCheckup] ([CheckupID], [StudentID], [Date], [Result], [Weight], [Height], [BloodPressure], [VisionLeft], [VisionRight]) VALUES (1, 1, CAST(N'2024-06-01T08:00:00.000' AS DateTime), N'Bình thường', CAST(30.50 AS Decimal(5, 2)), CAST(130.00 AS Decimal(5, 2)), N'110/70', N'10/10', N'10/10')
INSERT [dbo].[SchoolCheckup] ([CheckupID], [StudentID], [Date], [Result], [Weight], [Height], [BloodPressure], [VisionLeft], [VisionRight]) VALUES (2, 2, CAST(N'2024-06-01T08:00:00.000' AS DateTime), N'Bình thường', CAST(28.00 AS Decimal(5, 2)), CAST(128.00 AS Decimal(5, 2)), N'110/70', N'9/10', N'9/10')
INSERT [dbo].[SchoolCheckup] ([CheckupID], [StudentID], [Date], [Result], [Weight], [Height], [BloodPressure], [VisionLeft], [VisionRight]) VALUES (3, 3, CAST(N'2024-06-01T08:00:00.000' AS DateTime), N'Bình thường', CAST(32.00 AS Decimal(5, 2)), CAST(135.00 AS Decimal(5, 2)), N'110/70', N'10/10', N'10/10')
INSERT [dbo].[SchoolCheckup] ([CheckupID], [StudentID], [Date], [Result], [Weight], [Height], [BloodPressure], [VisionLeft], [VisionRight]) VALUES (4, 4, CAST(N'2024-06-01T08:00:00.000' AS DateTime), N'Bình thường', CAST(29.50 AS Decimal(5, 2)), CAST(129.00 AS Decimal(5, 2)), N'110/70', N'8/10', N'8/10')
INSERT [dbo].[SchoolCheckup] ([CheckupID], [StudentID], [Date], [Result], [Weight], [Height], [BloodPressure], [VisionLeft], [VisionRight]) VALUES (5, 5, CAST(N'2024-06-01T08:00:00.000' AS DateTime), N'Bình thường', CAST(31.00 AS Decimal(5, 2)), CAST(132.00 AS Decimal(5, 2)), N'110/70', N'10/10', N'10/10')
SET IDENTITY_INSERT [dbo].[SchoolCheckup] OFF
GO
SET IDENTITY_INSERT [dbo].[Student] ON 

INSERT [dbo].[Student] ([StudentID], [FullName], [Gender], [DateOfBirth], [ClassID], [ParentID], [UserID]) VALUES (1, N'Nguyễn Minh K', N'M', CAST(N'2015-01-01' AS Date), 1, 1, NULL)
INSERT [dbo].[Student] ([StudentID], [FullName], [Gender], [DateOfBirth], [ClassID], [ParentID], [UserID]) VALUES (2, N'Lê Thị L', N'F', CAST(N'2015-02-02' AS Date), 2, 2, NULL)
INSERT [dbo].[Student] ([StudentID], [FullName], [Gender], [DateOfBirth], [ClassID], [ParentID], [UserID]) VALUES (3, N'Trần Văn M', N'M', CAST(N'2014-03-03' AS Date), 3, 3, NULL)
INSERT [dbo].[Student] ([StudentID], [FullName], [Gender], [DateOfBirth], [ClassID], [ParentID], [UserID]) VALUES (4, N'Phạm Thị N', N'F', CAST(N'2014-04-04' AS Date), 4, 4, NULL)
INSERT [dbo].[Student] ([StudentID], [FullName], [Gender], [DateOfBirth], [ClassID], [ParentID], [UserID]) VALUES (5, N'Hoàng Văn O', N'M', CAST(N'2013-05-05' AS Date), 5, 5, NULL)
INSERT [dbo].[Student] ([StudentID], [FullName], [Gender], [DateOfBirth], [ClassID], [ParentID], [UserID]) VALUES (6, N'Lang Con', N'M', CAST(N'2013-05-05' AS Date), 4, 6, NULL)
SET IDENTITY_INSERT [dbo].[Student] OFF
GO
INSERT [dbo].[StudentAllergy] ([StudentID], [AllergenID], [Reaction], [Severity]) VALUES (1, 1, N'Hắt hơi', N'Nhẹ')
INSERT [dbo].[StudentAllergy] ([StudentID], [AllergenID], [Reaction], [Severity]) VALUES (2, 2, N'Nổi mề đay', N'Nặng')
INSERT [dbo].[StudentAllergy] ([StudentID], [AllergenID], [Reaction], [Severity]) VALUES (3, 3, N'Đau bụng', N'Vừa')
INSERT [dbo].[StudentAllergy] ([StudentID], [AllergenID], [Reaction], [Severity]) VALUES (4, 4, N'Chảy nước mắt', N'Nhẹ')
INSERT [dbo].[StudentAllergy] ([StudentID], [AllergenID], [Reaction], [Severity]) VALUES (5, 5, N'Khó thở', N'Nặng')
GO
SET IDENTITY_INSERT [dbo].[Teacher] ON 

INSERT [dbo].[Teacher] ([TeacherID], [FullName], [Gender], [DateOfBirth], [Phone], [Email], [Address], [HireDate]) VALUES (1, N'Nguyễn Văn A', N'M', CAST(N'1980-01-01' AS Date), N'0901111111', N'a@school.com', N'Hà Nội', CAST(N'2010-09-01' AS Date))
INSERT [dbo].[Teacher] ([TeacherID], [FullName], [Gender], [DateOfBirth], [Phone], [Email], [Address], [HireDate]) VALUES (2, N'Lê Thị B', N'F', CAST(N'1982-02-02' AS Date), N'0902222222', N'b@school.com', N'Hồ Chí Minh', CAST(N'2011-09-01' AS Date))
INSERT [dbo].[Teacher] ([TeacherID], [FullName], [Gender], [DateOfBirth], [Phone], [Email], [Address], [HireDate]) VALUES (3, N'Trần Văn C', N'M', CAST(N'1985-03-03' AS Date), N'0903333333', N'c@school.com', N'Đà Nẵng', CAST(N'2012-09-01' AS Date))
INSERT [dbo].[Teacher] ([TeacherID], [FullName], [Gender], [DateOfBirth], [Phone], [Email], [Address], [HireDate]) VALUES (4, N'Phạm Thị D', N'F', CAST(N'1987-04-04' AS Date), N'0904444444', N'd@school.com', N'Hải Phòng', CAST(N'2013-09-01' AS Date))
INSERT [dbo].[Teacher] ([TeacherID], [FullName], [Gender], [DateOfBirth], [Phone], [Email], [Address], [HireDate]) VALUES (5, N'Hoàng Văn E', N'M', CAST(N'1990-05-05' AS Date), N'0905555555', N'e@school.com', N'Cần Thơ', CAST(N'2014-09-01' AS Date))
SET IDENTITY_INSERT [dbo].[Teacher] OFF
GO
SET IDENTITY_INSERT [dbo].[VaccinationEvent] ON 

INSERT [dbo].[VaccinationEvent] ([EventID], [EventName], [Date], [Location], [ManagerID], [ClassID]) VALUES (1, N'Tiêm phòng cúm', CAST(N'2024-06-10T08:00:00.000' AS DateTime), N'Phòng y tế', 1, 1)
INSERT [dbo].[VaccinationEvent] ([EventID], [EventName], [Date], [Location], [ManagerID], [ClassID]) VALUES (2, N'Tiêm phòng sởi', CAST(N'2024-06-15T08:00:00.000' AS DateTime), N'Phòng y tế', 1, 2)
INSERT [dbo].[VaccinationEvent] ([EventID], [EventName], [Date], [Location], [ManagerID], [ClassID]) VALUES (3, N'Tiêm phòng thủy đậu', CAST(N'2024-06-20T08:00:00.000' AS DateTime), N'Phòng y tế', 1, 3)
INSERT [dbo].[VaccinationEvent] ([EventID], [EventName], [Date], [Location], [ManagerID], [ClassID]) VALUES (4, N'Tiêm phòng bạch hầu', CAST(N'2024-06-25T08:00:00.000' AS DateTime), N'Phòng y tế', 1, 4)
INSERT [dbo].[VaccinationEvent] ([EventID], [EventName], [Date], [Location], [ManagerID], [ClassID]) VALUES (5, N'Tiêm phòng viêm gan B', CAST(N'2024-06-30T08:00:00.000' AS DateTime), N'Phòng y tế', 1, 5)
SET IDENTITY_INSERT [dbo].[VaccinationEvent] OFF
GO
SET IDENTITY_INSERT [dbo].[VaccineRecord] ON 

INSERT [dbo].[VaccineRecord] ([VaccineRecordID], [StudentID], [VaccinationEventID], [NurseID], [VaccineName], [InjectionDate], [Reaction], [FollowUpStatus], [InjectionSite], [NextDoseDate]) VALUES (1, 1, 1, 1, N'Cúm', CAST(N'2024-06-10T08:30:00.000' AS DateTime), N'Không', N'Đã hoàn thành', N'Tay trái', NULL)
INSERT [dbo].[VaccineRecord] ([VaccineRecordID], [StudentID], [VaccinationEventID], [NurseID], [VaccineName], [InjectionDate], [Reaction], [FollowUpStatus], [InjectionSite], [NextDoseDate]) VALUES (2, 2, 2, 2, N'Sởi', CAST(N'2024-06-15T08:30:00.000' AS DateTime), N'Sốt nhẹ', N'Cần theo dõi', N'Tay phải', CAST(N'2024-07-15' AS Date))
INSERT [dbo].[VaccineRecord] ([VaccineRecordID], [StudentID], [VaccinationEventID], [NurseID], [VaccineName], [InjectionDate], [Reaction], [FollowUpStatus], [InjectionSite], [NextDoseDate]) VALUES (3, 3, 3, 1, N'Thủy đậu', CAST(N'2024-06-20T08:30:00.000' AS DateTime), N'Không', N'Đã hoàn thành', N'Tay trái', NULL)
INSERT [dbo].[VaccineRecord] ([VaccineRecordID], [StudentID], [VaccinationEventID], [NurseID], [VaccineName], [InjectionDate], [Reaction], [FollowUpStatus], [InjectionSite], [NextDoseDate]) VALUES (4, 4, 4, 2, N'Bạch hầu', CAST(N'2024-06-25T08:30:00.000' AS DateTime), N'Đau chỗ tiêm', N'Cần theo dõi', N'Tay phải', CAST(N'2024-07-25' AS Date))
INSERT [dbo].[VaccineRecord] ([VaccineRecordID], [StudentID], [VaccinationEventID], [NurseID], [VaccineName], [InjectionDate], [Reaction], [FollowUpStatus], [InjectionSite], [NextDoseDate]) VALUES (5, 5, 5, 1, N'Viêm gan B', CAST(N'2024-06-30T08:30:00.000' AS DateTime), N'Không', N'Đã hoàn thành', N'Tay trái', NULL)
SET IDENTITY_INSERT [dbo].[VaccineRecord] OFF
GO
/****** Object:  Index [UQ_ParentalConsent]    Script Date: 6/22/2025 12:23:17 AM ******/
ALTER TABLE [dbo].[ParentalConsent] ADD  CONSTRAINT [UQ_ParentalConsent] UNIQUE NONCLUSTERED 
(
	[StudentID] ASC,
	[VaccinationEventID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Account] ADD  DEFAULT ((1)) FOR [Active]
GO
ALTER TABLE [dbo].[Blog] ADD  DEFAULT (getdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Blog] ADD  DEFAULT ((1)) FOR [IsPublished]
GO
ALTER TABLE [dbo].[HealthReport] ADD  DEFAULT (getdate()) FOR [Date]
GO
ALTER TABLE [dbo].[MedicalEvent] ADD  DEFAULT (getdate()) FOR [EventTime]
GO
ALTER TABLE [dbo].[MedicalEventInventory] ADD  DEFAULT (getdate()) FOR [UsedTime]
GO
ALTER TABLE [dbo].[MedicalUsage] ADD  DEFAULT (getdate()) FOR [UsageDate]
GO
ALTER TABLE [dbo].[MedicineRequest] ADD  DEFAULT (getdate()) FOR [Date]
GO
ALTER TABLE [dbo].[Notification] ADD  DEFAULT (getdate()) FOR [SentDate]
GO
ALTER TABLE [dbo].[ParentNotification] ADD  DEFAULT (getdate()) FOR [IndividualSentDate]
GO
ALTER TABLE [dbo].[SchoolCheckup] ADD  DEFAULT (getdate()) FOR [Date]
GO
ALTER TABLE [dbo].[VaccinationEvent] ADD  DEFAULT (getdate()) FOR [Date]
GO
ALTER TABLE [dbo].[VaccineRecord] ADD  DEFAULT (getdate()) FOR [InjectionDate]
GO
ALTER TABLE [dbo].[Blog]  WITH CHECK ADD  CONSTRAINT [FK_Blog_Account] FOREIGN KEY([AuthorID])
REFERENCES [dbo].[Account] ([UserID])
GO
ALTER TABLE [dbo].[Blog] CHECK CONSTRAINT [FK_Blog_Account]
GO
ALTER TABLE [dbo].[Class]  WITH CHECK ADD  CONSTRAINT [FK_Class_Teacher] FOREIGN KEY([TeacherID])
REFERENCES [dbo].[Teacher] ([TeacherID])
GO
ALTER TABLE [dbo].[Class] CHECK CONSTRAINT [FK_Class_Teacher]
GO
ALTER TABLE [dbo].[HealthProfile]  WITH CHECK ADD  CONSTRAINT [FK_HealthProfile_Student] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Student] ([StudentID])
GO
ALTER TABLE [dbo].[HealthProfile] CHECK CONSTRAINT [FK_HealthProfile_Student]
GO
ALTER TABLE [dbo].[HealthReport]  WITH CHECK ADD  CONSTRAINT [FK_HealthReport_Nurse] FOREIGN KEY([NurseID])
REFERENCES [dbo].[Nurse] ([NurseID])
GO
ALTER TABLE [dbo].[HealthReport] CHECK CONSTRAINT [FK_HealthReport_Nurse]
GO
ALTER TABLE [dbo].[HealthReport]  WITH CHECK ADD  CONSTRAINT [FK_HealthReport_Student] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Student] ([StudentID])
GO
ALTER TABLE [dbo].[HealthReport] CHECK CONSTRAINT [FK_HealthReport_Student]
GO
ALTER TABLE [dbo].[ManagerAdmin]  WITH CHECK ADD  CONSTRAINT [FK_ManagerAdmin_Account] FOREIGN KEY([UserID])
REFERENCES [dbo].[Account] ([UserID])
GO
ALTER TABLE [dbo].[ManagerAdmin] CHECK CONSTRAINT [FK_ManagerAdmin_Account]
GO
ALTER TABLE [dbo].[MedicalEvent]  WITH CHECK ADD  CONSTRAINT [FK_MedicalEvent_Nurse] FOREIGN KEY([NurseID])
REFERENCES [dbo].[Nurse] ([NurseID])
GO
ALTER TABLE [dbo].[MedicalEvent] CHECK CONSTRAINT [FK_MedicalEvent_Nurse]
GO
ALTER TABLE [dbo].[MedicalEvent]  WITH CHECK ADD  CONSTRAINT [FK_MedicalEvent_Student] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Student] ([StudentID])
GO
ALTER TABLE [dbo].[MedicalEvent] CHECK CONSTRAINT [FK_MedicalEvent_Student]
GO
ALTER TABLE [dbo].[MedicalEventInventory]  WITH CHECK ADD FOREIGN KEY([EventID])
REFERENCES [dbo].[MedicalEvent] ([EventID])
GO
ALTER TABLE [dbo].[MedicalEventInventory]  WITH CHECK ADD FOREIGN KEY([ItemID])
REFERENCES [dbo].[MedicalInventory] ([ItemID])
GO
ALTER TABLE [dbo].[MedicalUsage]  WITH CHECK ADD  CONSTRAINT [FK_MedicalUsage_Item] FOREIGN KEY([ItemID])
REFERENCES [dbo].[MedicalInventory] ([ItemID])
GO
ALTER TABLE [dbo].[MedicalUsage] CHECK CONSTRAINT [FK_MedicalUsage_Item]
GO
ALTER TABLE [dbo].[MedicalUsage]  WITH CHECK ADD  CONSTRAINT [FK_MedicalUsage_Nurse] FOREIGN KEY([NurseID])
REFERENCES [dbo].[Nurse] ([NurseID])
GO
ALTER TABLE [dbo].[MedicalUsage] CHECK CONSTRAINT [FK_MedicalUsage_Nurse]
GO
ALTER TABLE [dbo].[MedicalUsage]  WITH CHECK ADD  CONSTRAINT [FK_MedicalUsage_Request] FOREIGN KEY([RequestID])
REFERENCES [dbo].[MedicineRequest] ([RequestID])
GO
ALTER TABLE [dbo].[MedicalUsage] CHECK CONSTRAINT [FK_MedicalUsage_Request]
GO
ALTER TABLE [dbo].[MedicalUsage]  WITH CHECK ADD  CONSTRAINT [FK_MedicalUsage_RequestDetail] FOREIGN KEY([RequestDetailID])
REFERENCES [dbo].[MedicineRequestDetail] ([RequestDetailID])
GO
ALTER TABLE [dbo].[MedicalUsage] CHECK CONSTRAINT [FK_MedicalUsage_RequestDetail]
GO
ALTER TABLE [dbo].[MedicalUsage]  WITH CHECK ADD  CONSTRAINT [FK_MedicalUsage_Student] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Student] ([StudentID])
GO
ALTER TABLE [dbo].[MedicalUsage] CHECK CONSTRAINT [FK_MedicalUsage_Student]
GO
ALTER TABLE [dbo].[MedicineRequest]  WITH CHECK ADD  CONSTRAINT [FK_MedicineRequest_Nurse] FOREIGN KEY([ApprovedBy])
REFERENCES [dbo].[Nurse] ([NurseID])
GO
ALTER TABLE [dbo].[MedicineRequest] CHECK CONSTRAINT [FK_MedicineRequest_Nurse]
GO
ALTER TABLE [dbo].[MedicineRequest]  WITH CHECK ADD  CONSTRAINT [FK_MedicineRequest_Parent] FOREIGN KEY([ParentID])
REFERENCES [dbo].[Parent] ([ParentID])
GO
ALTER TABLE [dbo].[MedicineRequest] CHECK CONSTRAINT [FK_MedicineRequest_Parent]
GO
ALTER TABLE [dbo].[MedicineRequest]  WITH CHECK ADD  CONSTRAINT [FK_MedicineRequest_Student] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Student] ([StudentID])
GO
ALTER TABLE [dbo].[MedicineRequest] CHECK CONSTRAINT [FK_MedicineRequest_Student]
GO
ALTER TABLE [dbo].[MedicineRequestDetail]  WITH CHECK ADD  CONSTRAINT [FK_MedicineRequestDetail_Request] FOREIGN KEY([RequestID])
REFERENCES [dbo].[MedicineRequest] ([RequestID])
GO
ALTER TABLE [dbo].[MedicineRequestDetail] CHECK CONSTRAINT [FK_MedicineRequestDetail_Request]
GO
ALTER TABLE [dbo].[MedicineRequestDetail]  WITH CHECK ADD  CONSTRAINT [FK_MedicineRequestDetail_RequestItemList] FOREIGN KEY([RequestItemID])
REFERENCES [dbo].[RequestItemList] ([RequestItemID])
GO
ALTER TABLE [dbo].[MedicineRequestDetail] CHECK CONSTRAINT [FK_MedicineRequestDetail_RequestItemList]
GO
ALTER TABLE [dbo].[Nurse]  WITH CHECK ADD  CONSTRAINT [FK_Nurse_Account] FOREIGN KEY([UserID])
REFERENCES [dbo].[Account] ([UserID])
GO
ALTER TABLE [dbo].[Nurse] CHECK CONSTRAINT [FK_Nurse_Account]
GO
ALTER TABLE [dbo].[Parent]  WITH CHECK ADD  CONSTRAINT [FK_Parent_Account] FOREIGN KEY([UserID])
REFERENCES [dbo].[Account] ([UserID])
GO
ALTER TABLE [dbo].[Parent] CHECK CONSTRAINT [FK_Parent_Account]
GO
ALTER TABLE [dbo].[ParentalConsent]  WITH CHECK ADD  CONSTRAINT [FK_ParentalConsent_Parent] FOREIGN KEY([ParentID])
REFERENCES [dbo].[Parent] ([ParentID])
GO
ALTER TABLE [dbo].[ParentalConsent] CHECK CONSTRAINT [FK_ParentalConsent_Parent]
GO
ALTER TABLE [dbo].[ParentalConsent]  WITH CHECK ADD  CONSTRAINT [FK_ParentalConsent_Student] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Student] ([StudentID])
GO
ALTER TABLE [dbo].[ParentalConsent] CHECK CONSTRAINT [FK_ParentalConsent_Student]
GO
ALTER TABLE [dbo].[ParentalConsent]  WITH CHECK ADD  CONSTRAINT [FK_ParentalConsent_VaccinationEvent] FOREIGN KEY([VaccinationEventID])
REFERENCES [dbo].[VaccinationEvent] ([EventID])
GO
ALTER TABLE [dbo].[ParentalConsent] CHECK CONSTRAINT [FK_ParentalConsent_VaccinationEvent]
GO
ALTER TABLE [dbo].[ParentNotification]  WITH CHECK ADD  CONSTRAINT [FK_ParentNotification_Notification] FOREIGN KEY([NotificationID])
REFERENCES [dbo].[Notification] ([NotificationID])
GO
ALTER TABLE [dbo].[ParentNotification] CHECK CONSTRAINT [FK_ParentNotification_Notification]
GO
ALTER TABLE [dbo].[ParentNotification]  WITH CHECK ADD  CONSTRAINT [FK_ParentNotification_Parent] FOREIGN KEY([ParentID])
REFERENCES [dbo].[Parent] ([ParentID])
GO
ALTER TABLE [dbo].[ParentNotification] CHECK CONSTRAINT [FK_ParentNotification_Parent]
GO
ALTER TABLE [dbo].[SchoolCheckup]  WITH CHECK ADD  CONSTRAINT [FK_SchoolCheckup_Student] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Student] ([StudentID])
GO
ALTER TABLE [dbo].[SchoolCheckup] CHECK CONSTRAINT [FK_SchoolCheckup_Student]
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD  CONSTRAINT [FK_Student_Account] FOREIGN KEY([UserID])
REFERENCES [dbo].[Account] ([UserID])
GO
ALTER TABLE [dbo].[Student] CHECK CONSTRAINT [FK_Student_Account]
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD  CONSTRAINT [FK_Student_Class] FOREIGN KEY([ClassID])
REFERENCES [dbo].[Class] ([ClassID])
GO
ALTER TABLE [dbo].[Student] CHECK CONSTRAINT [FK_Student_Class]
GO
ALTER TABLE [dbo].[Student]  WITH CHECK ADD  CONSTRAINT [FK_Student_Parent] FOREIGN KEY([ParentID])
REFERENCES [dbo].[Parent] ([ParentID])
GO
ALTER TABLE [dbo].[Student] CHECK CONSTRAINT [FK_Student_Parent]
GO
ALTER TABLE [dbo].[StudentAllergy]  WITH CHECK ADD  CONSTRAINT [FK_StudentAllergy_Allergen] FOREIGN KEY([AllergenID])
REFERENCES [dbo].[Allergen] ([AllergenID])
GO
ALTER TABLE [dbo].[StudentAllergy] CHECK CONSTRAINT [FK_StudentAllergy_Allergen]
GO
ALTER TABLE [dbo].[StudentAllergy]  WITH CHECK ADD  CONSTRAINT [FK_StudentAllergy_Student] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Student] ([StudentID])
GO
ALTER TABLE [dbo].[StudentAllergy] CHECK CONSTRAINT [FK_StudentAllergy_Student]
GO
ALTER TABLE [dbo].[VaccinationEvent]  WITH CHECK ADD  CONSTRAINT [FK_VaccinationEvent_Class] FOREIGN KEY([ClassID])
REFERENCES [dbo].[Class] ([ClassID])
GO
ALTER TABLE [dbo].[VaccinationEvent] CHECK CONSTRAINT [FK_VaccinationEvent_Class]
GO
ALTER TABLE [dbo].[VaccinationEvent]  WITH CHECK ADD  CONSTRAINT [FK_VaccinationEvent_ManagerAdmin] FOREIGN KEY([ManagerID])
REFERENCES [dbo].[ManagerAdmin] ([ManagerID])
GO
ALTER TABLE [dbo].[VaccinationEvent] CHECK CONSTRAINT [FK_VaccinationEvent_ManagerAdmin]
GO
ALTER TABLE [dbo].[VaccineRecord]  WITH CHECK ADD  CONSTRAINT [FK_VaccineRecord_Nurse] FOREIGN KEY([NurseID])
REFERENCES [dbo].[Nurse] ([NurseID])
GO
ALTER TABLE [dbo].[VaccineRecord] CHECK CONSTRAINT [FK_VaccineRecord_Nurse]
GO
ALTER TABLE [dbo].[VaccineRecord]  WITH CHECK ADD  CONSTRAINT [FK_VaccineRecord_Student] FOREIGN KEY([StudentID])
REFERENCES [dbo].[Student] ([StudentID])
GO
ALTER TABLE [dbo].[VaccineRecord] CHECK CONSTRAINT [FK_VaccineRecord_Student]
GO
ALTER TABLE [dbo].[VaccineRecord]  WITH CHECK ADD  CONSTRAINT [FK_VaccineRecord_VaccinationEvent] FOREIGN KEY([VaccinationEventID])
REFERENCES [dbo].[VaccinationEvent] ([EventID])
GO
ALTER TABLE [dbo].[VaccineRecord] CHECK CONSTRAINT [FK_VaccineRecord_VaccinationEvent]
GO
USE [master]
GO
ALTER DATABASE [MyWebAppDB] SET  READ_WRITE 
GO
