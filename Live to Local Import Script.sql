USE RhymeBinderLive;

GO

/*
	LAST EXECUTED:		DURATION: 
	2025-06-13			01:07


*/

-- Importing records from database hosted on azure
/*
the following are reference tables and don't need regular import

select * from [dbo].[GroupActions]
select * from [dbo].[PublicationTypes]
select * from [dbo].[SubmissionStatuses]
select * from [dbo].[PublicationRatings]
select * from [dbo].[TextRevisionStatuses]
select * from [dbo].[TimeZones]


*/

		-- ASPNET User tables

PRINT 'UPDATE AspNetUsers'

UPDATE [AspNetUsers]
SET 
            [UserName] = ImportAspNetUsers.UserName
           ,[NormalizedUserName] = ImportAspNetUsers.NormalizedUserName
           ,[Email] = ImportAspNetUsers.Email
           ,[NormalizedEmail] = ImportAspNetUsers.NormalizedEmail
           ,[EmailConfirmed] = ImportAspNetUsers.EmailConfirmed
           ,[PasswordHash] = ImportAspNetUsers.PasswordHash
           ,[SecurityStamp] = ImportAspNetUsers.SecurityStamp
           ,[ConcurrencyStamp] = ImportAspNetUsers.ConcurrencyStamp
           ,[PhoneNumber] = ImportAspNetUsers.PhoneNumber
           ,[PhoneNumberConfirmed] = ImportAspNetUsers.PhoneNumberConfirmed
           ,[TwoFactorEnabled] = ImportAspNetUsers.TwoFactorEnabled
           ,[LockoutEnd] = ImportAspNetUsers.LockoutEnd
           ,[LockoutEnabled] = ImportAspNetUsers.LockoutEnabled
           ,[AccessFailedCount] = ImportAspNetUsers.AccessFailedCount


FROM [AspNetUsers]
INNER JOIN  [AZURE SQL DATABASE].Rhymebinder.dbo.[AspNetUsers] ImportAspNetUsers
ON ImportAspNetUsers.Id = AspNetUsers.Id


PRINT 'INSERT AspNetUsers'

INSERT INTO [dbo].[AspNetUsers]
           ([Id]
           ,[UserName]
           ,[NormalizedUserName]
           ,[Email]
           ,[NormalizedEmail]
           ,[EmailConfirmed]
           ,[PasswordHash]
           ,[SecurityStamp]
           ,[ConcurrencyStamp]
           ,[PhoneNumber]
           ,[PhoneNumberConfirmed]
           ,[TwoFactorEnabled]
           ,[LockoutEnd]
           ,[LockoutEnabled]
           ,[AccessFailedCount])

SELECT     
			[Id]
           ,[UserName]
           ,[NormalizedUserName]
           ,[Email]
           ,[NormalizedEmail]
           ,[EmailConfirmed]
           ,[PasswordHash]
           ,[SecurityStamp]
           ,[ConcurrencyStamp]
           ,[PhoneNumber]
           ,[PhoneNumberConfirmed]
           ,[TwoFactorEnabled]
           ,[LockoutEnd]
           ,[LockoutEnabled]
           ,[AccessFailedCount]
FROM [AZURE SQL DATABASE].Rhymebinder.dbo.[AspNetUsers] ImportAspNetUsers
WHERE NOT EXISTS (SELECT 1 FROM AspNetUsers WHERE ImportAspNetUsers.Id = AspNetUsers.Id)


--  Other ASPNet tables aren't used currently

-- SimpleUsers
PRINT 'UPDATE SimpleUsers'

UPDATE SimpleUsers
SET  [UserName] = ImportSimpleUsers.UserName
    ,[DefaultRecordsPerPage] = ImportSimpleUsers.DefaultRecordsPerPage
    ,[DefaultShowLineCount] = ImportSimpleUsers.DefaultShowLineCount
    ,[DefaultShowParagraphCount] = ImportSimpleUsers.DefaultShowParagraphCount
    ,[TimeZone] = ImportSimpleUsers.TimeZone

