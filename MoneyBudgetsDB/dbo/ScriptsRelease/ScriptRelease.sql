CREATE TABLE [dbo].[Users]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[Email] NVARCHAR(256) NOT NULL,
	[EmailNormalized] NVARCHAR(256) NOT NULL,
	[PswHash] NVARCHAR(MAX) NOT NULL	

	CONSTRAINT [PK_UserId] PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[OperationType]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[Description] NVARCHAR(100) NOT NULL	

	CONSTRAINT [PK_OperationId] PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[AccountType]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[Name] NVARCHAR(100) NOT NULL,
	[UserId] INT NOT NULL,
	[Order] INT NOT NULL

	CONSTRAINT [PK_AccountTypeId] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_AccountTypeId_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])

);

CREATE TABLE [dbo].[Account]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[Name] NVARCHAR(100) NOT NULL,
	[AccountTypeId] INT NOT NULL,
	[Balance] Decimal NOT NULL,
	[Description] NVARCHAR(100) NOT NULL	

	CONSTRAINT [PK_AccountId] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_AccountId_AccountTypeId] FOREIGN KEY ([AccountTypeId]) REFERENCES [dbo].[AccountType] ([Id])

);

CREATE TABLE [dbo].[Categories]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[Name] NVARCHAR(100) NOT NULL,
	[UserId] INT NOT NULL,
	[OperationTypeId] INT NOT NULL

	CONSTRAINT [PK_CategoryId] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_CategoryId_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]),
	CONSTRAINT [FK_CategoryId_OperationTypeId] FOREIGN KEY ([OperationTypeId]) REFERENCES [dbo].[OperationType] ([Id])
);

CREATE TABLE [dbo].[Transactions]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[UserId] INT NOT NULL,
	[DateTranstaction] DATETIME NOT NULL,
	[Amount] Decimal NOT NULL,
	[OperationTypeId] INT NOT NULL,
	[Notes] NVARCHAR(1000) NULL,
	[AccountId] INT NOT NULL,
	[CategoryId] INT NOT NULL

	CONSTRAINT [PK_TransactionId] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_TranstactionsId_OperationId] FOREIGN KEY ([OperationTypeId]) REFERENCES [dbo].[OperationType] ([Id]),
	CONSTRAINT [FK_TranstactionsId_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Account] ([Id]),
	CONSTRAINT [FK_TranstactionsId_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]),
	CONSTRAINT [FK_TranstactionsId_CategoryId] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Categories] ([Id])


);



