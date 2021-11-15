using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RhymeBinder.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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



            return RedirectToAction("EditText");
        }

        [HttpGet]
        public IActionResult EditText()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EditText(Text text)
        {



            return RedirectToAction("Index");
        }
    }
}