FROM SimpleUsers

INNER JOIN [AZURE SQL DATABASE].Rhymebinder.dbo.SimpleUsers ImportSimpleUsers
ON ImportSimpleUsers.UserID = SimpleUsers.UserID


PRINT 'INSERT SimpleUsers'
SET IDENTITY_INSERT [SimpleUsers] ON 

INSERT INTO [dbo].[SimpleUsers]
           ( UserId
		   ,[AspNetUserID]
           ,[UserName]
           ,[DefaultRecordsPerPage]
           ,[DefaultShowLineCount]
           ,[DefaultShowParagraphCount]
           ,[TimeZone])

SELECT 
		    UserId
		   ,[AspNetUserID]
           ,[UserName]
           ,[DefaultRecordsPerPage]
           ,[DefaultShowLineCount]
           ,[DefaultShowParagraphCount]
           ,[TimeZone]
FROM [AZURE SQL DATABASE].Rhymebinder.dbo.SimpleUsers ImportSimpleUsers
WHERE NOT EXISTS (SELECT 1 FROM SimpleUsers WHERE SimpleUsers.UserID = ImportSimpleUsers.UserID)

SET IDENTITY_INSERT [SimpleUsers] OFF 



-- BINDERS
PRINT 'UPDATE Binders'
UPDATE Binders
SET			[LastModified] = ImportBinders.LastModified
           ,[LastModifiedBy] = ImportBinders.LastAccessedBy
           ,[Hidden] = ImportBinders.Hidden
           ,[Name] = ImportBinders.Name
           ,[Description] = ImportBinders.Description
           ,[Selected] = ImportBinders.Selected
           ,[LastAccessed] = ImportBinders.LastAccessed
           ,[LastAccessedBy] = ImportBinders.LastAccessedBy
           ,[NewTextDefaultShowLineCount] = ImportBinders.NewTextDefaultShowLineCount
           ,[NewTextDefaultShowParagraphCount] = ImportBinders.NewTextDefaultShowParagraphCount
           ,[TextHeaderTitleDefaultFormat] = ImportBinders.TextHeaderTitleDefaultFormat
		   ,LastWorkedIn = ImportBinders.LastWorkedIn
		   ,LastWorkedInBy = ImportBinders.LastWorkedInBy
		   ,Color = ImportBinders.Color
		   ,[ReadOnly] = ImportBinders.[ReadOnly]

FROM Binders

INNER JOIN [AZURE SQL DATABASE].Rhymebinder.dbo.Binders ImportBinders
ON Binders.BinderID = ImportBinders.BinderID

WHERE ImportBinders.LastModified > Binders.LastModified



SET IDENTITY_INSERT Binders ON
PRINT 'INSERT Binders'
INSERT INTO [dbo].[Binders]
           (BinderID
		   ,[UserID]
           ,[Created]
           ,[CreatedBy]
           ,[LastModified]
           ,[LastModifiedBy]
           ,[Hidden]
           ,[Name]
           ,[Description]
           ,[Selected]
           ,[LastAccessed]
           ,[LastAccessedBy]
           ,[NewTextDefaultShowLineCount]
           ,[NewTextDefaultShowParagraphCount]
           ,[TextHeaderTitleDefaultFormat]
		   ,LastWorkedIn
		   ,LastWorkedInBy
		   ,Color
		   ,[ReadOnly])

SELECT			BinderID
		   ,[UserID]
           ,[Created]
           ,[CreatedBy]
           ,[LastModified]
           ,[LastModifiedBy]
           ,[Hidden]
           ,[Name]
           ,[Description]
           ,[Selected]
           ,[LastAccessed]
           ,[LastAccessedBy]
           ,[NewTextDefaultShowLineCount]
           ,[NewTextDefaultShowParagraphCount]
           ,[TextHeaderTitleDefaultFormat]
		   ,LastWorkedIn
		   ,LastWorkedInBy
		   ,Color
		   ,[ReadOnly]

