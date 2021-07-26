DECLARE @levelAud INT, @deleted INT, @questionNumber INT, @questionCount INT, @auditionCount INT, @level INT, @answerNumber INT, @currentQuestionNumber INT;
SET @levelAud = 1
SET @deleted = 0
SET @questionNumber = 1
SET @questionCount = 0
SET @level = 1
SET @answerNumber = 1
SET @currentQuestionNumber = 0

WHILE @levelAud < 6
	BEGIN
		insert into Auditions(Id, AudioFilePath, Level, CreationDate, Deleted)
		values (NEWID(),'\\www\files',@levelAud,GETUTCDATE(),@deleted),
			   (NEWID(),'\\www\files',@levelAud,GETUTCDATE(),@deleted)
		SET @levelAud = @levelAud + 1
	END;

SET @levelAud = 1
SET @deleted = 1

WHILE @levelAud < 6
	BEGIN
		insert into Auditions(Id, AudioFilePath, Level, CreationDate, Deleted)
		values (NEWID(),'\\www\files',@levelAud,GETUTCDATE(),@deleted),
			   (NEWID(),'\\www\files',@levelAud,GETUTCDATE(),@deleted)
		SET @levelAud = @levelAud + 1
	END;

insert into Topics(Id, TopicName, Level, CreationDate, Deleted)
values (NEWID(),'Weater',1, GETUTCDATE(),0), 
(NEWID(),'Robots',2, GETUTCDATE(),0),
(NEWID(),'Creatures',3, GETUTCDATE(),0),
(NEWID(),'Plants',4, GETUTCDATE(),0),
(NEWID(),'Theory of probability',5, GETUTCDATE(),0),
(NEWID(),'Mother',1, GETDATE(),0),
(NEWID(),'Bikes',2, GETDATE(),0),
(NEWID(),'Forrest',3, GETDATE(),0),
(NEWID(),'Plancton',4, GETDATE(),0),
(NEWID(),'History',5, GETDATE(),0),
(NEWID(),'Kraken',1, GETUTCDATE(),1), 
(NEWID(),'Vanya',2, GETUTCDATE(),1),
(NEWID(),'Kisel',3, GETUTCDATE(),1),
(NEWID(),'Kinza',4, GETUTCDATE(),1),
(NEWID(),'Hook',5, GETUTCDATE(),1),
(NEWID(),'Vitalur',1, GETDATE(),1),
(NEWID(),'Laptop',2, GETDATE(),1),
(NEWID(),'Pen',3, GETDATE(),1),
(NEWID(),'Sun',4, GETDATE(),1),
(NEWID(),'Money',5, GETDATE(),1)

--Level 1
WHILE @questionCount < 10
	BEGIN

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values (NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, (SELECT Id FROM Auditions WHERE Deleted = 0 And Level = @level ORDER BY Id OFFSET 0 ROWS  FETCH NEXT 1 ROWS ONLY))

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values(NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, (SELECT Id FROM Auditions WHERE Deleted = 0 And Level = @level ORDER BY Id OFFSET 1 ROWS  FETCH NEXT 1 ROWS ONLY))

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values (NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 1, (SELECT Id FROM Auditions WHERE Deleted = 1 And Level = @level ORDER BY Id OFFSET 0 ROWS  FETCH NEXT 1 ROWS ONLY))

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values(NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 1, (SELECT Id FROM Auditions WHERE Deleted = 1 And Level = @level ORDER BY Id OFFSET 1 ROWS  FETCH NEXT 1 ROWS ONLY))

		SET @questionNumber = @questionNumber + 1

		SET @questionCount = @questionCount + 1
	END;

SET @questionCount = 0
SET @level = @level + 1

--Level 2
WHILE @questionCount < 10
	BEGIN

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values (NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, (SELECT TOP(1)Id FROM Auditions WHERE Deleted = 0 And Level = @level ORDER BY Id))

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values(NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, (SELECT Id FROM Auditions WHERE Deleted = 0 And Level = @level ORDER BY Id OFFSET 1 ROWS  FETCH NEXT 1 ROWS ONLY))

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values (NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 1, (SELECT TOP(1)Id FROM Auditions WHERE Deleted = 1 And Level = @level ORDER BY Id))

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values(NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 1, (SELECT Id FROM Auditions WHERE Deleted = 1 And Level = @level ORDER BY Id OFFSET 1 ROWS  FETCH NEXT 1 ROWS ONLY))

		SET @questionNumber = @questionNumber + 1

		SET @questionCount = @questionCount + 1
	END;

SET @questionCount = 0
SET @level = @level + 1

