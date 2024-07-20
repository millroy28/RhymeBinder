using System;
using System.Collections.Generic;
using System.Linq;

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
        //  GET methods         -- on failures, models will return with -1 in the ID field
        #region GetModelMethods
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
        { 
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
        public string GetBinderName (int binderId)
        {
            string name;
            try
            {
                name = _context.Binders.Single(x=>x.BinderId == binderId).Name;
            }
            catch
            {
                name = "FailedToFetchBinderName";
            }
            return name;
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
        public DisplaySimpleUser GetCurrentDisplaySimpleUser(int userId)
        {
            SimpleUser thisUser = GetCurrentSimpleUser(userId);
            List<TimeZone> timeZones = _context.TimeZones.OrderBy(x => x.TimeZoneId).ToList();
            DisplaySimpleUser thisDisplaySimpleUser = new DisplaySimpleUser()
            {
                UserId = thisUser.UserId,
                AspNetUserId = thisUser.AspNetUserId,
                UserName = thisUser.UserName,
                DefaultRecordsPerPage = thisUser.DefaultRecordsPerPage,
                DefaultShowLineCount = thisUser.DefaultShowLineCount,
                DefaultShowParagraphCount = thisUser.DefaultShowParagraphCount,
                TimeZone = thisUser.TimeZone,
                TimeZones = timeZones
            };

            return thisDisplaySimpleUser;
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
        public SavedView GetDefaultSavedView(int userId, int binderId)
        {
            SavedView defaultSavedView = new SavedView();
            try
            {
                defaultSavedView = _context.SavedViews.Single(x => x.UserId == userId
                                                                && x.BinderId == binderId
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
                displayBinder.Name = binder.Name.Length > 25 ? binder.Name.Substring(0,25) + "..." : binder.Name;
                displayBinder.Description = binder.Description;
                displayBinder.Selected = binder.Selected;
                displayBinder.GroupCount = groupCount;
                displayBinder.PageCount = textCount;
                displayBinder.TextHeaderTitleDefaultFormat = binder.TextHeaderTitleDefaultFormat;
                displayBinder.NewTextDefaultShowParagraphCount = binder.NewTextDefaultShowParagraphCount;
                displayBinder.NewTextDefaultShowLineCount = binder.NewTextDefaultShowLineCount;
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

                int textCount; int groupCount;

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
                        Selected = binder.Selected,
                        CreatedByName = GetUserName(binder.CreatedBy),
                        ModifyByName = GetUserName(binder.LastModifiedBy)
                    });

                }
            }
            catch
            {
                displayBinders.Add(new DisplayBinder() { BinderId = -1 });
            }
            return (displayBinders);
        }
        public List<DisplayTextGroup> GetDisplayTextGroups(int userId, int binderId)
        { 
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
                        BinderName = GetBinderName(group.BinderId),
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
        public List<DisplayTextHeader> GetDisplayTextHeaders(SavedView savedView, int page)
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
                                                                       x.Top == true &&
                                                                       x.Deleted == true &&
                                                                       x.BinderId == savedView.BinderId).ToList();
                    break;

                case "All":
                    theseTextHeaders = _context.TextHeaders.Where(x => x.CreatedBy == savedView.UserId &&
                                                                       x.Top == true &&
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
                    }
                    else
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
                    TextNoteId = textHeader.TextNoteId,
                    LastModifiedBy = textHeader.LastModifiedBy,
                    LastReadBy = textHeader.LastReadBy,
                    CreatedBy = textHeader.CreatedBy,
                    Title = textHeader.Title,
                    VisionNumber = textHeader.VisionNumber,
                    Created = textHeader.Created,
                    LastModified = textHeader.LastModified,
                    LastRead = textHeader.LastRead,
                    Groups = selectedGroups,
                    CreatedByName = GetUserName(textHeader.CreatedBy),
                    ModifyByName = GetUserName(textHeader.LastModifiedBy),
                    ReadByName = GetUserName(textHeader.LastReadBy),
                    RevisionStatus = _context.TextRevisionStatuses.Single(x => x.TextRevisionStatusId == textHeader.TextRevisionStatusId).TextRevisionStatus1
                }
                    ); ;
            }

            //reduce results by search value
            if (!string.IsNullOrWhiteSpace(savedView.SearchValue))
            {
                try
                {
                    // search text bodies
                    List<Text> searchFilteredTexts = _context.Texts.Where(x => theseTextHeaders.Select(y => y.TextId).Contains(x.TextId)
                                                                            && x.TextBody.ToLower().Contains(savedView.SearchValue.ToLower())
                                                                            ).ToList();

                    // seach notes also and as well...
                    List<TextNote> searchFilteredNotes = _context.TextNotes.Where(x => theseTextHeaders.Select(y => y.TextNoteId).Contains(x.TextNoteId)
                                                                              && x.Note.ToLower().Contains(savedView.SearchValue.ToLower())
                                                                            ).ToList();

                    // search text titles too...
                    List<DisplayTextHeader> searchFilteredDisplayTextHeaders = theseDisplayTextHeaders.Where(x => x.Title.ToLower().Contains(savedView.SearchValue.ToLower())
                                                                                                               || searchFilteredTexts.Select(y => y.TextId).Contains(x.TextId)
                                                                                                               || searchFilteredNotes.Select(z => z.TextNoteId).Contains(x.TextNoteId)
                                                                                                               ).ToList();


                    theseDisplayTextHeaders.RemoveAll(x => !searchFilteredDisplayTextHeaders.Exists(y => y.TextHeaderId == x.TextHeaderId));
                }
                catch
                {

                }
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
        public DisplayTextHeadersAndSavedView GetDisplayTextHeadersAndSavedView(int userId, int viewId, int page)
        {
            DisplayTextHeadersAndSavedView displayTextHeadersAndSavedView = new DisplayTextHeadersAndSavedView()
            {
                Page = page
            };

            SavedView savedView = GetSavedView(viewId);
            DisplayBinder binder = GetDisplayBinder(userId, (int)savedView.BinderId);
            List<DisplayTextGroup> groups = GetDisplayTextGroupsInBinder(userId, binder.BinderId);

            // Add default views to list of groups for display in dropdown.
            try
            {
                groups.Add(new DisplayTextGroup()
                {
                    GroupTitle = "All",
                    SavedViewId = _context.SavedViews.Single(x => x.BinderId == binder.BinderId
                                                               && x.SetValue == "All").SavedViewId,
                    TextGroupId = -1
                });
                groups.Add(new DisplayTextGroup()
                {
                    GroupTitle = "Default",
                    SavedViewId = _context.SavedViews.Single(x => x.BinderId == binder.BinderId
                                                               && x.SetValue == "Active").SavedViewId,
                    TextGroupId = -1
                });
                groups.Add(new DisplayTextGroup()
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
                displayTextHeadersAndSavedView.View = new SavedView() { SavedViewId = -1 };
                return displayTextHeadersAndSavedView;
            }

            // no error handling in GetDisplayTextHeaders because it's monster, so putting some here:
            List<DisplayTextHeader> textHeaders = new List<DisplayTextHeader>();
            try
            {
                textHeaders = GetDisplayTextHeaders(savedView, page);
            }
            catch
            {
                displayTextHeadersAndSavedView.View.SavedViewId = -1;
                return displayTextHeadersAndSavedView;
            }

            // do some page calculation
            int headerCount = textHeaders.Count;
            int upperIndex = savedView.RecordsPerPage * page;
            int lowerIndex = upperIndex - savedView.RecordsPerPage;
            if (upperIndex > headerCount) { upperIndex = headerCount; };
            int count = upperIndex - lowerIndex;
            int pageCount = (headerCount - 1) / savedView.RecordsPerPage + 1;

            // remove any headers not to be shown based on page and number of records per page
            List<DisplayTextHeader> displayTextHeadersOnPage = textHeaders.GetRange(lowerIndex, count);

            // Pop list of binders for transfer dropdown
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
            displayTextHeadersAndSavedView.MenuTitle = (binder.Name + ": " + savedView.ViewName).Length > 25
                                                       ? (binder.Name + ": " + savedView.ViewName).Substring(0, 25) + "..."
                                                       : binder.Name + ": " + savedView.ViewName;
            displayTextHeadersAndSavedView.View = savedView;
            displayTextHeadersAndSavedView.TextHeaders = displayTextHeadersOnPage;
            displayTextHeadersAndSavedView.Groups = groups;
            displayTextHeadersAndSavedView.Binder = binder;
            displayTextHeadersAndSavedView.UserBinders = userBinders;
            displayTextHeadersAndSavedView.Page = page;
            displayTextHeadersAndSavedView.TotalPages = pageCount;
            displayTextHeadersAndSavedView.LowIndex = lowerIndex + 1;
            displayTextHeadersAndSavedView.HighIndex = upperIndex;
            displayTextHeadersAndSavedView.TotalHeaders = headerCount;

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
        public List<DisplayTextGroup> GetDisplayTextGroupsInBinder(int userId, int binderId)
        {
            // Currently building out to accept selections from the form - not populating all fields
            List<TextGroup> groups = new List<TextGroup>();
            List<DisplayTextGroup> displayGroups = new List<DisplayTextGroup>();
            try
            {
                groups = _context.TextGroups.Where(x => x.OwnerId == userId
                                                     && x.BinderId == binderId).ToList();
            }
            catch
            {
                
            }

            foreach(var group in groups)
            {
                List<int> textHeaderIds = _context.LnkTextHeadersTextGroups.Where(x => x.TextGroupId == group.TextGroupId).Select(x => x.TextHeaderId).ToList(); 
                displayGroups.Add(new DisplayTextGroup()
                {
                    TextGroupId = group.TextGroupId,
                    GroupTitle = group.GroupTitle,
                    Locked = group.Locked,
                    Hidden = group.Hidden,
                    HeaderCount = 0,
                    BinderName = null,
                    Selected = false,
                    LinkedTextHeaderIds = textHeaderIds
                });
            }
            return (displayGroups);
        }
        public TextEdit GetTextHeaderBodyUserRecord(int userId, int textHeaderID)
        {
            // Build up a TextHeaderBodyUserRecord to pass into a view:
            // obvs, gonna need that TextHeader:
            TextHeader thisTextHeader = _context.TextHeaders.Find(textHeaderID);

            //grab the related text (if it exists)
            Text thisText = GetText((int)thisTextHeader.TextId);

            //grab the note for the text
            TextNote thisTextNote = _context.TextNotes.Single(x => x.TextNoteId == thisTextHeader.TextNoteId);

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

                foreach (var textHeader in previousTextHeaders)
                {
                    SimpleTextHeaderAndText simpleTextHeaderAndText = new SimpleTextHeaderAndText()
                    {
                        Created = textHeader.VisionCreated != null ? textHeader.VisionCreated : textHeader.Created,
                        CreatedBy = textHeader.VisionCreatedBy != null ? GetUserName(textHeader.VisionCreatedBy) : GetUserName(textHeader.CreatedBy),
                        LastModified = textHeader.LastModified,
                        LastModifiedBy = GetUserName(textHeader.LastModifiedBy),
                        Status = _context.TextRevisionStatuses.Where(x => x.TextRevisionStatusId == textHeader.TextRevisionStatusId).First().TextRevisionStatus1,
                        Title = textHeader.Title,
                        VisionNumber = textHeader.VisionNumber,
                        TextBody = _context.Texts.Where(x => x.TextId == textHeader.TextId).First().TextBody
                    };
                    previousTextsAndHeaders.Add(simpleTextHeaderAndText);
                }

                //previousTextsAndHeaders = (from TextHeader textHeader in previousTextHeaders
                //                           join Text text in previousTexts
                //                             on textHeader.TextId equals text.TextId
                //                           join TextRevisionStatus revisionStatus in revisionStatuses
                //                             on textHeader.TextRevisionStatusId equals revisionStatus.TextRevisionStatusId
                //                           select new SimpleTextHeaderAndText
                //                           {
                //                               Title = textHeader.Title,
                //                               TextBody = text.TextBody,
                //                               VisionNumber = textHeader.VisionNumber,
                //                               Created = textHeader.Created,
                //                               LastModified = textHeader.LastModified,
                //                               CreatedBy = GetUserName(textHeader.CreatedBy),
                //                               LastModifiedBy = GetUserName(textHeader.LastModifiedBy),
                //                               Status = revisionStatus.TextRevisionStatus1
                //                           }
                                          //).ToList();

                previousTextsAndHeaders = previousTextsAndHeaders.OrderByDescending(x => x.VisionNumber).ToList();
            }
            catch
            {
            }

            //get text groups
            List<TextGroup> memberOfGroups = (  from TextGroup textGroup in _context.TextGroups
                                                join LnkTextHeadersTextGroup link in _context.LnkTextHeadersTextGroups
                                                on textGroup.TextGroupId equals link.TextGroupId
                                                where link.TextHeaderId == thisTextHeader.TextHeaderId
                                                select new TextGroup
                                                {
                                                    GroupTitle = textGroup.GroupTitle,
                                                    TextGroupId = textGroup.TextGroupId,
                                                    Locked = textGroup.Locked
                                                }
                                                ).OrderBy(x => x.GroupTitle).ToList();
            

            List<TextGroup> availableGroups = ( from TextGroup textGroup in _context.TextGroups
                                                where (textGroup.BinderId == thisTextHeader.BinderId
                                                && !memberOfGroups.Contains(textGroup) //exclude group of which it's already a member
                                                && !textGroup.Locked)
                                                select new TextGroup
                                                {
                                                    GroupTitle = textGroup.GroupTitle,
                                                    TextGroupId = textGroup.TextGroupId
                                                }
                                                ).OrderBy(x => x.GroupTitle).ToList();

            //wrap it up and send it
            TextEdit textEdit = new TextEdit()
            {
                UserId = userId,

                TextId = thisText.TextId,
                TextBody = thisText.TextBody,

                TextNoteId = thisTextNote.TextNoteId,
                Note = thisTextNote.Note,

                TextHeaderId = thisTextHeader.TextHeaderId,
                Title = thisTextHeader.Title,
                Created = thisTextHeader.Created,
                CreatedBy = thisTextHeader.CreatedBy,
                LastModified = thisTextHeader.LastModified,
                LastModifiedBy = thisTextHeader.LastModifiedBy,
                LastRead = thisTextHeader.LastRead,
                LastReadBy = thisTextHeader.LastReadBy,
                TextRevisionStatusId = thisTextHeader.TextRevisionStatusId,
                VisionNumber = thisTextHeader.VisionNumber,
                VisionCreated = thisTextHeader.VisionCreated,
                VisionCreatedBy = thisTextHeader.VisionCreatedBy,
                Deleted = thisTextHeader.Deleted,
                Locked = thisTextHeader.Locked,
                Top = thisTextHeader.Top,
                BinderId = thisTextHeader.BinderId,

                DisplayTitle = thisTextHeader.Title.Length > 20 ? thisTextHeader.Title.Substring(0,20) + "..." : thisTextHeader.Title,
                CreatedByUserName = createdUser.UserName,
                LastModifiedByUserName = lastModifiedUser.UserName,
                CurrentRevisionStatus = currentRevisionStatus,
                MemberOfGroups = memberOfGroups,
                AvailableGroups = availableGroups,

                EditWindowPropertyId = thisEditWindowProperty.EditWindowPropertyId,
                ActiveElement = thisEditWindowProperty.ActiveElement,
                BodyCursorPosition = thisEditWindowProperty.BodyCursorPosition,
                BodyScrollPosition = thisEditWindowProperty.BodyScrollPosition,
                NoteCursorPosition = thisEditWindowProperty.NoteCursorPosition,
                NoteScrollPosition = thisEditWindowProperty.NoteScrollPosition,
                TitleCursorPosition = thisEditWindowProperty.TitleCursorPosition,
                ShowLineCount = thisEditWindowProperty.ShowLineCount,
                ShowParagraphCount = thisEditWindowProperty.ShowParagraphCount,

                AllRevisionStatuses = revisionStatuses,
                PreviousTexts = previousTextsAndHeaders,
              
            };

            return (textEdit);
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
        public int GetSavedViewIdOnStart(int userId, int binderId)
        {
            Status status = ClearSavedViewSearchValues(userId);
            if (binderId == 0)
            {
                binderId = GetCurrentBinderID(userId);
            }

            if (status.recordId == -1)
            {
                return status.recordId;
            }

            status = ResetActiveViewToDefaults(binderId);

            return status.recordId;
        }
        public string GetUserName(int? userId)
        {
            string name = "";
            if (userId != null)
            {
                try 
                {
                    name = _context.SimpleUsers.Where(x => x.UserId == userId).FirstOrDefault().UserName;
                }
                catch
                {
                    name = "";
                }
            }
            return name;
        }
        #endregion

        // UTILITY methods
        #region UtilityMethods
        public bool SimpleUserExists(int userId)
        {
            bool exists = false;

            SimpleUser thisUser = GetCurrentSimpleUser(userId);
            if (thisUser.UserId != -1)
            {
                exists = true;
            }
            else
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
        public DateTime GetUserLocalTime(int userId)
        {
            // Gets current local time for user
            int userTimeZoneId = _context.SimpleUsers.Where(x => x.UserId == userId).Select(x => x.TimeZone).First();
            int userTimeZoneOffset = _context.TimeZones.Where(x => x.TimeZoneId == userTimeZoneId).Select(x => x.UTCOffset).First();

            TimeZoneInfo timeZoneInfo = TimeZoneInfo.Local;
            double offsetDifference = userTimeZoneOffset - timeZoneInfo.BaseUtcOffset.TotalHours;
            DateTime userLocalNow = DateTime.Now.AddHours(offsetDifference);

            if (timeZoneInfo.IsDaylightSavingTime(userLocalNow))
            {
                userLocalNow = userLocalNow.AddHours(1);
            }

            return userLocalNow;                        
        }

        #endregion

        //  USER Methods: 
        #region UserMethods
        public Status SetupNewUser(SimpleUser newUser)
        {
            // Each user will have an entry in the SimpleUsers table 
            // linked to their AspNetUser entry that will provide an int
            // to use as a key
            newUser.DefaultRecordsPerPage = 25;
            newUser.DefaultShowLineCount = true;
            newUser.DefaultShowParagraphCount = true;

            Status status = new Status();

            try
            {
                _context.SimpleUsers.Add(newUser);
                _context.SaveChanges();

                status.success = true;
            }
            catch
            {
                status.success = false;
                status.message = "Failed to save SimpleUser model.";
                return status;
            }

            // Each user will need a new set of binders made...
            status = CreateNewUserBinderSet(newUser.UserId);
            return status;
        }
        public Status UpdateSimpleUser(SimpleUser user)
        {
            Status status = new Status();
            try
            {
                _context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(user);
                _context.SaveChanges();
                status.success = true;
                status.recordId = user.UserId;
            }
            catch
            {
                status.success = false;
                status.recordId = -1;
                status.message = $"Failed to update user {user.UserName}";
            }
            return status;
        } 
        #endregion

        //  Text Methods:
        #region TextMethods
        public Status StartNewText(int userId)
        {
            Status status = new Status();

            // Create a new blank Text
            Text newText = new Text();
            newText.TextBody = "";
            newText.Created = GetUserLocalTime(userId);
            
            try
            {
                _context.Texts.Add(newText);
                _context.SaveChanges();
                status = CreateNewTextHeader(userId, newText.TextId);
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
        public Status CreateNewText(int userId, string textBody)
        {
            Status status = new Status();

            Text newText = new Text()
            {
                TextBody = textBody,
                Created = GetUserLocalTime(userId)
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
        public Status CreateNewTextHeader(int userId, int newTextId)
        {
            Status status = new Status();
            // Create a new TextHeader entry (DEFAULTS of a new TextHeader set here):
            int binderId = GetCurrentBinderID(userId);
            DateTime userLocalNow = GetUserLocalTime(userId);
            TextNote textNote = new TextNote { Note = "" };

            TextHeader newTextHeader = new TextHeader()
            {
                Title = GetNewTextTitle(userId, binderId),
                Created = userLocalNow,
                CreatedBy = userId,
                LastModified = userLocalNow,
                LastModifiedBy = userId,
                VisionNumber = 1,
                Deleted = false,
                Locked = false,
                Top = true,
                TextRevisionStatusId = 1,
                LastRead = userLocalNow,
                LastReadBy = userId,
                TextId = newTextId,
                BinderId = binderId,
                TextNote = textNote
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
                status.message = $"Failed creating header for text id {newTextId}";
            }
            return status;
        }
        public Status CreateRevisionTextHeader(int userId, TextHeader parentHeader)
        {
            Status status = new Status();
            // Create a new TextHeader child (new "vision") of parentHeader:
            DateTime userLocalNow = GetUserLocalTime(userId);


            TextHeader newTextHeader = new TextHeader()
            {
                Title = parentHeader.Title,
                Created = parentHeader.Created,
                CreatedBy = parentHeader.CreatedBy,
                LastModified = userLocalNow,
                LastModifiedBy = userId,
                VisionNumber = parentHeader.VisionNumber + 1,
                VersionOf = parentHeader.TextHeaderId,
                VisionCreated = userLocalNow,
                VisionCreatedBy = userId,
                Deleted = false,
                Locked = false,
                Top = true,
                TextRevisionStatusId = parentHeader.TextRevisionStatusId,
                LastRead = userLocalNow,
                LastReadBy = userId,
                TextId = parentHeader.TextId,
                BinderId = parentHeader.BinderId,
                TextNoteId = parentHeader.TextNoteId
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
                status.message = $"Failed creating new child header for parent header id {parentHeader.TextHeaderId}";
            }

            // Migrate, if any, group associations from previous header to new header
            status = UpdateGroupsForNewVision(status.recordId);
            if (status.success) { status.recordId = newTextHeader.TextHeaderId; } // ensure TextHeaderId is returned as status

            return status;
        }
        public string GetNewTextTitle(int userId, int binderId)
        {
            /*
             *          replacing :
             * [DATE TIME] - current date and time
               [TIME] - current time
               [DATE] - current date
               [NUMBER] - latest number (based on active texts in binder - removing texts could cause duplicates)
            */
            string title = _context.Binders.Where(x => x.BinderId == binderId).First().TextHeaderTitleDefaultFormat;
            int textCount = 0;
            DateTime userLocalNow = GetUserLocalTime(userId);

            if (title.Contains("[NUMBER]"))
            {
                textCount = _context.TextHeaders.Where(x => x.BinderId == binderId
                                                         && x.Deleted == false
                                                         && x.Top == true).Count() + 1;
            }

            title = title.Replace("[DATE TIME]", userLocalNow.ToString());
            title = title.Replace("[TIME]", userLocalNow.ToShortTimeString());
            title = title.Replace("[DATE]", userLocalNow.ToShortDateString());
            title = title.Replace("[NUMBER]", textCount.ToString());

            return title;
        }
        public Status CreateNewTextRecord(int textHeaderId, int textId, int userId)
        {
            Status status = new Status();
            DateTime userLocalNow = GetUserLocalTime(userId);
            //Create a new log entry for the TextRecord table
            TextRecord newTextRecord = new TextRecord()
            {
                TextHeaderId = textHeaderId,
                TextId = textId,
                UserId = userId,
                Recorded = userLocalNow
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
        public Status CreateNewTextHeaderEditWindowProperty(int userId, int textHeaderId)
        {
            Status status = new Status();

            // EditWindowProperty entry helps autosave function/set ui elements to previous state on load after save
            try
            {
                Binder binder = _context.TextHeaders.Where(x => x.TextHeaderId == textHeaderId).Select(x => x.Binder).First();

                EditWindowProperty editWindowProperty = new EditWindowProperty()
                {
                    TextHeaderId = textHeaderId,
                    UserId = userId,
                    ActiveElement = "body_edit_field", //ID of text body edit field
                    ShowLineCount = binder.NewTextDefaultShowLineCount ? 1 : 0,
                    ShowParagraphCount = binder.NewTextDefaultShowParagraphCount ? 1 : 0,
                };
                // is this a new "vision"?
                TextHeader textHeader = _context.TextHeaders.Where(x => x.TextHeaderId == textHeaderId).First();
                if(textHeader.VersionOf != null)
                {
                    EditWindowProperty previousEditWindowProperty = _context.EditWindowProperties.Where(x => x.TextHeaderId == textHeader.VersionOf).First();
                    editWindowProperty.ShowLineCount = previousEditWindowProperty.ShowLineCount;
                    editWindowProperty.ShowParagraphCount = previousEditWindowProperty.ShowParagraphCount;
                }

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
        public Status Update<T>(T updatedEntity)
        {
            Status status = new Status();

            try
            {
                _context.Entry(updatedEntity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;  //remember to copy paste this honkin thing
                _context.Update(updatedEntity);
                _context.SaveChanges();
                status.success = true;
            }
            catch
            {
                status.success = false;
                string type = typeof(T).Name;
                status.message = $"Failed to save {type}.";
            }
            return status;
        }

        
        public Status SaveEditedText(TextEdit textEdit)
        {
            Status status = new Status();
            //prevent blank from being saved as title (which would break navitgation)
            if (textEdit.Title == null || textEdit.Title.Trim() == "") textEdit.Title = "(blank)";

            //Check for change and only save if something has changed
            bool noteUnchanged;
            bool textUnchanged;


            Text origText = GetText((int)textEdit.TextId);
            TextHeader origHeader = _context.TextHeaders.Find(textEdit.TextHeaderId);
            TextNote origNote = _context.TextNotes.Find(textEdit.TextNoteId);

            noteUnchanged = TextsAreSame(origNote.Note, textEdit.Note);
            textUnchanged = TextsAreSame(origText.TextBody, textEdit.TextBody);


            if (!textUnchanged)
            {
                status = CreateNewText(textEdit.UserId, textEdit.TextBody);  //New text created on each edit
                if (!status.success) { return status; }
                origHeader.TextId = status.recordId; //point header to new text id

                status = CreateNewTextRecord(textEdit.TextHeaderId, origHeader.TextId, textEdit.UserId);
                if (!status.success) { return status; }

                
            };

            if (!noteUnchanged)
            {
                origNote.Note = textEdit.Note;
                status = Update<TextNote>(origNote);
                if (!status.success) { return status; }
            }


            origHeader.Title = textEdit.Title;
            origHeader.LastModified = GetUserLocalTime(textEdit.UserId);
            origHeader.LastModifiedBy = textEdit.UserId;
            origHeader.TextRevisionStatusId = textEdit.TextRevisionStatusId;

            status = Update<TextHeader>(origHeader);
            if (!status.success) { return status; }
            

            //  update that EditWindowStatus to save the view preferences for this text header
            EditWindowProperty thisEditWindowProperty = _context.EditWindowProperties.Where(x => x.UserId == textEdit.UserId
                                                                                            && x.TextHeaderId == textEdit.TextHeaderId).First();

            thisEditWindowProperty.BodyCursorPosition = textEdit.BodyCursorPosition;
            thisEditWindowProperty.BodyScrollPosition = textEdit.BodyScrollPosition;
            thisEditWindowProperty.NoteCursorPosition = textEdit.NoteCursorPosition;
            thisEditWindowProperty.NoteScrollPosition = textEdit.NoteScrollPosition;
            thisEditWindowProperty.TitleCursorPosition = textEdit.TitleCursorPosition;
            thisEditWindowProperty.ActiveElement = textEdit.ActiveElement;
            thisEditWindowProperty.ShowLineCount = textEdit.ShowLineCount;
            thisEditWindowProperty.ShowParagraphCount = textEdit.ShowParagraphCount;

            status = Update<EditWindowProperty>(thisEditWindowProperty);
            if (!status.success) { return status; }           
            else
            {
                // Still have to return a success status even if no new record was saved
                status.success = true;
                status.recordId = textEdit.TextHeaderId;
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

            status = CreateRevisionTextHeader(userId, currentTextHeader);
            if (!status.success)
            {
                return status;
            }

            status = CreateNewTextHeaderEditWindowProperty(userId, status.recordId);
            if (!status.success)
            {
                return status;
            }

            currentTextHeader.Top = false;
            currentTextHeader.Locked = true;

            try
            {
                _context.Entry(currentTextHeader).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(currentTextHeader);
                _context.SaveChanges();
            }
            catch
            {
                status.success = false;
                status.recordId = -1;
                status.message = $"Failed to add revision to Text Header{textHeaderId}";
            }

            return status;
        } 
        #endregion

        //  View Methods:
        #region ViewMethods
        public Status UpdateView(DisplayTextHeadersAndSavedView savedView)
        {   // The intent here is that when the user changes the dispaly
            // in the ui, the values are changed in the Saved View
            // that is returned. This method will apply those changes and save them
            // to the database.
            Status status = new Status();

            SavedView viewToUpdate = GetSavedView(savedView.View.SavedViewId);

            viewToUpdate.RecordsPerPage = savedView.View.RecordsPerPage;
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
            viewToUpdate.RecordsPerPage = savedView.View.RecordsPerPage;
            viewToUpdate.SearchValue = savedView.View.SearchValue;

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

            defaultSavedView.RecordsPerPage = newDefaults.RecordsPerPage;
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
        public Status ResetActiveViewToDefaults(int binderId)
        {
            Status status = new Status();


            SavedView activeView = _context.SavedViews.Single(x => x.BinderId == binderId
                                                                && x.SetValue == "Active");

            SavedView defaultView = _context.SavedViews.Single(x => x.BinderId == binderId
                                                                 && x.SetValue == "Default");

            activeView.RecordsPerPage = defaultView.RecordsPerPage;
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
        public Status ClearSavedViewSearchValues(int userId)
        {
            Status status = new Status();
            try
            {
                List<SavedView> savedViews = _context.SavedViews.Where(x => x.UserId == userId).ToList();
                savedViews.ForEach(x => x.SearchValue = "");
                _context.UpdateRange(savedViews);
                _context.SaveChanges();

                status.success = true;
            }
            catch
            {
                status.success = false;
                status.recordId = -1;
                status.message = "Failed to clear search values from user's saved views";
            }
            return status;
        } 
        #endregion

        //  Binder Methods:
        #region BinderMethods
        public Status CreateNewUserBinderSet(int newUserId)
        {
            Status status = new Status();
            DateTime userLocalNow = GetUserLocalTime(newUserId);

            // Each New User gets a default binder
            Binder defaultBinder = new Binder()
            {
                UserId = newUserId,
                Name = "Your Binder",
                Description = "Welcome to your first binder!",
                Created = userLocalNow,
                LastModified = userLocalNow,
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
                Created = userLocalNow,
                LastModified = userLocalNow,
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
                Created = userLocalNow,
                LastModified = userLocalNow,
                CreatedBy = newUserId,
                LastModifiedBy = newUserId,
                Hidden = false,
                Selected = false
            };

            try
            {
                _context.Binders.Add(defaultBinder);
                _context.SaveChanges();
                _context.Binders.Add(trashBinder);
                _context.SaveChanges();
                _context.Binders.Add(loosePages);
                _context.SaveChanges();
                status.success = true;
            }
            catch
            {
                status.success = false;
                status.message = "Failed to create Default Binder Set";
                return status;
            }

            //Each Binder needs a view set as well: 
            status = CreateNewBinderViewSet(defaultBinder.BinderId, newUserId);
            if (status.success) status = CreateNewBinderViewSet(trashBinder.BinderId, newUserId);
            if (status.success) status = CreateNewBinderViewSet(loosePages.BinderId, newUserId);

            return status;
        }
        public Status CreateNewBinder(int userId)
        {
            Status status = new Status();
            SimpleUser simpleUser = GetCurrentSimpleUser(userId);
            DateTime userLocalNow = GetUserLocalTime(userId);


            Binder newBinder = new Binder()
            {
                UserId = userId,
                CreatedBy = userId,
                Created = userLocalNow,
                Description = "",
                Hidden = false,
                Name = "New Binder",
                NewTextDefaultShowLineCount = simpleUser.DefaultShowLineCount,
                NewTextDefaultShowParagraphCount = simpleUser.DefaultShowParagraphCount,
                TextHeaderTitleDefaultFormat = "Title",
                Selected = false //taken care of by OpenBinder                
            };

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
        public Status CreateNewBinder(int userId, Binder newBinder)
        {
            Status status = new Status();
            DateTime userLocalNow = GetUserLocalTime(userId);


            newBinder.UserId = userId;
            newBinder.Created = userLocalNow;
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

            SimpleUser user = GetCurrentSimpleUser(userId);
            if (user.UserId == -1)
            {
                status.success = false;
                status.recordId = -1;
                status.message = "Failed to retrieve user while creating view set for new binder";
                return status;
            }


            //create active saved view for user
            SavedView activeView = new SavedView()
            {
                UserId = userId,
                SetValue = "Active",
                SortValue = "title",
                ViewName = "Default",
                RecordsPerPage = user.DefaultRecordsPerPage,
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
                ViewName = "",
                RecordsPerPage = user.DefaultRecordsPerPage,
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
                RecordsPerPage = user.DefaultRecordsPerPage,
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
                RecordsPerPage = user.DefaultRecordsPerPage,
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
                _context.SaveChanges();
                _context.SavedViews.Add(defaultView);
                _context.SaveChanges();
                _context.SavedViews.Add(trashView);
                _context.SaveChanges();
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
        public Status UpdateBinder(int userId, Binder editedBinder)
        {
            Status status = new Status();
            DateTime userLocalNow = GetUserLocalTime(userId);

            editedBinder.LastModified = userLocalNow;
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
            DateTime userLocalNow = GetUserLocalTime(userId);

            // Only one Binder is to be "open" at a time,
            // So this method will find the user's Selected Binder
            // set Selected = false, and set the binder to open's Selected to True

            try
            {
                Binder currentlySelectedBinder = _context.Binders.Single(x => x.BinderId == GetCurrentBinderID(userId));

                Binder binderToOpen = _context.Binders.Single(x => x.BinderId == binderToOpenId);

                currentlySelectedBinder.Selected = false;
                binderToOpen.Selected = true;
                binderToOpen.LastAccessed = userLocalNow;

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
        public Status UpdateBinderLastAccessed(int binderId, int userId)
        {
            Status status = new Status();
            DateTime userLocalNow = GetUserLocalTime(userId);
            Binder thisBinder = _context.Binders.Single(x => x.BinderId == binderId);
            thisBinder.LastAccessed = userLocalNow;
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
        #endregion

        //  Group Methods:
        #region GroupMethods
        public Status CreateNewTextGroup(int userId, TextGroup newGroup)
        {
            Status status = new Status();
            // Create a new group and a new view for that group

            SavedView defaultSavedView = GetDefaultSavedView(userId, newGroup.BinderId);
            SavedView newGroupView = new SavedView()
            {
                UserId = userId,
                SetValue = null, //setting below, once new group is saved
                SortValue = defaultSavedView.SortValue,
                ViewName = null,
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
                RecordsPerPage = defaultSavedView.RecordsPerPage,
                Groups = defaultSavedView.Groups,
                BinderId = defaultSavedView.BinderId
            };

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
            

            newGroup.OwnerId = userId;
            newGroup.Hidden = false;
            newGroup.Locked = false;
            newGroup.SavedViewId = newGroupView.SavedViewId;

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

            newGroupView.SetValue = newGroup.TextGroupId.ToString();
            newGroupView.ViewName = newGroup.GroupTitle;

            try
            {
                _context.SavedViews.Update(newGroupView);
                _context.SaveChanges();
                status.success = true;
            }
            catch
            {
                status.success = false;
                status.message = $"Failed to save view for new text group {newGroup.GroupTitle}";
                status.recordId = -1;
            }

            return status;
        }
        public Status UpdateGroup(TextGroup editedGroup)
        {
            Status status = new Status();
            // May get a null value from the front end for locked
            //editedGroup.Locked = (editedGroup.Locked == null) ? false : editedGroup.Locked;
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
        public Status AddRemoveHeadersFromGroups(DisplayTextHeadersAndSavedView savedView, int groupID, bool add)
        {
            Status status = new Status();

            try
            {
                //Get a list of all the headerIDs that we're going to update:
                List<int> headerIDsToUpdate = savedView.TextHeaders.Where(x => x.Selected).Select(x => x.TextHeaderId).ToList();

                // TODO - It's inelegant to grab ALL linked records and search them. This can be refactored 
                //  Added a join on selected headers to limit results returned - previously was all. 
                List<LnkTextHeadersTextGroup> existingLinks = _context.LnkTextHeadersTextGroups.Where(x => headerIDsToUpdate.Contains(x.TextHeaderId)).ToList();
                    
                    
                    // _context.LnkTextHeadersTextGroups.Where(x => headerIDsToUpdate.Any(y => y == x.TextHeaderId)).ToList();

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
        public Status AddRemoveHeaderFromGroup(int textHeaderId, int groupId, bool add)
        {
            Status status = new Status();

            if (add)
            {
                LnkTextHeadersTextGroup link = new LnkTextHeadersTextGroup()
                {
                    TextGroupId = groupId,
                    TextHeaderId = textHeaderId
                };
                try
                {
                    _context.LnkTextHeadersTextGroups.Add(link);
                    _context.SaveChanges();
                    status.success = true;
                    status.recordId = link.LnkHeaderGroupId;
                }
                catch
                {
                    status.success = false;
                    status.message = $"Failed to add text header with Id {textHeaderId} to group with id {groupId}";
                }
            }
            else
            {
                try
                {
                    LnkTextHeadersTextGroup link = _context.LnkTextHeadersTextGroups.Single(x => x.TextHeaderId == textHeaderId
                                                                                              && x.TextGroupId == groupId);
                    _context.LnkTextHeadersTextGroups.Remove(link);
                    _context.SaveChanges();
                    status.success = true;
                }
                catch
                {
                    status.success = false;
                    status.message = $"Failed to remove text header with Id {textHeaderId} from group with id {groupId}";
                }
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
        public Status UpdateGroupsForNewVision(int textHeaderId) 
        {
            Status status = new Status();

            int? parentTextHeaderId = _context.TextHeaders.Single(x => x.TextHeaderId == textHeaderId).VersionOf;

            if (parentTextHeaderId == null)
            {
                status.success = false;
                status.message = "Attempted to update groups from parent to child header when no parent header exists";
                status.recordId = -1;
                return status;
            }

            List<LnkTextHeadersTextGroup> textHeadersTextGroups = _context.LnkTextHeadersTextGroups.Where(x => x.TextHeaderId == (int)parentTextHeaderId).ToList();
            if (textHeadersTextGroups.Count == 0)
            {
                status.success = true;
                return status;
            }

            //foreach(LnkTextHeadersTextGroup link in textHeadersTextGroups)
            //{
            //    link.TextHeaderId = textHeaderId;
            //}
            textHeadersTextGroups.ForEach(x => x.TextHeaderId = textHeaderId);

            try
            {
                _context.UpdateRange(textHeadersTextGroups);
                _context.SaveChanges();
                status.success = true;
            }
            catch
            {
                status.success = false;
                status.message = "Failed to migrate text groups from parent to child header";
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

        #endregion

    }
}