FROM [AZURE SQL DATABASE].Rhymebinder.dbo.Binders ImportBinders
WHERE NOT EXISTS (SELECT 1 FROM Binders WHERE Binders.BinderID = ImportBinders.BinderID)

SET IDENTITY_INSERT Binders OFF


--	Texts

--	Eventually I will add a text cleanup job and at that point I'll need to add a delete statement?

-- no update since texts aren't edited
PRINT 'INSERT Texts'
SET IDENTITY_INSERT Texts ON

INSERT INTO Texts (TextID, TextBody, Created)
SELECT TextId, TextBody, Created

FROM [AZURE SQL DATABASE].Rhymebinder.dbo.Texts ImportTexts

WHERE NOT EXISTS (SELECT 1 FROM Texts WHERE Texts.TextID = ImportTexts.TextID)

SET IDENTITY_INSERT Texts OFF




--	SavedViews
PRINT 'UPDATE SavedViews'
UPDATE SavedViews
SET
            [SetValue] = ImportSavedViews.SetValue
           ,[SortValue] = ImportSavedViews.SortValue
           ,[Descending] = ImportSavedViews.Descending
           ,[ViewName] = ImportSavedViews.ViewName
           ,[Default] = ImportSavedViews.[Default]
           ,[Saved] = ImportSavedViews.Saved
           ,[LastView] = ImportSavedViews.LastView
           ,[LastModified] = ImportSavedViews.LastModified
           ,[LastModifiedBy] = ImportSavedViews.LastModifiedBy
           ,[Created] = ImportSavedViews.Created
           ,[CreatedBy] = ImportSavedViews.CreatedBy
           ,[VisionNumber] = ImportSavedViews.VisionNumber
           ,[RevisionStatus] = ImportSavedViews.RevisionStatus
           ,[Groups] = ImportSavedViews.Groups
		   ,[GroupSequence] = ImportSavedViews.GroupSequence
           ,[BinderID] = ImportSavedViews.BinderID
           ,[RecordsPerPage] = ImportSavedViews.RecordsPerPage
           ,[SearchValue] = ImportSavedViews.SearchValue
		   ,WordCount = ImportSavedViews.WordCount
		   ,CharacterCount = ImportSavedViews.CharacterCount

FROM SavedViews
INNER JOIN  [AZURE SQL DATABASE].Rhymebinder.dbo.SavedViews ImportSavedViews
ON ImportSavedViews.SavedViewID = SavedViews.SavedViewID

WHERE		SavedViews.[SetValue] <> ImportSavedViews.SetValue
           OR SavedViews.[SortValue] <> ImportSavedViews.SortValue
           OR SavedViews.[Descending] <> ImportSavedViews.Descending
           OR SavedViews.[ViewName] <> ImportSavedViews.ViewName
           OR SavedViews.[Default] <> ImportSavedViews.[Default]
           OR SavedViews.[Saved] <> ImportSavedViews.Saved
           OR SavedViews.[LastView] <> ImportSavedViews.LastView
           OR SavedViews.[LastModified] <> ImportSavedViews.LastModified
           OR SavedViews.[LastModifiedBy] <> ImportSavedViews.LastModifiedBy
           OR SavedViews.[Created] <> ImportSavedViews.Created
           OR SavedViews.[CreatedBy] <> ImportSavedViews.CreatedBy
           OR SavedViews.[VisionNumber] <> ImportSavedViews.VisionNumber
           OR SavedViews.[RevisionStatus] <> ImportSavedViews.RevisionStatus
           OR SavedViews.[Groups] <> ImportSavedViews.Groups
		   OR SavedViews.[GroupSequence] <> ImportSavedViews.GroupSequence
           OR SavedViews.[BinderID] <> ImportSavedViews.BinderID
           OR SavedViews.[RecordsPerPage] <> ImportSavedViews.RecordsPerPage
           OR SavedViews.[SearchValue] <> ImportSavedViews.SearchValue
		   OR SavedViews.WordCount <> ImportSavedViews.WordCount
		   OR SavedViews.CharacterCount <> ImportSavedViews.CharacterCount



