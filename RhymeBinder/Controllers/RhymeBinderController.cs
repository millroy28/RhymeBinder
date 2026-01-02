using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using RhymeBinder.Models.DBModels;
using RhymeBinder.Models.DTOModels;
using RhymeBinder.Models.Enums;
using RhymeBinder.Models.HelperModels;
using RhymeBinder.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Claims;


namespace RhymeBinder.Controllers
{
    [Authorize]
    public class RhymeBinderController : Controller
    {
        public ModelHelper _modelHelper;
        public RhymeBinderController(ModelHelper modelHelper)
        {
            _modelHelper = modelHelper;
        }

        //-------USER:
        #region UserMethods
        [HttpGet]
        public IActionResult SetupNewUser()
        {
            SimpleUser newUser = new SimpleUser()
            {
                AspNetUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value
            };
            return View(newUser);
        }
        [HttpPost]
        public IActionResult SetupNewUser(SimpleUser newUser)
        {
            Status status = _modelHelper.UserHelper.SetupNewUser(newUser);
            SetAlertCookieGenericSaveStatus(status.success);
            if (status.success)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("ErrorPage", status);
            }
        }
        [HttpGet]
        public IActionResult EditUser()
        {
            // Check for alerts
            TempData["AlertMessage"] = HttpContext.Request.Cookies["AlertMessage"];
            TempData["AlertSeverity"] = HttpContext.Request.Cookies["AlertSeverity"];
            ClearAlertCookie();

            int userId = GetUserId();
            DisplaySimpleUser user = _modelHelper.UserHelper.GetCurrentDisplaySimpleUser(userId);


            if (user.UserId == -1)
            {
                SetAlertCookie("Failed to retrieve user data", "FAIL");

                return RedirectToAction("ListTextsOnSessionStart");
            }
            return View(user);
        }
        [HttpPost]
        public IActionResult EditUser(SimpleUser editedUser)
        {
            // Check for alerts
            TempData["AlertMessage"] = HttpContext.Request.Cookies["AlertMessage"];
            TempData["AlertSeverity"] = HttpContext.Request.Cookies["AlertSeverity"];
            ClearAlertCookie();

            Status status = _modelHelper.UserHelper.UpdateSimpleUser(editedUser);
            SetAlertCookieGenericSaveStatus(status.success);

            return RedirectToAction("ListTextsOnSessionStart");
        }
        #endregion

        //-------TEXT:
        #region TextMethods
        public IActionResult StartNewText(int binderId, int? groupId)
        { 
            //  Authorization check
            int userId = GetUserId();
            bool isAuthorised = true;
            if(groupId != null && groupId != -1)
            {
                isAuthorised = _modelHelper.TextHelper.UserAuthorized(userId, (int)groupId, SharedObjectTypeEnum.TextGroup, SharedObjectActionEnum.CREATE);
            }
            if (!isAuthorised)
            {
                SetAlertCookie("You do not have permission to add a text in this group", "WARN");
                return RedirectToAction("ListTextsOnSessionStart");
            }

            Status status = _modelHelper.TextHelper.StartNewText(userId, binderId, groupId);

            if (status.success)
            {
                return Redirect($"/RhymeBinder/EditText?textHeaderID={status.recordId}");
            }
            else
            {
                SetAlertCookie("Failed creating a new text", "FAIL");
                return RedirectToAction("ListTextsOnSessionStart");
            }
        }
        [HttpGet]
        public IActionResult ViewText(int textHeaderID)
        {
            // Check for alerts
            TempData["AlertMessage"] = HttpContext.Request.Cookies["AlertMessage"];
            TempData["AlertSeverity"] = HttpContext.Request.Cookies["AlertSeverity"];
            ClearAlertCookie();

            // Authorization check
            int userId = GetUserId();
            if (!_modelHelper.TextHelper.UserAuthorized(userId, textHeaderID, SharedObjectTypeEnum.TextHeader, SharedObjectActionEnum.READ))
            {
                SetAlertCookie("You do not have permission to read this text", "WARN");
                return RedirectToAction("ListTextsOnSessionStart");
            }

            TextEdit textEdit = _modelHelper.TextHelper.GetTextHeaderBodyUserRecord(userId, textHeaderID);

            return View(textEdit);
        }
        [HttpPost]
        public IActionResult ViewText(TextEdit textEdit, string action, string value)
        {
            Status status = new Status();

            switch (action) 
            {
                case "InsertNewTextInSequence":
                    status = _modelHelper.TextHelper.AddNewTextAtPositionInSequence(textEdit, value);
                    SetAlertCookieGenericSaveStatus(status.success);
                    return Redirect($"/RhymeBinder/EditText?textHeaderID={status.recordId}");
                default:
                    return Redirect($"/RhymeBinder/ViewText?textHeaderID={textEdit.TextHeaderId}");
            }
        }

