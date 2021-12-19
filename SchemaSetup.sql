--CREATE DATABASE RhymeBinder
--GO

USE RhymeBinder
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

CREATE TABLE SimpleUsers (
UserID INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
AspNetUserID NVARCHAR(450) FOREIGN KEY REFERENCES AspNetUsers(ID) NOT NULL,
UserName NVARCHAR(300),
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

CREATE TABLE PublicationRatings(
PublicationRatingID INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
PublicationRating VARCHAR(2000)
)

CREATE TABLE Publications (
PublicationID INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
[Name] VARCHAR(200),
[URL] VARCHAR(2000),
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

CREATE TABLE TextGroups (
TextGroupID INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
GroupTitle VARCHAR(1000),
OwnerID INT FOREIGN KEY REFERENCES SimpleUsers(UserID)
)

CREATE TABLE TextHeaders (
TextHeaderID INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
TextGroupID INT FOREIGN KEY REFERENCES TextGroups(TextGroupID) NOT NULL,
TextID INT FOREIGN KEY REFERENCES Texts (TextID),
Title VARCHAR(1000),
Created DATETIME,
CreatedBy INT FOREIGN KEY REFERENCES SimpleUsers(UserID),
LastModified DATETIME,
LastModifiedBy INT FOREIGN KEY REFERENCES SimpleUsers(UserID),
LastRead DATETIME,
LastReadBy INT FOREIGN KEY REFERENCES SimpleUsers(UserID),
TextRevisionStatusID INT FOREIGN KEY REFERENCES TextRevisionStatuses(TextRevisionStatusID),
VisionNumber INT,
VersionOf INT FOREIGN KEY REFERENCES TextHeaders(TextHeaderID), --parent id
Deleted BIT,
Locked BIT,
[Top] BIT
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


CREATE TABLE SavedViews (
SavedViewID INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
UserID INT FOREIGN KEY REFERENCES SimpleUsers(UserID),
SetValue VARCHAR (20),
SortValue VARCHAR (20),
Descending BIT,
ViewName VARCHAR(200),
[Default] BIT,
[Saved] BIT,
[LastView] BIT
)


CREATE TABLE EditWindowProperties (
EditWindowPropertyID INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
UserID INT FOREIGN KEY REFERENCES SimpleUsers(UserID),
TextHeaderID INT FOREIGN KEY REFERENCES TextHeaders(TextHeaderID) NOT NULL,
ActiveElement VARCHAR(300),
CursorPosition INT
)

INSERT INTO TextRevisionStatuses (TextRevisionStatus) VALUES
('Scraps'),
('First Draft'),
('Revised'),
('Polished'),
('Finished')

