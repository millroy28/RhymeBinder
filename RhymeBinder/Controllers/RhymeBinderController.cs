﻿using Microsoft.AspNetCore.Authorization;
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
        public RhymeBinder.Models.ModelHelper _modelHelper;
        public RhymeBinderController(ModelHelper modelHelper)
        {
            _modelHelper = modelHelper;
        }

        public IActionResult Index()
        {
            int userId = GetUserId();
            //check that a SimpleUser record has been created for this user; if not, create one;
            if (userId == -1)
            {
                return RedirectToAction("SetupNewUser");
            };
            return RedirectToAction("ListTextsOnSessionStart");
        }
        //-------USER:
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
            Status status = _modelHelper.SetupNewUser(newUser);
            if (status.success)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return Redirect(Url.Action("ErrorPage", status));                
            }
        }
        //-------TEXT:
        public IActionResult StartNewText()
        {
            int userId = GetUserId();
            Status status = _modelHelper.StartNewText(userId);

            if (status.success)
            {
                return Redirect($"/RhymeBinder/EditText?textHeaderID={status.recordId}");
            }
            else
            {
                return Redirect(Url.Action("ErrorPage", status));
            }
        }
        [HttpGet]
        public IActionResult EditText(int textHeaderID)
        {
            int userId = GetUserId();
           
            TextHeaderBodyUserRecord thisTextHeaderBodyUserRecord = _modelHelper.GetTextHeaderBodyUserRecord(userId, textHeaderID);

            return View(thisTextHeaderBodyUserRecord);
        }
        [HttpPost]
        public IActionResult EditText(TextHeaderBodyUserRecord editedTextHeaderBodyUserRecord, string action)
        {
            int userId = GetUserId();
            Status status = _modelHelper.SaveEditedText(editedTextHeaderBodyUserRecord);
            if (!status.success)
            {
                return Redirect(Url.Action("ErrorPage", status));
            }
            //Where do we go from here?
            switch (action)
            {
                case "Return":
                    return RedirectToAction("ListTextsOnSessionStart");
                case "Save":
                    return Redirect($"/RhymeBinder/EditText?textHeaderID={editedTextHeaderBodyUserRecord.TextHeader.TextHeaderId}");
                case "Revision":
                    status = _modelHelper.AddRevisionToText(userId, editedTextHeaderBodyUserRecord.TextHeader.TextHeaderId);
                    if (status.success)
                    {
                        return Redirect($"/RhymeBinder/EditText?textHeaderID={status.recordId}");
                    }
                    else
                    {
                        return Redirect(Url.Action("ErrorPage", status));
                    }
                default:
                    return Redirect($"/RhymeBinder/EditText?textHeaderID={editedTextHeaderBodyUserRecord.TextHeader.TextHeaderId}");
            }

        }
        public IActionResult ListTextsOnSessionStart()
        {   //grabs current user, then default view for that user, and sends viewID to ListTexts
            int userId = GetUserId();
            int savedViewId = _modelHelper.GetSavedViewIdOnStart(userId);

            if (savedViewId == -1)
            {
                Status status = new Status()
                {
                    success = false,
                    message = "Failed to retrieve Active saved view"
                };
                return Redirect(Url.Action("ErrorPage", status));
            };
            return Redirect($"/RhymeBinder/ListTexts?viewID={savedViewId}");
        }
        [HttpGet]
        public IActionResult ListTexts(int viewId)
        {
            int userId = GetUserId();
            DisplayTextHeadersAndSavedView displayTextHeadersAndSavedView = _modelHelper.GetDisplayTextHeadersAndSavedView(userId, viewId);

            if (displayTextHeadersAndSavedView.View.SavedViewId != -1)
            {
                return View(displayTextHeadersAndSavedView);
            }
            else
            {
                Status status = new Status()
                {
                    success = false,
                    message = "Failed to get list of texts for view",
                    recordId = viewId
                };
                return Redirect(Url.Action("ErrorPage", status));
            }
        }
        [HttpPost]
        public IActionResult ListTexts(DisplayTextHeadersAndSavedView savedView, string action, int groupID)
        {
            int userId = GetUserId();
            Status status = new Status();
            switch (action)
            {
                case "LastView":
                    // Update current saved view with changed form values
                    status = _modelHelper.UpdateView(savedView);
                    break;

                case "SaveDefault":
                    // Applies current view grid settings to default view settings
                    status = _modelHelper.SetDefaultView(userId, savedView.View);
                    break;     

                case "Hide":
                    status = _modelHelper.ToggleHideSelectedHeaders(savedView, true);
                    break;

                case "Restore":
                    status = _modelHelper.ToggleHideSelectedHeaders(savedView, false);
                    break;

                case "GroupAdd":
                    status = _modelHelper.AddRemoveHeadersFromGroups(savedView, groupID, true);
                    break;

                case "GroupRemove":
                    status = _modelHelper.AddRemoveHeadersFromGroups(savedView, groupID, false);
                    break;

                case "GroupFilter":
                    status = _modelHelper.SwitchToView(userId, savedView.View.SetValue);
                    break;

                case "Transfer":
                    status = _modelHelper.TransferHeadersAcrossBinders(savedView, groupID);
                    break;

                case "ManageGroups":
                    return RedirectToAction("ListGroups");

                case "CreateGroup":
                    return Redirect($"/RhymeBinder/CreateGroup?binderID={savedView.View.BinderId}");

                default:
                    return Redirect($"/RhymeBinder/ListTexts?viewID={savedView.View.SavedViewId}");
            }

            // For most switch cases we redirect back to the same list of texts...
            if (status.success)
            {
                return Redirect($"/RhymeBinder/ListTexts?viewID={status.recordId}");
            }
            else
            {
                return Redirect(Url.Action("ErrorPage", status));
            }
        }

        //-------GROUP:
        public IActionResult ListGroups()
        {
            int userId = GetUserId();
            List<DisplayTextGroup> displayTextGroups = _modelHelper.GetDisplayTextGroups(userId);

            if (displayTextGroups[0].TextGroupId == -1)
            {
                Status status = new Status()
                {
                    message = "Failed to retrieve Text Groups for display"
                };
                return Redirect(Url.Action("ErrorPage", status));
            }
            return View(displayTextGroups);
        }
        [HttpGet]
        public IActionResult EditGroup(int groupID)
        {
            TextGroup groupToEdit = _modelHelper.GetTextGroup(groupID);
            if (groupToEdit.TextGroupId == -1)
            {
                Status status = new Status()
                {
                    message = $"Failed to retrieve Text Group Id {groupID}"
                };
                return Redirect(Url.Action("ErrorPage", status));
            }
            return View(groupToEdit);
        }

        [HttpPost]
        public IActionResult EditGroup(TextGroup editedGroup, string action, string verifyDeleteGroup, string verifyDeleteAll, string verifyClear)
        {
            Status status = new Status();

            switch (action)
            {
                case "Submit Changes":
                    status = _modelHelper.UpdateGroup(editedGroup);
                    break;
                case "Clear":
                    if (verifyClear != null)
                    {
                        status = _modelHelper.ClearTextsFromGroup(editedGroup.TextGroupId);
                    }
                    break;
                case "Delete Group":
                    if (verifyDeleteGroup != null)
                    {
                        status = _modelHelper.DeleteGroup(editedGroup);
                    }
                    break;
            }

            if (!status.success) 
            {
                return Redirect(Url.Action("ErrorPage", status));
            }
            else 
            { 
                return RedirectToAction("ListGroups");
            }
        }

        [HttpGet]
        public IActionResult CreateGroup(int binderID)
        {
            return View(binderID);
        }
        [HttpPost] 
        public IActionResult CreateGroup(TextGroup newGroup)
        {
            int userId = GetUserId();
            Status status = new Status();

            status = _modelHelper.CreateGroup(userId, newGroup);

            if (!status.success)
            {
                return Redirect(Url.Action("ErrorPage", status));
            }
            else
            {
                return RedirectToAction("ListGroups");
            }
        }

        public IActionResult Manage()
        {
            int userId = GetUserId();
            return View(userId);
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
            int userId = GetUserId();
            Status status = new Status();

            status = _modelHelper.CreateNewBinder(userId, newBinder);

            if (status.success)
            {
                int viewId = _modelHelper.GetSavedViewIdOnStart(userId);
                if(viewId != -1)
                {
                    return Redirect($"/RhymeBinder/ListTexts?viewID={viewId}");
                }
            }
            return Redirect(Url.Action("ErrorPage", status));
        }
       
        public IActionResult ListBinders()
        {
            int userId = GetUserId();
            List<DisplayBinder> binders = _modelHelper.GetDisplayBinders(userId);
            if (binders[0].BinderId == -1)
            {
                Status status = new Status()
                {
                    message = "Failed to retrieve Display Binders"
                };
                return Redirect(Url.Action("ErrorPage", status));
            }
            return View(binders);
        }
        [HttpGet]
        public IActionResult EditBinder(int binderID)
        {
            DisplayBinder binder = _modelHelper.GetDisplayBinder(binderID);
            if (binder.BinderId == -1)
            {
                Status status = new Status()
                {
                    message = $"Failed to retrieve Display Binder {binderID}"
                };
                return Redirect(Url.Action("ErrorPage", status));
            }
            return View(binder);
        }
        //  ====================progress marker
        [HttpPost]
        public IActionResult EditBinder(DisplayBinder editedBinder, string action, string verifyClear, string verifyDelete, string verifyDeleteAll)
        {
            int userId = GetUserId();
            Status status = new Status();

            switch (action)
            {
                case "Submit Changes":
                    status = _modelHelper.UpdateBinder(editedBinder);
                    break;
                case "Clear":
                    if(verifyClear != null)
                    {
                        status = _modelHelper.ClearBinder(userId, editedBinder.BinderId);
                    }
                    break;
                case "Delete":
                    if(verifyDelete != null)
                    {
                        status = _modelHelper.DeleteBinder(userId, editedBinder.BinderId);
                    }
                    break;
                case "DeleteAll":

                    if(verifyDeleteAll != null)
                    {
                        status = _modelHelper.DeleteBinderAndContents(userId, editedBinder.BinderId);
                    }
                    break;
                default:
                    return RedirectToAction("ListBinders"); 
            }

            if (status.success)
            {
                return RedirectToAction("ListBinders");
            }
            else
            {
                return ErrorPage(status);
            }
        }
        public IActionResult OpenBinder(int binderId)
        {

            int userId = GetUserId();
            Status status = _modelHelper.OpenBinder(userId, binderId);
            if (!status.success)
            {
                return Redirect(Url.Action("ErrorPage", status));
            }
            return RedirectToAction("ListTextsOnSessionStart");
        }
        [HttpPost]
        public IActionResult ErrorPage(Status status)
        {
            string msg = status.message;

            return View(msg);
        }

        public int GetUserId()
        {
            string aspUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int userId = _modelHelper.GetCurrentSimpleUserID(aspUserId);
            return userId;
        }

    }
}