        [HttpGet]
        public IActionResult EditText(int textHeaderID)
        {
            // Check for alerts
            TempData["AlertMessage"] = HttpContext.Request.Cookies["AlertMessage"];
            TempData["AlertSeverity"] = HttpContext.Request.Cookies["AlertSeverity"]; 
            ClearAlertCookie();

            // Authorization check
            int userId = GetUserId();
            if(!_modelHelper.TextHelper.UserAuthorized(userId, textHeaderID, SharedObjectTypeEnum.TextHeader, SharedObjectActionEnum.EDIT))
            {
                SetAlertCookie("You do not have permission to edit this text", "WARN");
                return Redirect($"/RhymeBinder/ViewText?textHeaderID={textHeaderID}");
            }

            TextEdit textEdit = _modelHelper.TextHelper.GetTextHeaderBodyUserRecord(userId, textHeaderID);

            if ((bool)textEdit.Locked == true || textEdit.BinderReadOnly)
            {
                SetAlertCookie("Text Locked from Editing", "WARN");
                return Redirect($"/RhymeBinder/ViewText?textHeaderID={textHeaderID}");
            }

            return View(textEdit);
        }
        [HttpPost]
        public IActionResult EditText(TextEdit textEdit, string action, string value)
        {
            int userId = GetUserId();
            Status status = new Status();

            switch (action)
            {
                case "Return":
                    status = _modelHelper.TextHelper.SaveEditedText(textEdit);
                    SetAlertCookieGenericSaveStatus(status.success);
                    return Redirect($"/RhymeBinder/ListTextsOnSessionStart?binderId={textEdit.BinderId}");
                case "Save":
                    status = _modelHelper.TextHelper.SaveEditedText(textEdit);
                    SetAlertCookieGenericSaveStatus(status.success);
                    return Redirect($"/RhymeBinder/EditText?textHeaderID={textEdit.TextHeaderId}");
                case "Revision":
                    status = _modelHelper.TextHelper.SaveEditedText(textEdit);
                    if (status.success)
                    {
                        status = _modelHelper.TextHelper.AddRevisionToText(userId, textEdit.TextHeaderId);
                    }
                    if (status.success)
                    {
                        SetAlertCookieGenericSaveStatus(status.success);
                        return Redirect($"/RhymeBinder/EditText?textHeaderID={status.recordId}");
                    }
                    else
                    {
                        SetAlertCookie("Failed to add revision to text", "FAIL"); 
                        return Redirect($"/RhymeBinder/EditText?textHeaderID={status.recordId}");
                    };
                    
                case "Timeout":
                    SetAlertCookie("Editing timed out", "INFO");
                    return Redirect($"/RhymeBinder/ListTextsOnSessionStart?binderId={textEdit.BinderId}");
                case "InsertNewTextInSequence":
                    status = _modelHelper.TextHelper.SaveEditedText(textEdit);
                    if (status.success)
                    {
                        status = _modelHelper.TextHelper.AddNewTextAtPositionInSequence(textEdit, value);
                    }
                    SetAlertCookieGenericSaveStatus(status.success);

                    return Redirect($"/RhymeBinder/EditText?textHeaderID={status.recordId}");
                case "OpenText":
                    status = _modelHelper.TextHelper.SaveEditedText(textEdit);
                    SetAlertCookieGenericSaveStatus(status.success);
                    return Redirect($"/RhymeBinder/EditText?textHeaderID={value}");
                default:
                    SetAlertCookieGenericSaveStatus(status.success);
                    return Redirect($"/RhymeBinder/EditText?textHeaderID={textEdit.TextHeaderId}");
            }

        }
        public IActionResult ListTextsOnSessionStart(int? binderId)
        {   //grabs current user, then default view for that user, and sends viewID to ListTexts
            //  This has become a bit of a catch all route - failures, don't quite know which text to open, etc.
            // Can I redo this to using cookies to lesson queries on database - this may be inefficient

            int userId = GetUserId();
            
            if (binderId == null)
            {
                binderId = 0;
            };

            // If binderId is 0, GetSavedViewIdOnStart will look up user's selected (open) binder
            // Open binders is currently written so it is tied to user id, so user can't mark a binder they don't own as open
            // ...so auth here is not needed
            int savedViewId = _modelHelper.ViewHelper.GetSavedViewIdOnStart(userId, (int)binderId);

            if (savedViewId == -1)
            {
                Status status = new Status()
                {
                    success = false,
                    message = "Failed to retrieve Active saved view"
                };
                return RedirectToAction("ErrorPage", status);
            };
            return Redirect($"/RhymeBinder/ListTexts?viewID={savedViewId}");
        }
        [HttpGet]
        public IActionResult ListTexts(int viewId, int? page, string searchValue)
        {
            // Check for alerts
            TempData["AlertMessage"] = HttpContext.Request.Cookies["AlertMessage"];
            TempData["AlertSeverity"] = HttpContext.Request.Cookies["AlertSeverity"];
            SetAlertCookie("", "");

            //  Authorization check
            int userId = GetUserId();
            if (!_modelHelper.TextHelper.UserAuthorized(userId, viewId, SharedObjectTypeEnum.TextView, SharedObjectActionEnum.READ))
            {
                SetAlertCookie("You do not have permission to access this view", "WARN");
                return RedirectToAction("ListTextsOnSessionStart");
            }

            int currentPage;
            if (page == null) { currentPage = 1; } else { currentPage = (int)page; };

            // TO DO: ---> Figure out multi-user and how texts will be returned across views
            DisplayTextHeadersAndSavedView displayTextHeadersAndSavedView = _modelHelper.TextHelper.GetDisplayTextHeadersAndSavedView(userId, viewId, currentPage);

            if (displayTextHeadersAndSavedView.View.SavedViewId == -1)
            {
                TempData["AlertMessage"] = "Failed to retrieve texts";
                TempData["AlertSeverity"] = "WARN";
            }
            return View(displayTextHeadersAndSavedView);
        }
        [HttpPost]
        public IActionResult ListTexts(DisplayTextHeadersAndSavedView savedView, string action, string value)
        {
            int userId = GetUserId();
            Status status = new Status();
            switch (action)
            {
                case "NewText":
                    return Redirect($"/RhymeBinder/StartNewText?binderId={savedView.View.BinderId}&groupId={value}");

                case "LastView":
                    // Update current saved view with changed form values
                    status = _modelHelper.ViewHelper.UpdateView(savedView);
                    if(!status.success) { SetAlertCookie("Failed to update saved view", "FAIL"); };
                    break;

                case "SaveDefault":
                    // Applies current view grid settings to default view settings
                    status = _modelHelper.ViewHelper.SetDefaultView(userId, savedView.View);
                    SetAlertCookieGenericSaveStatus(status.success);
                    break;

                case "Hide":
                    status = _modelHelper.ViewHelper.UpdateView(savedView);
                    status = _modelHelper.TextHelper.ToggleHideSelectedHeaders(savedView, true);
                    if (!status.success) { SetAlertCookie("Failed to update saved view", "FAIL"); };

                    break;

                case "Restore":
                    status = _modelHelper.ViewHelper.UpdateView(savedView);
                    status = _modelHelper.TextHelper.ToggleHideSelectedHeaders(savedView, false);
                    if (!status.success) { SetAlertCookie("Failed to update saved view", "FAIL"); };

                    break;

                case "GroupAddRemove":
                    status = _modelHelper.ViewHelper.UpdateView(savedView);
                    status = _modelHelper.TextHelper.AddRemoveHeadersFromGroups(savedView);
                    if (!status.success) { SetAlertCookie("Failed to update group associations", "FAIL"); } else
                    {
                        SetAlertCookieGenericSaveStatus(status.success);
                    };
                    return Redirect($"/RhymeBinder/ListTexts?viewID={savedView.View.SavedViewId}");

                case "UpdateGroupSequence":
                    status = _modelHelper.GroupHelper.UpdateGroupSequence(savedView);
                    if (!status.success) { SetAlertCookie("Failed to update group sequence", "FAIL"); } else
                    {
                        SetAlertCookieGenericSaveStatus(status.success);
                    };
                    break;

                // previously used when setting groups individually from dropdowns
                //case "GroupAdd": 
                //    status = _modelHelper.UpdateView(savedView);
                //    status = _modelHelper.AddRemoveHeadersFromGroups(savedView, int.Parse(value), true);
                //    break;

                //case "GroupRemove":
                //    status = _modelHelper.UpdateView(savedView);
                //    status = _modelHelper.AddRemoveHeadersFromGroups(savedView, int.Parse(value), false);
                //    break;

                case "GroupFilter":
                    status = _modelHelper.ViewHelper.SwitchToViewBySet(userId, value);
                    if (!status.success) { SetAlertCookie("Failed to update view", "FAIL"); }
                    break;

                case "Transfer":
                    status = _modelHelper.ViewHelper.UpdateView(savedView);
                    status = _modelHelper.TextHelper.TransferHeadersAcrossBinders(savedView, (int)savedView.DestinationBinder);
                    if (!status.success) { SetAlertCookie("Failed to move texts across binders", "FAIL"); } else
                    {
                        SetAlertCookieGenericSaveStatus(status.success);
                    };
                    break;

                case "ManageGroups":
                    return Redirect($"/RhymeBinder/ListGroups?binderId={savedView.View.BinderId}");

                case "CreateGroup":
                    return Redirect($"/RhymeBinder/CreateGroup?binderID={savedView.View.BinderId}");

                case "ChangePage":
                    return Redirect($"/RhymeBinder/ListTexts?viewID={savedView.View.SavedViewId}&page={value}");

                case "Search":
                    return Redirect($"/RhymeBinder/ListTexts?viewID={savedView.View.SavedViewId}&page=1&searchValue={value}");

                default:
                    return Redirect($"/RhymeBinder/ListTexts?viewID={savedView.View.SavedViewId}");
            }

            // For most switch cases we redirect back to the same list of texts...
            return Redirect($"/RhymeBinder/ListTexts?viewID={status.recordId}&page={savedView.Page}");
        }
        public IActionResult ViewTextsInSequence(int groupId)
        {
            // Check for alerts
            TempData["AlertMessage"] = HttpContext.Request.Cookies["AlertMessage"];
            TempData["AlertSeverity"] = HttpContext.Request.Cookies["AlertSeverity"];
            ClearAlertCookie();

            // Authorization check -- TODO - when multi user set up, figure out how to check auth on each text header, b/c this is really checking the group and that's not  correct
            int userId = GetUserId();
            if(!_modelHelper.TextHelper.UserAuthorized(userId, groupId, SharedObjectTypeEnum.TextGroup, SharedObjectActionEnum.READ))
            {
                SetAlertCookie("You do not have permission to read texts in this group", "WARN");
                return RedirectToAction("ListTextsOnSessionStart");
            }

            DisplaySequencedTexts sequencedTexts = _modelHelper.TextHelper.GetSequenceOfTextHeaderBodyUserRecord(userId, groupId);

            return View(sequencedTexts);
        }
        [HttpGet]
        public IActionResult EditTextsInSequence(int groupId)
        {
            // Check for alerts
            TempData["AlertMessage"] = HttpContext.Request.Cookies["AlertMessage"];
            TempData["AlertSeverity"] = HttpContext.Request.Cookies["AlertSeverity"];
            SetAlertCookie("", "");


            // Authorization check -- TODO - when multi user set up, figure out how to check auth on each text header, b/c this is really checking the group and that's not  correct
            int userId = GetUserId();
            if (!_modelHelper.TextHelper.UserAuthorized(userId, groupId, SharedObjectTypeEnum.TextGroup, SharedObjectActionEnum.EDIT))
            {
                SetAlertCookie("You do not have permission to edit texts in this group", "WARN");
                return RedirectToAction("ListTextsOnSessionStart");
            }

            DisplaySequencedTexts sequencedTexts = _modelHelper.TextHelper.GetSequenceOfTextHeaderBodyUserRecord(userId, groupId);
            sequencedTexts.ActiveElementId = GetCookieValue($"Sequence_{sequencedTexts.GroupId}_ActiveElement");
            sequencedTexts.CursorPosition = GetCookieValue($"sequence_{sequencedTexts.GroupId}_CursorPosition");

            if (sequencedTexts.BinderReadOnly)
            {
                SetAlertCookie("Text Locked from Editing", "WARN");
                return Redirect($"/RhymeBinder/ViewTextsInSequence?groupId={groupId}");
            }

            return View(sequencedTexts);
        }
        [HttpPost]
        public IActionResult EditTextsInSequence(DisplaySequencedTexts editedTexts)
        {
            Status status = _modelHelper.TextHelper.SaveEditedTextsInSequence(editedTexts);
            if (!status.success)
            {
                SetAlertCookie("Failed to save changes to texts", "FAIL");
            }
            SetCookieValue($"Sequence_{editedTexts.GroupId}_ActiveElement", editedTexts.ActiveElementId);
            SetCookieValue($"Sequence_{editedTexts.GroupId}_CursorPosition", editedTexts.CursorPosition);

            return Redirect($"/RhymeBinder/EditTextsInSequence?groupId={editedTexts.GroupId}");
        }

