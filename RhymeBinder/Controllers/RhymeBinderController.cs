using Microsoft.AspNetCore.Authorization;
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

        private readonly RhymeBinderContext _context;

        public RhymeBinderController(RhymeBinderContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult StartNewTextGroup()
        {
            return View();
        }

        [HttpPost]
        public IActionResult StartNewTextGroup(string title)
        {
            string userID = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            //start by creating a new TextGroup
            TextGroup newTextGroup = new TextGroup();
           
            newTextGroup.GroupTitle = title;
            newTextGroup.OwnerId = userID;

            if (ModelState.IsValid)
            {
                _context.TextGroups.Add(newTextGroup);
                _context.SaveChanges();
            }
            else
            {
                return View();  //!!insert error handlin?
            }
            int newTextGroupID = newTextGroup.TextGroupId;


            //create a new TextHeader entry (DEFAULTS of a new TextHeader set here):
            TextHeader newTextHeader = new TextHeader();
            
            newTextHeader.TextGroupId = newTextGroupID;
            newTextHeader.Title = title;
            newTextHeader.Created = DateTime.Now;
            newTextHeader.CreatedBy = userID;
            newTextHeader.VisionNumber = 1;
            newTextHeader.Deleted = false;
            newTextHeader.Locked = false;
            newTextHeader.Top = true;

            if (ModelState.IsValid)
            {
                _context.TextHeaders.Add(newTextHeader);
                _context.SaveChanges();
            }
            else
            {
                return View();  //!!insert error handlin?
            }

            return Redirect($"/RhymeBinder/EditText?textHeaderID={newTextHeader.TextHeaderId}");
        }


        [HttpGet]
        public IActionResult EditText(int textHeaderID)
        {
            //Build up the TextHeaderBodyUserRecord to pass into view
            TextHeader thisTextHeader = _context.TextHeaders.Find(textHeaderID);
            Text thisText = new Text();

            if (thisTextHeader.TextId != null)
            {
                thisText = _context.Texts.Find(thisTextHeader.TextId);
            }
                        
            string userID = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            AspNetUser aspNetUser = _context.AspNetUsers.Find(userID);
            UserSimplified thisUser = new UserSimplified();
            thisUser.UserID = aspNetUser.Id;
            thisUser.UserName = aspNetUser.UserName;
            TextHeaderBodyUserRecord thisTextHeaderBodyUserRecord = new TextHeaderBodyUserRecord()
            {
                TextHeader = thisTextHeader,
                Text = thisText,
                User = thisUser
            };

            return View(thisTextHeaderBodyUserRecord);
        }

        [HttpPost]
        public IActionResult EditText(TextHeaderBodyUserRecord editedTextHeaderBodyUserRecord)
        {
            //Create a new record in the Text table 
            Text newText = new Text();

            newText.TextBody = editedTextHeaderBodyUserRecord.Text.TextBody;
            newText.Created = DateTime.Now;
            

            if (ModelState.IsValid)
            {
                _context.Texts.Add(newText);
                _context.SaveChanges();
            }
            else
            {
                return View();  //!!insert error handlin?
            }

            //Create a new log entry for the TextRecord table
            TextRecord newTextRecord = new TextRecord();
            
            newTextRecord.TextHeaderId = editedTextHeaderBodyUserRecord.TextHeader.TextHeaderId;
            newTextRecord.TextId = newText.TextId;
            newTextRecord.UserId = editedTextHeaderBodyUserRecord.User.UserID;
            newTextRecord.Recorded = newText.Created;

            if (ModelState.IsValid)
            {
                _context.TextRecords.Add(newTextRecord);
                _context.SaveChanges();
            }
            else
            {
                return View();  //!!insert error handlin?
            }

            //Update the TextHeader with the new TextID, etc
            TextHeader updatedTextHeader = _context.TextHeaders.Find(editedTextHeaderBodyUserRecord.TextHeader.TextHeaderId);

            updatedTextHeader.LastModified = newText.Created;
            updatedTextHeader.LastModifiedBy = editedTextHeaderBodyUserRecord.User.UserID;
            updatedTextHeader.LastRead = newText.Created;
            updatedTextHeader.LastReadBy = editedTextHeaderBodyUserRecord.User.UserID;
            updatedTextHeader.TextId = newTextRecord.TextId;

            if (ModelState.IsValid)
            {
                _context.Entry(updatedTextHeader).State = Microsoft.EntityFrameworkCore.EntityState.Modified;  //remember to copy paste this honkin thing
                _context.Update(updatedTextHeader);
                _context.SaveChanges();
            }
            else
            {
                return View();  //!!insert error handlin?
            }

            return RedirectToAction("Index");
        }

        public IActionResult ListTexts()
        {
            string userID = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<TextHeader> texts = new List<TextHeader>();

            texts = _context.TextHeaders.OrderByDescending(x => x.LastModified).Where(x => x.CreatedBy == userID &&
                                                    x.Top == true &&
                                                    x.Deleted == false).ToList();


            return View(texts);
        }
    }
}
