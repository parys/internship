DECLARE @levelAud INT, @deleted INT, @questionNumber INT, @questionCount INT, @auditionCount INT, @level INT, @answerNumber INT, @currentQuestionNumber INT, @coach NVARCHAR(MAX);
SET @levelAud = 1
SET @deleted = 0
SET @questionNumber = 1
SET @questionCount = 0
SET @level = 1
SET @answerNumber = 1
SET @currentQuestionNumber = 0
SET @coach = (select TOP(1)u.UserId from AspNetUserRoles as u join AspNetRoles as r on u.RoleId = r.Id where r.Name = 'Coach')

WHILE @levelAud < 6
	BEGIN
		insert into Auditions(Id, AudioFilePath, Level, CreationDate, Deleted, CreatorId)
		values (NEWID(),'\\www\files',@levelAud,GETUTCDATE(),@deleted, @coach),
			   (NEWID(),'\\www\files',@levelAud,GETUTCDATE(),@deleted, @coach)
		SET @levelAud = @levelAud + 1
	END;

SET @levelAud = 1
SET @deleted = 1

WHILE @levelAud < 6
	BEGIN
		insert into Auditions(Id, AudioFilePath, Level, CreationDate, Deleted, CreatorId)
		values (NEWID(),'\\www\files',@levelAud,GETUTCDATE(),@deleted, @coach),
			   (NEWID(),'\\www\files',@levelAud,GETUTCDATE(),@deleted, @coach)
		SET @levelAud = @levelAud + 1
	END;

insert into Topics(Id, TopicName, Level, CreationDate, Deleted, CreatorId)
values (NEWID(),'Weater',1, GETUTCDATE(),0, @coach), 
(NEWID(),'Robots',2, GETUTCDATE(),0, @coach),
(NEWID(),'Creatures',3, GETUTCDATE(),0, @coach),
(NEWID(),'Plants',4, GETUTCDATE(),0, @coach),
(NEWID(),'Theory of probability',5, GETUTCDATE(),0, @coach),
(NEWID(),'Mother',1, GETDATE(),0, @coach),
(NEWID(),'Bikes',2, GETDATE(),0, @coach),
(NEWID(),'Forrest',3, GETDATE(),0, @coach),
(NEWID(),'Plancton',4, GETDATE(),0, @coach),
(NEWID(),'History',5, GETDATE(),0, @coach),
(NEWID(),'Kraken',1, GETUTCDATE(),1, @coach), 
(NEWID(),'Vanya',2, GETUTCDATE(),1, @coach),
(NEWID(),'Kisel',3, GETUTCDATE(),1, @coach),
(NEWID(),'Kinza',4, GETUTCDATE(),1, @coach),
(NEWID(),'Hook',5, GETUTCDATE(),1, @coach),
(NEWID(),'Vitalur',1, GETDATE(),1, @coach),
(NEWID(),'Laptop',2, GETDATE(),1, @coach),
(NEWID(),'Pen',3, GETDATE(),1, @coach),
(NEWID(),'Sun',4, GETDATE(),1, @coach),
(NEWID(),'Money',5, GETDATE(),1, @coach)

--Level 1
WHILE @questionCount < 10
	BEGIN

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values (NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, (SELECT Id FROM Auditions WHERE Deleted = 0 And Level = @level ORDER BY Id OFFSET 0 ROWS  FETCH NEXT 1 ROWS ONLY), @coach)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values(NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, (SELECT Id FROM Auditions WHERE Deleted = 0 And Level = @level ORDER BY Id OFFSET 1 ROWS  FETCH NEXT 1 ROWS ONLY), @coach)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values (NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 1, (SELECT Id FROM Auditions WHERE Deleted = 1 And Level = @level ORDER BY Id OFFSET 0 ROWS  FETCH NEXT 1 ROWS ONLY), @coach)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values(NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 1, (SELECT Id FROM Auditions WHERE Deleted = 1 And Level = @level ORDER BY Id OFFSET 1 ROWS  FETCH NEXT 1 ROWS ONLY), @coach)

		SET @questionNumber = @questionNumber + 1

		SET @questionCount = @questionCount + 1
	END;

SET @questionCount = 0
SET @level = @level + 1

