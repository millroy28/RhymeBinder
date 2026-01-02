using Microsoft.Extensions.Logging;
using NuGet.Protocol;
using RhymeBinder.Models.DBModels;
using RhymeBinder.Models.Enums;
using RhymeBinder.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RhymeBinder.Models.HelperModels
{
    public class UserHelper : BaseHelper
    {
        private readonly RhymeBinderContext _context;
        private readonly ILogger<BaseHelper> _logger;

        public UserHelper(RhymeBinderContext context, ILogger<BaseHelper> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

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
                status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
                status.message = "Changes saved!";
            }
            catch
            {
                status.success = false;
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.message = "Failed to save new user!";
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
                status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
                status.message = "Changes saved!";
                status.recordId = user.UserId;
            }
            catch
            {
                status.success = false;
                status.recordId = -1;
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.message = $"Failed Saving User Changes!";
            }
            return status;
        }
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
                Selected = true,
                ReadOnly = false
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
                Selected = false,
                ReadOnly = false
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
                Selected = false,
                ReadOnly = false
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
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.message = "Failed to create Default Binder Set";
                return status;
            }

            //Each Binder needs a view set as well: 
            status = CreateNewBinderViewSet(defaultBinder.BinderId, newUserId);
            if (status.success) status = CreateNewBinderViewSet(trashBinder.BinderId, newUserId);
            if (status.success) status = CreateNewBinderViewSet(loosePages.BinderId, newUserId);

            return status;
        }
        public Status SaveUserFontSize(int userId, int fontSize)
        {
            Status status = new();
            if(fontSize > 40 || fontSize < 8)
            {
                status.success = false;
                status.alertLevel = Enums.AlertLevelEnum.WARN;
                status.message = "Minimum or Maximum Allowable Font Size Reached";
                return status;
            }
            try
            {
                var currentUser = _context.SimpleUsers.Single(x => x.UserId == userId);
                currentUser.EditViewFontSize = fontSize;
                _context.SaveChanges();

                status.success = true;
                status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
            }
            catch
            {
                status.success = false;
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.message = "Failed to save User's font preference";
                return status;
            }

            return status;
        }

        public Status SaveUserWindowWidth(int userId, int widthLevel)
        {
            Status status = new();
            if(!Enum.IsDefined(typeof(EditWindowWidthLevel), widthLevel)){
                status.success = false;
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.message = "Failed to save User's width preference";
                return status;
            }
            try
            {
                var currentUser = _context.SimpleUsers.Single(x => x.UserId == userId);
                currentUser.EditViewExpandLevel = widthLevel;
                _context.SaveChanges();

                status.success = true;
                status.alertLevel = Enums.AlertLevelEnum.SUCCESS;
            }
            catch
            {
                status.success = false;
                status.alertLevel = Enums.AlertLevelEnum.FAIL;
                status.message = "Failed to save User's font preference";
                return status;
            }

            return status;
        }

        public DisplaySimpleUser GetCurrentDisplaySimpleUser(int userId)
        {
            SimpleUser thisUser = GetCurrentSimpleUser(userId);
            List<DBModels.TimeZone> timeZones = _context.TimeZones.OrderBy(x => x.TimeZoneId).ToList();
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
    }
}