        #endregion

        //-------GROUP:
        #region GroupMethods
        public IActionResult ListGroups(int binderId)
        {
            // Check for alerts
            TempData["AlertMessage"] = HttpContext.Request.Cookies["AlertMessage"];
            TempData["AlertSeverity"] = HttpContext.Request.Cookies["AlertSeverity"];
            ClearAlertCookie();

            // Authorization check
            int userId = GetUserId();
            if(!_modelHelper.BinderHelper.UserAuthorized(userId, binderId, SharedObjectTypeEnum.Binder, SharedObjectActionEnum.READ))
            {
                SetAlertCookie("You do not have permission to view this binder", "WARN");
                return RedirectToAction("ListTextsOnSessionStart");
            }

            List<DisplayTextGroup> displayTextGroups = _modelHelper.TextHelper.GetDisplayTextGroups(userId, binderId);
            if (displayTextGroups.Count == 0) { displayTextGroups.Add(new DisplayTextGroup()); };

            if (displayTextGroups[0].TextGroupId == -1)
            {
                SetAlertCookie("Failed to retrieve list of groups", "FAIL");
                return Redirect($"/RhymeBinder/ListTextsOnSessionStart?binderId={binderId}");
            }
            return View(displayTextGroups);
        }
        [HttpGet]
        public IActionResult EditGroup(int groupID)
        {
            // Check for alerts
            TempData["AlertMessage"] = HttpContext.Request.Cookies["AlertMessage"];
            TempData["AlertSeverity"] = HttpContext.Request.Cookies["AlertSeverity"];
            ClearAlertCookie();

            //  Authorization check
            int userId = GetUserId();
            if(!_modelHelper.GroupHelper.UserAuthorized(userId, groupID, SharedObjectTypeEnum.TextGroup, SharedObjectActionEnum.EDIT))
            {
                SetAlertCookie("You do not have permission to edit this group", "WARN");
                return RedirectToAction("ListTextsOnSessionStart");
            }

            TextGroup groupToEdit = _modelHelper.TextHelper.GetTextGroup(groupID);
            if (groupToEdit.TextGroupId == -1)
            {
                SetAlertCookie("Failed to retrieve group", "FAIL");
                return RedirectToAction("ListTextsOnSessionStart");
            }
            return View(groupToEdit);
        }