--Level 2
WHILE @questionCount < 10
	BEGIN

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values (NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, (SELECT TOP(1)Id FROM Auditions WHERE Deleted = 0 And Level = @level ORDER BY Id), @coach)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values(NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, (SELECT Id FROM Auditions WHERE Deleted = 0 And Level = @level ORDER BY Id OFFSET 1 ROWS  FETCH NEXT 1 ROWS ONLY), @coach)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values (NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 1, (SELECT TOP(1)Id FROM Auditions WHERE Deleted = 1 And Level = @level ORDER BY Id), @coach)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values(NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 1, (SELECT Id FROM Auditions WHERE Deleted = 1 And Level = @level ORDER BY Id OFFSET 1 ROWS  FETCH NEXT 1 ROWS ONLY), @coach)

		SET @questionNumber = @questionNumber + 1

		SET @questionCount = @questionCount + 1
	END;

SET @questionCount = 0
SET @level = @level + 1

--Level 3
WHILE @questionCount < 10
	BEGIN

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values (NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, (SELECT TOP(1)Id FROM Auditions WHERE Deleted = 0 And Level = @level ORDER BY Id), @coach)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values(NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, (SELECT Id FROM Auditions WHERE Deleted = 0 And Level = @level ORDER BY Id OFFSET 1 ROWS  FETCH NEXT 1 ROWS ONLY), @coach)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values (NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 1, (SELECT TOP(1)Id FROM Auditions WHERE Deleted = 1 And Level = @level ORDER BY Id), @coach)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values(NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 1, (SELECT Id FROM Auditions WHERE Deleted = 1 And Level = @level ORDER BY Id OFFSET 1 ROWS  FETCH NEXT 1 ROWS ONLY), @coach)

		SET @questionNumber = @questionNumber + 1

		SET @questionCount = @questionCount + 1
	END;

SET @questionCount = 0
SET @level = @level + 1

--Level 4
WHILE @questionCount < 10
	BEGIN

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values (NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, (SELECT TOP(1)Id FROM Auditions WHERE Deleted = 0 And Level = @level ORDER BY Id), @coach)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values(NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, (SELECT Id FROM Auditions WHERE Deleted = 0 And Level = @level ORDER BY Id OFFSET 1 ROWS  FETCH NEXT 1 ROWS ONLY), @coach)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values (NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 1, (SELECT TOP(1)Id FROM Auditions WHERE Deleted = 1 And Level = @level ORDER BY Id), @coach)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values(NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 1, (SELECT Id FROM Auditions WHERE Deleted = 1 And Level = @level ORDER BY Id OFFSET 1 ROWS  FETCH NEXT 1 ROWS ONLY), @coach)

		SET @questionNumber = @questionNumber + 1

		SET @questionCount = @questionCount + 1
	END;

SET @questionCount = 0
SET @level = @level + 1

--Level 5
WHILE @questionCount < 10
	BEGIN

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values (NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, (SELECT TOP(1)Id FROM Auditions WHERE Deleted = 0 And Level = @level ORDER BY Id), @coach)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values(NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, (SELECT Id FROM Auditions WHERE Deleted = 0 And Level = @level ORDER BY Id OFFSET 1 ROWS  FETCH NEXT 1 ROWS ONLY), @coach)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values (NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 1, (SELECT TOP(1)Id FROM Auditions WHERE Deleted = 1 And Level = @level ORDER BY Id), @coach)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values(NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 1, (SELECT Id FROM Auditions WHERE Deleted = 1 And Level = @level ORDER BY Id OFFSET 1 ROWS  FETCH NEXT 1 ROWS ONLY), @coach)

		SET @questionNumber = @questionNumber + 1

		SET @questionCount = @questionCount + 1
	END;


SET @questionCount = 0
SET @level = 1

--Level 1
WHILE @questionCount < 12
	BEGIN

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values (NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, NULL, @coach)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values(NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, NULL, @coach)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values (NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, NULL, @coach)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values(NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 1, NULL, @coach)

		SET @questionNumber = @questionNumber + 1

		SET @questionCount = @questionCount + 1
	END;

SET @questionCount = 0
SET @level = @level + 1

