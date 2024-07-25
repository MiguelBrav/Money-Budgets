CREATE PROCEDURE [dbo].[AccountType_Insert]
	@Name varchar(50),
	@UserId int
AS
BEGIN

	SET NOCOUNT ON;

	DECLARE @Order int;

	SELECT @Order = COALESCE(MAX([Order]),0)+1
	FROM AccountType
	WHERE UserId = @UserId

	INSERT INTO AccountType([Name],[UserId],[Order])
	VALUES (@Name,@UserId,@Order);

	SELECT SCOPE_IDENTITY();

END
GO