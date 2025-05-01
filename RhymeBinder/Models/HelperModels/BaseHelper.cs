using Microsoft.Extensions.Logging;
using System.Linq;
using System;
using RhymeBinder.Models.DBModels;
using RhymeBinder.Models.ViewModels;
using System.Collections.Generic;
using RhymeBinder.Models.Enums;

namespace RhymeBinder.Models.HelperModels
{
    public class BaseHelper
    {
        private readonly RhymeBinderContext _context;
        private readonly ILogger<BaseHelper> _logger;

        // private string _aspUserId;
        public BaseHelper(RhymeBinderContext context, ILogger<BaseHelper> logger)
        {
            _context = context;
            _logger = logger;
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
            return thisUserId;
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
                SortValue = "Title",
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
                GroupSequence = false,
                WordCount = false,
                CharacterCount = false,
                BinderId = binderId,

            };

            SavedView defaultView = new SavedView()
            {
                UserId = userId,
                SetValue = "Default",
                SortValue = "Title",
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
                WordCount = false,
                CharacterCount = false,
                GroupSequence = false,
                BinderId = binderId,

            };

            SavedView trashView = new SavedView()
            {
                UserId = userId,
                SetValue = "Hidden",
                SortValue = "Title",
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
                WordCount = false,
                CharacterCount = false,
                GroupSequence = false,
                BinderId = binderId
            };

            SavedView loosePagesView = new SavedView()
            {
                UserId = userId,
                SetValue = "All",
                SortValue = "Title",
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
                GroupSequence = false,
                WordCount = false,
                CharacterCount = false,
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
        public int GetWordCount(string text)
        {
            int count = 0;

            if (!string.IsNullOrEmpty(text))
            {
                char[] delimiters = new char[] { ' ', '\r', '\n' };
                count = text.Split(delimiters, StringSplitOptions.RemoveEmptyEntries).Length;
            }

            return count;
        }
        public string GetMenuTextColor(string hexColor)
        {
            if (string.IsNullOrEmpty(hexColor)) return "black";
            // Remove the # at the start if it's there
            hexColor = hexColor.TrimStart('#');

            // Parse the hex color to RGB values
            int r = int.Parse(hexColor.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            int g = int.Parse(hexColor.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            int b = int.Parse(hexColor.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

            // Calculate the relative luminance
            double luminance = (0.2126 * r + 0.7152 * g + 0.0722 * b) / 255;

            // Return "black" or "white" based on luminance
            return luminance > 0.5 ? "black" : "white";
        }
        public string GetBinderName(int binderId)
        {
            string name;
            try
            {
                name = _context.Binders.Single(x => x.BinderId == binderId).Name;
            }
            catch
            {
                name = "FailedToFetchBinderName";
            }
            return name;
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

            return displayBinder;
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
                displayBinder.Name = binder.Name;//binder.Name.Length > 25 ? binder.Name.Substring(0,25) + "..." : binder.Name;
                displayBinder.Description = binder.Description;
                displayBinder.Color = binder.Color;
                displayBinder.Selected = binder.Selected;
                displayBinder.ReadOnly = binder.ReadOnly;
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
            return displayBinder;
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

                int textCount; int groupCount; int wordCount; int characterCount;

                foreach (var binder in binders)
                {
                    if (_context.Shelves.Any(y => y.BinderId == binder.BinderId && y.UserId == userId))
                    {
                        binder.Shelf = _context.Shelves.DefaultIfEmpty().Single(y => y.BinderId == binder.BinderId && y.UserId == userId);
                    }
                    else
                    {
                        binder.Shelf = new Shelf()
                        {
                            ShelfId = 0,
                            ShelfLevel = 0,
                            SortOrder = 0,
                            BinderId = binder.BinderId,
                            UserId = userId,

                        };
                    }
                    List<TextHeader> headers = textHeaders.Where(x => x.BinderId == binder.BinderId).ToList();
                    textCount = headers.Count();

                    characterCount = headers.Sum(x => x.CharacterCount ?? 0);

                    wordCount = headers.Sum(x => x.WordCount ?? 0);

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
                        CharacterCount = characterCount,
                        WordCount = wordCount,
                        Selected = binder.Selected,
                        ReadOnly = binder.ReadOnly,
                        Color = binder.Color,
                        TitleColor = GetMenuTextColor(binder.Color),
                        Shelf = binder.Shelf,
                        CreatedByName = GetUserName(binder.CreatedBy),
                        ModifyByName = GetUserName(binder.LastModifiedBy),
                        LastAccessed = binder.LastAccessed,
                        LastAccessedByName = GetUserName(binder.LastAccessedBy),
                        LastWorkedIn = binder.LastWorkedIn,
                        WorkedInName = GetUserName(binder.LastWorkedInBy)
                    });

                }
            }
            catch
            {
                displayBinders.Add(new DisplayBinder() { BinderId = -1 });
            }
            return displayBinders;
        }
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
        public bool UserAuthorized(int userId, int objectId, SharedObjectTypeEnum objectType, SharedObjectActionEnum requestedAction)
        {
            bool userAuthorized = false;
           
            // First, check ownership - assuming if you are owner, you have full authority
            switch (objectType)
            {
                case SharedObjectTypeEnum.Binder:
                    userAuthorized = _context.Binders.Any(x => x.BinderId == objectId && x.CreatedBy == userId);
                    break;
                case SharedObjectTypeEnum.TextHeader:
                    userAuthorized = _context.TextHeaders.Any(x => x.TextHeaderId == objectId && x.CreatedBy == userId);
                    break;
                case SharedObjectTypeEnum.TextGroup:
                    userAuthorized = _context.TextGroups.Any(x => x.TextGroupId == objectId && x.OwnerId == userId);
                    break;
                case SharedObjectTypeEnum.TextView:
                    userAuthorized = _context.SavedViews.Any(x => x.SavedViewId == objectId && x.UserId == userId);
                    break;
            }
            // If user is not owner, then check shared authorization
            if (!userAuthorized)
            {
                userAuthorized = _context.SharedObjects.Any(x => x.Grantee == userId
                                                            && x.SharedObjectTypeId == (int)objectType
                                                            && x.ObjectId == objectId
                                                            && x.SharedObjectActionId == (int)requestedAction);
            }

            return userAuthorized;
        }
    }
}
