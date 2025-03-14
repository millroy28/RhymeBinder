using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RhymeBinder.Models.DBModels;
using RhymeBinder.Models.DTOModels;
using RhymeBinder.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RhymeBinder.Models.HelperModels
{
    public class TextHelper : BaseHelper
    {
        private readonly RhymeBinderContext _context;
        private readonly ILogger<BaseHelper> _logger;

        // private string _aspUserId;
        public TextHelper(RhymeBinderContext context, ILogger<BaseHelper> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }
        public Status StartNewText(int userId, int? groupId)
        {
            Status status = new Status();
            int verifiedGroupId = 0;
            if (groupId != null && groupId > 0)
            {
                verifiedGroupId = groupId.Value;
            }

            // Create a new blank Text
            Text newText = new Text();
            newText.TextBody = "";
            newText.Created = GetUserLocalTime(userId);
            try
            {
                _context.Texts.Add(newText);
                _context.SaveChanges();
                status = CreateNewTextHeader(userId, newText.TextId);
                int newHeaderId = status.recordId;
                status = CreateNewTextHeaderEditWindowProperty(userId, status.recordId);

                if (status.success && verifiedGroupId != 0)
                {
                    status = AddRemoveHeaderFromGroup(newHeaderId, verifiedGroupId, true);
                    // calling function relies on textheaderid to redirect
                    status.recordId = newHeaderId;
                    status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
                    status.message = "Changes saved!";

                }
            }
            catch
            {
                status.success = false;
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
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
                status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
                status.message = "Changes saved!";

            }
            catch
            {
                status.success = false;
                status.recordId = -1;
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.message = "Failed to save Text";
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
                status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
                status.message = "Changes saved!";
            }
            catch
            {
                status.success = false;
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.message = $"Failed creating header for text";
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
                status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
                status.message = "Changes saved!";
            }
            catch
            {
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.success = false;
                status.message = $"Failed creating new child header for parent header";
            }

            // Migrate, if any, group associations from previous header to new header
            status = UpdateGroupsForNewVision(status.recordId);
            if (status.success) { status.recordId = newTextHeader.TextHeaderId; } // ensure TextHeaderId is returned as status

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
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                return status;
            }

            List<LnkTextHeadersTextGroup> textHeadersTextGroups = _context.LnkTextHeadersTextGroups.Where(x => x.TextHeaderId == (int)parentTextHeaderId).ToList();
            if (textHeadersTextGroups.Count == 0)
            {
                status.success = true;
                status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
                status.message = "Changes saved!";
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
                status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
                status.message = "Changes saved!";
            }
            catch
            {
                status.success = false; 
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.message = "Failed to migrate text groups from parent to child header";
            }

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
                status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
                status.message = "Changes saved!";
            }
            catch
            {
                status.success = false;
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
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
                if (textHeader.VersionOf != null)
                {
                    EditWindowProperty previousEditWindowProperty = _context.EditWindowProperties.Where(x => x.TextHeaderId == textHeader.VersionOf).First();
                    editWindowProperty.ShowLineCount = previousEditWindowProperty.ShowLineCount;
                    editWindowProperty.ShowParagraphCount = previousEditWindowProperty.ShowParagraphCount;
                }

                _context.EditWindowProperties.Add(editWindowProperty);
                _context.SaveChanges();
                status.success = true;
                status.recordId = textHeaderId;
                status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
                status.message = "Changes saved!";
            }
            catch
            {
                status.success = false;
                status.recordId = -1;
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.message = $"Failed to set default Edit Window Properties for text header";
            }
            return status;
        }

        public List<DisplayTextGroup> GetDisplayTextGroups(int userId, int binderId, int textHeaderId)
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
                        Selected = _context.LnkTextHeadersTextGroups.Any(x => x.TextGroupId == group.TextGroupId
                                                                           && x.TextHeaderId == textHeaderId)
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

            return displayTextGroups;
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
            return displayTextGroups;
        }
        public List<DisplayTextHeader> GetDisplayTextHeaders(SavedView savedView, int page)
        {
            //TO DO: make this less ponderous and introduce error handling?
            //Get all text headers for this view
            List<TextHeader> theseTextHeaders = new List<TextHeader>();
            List<LnkTextHeadersTextGroup> theseTextHeaderGroupLinks = new List<LnkTextHeadersTextGroup>();
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
                        theseTextHeaderGroupLinks = _context.LnkTextHeadersTextGroups.Where(x => theseTextHeaders.Select(y => y.TextHeaderId).Contains(x.TextHeaderId)
                                                                                              && x.TextGroupId == groupId).ToList();
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

            List<LnkTextHeadersTextGroup> links = _context.LnkTextHeadersTextGroups.Where(x => allGroups.Select(y => y.TextGroupId).Contains(x.TextGroupId)).ToList();

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

                int? groupSequence = theseTextHeaderGroupLinks.Where(x => x.TextHeaderId == textHeader.TextHeaderId).Select(x => x.Sequence).SingleOrDefault();

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
                    RevisionStatus = _context.TextRevisionStatuses.Single(x => x.TextRevisionStatusId == textHeader.TextRevisionStatusId).TextRevisionStatus1,
                    GroupSequence = groupSequence,
                    CharacterCount = textHeader.CharacterCount ?? 0,
                    WordCount = textHeader.WordCount ?? 0
                });
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
                        BinderId = savedView.BinderId
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
                case "Sequence":
                    theseDisplayTextHeaders = theseDisplayTextHeaders.OrderBy(x => x.GroupSequence).ToList();
                    break;
            }
            if (savedView.Descending == true)
            {
                theseDisplayTextHeaders.Reverse();
            }

            return theseDisplayTextHeaders;
        }
        public DisplayTextHeadersAndSavedView GetDisplayTextHeadersAndSavedView(int userId, int viewId, int page)
        {
            DisplayTextHeadersAndSavedView displayTextHeadersAndSavedView = new DisplayTextHeadersAndSavedView()
            {
                Page = page
            };

            SavedView savedView = GetSavedView(viewId);
            DisplayBinder binder = GetDisplayBinder(userId, savedView.BinderId);
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
            //displayTextHeadersAndSavedView.MenuTitle = (binder.Name + ": " + savedView.ViewName).Length > 25
            //                                           ? (binder.Name + ": " + savedView.ViewName).Substring(0, 25) + "..."
            //                                           : binder.Name + ": " + savedView.ViewName;
            displayTextHeadersAndSavedView.MenuTitle = binder.Name + ": " + savedView.ViewName;
            displayTextHeadersAndSavedView.MenuTitleColor = GetMenuTextColor(binder.Color);
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
            return groups;
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

            foreach (var group in groups)
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
            return displayGroups;
        }
        public TextEdit GetTextHeaderBodyUserRecord(int userId, int textHeaderID, bool details = true)
        {
            // Build up a TextHeaderBodyUserRecord to pass into a view:
            // obvs, gonna need that TextHeader:
            TextHeader thisTextHeader = _context.TextHeaders.Find(textHeaderID);

            //grab the related text (if it exists)
            Text thisText = GetText(thisTextHeader.TextId);

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
            if (details)
            {
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
            }


            //build up previous Text "visions" and TextHeaders for them
            List<TextHeader> previousTextHeaders = new List<TextHeader>();
            List<Text> previousTexts = new List<Text>();
            List<SimpleTextHeaderAndText> previousTextsAndHeaders = new List<SimpleTextHeaderAndText>();
            List<SimpleUser> users = _context.SimpleUsers.ToList();

            if (details)
            {
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

                    previousTextsAndHeaders = previousTextsAndHeaders.OrderByDescending(x => x.VisionNumber).ToList();
                }
                catch
                {
                }
            }

            //get text groups
            List<DisplayTextGroup> displayTextGroups = GetDisplayTextGroups(userId, thisTextHeader.BinderId, textHeaderID);

            string binderColor = _context.Binders.DefaultIfEmpty().Single(x => x.BinderId == thisTextHeader.BinderId).Color;
            bool readOnly = _context.Binders.DefaultIfEmpty().Single(x => x.BinderId == thisTextHeader.BinderId).ReadOnly;

            //wrap it up and send it
            TextEdit textEdit = new TextEdit()
            {
                UserId = userId,

                BinderReadOnly = readOnly,

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

                BinderColor = binderColor,
                DisplayTitleColor = GetMenuTextColor(binderColor),
                DisplayTitle = thisTextHeader.Title.Length > 20 ? thisTextHeader.Title.Substring(0, 20) + "..." : thisTextHeader.Title,
                CreatedByUserName = createdUser.UserName,
                LastModifiedByUserName = lastModifiedUser.UserName,
                CurrentRevisionStatus = currentRevisionStatus,
                Groups = displayTextGroups,
                //MemberOfGroups = memberOfGroups,
                //AvailableGroups = availableGroups,

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

            return textEdit;
        }
        public DisplaySequencedTexts GetSequenceOfTextHeaderBodyUserRecord(int userId, int groupId)
        {
            // To serve view where user can read all texts in a sequence
            TextGroup group = _context.TextGroups.Single(x => x.TextGroupId == groupId);
            string binderColor = _context.Binders.DefaultIfEmpty().Single(x => x.BinderId == group.BinderId).Color;
            bool binderReadOnly = _context.Binders.DefaultIfEmpty().Single(x => x.BinderId == group.BinderId).ReadOnly;
            DisplaySequencedTexts displaySequencedTexts = new()
            {
                UserId = userId,
                GroupName = group.GroupTitle,
                GroupId = group.TextGroupId,
                BinderId = group.BinderId,
                BinderName = GetBinderName(group.BinderId),
                BinderColor = binderColor,
                BinderReadOnly = binderReadOnly,
                BinderNameColor = GetMenuTextColor(binderColor)
            };

            List<TextHeader> textHeaders = GetTextHeadersInGroupSequence(groupId);
            List<DisplaySimpleText> simpleTexts = new();

            foreach (TextHeader textHeader in textHeaders)
            {
                // check that text has a sequence number
                if (_context.LnkTextHeadersTextGroups.Any(x => x.TextGroupId == groupId && x.TextHeaderId == textHeader.TextHeaderId && x.Sequence != null))
                {
                    simpleTexts.Add(new DisplaySimpleText
                    {
                        TextHeaderId = textHeader.TextHeaderId,
                        Title = textHeader.Title,
                        TextBody = _context.Texts.Single(x => x.TextId == textHeader.TextId).TextBody,
                        SequenceNumber = (int)_context.LnkTextHeadersTextGroups.Single(x => x.TextHeaderId == textHeader.TextHeaderId && x.TextGroupId == groupId).Sequence,
                        MemberOfGroups = _context.TextGroups.Where(x => _context.LnkTextHeadersTextGroups.Where(y => y.TextHeaderId == textHeader.TextHeaderId)
                                                                                                     .Select(y => y.TextGroupId)
                                                                                                         .Contains(x.TextGroupId)
                                                                                                         ).Select(x => x.GroupTitle).ToList(),
                        Note = _context.TextNotes.Single(x => x.TextNoteId == textHeader.TextNoteId).Note,
                        LastModified = textHeader.LastModified,
                        LastModifiedBy = GetUserName(textHeader.LastModifiedBy)

                    });
                }
            };

            displaySequencedTexts.SimpleTexts = simpleTexts;
            displaySequencedTexts.EditedSimpleTexts = simpleTexts;

            return displaySequencedTexts;
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
        public List<TextHeader> GetTextHeadersInGroupSequence(int textGroupId)
        {
            List<TextHeader> textHeaders = new();
            List<LnkTextHeadersTextGroup> lnks = _context.LnkTextHeadersTextGroups.Where(x => x.TextGroupId == textGroupId).OrderBy(x => x.Sequence).ToList();
            foreach (var lnk in lnks)
            {
                textHeaders.Add(_context.TextHeaders.Single(x => x.TextHeaderId == lnk.TextHeaderId));
            }

            // going to have to two step this as the this expression is not reliably setting the order correctly. Saving for reference:
            /*
            textHeaders = _context.TextHeaders.Where(x => _context.LnkTextHeadersTextGroups.Where(y => y.TextGroupId == textGroupId
                                                                                                    && y.Sequence != null)
                                                                                           .OrderBy(y => y.Sequence)
                                                                                           .Select(y => y.TextHeaderId)
                                                                                           .Contains(x.TextHeaderId)
                                                                                            ).ToList();
            */

            return textHeaders;
        }

        public Status SaveEditedText(TextEdit textEdit)
        {
            Status status = new Status();
            //prevent blank from being saved as title (which would break navitgation)
            if (textEdit.Title == null || textEdit.Title.Trim() == "") textEdit.Title = "(blank)";

            //Check for change and only save if something has changed
            bool noteUnchanged;
            bool textUnchanged;


            Text origText = GetText(textEdit.TextId);
            TextHeader origHeader = _context.TextHeaders.Find(textEdit.TextHeaderId);
            TextNote origNote = _context.TextNotes.Find(textEdit.TextNoteId);

            noteUnchanged = TextsAreSame(origNote.Note, textEdit.Note);
            textUnchanged = TextsAreSame(origText.TextBody, textEdit.TextBody);


            if (!textUnchanged)
            {
                status = CreateNewText(textEdit.UserId, textEdit.TextBody);  //New text created on each edit
                if (!status.success) 
                {
                    status.alertLevel = Enums.AlertLevelEnum.FAIL;
                    status.message = "Failed to create new text body on edit";
                    return status; 
                }
                origHeader.TextId = status.recordId; //point header to new text id

                status = CreateNewTextRecord(textEdit.TextHeaderId, origHeader.TextId, textEdit.UserId);
                if (!status.success) 
                {
                    status.alertLevel = Enums.AlertLevelEnum.FAIL;
                    status.message = "Failed to save text record on edit";
                    return status; 
                }


            };

            if (!noteUnchanged)
            {
                origNote.Note = textEdit.Note;
                status = Update(origNote);
                if (!status.success) 
                {
                    status.alertLevel = Enums.AlertLevelEnum.FAIL;
                    status.message = "Failed to save note on text edit";
                    return status; 
                }
            }


            origHeader.Title = textEdit.Title;
            origHeader.LastModified = GetUserLocalTime(textEdit.UserId);
            origHeader.LastModifiedBy = textEdit.UserId;
            origHeader.TextRevisionStatusId = textEdit.TextRevisionStatusId;

            origHeader.CharacterCount = textEdit.TextBody?.Length ?? 0;
            origHeader.WordCount = GetWordCount(textEdit.TextBody);

            status = Update(origHeader);
            if (!status.success) 
            {
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.message = "Failed to update header on text edit";
                return status; 
            }


            //Update group association changes, if any
            status = AddRemoveHeaderFromGroups(textEdit);
            if (!status.success) 
            {
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.message = "Failed to update group associations on text edit";
                return status; 
            }

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

            status = Update(thisEditWindowProperty);

            if (status.success)
            {
                status = UpdateBinderLastWorkedIn(origHeader.BinderId, textEdit.UserId);
            }
            if (!status.success) 
            {
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.message = "Failed to update window properties on text edit";
                return status; 
            }
            else
            {
                // Still have to return a success status even if no new record was saved
                status.success = true;
                status.recordId = textEdit.TextHeaderId;
                status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
                status.message = "Changes saved!";
            }
            return status;
        }
        public Status SaveEditedTextsInSequence(DisplaySequencedTexts displaySequencedTexts)
        {
            Status status = new();
            List<DisplaySimpleText> editedTexts = displaySequencedTexts.EditedSimpleTexts.Where(x => x.IsChanged == true).ToList();

            if (editedTexts == null || editedTexts.Count() == 0)
            {
                status.success = false;
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.message = "Failed to receive edited texts in edit sequence texts form";
                return status;
            }

            foreach (var text in editedTexts)
            {
                //prevent blank from being saved as title (which would break navitgation)
                if (text.Title == null || text.Title.Trim() == "") text.Title = "(blank)";

                TextHeader origHeader = _context.TextHeaders.Find(text.TextHeaderId);
                Text origText = _context.Texts.Find(origHeader.TextId);
                TextNote origNote = new();
                if (_context.TextNotes.Any(x => x.TextNoteId == origHeader.TextNoteId))
                {
                    origNote = _context.TextNotes.Find(origHeader.TextNoteId);
                }

                if (!TextsAreSame(origText.TextBody, text.TextBody))
                {
                    status = CreateNewText(displaySequencedTexts.UserId, text.TextBody);
                    if (!status.success) 
                    {
                        status.alertLevel = Enums.AlertLevelEnum.FAIL;
                        status.message = "Failed to create new text body in edited text sequence";
                        return status; 
                    }
                    origHeader.TextId = status.recordId; //point header to new text id

                    status = CreateNewTextRecord(origHeader.TextHeaderId, origHeader.TextId, displaySequencedTexts.UserId);
                    if (!status.success) 
                    {
                        status.alertLevel = Enums.AlertLevelEnum.FAIL;
                        status.message = "Failed to create new text record in edited text sequence";
                        return status; 
                    }
                }

                origHeader.Title = text.Title;
                origHeader.LastModified = GetUserLocalTime(displaySequencedTexts.UserId);
                origHeader.LastModifiedBy = displaySequencedTexts.UserId;
                status = Update(origHeader);
                if (!status.success) 
                {
                    status.alertLevel = Enums.AlertLevelEnum.FAIL;
                    status.message = "Failed to update header in edited text sequence";
                    return status; 
                }
                if (origNote.Note != text.Note)
                {
                    origNote.Note = text.Note;
                    status = Update(origNote);
                    if (!status.success) 
                    {
                        status.alertLevel = Enums.AlertLevelEnum.FAIL;
                        status.message = "Failed to update note in edited text sequence";
                        return status; 
                    }
                }
            }

            status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
            status.message = "Changes saved!";
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
                status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
                status.message = "Changes saved!";
            }
            catch
            {
                status.success = false;
                status.recordId = savedView.View.SavedViewId;
                status.message = $"Failed to save Text Hidden state to {hide} for view";
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
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
                        status.message = $"Failed to transfer text header across binders";
                        status.alertLevel = Enums.AlertLevelEnum.FAIL;
                        return status;
                    }
                }
            }

            _context.SaveChanges();
            status.success = true;
            status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
            status.message = "Changes saved!";
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
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.message = "Failed to create new header while adding revision";
                return status;
            }

            status = CreateNewTextHeaderEditWindowProperty(userId, status.recordId);
            if (!status.success)
            {
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.message = "Failed to create new window properties while adding revision";
                return status;
            }

            currentTextHeader.Top = false;
            currentTextHeader.Locked = true;

            try
            {
                _context.Entry(currentTextHeader).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(currentTextHeader);
                _context.SaveChanges();
                status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
                status.message = "Changes saved!";
            }
            catch
            {
                status.success = false;
                status.recordId = -1;
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.message = "Failed to save changes to new header while adding revision";
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
                return prevTextHeaders;
            }
            return prevTextHeaders;
        }
        public Status AddRemoveHeadersFromGroups(DisplayTextHeadersAndSavedView savedView)
        {
            /*
             * New approach: Selected texts are added to / removed from groups via modal where all groups are listed.
             * 
             * On modal load:
             *  In UI, if all selected texts are in a group, the checkbox for that group is checked,
             *  ...if none of the selected texts are in a group, the checkbox for that group is unchecked,
             *  ...if some of the selected texts are in a group, the checkbox is indeterminate
             * 
             *  Checkbox values are returned as null, regardless of state on ui load, and are only set on click.
             *  So assuming any groups in Saved View returning with null selected values require no action.
             * 
             *  groups with select = true - will add all selected texts to groups (checking against duplicateS)
             *  groups with select = false - will remove all selected texts from groups
             */
            Status status = new Status();

            List<int> selectedTextHeaderIds = savedView.TextHeaders.Where(x => x.Selected == true).Select(x => x.TextHeaderId).ToList();
            List<int> groupIdsToRemove = savedView.Groups.Where(x => x.Selected != null && x.Selected == false).Select(x => x.TextGroupId).ToList();
            List<int> groupIdsToAdd = savedView.Groups.Where(x => x.Selected != null && x.Selected == true).Select(x => x.TextGroupId).ToList();

            // removals
            if (groupIdsToRemove.Count > 0)
            {
                List<LnkTextHeadersTextGroup> lnkTextHeadersTextGroupsToRemove = _context.LnkTextHeadersTextGroups.Where(x => groupIdsToRemove.Select(y => y).Contains(x.TextGroupId)
                                                                                                                   && selectedTextHeaderIds.Select(y => y).Contains(x.TextHeaderId)).ToList();

                try
                {
                    _context.LnkTextHeadersTextGroups.RemoveRange(lnkTextHeadersTextGroupsToRemove);
                    _context.SaveChanges();
                    status.success = true;
                    status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
                    status.message = "Changes saved!";
                }
                catch
                {
                    status.success = false;
                    status.recordId = -1;
                    status.message = "Failed to remove selected texts from selected groups";
                    status.alertLevel = Enums.AlertLevelEnum.FAIL;
                }
            }

            // additions
            if (groupIdsToAdd.Count > 0)
            {
                List<LnkTextHeadersTextGroup> lnkTextHeadersTextGroupsToAdd = new List<LnkTextHeadersTextGroup>();
                List<LnkTextHeadersTextGroup> lnkTextHeadersTextGroupAlreadyExisting = _context.LnkTextHeadersTextGroups.Where(x => groupIdsToAdd.Select(y => y).Contains(x.TextGroupId)
                                                                                                                   && selectedTextHeaderIds.Select(y => y).Contains(x.TextHeaderId)).ToList();


                foreach (var groupId in groupIdsToAdd)
                {
                    foreach (var textId in selectedTextHeaderIds)
                    {
                        if (!lnkTextHeadersTextGroupAlreadyExisting.Any(x => x.TextGroupId == groupId && x.TextHeaderId == textId))
                        {
                            lnkTextHeadersTextGroupsToAdd.Add(new LnkTextHeadersTextGroup()
                            {
                                TextGroupId = groupId,
                                TextHeaderId = textId
                            });
                        }
                    }
                }

                if (lnkTextHeadersTextGroupsToAdd.Count > 0)
                {
                    try
                    {
                        _context.LnkTextHeadersTextGroups.AddRange(lnkTextHeadersTextGroupsToAdd);
                        _context.SaveChanges();
                        status.success = true;
                        status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
                        status.message = "Changes saved!";
                    }
                    catch
                    {
                        status.success = false;
                        status.recordId = -1;
                        status.message = "Failed to add selected texts to selected groups";
                        status.alertLevel = Enums.AlertLevelEnum.FAIL;
                    }
                }
            }
            return status;
        }
        public Status AddRemoveHeaderFromGroups(TextEdit textEdit)
        {
            Status status = new Status();

            if (!_context.TextGroups.Any(x => x.BinderId == textEdit.BinderId))
            {
                status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
                status.message = "Changes saved!";
                status.success = true;
                return status;
            }

            List<int> groupIdsToRemove = textEdit.Groups.Where(x => x.Selected != null && x.Selected == false).Select(x => x.TextGroupId).ToList();
            List<int> groupIdsToAdd = textEdit.Groups.Where(x => x.Selected != null && x.Selected == true).Select(x => x.TextGroupId).ToList();

            //removals
            if (groupIdsToRemove.Count > 0)
            {
                List<LnkTextHeadersTextGroup> lnkTextHeadersTextGroupsToRemove = _context.LnkTextHeadersTextGroups.Where(x => groupIdsToRemove.Select(y => y).Contains(x.TextGroupId)
                                                                                                                           && x.TextHeaderId == textEdit.TextHeaderId).ToList();

                try
                {
                    _context.LnkTextHeadersTextGroups.RemoveRange(lnkTextHeadersTextGroupsToRemove);
                    _context.SaveChanges();
                    status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
                    status.message = "Changes saved!";
                    status.success = true;
                }
                catch
                {
                    status.success = false;
                    status.recordId = -1;
                    status.alertLevel = Enums.AlertLevelEnum.FAIL;
                    status.message = "Failed to remove selected texts from selected groups";
                }
            }

            // additions
            if (groupIdsToAdd.Count > 0)
            {
                List<LnkTextHeadersTextGroup> lnkTextHeadersTextGroupsToAdd = new List<LnkTextHeadersTextGroup>();
                List<LnkTextHeadersTextGroup> lnkTextHeadersTextGroupAlreadyExisting = _context.LnkTextHeadersTextGroups.Where(x => groupIdsToAdd.Select(y => y).Contains(x.TextGroupId)
                                                                                                                   && x.TextHeaderId == textEdit.TextHeaderId).ToList();


                foreach (var groupId in groupIdsToAdd)
                {
                    if (!lnkTextHeadersTextGroupAlreadyExisting.Any(x => x.TextGroupId == groupId))
                    {
                        lnkTextHeadersTextGroupsToAdd.Add(new LnkTextHeadersTextGroup()
                        {
                            TextGroupId = groupId,
                            TextHeaderId = textEdit.TextHeaderId
                        });
                    }
                }

                if (lnkTextHeadersTextGroupsToAdd.Count > 0)
                {
                    try
                    {
                        _context.LnkTextHeadersTextGroups.AddRange(lnkTextHeadersTextGroupsToAdd);
                        _context.SaveChanges();
                        status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
                        status.message = "Changes saved!";
                        status.success = true;
                    }
                    catch
                    {
                        status.success = false;
                        status.recordId = -1;
                        status.alertLevel = Enums.AlertLevelEnum.FAIL;
                        status.message = "Failed to add selected texts to selected groups";
                    }
                }
                else
                {
                    status.success = true;
                }
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
                    status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
                    status.message = "Changes saved!";
                }
                catch
                {
                    status.success = false;
                    status.message = $"Failed to add text header to group";
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
                    status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
                    status.message = "Changes saved!";
                }
                catch
                {
                    status.success = false;
                    status.alertLevel = Enums.AlertLevelEnum.FAIL;
                    status.message = $"Failed to remove text header with from group";
                }
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
                status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
                status.message = "Changes saved!";
            }
            catch
            {
                status.success = false;
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.message = "Failure to update Binder";
            }
            return status;

        }
        public Status UpdateBinderLastWorkedIn(int binderId, int userId)
        {
            Status status = new Status();
            DateTime userLocalNow = GetUserLocalTime(userId);
            Binder thisBinder = _context.Binders.Single(x => x.BinderId == binderId);
            thisBinder.LastWorkedIn = userLocalNow;
            thisBinder.LastWorkedInBy = userId;
            try
            {
                _context.Entry(thisBinder).State = Microsoft.EntityFrameworkCore.EntityState.Modified;  //remember to copy paste this honkin thing
                _context.Update(thisBinder);
                _context.SaveChanges();
                status.success = true;
                status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
                status.message = "Changes saved!";
            }
            catch
            {
                status.success = false;
                status.message = "Failure to update Binder";
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
            }
            return status;
        }

    }


}
