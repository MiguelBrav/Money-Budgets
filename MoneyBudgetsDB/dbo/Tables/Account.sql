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
