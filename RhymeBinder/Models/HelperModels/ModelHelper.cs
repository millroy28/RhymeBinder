using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.Build.Framework;
using Microsoft.Extensions.Logging;
using NuGet.Protocol;
using RhymeBinder.Controllers;
using RhymeBinder.Models.DBModels;
using RhymeBinder.Models.DTOModels;
using RhymeBinder.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;

namespace RhymeBinder.Models.HelperModels
{
    public class ModelHelper
    {
        public BinderHelper BinderHelper { get; set; }
        public GroupHelper GroupHelper { get; set; }
        public TextHelper TextHelper { get; set; }
        public UserHelper UserHelper { get; set; }
        public ViewHelper ViewHelper { get; set; }

        // Constructor
        public ModelHelper(RhymeBinderContext context, ILogger<BaseHelper> logger)
        {
            BinderHelper = new BinderHelper(context, logger);
            GroupHelper = new GroupHelper(context, logger);
            TextHelper = new TextHelper(context, logger);
            UserHelper = new UserHelper(context, logger);
            ViewHelper = new ViewHelper(context, logger);
        }
    }
}