        [HttpPost]
        public IActionResult EditGroup(TextGroup editedGroup, string action, string verifyDeleteGroup, string verifyDeleteAll, string verifyClear)
        {
            Status status = new Status() { success = true };

            switch (action)
            {
                case "Submit Changes":
                    status = _modelHelper.GroupHelper.UpdateGroup(editedGroup);
                    break;
                case "Clear":
                    if (verifyClear != null)
                    {
                        status = _modelHelper.GroupHelper.ClearTextsFromGroup(editedGroup.TextGroupId);
                    }
                    break;
                case "Delete Group":
                    if (verifyDeleteGroup != null)
                    {
                        status = _modelHelper.GroupHelper.DeleteGroup(editedGroup);
                    }
                    break;
            }

            SetAlertCookieGenericSaveStatus(status.success);
            return Redirect($"/Rhymebinder/ListGroups?binderId={editedGroup.BinderId}");
        }

        [HttpGet]
        public IActionResult CreateGroup(int binderID)
        {
            // Check for alerts
            TempData["AlertMessage"] = HttpContext.Request.Cookies["AlertMessage"];
            TempData["AlertSeverity"] = HttpContext.Request.Cookies["AlertSeverity"];
            ClearAlertCookie();

            //  Authorization check
            int userId = GetUserId();
            if (!_modelHelper.GroupHelper.UserAuthorized(userId, binderID, SharedObjectTypeEnum.Binder, SharedObjectActionEnum.EDIT))
            {
                SetAlertCookie("You do not have permission to make edits in this binder", "WARN");
                return RedirectToAction("ListTextsOnSessionStart");
            }

            return View(binderID);
        }
        [HttpPost]
        public IActionResult CreateGroup(TextGroup newGroup)
        {
            int userId = GetUserId();
            Status status = new Status();

            status = _modelHelper.GroupHelper.CreateNewTextGroup(userId, newGroup);
            SetAlertCookieGenericSaveStatus(status.success);

            return Redirect($"/Rhymebinder/ListGroups?binderId={newGroup.BinderId}");
        } 
        #endregion

