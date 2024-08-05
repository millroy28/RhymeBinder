--CREATE DATABASE RhymeBinder
--GO


/*
DROP TABLE SimpleUsers
DROP TABLE TextRecord
DROP TABLE lnkTextSubmission
DROP TABLE Texts
DROP TABLE TextHeaders
DROP TABLE TextGroups
DROP TABLE Publications
DROP TABLE PublicationTypes
DROP TABLE PublicationRatings
DROP TABLE SubmissionStatuses
DROP TABLE TextRevisionStatuses
DROP TABLE Submissions
DROP TABLE SavedViews
DROP TABLE Binders
DROP TABLE EditWindowProperties
DROP TABLE GroupACtions
DROP TABLE GroupHistory

DROP TABLE lnkTextHeadersTextGroups





*/
/*
Example of organization:
1. User creates a "Rough Draft" 
	a. new TextHeader row is inserted
	b. a new TextGroup row is inserted 
		i.  Title from the Header is written to the TextGroup title
		ii. ID from the TextGroup is written to the Header
	c. new Text row is inserted with each auto-save/manual save 
		i. Text rows are not edited; just created with the foreign key updated in the TextHeader table
2. User can go back an edit this draft at any time, each time creating new Text inserts
3. When user wants to create a new revision of the draft:
	a. current TextHeader is set to Locked
	b. new TextHeader row is inserted with
		i.  same TextGroup id
		ii. VersionOf id set to the original 


*/


CREATE TABLE TimeZones (
    TimeZoneId INT PRIMARY KEY IDENTITY(1,1),
    TimeZoneName VARCHAR(50),
    UTCOffset INT
)


INSERT INTO TimeZones (TimeZoneName, UTCOffset)
VALUES ('Hawaii Standard Time', -10),
       ('Alaska Standard Time', -9),
       ('Pacific Standard Time', -8),
       ('Mountain Standard Time', -7),
       ('Central Standard Time', -6),
       ('Eastern Standard Time', -5)

	   		  
CREATE TABLE SimpleUsers (
UserID INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
AspNetUserID NVARCHAR(450) FOREIGN KEY REFERENCES AspNetUsers(ID) NOT NULL,
UserName NVARCHAR(300),
DefaultRecordsPerPage INT NOT NULL,
DefaultShowLineCount BIT NOT NULL,
DefaultShowParagraphCount BIT NOT NULL,
TimeZone INT FOREIGN KEY REFERENCES TimeZones(TimeZoneId) NOT NULL DEFAULT 6
)



CREATE TABLE Texts (
TextID INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
TextBody NVARCHAR(MAX),
Created DATETIME
)

CREATE TABLE PublicationTypes(
PublicationTypeID INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
PublicationType VARCHAR(100)
)

CREATE TABLE Binders(
BinderID INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
UserID INT FOREIGN KEY REFERENCES SimpleUsers(UserID),
Created DATETIME,
CreatedBy INT FOREIGN KEY REFERENCES SimpleUsers(UserID),
LastModified DATETIME,
LastModifiedBy INT FOREIGN KEY REFERENCES SimpleUsers(UserID),
[Hidden] BIT,
[Name] NVARCHAR(1000),
[Description] NVARCHAR(MAX),
[Selected] BIT,
NewTextDefaultShowLineCount BIT NOT NULL DEFAULT 1,
NewTextDefaultShowParagraphCount BIT NOT NULL DEFAULT 1,
TextHeaderTitleDefaultFormat NVARCHAR(200),
LastAccessed DATETIME,
LastAccessedBy INT FOREIGN KEY REFERENCES SimpleUsers(UserID)
)



CREATE TABLE PublicationRatings(
PublicationRatingID INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
PublicationRating VARCHAR(2000)
)

CREATE TABLE Publications (
PublicationID INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
[Name] NVARCHAR(200),
[URL] NVARCHAR(2000),
PublicationTypeID INT FOREIGN KEY REFERENCES PublicationTypes(PublicationTypeID),
PublicationRatingID INT FOREIGN KEY REFERENCES PublicationRatings(PublicationRatingID),
)

CREATE TABLE SubmissionStatuses (
SubmissionStatusID INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
SubmissionStatus VARCHAR(100),
)

CREATE TABLE TextRevisionStatuses (
TextRevisionStatusID INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
TextRevisionStatus VARCHAR(100)
)

CREATE TABLE SavedViews (
SavedViewID INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
UserID INT FOREIGN KEY REFERENCES SimpleUsers(UserID) NOT NULL,
SetValue NVARCHAR (20),
SortValue NVARCHAR (20),
Descending BIT,
ViewName NVARCHAR(200),
[Default] BIT,
[Saved] BIT,
[LastView] BIT,
LastModified BIT,
LastModifiedBy BIT,
Created BIT,
CreatedBy BIT,
VisionNumber BIT,
RevisionStatus BIT,
Groups BIT,
GroupSequence BIT,
BinderID INT FOREIGN KEY REFERENCES Binders(BinderID),
SearchValue nvarchar (150),
RecordsPerPage INT NOT NULL
)


