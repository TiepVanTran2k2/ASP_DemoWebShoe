USE [master]
GO
/****** Object:  Database [BanGiayPs17468]    Script Date: 2/24/2022 12:17:53 PM ******/
CREATE DATABASE [BanGiayPs17468]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BanGiayPs17468', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\BanGiayPs17468.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BanGiayPs17468_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\BanGiayPs17468_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [BanGiayPs17468] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BanGiayPs17468].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BanGiayPs17468] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BanGiayPs17468] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BanGiayPs17468] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BanGiayPs17468] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BanGiayPs17468] SET ARITHABORT OFF 
GO
ALTER DATABASE [BanGiayPs17468] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [BanGiayPs17468] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BanGiayPs17468] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BanGiayPs17468] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BanGiayPs17468] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BanGiayPs17468] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BanGiayPs17468] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BanGiayPs17468] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BanGiayPs17468] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BanGiayPs17468] SET  ENABLE_BROKER 
GO
ALTER DATABASE [BanGiayPs17468] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BanGiayPs17468] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BanGiayPs17468] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BanGiayPs17468] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BanGiayPs17468] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BanGiayPs17468] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [BanGiayPs17468] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BanGiayPs17468] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [BanGiayPs17468] SET  MULTI_USER 
GO
ALTER DATABASE [BanGiayPs17468] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BanGiayPs17468] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BanGiayPs17468] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BanGiayPs17468] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [BanGiayPs17468] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [BanGiayPs17468] SET QUERY_STORE = OFF
GO
USE [BanGiayPs17468]
GO
/****** Object:  Table [dbo].[Cart]    Script Date: 2/24/2022 12:17:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cart](
	[CartId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[Status] [bit] NULL,
	[SumPrice] [decimal](18, 0) NULL,
	[Phone] [varchar](10) NULL,
	[Address] [varchar](100) NULL,
	[NameRecipient] [varchar](30) NULL,
PRIMARY KEY CLUSTERED 
(
	[CartId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CartDetail]    Script Date: 2/24/2022 12:17:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CartDetail](
	[Stt] [int] IDENTITY(1,1) NOT NULL,
	[CartId] [int] NULL,
	[ProductId] [int] NULL,
	[Quantily] [int] NULL,
	[Total] [decimal](18, 0) NULL,
PRIMARY KEY CLUSTERED 
(
	[Stt] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 2/24/2022 12:17:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[CustomerId] [int] IDENTITY(1,1) NOT NULL,
	[Email] [varchar](45) NOT NULL,
	[PassWord] [varchar](20) NOT NULL,
	[Status] [bit] NOT NULL,
	[Role] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomerDetail]    Script Date: 2/24/2022 12:17:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomerDetail](
	[CustomerId] [int] NOT NULL,
	[Name] [nvarchar](45) NULL,
	[Age] [int] NULL,
	[DeliveryAddress] [nvarchar](200) NULL,
	[Phone] [varchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 2/24/2022 12:17:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](45) NOT NULL,
	[Price] [decimal](18, 0) NOT NULL,
	[Introduce] [nvarchar](500) NOT NULL,
	[SupplierId] [int] NOT NULL,
	[Status] [bit] NULL,
	[Images] [nvarchar](700) NOT NULL,
	[Quantity] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Supplier]    Script Date: 2/24/2022 12:17:53 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Supplier](
	[SupplierId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](200) NOT NULL,
	[Status] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[SupplierId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Cart] ON 

INSERT [dbo].[Cart] ([CartId], [CustomerId], [CreateDate], [Status], [SumPrice], [Phone], [Address], [NameRecipient]) VALUES (15, 13, CAST(N'2022-02-24T09:56:45.000' AS DateTime), 0, CAST(100 AS Decimal(18, 0)), N'0384067997', N'thon 1 duc nhuan mo duc quang ngai', N'Van Tiep')
INSERT [dbo].[Cart] ([CartId], [CustomerId], [CreateDate], [Status], [SumPrice], [Phone], [Address], [NameRecipient]) VALUES (16, 13, CAST(N'2022-02-24T10:01:31.000' AS DateTime), 0, CAST(100 AS Decimal(18, 0)), N'0384067990', N'thon 1 duc nhuan mo duc quang ngai', N'Van Tiep')
INSERT [dbo].[Cart] ([CartId], [CustomerId], [CreateDate], [Status], [SumPrice], [Phone], [Address], [NameRecipient]) VALUES (17, 13, CAST(N'2022-02-24T10:12:27.000' AS DateTime), 1, CAST(99 AS Decimal(18, 0)), N'0384067990', N'thon 1 duc nhuan mo duc quang ngai', N'Van Tiep')
SET IDENTITY_INSERT [dbo].[Cart] OFF
GO
SET IDENTITY_INSERT [dbo].[CartDetail] ON 

INSERT [dbo].[CartDetail] ([Stt], [CartId], [ProductId], [Quantily], [Total]) VALUES (19, 15, 4, 1, CAST(100 AS Decimal(18, 0)))
INSERT [dbo].[CartDetail] ([Stt], [CartId], [ProductId], [Quantily], [Total]) VALUES (20, 16, 4, 1, CAST(100 AS Decimal(18, 0)))
INSERT [dbo].[CartDetail] ([Stt], [CartId], [ProductId], [Quantily], [Total]) VALUES (21, 17, 6, 1, CAST(99 AS Decimal(18, 0)))
SET IDENTITY_INSERT [dbo].[CartDetail] OFF
GO
SET IDENTITY_INSERT [dbo].[Customer] ON 

INSERT [dbo].[Customer] ([CustomerId], [Email], [PassWord], [Status], [Role]) VALUES (5, N'tranvantiep@gmail.com', N'a', 1, 1)
INSERT [dbo].[Customer] ([CustomerId], [Email], [PassWord], [Status], [Role]) VALUES (8, N'a@gmail.com', N'a', 1, 0)
INSERT [dbo].[Customer] ([CustomerId], [Email], [PassWord], [Status], [Role]) VALUES (9, N'tiep@gmail.com', N'1', 1, 0)
INSERT [dbo].[Customer] ([CustomerId], [Email], [PassWord], [Status], [Role]) VALUES (10, N'b@gmail.com', N'a', 1, 0)
INSERT [dbo].[Customer] ([CustomerId], [Email], [PassWord], [Status], [Role]) VALUES (11, N'tiep1@gmail.com', N'1', 1, 0)
INSERT [dbo].[Customer] ([CustomerId], [Email], [PassWord], [Status], [Role]) VALUES (12, N'tiep2@gmail.com', N'h', 0, 0)
INSERT [dbo].[Customer] ([CustomerId], [Email], [PassWord], [Status], [Role]) VALUES (13, N'tranvantiep2506@gmail.com', N'a', 1, 0)
SET IDENTITY_INSERT [dbo].[Customer] OFF
GO
INSERT [dbo].[CustomerDetail] ([CustomerId], [Name], [Age], [DeliveryAddress], [Phone]) VALUES (5, N'a', 2, N'a', N'a')
INSERT [dbo].[CustomerDetail] ([CustomerId], [Name], [Age], [DeliveryAddress], [Phone]) VALUES (12, N'Not', 0, N'Not', N'Not')
INSERT [dbo].[CustomerDetail] ([CustomerId], [Name], [Age], [DeliveryAddress], [Phone]) VALUES (13, N'Not', 0, N'đ', N'd')
GO
SET IDENTITY_INSERT [dbo].[Product] ON 

INSERT [dbo].[Product] ([ProductId], [Name], [Price], [Introduce], [SupplierId], [Status], [Images], [Quantity]) VALUES (4, N'Bitis Hunter X 2018', CAST(100 AS Decimal(18, 0)), N'Biti''s Hunter still seems to show no signs of slowing down with the ambition to stir up the domestic sneaker market in 2018. After two "bombardments" in the first half is the international collaboration BST Biti''s Hunter x Marvel and nearly This is the Shades of Dark - Cooler You collection with a style guide in collaboration with H&M, the "Hunter" decided to shoot the next cannon with Biti''s Hunter 2K18 The Evolution BST.', 1, 1, N'266a2c21-dfab-4467-8d54-bcb18ca8711f_hunterX2018.png', 98)
INSERT [dbo].[Product] ([ProductId], [Name], [Price], [Introduce], [SupplierId], [Status], [Images], [Quantity]) VALUES (5, N'Bitis A', CAST(99 AS Decimal(18, 0)), N'adfsghdtghhhhhh', 1, 1, N'88994d9d-f497-4887-a049-c6be800a136c_[removal.ai]_tmp-621256d35908b.png', 100)
INSERT [dbo].[Product] ([ProductId], [Name], [Price], [Introduce], [SupplierId], [Status], [Images], [Quantity]) VALUES (6, N'Bitis hunter X 2017', CAST(99 AS Decimal(18, 0)), N'y', 1, 1, N'2b7f1b1b-165f-4f84-bd24-bea04f60e89f_[removal.ai]_tmp-621256d35908b.png', 99)
SET IDENTITY_INSERT [dbo].[Product] OFF
GO
SET IDENTITY_INSERT [dbo].[Supplier] ON 

INSERT [dbo].[Supplier] ([SupplierId], [Name], [Address], [Status]) VALUES (1, N'Bistis', N'Quận 1', 1)
INSERT [dbo].[Supplier] ([SupplierId], [Name], [Address], [Status]) VALUES (2, N'a', N'ab', 1)
SET IDENTITY_INSERT [dbo].[Supplier] OFF
GO
ALTER TABLE [dbo].[Cart] ADD  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[Cart]  WITH CHECK ADD FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([CustomerId])
GO
ALTER TABLE [dbo].[CartDetail]  WITH CHECK ADD FOREIGN KEY([CartId])
REFERENCES [dbo].[Cart] ([CartId])
GO
ALTER TABLE [dbo].[CartDetail]  WITH CHECK ADD FOREIGN KEY([ProductId])
REFERENCES [dbo].[Product] ([ProductId])
GO
ALTER TABLE [dbo].[CustomerDetail]  WITH CHECK ADD FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([CustomerId])
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD FOREIGN KEY([SupplierId])
REFERENCES [dbo].[Supplier] ([SupplierId])
GO
USE [master]
GO
ALTER DATABASE [BanGiayPs17468] SET  READ_WRITE 
GO
