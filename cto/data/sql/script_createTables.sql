USE [CTO]
GO

/****** Object:  Table [dbo].[Inspections]    Script Date: 2016-08-31 11:28:43 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Inspections](
	[insNumber] [int] NOT NULL,
	[refNumber] [varchar](15) NOT NULL,
	[estName] [varchar](200) NOT NULL,
	[regNumber] [varchar](50) NULL,
	[street] [varchar](200) NULL,
	[city] [varchar](100) NULL,
	[province] [varchar](100) NULL,
	[country] [varchar](50) NULL,
	[postalCode] [varchar](20) NULL,
	[curRegistered] [bit] NULL,
	[activityEn] [varchar](500) NULL,
	[activityFr] [varchar](500) NULL,
	[insTypeEn] [varchar](100) NULL,
	[insTypeFr] [varchar](100) NULL,
	[insStartDate] [datetime] NULL,
	[insEndDate] [datetime] NULL,
	[rating] [varchar](2) NULL,
	[reportCard] [bit] NOT NULL,
	[createdDate] [datetime] NOT NULL,
	[updatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Inspections] PRIMARY KEY CLUSTERED 
(
	[insNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


/****** Object:  Table [dbo].[ReportCardSummary]    Script Date: 2016-08-31 11:28:46 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ReportCardSummary](
	[insNumber] [int] NOT NULL,
	[refNumber] [varchar](15) NOT NULL,
	[orderNo] [int] NOT NULL,
	[subOrderNo] [int] NOT NULL,
	[regEn] [varchar](500) NOT NULL,
	[regFr] [varchar](500) NOT NULL,
	[summaryEn] [nvarchar](max) NOT NULL,
	[summaryFr] [nvarchar](max) NOT NULL,
	[insOutcomeEn] [nvarchar](max) NULL,
	[insOutcomeFr] [nvarchar](max) NULL,
	[measureTakenEn] [nvarchar](max) NULL,
	[measureTakenFr] [nvarchar](max) NULL,
	[createdDate] [datetime] NOT NULL,
	[updatedDate] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


/****** Object:  Table [dbo].[InitialDeficiencies]    Script Date: 2016-08-31 11:28:37 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[InitialDeficiencies](
	[insNumber] [int] NOT NULL,
	[refNumber] [varchar](15) NOT NULL,
	[orderNo] [int] NOT NULL,
	[subOrderNo] [int] NOT NULL,
	[regEn] [varchar](500) NOT NULL,
	[regFr] [varchar](500) NOT NULL,
	[summaryEn] [nvarchar](max) NOT NULL,
	[summaryFr] [nvarchar](max) NOT NULL,
	[createdDate] [datetime] NOT NULL,
	[updatedDate] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[InitialDeficiencies] ADD  CONSTRAINT [DF_InitialDeficiencies]  DEFAULT (getdate()) FOR [createdDate]
GO


