using Microsoft.Extensions.Logging;
using RhymeBinder.Models.DBModels;
using RhymeBinder.Models.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace RhymeBinder.Models.HelperModels
{
    public class ViewHelper : BaseHelper
    {
        private readonly RhymeBinderContext _context;
        private readonly ILogger<BaseHelper> _logger;

        public ViewHelper(RhymeBinderContext context, ILogger<BaseHelper> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public int GetSavedViewIdBySetValue(int userId, string setValue)
        {
            int binderId = GetCurrentBinderID(userId);
            int savedViewId;

            try
            {
                try
                {
                    savedViewId = int.Parse(setValue);
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
            viewToUpdate.WordCount = (bool)savedView.View.WordCount;
            viewToUpdate.CharacterCount = (bool)savedView.View.CharacterCount;
            viewToUpdate.GroupSequence = savedView.View.GroupSequence ?? false;
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
            defaultSavedView.GroupSequence = newDefaults.GroupSequence;
            defaultSavedView.WordCount = newDefaults.WordCount;
            defaultSavedView.CharacterCount = newDefaults.CharacterCount;

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
            activeView.GroupSequence = defaultView.GroupSequence;
            activeView.WordCount = defaultView.WordCount;
            activeView.CharacterCount = defaultView.CharacterCount;

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
    }
}