PRINT 'INSERT SavedViews'
SET IDENTITY_INSERT SavedViews ON
INSERT INTO [dbo].[SavedViews]
           (SavedViewID
		   ,[UserID]
           ,[SetValue]
           ,[SortValue]
           ,[Descending]
           ,[ViewName]
           ,[Default]
           ,[Saved]
           ,[LastView]
           ,[LastModified]
           ,[LastModifiedBy]
           ,[Created]
           ,[CreatedBy]
           ,[VisionNumber]
           ,[RevisionStatus]
           ,[Groups]
		   ,[GroupSequence]
           ,[BinderID]
           ,[RecordsPerPage]
           ,[SearchValue]
		   ,WordCount
		   ,CharacterCount)
 SELECT 
			SavedViewID
		   ,[UserID]
           ,[SetValue]
           ,[SortValue]
           ,[Descending]
           ,[ViewName]
           ,[Default]
           ,[Saved]
           ,[LastView]
           ,[LastModified]
           ,[LastModifiedBy]
           ,[Created]
           ,[CreatedBy]
           ,[VisionNumber]
           ,[RevisionStatus]
           ,[Groups]
		   ,[GroupSequence]
           ,[BinderID]
           ,[RecordsPerPage]
           ,[SearchValue]
		   ,WordCount
		   ,CharacterCount

FROM  [AZURE SQL DATABASE].Rhymebinder.dbo.SavedViews ImportSavedViews
WHERE NOT EXISTS (SELECT 1 FROM SavedViews WHERE SavedViews.SavedViewID = ImportSavedViews.SavedViewID)

SET IDENTITY_INSERT SavedViews OFF



-- TO DO Publications, Submissions

--	TEXT NOTES
PRINT 'UPDATE TextNotes'

UPDATE TextNotes

SET TextNotes.Note = ImportTextNotes.Note

FROM TextNotes
INNER JOIN [AZURE SQL DATABASE].Rhymebinder.dbo.TextNotes ImportTextNotes
ON ImportTextNotes.TextNoteID = TextNotes.TextNoteId
WHERE TextNotes.Note <> ImportTextNotes.Note


SET IDENTITY_INSERT TextNotes ON

PRINT 'INSERT TextNotes'

INSERT INTO TextNotes (TextNoteId, Note)

SELECT TextNoteId, Note
FROM [AZURE SQL DATABASE].Rhymebinder.dbo.TextNotes ImportTextNotes
WHERE NOT EXISTS (SELECT 1 FROM TextNotes WHERE TextNotes.TextNoteId = ImportTextNotes.TextNoteID)

SET IDENTITY_INSERT TextNotes OFF




--		Text Headers
PRINT 'UPDATE TextHeaders'

UPDATE TextHeaders
	SET
			[TextID] = ImportTextHeaders.TextID
           ,[Title] = ImportTextHeaders.Title
           ,[LastModified] = ImportTextHeaders.LastModified
           ,[LastModifiedBy] = ImportTextHeaders.LastModifiedBy
           ,[LastRead] = ImportTextHeaders.LastRead
           ,[LastReadBy] = ImportTextHeaders.LastReadBy
           ,[TextRevisionStatusID] = ImportTextHeaders.TextRevisionStatusID
           ,[VisionNumber] = ImportTextHeaders.VisionNumber
           ,[VersionOf] = ImportTextHeaders.VersionOf
           ,[Deleted] = ImportTextHeaders.Deleted
           ,[Locked] = ImportTextHeaders.Locked
           ,[Top] = ImportTextHeaders.[Top]
           ,[BinderID] = ImportTextHeaders.BinderID
           ,[VisionCreated] = ImportTextHeaders.VisionCreated
           ,[VisionCreatedBy] = ImportTextHeaders.VisionCreatedBy
           ,[TextNoteID] = ImportTextHeaders.TextNoteID
		   ,WordCount = ImportTextHeaders.WordCount
		   ,CharacterCount = ImportTextHeaders.CharacterCount

