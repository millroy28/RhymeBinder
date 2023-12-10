using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RhymeBinder.Models;
using System.Collections.Generic;
using System.Security.Claims;


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
                return RedirectToAction("ErrorPage", status);
            }
        }

        [HttpGet]
        public IActionResult EditUser()
        {
            int userId = GetUserId();
            SimpleUser user = _modelHelper.GetCurrentSimpleUser(userId);
          

            if (user.UserId == -1)
            {
                Status status = new Status()
                {
                    success = false,
                    message = $"Failed to retrieve user {userId} to edit"
                };

                return RedirectToAction("ErrorPage", status);
            }
            return View(user);
        }

        [HttpPost]
        public IActionResult EditUser(SimpleUser editedUser)
        {
        
            Status status = _modelHelper.UpdateSimpleUser(editedUser);

            if (!status.success)
            {
                return RedirectToAction("ErrorPage", status);
            }
            else
            {
                return RedirectToAction("ListTextsOnSessionStart");
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
                return RedirectToAction("ErrorPage", status);
            }
        }
        public IActionResult ViewText(int textHeaderID)
        {
            int userId = GetUserId();

            TextHeaderBodyUserRecord thisTextHeaderBodyUserRecord = _modelHelper.GetTextHeaderBodyUserRecord(userId, textHeaderID);

            return View(thisTextHeaderBodyUserRecord);
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
                return RedirectToAction("ErrorPage", status);
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
                        return RedirectToAction("ErrorPage", status);
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
                return RedirectToAction("ErrorPage", status);
            };
            return Redirect($"/RhymeBinder/ListTexts?viewID={savedViewId}");
        }
        [HttpGet]
        public IActionResult ListTexts(int viewId, int? page, string searchValue)
        {
            int userId = GetUserId();
            int currentPage;
            if (page == null) { currentPage = 1; } else { currentPage = (int)page; };

            DisplayTextHeadersAndSavedView displayTextHeadersAndSavedView = _modelHelper.GetDisplayTextHeadersAndSavedView(userId, viewId, currentPage, searchValue);

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
                return RedirectToAction("ErrorPage", status);
            }
        }
        [HttpPost]
        public IActionResult ListTexts(DisplayTextHeadersAndSavedView savedView, string action, string value)
        {
            int userId = GetUserId();
            Status status = new Status();
            switch (action)
            {
                case "NewText":
                    return RedirectToAction("StartNewText");

                case "LastView":
                    // Update current saved view with changed form values
                    status = _modelHelper.UpdateView(savedView);
                    break;

                case "SaveDefault":
                    // Applies current view grid settings to default view settings
                    status = _modelHelper.SetDefaultView(userId, savedView.View);
                    break;     

                case "Hide":
                    status = _modelHelper.UpdateView(savedView);
                    status = _modelHelper.ToggleHideSelectedHeaders(savedView, true);
                    break;

                case "Restore":
                    status = _modelHelper.UpdateView(savedView);
                    status = _modelHelper.ToggleHideSelectedHeaders(savedView, false);
                    break;

                case "GroupAdd":
                    status = _modelHelper.UpdateView(savedView);
                    status = _modelHelper.AddRemoveHeadersFromGroups(savedView, int.Parse(value), true);
                    break;

                case "GroupRemove":
                    status = _modelHelper.UpdateView(savedView);
                    status = _modelHelper.AddRemoveHeadersFromGroups(savedView, int.Parse(value), false);
                    break;

                case "GroupFilter":
                    status = _modelHelper.SwitchToViewBySet(userId, value);
                    break;

                case "Transfer":
                    status = _modelHelper.UpdateView(savedView);
                    status = _modelHelper.TransferHeadersAcrossBinders(savedView, int.Parse(value));
                    break;

                case "ManageGroups":
                    return RedirectToAction("ListGroups");

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
            if (status.success)
            {
                return Redirect($"/RhymeBinder/ListTexts?viewID={status.recordId}&page={savedView.Page}");
            }
            else
            {
                return RedirectToAction("ErrorPage", status);
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
                return RedirectToAction("ErrorPage", status);
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
                return RedirectToAction("ErrorPage", status);
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
                 return RedirectToAction("ErrorPage", status);
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

            status = _modelHelper.CreateNewTextGroup(userId, newGroup);

            if (!status.success)
            {
                return RedirectToAction("ErrorPage", status);
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
        public IActionResult CreateBinder()
        {
            int userId = GetUserId();
            Status status = new Status();

            status = _modelHelper.CreateNewBinder(userId);

            if (!status.success)
            {
                return RedirectToAction("ErrorPage", status);
            }

            return Redirect($"/RhymeBinder/EditBinder?binderID={status.recordId}");
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
                return RedirectToAction("ErrorPage", status);
            }
            return View(binders);
        }
        [HttpGet]
        public IActionResult EditBinder(int binderID)
        {
            int userId = GetUserId();
            DisplayBinder binder = _modelHelper.GetDisplayBinder(userId, binderID);
            if (binder.BinderId == -1)
            {
                Status status = new Status()
                {
                    message = $"Failed to retrieve Display Binder {binderID}"
                };
                return RedirectToAction("ErrorPage", status);
            }
            return View(binder);
        }
        [HttpPost]
        public IActionResult EditBinder(DisplayBinder editedBinder, string action, string verifyClear, string verifyDelete, string verifyDeleteAll)
        {
            int userId = GetUserId();
            Status status = new Status() { success = true};

            switch (action)
            {
                case "Submit Changes":
                    status = _modelHelper.UpdateBinder(userId, editedBinder);
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
                return RedirectToAction("ErrorPage", status);
            }
            return RedirectToAction("ListTextsOnSessionStart");
        }

        public IActionResult ErrorPage(Status status)
        {
            status.userId = GetUserId();
            return View(status);
        }



        public int GetUserId()
        {
            string aspUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            int userId = _modelHelper.GetCurrentSimpleUserID(aspUserId);
            return userId;
        }

    }
}
