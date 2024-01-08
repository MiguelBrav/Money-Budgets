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