FROM TextHeaders
INNER JOIN [AZURE SQL DATABASE].Rhymebinder.dbo.TextHeaders ImportTextHeaders
ON ImportTextHeaders.TextHeaderID = TextHeaders.TextHeaderID
WHERE ImportTextHeaders.LastModifiedBy > TextHeaders.LastModifiedBy
  OR ImportTextHeaders.LastReadBy > TextHeaders.LastReadBy
  
  PRINT 'INSERT TextHeaders'
SET IDENTITY_INSERT TextHeaders ON

INSERT INTO [dbo].[TextHeaders]
           (TextHeaderID
		   ,[TextID]
           ,[Title]
           ,[Created]
           ,[CreatedBy]
           ,[LastModified]
           ,[LastModifiedBy]
           ,[LastRead]
           ,[LastReadBy]
           ,[TextRevisionStatusID]
           ,[VisionNumber]
           ,[VersionOf]
           ,[Deleted]
           ,[Locked]
           ,[Top]
           ,[BinderID]
           ,[VisionCreated]
           ,[VisionCreatedBy]
           ,[TextNoteID]
		   ,WordCount
		   ,CharacterCount)

SELECT TextHeaderID
		   ,[TextID]
           ,[Title]
           ,[Created]
           ,[CreatedBy]
           ,[LastModified]
           ,[LastModifiedBy]
           ,[LastRead]
           ,[LastReadBy]
           ,[TextRevisionStatusID]
           ,[VisionNumber]
           ,[VersionOf]
           ,[Deleted]
           ,[Locked]
           ,[Top]
           ,[BinderID]
           ,[VisionCreated]
           ,[VisionCreatedBy]
           ,[TextNoteID]
		   ,WordCount
		   ,CharacterCount

FROM [AZURE SQL DATABASE].Rhymebinder.dbo.TextHeaders ImportTextHeaders
WHERE NOT EXISTS (SELECT 1 FROM TextHeaders WHERE ImportTextHeaders.TextHeaderID = TextHEaders.TextHeaderID)

SET IDENTITY_INSERT TextHeaders OFF




PRINT 'UPDATE TextGroups'
--	Text GRoups

UPDATE TextGroups
SET TextGroups.GroupTitle = ImportTextGroups.GroupTitle
   , TextGroups.[Hidden] = ImportTextGroups.[Hidden]
   , TextGroups.Locked = ImportTextGroups.Locked
   , textGroups.Notes = ImportTextGroups.Notes
   , TextGroups.OwnerID = ImportTextGroups.OwnerID
   , TextGroups.SavedViewID = ImportTextGroups.SavedViewID
   

FROM TextGroups
INNER JOIN [AZURE SQL DATABASE].Rhymebinder.dbo.TextGroups ImportTextGroups
ON ImportTextGroups.TextGroupID = TextGroups.TextGroupID
WHERE TextGroups.GroupTitle <> ImportTextGroups.GroupTitle
   OR TextGroups.[Hidden] <> ImportTextGroups.[Hidden]
   OR TextGroups.Locked <> ImportTextGroups.Locked
   OR textGroups.Notes <> ImportTextGroups.Notes
   OR TextGroups.OwnerID <> ImportTextGroups.OwnerID
   OR TextGroups.SavedViewID <> ImportTextGroups.SavedViewID
   

   PRINT 'INSERT TextGroups'
   SET Identity_Insert TextGroups ON

INSERT INTO [dbo].[TextGroups]
           (TextGroupID
		   ,[GroupTitle]
           ,[OwnerID]
           ,[Notes]
           ,[Locked]
           ,[Hidden]
           ,[BinderID]
           ,[SavedViewID])

