CREATE TRIGGER ScientificDirectorSafeTrigger on ScientificDirectors
INSTEAD OF INSERT
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @realyHave int;
	SELECT @realyHave = COUNT(*) FROM ScientificDirectors WHERE
		FirstName = (SELECT FirstName FROM inserted) AND
		SecondName = (SELECT SecondName FROM inserted) AND
		Organization = (SELECT Organization FROM inserted);

	IF @realyHave = 0
	BEGIN
		INSERT INTO ScientificDirectors SELECT FirstName, SecondName, LastName, Degree, Organization, Organization_Department, Organization_Post FROM inserted;
		SELECT TOP(1) * FROM ScientificDirectors WHERE Id = SCOPE_IDENTITY();
	END
	ELSE
		SELECT TOP(1) * FROM ScientificDirectors WHERE
		FirstName = (SELECT FirstName FROM inserted) AND
		SecondName = (SELECT SecondName FROM inserted) AND
		Organization = (SELECT Organization FROM inserted);
END
GO

CREATE PROCEDURE [dbo].[GetDissertations]
AS
BEGIN
    SELECT D.* FROM [Dissertations] AS D ORDER BY D.Defensed ASC, D.[Type] DESC, D.Title ASC
END
GO

CREATE PROCEDURE [dbo].[GetMembersByScienceBranch]
	@scienceBranch nvarchar(128)
AS
	SELECT U.*, M.* FROM [Users] AS U INNER JOIN [guest].[Members] AS M ON U.Id = M.Id WHERE M.Speciality_Number IN (SELECT S.Number FROM Specialities AS S WHERE S.ScienceBranch = @scienceBranch)
RETURN 0
GO

CREATE PROCEDURE [dbo].[GetScienceBranches]
AS
	SELECT S.ScienceBranch FROM Specialities AS S GROUP BY S.ScienceBranch
RETURN 0
GO

CREATE PROCEDURE [dbo].[FindDissertations]
	@phrase nvarchar(MAX)
AS
	SELECT D.* FROM Dissertations as D WHERE LOWER(D.Title) LIKE '%' + LOWER(@phrase) + '%'
RETURN 0
GO

