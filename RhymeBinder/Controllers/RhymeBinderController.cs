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

            //creating a new TextGroup
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


            //creating a new TextHeader entry (DEFAULTS of a new TextHeader set here):
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

            //get header id to pass into the editing portion
            int newTextHeaderID = newTextHeader.TextHeaderId;

            return AddText(newTextHeaderID);
        }

        public IActionResult AddText(int textHeaderID)
        {

            Text newText = new Text();

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

            //write the new textID into the TextHeader

            TextHeader thisTextHeader = _context.TextHeaders.Find(textHeaderID);

            if (ModelState.IsValid)
            {
                thisTextHeader.TextId = newText.TextId;
                _context.Entry(thisTextHeader).State = Microsoft.EntityFrameworkCore.EntityState.Modified;  //remember to copy paste this honkin thing
                _context.Update(thisTextHeader);
                _context.SaveChanges();
            }
            else
            {
                return View();  //!!insert error handlin?
            }

            return Redirect($"/RhymeBinder/EditText?textId={newText.TextId}");
        }

        [HttpGet]
        public IActionResult EditText(int textId)
        {
            Text thisText = _context.Texts.Find(textId);
            return View(thisText);
        }

        [HttpPost]
        public IActionResult EditText(Text editedText)
        {
            Text thisText = _context.Texts.Find(editedText.TextId);

            if (ModelState.IsValid)
            {
                thisText.TextBody = editedText.TextBody;
                thisText.Created = editedText.Created;
                _context.Entry(thisText).State = Microsoft.EntityFrameworkCore.EntityState.Modified;  
                _context.Update(thisText);
                _context.SaveChanges();
            }
            else
            {
                return View();  //!!insert error handlin?
            }


            return RedirectToAction("Index");
        }
    }
}