SELECT TextGroupID
		  ,[GroupTitle]
           ,[OwnerID]
           ,[Notes]
           ,[Locked]
           ,[Hidden]
           ,[BinderID]
           ,[SavedViewID]

FROM [AZURE SQL DATABASE].Rhymebinder.dbo.TextGroups ImportTextGroups
WHERE NOT EXISTS (SELECT 1 FROM TextGroups WHERE TextGroups.TextGroupID = ImportTextGroups.TextGroupID)

   SET Identity_Insert TextGroups OFF


   --Skipping lnkTextSubmission for now

   -- Edit Window Properties

   PRINT 'UPDATE EditWindowProperties'
UPDATE EditWindowProperties
SET 
            [ActiveElement] = ImportEditWindowProperties.ActiveElement
           ,NoteCursorPosition = ImportEditWindowProperties.NoteCursorPosition
		   ,NoteScrollPosition = ImportEditWindowProperties.NoteScrollPosition
           ,[ShowLineCount] = ImportEditWindowProperties.ShowLineCount
		   ,BodyCursorPosition = ImportEditWindowProperties.BodyCursorPosition
		   ,BodyScrollPosition = ImportEditWindowProperties.BodyScrollPosition
		   ,TitleCursorPosition = ImportEditWindowProperties.TitleCursorPosition
           ,[ShowParagraphCount] = ImportEditWindowProperties.ShowParagraphCount

FROM EditWindowProperties
INNER JOIN [AZURE SQL DATABASE].Rhymebinder.dbo.EditWindowProperties ImportEditWindowProperties
ON ImportEditWindowProperties.EditWindowPropertyID = EditWindowProperties.EditWindowPropertyID

WHERE EditWindowProperties.ActiveElement <> ImportEditWindowProperties.ActiveElement
   OR EditWindowProperties.BodyCursorPosition <> ImportEditWindowProperties.BodyCursorPosition
      OR EditWindowProperties.NoteCursorPosition <> ImportEditWindowProperties.NoteCursorPosition
	     OR EditWindowProperties.BodyscrollPosition <> ImportEditWindowProperties.BodyscrollPosition
      OR EditWindowProperties.NotescrollPosition <> ImportEditWindowProperties.NotescrollPosition
	  OR EditWindowProperties.titleCursorPosition <> ImportEditWindowProperties.TitleCursorPosition
   OR EditWindowProperties.ShowLineCount <> ImportEditWindowProperties.ShowLineCount
   OR EditWindowProperties.ShowParagraphCount <> ImportEditWindowProperties.ShowParagraphCount


   PRINT 'INSERT EditWindowProperties'
   SET IDENTITY_INSERT EditWindowProperties ON

USE [RhymeBinderLive]
GO

INSERT INTO [dbo].[EditWindowProperties]
           (
		   EditWindowPropertyID
		   ,[UserID]
           ,[TextHeaderID]
           ,[ActiveElement]
           ,[ShowLineCount]
           ,[ShowParagraphCount]
           
           ,[BodyScrollPosition]
           ,[BodyCursorPosition]
           ,[NoteScrollPosition]
           ,[NoteCursorPosition]
           ,[TitleCursorPosition])
    
SELECT 
			EditWindowPropertyID
			,[UserID]
           ,[TextHeaderID]
           ,[ActiveElement]
           ,[ShowLineCount]
           ,[ShowParagraphCount]
           
           ,[BodyScrollPosition]
           ,[BodyCursorPosition]
           ,[NoteScrollPosition]
           ,[NoteCursorPosition]
           ,[TitleCursorPosition]
		   FROM [AZURE SQL DATABASE].Rhymebinder.dbo.EditWindowProperties ImportEditWindowProperties
		   WHERE NOT EXISTS (SELECT 1 FROM EditWindowProperties WHERE EditWindowProperties.EditWindowPropertyID = ImportEditWindowProperties.EditWindowPropertyID)

