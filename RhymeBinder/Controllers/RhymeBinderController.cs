using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RhymeBinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RhymeBinder.Controllers
{
    [Authorize]
    public class RhymeBinderController : Controller
    {

        private readonly RhymeBinderContext _context;

        public RhymeBinderController(RhymeBinderContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            //check that a SimpleUser record has been created for this user; if not, create one;
            string aspUserID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            SimpleUser thisUser = new SimpleUser();
            try
            {
                thisUser = _context.SimpleUsers.Where(x => x.AspNetUserId == aspUserID).First();
            }
            catch
            {
                return RedirectToAction("SetupNewUser");
            }

            return View(thisUser);
        }

        [HttpGet]
        public IActionResult SetupNewUser()
        {
            string aspUserID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            SimpleUser newUser = new SimpleUser();

            newUser.AspNetUserId = aspUserID;
            return View(newUser);
        }
        [HttpPost]
        public IActionResult SetupNewUser(SimpleUser newUser)
        {
            if (ModelState.IsValid)
            {
                _context.SimpleUsers.Add(newUser);
                _context.SaveChanges();
            }
            else
            {
                return View();  //!!insert error handlin?
            }


            //create default saved view for user
            SavedView newSavedView = new SavedView()
            {
                UserId = newUser.UserId,
                SetValue = "active",
                SortValue = "title",
                Descending = false,
                Default = true,
                Saved = false,
                LastView = false
            };
            //create artificial last saved view for user
            SavedView lastSavedView = new SavedView()
            {
                UserId = newUser.UserId,
                SetValue = "active",
                SortValue = "title",
                Descending = false,
                Default = false,
                Saved = false,
                LastView = true
            };
            //create artificial last saved view for user
            SavedView deleted = new SavedView()
            {
                UserId = newUser.UserId,
                SetValue = "deleted",
                SortValue = "title",
                Descending = false,
                Default = false,
                Saved = false,
                LastView = false
            };


            if (ModelState.IsValid)
            {
                _context.SavedViews.Add(newSavedView);
                _context.SavedViews.Add(lastSavedView);
                _context.SaveChanges();
            }
            else
            {
                return View();  //!!insert error handlin?
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult StartNewTextGroup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult StartNewTextGroup(string title)
        {
            string aspUserID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            SimpleUser thisUser = _context.SimpleUsers.Where(x => x.AspNetUserId == aspUserID).First();

            //start by creating a new TextGroup
            TextGroup newTextGroup = new TextGroup();
           
            newTextGroup.GroupTitle = title;
            newTextGroup.OwnerId = thisUser.UserId;

            if (ModelState.IsValid)
            {
                _context.TextGroups.Add(newTextGroup);
                _context.SaveChanges();
            }
            else
            {
                return View();  //!!insert error handlin?
            }
            int newTextGroupID = newTextGroup.TextGroupId;

            //create a new blank Text
            Text newText = new Text();
            newText.TextBody = "";
            newText.Created = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Texts.Add(newText);
                _context.SaveChanges();
            }
            else
            {
                return View();  //!!insert error handlin?
            }


            //create a new TextHeader entry (DEFAULTS of a new TextHeader set here):
            TextHeader newTextHeader = new TextHeader();
            
            newTextHeader.TextGroupId = newTextGroupID;
            newTextHeader.Title = title;
            newTextHeader.Created = DateTime.Now;
            newTextHeader.CreatedBy = thisUser.UserId;
            newTextHeader.LastModified = DateTime.Now;
            newTextHeader.LastModifiedBy = thisUser.UserId;
            newTextHeader.VisionNumber = 1;
            newTextHeader.Deleted = false;
            newTextHeader.Locked = false;
            newTextHeader.Top = true;
            newTextHeader.TextRevisionStatusId = 1;
            newTextHeader.LastRead = DateTime.Now;
            newTextHeader.LastReadBy = thisUser.UserId;
            newTextHeader.TextId = newText.TextId;

            if (ModelState.IsValid)
            {
                _context.TextHeaders.Add(newTextHeader);
                _context.SaveChanges();
            }
            else
            {
                return View();  //!!insert error handlin?
            }

            return Redirect($"/RhymeBinder/EditText?textHeaderID={newTextHeader.TextHeaderId}");
        }

        public IActionResult ReadText(int textHeaderID)
        {
            TextHeaderBodyUserRecord thisTextHeaderBodyUserRecord = BuildTextHeaderBodyUserRecord(textHeaderID);

            TextHeader thisTextHeader = thisTextHeaderBodyUserRecord.TextHeader;

            thisTextHeader.LastRead = DateTime.Now;
            thisTextHeader.LastReadBy = thisTextHeaderBodyUserRecord.User.UserId;

            if (ModelState.IsValid)
            {
                _context.Entry(thisTextHeader).State = Microsoft.EntityFrameworkCore.EntityState.Modified;  //remember to copy paste this honkin thing
                _context.Update(thisTextHeader);
                _context.SaveChanges();
            }
            else
            {
                return View();  //!!insert error handlin?
            }

            return View(thisTextHeaderBodyUserRecord);

        }

        [HttpGet]
        public IActionResult EditText(int textHeaderID)
        {
            //Build up the TextHeaderBodyUserRecord to pass into view
            TextHeaderBodyUserRecord thisTextHeaderBodyUserRecord = BuildTextHeaderBodyUserRecord(textHeaderID);

            return View(thisTextHeaderBodyUserRecord);
        }

        [HttpPost]
        public IActionResult EditText(TextHeaderBodyUserRecord editedTextHeaderBodyUserRecord, string action)
        {
            //Check for change and only save if the text has changed
            bool unchanged;
            Text origText = new Text();

            origText = _context.Texts.Find(editedTextHeaderBodyUserRecord.TextHeader.TextId);
    
            unchanged = TextComparitor(origText.TextBody, editedTextHeaderBodyUserRecord.Text.TextBody);
           
            //also checking for status changes and title changes
            if (unchanged)
            {
                TextHeader origHeader = new TextHeader();
                origHeader = _context.TextHeaders.Find(editedTextHeaderBodyUserRecord.TextHeader.TextHeaderId);
                if (origHeader.TextRevisionStatusId != editedTextHeaderBodyUserRecord.TextHeader.TextRevisionStatusId)
                {
                    unchanged = false;
                }
                if (unchanged)
                {
                    unchanged = TextComparitor(origHeader.Title, editedTextHeaderBodyUserRecord.TextHeader.Title);
                }
            }

            if (!unchanged)
            {
                //Create a new record in the Text table 
                Text newText = new Text();
                newText.TextBody = editedTextHeaderBodyUserRecord.Text.TextBody;

                newText.Created = DateTime.Now;
                
                if (ModelState.IsValid)
                {
                    _context.Texts.Add(newText);
                    _context.SaveChanges();
                }
                else
                {
                    return View();  //!!insert error handlin?
                }

                //Create a new log entry for the TextRecord table
                TextRecord newTextRecord = new TextRecord();
                
                newTextRecord.TextHeaderId = editedTextHeaderBodyUserRecord.TextHeader.TextHeaderId;
                newTextRecord.TextId = newText.TextId;
                newTextRecord.UserId = editedTextHeaderBodyUserRecord.User.UserId;
                newTextRecord.Recorded = newText.Created;

                if (ModelState.IsValid)
                {
                    _context.TextRecords.Add(newTextRecord);
                    _context.SaveChanges();
                }
                else
                {
                    return View();  //!!insert error handlin?
                }

                //Update the TextHeader with the new TextID, etc
                TextHeader updatedTextHeader = _context.TextHeaders.Find(editedTextHeaderBodyUserRecord.TextHeader.TextHeaderId);

                updatedTextHeader.LastModified = newText.Created;
                updatedTextHeader.LastModifiedBy = editedTextHeaderBodyUserRecord.User.UserId;
                updatedTextHeader.LastRead = newText.Created;
                updatedTextHeader.LastReadBy = editedTextHeaderBodyUserRecord.User.UserId;
                updatedTextHeader.TextId = newTextRecord.TextId;
                updatedTextHeader.TextRevisionStatusId = editedTextHeaderBodyUserRecord.TextHeader.TextRevisionStatusId;
                updatedTextHeader.Title = editedTextHeaderBodyUserRecord.TextHeader.Title;


                if (ModelState.IsValid)
                {
                    _context.Entry(updatedTextHeader).State = Microsoft.EntityFrameworkCore.EntityState.Modified;  //remember to copy paste this honkin thing
                    _context.Update(updatedTextHeader);
                    _context.SaveChanges();
                }
                else
                {
                    return View();  //!!insert error handlin?
                }
                //If we are coming from autosave, update that EditWindowStatus
                EditWindowProperty thisEditWindowProperty = _context.EditWindowProperties.Where(x => x.UserId == editedTextHeaderBodyUserRecord.User.UserId
                                                                                                && x.TextHeaderId == editedTextHeaderBodyUserRecord.TextHeader.TextHeaderId).First();
                if (action == "EditText")
                {
                    thisEditWindowProperty.CursorPosition = editedTextHeaderBodyUserRecord.EditWindowProperty.CursorPosition;
                    thisEditWindowProperty.ActiveElement = editedTextHeaderBodyUserRecord.EditWindowProperty.ActiveElement;
                    thisEditWindowProperty.ShowLineCount = editedTextHeaderBodyUserRecord.EditWindowProperty.ShowLineCount;
                    thisEditWindowProperty.ShowParagraphCount = editedTextHeaderBodyUserRecord.EditWindowProperty.ShowParagraphCount;

                    if (ModelState.IsValid)
                    {
                        _context.Entry(thisEditWindowProperty).State = Microsoft.EntityFrameworkCore.EntityState.Modified;  //remember to copy paste this honkin thing
                        _context.Update(thisEditWindowProperty);
                        _context.SaveChanges();
                    }
                    else
                    {
                        return View();  //!!insert error handlin?
                    }
                }
            }

            //Where do we go from here?
            switch (action)
            {
                case "Return":
                    return RedirectToAction("ListTextsNUID");
                case "Save":
                    return Redirect($"/RhymeBinder/EditText?textHeaderID={editedTextHeaderBodyUserRecord.TextHeader.TextHeaderId}");
                case "Revision":
                    return Redirect($"/RhymeBinder/AddRevision?textHeaderID={editedTextHeaderBodyUserRecord.TextHeader.TextHeaderId}");
                default:
                    return Redirect($"/RhymeBinder/EditText?textHeaderID={editedTextHeaderBodyUserRecord.TextHeader.TextHeaderId}");
            }

        }
        
        [HttpGet]
        public IActionResult HideHeader(int textHeaderID)
        {
            TextHeaderBodyUserRecord thisTextHeader = BuildTextHeaderBodyUserRecord(textHeaderID);

            return View(thisTextHeader);
        }
        [HttpPost]
        public IActionResult HideHeader(TextHeaderBodyUserRecord textHeaderConfirmed)
        {
            //texts are "deleted" when the deleted field in the text header is true
            //no entries are actually removed from the database, however, so recoverability is always possible
            TextHeader textHeaderToHide = _context.TextHeaders.Where(x => x.TextHeaderId == textHeaderConfirmed.TextHeader.TextHeaderId).First();

            textHeaderToHide.Deleted = true;
            if (ModelState.IsValid)
            {
                _context.Entry(textHeaderToHide).State = Microsoft.EntityFrameworkCore.EntityState.Modified;  //remember to copy paste this honkin thing
                _context.Update(textHeaderToHide);
                _context.SaveChanges();
            }
            else
            {
                return View();  //!!insert error handlin?
            }

            return RedirectToAction("Index");
        }
        public IActionResult ListTextsNUID()
        {
            string aspUserID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            SimpleUser thisUser = _context.SimpleUsers.Where(x => x.AspNetUserId == aspUserID).First();
            return Redirect($"/RhymeBinder/ListTexts?userID={thisUser.UserId}");
        }
        public IActionResult ListTexts(int userID)
        {
            SavedView thisView = _context.SavedViews.Where(x => x.UserId == userID && x.LastView == true).First();
                        
            List<DisplayTextHeader> theseTextHeaders = GetTextHeaders(thisView.SavedViewId);

            return View(theseTextHeaders);
        }
        public IActionResult ScrapStack(int userID)
        {
            SavedView thisView = _context.SavedViews.Where(x => x.UserId == userID && x.SetValue == "deleted").First();
            
            List<DisplayTextHeader> theseTextHeaders = GetTextHeaders(thisView.SavedViewId);

            return View(theseTextHeaders);
        }
        public IActionResult ReviewScrappedText(int textHeaderID)
        {
            TextHeaderBodyUserRecord textHeaderBodyUserRecord = BuildTextHeaderBodyUserRecord(textHeaderID);
            return View(textHeaderBodyUserRecord);
        }
        public IActionResult RestoreScrappedText(int textHeaderID)
        {
            string aspUserID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            SimpleUser thisUser = _context.SimpleUsers.Where(x => x.AspNetUserId == aspUserID).First();

            TextHeader restoredTextHeader = _context.TextHeaders.Where(x => x.TextHeaderId == textHeaderID).First();
            restoredTextHeader.Deleted = false;

            if (ModelState.IsValid)
            {
                _context.Entry(restoredTextHeader).State = Microsoft.EntityFrameworkCore.EntityState.Modified;  //remember to copy paste this honkin thing
                _context.Update(restoredTextHeader);
                _context.SaveChanges();
            }
            else
            {
                return View();  //!!insert error handlin?
            }

            return Redirect($"/RhymeBinder/ListTexts?userID={thisUser.UserId}");
        }
        public IActionResult AddRevision(int textHeaderID)
        {
            string aspUserID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            SimpleUser thisUser = _context.SimpleUsers.Where(x => x.AspNetUserId == aspUserID).First();

            TextHeader oldTextHeader = _context.TextHeaders.Where(x => x.TextHeaderId == textHeaderID).First();
            TextHeader newTextHeader = new TextHeader();

            newTextHeader.Created = DateTime.Now;
            newTextHeader.CreatedBy = thisUser.UserId;
            newTextHeader.LastModified = DateTime.Now;
            newTextHeader.LastModifiedBy = thisUser.UserId;
            newTextHeader.LastRead = DateTime.Now;
            newTextHeader.LastReadBy = thisUser.UserId;
            newTextHeader.Deleted = false;
            newTextHeader.Locked = false;
            newTextHeader.Top = true;
            newTextHeader.TextId = oldTextHeader.TextId;
            newTextHeader.Title = oldTextHeader.Title;
            newTextHeader.TextGroupId = oldTextHeader.TextGroupId;
            newTextHeader.VersionOf = oldTextHeader.TextHeaderId;
            newTextHeader.VisionNumber = oldTextHeader.VisionNumber + 1;
            newTextHeader.TextRevisionStatusId = oldTextHeader.TextRevisionStatusId;

            oldTextHeader.Top = false;
            oldTextHeader.Locked = true;


            if (ModelState.IsValid)
            {
                _context.Entry(oldTextHeader).State = Microsoft.EntityFrameworkCore.EntityState.Modified;  //remember to copy paste this honkin thing
                _context.Update(oldTextHeader);
                _context.SaveChanges();
            }
            else
            {
                return View();  //!!insert error handlin?
            }

            if (ModelState.IsValid)
            {
                _context.TextHeaders.Add(newTextHeader);
                _context.SaveChanges();
            }
            else
            {
                return View();  //!!insert error handlin?
            }

            return Redirect($"/RhymeBinder/EditText?textHeaderID={newTextHeader.TextHeaderId}");
        }
        public IActionResult ChangeListDisplay (string change)
        {
            string aspUserID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            SimpleUser thisUser = _context.SimpleUsers.Where(x => x.AspNetUserId == aspUserID).First();

            SavedView thisView = _context.SavedViews.Where(x => x.UserId == thisUser.UserId && x.LastView == true).First();

            //when clicking the same column already sorting by, toggle desc/asc
            if (change == thisView.SortValue)
            {
                thisView.Descending = !thisView.Descending;
            }

            if(change == "default")
            {
                SavedView defaultView = _context.SavedViews.Where(x => x.UserId == thisUser.UserId && x.Default == true).First();
                thisView = defaultView;
                thisView.Default = false;
                thisView.LastView = true;
            }
            else if (change == "all" || change == "deleted" || change == "active")
            {
                thisView.SetValue = change;
            }
            else
            {
                thisView.SortValue = change;
            }

            if (ModelState.IsValid)
            {
                _context.Entry(thisView).State = Microsoft.EntityFrameworkCore.EntityState.Modified;  //remember to copy paste this honkin thing
                _context.Update(thisView);
                _context.SaveChanges();
            }
            else
            {
                return View();  //!!insert error handlin?
            }


            return Redirect($"/RhymeBinder/ListTexts?userID={thisView.UserId}");
        }

        //Utility Methods:
        public TextHeaderBodyUserRecord BuildTextHeaderBodyUserRecord(int textHeaderID)
        {
            //build up a TextHeaderBodyUserRecord to pass into a view:
            //obvs, gonna need that TextHeader:
            TextHeader thisTextHeader = _context.TextHeaders.Find(textHeaderID);
            
            //grab the related text (if it exists)
            Text thisText = new Text();

            if (thisTextHeader.TextId != null)
            {
                thisText = _context.Texts.Find(thisTextHeader.TextId);
            }

            //grab up the revision statuses for display in the dropdown list
            List<TextRevisionStatus> revisionStatuses = _context.TextRevisionStatuses.ToList();

            //grab the current revision status in a readable format for display
            string currentRevisionStatus = revisionStatuses.Single(x => x.TextRevisionStatusId == thisTextHeader.TextRevisionStatusId).TextRevisionStatus1;

            //grab the SimpleUser object for current user
            string aspUserID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            SimpleUser currentUser = _context.SimpleUsers.Where(x => x.AspNetUserId == aspUserID).First();

            //grab the SimpleUser object for the user who created the TextHeader
            SimpleUser createdUser = new SimpleUser();
            try
            {
                createdUser = _context.SimpleUsers.Where(x => x.UserId == thisTextHeader.CreatedBy).First();
            }
            catch
            {
            }

            //grab the SimpleUser object for the user who last modified the TextHeader
            SimpleUser lastModifiedUser = new SimpleUser();
            try
            {
                lastModifiedUser = _context.SimpleUsers.Where(x => x.UserId == thisTextHeader.LastModifiedBy).First();
            }
            catch
            {
            }

            //grab the EditWindowStatus for the current user/header (if it exists; if not-create it)
            EditWindowProperty thisEditWindowProperty = new EditWindowProperty();
            try
            {
                thisEditWindowProperty = _context.EditWindowProperties.Where(x => x.UserId == currentUser.UserId
                                                                               && x.TextHeaderId == textHeaderID).First();
            } catch
            {
                EditWindowProperty newEditWindowProperty = new EditWindowProperty();
                newEditWindowProperty.UserId = currentUser.UserId;
                newEditWindowProperty.TextHeaderId = textHeaderID;
                newEditWindowProperty.CursorPosition = 0;
                newEditWindowProperty.ActiveElement = "body_edit_field";
                newEditWindowProperty.ShowLineCount = 1;
                newEditWindowProperty.ShowParagraphCount = 1;
                
                if (ModelState.IsValid)
                {
                    _context.EditWindowProperties.Add(newEditWindowProperty);
                    _context.SaveChanges();
                }
            }


            //build up previous Text "visions" and TextHeaders for them
            List<TextHeader> previousTextHeaders = new List<TextHeader>();
            List<Text> previousTexts = new List<Text>();
            List<SimpleTextHeaderAndText> previousTextsAndHeaders = new List<SimpleTextHeaderAndText>();
            List<SimpleUser> users = _context.SimpleUsers.ToList();

            try
            {
                previousTextHeaders = _context.TextHeaders.Where(x => x.TextGroupId == thisTextHeader.TextGroupId && x.Top == false).ToList();
                foreach(var textHeader in previousTextHeaders)
                {
                    previousTexts.Add(_context.Texts.Where(x => x.TextId == textHeader.TextId).First());
                }

                previousTextsAndHeaders = (from TextHeader textHeader in previousTextHeaders
                                           join Text text in previousTexts
                                             on textHeader.TextId equals text.TextId
                                           join SimpleUser createdByUser in users
                                             on textHeader.CreatedBy equals createdByUser.UserId
                                           join SimpleUser modifiedUser in users
                                             on textHeader.LastModifiedBy equals modifiedUser.UserId
                                           join TextRevisionStatus revisionStatus in revisionStatuses
                                             on textHeader.TextRevisionStatusId equals revisionStatus.TextRevisionStatusId
                                           select new SimpleTextHeaderAndText
                                            {
                                              Title = textHeader.Title,
                                              TextBody = text.TextBody,
                                              VisionNumber = textHeader.VisionNumber,
                                              Created = textHeader.Created,
                                              LastModified = textHeader.LastModified,
                                              CreatedBy = createdByUser.UserName,
                                              LastModifiedBy = modifiedUser.UserName,
                                              Status = revisionStatus.TextRevisionStatus1
                                            }
                                          ).ToList();

                previousTextsAndHeaders = previousTextsAndHeaders.OrderByDescending(x => x.VisionNumber).ToList();
            }
            catch
            {
            }

            //wrap it up and send it
            TextHeaderBodyUserRecord thisTextHeaderBodyUserRecord = new TextHeaderBodyUserRecord()
            {
                TextHeader = thisTextHeader,
                Text = thisText,
                User = currentUser,
                CreatedByUser = createdUser,
                LastModifiedByUser = lastModifiedUser,
                AllRevisionStatuses = revisionStatuses,
                PreviousTexts = previousTextsAndHeaders,
                CurrentRevisionStatus = currentRevisionStatus,
                EditWindowProperty = thisEditWindowProperty
            };

            return (thisTextHeaderBodyUserRecord);
        }

        public List<DisplayTextHeader> GetTextHeaders (int savedViewID)
        {
            List<TextHeader> theseTextHeaders = new List<TextHeader>();
            SavedView thisView = _context.SavedViews.Where(x => x.SavedViewId == savedViewID).First();

            //Populate list of TextHeaders
            switch (thisView.SetValue)
            {
                case "active":
                    theseTextHeaders =  _context.TextHeaders.Where(x => x.CreatedBy == thisView.UserId &&
                                                                  x.Top == true &&
                                                                  x.Deleted == false).ToList();
                    break;

                case "deleted":
                    theseTextHeaders = _context.TextHeaders.Where(x => x.CreatedBy == thisView.UserId &&
                                                                  x.Deleted == true).ToList();
                    break;

                case "all":
                    theseTextHeaders = _context.TextHeaders.Where(x => x.CreatedBy == thisView.UserId).ToList();
                    break;
            }

            //make a list of DisplayTextHeaders and populate the written names/statuses
            List<DisplayTextHeader> theseDisplayTextHeaders = new List<DisplayTextHeader>();

            //doing this manually cause I'm a dum-dum what can't get the cast/convert right
            foreach (TextHeader textHeader in theseTextHeaders)
            {
                theseDisplayTextHeaders.Add(new DisplayTextHeader
                {
                    TextHeaderId = textHeader.TextHeaderId,
                    TextId = textHeader.TextId,
                    TextRevisionStatusId = textHeader.TextRevisionStatusId,
                    LastModifiedBy = textHeader.LastModifiedBy,
                    LastReadBy = textHeader.LastReadBy,
                    CreatedBy = textHeader.CreatedBy,
                    Title = textHeader.Title,
                    VisionNumber = textHeader.VisionNumber,
                    Created = textHeader.Created,
                    LastModified = textHeader.LastModified,
                    LastRead = textHeader.LastRead
                }
                    );
            }

            foreach (var textHeader in theseDisplayTextHeaders)
            {
                textHeader.CreatedByName = _context.SimpleUsers.Where(x => x.UserId == textHeader.CreatedBy).First().UserName;
                textHeader.ModifyByName = _context.SimpleUsers.Where(x => x.UserId == textHeader.LastModifiedBy).First().UserName;
                textHeader.ReadByName = _context.SimpleUsers.Where(x => x.UserId == textHeader.LastReadBy).First().UserName;
                textHeader.RevisionStatus = _context.TextRevisionStatuses.Where(x => x.TextRevisionStatusId == textHeader.TextRevisionStatusId).First().TextRevisionStatus1;
            }

            //sort the list based on the sort values/descending value
            if (thisView.Descending == false)
            {
                switch (thisView.SortValue)
                {
                    case "title":
                        theseDisplayTextHeaders = theseDisplayTextHeaders.OrderBy(x => x.Title).ToList();
                        break;
                    case "lastModified":
                        theseDisplayTextHeaders = theseDisplayTextHeaders.OrderBy(x => x.LastModified).ToList();
                        break;
                    case "created":
                        theseDisplayTextHeaders = theseDisplayTextHeaders.OrderBy(x => x.Created).ToList();
                        break;
                    case "createdBy":
                        theseDisplayTextHeaders = theseDisplayTextHeaders.OrderBy(x => x.CreatedByName).ToList();
                        break;
                    case "visionNumber":
                        theseDisplayTextHeaders = theseDisplayTextHeaders.OrderBy(x => x.VisionNumber).ToList();
                        break;
                    case "revision":
                        theseDisplayTextHeaders = theseDisplayTextHeaders.OrderBy(x => x.TextRevisionStatusId).ToList();
                        break;
                }
            }
            else
            {
                switch (thisView.SortValue)
                {
                    case "title":
                        theseDisplayTextHeaders = theseDisplayTextHeaders.OrderByDescending(x => x.Title).ToList();
                        break;
                    case "lastModified":
                        theseDisplayTextHeaders = theseDisplayTextHeaders.OrderByDescending(x => x.LastModified).ToList();
                        break;
                    case "created":
                        theseDisplayTextHeaders = theseDisplayTextHeaders.OrderByDescending(x => x.Created).ToList();
                        break;
                    case "createdBy":
                        theseDisplayTextHeaders = theseDisplayTextHeaders.OrderByDescending(x => x.CreatedByName).ToList();
                        break;
                    case "visionNumber":
                        theseDisplayTextHeaders = theseDisplayTextHeaders.OrderByDescending(x => x.VisionNumber).ToList();
                        break;
                    case "revision":
                        theseDisplayTextHeaders = theseDisplayTextHeaders.OrderByDescending(x => x.TextRevisionStatusId).ToList();
                        break;
                }
            }


            return (theseDisplayTextHeaders);
        }

        public bool TextComparitor (string originalText, string textToCompare)
        {
            //I don't know if it will be quicker to convert the text to a unique (or semi-unique) numeric value,
            //then compare those values, or to do straight string comparison. I'll have to test with a large text value
            bool same;

            if (originalText == textToCompare)
            {
                same = true;
            }else
            {
                same = false;
            }

            return same;
        }
    }
}
