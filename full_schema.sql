IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Agencies] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Country] nvarchar(max) NOT NULL,
    [Founded] int NOT NULL,
    CONSTRAINT [PK_Agencies] PRIMARY KEY ([Id])
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Country', N'Founded', N'Name') AND [object_id] = OBJECT_ID(N'[Agencies]'))
    SET IDENTITY_INSERT [Agencies] ON;
INSERT INTO [Agencies] ([Id], [Country], [Founded], [Name])
VALUES (1, N'USA', 1958, N'NASA'),
(2, N'Europe', 1975, N'ESA'),
(3, N'Russia', 1992, N'Roscosmos'),
(4, N'USA', 2002, N'SpaceX');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Country', N'Founded', N'Name') AND [object_id] = OBJECT_ID(N'[Agencies]'))
    SET IDENTITY_INSERT [Agencies] OFF;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260402224242_Initial', N'8.0.25');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Astronauts] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Nationality] nvarchar(max) NOT NULL,
    [BirthYear] int NOT NULL,
    CONSTRAINT [PK_Astronauts] PRIMARY KEY ([Id])
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'BirthYear', N'Name', N'Nationality') AND [object_id] = OBJECT_ID(N'[Astronauts]'))
    SET IDENTITY_INSERT [Astronauts] ON;
INSERT INTO [Astronauts] ([Id], [BirthYear], [Name], [Nationality])
VALUES (1, 1930, N'Neil Armstrong', N'USA'),
(2, 1930, N'Buzz Aldrin', N'USA'),
(3, 1934, N'Yuri Gagarin', N'USSR'),
(4, 1951, N'Sally Ride', N'USA');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'BirthYear', N'Name', N'Nationality') AND [object_id] = OBJECT_ID(N'[Astronauts]'))
    SET IDENTITY_INSERT [Astronauts] OFF;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260403003030_AddAstronautsTable', N'8.0.25');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Rockets] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [AgencyId] int NOT NULL,
    [FirstLaunch] int NOT NULL,
    CONSTRAINT [PK_Rockets] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Rockets_Agencies_AgencyId] FOREIGN KEY ([AgencyId]) REFERENCES [Agencies] ([Id]) ON DELETE CASCADE
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AgencyId', N'FirstLaunch', N'Name') AND [object_id] = OBJECT_ID(N'[Rockets]'))
    SET IDENTITY_INSERT [Rockets] ON;
INSERT INTO [Rockets] ([Id], [AgencyId], [FirstLaunch], [Name])
VALUES (1, 1, 1967, N'Saturn V'),
(2, 4, 2010, N'Falcon 9'),
(3, 3, 1966, N'Soyuz');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'AgencyId', N'FirstLaunch', N'Name') AND [object_id] = OBJECT_ID(N'[Rockets]'))
    SET IDENTITY_INSERT [Rockets] OFF;
GO

CREATE INDEX [IX_Rockets_AgencyId] ON [Rockets] ([AgencyId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260403140208_AddRocketsTable', N'8.0.25');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Missions] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [RocketId] int NOT NULL,
    [LaunchDate] datetime2 NOT NULL,
    [Status] nvarchar(max) NOT NULL,
    [Destination] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_Missions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Missions_Rockets_RocketId] FOREIGN KEY ([RocketId]) REFERENCES [Rockets] ([Id]) ON DELETE CASCADE
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Destination', N'LaunchDate', N'Name', N'RocketId', N'Status') AND [object_id] = OBJECT_ID(N'[Missions]'))
    SET IDENTITY_INSERT [Missions] ON;
INSERT INTO [Missions] ([Id], [Destination], [LaunchDate], [Name], [RocketId], [Status])
VALUES (1, N'Moon', '1969-07-16T00:00:00.0000000', N'Apollo 11', 1, N'Completed'),
(2, N'Earth Orbit', '1961-04-12T00:00:00.0000000', N'Vostok 1', 3, N'Completed'),
(3, N'Moon Orbit', '2022-11-16T00:00:00.0000000', N'Artemis I', 1, N'Completed');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Destination', N'LaunchDate', N'Name', N'RocketId', N'Status') AND [object_id] = OBJECT_ID(N'[Missions]'))
    SET IDENTITY_INSERT [Missions] OFF;
GO

CREATE INDEX [IX_Missions_RocketId] ON [Missions] ([RocketId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260403150129_AddMissionsTable', N'8.0.25');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [MissionAstronauts] (
    [MissionId] int NOT NULL,
    [AstronautId] int NOT NULL,
    CONSTRAINT [PK_MissionAstronauts] PRIMARY KEY ([MissionId], [AstronautId]),
    CONSTRAINT [FK_MissionAstronauts_Astronauts_AstronautId] FOREIGN KEY ([AstronautId]) REFERENCES [Astronauts] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_MissionAstronauts_Missions_MissionId] FOREIGN KEY ([MissionId]) REFERENCES [Missions] ([Id]) ON DELETE CASCADE
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'AstronautId', N'MissionId') AND [object_id] = OBJECT_ID(N'[MissionAstronauts]'))
    SET IDENTITY_INSERT [MissionAstronauts] ON;
INSERT INTO [MissionAstronauts] ([AstronautId], [MissionId])
VALUES (1, 1),
(2, 1),
(3, 2);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'AstronautId', N'MissionId') AND [object_id] = OBJECT_ID(N'[MissionAstronauts]'))
    SET IDENTITY_INSERT [MissionAstronauts] OFF;
GO

CREATE INDEX [IX_MissionAstronauts_AstronautId] ON [MissionAstronauts] ([AstronautId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260403153724_AddMissionAstronautTable', N'8.0.25');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [AspNetRoles] (
    [Id] uniqueidentifier NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetUsers] (
    [Id] uniqueidentifier NOT NULL,
    [PersonName] nvarchar(max) NULL,
    [RefreshToken] nvarchar(max) NULL,
    [RefreshTokenExpirationDateTime] datetime2 NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);
GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] uniqueidentifier NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] uniqueidentifier NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] uniqueidentifier NOT NULL,
    [RoleId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] uniqueidentifier NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20260403202711_AddIdentityTables', N'8.0.25');
GO

COMMIT;
GO

