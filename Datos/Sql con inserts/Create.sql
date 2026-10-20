-- DROP SCHEMA dbo;

CREATE SCHEMA dbo;
-- VirtualParkDb.dbo.Attractions definition

-- Drop table

-- DROP TABLE VirtualParkDb.dbo.Attractions;

CREATE TABLE VirtualParkDb.dbo.Attractions (
	Id int IDENTITY(1,1) NOT NULL,
	Name nvarchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	TypeAttraction nvarchar(50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	MinAge int NOT NULL,
	MaxCapacity int NOT NULL,
	Description nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CurrentCapacity int NOT NULL,
	CONSTRAINT PK_Attractions PRIMARY KEY (Id)
);
 CREATE UNIQUE NONCLUSTERED INDEX IX_Attractions_Name ON VirtualParkDb.dbo.Attractions (  Name ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- VirtualParkDb.dbo.Rewards definition

-- Drop table

-- DROP TABLE VirtualParkDb.dbo.Rewards;

CREATE TABLE VirtualParkDb.dbo.Rewards (
	Id int IDENTITY(1,1) NOT NULL,
	Name nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Description nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	PointsCost int NOT NULL,
	AvailableQuantity int NOT NULL,
	RequiredMembershipLevel nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	Active bit NOT NULL,
	CONSTRAINT PK_Rewards PRIMARY KEY (Id)
);


-- VirtualParkDb.dbo.SpecialEvents definition

-- Drop table

-- DROP TABLE VirtualParkDb.dbo.SpecialEvents;

CREATE TABLE VirtualParkDb.dbo.SpecialEvents (
	Id int IDENTITY(1,1) NOT NULL,
	Name nvarchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Date] datetime2 NOT NULL,
	StartTime time NOT NULL,
	EndTime time NOT NULL,
	MaxCapacity int NOT NULL,
	CurrentCapacity int NOT NULL,
	AdditionalCost decimal(10,2) NOT NULL,
	Active bit NOT NULL,
	CONSTRAINT PK_SpecialEvents PRIMARY KEY (Id)
);


-- VirtualParkDb.dbo.Visitors definition

-- Drop table

-- DROP TABLE VirtualParkDb.dbo.Visitors;

CREATE TABLE VirtualParkDb.dbo.Visitors (
	Id int IDENTITY(1,1) NOT NULL,
	BirthDate datetime2 NOT NULL,
	Name nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Password nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Email nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK_Visitors PRIMARY KEY (Id)
);
 CREATE UNIQUE NONCLUSTERED INDEX IX_Visitors_Email ON VirtualParkDb.dbo.Visitors (  Email ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- VirtualParkDb.dbo.Workers definition

-- Drop table

-- DROP TABLE VirtualParkDb.dbo.Workers;

CREATE TABLE VirtualParkDb.dbo.Workers (
	Id int IDENTITY(1,1) NOT NULL,
	Roles nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Name nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Password nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Email nvarchar(450) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK_Workers PRIMARY KEY (Id)
);
 CREATE UNIQUE NONCLUSTERED INDEX IX_Workers_Email ON VirtualParkDb.dbo.Workers (  Email ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- VirtualParkDb.dbo.[__EFMigrationsHistory] definition

-- Drop table

-- DROP TABLE VirtualParkDb.dbo.[__EFMigrationsHistory];

CREATE TABLE VirtualParkDb.dbo.[__EFMigrationsHistory] (
	MigrationId nvarchar(150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ProductVersion nvarchar(32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT PK___EFMigrationsHistory PRIMARY KEY (MigrationId)
);


-- VirtualParkDb.dbo.Incidences definition

-- Drop table

-- DROP TABLE VirtualParkDb.dbo.Incidences;

CREATE TABLE VirtualParkDb.dbo.Incidences (
	Id int IDENTITY(1,1) NOT NULL,
	Description nvarchar(500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Active bit DEFAULT CONVERT([bit],(0)) NOT NULL,
	AttractionId int NOT NULL,
	AttractionName nvarchar(200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Duration int DEFAULT 0 NOT NULL,
	IsMaintenance bit DEFAULT CONVERT([bit],(0)) NOT NULL,
	StartDate datetime2 DEFAULT '0001-01-01T00:00:00.0000000' NOT NULL,
	StartTime time DEFAULT '00:00:00' NOT NULL,
	CONSTRAINT PK_Incidences PRIMARY KEY (Id),
	CONSTRAINT FK_Incidences_Attractions_AttractionId FOREIGN KEY (AttractionId) REFERENCES VirtualParkDb.dbo.Attractions(Id) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_Incidences_AttractionId ON VirtualParkDb.dbo.Incidences (  AttractionId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- VirtualParkDb.dbo.VisitorParkInformations definition

-- Drop table

-- DROP TABLE VirtualParkDb.dbo.VisitorParkInformations;

CREATE TABLE VirtualParkDb.dbo.VisitorParkInformations (
	Id int NOT NULL,
	VisitorMembership nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Points int NOT NULL,
	NFC uniqueidentifier DEFAULT newid() NOT NULL,
	RedeemPointsToday int DEFAULT 0 NOT NULL,
	TodayPoints int DEFAULT 0 NOT NULL,
	CONSTRAINT PK_VisitorParkInformations PRIMARY KEY (Id),
	CONSTRAINT FK_VisitorParkInformations_Visitors_Id FOREIGN KEY (Id) REFERENCES VirtualParkDb.dbo.Visitors(Id) ON DELETE CASCADE
);
 CREATE UNIQUE NONCLUSTERED INDEX IX_VisitorParkInformations_NFC ON VirtualParkDb.dbo.VisitorParkInformations (  NFC ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- VirtualParkDb.dbo.specialEvent_Attraction definition

-- Drop table

-- DROP TABLE VirtualParkDb.dbo.specialEvent_Attraction;

CREATE TABLE VirtualParkDb.dbo.specialEvent_Attraction (
	IncludedAttractionsId int NOT NULL,
	SpecialEventId int NOT NULL,
	CONSTRAINT PK_specialEvent_Attraction PRIMARY KEY (IncludedAttractionsId,SpecialEventId),
	CONSTRAINT FK_specialEvent_Attraction_Attractions_IncludedAttractionsId FOREIGN KEY (IncludedAttractionsId) REFERENCES VirtualParkDb.dbo.Attractions(Id) ON DELETE CASCADE,
	CONSTRAINT FK_specialEvent_Attraction_SpecialEvents_SpecialEventId FOREIGN KEY (SpecialEventId) REFERENCES VirtualParkDb.dbo.SpecialEvents(Id) ON DELETE CASCADE
);
 CREATE NONCLUSTERED INDEX IX_specialEvent_Attraction_SpecialEventId ON VirtualParkDb.dbo.specialEvent_Attraction (  SpecialEventId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- VirtualParkDb.dbo.PointsHistory definition

-- Drop table

-- DROP TABLE VirtualParkDb.dbo.PointsHistory;

CREATE TABLE VirtualParkDb.dbo.PointsHistory (
	Id int IDENTITY(1,1) NOT NULL,
	VisitorParkInformationId int NOT NULL,
	TransactionDate datetime2 NOT NULL,
	Points int NOT NULL,
	Origin nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	Description nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	ActiveStrategy nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	RelatedEntityId int NULL,
	VisitorParkInformationId1 int NULL,
	CONSTRAINT PK_PointsHistory PRIMARY KEY (Id),
	CONSTRAINT FK_PointsHistory_VisitorParkInformations_VisitorParkInformationId FOREIGN KEY (VisitorParkInformationId) REFERENCES VirtualParkDb.dbo.VisitorParkInformations(Id),
	CONSTRAINT FK_PointsHistory_VisitorParkInformations_VisitorParkInformationId1 FOREIGN KEY (VisitorParkInformationId1) REFERENCES VirtualParkDb.dbo.VisitorParkInformations(Id)
);
 CREATE NONCLUSTERED INDEX IX_PointsHistory_VisitorParkInformationId ON VirtualParkDb.dbo.PointsHistory (  VisitorParkInformationId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_PointsHistory_VisitorParkInformationId1 ON VirtualParkDb.dbo.PointsHistory (  VisitorParkInformationId1 ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- VirtualParkDb.dbo.RewardRedemptions definition

-- Drop table

-- DROP TABLE VirtualParkDb.dbo.RewardRedemptions;

CREATE TABLE VirtualParkDb.dbo.RewardRedemptions (
	Id int IDENTITY(1,1) NOT NULL,
	VisitorParkInformationId int NOT NULL,
	RewardId int NOT NULL,
	RedemptionDate datetime2 NOT NULL,
	PointsSpent int NOT NULL,
	Status int NOT NULL,
	RewardId1 int NULL,
	VisitorParkInformationId1 int NULL,
	CONSTRAINT PK_RewardRedemptions PRIMARY KEY (Id),
	CONSTRAINT FK_RewardRedemptions_Rewards_RewardId FOREIGN KEY (RewardId) REFERENCES VirtualParkDb.dbo.Rewards(Id),
	CONSTRAINT FK_RewardRedemptions_Rewards_RewardId1 FOREIGN KEY (RewardId1) REFERENCES VirtualParkDb.dbo.Rewards(Id),
	CONSTRAINT FK_RewardRedemptions_VisitorParkInformations_VisitorParkInformationId FOREIGN KEY (VisitorParkInformationId) REFERENCES VirtualParkDb.dbo.VisitorParkInformations(Id),
	CONSTRAINT FK_RewardRedemptions_VisitorParkInformations_VisitorParkInformationId1 FOREIGN KEY (VisitorParkInformationId1) REFERENCES VirtualParkDb.dbo.VisitorParkInformations(Id)
);
 CREATE NONCLUSTERED INDEX IX_RewardRedemptions_RewardId ON VirtualParkDb.dbo.RewardRedemptions (  RewardId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_RewardRedemptions_RewardId1 ON VirtualParkDb.dbo.RewardRedemptions (  RewardId1 ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_RewardRedemptions_VisitorParkInformationId ON VirtualParkDb.dbo.RewardRedemptions (  VisitorParkInformationId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_RewardRedemptions_VisitorParkInformationId1 ON VirtualParkDb.dbo.RewardRedemptions (  VisitorParkInformationId1 ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- VirtualParkDb.dbo.Tickets definition

-- Drop table

-- DROP TABLE VirtualParkDb.dbo.Tickets;

CREATE TABLE VirtualParkDb.dbo.Tickets (
	Id int IDENTITY(1,1) NOT NULL,
	VisitorParkInformationId int NOT NULL,
	[Type] nvarchar(MAX) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Date] datetime2 NOT NULL,
	QR uniqueidentifier DEFAULT newid() NOT NULL,
	Consumed bit NOT NULL,
	SpecialEventId int NULL,
	CONSTRAINT PK_Tickets PRIMARY KEY (Id),
	CONSTRAINT FK_Tickets_SpecialEvents_SpecialEventId FOREIGN KEY (SpecialEventId) REFERENCES VirtualParkDb.dbo.SpecialEvents(Id) ON DELETE SET NULL,
	CONSTRAINT FK_Tickets_VisitorParkInformations_VisitorParkInformationId FOREIGN KEY (VisitorParkInformationId) REFERENCES VirtualParkDb.dbo.VisitorParkInformations(Id) ON DELETE CASCADE
);
 CREATE UNIQUE NONCLUSTERED INDEX IX_Tickets_QR ON VirtualParkDb.dbo.Tickets (  QR ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_Tickets_SpecialEventId ON VirtualParkDb.dbo.Tickets (  SpecialEventId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_Tickets_VisitorParkInformationId ON VirtualParkDb.dbo.Tickets (  VisitorParkInformationId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;


-- VirtualParkDb.dbo.VisitedAttractions definition

-- Drop table

-- DROP TABLE VirtualParkDb.dbo.VisitedAttractions;

CREATE TABLE VirtualParkDb.dbo.VisitedAttractions (
	VisitorParkInformationId int NOT NULL,
	AttractionId int NOT NULL,
	VisitDate datetime2 NOT NULL,
	VisitorParkInformationId1 int NULL,
	AttractionType int DEFAULT 0 NOT NULL,
	IsSpecialEvent bit DEFAULT CONVERT([bit],(0)) NOT NULL,
	CONSTRAINT PK_VisitedAttractions PRIMARY KEY (VisitorParkInformationId,AttractionId,VisitDate),
	CONSTRAINT FK_VisitedAttractions_Attractions_AttractionId FOREIGN KEY (AttractionId) REFERENCES VirtualParkDb.dbo.Attractions(Id) ON DELETE CASCADE,
	CONSTRAINT FK_VisitedAttractions_VisitorParkInformations_VisitorParkInformationId FOREIGN KEY (VisitorParkInformationId) REFERENCES VirtualParkDb.dbo.VisitorParkInformations(Id) ON DELETE CASCADE,
	CONSTRAINT FK_VisitedAttractions_VisitorParkInformations_VisitorParkInformationId1 FOREIGN KEY (VisitorParkInformationId1) REFERENCES VirtualParkDb.dbo.VisitorParkInformations(Id)
);
 CREATE NONCLUSTERED INDEX IX_VisitedAttractions_AttractionId ON VirtualParkDb.dbo.VisitedAttractions (  AttractionId ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;
 CREATE NONCLUSTERED INDEX IX_VisitedAttractions_VisitorParkInformationId1 ON VirtualParkDb.dbo.VisitedAttractions (  VisitorParkInformationId1 ASC  )  
	 WITH (  PAD_INDEX = OFF ,FILLFACTOR = 100  ,SORT_IN_TEMPDB = OFF , IGNORE_DUP_KEY = OFF , STATISTICS_NORECOMPUTE = OFF , ONLINE = OFF , ALLOW_ROW_LOCKS = ON , ALLOW_PAGE_LOCKS = ON  )
	 ON [PRIMARY ] ;