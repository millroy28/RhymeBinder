#pragma checksum "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8ac8bc4ced526b6d339b29a0012e0f10dabf203b"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_RhymeBinder_Index), @"mvc.1.0.view", @"/Views/RhymeBinder/Index.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\_ViewImports.cshtml"
using RhymeBinder;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\_ViewImports.cshtml"
using RhymeBinder.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8ac8bc4ced526b6d339b29a0012e0f10dabf203b", @"/Views/RhymeBinder/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"09a45f068354f87046b22aebe54e24a87c312eff", @"/Views/_ViewImports.cshtml")]
    public class Views_RhymeBinder_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<SimpleUser>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\Index.cshtml"
  
    ViewData["Title"] = "Index";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>RhymeBinder Index Page</h1>\r\n\r\n<h2>Hello, ");
#nullable restore
#line 11 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\Index.cshtml"
      Write(Model.UserName);

#line default
#line hidden
#nullable disable
            WriteLiteral("!</h2>\r\n<h3>Links for what will be a menu at some point:</h3>\r\n<ul>\r\n    <li>Directory</li>\r\n    <li>Editor</li>\r\n    <li><a href=\"/RhymeBinder/StartNewTextGroup\">Start new</a></li>\r\n    <li><a");
            BeginWriteAttribute("href", " href=\"", 326, "\"", 376, 2);
            WriteAttributeValue("", 333, "/RhymeBinder/ListTexts?userID=", 333, 30, true);
#nullable restore
#line 17 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\Index.cshtml"
WriteAttributeValue("", 363, Model.UserId, 363, 13, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">List Texts</a></li>\r\n    <li><a");
            BeginWriteAttribute("href", " href=\"", 409, "\"", 460, 2);
            WriteAttributeValue("", 416, "/RhymeBinder/ScrapStack?userID=", 416, 31, true);
#nullable restore
#line 18 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\Index.cshtml"
WriteAttributeValue("", 447, Model.UserId, 447, 13, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">Scrap Stack</a></li>\r\n\r\n</ul>");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<SimpleUser> Html { get; private set; }
    }
}
#pragma warning restore 1591
