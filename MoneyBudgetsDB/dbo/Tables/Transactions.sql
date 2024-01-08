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