--Level 2----------------------------------------------------------------------------------------------------------------------------------------------
WHILE @questionCount < 12
	BEGIN

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values (NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, NULL, @coach)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values(NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, NULL, @coach)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values (NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, NULL, @coach)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values(NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 1, NULL, @coach)

		SET @questionNumber = @questionNumber + 1

		SET @questionCount = @questionCount + 1
	END;

SET @questionCount = 0
SET @level = @level + 1

--Level 3
WHILE @questionCount < 12
	BEGIN

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values (NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, NULL, @coach)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values(NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, NULL, @coach)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values (NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, NULL, @coach)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values(NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 1, NULL, @coach)

		SET @questionNumber = @questionNumber + 1

		SET @questionCount = @questionCount + 1
	END;

SET @questionCount = 0
SET @level = @level + 1

--Level 4
WHILE @questionCount < 12
	BEGIN

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values (NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, NULL, @coach)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values(NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, NULL, @coach)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values (NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, NULL, @coach)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values(NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 1, NULL, @coach)

		SET @questionNumber = @questionNumber + 1

		SET @questionCount = @questionCount + 1
	END;

SET @questionCount = 0
SET @level = @level + 1

--Level 5
WHILE @questionCount < 12
	BEGIN

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values (NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, NULL, @coach)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values(NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, NULL, @coach)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values (NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, NULL, @coach)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId, CreatorId)
		values(NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 1, NULL, @coach)

		SET @questionNumber = @questionNumber + 1

		SET @questionCount = @questionCount + 1
	END;



WHILE @currentQuestionNumber < 439
	BEGIN
		insert into Answers(Id, NameAnswer, IsRight, QuestionId)
		values(NEWID(),'right'+CONVERT(CHAR, @answerNumber),1, (SELECT q.Id FROM Questions as q ORDER BY q.NameQuestion OFFSET @currentQuestionNumber ROWS FETCH NEXT 1 ROWS ONLY))
			
		SET @answerNumber = @answerNumber + 1

		insert into Answers(Id, NameAnswer, IsRight, QuestionId)
		values(NEWID(),'wrong'+CONVERT(CHAR, @answerNumber),0, (SELECT q.Id FROM Questions as q ORDER BY q.NameQuestion OFFSET @currentQuestionNumber ROWS FETCH NEXT 1 ROWS ONLY))
			
		SET @answerNumber = @answerNumber + 1

		insert into Answers(Id, NameAnswer, IsRight, QuestionId)
		values(NEWID(),'wrong'+CONVERT(CHAR, @answerNumber),0, (SELECT q.Id FROM Questions as q ORDER BY q.NameQuestion OFFSET @currentQuestionNumber ROWS FETCH NEXT 1 ROWS ONLY))
			
		SET @answerNumber = @answerNumber + 1

		insert into Answers(Id, NameAnswer, IsRight, QuestionId)
		values(NEWID(),'wrong'+CONVERT(CHAR, @answerNumber),0, (SELECT q.Id FROM Questions as q ORDER BY q.NameQuestion OFFSET @currentQuestionNumber ROWS FETCH NEXT 1 ROWS ONLY))
			
		SET @answerNumber = @answerNumber + 1

		SET @currentQuestionNumber = @currentQuestionNumber + 4
	END;

SET @currentQuestionNumber = 1


WHILE @currentQuestionNumber < 439
	BEGIN
		insert into Answers(Id, NameAnswer, IsRight, QuestionId)
		values(NEWID(),'wrong'+CONVERT(CHAR, @answerNumber),0, (SELECT q.Id FROM Questions as q ORDER BY q.NameQuestion OFFSET @currentQuestionNumber ROWS FETCH NEXT 1 ROWS ONLY))
			
		SET @answerNumber = @answerNumber + 1

		insert into Answers(Id, NameAnswer, IsRight, QuestionId)
		values(NEWID(),'right'+CONVERT(CHAR, @answerNumber),1, (SELECT q.Id FROM Questions as q ORDER BY q.NameQuestion OFFSET @currentQuestionNumber ROWS FETCH NEXT 1 ROWS ONLY))
			
		SET @answerNumber = @answerNumber + 1

		insert into Answers(Id, NameAnswer, IsRight, QuestionId)
		values(NEWID(),'wrong'+CONVERT(CHAR, @answerNumber),0, (SELECT q.Id FROM Questions as q ORDER BY q.NameQuestion OFFSET @currentQuestionNumber ROWS FETCH NEXT 1 ROWS ONLY))
			
		SET @answerNumber = @answerNumber + 1

		insert into Answers(Id, NameAnswer, IsRight, QuestionId)
		values(NEWID(),'wrong'+CONVERT(CHAR, @answerNumber),0, (SELECT q.Id FROM Questions as q ORDER BY q.NameQuestion OFFSET @currentQuestionNumber ROWS FETCH NEXT 1 ROWS ONLY))
			
		SET @answerNumber = @answerNumber + 1

		SET @currentQuestionNumber = @currentQuestionNumber + 4
	END;

