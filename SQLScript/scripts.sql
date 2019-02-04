CREATE DATABASE VMS
GO
USE [VMS]
GO
/****** Object:  Table [dbo].[Meeting]    Script Date: 2/4/2019 5:32:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Meeting](
	[MeetingId] [int] IDENTITY(1,1) NOT NULL,
	[OrganizerId] [int] NOT NULL,
	[VisitorId] [int] NOT NULL,
	[Date] [datetime] NOT NULL,
	[Purpose] [nvarchar](max) NOT NULL,
	[OTP] [int] NULL,
 CONSTRAINT [PK_Meeting] PRIMARY KEY CLUSTERED 
(
	[MeetingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MeetingStatus]    Script Date: 2/4/2019 5:32:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MeetingStatus](
	[MeetingStatusId] [int] NOT NULL,
	[Status] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_MeetingStatus] PRIMARY KEY CLUSTERED 
(
	[MeetingStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MeetingStatusHistory]    Script Date: 2/4/2019 5:32:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MeetingStatusHistory](
	[MeetingHistoryId] [int] IDENTITY(1,1) NOT NULL,
	[MeetingId] [int] NOT NULL,
	[MeetingStatusId] [int] NOT NULL,
	[LasteUpdatedAt] [datetime] NOT NULL,
	[Remarks] [nvarchar](max) NULL,
	[UserEmail] [nvarchar](250) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Organizer]    Script Date: 2/4/2019 5:32:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Organizer](
	[OrganizerId] [int] IDENTITY(1,1) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Department] [nvarchar](250) NOT NULL,
	[Designation] [nvarchar](250) NOT NULL,
 CONSTRAINT [PK_Organizer] PRIMARY KEY CLUSTERED 
(
	[OrganizerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Visitor]    Script Date: 2/4/2019 5:32:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Visitor](
	[VisitorId] [int] IDENTITY(1,1) NOT NULL,
	[EmailId] [nvarchar](250) NOT NULL,
	[ContactNumber] [nvarchar](15) NOT NULL,
 CONSTRAINT [PK_visitor] PRIMARY KEY CLUSTERED 
(
	[VisitorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
GO
INSERT [dbo].[MeetingStatus] ([MeetingStatusId], [Status]) VALUES (1, N'Initiated')
GO
INSERT [dbo].[MeetingStatus] ([MeetingStatusId], [Status]) VALUES (2, N'Acknowledged')
GO
INSERT [dbo].[MeetingStatus] ([MeetingStatusId], [Status]) VALUES (3, N'Visited')
GO
INSERT [dbo].[MeetingStatus] ([MeetingStatusId], [Status]) VALUES (4, N'Closed')
GO
INSERT [dbo].[MeetingStatus] ([MeetingStatusId], [Status]) VALUES (5, N'Rejected')
GO
INSERT [dbo].[MeetingStatus] ([MeetingStatusId], [Status]) VALUES (6, N'Postponed')
GO
INSERT [dbo].[MeetingStatus] ([MeetingStatusId], [Status]) VALUES (7, N'Cancelled')
GO

ALTER TABLE [dbo].[Meeting]  WITH CHECK ADD  CONSTRAINT [FK_Meeting_Organizer] FOREIGN KEY([OrganizerId])
REFERENCES [dbo].[Organizer] ([OrganizerId])
GO
ALTER TABLE [dbo].[Meeting] CHECK CONSTRAINT [FK_Meeting_Organizer]
GO
ALTER TABLE [dbo].[Meeting]  WITH CHECK ADD  CONSTRAINT [FK_Meeting_Visitor] FOREIGN KEY([VisitorId])
REFERENCES [dbo].[Visitor] ([VisitorId])
GO
ALTER TABLE [dbo].[Meeting] CHECK CONSTRAINT [FK_Meeting_Visitor]
GO
ALTER TABLE [dbo].[MeetingStatusHistory]  WITH CHECK ADD  CONSTRAINT [FK_MeetingStatusHistory_Meeting] FOREIGN KEY([MeetingId])
REFERENCES [dbo].[Meeting] ([MeetingId])
GO
ALTER TABLE [dbo].[MeetingStatusHistory] CHECK CONSTRAINT [FK_MeetingStatusHistory_Meeting]
GO
ALTER TABLE [dbo].[MeetingStatusHistory]  WITH CHECK ADD  CONSTRAINT [FK_MeetingStatusHistory_MeetingStatus] FOREIGN KEY([MeetingStatusId])
REFERENCES [dbo].[MeetingStatus] ([MeetingStatusId])
GO
ALTER TABLE [dbo].[MeetingStatusHistory] CHECK CONSTRAINT [FK_MeetingStatusHistory_MeetingStatus]
GO
/****** Object:  StoredProcedure [dbo].[spCreateMeeting]    Script Date: 2/4/2019 5:32:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spCreateMeeting] 
	@OrganizerId INT,
	@VisitorEmail NVARCHAR(250),
	@ContactNumber NVARCHAR(50),
	@ScheduledDate DATETIME,
	@Purpose NVARCHAR(MAX)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @VisitorId AS INT;

	SET @VisitorId = -1;

	SELECT @VisitorId = VisitorId from Visitor where EmailId = @VisitorEmail;

		IF @VisitorId = -1
		BEGIN
			INSERT INTO [dbo].[Visitor]
				([EmailId]
				,[ContactNumber])
			VALUES
				(@VisitorEmail
				,@ContactNumber);
		END

		SELECT @VisitorId = VisitorId from Visitor where EmailId = @VisitorEmail;

		INSERT INTO [dbo].[Meeting]
			([OrganizerId]
			,[VisitorId]
			,[Date]
			,[Purpose])
		VALUES
			(@OrganizerId
			,@VisitorId
			,@ScheduledDate
			,@Purpose);

		DECLARE @MeetingID AS INT;
		SELECT @MeetingID = IDENT_CURRENT('Meeting')
		INSERT INTO [dbo].[MeetingStatusHistory]
           (
				[MeetingId]
			   ,[MeetingStatusId]
			   ,[LasteUpdatedAt]
			   ,[Remarks]
			   ,[UserEmail]
		   )VALUES(@MeetingID,1,GETDATE(),'',@VisitorEmail)

END
GO
/****** Object:  StoredProcedure [dbo].[spUpdateMeeting]    Script Date: 2/4/2019 5:32:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spUpdateMeeting]
	-- Add the parameters for the stored procedure here
	@MeetingID int,
	@statusId int,
	@email varchar(250),
	@OTP INT
AS
BEGIN

IF @OTP > -1
BEGIN

	UPDATE [dbo].[Meeting]
    SET [OTP] = @OTP
	WHERE MeetingId = @MeetingID;
END


INSERT INTO [dbo].[MeetingStatusHistory]
(
     [MeetingId]
	,[MeetingStatusId]
	,[LasteUpdatedAt]
	,[Remarks]
	,[UserEmail]
)
VALUES(@MeetingID,@statusId,GETDATE(),'',@email);

END
GO
INSERT [dbo].[MeetingStatus] ([MeetingStatusId], [Status]) VALUES (1, N'Initiated')
GO
INSERT [dbo].[MeetingStatus] ([MeetingStatusId], [Status]) VALUES (2, N'Acknowledged')
GO
INSERT [dbo].[MeetingStatus] ([MeetingStatusId], [Status]) VALUES (3, N'Visited')
GO
INSERT [dbo].[MeetingStatus] ([MeetingStatusId], [Status]) VALUES (4, N'Closed')
GO
INSERT [dbo].[MeetingStatus] ([MeetingStatusId], [Status]) VALUES (5, N'Rejected')
GO
INSERT [dbo].[MeetingStatus] ([MeetingStatusId], [Status]) VALUES (6, N'Postponed')
GO
INSERT [dbo].[MeetingStatus] ([MeetingStatusId], [Status]) VALUES (7, N'Cancelled')
GO
