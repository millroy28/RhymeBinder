using Microsoft.Extensions.Logging;
using RhymeBinder.Models.DBModels;
using RhymeBinder.Models.DTOModels;
using RhymeBinder.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RhymeBinder.Models.HelperModels
{
    public class BinderHelper : BaseHelper
    {
        private readonly RhymeBinderContext _context;
        private readonly ILogger<BaseHelper> _logger;

        // private string _aspUserId;
        public BinderHelper(RhymeBinderContext context, ILogger<BaseHelper> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
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
                Selected = false, //taken care of by OpenBinder
                ReadOnly = false
            };

            try
            {
                _context.Binders.Add(newBinder);
                _context.SaveChanges();
                status.success = true;
                status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
                status.message = "Changes saved!";
                status.recordId = newBinder.BinderId;
            }
            catch
            {
                status.recordId = -1;
                status.success = false;
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.message = $"Failed to create new binder";
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
            newBinder.ReadOnly = false;


            try
            {
                _context.Binders.Add(newBinder);
                _context.SaveChanges();
                status.success = true;
                status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
                status.message = "Changes saved!";
                status.recordId = newBinder.BinderId;
            }
            catch
            {
                status.recordId = -1;
                status.success = false;
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.message = $"Failed to create binder";
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
                status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
                status.message = "Changes saved!";
                status.recordId = editedBinder.BinderId;
            }
            catch
            {
                status.success = false;
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
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
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.message = $"Failed to open binder";
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
                status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
                status.message = "Changes saved!";
            }
            catch
            {
                status.success = false;
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.message = $"Failed moving binder contents";
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
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
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
                    status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
                    status.message = "Changes saved!";
                }
                catch
                {
                    status.success = false;
                    status.alertLevel = Enums.AlertLevelEnum.FAIL;
                    status.message = $"Failed to delete binder";
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
                if (!status.success) return status;
                status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
                status.message = "Changes saved!";
            }
            catch
            {
                status.success = false;
                status.message = $"Failed to delete binder and contents";
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
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
                status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
                status.message = "Changes saved!";
            }
            catch
            {
                status.success = false;
                status.recordId = -1;
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.message = "Failure to check or update Loose Pages Binder status";
            }

            return status;
        }
        public Status DuplicateBinder(int userId, int binderId)
        {
            // We want to create a copy of the binder and all its contents
            Status status = new();


            // Get all our models with the original IDs
            Binder binder = _context.Binders.Single(x => x.BinderId == binderId);
            List<SavedView> savedViews = _context.SavedViews.Where(x => x.BinderId == binderId).ToList();
            List<TextGroup> textGroups = _context.TextGroups.Where(x => x.BinderId == binderId).ToList();
            List<TextHeader> textHeaders = _context.TextHeaders.Where(x => x.BinderId == binderId)
                                                               .ToList();
            List<TextNote> textNotes = _context.TextNotes.Where(x => textHeaders.Select(y => y.TextNoteId).Contains(x.TextNoteId)).ToList();
            List<LnkTextHeadersTextGroup> lnkTextHeadersTextGroups = _context.LnkTextHeadersTextGroups.Where(x => textHeaders.Select(y => y.TextHeaderId).Contains(x.TextHeaderId)).ToList();
            List<TextRecord> textRecords = _context.TextRecords.Where(x => textHeaders.Select(y => y.TextHeaderId).Contains(x.TextHeaderId)).ToList();
            List<Text> texts = _context.Texts.Where(x => textRecords.Select(y => y.TextId).Contains(x.TextId)).ToList();
            List<EditWindowProperty> editWindowProperties = _context.EditWindowProperties.Where(x => textHeaders.Select(y => y.TextHeaderId).Contains(x.TextHeaderId)).ToList();
            // used in a query - not for changing/saving data
            List<SavedView> groupViews = savedViews.Where(x => int.TryParse(x.SetValue, out int y)).ToList();


            // Detatch all models (so we can change IDs and submit via add)
            _context.Entry(binder).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            foreach (var savedView in savedViews)
            {
                _context.Entry(savedView).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
            foreach (var textGroup in textGroups)
            {
                _context.Entry(textGroup).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
            foreach (var textHeader in textHeaders)
            {
                _context.Entry(textHeader).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
            foreach (var textNote in textNotes)
            {
                _context.Entry(textNote).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
            foreach (var lnkTextHeadersTextGroup in lnkTextHeadersTextGroups)
            {
                _context.Entry(lnkTextHeadersTextGroup).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
            foreach (var textRecord in textRecords)
            {
                _context.Entry(textRecord).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
            foreach (var text in texts)
            {
                _context.Entry(text).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }
            foreach (var editWindowProperty in editWindowProperties)
            {
                _context.Entry(editWindowProperty).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
            }


            // Binder
            binder.Name = binder.Name + " COPY";
            binder.Created = GetUserLocalTime(userId);
            binder.CreatedBy = userId;
            binder.BinderId = 0;
            binder.TextGroups = null;
            binder.TextHeaders = null;
            binder.SavedViews = null;

            try
            {
                _context.Binders.Add(binder);
                _context.SaveChanges();
                status.success = true;
                status.recordId = binder.BinderId;
            }
            catch
            {
                status.success = false;
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.message = "Failed to save Binder while duplicating from source Binder";
                return status;
            }


            // Groups
            // >> need to grab newly created SavedViewId for each
            textGroups.ForEach(x => x.BinderId = binder.BinderId);
            textGroups.ForEach(x => x.SavedViewId = groupViews.Where(y => int.Parse(y.SetValue) == x.TextGroupId)
                                                              .Select(y => y.SavedViewId).First());
            textGroups.ForEach(x => x.LnkTextHeadersTextGroups = null);
            textGroups.ForEach(x => x.SavedView = null);
            textGroups.ForEach(x => x.GroupHistories = null);

            try
            {
                foreach (var textGroup in textGroups)
                {
                    int origTextGroupId = textGroup.TextGroupId;
                    textGroup.TextGroupId = 0;
                    _context.TextGroups.Add(textGroup);
                    _context.SaveChanges();

                    // Update lnkTextHeadersTextGroups
                    lnkTextHeadersTextGroups.Where(x => x.TextGroupId == origTextGroupId).ToList().ForEach(x => x.TextGroupId = textGroup.TextGroupId);

                    // Update SavedView
                    savedViews.Where(x => int.TryParse(x.SetValue, out int y) && int.Parse(x.SetValue) == origTextGroupId).ToList().ForEach(x => x.SetValue = textGroup.TextGroupId.ToString());
                }

            }
            catch
            {
                status.success = false;
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.message = "Failed to save Text Groups for new Binder while duplicating";
                return status;
            }


            // Saved Views
            savedViews.ForEach(x => x.BinderId = binder.BinderId);
            savedViews.ForEach(x => x.TextGroups = null);

            try
            {
                foreach (var view in savedViews)
                {
                    int originalSavedViewId = view.SavedViewId;
                    view.SavedViewId = 0;
                    _context.SavedViews.Add(view);
                    _context.SaveChanges();

                    // circle back to update Text Group Saved View Id
                    textGroups.Where(x => x.SavedViewId == originalSavedViewId).ToList().ForEach(x => x.SavedViewId = view.SavedViewId);
                    _context.TextGroups.UpdateRange(textGroups);
                    _context.SaveChanges();
                }
            }
            catch
            {
                status.success = false;
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.message = "Failed to save SavedViews for new Binder while duplicating";
                return status;
            }

            // Text Notes
            textNotes.ForEach(x => x.TextHeaders = null);

            try
            {
                foreach (var note in textNotes)
                {
                    int origNoteId = note.TextNoteId;
                    note.TextNoteId = 0;
                    _context.TextNotes.Add(note);
                    _context.SaveChanges();

                    // update TextHeader records
                    textHeaders.Where(x => x.TextNoteId == origNoteId).ToList().ForEach(x => x.TextNoteId = note.TextNoteId);
                }
            }
            catch
            {
                status.success = false;
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.message = "Failed to save Text Notes for new Binder while duplicating";
                return status;
            }

            // Texts
            texts.ForEach(x => x.TextRecords = null);
            texts.ForEach(x => x.TextHeaders = null);
            try
            {
                foreach (var text in texts)
                {
                    int origTextId = text.TextId;
                    text.TextId = 0;
                    _context.Texts.Add(text);
                    _context.SaveChanges();

                    // update TextHeader records
                    textHeaders.Where(x => x.TextId == origTextId).ToList().ForEach(x => x.TextId = text.TextId);

                    // update text records
                    textRecords.Where(x => x.TextId == origTextId).ToList().ForEach(x => x.TextId = text.TextId);
                }
            }
            catch
            {
                status.success = false;
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.message = "Failed to save Texts for new Binder while duplicating";
                return status;
            }


            // Text Headers
            textHeaders.ForEach(x => x.BinderId = binder.BinderId);
            textHeaders.ForEach(x => x.TextRecords = null);
            textHeaders.ForEach(x => x.Text = null);
            textHeaders.ForEach(x => x.Binder = null);
            textHeaders.ForEach(x => x.EditWindowProperties = null);
            textHeaders.ForEach(x => x.GroupHistories = null);
            textHeaders.ForEach(x => x.Submissions = null);
            textHeaders.ForEach(x => x.TextNote = null);
            textHeaders.ForEach(x => x.LnkTextHeadersTextGroups = null);
            textHeaders.ForEach(x => x.LnkTextSubmissions = null);
            textHeaders.ForEach(x => x.TextRevisionStatus = null);
            textHeaders.ForEach(x => x.LastModifiedByNavigation = null);
            textHeaders.ForEach(x => x.CreatedByNavigation = null);
            textHeaders.ForEach(x => x.LastReadByNavigation = null);
            textHeaders.ForEach(x => x.InverseVersionOfNavigation = null);

            try
            {
                foreach (var textHeader in textHeaders)
                {
                    int origTextHeaderId = textHeader.TextHeaderId;
                    textHeader.TextHeaderId = 0;
                    _context.TextHeaders.Add(textHeader);
                    _context.SaveChanges();

                    // update lnkTextHeadersTextGroups
                    lnkTextHeadersTextGroups.Where(x => x.TextHeaderId == origTextHeaderId).ToList().ForEach(x => x.TextHeaderId = textHeader.TextHeaderId);

                    // update TextRecords
                    textRecords.Where(x => x.TextHeaderId == origTextHeaderId).ToList().ForEach(x => x.TextHeaderId = textHeader.TextHeaderId);

                    // update editWindowProperties
                    editWindowProperties.Where(x => x.TextHeaderId == origTextHeaderId).ToList().ForEach(x => x.TextHeaderId = textHeader.TextHeaderId);
                }
            }
            catch
            {
                status.success = false;
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.message = "Failed to save Text Headers for new Binder while duplicating";
                return status;
            }



            // lnkTextHeadersTextGroups
            lnkTextHeadersTextGroups.ForEach(x => x.LnkHeaderGroupId = 0);
            try
            {
                _context.LnkTextHeadersTextGroups.AddRange(lnkTextHeadersTextGroups);
                _context.SaveChanges();
            }
            catch
            {
                status.success = false;
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.message = "Failed to save links between Text Headers and Text Groups for new Binder while duplicating";
                return status;
            }

            // Text Records
            textRecords.ForEach(x => x.TextRecordId = 0);
            try
            {
                _context.TextRecords.AddRange(textRecords);
                _context.SaveChanges();
            }
            catch
            {
                status.success = false;
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.message = "Failed to save Text Records for new Binder while duplicating";
                return status;
            }

            // EditWindowProperties
            editWindowProperties.ForEach(x => x.EditWindowPropertyId = 0);
            try
            {
                _context.EditWindowProperties.AddRange(editWindowProperties);
                _context.SaveChanges();
                status.success = true;
                status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
                status.message = "Changes saved!";
            }
            catch
            {
                status.success = false;
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.message = "Failed to save Edit Window Properties for new Binder while duplicating";
                return status;
            }

            return status;
        }
        public Status UpdateShelf(int userId, List<ShelfUpdateModel> shelfUpdateModels)
        {
            Status status = new();
            List<Shelf> existingShelves = new();

            // get existing
            if (_context.Shelves.Any(x => x.UserId == userId))
            {
                existingShelves = _context.Shelves.Where(x => x.UserId == userId).ToList();
            }


            if (shelfUpdateModels.Count > 0)
            {
                foreach (ShelfUpdateModel shelfUpdateModel in shelfUpdateModels)
                {

                    if (existingShelves.Count() == 0 || !existingShelves.Any(x => x.BinderId == shelfUpdateModel.BinderId))
                    {
                        _context.Shelves.Add(new Shelf
                        {
                            UserId = userId,
                            BinderId = shelfUpdateModel.BinderId,
                            ShelfLevel = shelfUpdateModel.ShelfLevel,
                            SortOrder = shelfUpdateModel.SortOrder
                        });
                    }
                    else
                    {
                        foreach (Shelf shelf in existingShelves.Where(x => x.BinderId == shelfUpdateModel.BinderId))
                        {
                            shelf.ShelfLevel = shelfUpdateModel.ShelfLevel;
                            shelf.SortOrder = shelfUpdateModel.SortOrder;
                        }
                    }

                }

                _context.Shelves.UpdateRange(existingShelves);

                if (existingShelves.Count() > 0)
                {
                    foreach (Shelf shelf in existingShelves)
                    {
                        if (!shelfUpdateModels.Any(x => x.BinderId == shelf.BinderId))
                        {
                            _context.Shelves.Remove(shelf);
                        }
                    }
                }

            }
            else
            {
                if (existingShelves.Count() > 0)
                {
                    _context.Shelves.RemoveRange(existingShelves);
                }
            }

            try
            {
                _context.SaveChanges();
                status.success = true;
                status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
                status.message = "Changes saved!";
            }
            catch
            {
                status.success = false;
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.message = "Failed to update Binder Shelf settings";
            }

            return status;
        }

    }
}