CREATE TABLE TextGroups (
TextGroupID INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
GroupTitle NVARCHAR(1000),
OwnerID INT FOREIGN KEY REFERENCES SimpleUsers(UserID) NOT NULL,
Notes NVARCHAR(Max),
Locked BIT NOT NULL DEFAULT 0,
[Hidden] BIT NOT NULL DEFAULT 0,
BinderID INT FOREIGN KEY REFERENCES Binders(BinderID) NOT NULL,
SavedViewId INT FOREIGN KEY REFERENCES SavedViews(SavedViewID) NOT NULL
)

CREATE TABLE TextNotes (
TextNoteID INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
Note NVARCHAR(Max)
)


CREATE TABLE TextHeaders (
TextHeaderID INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
TextID INT FOREIGN KEY REFERENCES Texts (TextID) NOT NULL,
Title NVARCHAR(1000),
TextNoteID INT FOREIGN KEY REFERENCES TextNotes (TextNoteID) NOT NULL,
Created DATETIME,
CreatedBy INT FOREIGN KEY REFERENCES SimpleUsers(UserID),
LastModified DATETIME,
LastModifiedBy INT FOREIGN KEY REFERENCES SimpleUsers(UserID),
LastRead DATETIME,
LastReadBy INT FOREIGN KEY REFERENCES SimpleUsers(UserID),
TextRevisionStatusID INT FOREIGN KEY REFERENCES TextRevisionStatuses(TextRevisionStatusID),
VisionNumber INT,
VersionOf INT FOREIGN KEY REFERENCES TextHeaders(TextHeaderID), --parent id
VisionCreated DATETIME,
VisionCreatedBy INT FOREIGN KEY REFERENCES SimpleUsers(UserID),
Deleted BIT,
Locked BIT,
[Top] BIT,
BinderID INT FOREIGN KEY REFERENCES Binders(BinderID)
)


CREATE TABLE lnkTextHeadersTextGroups (
lnkHeaderGroupID INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
TextHeaderID INT FOREIGN KEY REFERENCES TextHeaders(TextHeaderID) NOT NULL,
TextGroupID INT FOREIGN KEY REFERENCES TextGroups(TextGroupID) NOT NULL,
[Sequence] INT
)

CREATE TABLE TextRecord (
TextRecordID INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
TextHeaderID INT FOREIGN KEY REFERENCES TextHeaders(TextHeaderID) NOT NULL,
TextID INT FOREIGN KEY REFERENCES Texts(TextID) NOT NULL,
UserID INT FOREIGN KEY REFERENCES SimpleUsers(UserID),
Recorded DATETIME
)


CREATE TABLE Submissions (
SubmissionID INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
PublicationID INT FOREIGN KEY REFERENCES Publications(PublicationID) NOT NULL,
SubmissionStatusID INT FOREIGN KEY REFERENCES SubmissionStatuses(SubmissionStatusID) NOT NULL,
TextHeaderID INT FOREIGN KEY REFERENCES TextHeaders(TextHeaderID) NOT NULL,
Created DATETIME,
LastModified DATETIME,
Submitted DATETIME,
Reply DATETIME
)

CREATE TABLE lnkTextSubmission (
lnkTextSumbissionID INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
TextHeaderID INT FOREIGN KEY REFERENCES TextHeaders(TextHeaderID) NOT NULL,
SubmissionID INT FOREIGN KEY REFERENCES Submissions(SubmissionID) NOT NULL,
Created DATETIME
)




CREATE TABLE EditWindowProperties (
EditWindowPropertyID INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
UserID INT FOREIGN KEY REFERENCES SimpleUsers(UserID),
TextHeaderID INT FOREIGN KEY REFERENCES TextHeaders(TextHeaderID) NOT NULL,
ActiveElement NVARCHAR(300),
CursorPosition INT,
ShowLineCount INT,
ShowParagraphCount INT
)

CREATE TABLE GroupActions (
GroupActionID INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
GroupAction VARCHAR(100)
)

CREATE TABLE GroupHistory (
GroupHistoryLogID INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
UserID INT FOREIGN KEY REFERENCES SimpleUsers(UserID),
TextHeaderID INT FOREIGN KEY REFERENCES TextHeaders(TextHeaderID),
TextGroupID INT FOREIGN KEY REFERENCES TextGroups(TextGroupID),
GroupActionID INT FOREIGN KEY REFERENCES GroupActions(GroupActionID),
DateLogged DATETIME
)

INSERT INTO GroupActions (GroupAction) VALUES 
('Added'),
('Removed'),
('Group Created'),
('Group Modified'),
('Group Deleted'),
('Group Locked'),
('Group Unlocked'),
('Group Hidden'),
('Group UnHidden')

INSERT INTO TextRevisionStatuses (TextRevisionStatus) VALUES
('Scraps'),
('First Draft'),
('Revised'),
('Polished'),
('Finished')

