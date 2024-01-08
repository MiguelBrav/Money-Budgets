CREATE TABLE [dbo].[AccountType]
(
	[Id] INT IDENTITY(1,1) NOT NULL, 
	[Name] NVARCHAR(100) NOT NULL,
	[UserId] INT NOT NULL,
	[Order] INT NOT NULL

	CONSTRAINT [PK_AccountTypeId] PRIMARY KEY CLUSTERED ([Id] ASC),
	CONSTRAINT [FK_AccountTypeId_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])

);