--Level 3
WHILE @questionCount < 10
	BEGIN

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values (NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, (SELECT TOP(1)Id FROM Auditions WHERE Deleted = 0 And Level = @level ORDER BY Id))

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values(NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, (SELECT Id FROM Auditions WHERE Deleted = 0 And Level = @level ORDER BY Id OFFSET 1 ROWS  FETCH NEXT 1 ROWS ONLY))

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values (NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 1, (SELECT TOP(1)Id FROM Auditions WHERE Deleted = 1 And Level = @level ORDER BY Id))

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values(NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 1, (SELECT Id FROM Auditions WHERE Deleted = 1 And Level = @level ORDER BY Id OFFSET 1 ROWS  FETCH NEXT 1 ROWS ONLY))

		SET @questionNumber = @questionNumber + 1

		SET @questionCount = @questionCount + 1
	END;

SET @questionCount = 0
SET @level = @level + 1

--Level 4
WHILE @questionCount < 10
	BEGIN

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values (NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, (SELECT TOP(1)Id FROM Auditions WHERE Deleted = 0 And Level = @level ORDER BY Id))

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values(NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, (SELECT Id FROM Auditions WHERE Deleted = 0 And Level = @level ORDER BY Id OFFSET 1 ROWS  FETCH NEXT 1 ROWS ONLY))

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values (NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 1, (SELECT TOP(1)Id FROM Auditions WHERE Deleted = 1 And Level = @level ORDER BY Id))

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values(NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 1, (SELECT Id FROM Auditions WHERE Deleted = 1 And Level = @level ORDER BY Id OFFSET 1 ROWS  FETCH NEXT 1 ROWS ONLY))

		SET @questionNumber = @questionNumber + 1

		SET @questionCount = @questionCount + 1
	END;

SET @questionCount = 0
SET @level = @level + 1

--Level 5
WHILE @questionCount < 10
	BEGIN

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values (NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, (SELECT TOP(1)Id FROM Auditions WHERE Deleted = 0 And Level = @level ORDER BY Id))

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values(NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, (SELECT Id FROM Auditions WHERE Deleted = 0 And Level = @level ORDER BY Id OFFSET 1 ROWS  FETCH NEXT 1 ROWS ONLY))

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values (NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 1, (SELECT TOP(1)Id FROM Auditions WHERE Deleted = 1 And Level = @level ORDER BY Id))

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values(NEWID(),'AQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 1, (SELECT Id FROM Auditions WHERE Deleted = 1 And Level = @level ORDER BY Id OFFSET 1 ROWS  FETCH NEXT 1 ROWS ONLY))

		SET @questionNumber = @questionNumber + 1

		SET @questionCount = @questionCount + 1
	END;


SET @questionCount = 0
SET @level = 1

--Level 1
WHILE @questionCount < 12
	BEGIN

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values (NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, NULL)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values(NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, NULL)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values (NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, NULL)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values(NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 1, NULL)

		SET @questionNumber = @questionNumber + 1

		SET @questionCount = @questionCount + 1
	END;

SET @questionCount = 0
SET @level = @level + 1

--Level 2
WHILE @questionCount < 12
	BEGIN

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values (NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, NULL)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values(NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, NULL)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values (NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, NULL)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values(NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 1, NULL)

		SET @questionNumber = @questionNumber + 1

		SET @questionCount = @questionCount + 1
	END;

SET @questionCount = 0
SET @level = @level + 1

--Level 3
WHILE @questionCount < 12
	BEGIN

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values (NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, NULL)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values(NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, NULL)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values (NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, NULL)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values(NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 1, NULL)

		SET @questionNumber = @questionNumber + 1

		SET @questionCount = @questionCount + 1
	END;

SET @questionCount = 0
SET @level = @level + 1

--Level 4
WHILE @questionCount < 12
	BEGIN

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values (NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, NULL)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values(NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, NULL)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values (NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, NULL)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values(NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 1, NULL)

		SET @questionNumber = @questionNumber + 1

		SET @questionCount = @questionCount + 1
	END;

SET @questionCount = 0
SET @level = @level + 1

--Level 5
WHILE @questionCount < 12
	BEGIN

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values (NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, NULL)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values(NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, NULL)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values (NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 0, NULL)

		SET @questionNumber = @questionNumber + 1

		insert into Questions (Id, NameQuestion, Level, CreationDate, Deleted, AuditionId)
		values(NEWID(),'GQ'+CONVERT(CHAR, @questionNumber), @level, GETUTCDATE(), 1, NULL)

		SET @questionNumber = @questionNumber + 1

		SET @questionCount = @questionCount + 1
	END;


-- 3 answers
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

		SET @currentQuestionNumber = @currentQuestionNumber + 4
	END;

SET @currentQuestionNumber = 1

-- 4 answers
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

-- 4 answers
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

SET @currentQuestionNumber = 3

-- 4 answers
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
