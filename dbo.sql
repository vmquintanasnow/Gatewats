/*
 Navicat Premium Data Transfer

 Source Server         : LocalDb
 Source Server Type    : SQL Server
 Source Server Version : 13004001
 Source Host           : (localdb)\MSSQLLocalDB:1433
 Source Catalog        : MusalaDB
 Source Schema         : dbo

 Target Server Type    : SQL Server
 Target Server Version : 13004001
 File Encoding         : 65001

 Date: 15/10/2020 15:10:23
*/


-- ----------------------------
-- Table structure for __EFMigrationsHistory
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[__EFMigrationsHistory]') AND type IN ('U'))
	DROP TABLE [dbo].[__EFMigrationsHistory]
GO

CREATE TABLE [dbo].[__EFMigrationsHistory] (
  [MigrationId] nvarchar(150) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [ProductVersion] nvarchar(32) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL
)
GO

ALTER TABLE [dbo].[__EFMigrationsHistory] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for device
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[device]') AND type IN ('U'))
	DROP TABLE [dbo].[device]
GO

CREATE TABLE [dbo].[device] (
  [id] int  IDENTITY(1,1) NOT NULL,
  [vendor] varchar(64) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [date_creation] datetime  NULL,
  [status] bit  NULL,
  [created_at] datetime  NOT NULL,
  [is_deleted] bit  NOT NULL,
  [gateway_id] varchar(64) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL
)
GO

ALTER TABLE [dbo].[device] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Table structure for gateway
-- ----------------------------
IF EXISTS (SELECT * FROM sys.all_objects WHERE object_id = OBJECT_ID(N'[dbo].[gateway]') AND type IN ('U'))
	DROP TABLE [dbo].[gateway]
GO

CREATE TABLE [dbo].[gateway] (
  [id] varchar(64) COLLATE SQL_Latin1_General_CP1_CI_AS  NOT NULL,
  [name] varchar(64) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [ipv4] varchar(15) COLLATE SQL_Latin1_General_CP1_CI_AS  NULL,
  [created_at] datetime  NOT NULL,
  [is_deleted] bit  NOT NULL
)
GO

ALTER TABLE [dbo].[gateway] SET (LOCK_ESCALATION = TABLE)
GO


-- ----------------------------
-- Primary Key structure for table __EFMigrationsHistory
-- ----------------------------
ALTER TABLE [dbo].[__EFMigrationsHistory] ADD CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED ([MigrationId])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Indexes structure for table device
-- ----------------------------
CREATE NONCLUSTERED INDEX [Gateway_Device]
ON [dbo].[device] (
  [gateway_id] ASC
)
GO


-- ----------------------------
-- Primary Key structure for table device
-- ----------------------------
ALTER TABLE [dbo].[device] ADD CONSTRAINT [PK_device] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Primary Key structure for table gateway
-- ----------------------------
ALTER TABLE [dbo].[gateway] ADD CONSTRAINT [PK_gateway] PRIMARY KEY CLUSTERED ([id])
WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)  
ON [PRIMARY]
GO


-- ----------------------------
-- Foreign Keys structure for table device
-- ----------------------------
ALTER TABLE [dbo].[device] ADD CONSTRAINT [Gateway_Device] FOREIGN KEY ([gateway_id]) REFERENCES [dbo].[gateway] ([id]) ON DELETE CASCADE ON UPDATE NO ACTION
GO