        //-------BINDER METHODS:
        #region BinderMethods
        public IActionResult CreateBinder()
        {
            int userId = GetUserId();
            Status status = new Status();

            status = _modelHelper.BinderHelper.CreateNewBinder(userId);

            if (!status.success)
            {
                SetAlertCookie("Failed to create new binder", "FAIL");
                return Redirect($"/RhymeBinder/ListTextsOnSessionStart");
            }
            else
            {
                SetAlertCookieGenericSaveStatus(status.success);
            }

            return Redirect($"/RhymeBinder/EditBinder?binderID={status.recordId}");
        }
        [HttpGet]
        public IActionResult ListBinders()
        {
            // Check for alerts
            TempData["AlertMessage"] = HttpContext.Request.Cookies["AlertMessage"];
            TempData["AlertSeverity"] = HttpContext.Request.Cookies["AlertSeverity"];
            ClearAlertCookie();

            int userId = GetUserId();
            List<DisplayBinder> binders = _modelHelper.BinderHelper.GetDisplayBinders(userId);
            if (binders[0].BinderId == -1)
            {
                SetAlertCookie("Failed to retrieve binders", "FAIL");
                return Redirect($"/RhymeBinder/ListTextsOnSessionStart");
            }
            return View(binders);
        }
        [HttpGet]
        public IActionResult EditBinder(int binderID)
        {
            // Check for alerts
            TempData["AlertMessage"] = HttpContext.Request.Cookies["AlertMessage"];
            TempData["AlertSeverity"] = HttpContext.Request.Cookies["AlertSeverity"];
            ClearAlertCookie();

            //  Authorization check
            int userId = GetUserId();
            if (!_modelHelper.GroupHelper.UserAuthorized(userId, binderID, SharedObjectTypeEnum.Binder, SharedObjectActionEnum.EDIT))
            {
                SetAlertCookie("You do not have permission to make edits in this binder", "WARN");
                return RedirectToAction("ListTextsOnSessionStart");
            }

            DisplayBinder binder = _modelHelper.BinderHelper.GetDisplayBinder(userId, binderID);
            if (binder.BinderId == -1)
            {
                SetAlertCookie("Failed to retrieve binder", "FAIL");
                return Redirect($"/RhymeBinder/ListTextsOnSessionStart");
            }
            return View(binder);
        }
        [HttpPost]
        public IActionResult EditBinder(DisplayBinder editedBinder, string action, string verifyClear, string verifyDelete, string verifyDeleteAll, string verifyDuplicate)
        {
            int userId = GetUserId();
            Status status = new Status() { success = true };

            switch (action)
            {
                case "Submit Changes":
                    status = _modelHelper.BinderHelper.UpdateBinder(userId, editedBinder);
                    break;
                case "Duplicate":
                    status = _modelHelper.BinderHelper.DuplicateBinder(userId, editedBinder.BinderId);
                    break;
                case "Clear":
                    if (verifyClear != null)
                    {
                        status = _modelHelper.BinderHelper.ClearBinder(userId, editedBinder.BinderId);
                    }
                    break;
                case "Delete":
                    if (verifyDelete != null)
                    {
                        status = _modelHelper.BinderHelper.DeleteBinder(userId, editedBinder.BinderId);
                    }
                    break;
                case "Delete All":
                    if (verifyDeleteAll != null)
                    {
                        status = _modelHelper.BinderHelper.DeleteBinderAndContents(userId, editedBinder.BinderId);
                    }
                    break;
                default:
                    return RedirectToAction("ListBinders");
            }

            SetAlertCookieGenericSaveStatus(status.success);
            return RedirectToAction("ListBinders");
         }
        public IActionResult OpenBinder(int binderId)
        {
            // "Opening" binder sets the selected flag in the binders table to true
            // We only want to do that for users who own the binder, natrually
            // ...and it only affects users who own that binder
            // ...therefore OpenBinder method checks for ownership, no auth needed here...
            int userId = GetUserId();
            Status status = _modelHelper.BinderHelper.OpenBinder(userId, binderId);

            if (!status.success) { SetAlertCookie("Failed to open binder", "FAIL"); };

            return RedirectToAction("ListTextsOnSessionStart");
        }
        [HttpPost]
        public void SaveShelfChanges([FromBody] List<ShelfUpdateModel> shelfUpdates)
        {
            int userId = GetUserId();
            Status status = _modelHelper.BinderHelper.UpdateShelf(userId, shelfUpdates);
            SetAlertCookieGenericSaveStatus(status.success);
            return;
        }

