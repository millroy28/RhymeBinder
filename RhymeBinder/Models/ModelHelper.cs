using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RhymeBinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace RhymeBinder.Models
{
    public class ModelHelper 
    {
        private readonly RhymeBinderContext _context;
       // private string _aspUserId;
        public ModelHelper(RhymeBinderContext context)
        {
            _context = context;
            
        }
        // CHECK and GET methods
                //  on failures, models will return with -1 in the ID field
        public List<Binder> GetBinders(int userId, string set)
        {
            

            List<Binder> binders = new List<Binder>();

            try
            {
                if (set == "all")
                {
                    binders = _context.Binders.Where(x => x.UserId == userId).ToList();
                }
                if (set == "active_excluding_current")
                {
                    int currentBinderId = GetCurrentBinderID(userId);
                    binders = _context.Binders.Where(x => x.UserId == userId
                                                       && x.Hidden == false
                                                       && x.BinderId != currentBinderId).ToList();
                }

            }
            catch
            {

            }

            return binders;
        }
        public int GetCurrentBinderID(int userId)
        { // There should be ONE selected binder at any time
          // While usually code would return -1 on a failure
          // There are cases where another function malfunctions and fails to select a binder
          // So this function will ensure that there is always at least 1 currently selected binder...

            int binderId;
            try
            {
                binderId = _context.Binders.Single(x => x.UserId == userId
                                                     && x.Selected == true).BinderId;
            }
            catch
            {
                try
                {
                    // If no binders are returned as selected, set the most recently accessed one to true
                    Binder mostRecentlyAccessedBinder = _context.Binders.OrderByDescending(x => x.LastAccessed).Where(x => x.UserId == userId).First();
                    mostRecentlyAccessedBinder.Selected = true;
                    _context.Entry(mostRecentlyAccessedBinder).State = Microsoft.EntityFrameworkCore.EntityState.Modified;  //remember to copy paste this honkin thing
                    _context.Update(mostRecentlyAccessedBinder);
                    _context.SaveChanges();
                    binderId = mostRecentlyAccessedBinder.BinderId;
                }
                catch
                {
                    binderId = -1;
                }
            }

            return binderId;
        }
        public SimpleUser GetCurrentSimpleUser(int userId)
        {
           // string aspUserID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
           // string aspUserID = aspNetUserId;
            SimpleUser thisUser = new SimpleUser();
            try
            {
                thisUser = _context.SimpleUsers.Single(x => x.UserId == userId);
            }
            catch
            {
                thisUser.UserId = -1;
            }
            return thisUser;
        }
        public int GetCurrentSimpleUserID(string aspUserID)
        {
            // string aspUserID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            // string aspUserID = aspNetUserId; //_aspNetUserId;
            int thisUserId;
            try
            {
                thisUserId = _context.SimpleUsers.Where(x => x.AspNetUserId == aspUserID).First().UserId;
            }
            catch
            {
                thisUserId = -1;
            }
            return (thisUserId);
        }
        public SavedView GetDefaultSavedView(int userId)
        {
            int currentBinderId = GetCurrentBinderID(userId);

            SavedView defaultSavedView = new SavedView();
            try
            {
                defaultSavedView = _context.SavedViews.Single(x => x.UserId == userId
                                                                && x.BinderId == currentBinderId
                                                                && x.SetValue == "Default");
            }
            catch
            {
                defaultSavedView.SavedViewId = -1;
            }
            return defaultSavedView;
        }
        public DisplayBinder GetDisplayBinder(int userId)
        {
            int binderId = GetCurrentBinderID(userId);

            DisplayBinder displayBinder = new DisplayBinder();
            try
            {
                displayBinder = GetDisplayBinder(userId, binderId);
            }
            catch
            {
                displayBinder.BinderId = -1;
            }

            return (displayBinder);
        }
        public DisplayBinder GetDisplayBinder(int userId, int binderId)
        {
            DisplayBinder displayBinder = new DisplayBinder();
            try
            {
                Binder binder = _context.Binders.Single(x => x.BinderId == binderId);

                int textCount = _context.TextHeaders.Where(x => x.Top == true
                                                             && x.Deleted == false
                                                             && x.BinderId == binderId).Count();

                int groupCount = _context.TextGroups.Where(x => x.Hidden == false
                                                             && x.BinderId == binderId).Count();

                displayBinder.BinderId = binder.BinderId;
                displayBinder.UserId = binder.UserId;
                displayBinder.Created = binder.Created;
                displayBinder.CreatedBy = binder.CreatedBy;
                displayBinder.LastModified = binder.LastModified;
                displayBinder.LastModifiedBy = binder.LastModifiedBy;
                displayBinder.Hidden = binder.Hidden;
                displayBinder.Name = binder.Name;
                displayBinder.Description = binder.Description;
                displayBinder.Selected = binder.Selected;
                displayBinder.GroupCount = groupCount;
                displayBinder.PageCount = textCount;
            }
            catch
            {
                displayBinder.BinderId = -1;
            }
            return (displayBinder);
        }
        public List<DisplayBinder> GetDisplayBinders(int userId)
        {
            List<DisplayBinder> displayBinders = new List<DisplayBinder>();
            try
            {
                List<Binder> binders = _context.Binders.Where(x => x.UserId == userId && x.Hidden == false).OrderByDescending(x => x.LastAccessed).ToList();

                List<TextHeader> textHeaders = _context.TextHeaders.Where(x => x.CreatedBy == userId
                                                                            && x.Top == true
                                                                            && x.Deleted == false).ToList();

                List<LnkTextHeadersTextGroup> groupLinks = _context.LnkTextHeadersTextGroups.ToList();
                List<TextGroup> textGroups = _context.TextGroups.Where(x => x.Owner.UserId == userId
                                                                         && x.Hidden == false).ToList();

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
                    });

                }
            }
            catch
            {
                displayBinders.Add(new DisplayBinder() { BinderId = -1 });
            }
            return (displayBinders);
        }
        public List<DisplayTextGroup> GetDisplayTextGroups(int userId)
        {
            int binderId = GetCurrentBinderID(userId);
            List<TextGroup> groups = GetTextGroupsInBinder(userId, binderId);
            List<DisplayTextGroup> displayTextGroups = new List<DisplayTextGroup>();

            try
            {
                foreach (var group in groups)
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
                    });
                }
            }
            catch
            {
                displayTextGroups.Add(new DisplayTextGroup()
                {
                    TextGroupId = -1,
                });
            }
            return (displayTextGroups);
        }
        public List<DisplayTextHeader> GetDisplayTextHeaders(SavedView savedView)
        {
            //TO DO: make this less ponderous and introduce error handling?
            //Get all text headers for this view
            List<TextHeader> theseTextHeaders = new List<TextHeader>();

            //Populate a list of TextHeaders based on these hard-coded categories
            switch (savedView.SetValue)
            {
                case "Active":
                    theseTextHeaders = _context.TextHeaders.Where(x => x.CreatedBy == savedView.UserId &&
                                                                        x.Top == true &&
                                                                        x.Deleted == false &&
                                                                        x.BinderId == savedView.BinderId).ToList();
                    break;

                case "Hidden":
                    theseTextHeaders = _context.TextHeaders.Where(x => x.CreatedBy == savedView.UserId &&
                                                                       x.Deleted == true &&
                                                                       x.BinderId == savedView.BinderId).ToList();
                    break;

                case "All":
                    theseTextHeaders = _context.TextHeaders.Where(x => x.CreatedBy == savedView.UserId &&
                                                                       x.BinderId == savedView.BinderId).ToList();
                    break;

                default:
                    // If not one of the three above, will be a view created for a view
                    // ...and the SetValue should be the group Id. 
                    // If this fails, let's just return the active list as a fallback.
                    bool isSetValueInt = int.TryParse(savedView.SetValue, out int groupId);
                    if (isSetValueInt)
                    {
                        TextGroup group = _context.TextGroups.Single(x => x.TextGroupId == groupId);
                        theseTextHeaders = GetTextHeadersInGroup(group);
                    } else
                    {   // Returning active set in the case of a parsing failure on group ID
                        theseTextHeaders = _context.TextHeaders.Where(x => x.CreatedBy == savedView.UserId &&
                                                    x.Top == true &&
                                                    x.Deleted == false &&
                                                    x.BinderId == savedView.BinderId).ToList();
                    }
                    break;
            }

            //make a list of DisplayTextHeaders and populate the written names/statuses
            List<DisplayTextHeader> theseDisplayTextHeaders = new List<DisplayTextHeader>();

            //doing this manually cause I'm a dum-dum what can't get the cast/convert right

            List<TextGroup> selectedGroups = new List<TextGroup>(); // groups associated with text
            List<TextGroup> allGroups = _context.TextGroups.Where(x => x.BinderId == savedView.BinderId).ToList();

            List<LnkTextHeadersTextGroup> links = _context.LnkTextHeadersTextGroups.ToList();

            foreach (TextHeader textHeader in theseTextHeaders)
            {
                selectedGroups = (from LnkTextHeadersTextGroup lnkTextHeadersTextGroup in links
                                  join TextGroup textGroup in allGroups
                                    on lnkTextHeadersTextGroup.TextGroupId equals textGroup.TextGroupId
                                  join TextHeader joinTextHeader in theseTextHeaders
                                    on lnkTextHeadersTextGroup.TextHeaderId equals joinTextHeader.TextHeaderId
                                  where lnkTextHeadersTextGroup.TextHeaderId == textHeader.TextHeaderId
                                  select new TextGroup
                                  {
                                      GroupTitle = textGroup.GroupTitle,
                                      TextGroupId = textGroup.TextGroupId,
                                      SavedViewId = textGroup.SavedViewId
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
                    Groups = selectedGroups,
                    CreatedByName = _context.SimpleUsers.Where(x => x.UserId == textHeader.CreatedBy).First().UserName,
                    ModifyByName = _context.SimpleUsers.Where(x => x.UserId == textHeader.LastModifiedBy).First().UserName,
                    ReadByName = _context.SimpleUsers.Where(x => x.UserId == textHeader.LastReadBy).First().UserName,
                    RevisionStatus = _context.TextRevisionStatuses.Single(x => x.TextRevisionStatusId == textHeader.TextRevisionStatusId).TextRevisionStatus1
                }
                    );
            }

            //fudge a blank if there are no results
            if (theseDisplayTextHeaders.Count == 0)
            {
                DisplayTextHeader blankTextHeader = new DisplayTextHeader()
                {
                    BinderId = savedView.BinderId,
                    Groups = new List<TextGroup> { new TextGroup
                    {
                        BinderId = (int)savedView.BinderId
                    } }
                };
                theseDisplayTextHeaders.Add(blankTextHeader);
            }

            //sort the list based on the sort values/descending value
            switch (savedView.SortValue)
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
            if (savedView.Descending == true)
            {
                theseDisplayTextHeaders.Reverse();
            }

            return (theseDisplayTextHeaders);
        }
        public DisplayTextHeadersAndSavedView GetDisplayTextHeadersAndSavedView(int userId, int viewId)
        {
            DisplayTextHeadersAndSavedView displayTextHeadersAndSavedView = new DisplayTextHeadersAndSavedView();

            SavedView savedView = GetSavedView(viewId);
            DisplayBinder binder = GetDisplayBinder(userId);
            List<TextGroup> groups = GetTextGroupsInBinder(userId, binder.BinderId);

            // Add default views to list of groups for display in dropdown.
            try
            {
                groups.Add(new TextGroup(){
                    GroupTitle = "All",
                    SavedViewId = _context.SavedViews.Single(x => x.BinderId == binder.BinderId
                                                               && x.SetValue == "All").SavedViewId,
                    TextGroupId = -1
                });
                groups.Add(new TextGroup()
                {
                    GroupTitle = "Active",
                    SavedViewId = _context.SavedViews.Single(x => x.BinderId == binder.BinderId
                                                               && x.SetValue == "Active").SavedViewId,
                    TextGroupId = -1
                });
                groups.Add(new TextGroup()
                {
                    GroupTitle = "Hidden",
                    SavedViewId = _context.SavedViews.Single(x => x.BinderId == binder.BinderId
                                                               && x.SetValue == "Hidden").SavedViewId,
                    TextGroupId = -1
                });
            }
            catch
            {
                displayTextHeadersAndSavedView.View = new SavedView() { SavedViewId = -1 };
                return displayTextHeadersAndSavedView;
            }

            // let's pause for station identification and an error check
            if (savedView.SavedViewId == -1 || binder.BinderId == -1)
            {
                displayTextHeadersAndSavedView.View = new SavedView() {SavedViewId = -1};
                return displayTextHeadersAndSavedView;
            }

            // no error handling in GetDisplayTextHeaders because it's monster, so putting some here:
            List<DisplayTextHeader> textHeaders = new List<DisplayTextHeader>();
            try
            {
                textHeaders = GetDisplayTextHeaders(savedView);
            }
            catch
            {
                displayTextHeadersAndSavedView.View.SavedViewId = -1;
                return displayTextHeadersAndSavedView;
            }
            List<Binder> userBinders = GetBinders(userId, "active_excluding_current");

            // new binders might not have a text header, so make a blank one as a placeholder:
            if (textHeaders.Count == 0)
            {
                textHeaders.Add(new DisplayTextHeader()
                {
                    BinderId = binder.BinderId,

                });
            }

            // put it all together
            displayTextHeadersAndSavedView.View = savedView;
            displayTextHeadersAndSavedView.TextHeaders = textHeaders;
            displayTextHeadersAndSavedView.Groups = groups;
            displayTextHeadersAndSavedView.Binder = binder;
            displayTextHeadersAndSavedView.UserBinders = userBinders;

            // update last accessed
            UpdateBinderLastAccessed(binder.BinderId, userId);

            return displayTextHeadersAndSavedView;
        }
        public Text GetText(int textId)
        {
            Text text = new Text();
            try
            {
                text = _context.Texts.SingleOrDefault(x => x.TextId == textId);
            }
            catch
            {
                text.TextId = -1;
            }
            return text;
        }
        public TextGroup GetTextGroup(int textGroupId)
        {
            TextGroup textGroup = new TextGroup();
            try
            {
                textGroup = _context.TextGroups.Single(x => x.TextGroupId == textGroupId);
            }
            catch
            {
                textGroup.TextGroupId = -1;
            }
            return textGroup;
        }
        public List<TextGroup> GetTextGroupsInBinder(int userId, int binderId)
        {
            List<TextGroup> groups = new List<TextGroup>();
            try
            {
                groups = _context.TextGroups.Where(x => x.OwnerId == userId
                                                     && x.BinderId == binderId).ToList();
            }
            catch
            {
                groups = new List<TextGroup>();
            }
            return (groups);
        }
        public TextHeaderBodyUserRecord GetTextHeaderBodyUserRecord(int userId, int textHeaderID)
        {
            // Build up a TextHeaderBodyUserRecord to pass into a view:
                // obvs, gonna need that TextHeader:
            TextHeader thisTextHeader = _context.TextHeaders.Find(textHeaderID);

                //grab the related text (if it exists)
            Text thisText = new Text();

            if (thisTextHeader.TextId != null)
            {
                thisText = GetText((int)thisTextHeader.TextId);
            }

                //grab up the revision statuses for display in the dropdown list
            List<TextRevisionStatus> revisionStatuses = _context.TextRevisionStatuses.ToList();

                //grab the current revision status in a readable format for display
            string currentRevisionStatus = revisionStatuses.Single(x => x.TextRevisionStatusId == thisTextHeader.TextRevisionStatusId).TextRevisionStatus1;

                //grab the SimpleUser object for current user
            SimpleUser currentUser = GetCurrentSimpleUser(userId);

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
            }
            catch
            {
                EditWindowProperty newEditWindowProperty = new EditWindowProperty();
                newEditWindowProperty.UserId = currentUser.UserId;
                newEditWindowProperty.TextHeaderId = textHeaderID;
                newEditWindowProperty.CursorPosition = 0;
                newEditWindowProperty.ActiveElement = "body_edit_field";
                newEditWindowProperty.ShowLineCount = 1;
                newEditWindowProperty.ShowParagraphCount = 1;

                _context.EditWindowProperties.Add(newEditWindowProperty);
                _context.SaveChanges();
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
                foreach (var textHeader in previousTextHeaders)
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
        public List<TextHeader> GetTextHeadersInGroup(TextGroup group)
        {
            List<LnkTextHeadersTextGroup> groupLinks = _context.LnkTextHeadersTextGroups.Where(x => x.TextGroupId == group.TextGroupId).ToList();

            List<TextHeader> textHeaders = _context.TextHeaders.Where(x => x.BinderId == group.BinderId
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
        public SavedView GetSavedView(int viewId)
        {
            SavedView savedView = new SavedView();
            try
            {
                savedView = _context.SavedViews.Single(x => x.SavedViewId == viewId);
            }
            catch
            {
                savedView.SavedViewId = -1;
            }
            return savedView;
        }
        public int GetSavedViewIdBySetValue(int userId, string setValue)
        {
            int binderId = GetCurrentBinderID(userId);
            int savedViewId;
            
            try
            {
                try
                {
                    savedViewId = Int32.Parse(setValue);
                }
                catch
                {
                    savedViewId = _context.SavedViews.Single(x => x.UserId == userId
                                                             && x.BinderId == binderId
                                                             && x.SetValue == setValue).SavedViewId;
                }
  
            }
            catch
            {
                savedViewId = -1;
            }
            return savedViewId;
        }
        public int GetSavedViewIdOnStart(int userId)
        {
            Status status = ResetActiveViewToDefaults(userId);

            return status.recordId;
        }
        public bool SimpleUserExists(int userId)
        {
            bool exists = false;

            SimpleUser thisUser = GetCurrentSimpleUser(userId);
            if (thisUser.UserId != -1)
            {
                exists = true;
            } else
            {
                exists = false;
            }
            return exists;
        }
        public bool TextsAreSame(string originalText, string textToCompare)
        {
            //I don't know if it will be quicker to convert the text to a unique (or semi-unique) numeric value,
            //then compare those values, or to do straight string comparison. I'll have to test with a large text value
            bool same;

            if (originalText == textToCompare)
            {
                same = true;
            }
            else
            {
                same = false;
            }

            return same;
        }


        // CRUD Methods
            //  Setup Methods:
        public Status SetupNewUser(SimpleUser newUser)
        {
            // Each user will have an entry in the SimpleUsers table 
            // linked to their AspNetUser entry that will provide an int
            // to use as a key

            Status status = new Status();

            try
            {
                _context.SimpleUsers.Add(newUser);
                _context.SaveChanges();

                status.success = true;
            } catch
            {
                status.success = false;
                status.message = "Failed to save SimpleUser model.";
                return status;
            }

            // Each user will need a new set of binders made...
            status = CreateNewUserBinderSet(newUser.UserId);
            return status;
        }
        public Status CreateNewUserBinderSet(int newUserId)
        {
            Status status = new Status();
            // Each New User gets a default binder
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

            // ... and a trash binder
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

            // ... and loose pages binder
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

            try
            {
                _context.Binders.Add(defaultBinder);
                _context.Binders.Add(trashBinder);
                _context.Binders.Add(loosePages);
                _context.SaveChanges();
                status.success = true;
            }
            catch
            {
                status.success=false;
                status.message = "Failed to create Default Binder Set";
                return status;
            }

            //Each Binder needs a view set as well: 
            status = CreateNewBinderViewSet(defaultBinder.BinderId, newUserId);
            if (status.success) status = CreateNewBinderViewSet(trashBinder.BinderId, newUserId);
            if (status.success) status = CreateNewBinderViewSet(loosePages.BinderId, newUserId);

            return status;
        }
        public Status CreateNewBinderViewSet(int userId, int binderId)
        {
            Status status = new Status();
            //NOTES: revised intentions on binder / view organization and behavior:
            /*
             * Each binder will have f views created for it:
             *  Active
             *  All
             *  Trash
             *  Default
             *  
             *  Active = not deleted
             *  Trash = "deleted"
             *  All = both Active & Trash
             *  Default = holds values of user preferred column and sort to be applied to new group views
             * 
             * Texts can be moved to a "deleted" state within a binder and they are effectively hidden from view
             * They can also be removed from the binder and sent to the trash binder.
             */

            //create active saved view for user
            SavedView activeView = new SavedView()
            {
                UserId = userId,
                SetValue = "Active",
                SortValue = "title",
                ViewName = "",
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

            SavedView defaultView = new SavedView()
            {
                UserId = userId,
                SetValue = "Default",
                SortValue = "title",
                ViewName = "Default - AutoCreated",
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
                ViewName = "Hidden Texts",
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
                ViewName = "All Texts",
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


            try
            {
                _context.SavedViews.Add(activeView);
                _context.SavedViews.Add(defaultView);
                _context.SavedViews.Add(trashView);
                _context.SavedViews.Add(loosePagesView);

                _context.SaveChanges();
                status.success = true;
            }
            catch
            {
                status.success = false;
                status.message = $"Failed to create Default View Set for binder {binderId}";
            }
            return status;
        }
        public Status CreateNewText(string textBody)
        {
            Status status = new Status();

            Text newText = new Text()
            {
                TextBody = textBody,
                Created = DateTime.Now
            };

            try
            {
                _context.Texts.Add(newText);
                _context.SaveChanges();
                status.success = true;
                status.recordId = newText.TextId;
            }
            catch
            {
                status.success = false;
                status.recordId = -1;
                status.message = "Failed to save Text.";
            }
            return status;
        }
        public Status CreateNewTextRecord(int textHeaderId, int textId, int userId)
        {
            Status status = new Status();
            //Create a new log entry for the TextRecord table
            TextRecord newTextRecord = new TextRecord()
            {
                TextHeaderId = textHeaderId,
                TextId = textId,
                UserId = userId,
                Recorded = DateTime.Now
            };

            try
            {
                _context.TextRecords.Add(newTextRecord);
                _context.SaveChanges();
                status.success = true;
                status.recordId = newTextRecord.TextRecordId;
            }
            catch
            {
                status.success = false;
                status.message = "Failed to save new Text Record";
                status.recordId = -1;
            }
            return status;
        }
        public Status CreateNewTextGroup(int userId, TextGroup newGroup)
        {
            Status status = new Status();
            // Create a new group and a new view for that group

            newGroup.OwnerId = userId;
            newGroup.Hidden = false;
            newGroup.Locked = false;
            try
            {
                _context.TextGroups.Add(newGroup);
                _context.SaveChanges();
                status.recordId = newGroup.TextGroupId;
            }
            catch
            {
                status.success = false;
                status.message = $"Failed to save new text group {newGroup.GroupTitle}";
                status.recordId = -1;
            }

            if (status.recordId != -1)
            {
                SavedView newGroupView = _context.SavedViews.Single(x => x.UserId == userId
                                                                     && x.SetValue == "Default");

                newGroupView.SetValue = newGroup.TextGroupId.ToString();
                newGroupView.ViewName = newGroup.GroupTitle;

                try
                {
                    _context.SavedViews.Add(newGroupView);
                    _context.SaveChanges();
                    status.success = true;
                }
                catch
                {
                    status.success = false;
                    status.message = $"Failed to save view for new text group {newGroup.GroupTitle}";
                    status.recordId = -1;
                }
            }
            return status;
        }
        public Status UpdateTextHeader(TextHeader updatedTextHeader)
        {
            Status status = new Status();

            try
            {
                _context.Entry(updatedTextHeader).State = Microsoft.EntityFrameworkCore.EntityState.Modified;  //remember to copy paste this honkin thing
                _context.Update(updatedTextHeader);
                _context.SaveChanges();
                status.success = true;
            }
            catch
            {
                status.success = false;
                status.message = "Failed to save updated TextHeader.";
            }
            return status;
        }
        public Status UpdateEditWindowProperty(EditWindowProperty updatedEditWindowProperty)
        {
            Status status = new Status();
            try
            {
                _context.Entry(updatedEditWindowProperty).State = Microsoft.EntityFrameworkCore.EntityState.Modified;  //remember to copy paste this honkin thing
                _context.Update(updatedEditWindowProperty);
                _context.SaveChanges();
                status.success = true;
            }
            catch
            {
                status.success = false;
                status.message = "Failed to save updated Edit Window Property";
            }
            return status;
        }
        public Status UpdateBinderLastAccessed(int binderId, int userId)
        {
            Status status = new Status();
            Binder thisBinder = _context.Binders.Single(x => x.BinderId == binderId);
            thisBinder.LastAccessed = DateTime.Now;
            thisBinder.LastAccessedBy = userId;
            try
            {
                _context.Entry(thisBinder).State = Microsoft.EntityFrameworkCore.EntityState.Modified;  //remember to copy paste this honkin thing
                _context.Update(thisBinder);
                _context.SaveChanges();
                status.success = true;
            }
            catch
            {
                status.success = false;
                status.message = "Failure to update Binder";
            }
            return status;

        }


            //  Text Methods:
        public Status StartNewText(int userId)
        {
            Status status = new Status();

            // Create a new blank Text
            Text newText = new Text();
            newText.TextBody = "";
            newText.Created = DateTime.Now;
          
            try
            {
                _context.Texts.Add(newText);
                _context.SaveChanges();
                status = CreateNewTextHeader(userId, newText);
                status = CreateNewTextHeaderEditWindowProperty(userId, status.recordId);
            }
            catch
            {
                status.success = false;
                status.message = "Failed to create new Text object";
                return status;
            }
            return status;
        }
        public Status CreateNewTextHeader(int userId, Text newText)
        {
            Status status = new Status();
            // Create a new TextHeader entry (DEFAULTS of a new TextHeader set here):
            int binderId = GetCurrentBinderID(userId);
            TextHeader newTextHeader = new TextHeader()
            {
                Title = DateTime.Now.ToString(),
                Created = DateTime.Now,
                CreatedBy = userId,
                LastModified = DateTime.Now,
                LastModifiedBy = userId,
                VisionNumber = 1,
                Deleted = false,
                Locked = false,
                Top = true,
                TextRevisionStatusId = 1,
                LastRead = DateTime.Now,
                LastReadBy = userId,
                TextId = newText.TextId,
                BinderId = binderId
            };

            try
            {
                _context.TextHeaders.Add(newTextHeader);
                _context.SaveChanges();
                status.success = true;
                status.recordId = newTextHeader.TextHeaderId;
            }
            catch
            {
                status.success = false;
                status.message = $"Failed creating header for text id {newText.TextId}";
            }
            return status;
        }
        public Status CreateNewTextHeaderEditWindowProperty(int userId, int textHeaderId)
        {
            Status status = new Status();
            // EditWindowProperty entry helps autosave function/set ui elements to previous state on load after save
            try
            {
                EditWindowProperty editWindowProperty = new EditWindowProperty()
                {
                    TextHeaderId = textHeaderId,
                    UserId = userId,
                    ActiveElement = "body_edit_field", //ID of text body edit field
                    CursorPosition = 0,
                    ShowLineCount = 1,
                    ShowParagraphCount = 1
                };
                _context.EditWindowProperties.Add(editWindowProperty);
                _context.SaveChanges();
                status.success = true;
                status.recordId = textHeaderId;
            }
            catch
            {
                status.success = false;
                status.recordId = -1;
                status.message = $"Failed to set default Edit Window Properties for text header {textHeaderId}";
            }
            return status;
        }
        public Status SaveEditedText(TextHeaderBodyUserRecord editedTextHeaderBodyUserRecord)
        {
            Status status = new Status(); 
            //Check for change and only save if the text has changed
            bool unchanged;
            Text origText = new Text();

            origText = GetText((int)editedTextHeaderBodyUserRecord.TextHeader.TextId);
            unchanged = TextsAreSame(origText.TextBody, editedTextHeaderBodyUserRecord.Text.TextBody);

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
                    unchanged = TextsAreSame(origHeader.Title, editedTextHeaderBodyUserRecord.TextHeader.Title);
                }
            }

            // if something has changed, let's save it by creating a new Text and pointing the header towards it
            if (!unchanged)
            {
                //Create a new record in the Text table 
                status = CreateNewText(editedTextHeaderBodyUserRecord.Text.TextBody);
                if (!status.success) { return status; }
                Text newText = GetText(status.recordId);

                status = CreateNewTextRecord(editedTextHeaderBodyUserRecord.TextHeader.TextHeaderId, newText.TextId, editedTextHeaderBodyUserRecord.User.UserId);
                if (!status.success) { return status; }

                //Update the TextHeader with the new TextID, etc
                TextHeader updatedTextHeader = _context.TextHeaders.Find(editedTextHeaderBodyUserRecord.TextHeader.TextHeaderId);

                updatedTextHeader.LastModified = newText.Created;
                updatedTextHeader.LastModifiedBy = editedTextHeaderBodyUserRecord.User.UserId;
                updatedTextHeader.LastRead = newText.Created;
                updatedTextHeader.LastReadBy = editedTextHeaderBodyUserRecord.User.UserId;
                updatedTextHeader.TextId = newText.TextId;
                updatedTextHeader.TextRevisionStatusId = editedTextHeaderBodyUserRecord.TextHeader.TextRevisionStatusId;
                updatedTextHeader.Title = editedTextHeaderBodyUserRecord.TextHeader.Title;

                status = UpdateTextHeader(updatedTextHeader);
                if (!status.success) { return status; }

                //  update that EditWindowStatus to save the view preferences for this text header
                EditWindowProperty thisEditWindowProperty = _context.EditWindowProperties.Where(x => x.UserId == editedTextHeaderBodyUserRecord.User.UserId
                                                                                                && x.TextHeaderId == editedTextHeaderBodyUserRecord.TextHeader.TextHeaderId).First();

                thisEditWindowProperty.CursorPosition = editedTextHeaderBodyUserRecord.EditWindowProperty.CursorPosition;
                thisEditWindowProperty.ActiveElement = editedTextHeaderBodyUserRecord.EditWindowProperty.ActiveElement;
                thisEditWindowProperty.ShowLineCount = editedTextHeaderBodyUserRecord.EditWindowProperty.ShowLineCount;
                thisEditWindowProperty.ShowParagraphCount = editedTextHeaderBodyUserRecord.EditWindowProperty.ShowParagraphCount;

                status = UpdateEditWindowProperty(thisEditWindowProperty);
            }
            else
            {
                // Still have to return a success status even if no new record was saved
                status.success = true;
                status.recordId = editedTextHeaderBodyUserRecord.TextHeader.TextHeaderId;
            }
            return status;
        }
        public Status ToggleHideSelectedHeaders(DisplayTextHeadersAndSavedView savedView, bool hide)
        {
            Status status = new Status();

            foreach (var header in savedView.TextHeaders)
            {
                if (header.Selected)
                {
                    TextHeader thisTextHeader = _context.TextHeaders.Single(x => x.TextHeaderId == header.TextHeaderId);
                    thisTextHeader.Deleted = hide;
                    _context.Entry(thisTextHeader).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.Update(thisTextHeader);
                }
            }
            try
            {
                _context.SaveChanges();
                status.success = true;
                status.recordId = savedView.View.SavedViewId;
            }
            catch
            {
                status.success = false;
                status.recordId = savedView.View.SavedViewId;
                status.message = $"Failed to save Text Hidden state to {hide} for view {savedView.View.SavedViewId}";
            }

            return status;
        }
        public Status AddRemoveHeadersFromGroups(DisplayTextHeadersAndSavedView savedView, int groupID, bool add)
        {   
            Status status = new Status();

            try
            {
                //Get a list of all the headerIDs that we're going to update:
                List<int> headerIDsToUpdate = savedView.TextHeaders.Where(x => x.Selected).Select(x => x.TextHeaderId).ToList();

                // TODO - It's inelegant to grab ALL linked records and search them. This can be refactored 
                //  Added a join on selected headers to limit results returned - previously was all. 
                List<LnkTextHeadersTextGroup> existingLinks = _context.LnkTextHeadersTextGroups.Where(x => headerIDsToUpdate.Any(y => y == x.TextHeaderId)).ToList();
                    
                bool exists = true;

                if (add)
                {
                    foreach (int headerID in headerIDsToUpdate)
                    {
                        // Check for existing links to avoid inserting duplicates
                        exists = existingLinks.Any(x => x.TextHeaderId == headerID &&
                                                        x.TextGroupId == groupID);

                        if (!exists)
                        {
                            LnkTextHeadersTextGroup newLink = new LnkTextHeadersTextGroup();

                            newLink.TextGroupId = groupID;
                            newLink.TextHeaderId = headerID;

                            _context.LnkTextHeadersTextGroups.Add(newLink);
                        }
                    }
                }

                if (!add)
                {
                    foreach (int headerID in headerIDsToUpdate)
                    {
                        exists = existingLinks.Any(x => x.TextHeaderId == headerID &&
                                                        x.TextGroupId == groupID);

                        if (exists)
                        {
                            LnkTextHeadersTextGroup linkToRemove = new LnkTextHeadersTextGroup();

                            linkToRemove = _context.LnkTextHeadersTextGroups.Where(x => x.TextHeaderId == headerID &&
                                                                                        x.TextGroupId == groupID).First();

                            _context.LnkTextHeadersTextGroups.Remove(linkToRemove);
                        }
                    }
                }

                    _context.SaveChanges();
                    status.success = true;
                    status.recordId = savedView.View.SavedViewId;
            }
            catch
            {
                status.success = false;
                status.recordId = savedView.View.SavedViewId;
                string addOrRemove; string toOrFrom;
                if (add) { addOrRemove = " add "; toOrFrom = " to "; } else { addOrRemove = " remove "; toOrFrom = " from "; };

                status.message = $"Failed to {addOrRemove} texts {toOrFrom} group {groupID} in view {savedView.View.SavedViewId}";
            }

            return status;
        }
        public Status TransferHeadersAcrossBinders(DisplayTextHeadersAndSavedView savedView, int newBinderId)
        {
            Status status = new Status();
            
            foreach (var header in savedView.TextHeaders)
            {
                if (header.Selected)
                {
                    TextHeader thisTextHeader = _context.TextHeaders.Single(x => x.TextHeaderId == header.TextHeaderId);
                    thisTextHeader.BinderId = newBinderId;
                    thisTextHeader.Deleted = false; // deleted == hidden, and I don't want transferred headers to be hidden even if they were previously

                    try
                    {
                        _context.Entry(thisTextHeader).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        _context.Update(thisTextHeader);
                    }
                    catch
                    {
                        status.success = false;
                        status.recordId = -1;
                        status.message = $"Failed to transfer text header {thisTextHeader.TextHeaderId} into binder {newBinderId}";
                        return status;
                    }
                }
            }

            _context.SaveChanges();
            status.success = true;
            status.recordId = savedView.View.SavedViewId;
            return status;
        }
        public Status AddRevisionToText(int userId, int textHeaderId)
        {
            Status status = new Status();

            TextHeader currentTextHeader = _context.TextHeaders.Single(x => x.TextHeaderId == textHeaderId);
            TextHeader newTextHeader = new TextHeader()
            {
                Created = currentTextHeader.Created,
                CreatedBy = userId,
                LastModified = DateTime.Now,
                LastModifiedBy = userId,
                LastRead = DateTime.Now,
                LastReadBy = userId,
                Deleted = false,
                Locked = false,
                Top = true,
                TextId = currentTextHeader.TextId,
                Title = currentTextHeader.Title,
                VersionOf = currentTextHeader.TextHeaderId,
                VisionNumber = currentTextHeader.VisionNumber + 1,
                TextRevisionStatusId = currentTextHeader.TextRevisionStatusId,
                BinderId = currentTextHeader.BinderId
            };

            currentTextHeader.Top = false;
            currentTextHeader.Locked = true;

            try
            {
                _context.Entry(currentTextHeader).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(currentTextHeader);
                _context.TextHeaders.Add(newTextHeader);
                _context.SaveChanges();
                status.success = true;
                status.recordId = newTextHeader.TextHeaderId;
            }
            catch
            {
                status.success = false;
                status.recordId = -1;
                status.message = $"Failed to add revision to Text Header{textHeaderId}";
            }

            return status;
        }

            //  View Methods:
        public Status UpdateView(DisplayTextHeadersAndSavedView savedView)
        {   // The intent here is that when the user changes the dispaly
            // in the ui, the values are changed in the Saved View
            // that is returned. This method will apply those changes and save them
            // to the database.
            Status status = new Status();

            SavedView viewToUpdate = GetSavedView(savedView.View.SavedViewId);

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

            try
            {
                _context.Entry(viewToUpdate).State = Microsoft.EntityFrameworkCore.EntityState.Modified;  //remember to copy paste this honkin thing
                _context.Update(viewToUpdate);
                _context.SaveChanges();
                status.success = true;
                status.recordId = viewToUpdate.SavedViewId;
            }
            catch
            {
                status.success = false;
                status.recordId = savedView.View.SavedViewId;
                status.message = "Failed to update SavedView";
            }
            return status;
        }
        public Status SetDefaultView(int userId, SavedView newDefaults)
        {
            Status status = new Status();
            // When user clicks SaveDefault we will apply current view's grid arrangement to the default saved view
            SavedView defaultSavedView = GetDefaultSavedView(userId);

            defaultSavedView.Descending = newDefaults.Descending;
            defaultSavedView.SortValue = newDefaults.SortValue;
            defaultSavedView.Default = newDefaults.Default;
            defaultSavedView.Saved = newDefaults.Saved;
            defaultSavedView.LastView = newDefaults.LastView;
            defaultSavedView.LastModified = newDefaults.LastModified;
            defaultSavedView.LastModifiedBy = newDefaults.LastModifiedBy;
            defaultSavedView.Created = newDefaults.Created;
            defaultSavedView.CreatedBy = newDefaults.CreatedBy;
            defaultSavedView.VisionNumber = newDefaults.VisionNumber;
            defaultSavedView.RevisionStatus = newDefaults.RevisionStatus;
            defaultSavedView.Groups = newDefaults.Groups;

            try
            {
                _context.Entry(defaultSavedView).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(defaultSavedView);
                _context.SaveChanges();
                status.success = true;
                status.recordId = newDefaults.SavedViewId;
            }
            catch
            {
                status.success = false;
                status.recordId = -1;
                status.message = "Failed to save view defaults.";
            }

            return status;
        }
        public Status SwitchToViewBySet(int userId, string setValue)
        {
            Status status = new Status();

            int viewId = GetSavedViewIdBySetValue(userId, setValue);

            if (viewId == -1)
            {
                status.success = false;
                status.recordId = viewId;
                status.message = $"Failed to retrieve view with set value {setValue}";
            }
            else
            {
                status.success = true;
                status.recordId = viewId;
            }
            return status;
        }

        public Status ResetActiveViewToDefaults(int userId)
        {
            Status status = new Status();

            int binderId = GetCurrentBinderID(userId);

            if(binderId == -1)
            {
                status.success = false;
                status.message = $"Failed to retrieve current binder Id";
                return status;
            };

            SavedView activeView = _context.SavedViews.Single(x => x.BinderId == binderId
                                                                && x.SetValue == "Active");

            SavedView defaultView = _context.SavedViews.Single(x => x.BinderId == binderId
                                                                 && x.SetValue == "Default");

            activeView.Descending = defaultView.Descending;
            activeView.SortValue = defaultView.SortValue;
            activeView.Default = defaultView.Default;
            activeView.Saved = defaultView.Saved;
            activeView.LastView = defaultView.LastView;
            activeView.LastModified = defaultView.LastModified;
            activeView.LastModifiedBy = defaultView.LastModifiedBy;
            activeView.Created = defaultView.Created;
            activeView.CreatedBy = defaultView.CreatedBy;
            activeView.VisionNumber = defaultView.VisionNumber;
            activeView.RevisionStatus = defaultView.RevisionStatus;
            activeView.Groups = defaultView.Groups;

            try
            {
                _context.Entry(activeView).State = Microsoft.EntityFrameworkCore.EntityState.Modified;  //remember to copy paste this honkin thing
                _context.Update(activeView);
                _context.SaveChanges();
                status.success = true;
                status.recordId = activeView.SavedViewId;
            }
            catch
            {
                status.success = false;
                status.message = "Failure to reset Active saved view to default values";
                status.recordId = -1;
            }
           
            return status;
        }

            //  Binder Methods:
        public Status CreateNewBinder(int userId, Binder newBinder)
        {
            Status status = new Status();

            newBinder.UserId = userId;
            newBinder.Created = DateTime.Now;
            newBinder.CreatedBy = userId;
            newBinder.Hidden = false;
            newBinder.Selected = false;


            try
            {
                _context.Binders.Add(newBinder);
                _context.SaveChanges();
                status.success = true;
                status.recordId = newBinder.BinderId;
            }
            catch
            {
                status.recordId = -1;
                status.success = false;
                status.message = $"Failed to create binder {newBinder.Name}";
                return status;
            }

            status = CreateNewBinderViewSet(userId, newBinder.BinderId);

            if (!status.success)
            {
                return status;
            }

            status = OpenBinder(userId, newBinder.BinderId);

            
            return status;
        }
        public Status UpdateBinder(int userId ,Binder editedBinder)
        {
            Status status = new Status();
            editedBinder.LastModified = DateTime.Now;
            editedBinder.LastModifiedBy = userId;

            try
            {
                _context.Entry(editedBinder).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(editedBinder);
                _context.SaveChanges();
                status.success = true;
                status.recordId = editedBinder.BinderId;
            }
            catch
            {
                status.success = false;
                status.message = $"Failed to update binder {editedBinder.Name}";
                status.recordId = -1;
            }
            return status;
        }
        public Status OpenBinder(int userId, int binderToOpenId)
        {
            Status status = new Status();
            // Only one Binder is to be "open" at a time,
            // So this method will find the user's Selected Binder
            // set Selected = false, and set the binder to open's Selected to True

            try
            {
                Binder currentlySelectedBinder = _context.Binders.Single(x => x.BinderId == GetCurrentBinderID(userId));

                Binder binderToOpen = _context.Binders.Single(x => x.BinderId == binderToOpenId);

                currentlySelectedBinder.Selected = false;
                binderToOpen.Selected = true;
                binderToOpen.LastAccessed = DateTime.Now;

                _context.Entry(currentlySelectedBinder).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Entry(binderToOpen).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(currentlySelectedBinder);
                _context.Update(binderToOpen);
                _context.SaveChanges();

                status.success = true;
                status.recordId = binderToOpenId;
            }
            catch
            {
                status.success = false;
                status.message = $"Failed to open binder {binderToOpenId}";
            }
            return status;        
        }
        public Status MoveAllBinderContents(int binderSourceId, int binderDestinationId)
        {
            Status status = new Status();
            try
            {
                List<TextHeader> headersToMove = _context.TextHeaders.Where(x => x.BinderId == binderSourceId).ToList();
                List<TextGroup> groupsToMove = _context.TextGroups.Where(x => x.BinderId == binderSourceId).ToList();

                headersToMove.ForEach(x => x.BinderId = binderDestinationId);
                headersToMove.ForEach(x => x.Deleted = false);
                groupsToMove.ForEach(x => x.BinderId = binderDestinationId);

                _context.UpdateRange(headersToMove);
                _context.UpdateRange(groupsToMove);
                _context.SaveChanges();
                status.success = true;
            }
            catch
            {
                status.success = false;
                status.message = $"Failed to clear move contents from {binderSourceId} to {binderDestinationId}";
            }

            return status;
        }
        public Status ClearBinder(int userId, int binderId)
        {
            Status status = new Status();
            // "Clearing" binder will:
            //      -   Remove all text headers
            //      -   Add all text headers to Loose Pages binder
            //      -   Un-hide loose pages binder if it was previously empty and hidden
            //      -   move text groups to loose pages binder as well

            try
            {
                int loosePagesBinderId = _context.Binders.Single(x => x.UserId == userId
                                                                    && x.Name == "Loose Pages").BinderId;

                status = MoveAllBinderContents(binderId, loosePagesBinderId);
                if (!status.success) return status;

                status = SetLoosePagesBinderHideState(userId);
            }
            catch
            {
                status.success = false;
                status.message = $"Failed to clear binder {binderId}";
            }
            return status;
        }
        public Status DeleteBinder(int userId, int binderId)
        {
            Status status = new Status();
            // "Deleting" binder will:
            //      -   do all the tasks ClearBinder does, but also
            //      -   Set Binder.Hidden = true
            status = ClearBinder(userId, binderId);

            if (status.success)
            {
                try
                {
                    Binder binderToDelete = _context.Binders.Single(x => x.BinderId == binderId);
                    binderToDelete.Hidden = true;
                    _context.Entry(binderToDelete).State = Microsoft.EntityFrameworkCore.EntityState.Modified;  //remember to copy paste this honkin thing
                    _context.Update(binderToDelete);
                    _context.SaveChanges();
                    status.success = true;
                }
                catch
                {
                    status.success = false;
                    status.message = $"Failed to delete binder {binderId}";
                    status.recordId = -1;
                }
            }
            return status;
        }
        public Status DeleteBinderAndContents(int userId, int binderId)
        {
            Status status = new Status();
            // "Deleting" binder and all contents will will:
            //      -   Remove all text headers
            //      -   Add all text headers to Trash binder
            //      -   Mark all headers as deleted  << no, this is "hiding" them within the binder, something we don't want to do if it's going to the trash binder
            //      -   Mark deleted binder as hidden

            try
            {
                int trashBinderId = _context.Binders.Single(x => x.UserId == userId
                                                               && x.Name == "Trash").BinderId;

                status = MoveAllBinderContents(binderId, trashBinderId);
                if (!status.success) return status;

                status = DeleteBinder(userId, binderId);
            }
            catch
            {
                status.success = false;
                status.message = $"Failed to delete binder and contents in binder {binderId}";
            }
            return status;
        }
        public Status SetLoosePagesBinderHideState(int userId)
        {
            Status status = new Status();
            // If LoosePages contains 0 items, hide it

            try
            {
            
                Binder loosePagesBinder = _context.Binders.Single(x => x.UserId == userId 
                                                                    && x.Name == "Loose Pages");

                int headerCount = _context.TextHeaders.Count(x => x.BinderId == loosePagesBinder.BinderId);

                if (headerCount == 0 && loosePagesBinder.Hidden == false)
                {
                    loosePagesBinder.Hidden = true;
                    _context.Entry(loosePagesBinder).State = Microsoft.EntityFrameworkCore.EntityState.Modified;  //remember to copy paste this honkin thing
                    _context.Update(loosePagesBinder);
                    _context.SaveChanges();
                }
                if (headerCount > 0 && loosePagesBinder.Hidden == true)
                {
                    loosePagesBinder.Hidden = false;
                    _context.Entry(loosePagesBinder).State = Microsoft.EntityFrameworkCore.EntityState.Modified;  //remember to copy paste this honkin thing
                    _context.Update(loosePagesBinder);
                    _context.SaveChanges();
                }

                status.success = true;
                status.recordId = loosePagesBinder.BinderId;
            }
            catch
            {
                status.success = false;
                status.recordId = -1;
                status.message = "Failure to check or update Loose Pages Binder status";
            }

            return status;
        }

        //  Group Methods:
        public Status UpdateGroup(TextGroup editedGroup)
        {
            Status status = new Status();
            // May get a null value from the front end for locked
            editedGroup.Locked = (editedGroup.Locked == null) ? false : editedGroup.Locked;
            try
            {
                _context.Entry(editedGroup).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(editedGroup);
                _context.SaveChanges();
                status.success = true;
                status.recordId = editedGroup.TextGroupId;
            }
            catch
            {
                status.success = false;
                status.message = $"Failed to update group {editedGroup.GroupTitle}";
                status.recordId = -1;
            }
            return status;
        }
        public Status ClearTextsFromGroup(int groupId)
        {
            Status status = new Status();
            try
            {
                List<LnkTextHeadersTextGroup> groupHeaderLinks = _context.LnkTextHeadersTextGroups.Where(x => x.TextGroupId == groupId).ToList();
                _context.LnkTextHeadersTextGroups.RemoveRange(groupHeaderLinks);
                _context.SaveChanges();
                status.success = true;
            }
            catch
            {
                status.success = false;
                status.message = $"Failed to clear text headers from group {groupId}";
            }
            return status;
        }
        public Status DeleteGroup(TextGroup editedGroup)
        {
            Status status = new Status();

            status = ClearTextsFromGroup(editedGroup.TextGroupId);

            if (status.success)
            {
                try
                {
                    _context.TextGroups.Remove(editedGroup);
                    _context.SaveChanges();
                    status.success = true;
                }
                catch
                {
                    status.success = false;
                    status.message = $"Failed to delete text group {editedGroup.TextGroupId}";
                }
            }
            return status;
        }
        public Status CreateGroup(int userId, TextGroup newGroup)
        {   // Must also create a new view for each group.
            Status status = new Status();

            newGroup.OwnerId = userId;
            newGroup.Hidden = false;
            newGroup.Locked = false;

            try
            {
                _context.TextGroups.Add(newGroup);
                _context.SaveChanges();
                status = CreateViewForGroup(newGroup);
            }
            catch
            {
                status.success = false;
            }
            return status;
        }
        public Status CreateViewForGroup(TextGroup newGroup)
        {
            Status status = new Status();

            try
            {
                SavedView defaultSavedView = GetDefaultSavedView(newGroup.OwnerId);
                SavedView newGroupView = new SavedView()
                {
                    UserId = newGroup.OwnerId,
                    SetValue = newGroup.TextGroupId.ToString(),
                    SortValue = defaultSavedView.SortValue,
                    ViewName = newGroup.GroupTitle,
                    Descending = defaultSavedView.Descending,
                    Default = defaultSavedView.Default,
                    Saved = defaultSavedView.Saved,
                    LastView = defaultSavedView.LastView,
                    Created = defaultSavedView.Created,
                    CreatedBy = defaultSavedView.CreatedBy,
                    LastModified = defaultSavedView.LastModified,
                    LastModifiedBy = defaultSavedView.LastModifiedBy,
                    VisionNumber = defaultSavedView.VisionNumber,
                    RevisionStatus = defaultSavedView.RevisionStatus,
                    Groups = defaultSavedView.Groups,
                    BinderId = defaultSavedView.BinderId
                };

                _context.SavedViews.Add(newGroupView);
                _context.SaveChanges();

                newGroup.SavedViewId = newGroupView.SavedViewId;

                _context.Entry(newGroup).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(newGroup);
                _context.SaveChanges();

                status.success = true;
            }
            catch
            {
                status.success = false;
                status.recordId = newGroup.TextGroupId;
                status.message = $"Failed to create a view for new group {newGroup.GroupTitle}";
            }
            return status;
        }
        public List<TextHeader> GetPreviousVisions(int textHeaderID, List<TextHeader> prevTextHeaders)
        {
            //Recursively build out list of prevision visions for a given header
            try
            {
                int prevTextHeaderID = (int)_context.TextHeaders.Where(x => x.TextHeaderId == textHeaderID).First().VersionOf;
                prevTextHeaders.Add(_context.TextHeaders.Where(x => x.TextHeaderId == prevTextHeaderID).First());
                GetPreviousVisions(prevTextHeaderID, prevTextHeaders);
            }
            catch
            {
                return (prevTextHeaders);
            }
            return (prevTextHeaders);
        }

    }
}
