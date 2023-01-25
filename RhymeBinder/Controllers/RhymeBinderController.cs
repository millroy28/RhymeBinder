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

        //-------USER:
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

            int defaultBinderId = CreateNewUserBinderSet(newUser.UserId);

            return RedirectToAction("Index");
        }
        //-------TEXT:
        public IActionResult StartNewText()
        {
            string aspUserID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            SimpleUser thisUser = _context.SimpleUsers.Where(x => x.AspNetUserId == aspUserID).First();

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
            int binderId = GetCurrentBinderID();
            TextHeader newTextHeader = new TextHeader();

            newTextHeader.Title = DateTime.Now.ToString();
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
            newTextHeader.BinderId = binderId;

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
                //  update that EditWindowStatus to save the view preferences for this text header
                EditWindowProperty thisEditWindowProperty = _context.EditWindowProperties.Where(x => x.UserId == editedTextHeaderBodyUserRecord.User.UserId
                                                                                                && x.TextHeaderId == editedTextHeaderBodyUserRecord.TextHeader.TextHeaderId).First();

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
        {   //grabs current user, then default view for that user, and sends viewID to ListTexts
            int userID = GetCurrentSimpleUserID();
            SavedView thisView = _context.SavedViews.Where(x => x.UserId == userID 
                                                             && x.LastView == true
                                                             && x.Binder.Selected == true).First();
            return Redirect($"/RhymeBinder/ListTexts?viewID={thisView.SavedViewId}");
        }
        [HttpGet]
        public IActionResult ListTexts(int viewID)
        {
            int userID = GetCurrentSimpleUserID();
            SavedView thisView = _context.SavedViews.Where(x => x.SavedViewId == viewID).First();
            List<TextGroup> groups = GetTextGroups();
            DisplayBinder binder = GetDisplayBinder();
            List<DisplayTextHeader> theseTextHeaders = GetTextHeaders(thisView.SavedViewId);
            List<Binder> userBinders = _context.Binders.Where(x => x.UserId == userID 
                                                                && x.Hidden == false
                                                                && x.BinderId != binder.BinderId).ToList();
            if(theseTextHeaders.Count == 0)
            {
                theseTextHeaders.Add(new DisplayTextHeader()
                {
                    BinderId = binder.BinderId,
                    
                });
            }
            DisplayTextHeadersAndSavedView theseHeadersAndSavedView = new DisplayTextHeadersAndSavedView {
                View = thisView,
                TextHeaders = theseTextHeaders,
                Groups = groups,
                Binder = binder,
                UserBinders = userBinders
            };

            //Update last accessed field in current binder
            UpdateBinderLastAccessed(binder.BinderId, userID);
            return View(theseHeadersAndSavedView);
        }
        [HttpPost]
        public IActionResult ListTexts(DisplayTextHeadersAndSavedView savedView, string action, int groupID)
        {
            SavedView viewToUpdate = new SavedView(); 

            switch (action)
            {
                case "LastView":
                    // -> TO DO: fix bug. If a group filter is currently selected, then you select All, or Active, or Scrapped...
                    //           then it over-writes the Set Value in the savedView (the view for that group) with the All/Active/Scrapped that's passed in

                    viewToUpdate = _context.SavedViews.Where(x => x.SavedViewId == savedView.View.SavedViewId).First();
                    UpdateView(viewToUpdate, savedView);
                    return Redirect($"/RhymeBinder/ListTexts?viewID={viewToUpdate.SavedViewId}");
                    break;
                case "SaveDefault":
                    viewToUpdate = _context.SavedViews.Where(x => x.UserId == savedView.View.UserId && x.Default == true).First();
                    UpdateView(viewToUpdate, savedView);
                    return Redirect($"/RhymeBinder/ListTexts?viewID={viewToUpdate.SavedViewId}");
                    break;
                case "Hide":
                    ToggleHideSelectedHeaders(savedView, true);
                    return Redirect($"/RhymeBinder/ListTexts?viewID={savedView.View.SavedViewId}");
                    break;
                case "Restore":
                    ToggleHideSelectedHeaders(savedView, false);
                    return Redirect($"/RhymeBinder/ListTexts?viewID={savedView.View.SavedViewId}");
                case "GroupAdd":
                    AddRemoveHeadersFromGroups(savedView, groupID, true);
                    return Redirect($"/RhymeBinder/ListTexts?viewID={savedView.View.SavedViewId}");
                    break;
                case "GroupRemove":
                    AddRemoveHeadersFromGroups(savedView, groupID, false);
                    return Redirect($"/RhymeBinder/ListTexts?viewID={savedView.View.SavedViewId}");
                    break;
                case "GroupFilter":
                    TextGroup group = _context.TextGroups.Where(x => x.TextGroupId == groupID).FirstOrDefault();
                    if (groupID == -1)
                    {
                        viewToUpdate = _context.SavedViews.Where(x => x.SetValue == savedView.View.SetValue
                                                                   && x.BinderId == savedView.View.BinderId).FirstOrDefault();
                    }
                    else
                    {
                        viewToUpdate = _context.SavedViews.Where(x => x.SetValue == group.GroupTitle
                                                                   && x.BinderId == savedView.View.BinderId).FirstOrDefault();
                    }
                    return Redirect($"/RhymeBinder/ListTexts?viewID={viewToUpdate.SavedViewId}");
                    break;
                case "Transfer":
                    TransferHeadersAcrossBinders(savedView, groupID);
                    return Redirect($"/RhymeBinder/ListTexts?viewID={savedView.View.SavedViewId}");
                    break;
                case "ManageGroups":
                    return RedirectToAction("ListGroups");
                case "CreateGroup":
                    return Redirect($"/RhymeBinder/CreateGroup?binderID={savedView.View.BinderId}");
                default:
                    return Redirect($"/RhymeBinder/ListTexts?viewID={savedView.View.SavedViewId}");
                    break;
            }

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
            newTextHeader.VersionOf = oldTextHeader.TextHeaderId;
            newTextHeader.VisionNumber = oldTextHeader.VisionNumber + 1;
            newTextHeader.TextRevisionStatusId = oldTextHeader.TextRevisionStatusId;
            newTextHeader.BinderId = oldTextHeader.BinderId;

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
        //-------GROUP:
        public IActionResult ListGroups()
        {
            int userID = GetCurrentSimpleUserID();
            int currentBinderID = GetCurrentBinderID();
            List<TextGroup> theseGroups = _context.TextGroups.Where(x => x.BinderId == currentBinderID).ToList();
            List<DisplayTextGroup> displayTextGroups = new List<DisplayTextGroup>();

            foreach(var group in theseGroups)
            {
                displayTextGroups.Add(new DisplayTextGroup()
                {
                    TextGroupId = group.TextGroupId,
                    BinderId = group.BinderId,
                    Binder = group.Binder,
                    GroupTitle = group.GroupTitle,
                    Notes = group.Notes,
                    Locked = group.Locked,
                    HeaderCount = _context.LnkTextHeadersTextGroups.Where(x => x.TextGroupId == group.TextGroupId).Count()
                }) ;
            }

            return View(displayTextGroups);
        }
        [HttpGet]
        public IActionResult EditGroup(int groupID)
        {
            TextGroup groupToEdit = _context.TextGroups.Where(x => x.TextGroupId == groupID).First();
            return View(groupToEdit);
        }

        [HttpPost]
        public IActionResult EditGroup(TextGroup editedGroup, string action, string verifyDeleteGroup, string verifyDeleteAll, string verifyClear)
        {
            TextGroup groupToEdit = _context.TextGroups.Where(x => x.TextGroupId == editedGroup.TextGroupId).First();
            List<LnkTextHeadersTextGroup> groupHeaderLinks = _context.LnkTextHeadersTextGroups.Where(x => x.TextGroupId == editedGroup.TextGroupId).ToList();
            List<TextHeader> textHeaders = GetTextHeadersInGroup(groupToEdit.TextGroupId);
            
            switch (action)
            {
                case "Submit Changes":
                    groupToEdit.Locked = (editedGroup.Locked == null) ? false : editedGroup.Locked;
                    groupToEdit.GroupTitle = editedGroup.GroupTitle;
                    groupToEdit.Notes = editedGroup.Notes;

                    if (ModelState.IsValid)
                    {
                        _context.Entry(groupToEdit).State = Microsoft.EntityFrameworkCore.EntityState.Modified;  //remember to copy paste this honkin thing
                        _context.Update(groupToEdit);
                        _context.SaveChanges();
                    }
                    break;
                case "Clear":
                    if (verifyClear != null)
                    {                        
                        _context.LnkTextHeadersTextGroups.RemoveRange(groupHeaderLinks);
                        _context.SaveChanges();
                    }
                    break;
                case "Delete Group":
                    if (verifyDeleteGroup != null)
                    {
                        _context.LnkTextHeadersTextGroups.RemoveRange(groupHeaderLinks);
                        _context.TextGroups.Remove(groupToEdit);
                        _context.SaveChanges();
                    }
                    break;

            }
            return RedirectToAction("ListGroups");
        }

        [HttpGet]
        public IActionResult CreateGroup(int binderID)
        {
            return View(binderID);
        }
        [HttpPost] 
        public IActionResult CreateGroup(TextGroup newGroup)
        {
            newGroup.OwnerId = GetCurrentSimpleUserID();
            newGroup.Hidden = false;
            newGroup.Locked = false;

            if (ModelState.IsValid)
            {
                _context.TextGroups.Add(newGroup);
                _context.SaveChanges();
            }

            return RedirectToAction("ListGroups");
        }
        public IActionResult ChangeListDisplay (string change)
        {   //NOT SURE THIS IS BEING USED --- DROP?
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
        public IActionResult Manage()
        {
            int userId = GetCurrentSimpleUserID();
            return View(userId);
        }
        public IActionResult ManageGroups(TextGroup group, string action)
        {
            if (action == "Add")
            {
                int userID = GetCurrentSimpleUserID();
                TextGroup newGroup = new TextGroup();

                newGroup.GroupTitle = group.GroupTitle;
                newGroup.OwnerId = userID;
                newGroup.Hidden = false;
                newGroup.Locked = false;
                newGroup.Notes = group.Notes;
                newGroup.BinderId = group.BinderId;

                //Create a saved view for sorting lists of texts on this group
                SavedView defaultView = _context.SavedViews.Where(x => x.BinderId == group.BinderId
                                                                    && x.Default == true).FirstOrDefault();

                SavedView newSavedView = new SavedView()
                {
                    UserId = userID,
                    SetValue = newGroup.GroupTitle,
                    SortValue = defaultView.SortValue,
                    Descending = defaultView.Descending,
                    ViewName = newGroup.GroupTitle,
                    Default = false,
                    Saved = defaultView.Saved,
                    LastView = defaultView.LastView,
                    LastModified = defaultView.LastModified,
                    LastModifiedBy = defaultView.LastModifiedBy,
                    Created = defaultView.Created,
                    CreatedBy = defaultView.CreatedBy,
                    VisionNumber = defaultView.VisionNumber,
                    RevisionStatus = defaultView.RevisionStatus,
                    Groups = defaultView.Groups,
                    BinderId = defaultView.BinderId
                };

                if (ModelState.IsValid)
                {
                    _context.TextGroups.Add(newGroup);
                    _context.SavedViews.Add(newSavedView);
                    _context.SaveChanges();
                }
                else
                {
                    //string msg = "ManageGroups: invalid TextGroup model?";
                    //return Redirect($"/RhymeBinder/ErrorPage?msgID=1");
                    return RedirectToAction("ErrorPage");
                    //!!insert error handlin?
                }
            }

            return RedirectToAction("ManageGroups");
        }
        //-------BINDER METHODS:
        [HttpGet]
        public IActionResult CreateBinder()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateBinder(Binder newBinder)
        {
            int userId = GetCurrentSimpleUserID();
                        
            newBinder.UserId = userId;
            newBinder.Name = newBinder.Name;
            newBinder.Description = newBinder.Description;
            newBinder.Created = DateTime.Now;
            newBinder.CreatedBy = userId;
            newBinder.Hidden = false;
            newBinder.Selected = false;


            
            if (ModelState.IsValid)
            {
                _context.Binders.Add(newBinder);
                _context.SaveChanges();
            }
            else
            {
                return View();  //!!insert error handlin?
            }

            // Create a new saved view for this binder
            CreateNewBinderViewSet(newBinder.BinderId, userId);

            //Set this new binder as the selected binder.
            OpenBinder(newBinder.BinderId);
                       
            return RedirectToAction("ListTextsNUID");

        }
        public IActionResult OpenBinder(int binderID)
        {   
            //un-mark previously selected binder
            int userID = GetCurrentSimpleUserID();
            Binder prevSelectedBinder = _context.Binders.Where(x => x.UserId == userID 
                                                                 && x.Selected==true).FirstOrDefault();
            prevSelectedBinder.Selected = false;

            //Mark selected binder as selected
            Binder selectedBinder = _context.Binders.Where(x => x.BinderId == binderID).FirstOrDefault();
            selectedBinder.Selected = true;

            //Save changes
            if (ModelState.IsValid)
            {
                _context.Entry(prevSelectedBinder).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Entry(selectedBinder).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(prevSelectedBinder);
                _context.Update(selectedBinder);
                _context.SaveChanges();
            }
            else
            {
                return View();  //!!insert error handlin?
            }
            return RedirectToAction("ListTextsNUID");

        }
        public IActionResult ListBinders()
        {
            List<DisplayBinder> binders = GetDisplayBinders();
            return View(binders);
        }
        [HttpGet]
        public IActionResult EditBinder(int binderID)
        {
            DisplayBinder binder = GetDisplayBinder(binderID);
            return View(binder);
        }
        [HttpPost]
        public IActionResult EditBinder(DisplayBinder editedBinder, string action, string verifyClear, string verifyDelete, string verifyDeleteAll)
        {
            int userId = GetCurrentSimpleUserID();

            switch (action)
            {
                case "Submit Changes":
                    {
                        Binder binderToUpdate = _context.Binders.Where(x => x.BinderId == editedBinder.BinderId).FirstOrDefault();

                        binderToUpdate.Name = editedBinder.Name;
                        binderToUpdate.Description = editedBinder.Description;
                        binderToUpdate.LastModified = DateTime.Now;
                        binderToUpdate.LastModifiedBy = userId;
                        _context.Entry(binderToUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;  //remember to copy paste this honkin thing
                        _context.Update(binderToUpdate);
                    }
                        break;

                case "Clear":
                    {
                        if(verifyClear != null)
                        {

                            Binder loosePagesBinder = _context.Binders.Where(x => x.UserId == userId
                                                                           && x.Name == "Loose Pages").FirstOrDefault();

                            List<TextHeader> binderHeaders = _context.TextHeaders.Where(x => x.BinderId == editedBinder.BinderId).ToList();
                            foreach (var header in binderHeaders)
                            {
                                header.BinderId = loosePagesBinder.BinderId;
                            }
                            if (binderHeaders.Count > 0)
                            {
                                loosePagesBinder.Hidden = false;
                            }
                            _context.TextHeaders.UpdateRange(binderHeaders);

                            _context.Entry(loosePagesBinder).State = Microsoft.EntityFrameworkCore.EntityState.Modified;  //remember to copy paste this honkin thing
                            _context.Update(loosePagesBinder);

                            List<TextGroup> binderGroups = _context.TextGroups.Where(x => x.BinderId == editedBinder.BinderId).ToList();
                            foreach (var group in binderGroups)
                            {
                                group.BinderId = loosePagesBinder.BinderId;
                            }
                            _context.TextGroups.UpdateRange(binderGroups);
                        }
                    }
                        break;
                case "Delete":
                    {
                        if(verifyDelete != null)
                        {

                            Binder binderToUpdate = _context.Binders.Where(x => x.BinderId == editedBinder.BinderId).FirstOrDefault();
                            binderToUpdate.Hidden = true;

                            Binder loosePagesBinder = _context.Binders.Where(x => x.UserId == userId
                                                                           && x.Name == "Loose Pages").FirstOrDefault();


                            List<TextHeader> binderHeaders = _context.TextHeaders.Where(x => x.BinderId == editedBinder.BinderId).ToList();
                            foreach (var header in binderHeaders)
                            {
                                header.BinderId = loosePagesBinder.BinderId;
                            }
                            if (binderHeaders.Count > 0)
                            {
                                loosePagesBinder.Hidden = false;
                            }

                            List<TextGroup> binderGroups = _context.TextGroups.Where(x => x.BinderId == editedBinder.BinderId).ToList();
                            foreach (var group in binderGroups)
                            {
                                group.BinderId = loosePagesBinder.BinderId;
                            }

                            _context.Entry(loosePagesBinder).State = Microsoft.EntityFrameworkCore.EntityState.Modified;  //remember to copy paste this honkin thing
                            _context.Update(loosePagesBinder);

                            _context.TextGroups.UpdateRange(binderGroups);

                            _context.Entry(binderToUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;  //remember to copy paste this honkin thing
                            _context.Update(binderToUpdate);

                            _context.TextHeaders.UpdateRange(binderHeaders);
                        }
                    }
                    break;
                case "DeleteAll":
                    {
                        if(verifyDeleteAll != null)
                        {
                            Binder binderToUpdate = _context.Binders.Where(x => x.BinderId == editedBinder.BinderId).FirstOrDefault();
                            binderToUpdate.Hidden = true;

                            Binder trashBinder = _context.Binders.Where(x => x.UserId == userId
                                                                           && x.Name == "Trash").FirstOrDefault();


                            List<TextHeader> binderHeaders = _context.TextHeaders.Where(x => x.BinderId == editedBinder.BinderId).ToList();
                            foreach (var header in binderHeaders)
                            {
                                header.Deleted = true;
                                header.BinderId = trashBinder.BinderId;
                            }

                            List<TextGroup> binderGroups = _context.TextGroups.Where(x => x.BinderId == editedBinder.BinderId).ToList();
                            foreach (var group in binderGroups)
                            {
                                group.BinderId = trashBinder.BinderId;
                            }

                            _context.TextGroups.UpdateRange(binderGroups);

                            _context.Entry(binderToUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;  //remember to copy paste this honkin thing
                            _context.Update(binderToUpdate);

                            _context.TextHeaders.UpdateRange(binderHeaders);
                        }
                    }
                    break;
                default:
                    return RedirectToAction("ListBinders"); 
                    break;
            }

            if (ModelState.IsValid)
            {
                _context.SaveChanges();
            }
            else
            {
                return View();  //!!insert error handlin?
            }
            return RedirectToAction("ListBinders");
        }


        public IActionResult ErrorPage()
        {
            string msg = "ManageGroups: invalid TextGroup model?";

            return View(msg);
        }

        //---------------------------------UTILITY METHODS:
        public int CreateNewUserBinderSet(int newUserId)
        {
            //create default binder
            Binder defaultBinder = new Binder()
            {
                UserId = newUserId,
                Name = "Your Binder",
                Description = "Welcome to your first binder!",
                Created = DateTime.Now,
                LastModified = DateTime.Now,
                CreatedBy = newUserId,
                LastModifiedBy = newUserId,
                Hidden = false,
                Selected = true
            };

            //create trash binder
            Binder trashBinder = new Binder()
            {
                UserId = newUserId,
                Name = "Trash",
                Description = "Texts that have been deleted (but they never go away completely).",
                Created = DateTime.Now,
                LastModified = DateTime.Now,
                CreatedBy = newUserId,
                LastModifiedBy = newUserId,
                Hidden = false,
                Selected = false
            };

            //create loose pages binder
            Binder loosePages = new Binder()
            {
                UserId = newUserId,
                Name = "Loose Pages",
                Description = "Texts without a home otherwise.",
                Created = DateTime.Now,
                LastModified = DateTime.Now,
                CreatedBy = newUserId,
                LastModifiedBy = newUserId,
                Hidden = false,
                Selected = false
            };

            if (ModelState.IsValid)
            {
                _context.Binders.Add(defaultBinder);
                _context.Binders.Add(trashBinder);
                _context.Binders.Add(loosePages);
                _context.SaveChanges();
            }
            else
            {
                return -1;  //!!insert error handlin?
            }

            //Create views for each new binder
            CreateNewBinderViewSet(defaultBinder.BinderId, newUserId);
            CreateNewBinderViewSet(trashBinder.BinderId, newUserId);
            CreateNewBinderViewSet(loosePages.BinderId, newUserId);

            return defaultBinder.BinderId;
        }
        public void CreateNewBinderViewSet(int binderId, int userId)
        {
            //NOTES: revised intentions on binder / view organization and behavior:
            /*
             * Each binder will have 3 views created for it:
             *  Active
             *  All
             *  Trash
             *  
             *  Active = not deleted
             *  Trash = "deleted"
             *  All = both Active & Trash
             * 
             * Texts can be moved to a "deleted" state within a binder and they are effectively hidden from view
             * They can also be removed from the binder and sent to the trash binder.
             * 
             * 
             */

            //create default saved view for user
            SavedView defaultView = new SavedView()
            {
                UserId = userId,
                SetValue = "Active",
                SortValue = "title",
                ViewName = "Active - AutoCreated",
                Descending = false,
                Default = true,
                Saved = false,
                LastView = true,
                Created = true,
                CreatedBy = false,
                LastModified = false,
                LastModifiedBy = false,
                VisionNumber = false,
                RevisionStatus = false,
                Groups = false,
                BinderId = binderId,

            };

            SavedView trashView = new SavedView()
            {
                UserId = userId,
                SetValue = "Hidden",
                SortValue = "title",
                ViewName = "Hidden - AutoCreated",
                Descending = false,
                Default = false,
                Saved = false,
                LastView = false,
                Created = true,
                CreatedBy = false,
                LastModified = false,
                LastModifiedBy = false,
                VisionNumber = false,
                RevisionStatus = false,
                Groups = false,
                BinderId = binderId
            };

            SavedView loosePagesView = new SavedView()
            {
                UserId = userId,
                SetValue = "All",
                SortValue = "title",
                ViewName = "All - AutoCreated",
                Descending = false,
                Default = false,
                Saved = false,
                LastView = false,
                Created = true,
                CreatedBy = false,
                LastModified = false,
                LastModifiedBy = false,
                VisionNumber = false,
                RevisionStatus = false,
                Groups = false,
                BinderId = binderId
            };


            if (ModelState.IsValid)
            {
                _context.SavedViews.Add(defaultView);
                _context.SavedViews.Add(trashView);
                _context.SavedViews.Add(loosePagesView);

                _context.SaveChanges();
            }
            else
            {
                return;   //!!insert error handlin?
            }
            return;
        }
        public void UpdateBinderLastAccessed(int binderId, int userId)
        {
            Binder thisBinder = _context.Binders.Where(x => x.BinderId == binderId).FirstOrDefault();
            thisBinder.LastAccessed = DateTime.Now;
            thisBinder.LastAccessedBy = userId;
            if (ModelState.IsValid)
            {
                _context.Entry(thisBinder).State = Microsoft.EntityFrameworkCore.EntityState.Modified;  //remember to copy paste this honkin thing
                _context.Update(thisBinder);
                _context.SaveChanges();
            }
            else
            {
                return;  //!!insert error handlin?
            }
            return;

        }
        public void UpdateView (SavedView viewToUpdate, DisplayTextHeadersAndSavedView savedView)
        {

            viewToUpdate.UserId = savedView.View.UserId;
            viewToUpdate.SetValue = savedView.View.SetValue;
            viewToUpdate.SortValue = savedView.View.SortValue;
            viewToUpdate.Descending = (bool)savedView.View.Descending;
            viewToUpdate.ViewName = savedView.View.ViewName;
            viewToUpdate.Default = (bool)savedView.View.Default;
            viewToUpdate.Saved = (bool)savedView.View.Saved;
            viewToUpdate.LastModified = (bool)savedView.View.LastModified;
            viewToUpdate.LastModifiedBy = (bool)savedView.View.LastModifiedBy;
            viewToUpdate.Created = (bool)savedView.View.Created;
            viewToUpdate.CreatedBy = (bool)savedView.View.CreatedBy;
            viewToUpdate.VisionNumber = (bool)savedView.View.VisionNumber;
            viewToUpdate.RevisionStatus = (bool)savedView.View.RevisionStatus;
            viewToUpdate.Groups = (bool)savedView.View.Groups;

            if (ModelState.IsValid)
            {
                _context.Entry(viewToUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;  //remember to copy paste this honkin thing
                _context.Update(viewToUpdate);
                _context.SaveChanges();
            }
            else
            {
                return;  //!!insert error handlin?
            }
            return;
        }
        public void TransferHeadersAcrossBinders(DisplayTextHeadersAndSavedView savedView, int newBinderId)
        {

            foreach(var header in savedView.TextHeaders)
            {
                if (header.Selected)
                {
                    TextHeader thisTextHeader = _context.TextHeaders.Where(x => x.TextHeaderId == header.TextHeaderId).First();
                    thisTextHeader.BinderId = newBinderId;
                    thisTextHeader.Deleted = false; // deleted == hidden, and I don't want transferred headers to be hidden even if they were previously

                    if (ModelState.IsValid)
                    {
                        _context.Entry(thisTextHeader).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        _context.Update(thisTextHeader);
                  
                    }
                
                }
            }

            return;
        }
       
        public void AddRemoveHeadersFromGroups (DisplayTextHeadersAndSavedView savedView, int groupID, bool add)
        {   //Get a list of all the headerIDs that we're going to update:
            List<int> headerIDsToUpdate = new List<int>();

            foreach(var header in savedView.TextHeaders)
            {
                if (header.Selected)
                {
                    headerIDsToUpdate.Add(header.TextHeaderId);
                }
            }

            List<LnkTextHeadersTextGroup> links = _context.LnkTextHeadersTextGroups.ToList();
            bool exists = true;

            if (add)
            {

                foreach(int headerID in headerIDsToUpdate)
                {
                    exists = links.Any(x => x.TextHeaderId == headerID &&
                                           x.TextGroupId == groupID);

                    if (!exists)
                    {
                        LnkTextHeadersTextGroup newLink = new LnkTextHeadersTextGroup();
                        
                        newLink.TextGroupId = groupID;
                        newLink.TextHeaderId = headerID;

                        if (ModelState.IsValid)
                        {
                            _context.LnkTextHeadersTextGroups.Add(newLink);
                            _context.SaveChanges();
                        }
                        else
                        {
                            return;  //!!insert error handlin?
                        }

                    }
                }
            }

            if (!add)
            {

                foreach (int headerID in headerIDsToUpdate)
                {
                    exists = links.Any(x => x.TextHeaderId == headerID &&
                                           x.TextGroupId == groupID);

                    if (exists)
                    {
                        LnkTextHeadersTextGroup linkToRemove = new LnkTextHeadersTextGroup();
                        
                        linkToRemove = _context.LnkTextHeadersTextGroups.Where(x => x.TextHeaderId == headerID &&
                                                                                    x.TextGroupId == groupID).First();

                        if (ModelState.IsValid)
                        {
                            _context.LnkTextHeadersTextGroups.Remove(linkToRemove);
                            _context.SaveChanges();
                        }
                        else
                        {
                            return;  //!!insert error handlin?
                        }
                    }
                }
            }

            return;
        }
        public void ToggleHideSelectedHeaders(DisplayTextHeadersAndSavedView savedView, bool hide)
        {
            foreach(var header in savedView.TextHeaders)
            {
                if (header.Selected)
                {
                    TextHeader thisTextHeader = _context.TextHeaders.Where(x => x.TextHeaderId == header.TextHeaderId).First();
                    thisTextHeader.Deleted = hide;
                    if (ModelState.IsValid)
                    {
                        _context.Entry(thisTextHeader).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        _context.Update(thisTextHeader);
                        _context.SaveChanges();
                    }
                }
            }
            return;
        }
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

                previousTextHeaders = GetPreviousVisions(textHeaderID, previousTextHeaders);

                //previousTextHeaders = _context.TextHeaders.Where(x => x.TextGroupId == thisTextHeader.TextGroupId && x.Top == false).ToList();
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
        public List<DisplayBinder> GetDisplayBinders()
        {
            int userID = GetCurrentSimpleUserID();

            List<Binder> binders = _context.Binders.Where(x => x.UserId == userID && x.Hidden == false).OrderByDescending(x => x.LastAccessed).ToList();
           
            List<TextHeader> textHeaders = _context.TextHeaders.Where(x => x.CreatedBy == userID 
                                                                        && x.Top == true 
                                                                        && x.Deleted == false).ToList();

            List<LnkTextHeadersTextGroup> groupLinks = _context.LnkTextHeadersTextGroups.ToList();
            List<TextGroup> textGroups = _context.TextGroups.Where(x => x.Owner.UserId == userID
                                                                     && x.Hidden == false).ToList();

            List<DisplayBinder> displayBinders = new List<DisplayBinder>();
            int textCount;
            int groupCount;

            foreach (var binder in binders)
            {
                textCount = textHeaders.Where(x => x.BinderId == binder.BinderId).Count();

                groupCount = (from LnkTextHeadersTextGroup lnkTextHeadersTextGroups in groupLinks
                              join TextHeader header in textHeaders
                                on lnkTextHeadersTextGroups.TextHeaderId equals header.TextHeaderId
                              join TextGroup textGroup in textGroups
                                on lnkTextHeadersTextGroups.TextGroupId equals textGroup.TextGroupId
                             where header.BinderId == binder.BinderId
                            select textGroup.TextGroupId).Distinct().Count();

                displayBinders.Add(new DisplayBinder
                {
                    BinderId = binder.BinderId,
                    UserId = binder.UserId,
                    Created = binder.Created,
                    CreatedBy = binder.CreatedBy,
                    LastModified = binder.LastModified,
                    LastModifiedBy = binder.LastModifiedBy,
                    Hidden = binder.Hidden,
                    Name = binder.Name,
                    Description = binder.Description,
                    PageCount = textCount,
                    GroupCount = groupCount,
                    Selected = binder.Selected
                }) ;

            }
            return (displayBinders);
        }
        public DisplayBinder GetDisplayBinder()
        {
            int userID = GetCurrentSimpleUserID();
            int binderID = GetCurrentBinderID();

            Binder binder = _context.Binders.Where(x => x.BinderId == binderID).FirstOrDefault();

            int textCount = _context.TextHeaders.Where(x => x.Top == true
                                                         && x.Deleted == false
                                                         && x.BinderId == binderID).Count();
                      
            int groupCount = _context.TextGroups.Where(x => x.Hidden == false
                                                         && x.BinderId == binderID).Count();

            DisplayBinder displayBinder = new DisplayBinder()
            {
                BinderId=binder.BinderId,
                UserId=binder.UserId,
                Created=binder.Created,
                CreatedBy=binder.CreatedBy,
                LastModified=binder.LastModified,
                LastModifiedBy=binder.LastModifiedBy,
                Hidden=binder.Hidden,
                Name=binder.Name,
                Description=binder.Description,
                Selected=binder.Selected,
                GroupCount=groupCount,
                PageCount=textCount,
            };
                        
            return (displayBinder);
        }
        public DisplayBinder GetDisplayBinder(int binderID)
        {
            Binder binder = _context.Binders.Where(x => x.BinderId == binderID).FirstOrDefault();

            int textCount = _context.TextHeaders.Where(x => x.Top == true
                                                         && x.Deleted == false
                                                         && x.BinderId == binderID).Count();

            int groupCount = _context.TextGroups.Where(x => x.Hidden == false
                                                         && x.BinderId == binderID).Count();

            DisplayBinder displayBinder = new DisplayBinder()
            {
                BinderId = binder.BinderId,
                UserId = binder.UserId,
                Created = binder.Created,
                CreatedBy = binder.CreatedBy,
                LastModified = binder.LastModified,
                LastModifiedBy = binder.LastModifiedBy,
                Hidden = binder.Hidden,
                Name = binder.Name,
                Description = binder.Description,
                Selected = binder.Selected,
                GroupCount = groupCount,
                PageCount = textCount,
            };

            return (displayBinder);
        }
        public List<TextGroup> GetTextGroups()
        {
            int userID = GetCurrentSimpleUserID();
            int binderID = GetCurrentBinderID();
            List<TextGroup> groups = _context.TextGroups.Where(x => x.OwnerId == userID
                                                                 && x.BinderId == binderID).ToList();
            return (groups);
        }
        public List<TextHeader> GetTextHeadersInGroup(int groupId)
        {
            List<LnkTextHeadersTextGroup> groupLinks = _context.LnkTextHeadersTextGroups.Where(x => x.TextGroupId == groupId).ToList();
            int binderID = GetCurrentBinderID();
            List<TextHeader> textHeaders = _context.TextHeaders.Where(x => x.BinderId == binderID
                                                                        && x.Top == true
                                                                        && x.Deleted == false).ToList();

            textHeaders = (from LnkTextHeadersTextGroup lnkTextHeadersTextGroup in groupLinks
                           join TextHeader joinTextHeader in textHeaders
                             on lnkTextHeadersTextGroup.TextHeaderId equals joinTextHeader.TextHeaderId
                                            select new TextHeader
                                            {
                                                TextHeaderId = joinTextHeader.TextHeaderId,
                                                TextId = joinTextHeader.TextId,
                                                Title = joinTextHeader.Title,
                                                Created = joinTextHeader.Created,
                                                CreatedBy = joinTextHeader.CreatedBy,
                                                LastModified = joinTextHeader.LastModified,
                                                LastModifiedBy = joinTextHeader.LastModifiedBy,
                                                LastRead = joinTextHeader.LastRead,
                                                LastReadBy = joinTextHeader.LastReadBy,
                                                TextRevisionStatusId = joinTextHeader.TextRevisionStatusId,
                                                VisionNumber = joinTextHeader.VisionNumber,
                                                VersionOf = joinTextHeader.VersionOf,
                                                Deleted = joinTextHeader.Deleted,
                                                Locked = joinTextHeader.Locked,
                                                Top = joinTextHeader.Top,
                                                BinderId = joinTextHeader.BinderId
                                            }
                                  ).ToList();
                return textHeaders;
        }
        public List<DisplayTextHeader> GetTextHeaders (int savedViewID)
        {
            int binderID = GetCurrentBinderID();
            
            //Get all text headers for this view
            List<TextHeader> theseTextHeaders = new List<TextHeader>();
            SavedView thisView = _context.SavedViews.Where(x => x.SavedViewId == savedViewID
                                                             && x.BinderId == binderID).First();

            //Populate a list of TextHeaders based on these hard-coded categories
            switch (thisView.SetValue)
            {
                case "Active":
                    theseTextHeaders =  _context.TextHeaders.Where(x => x.CreatedBy == thisView.UserId &&
                                                                        x.Top == true &&
                                                                        x.Deleted == false &&
                                                                        x.BinderId == binderID).ToList();
                    break;

                case "Hidden":
                    theseTextHeaders = _context.TextHeaders.Where(x => x.CreatedBy == thisView.UserId &&
                                                                       x.Deleted == true &&
                                                                       x.BinderId == binderID).ToList();
                    break;

                case "All":
                    theseTextHeaders = _context.TextHeaders.Where(x => x.CreatedBy == thisView.UserId &&
                                                                       x.BinderId == binderID).ToList();
                    break;
                default:
                    TextGroup thisGroup = _context.TextGroups.Where(x => x.GroupTitle == thisView.SetValue).FirstOrDefault();
                    theseTextHeaders = GetTextHeadersInGroup(thisGroup.TextGroupId);
                    break;

            }
                       
            //make a list of DisplayTextHeaders and populate the written names/statuses
            List<DisplayTextHeader> theseDisplayTextHeaders = new List<DisplayTextHeader>();

            //doing this manually cause I'm a dum-dum what can't get the cast/convert right
            int userID = GetCurrentSimpleUserID(); int binderId = GetCurrentBinderID();
            List<TextGroup> selectedGroups = new List<TextGroup>();
            List<TextGroup> groups = _context.TextGroups.Where(x => x.OwnerId == userID
                                                                 && x.BinderId == binderId ).ToList();
            List<LnkTextHeadersTextGroup> links = _context.LnkTextHeadersTextGroups.ToList();

            selectedGroups = (from LnkTextHeadersTextGroup lnkTextHeadersTextGroup in links
                              join TextGroup textGroup in groups
                                on lnkTextHeadersTextGroup.TextGroupId equals textGroup.TextGroupId
                              join TextHeader joinTextHeader in theseTextHeaders
                                on lnkTextHeadersTextGroup.TextHeaderId equals joinTextHeader.TextHeaderId
                            select new TextGroup
                                 {
                                     GroupTitle = textGroup.GroupTitle,
                                     TextGroupId = textGroup.TextGroupId
                                 }
                                  ).ToList();

            foreach (TextHeader textHeader in theseTextHeaders)
            {
                selectedGroups = (from LnkTextHeadersTextGroup lnkTextHeadersTextGroup in links
                                  join TextGroup textGroup in groups
                                    on lnkTextHeadersTextGroup.TextGroupId equals textGroup.TextGroupId
                                  join TextHeader joinTextHeader in theseTextHeaders
                                    on lnkTextHeadersTextGroup.TextHeaderId equals joinTextHeader.TextHeaderId
                                 where lnkTextHeadersTextGroup.TextHeaderId == textHeader.TextHeaderId
                                  select new TextGroup
                                  {
                                      GroupTitle = textGroup.GroupTitle,
                                      TextGroupId = textGroup.TextGroupId
                                  }
                      ).ToList();

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
                    LastRead = textHeader.LastRead,
                    Groups = selectedGroups
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

            //fudge a blank if there are no results
            if(theseDisplayTextHeaders.Count == 0)
            {
                DisplayTextHeader blankTextHeader = new DisplayTextHeader()
                {
                    BinderId = binderID,
                    Groups = new List<TextGroup> { new TextGroup
                    {
                        BinderId = binderID
                    } }
                };
                theseDisplayTextHeaders.Add(blankTextHeader);
            }

            //sort the list based on the sort values/descending value
            switch (thisView.SortValue)
            {
                case "Title":
                    theseDisplayTextHeaders = theseDisplayTextHeaders.OrderBy(x => x.Title).ToList();
                    break;
                case "Last Modified":
                    theseDisplayTextHeaders = theseDisplayTextHeaders.OrderBy(x => x.LastModified).ToList();
                    break;
                case "Last Modified By":
                    theseDisplayTextHeaders = theseDisplayTextHeaders.OrderBy(x => x.ModifyByName).ToList();
                    break;
                case "Created":
                    theseDisplayTextHeaders = theseDisplayTextHeaders.OrderBy(x => x.Created).ToList();
                    break;
                case "Created By":
                    theseDisplayTextHeaders = theseDisplayTextHeaders.OrderBy(x => x.CreatedByName).ToList();
                    break;
                case "Vision Number":
                    theseDisplayTextHeaders = theseDisplayTextHeaders.OrderBy(x => x.VisionNumber).ToList();
                    break;
                case "Revision":
                    theseDisplayTextHeaders = theseDisplayTextHeaders.OrderBy(x => x.TextRevisionStatusId).ToList();
                    break;
            }
            if (thisView.Descending == true)
            {
                theseDisplayTextHeaders.Reverse();
            }

            return (theseDisplayTextHeaders);
        }
        public List<TextHeader> GetPreviousVisions(int textHeaderID, List<TextHeader> prevTextHeaders)
        {
            //Recursively build out list of prevision visions for a given header
            try
            {
                int prevTextHeaderID = (int) _context.TextHeaders.Where(x => x.TextHeaderId == textHeaderID).First().VersionOf;
                prevTextHeaders.Add(_context.TextHeaders.Where(x => x.TextHeaderId == prevTextHeaderID).First());
                GetPreviousVisions(prevTextHeaderID, prevTextHeaders);
            }
            catch
            {
                return (prevTextHeaders);
            }
            return (prevTextHeaders);
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
        public int GetCurrentSimpleUserID()
        {
            string aspUserID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int thisUserID = _context.SimpleUsers.Where(x => x.AspNetUserId == aspUserID).First().UserId;
            return (thisUserID);
        }
        public int GetCurrentBinderID()
        {
            int userID = GetCurrentSimpleUserID();
            Binder currentBinder = _context.Binders.Where(x => x.UserId == userID
                                                         && x.Selected == true).FirstOrDefault();
            return currentBinder.BinderId;
        }

    }
}