SET @currentQuestionNumber = 2


WHILE @currentQuestionNumber < 439
	BEGIN
		insert into Answers(Id, NameAnswer, IsRight, QuestionId)
		values(NEWID(),'wrong'+CONVERT(CHAR, @answerNumber),0, (SELECT q.Id FROM Questions as q ORDER BY q.NameQuestion OFFSET @currentQuestionNumber ROWS FETCH NEXT 1 ROWS ONLY))
			
		SET @answerNumber = @answerNumber + 1

		insert into Answers(Id, NameAnswer, IsRight, QuestionId)
		values(NEWID(),'wrong'+CONVERT(CHAR, @answerNumber),0, (SELECT q.Id FROM Questions as q ORDER BY q.NameQuestion OFFSET @currentQuestionNumber ROWS FETCH NEXT 1 ROWS ONLY))
			
		SET @answerNumber = @answerNumber + 1

		insert into Answers(Id, NameAnswer, IsRight, QuestionId)
		values(NEWID(),'wrong'+CONVERT(CHAR, @answerNumber),0, (SELECT q.Id FROM Questions as q ORDER BY q.NameQuestion OFFSET @currentQuestionNumber ROWS FETCH NEXT 1 ROWS ONLY))
			
		SET @answerNumber = @answerNumber + 1

		insert into Answers(Id, NameAnswer, IsRight, QuestionId)
		values(NEWID(),'right'+CONVERT(CHAR, @answerNumber),1, (SELECT q.Id FROM Questions as q ORDER BY q.NameQuestion OFFSET @currentQuestionNumber ROWS FETCH NEXT 1 ROWS ONLY))
			
		SET @answerNumber = @answerNumber + 1

		SET @currentQuestionNumber = @currentQuestionNumber + 4
	END;

SET @currentQuestionNumber = 3


WHILE @currentQuestionNumber < 439
	BEGIN
		insert into Answers(Id, NameAnswer, IsRight, QuestionId)
		values(NEWID(),'wrong'+CONVERT(CHAR, @answerNumber),0, (SELECT q.Id FROM Questions as q ORDER BY q.NameQuestion OFFSET @currentQuestionNumber ROWS FETCH NEXT 1 ROWS ONLY))
			
		SET @answerNumber = @answerNumber + 1

		insert into Answers(Id, NameAnswer, IsRight, QuestionId)
		values(NEWID(),'wrong'+CONVERT(CHAR, @answerNumber),0, (SELECT q.Id FROM Questions as q ORDER BY q.NameQuestion OFFSET @currentQuestionNumber ROWS FETCH NEXT 1 ROWS ONLY))
			
		SET @answerNumber = @answerNumber + 1

		insert into Answers(Id, NameAnswer, IsRight, QuestionId)
		values(NEWID(),'right'+CONVERT(CHAR, @answerNumber),1, (SELECT q.Id FROM Questions as q ORDER BY q.NameQuestion OFFSET @currentQuestionNumber ROWS FETCH NEXT 1 ROWS ONLY))
			
		SET @answerNumber = @answerNumber + 1

		insert into Answers(Id, NameAnswer, IsRight, QuestionId)
		values(NEWID(),'wrong'+CONVERT(CHAR, @answerNumber),0, (SELECT q.Id FROM Questions as q ORDER BY q.NameQuestion OFFSET @currentQuestionNumber ROWS FETCH NEXT 1 ROWS ONLY))
			
		SET @answerNumber = @answerNumber + 1

		SET @currentQuestionNumber = @currentQuestionNumber + 4
	END;
