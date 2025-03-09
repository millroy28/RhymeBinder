using Microsoft.Extensions.Logging;
using RhymeBinder.Models.DBModels;
using RhymeBinder.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace RhymeBinder.Models.HelperModels
{
    public class GroupHelper : BaseHelper
    {
        private readonly RhymeBinderContext _context;
        private readonly ILogger<BaseHelper> _logger;

        // private string _aspUserId;
        public GroupHelper(RhymeBinderContext context, ILogger<BaseHelper> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

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
                GroupSequence = true,
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

            // Update saved view with new group name.

            SavedView groupSavedView = _context.SavedViews.SingleOrDefault(x => x.SetValue == editedGroup.TextGroupId.ToString());
            // bool isSetValueInt = int.TryParse(savedView.View.SetValue, out int groupId);
            if (groupSavedView != null)
                try
                {
                    groupSavedView.ViewName = editedGroup.GroupTitle;
                    _context.Entry(editedGroup).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.Update(editedGroup);
                    _context.SaveChanges();
                    status.success = true;
                }
                catch
                {
                    status.success = false;
                    status.message = $"Failed to update save view for group {editedGroup.GroupTitle}";
                    status.recordId = -1;
                }

            return status;
        }
        public Status UpdateGroupSequence(DisplayTextHeadersAndSavedView savedView)
        {
            Status status = new Status();

            bool isSetValueInt = int.TryParse(savedView.View.SetValue, out int groupId);
            if (!isSetValueInt)
            {
                status.success = false;
                status.recordId = -1;
                status.message = "Unable to parse group ID from saved view when attempting to update Group Sequences";
                return status;
            }
            List<LnkTextHeadersTextGroup> lnkTextHeadersTextGroup = _context.LnkTextHeadersTextGroups.Where(x => savedView.TextHeaders.Select(y => y.TextHeaderId).Contains(x.TextHeaderId)
                                                                                                        && x.TextGroupId == groupId).ToList();
            if (lnkTextHeadersTextGroup.Count > 0)
            {
                foreach (var lnk in lnkTextHeadersTextGroup)
                {
                    lnk.Sequence = savedView.TextHeaders.Where(x => x.TextHeaderId == lnk.TextHeaderId).Select(x => x.GroupSequence).First();
                }
                try
                {
                    _context.UpdateRange(lnkTextHeadersTextGroup);
                    _context.SaveChanges();
                    status.success = true;
                    status.recordId = savedView.View.SavedViewId;
                }
                catch
                {
                    status.success = false;
                    status.recordId = -1;
                    status.message = "Unable to save group sequence";
                    return status;
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
                    GroupSequence = defaultSavedView.GroupSequence,
                    WordCount = defaultSavedView.WordCount,
                    CharacterCount = defaultSavedView.CharacterCount,
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

    }
}