        #endregion

        //-------MISC:
        #region Misc
        public IActionResult Index()
        {
            //if (!HttpContext.Request.Cookies.ContainsKey("AlertMessage"))
            //{
            //    HttpContext.Response.Cookies.Append("AlertMessage", "", new Microsoft.AspNetCore.Http.CookieOptions { Path = "/" });
            //};
            //if (!HttpContext.Request.Cookies.ContainsKey("AlertSeverity"))
            //{
            //    HttpContext.Response.Cookies.Append("AlertSeverity", "", new Microsoft.AspNetCore.Http.CookieOptions { Path = "/" });
            //};


            int userId = GetUserId();
            //check that a SimpleUser record has been created for this user; if not, create one;
            if (userId == -1)
            {
                return RedirectToAction("SetupNewUser");
            };
            return RedirectToAction("ListBinders");
        }
        public IActionResult Manage()
        {
            int userId = GetUserId();
            return View(userId);
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult ErrorPage(Status status)
        {
            status.userId = GetUserId();
            return View(status);
        }
        [HttpPost]
        public JsonResult SaveUserFontSize(int userId, int fontSize)
        {
            try
            {
                _modelHelper.UserHelper.SaveUserFontSize(userId, fontSize);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public int GetUserId()
        {
            string aspUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int userId = _modelHelper.UserHelper.GetCurrentSimpleUserID(aspUserId);
            return userId;
        }
        public void ClearAlertCookie()
        {
            HttpContext.Response.Cookies.Append("AlertMessage", "");
            HttpContext.Response.Cookies.Append("AlertSeverity", "");
            return;
        }
        public void SetAlertCookie(string alertMessage, string alertSeverity)
        {
            HttpContext.Response.Cookies.Append("AlertMessage", alertMessage);
            HttpContext.Response.Cookies.Append("AlertSeverity", alertSeverity);
            return;
        }
        public void SetAlertCookieGenericSaveStatus(bool success)
        {
            if (success)
            {
                HttpContext.Response.Cookies.Append("AlertMessage", "Changes saved!");
                HttpContext.Response.Cookies.Append("AlertSeverity", "SUCCESS");
            }
            else
            {
                HttpContext.Response.Cookies.Append("AlertMessage", "Failed to save changes!");
                HttpContext.Response.Cookies.Append("AlertSeverity", "FAIL");
            }
            return;
        }
        public void SetCookieValue(string key, string value)
        {
            if(value == null) { value = "";  };
            HttpContext.Response.Cookies.Append(key, value);
            return;
        }
        public string GetCookieValue(string key)
        {
            return HttpContext.Request.Cookies[key];
        }
        #endregion
    }
}