SET IDENTITY_INSERT EditWindowProperties OFF

  

  --Group History

  --not using



  --lnkTextHeadersTextGroups - will need to delete as well
  PRINT 'INSERT [lnkTextHeadersTextGroups]'
  SET IDENTITY_INSERT [lnkTextHeadersTextGroups] ON

  INSERT INTO lnkTextHeadersTextGroups (lnkHeaderGroupID, TextGroupID, TextHeaderID, [Sequence])

  SELECT lnkHeaderGroupID, TextGroupID, TextHeaderID, [Sequence]
  FROM [AZURE SQL DATABASE].Rhymebinder.dbo.[lnkTextHeadersTextGroups] Import
  WHERE NOT EXISTS (SELECT 1 FROM [lnkTextHeadersTextGroups] WHERE Import.lnkHeaderGroupID = [lnkTextHeadersTextGroups].lnkHeaderGroupID)

    SET IDENTITY_INSERT [lnkTextHeadersTextGroups] OFF

  UPDATE lnkTextHeadersTextGroups
  SET lnkTextHeadersTextGroups.[Sequence] = Import.[Sequence]
  FROM lnkTextHeadersTextGroups
  INNER JOIN [AZURE SQL DATABASE].Rhymebinder.dbo.[lnkTextHeadersTextGroups] Import
  ON Import.lnkHeaderGroupID = lnkTextHeadersTextGroups.lnkHeaderGroupID
  WHERE Import.[Sequence] <> lnkTextHeadersTextGroups.[Sequence]


	DELETE lnkTextHeadersTextGroups
	FROM lnkTextHeadersTextGroups
	WHERE NOT EXISTS (SELECT 1 FROM [AZURE SQL DATABASE].Rhymebinder.dbo.[lnkTextHeadersTextGroups] Import WHERE Import.lnkHeaderGroupID = [lnkTextHeadersTextGroups].lnkHeaderGroupID)



	-- TextRecord
	PRINT 'INSERT TextRecord'
	SET IDENTITY_INSERT TextRecord ON

INSERT INTO TextRecord(
TextRecordID
,TextHeaderID
,TextID
,UserID
,Recorded
)
SELECT
TextRecordID
,TextHeaderID
,TextID
,UserID
,Recorded

FROM  [AZURE SQL DATABASE].Rhymebinder.dbo.TextRecord ImportTextRecord
WHERE NOT EXISTS (SELECT 1 FROM TextRecord WHERE TextRecord.TextRecordID = ImportTextRecord.TextRecordId)

	SET IDENTITY_INSERT TextRecord OFF




	--Shelves
	PRINT 'UPDATE Shelves'

UPDATE Shelves
SET Shelves.UserId = ImportShelves.UserId
,Shelves.BinderId = ImportShelves.BinderId
,Shelves.ShelfLevel = ImportShelves.ShelfLevel
,Shelves.SortOrder = ImportShelves.SortOrder

FROM Shelves
INNER JOIN [AZURE SQL DATABASE].Rhymebinder.dbo.Shelves ImportShelves
ON ImportShelves.ShelfId = Shelves.ShelfId

WHERE  Shelves.UserId <> ImportShelves.UserId
OR Shelves.BinderId <> ImportShelves.BinderId
OR Shelves.ShelfLevel <> ImportShelves.ShelfLevel
OR Shelves.SortOrder <> ImportShelves.SortOrder


PRINT 'INSERT Shelves'
SET IDENTITY_INSERT Shelves ON

INSERT INTO Shelves(
 ShelfId
,UserId
,BinderId
,ShelfLevel
,SortOrder
)
SELECT 
 ShelfId
,UserId
,BinderId
,ShelfLevel
,SortOrder

FROM  [AZURE SQL DATABASE].Rhymebinder.dbo.Shelves ImportShelves
WHERE NOT EXISTS (SELECT 1 FROM Shelves WHERE Shelves.ShelfId = ImportShelves.ShelfId)
SET IDENTITY_INSERT Shelves OFF

