#pragma checksum "C:\Users\millr\source\repos\RhymeBinder_CleanBuild\RhymeBinder\RhymeBinder\Views\RhymeBinder\ScrapStack.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "6ea04808cfc5eeefdb5ffe367306fc1af7fab65f"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_RhymeBinder_ScrapStack), @"mvc.1.0.view", @"/Views/RhymeBinder/ScrapStack.cshtml")]
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
#line 1 "C:\Users\millr\source\repos\RhymeBinder_CleanBuild\RhymeBinder\RhymeBinder\Views\_ViewImports.cshtml"
using RhymeBinder;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\millr\source\repos\RhymeBinder_CleanBuild\RhymeBinder\RhymeBinder\Views\_ViewImports.cshtml"
using RhymeBinder.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"6ea04808cfc5eeefdb5ffe367306fc1af7fab65f", @"/Views/RhymeBinder/ScrapStack.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"09a45f068354f87046b22aebe54e24a87c312eff", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_RhymeBinder_ScrapStack : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<DisplayTextHeader>>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "C:\Users\millr\source\repos\RhymeBinder_CleanBuild\RhymeBinder\RhymeBinder\Views\RhymeBinder\ScrapStack.cshtml"
  
    ViewData["Title"] = "ScrapStack";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            WriteLiteral("\r\n<h2>Scrap Stack:</h2>\r\n<p>Here you may restore what has been cast aside</p>\r\n<table class=\"table table-responsive\">\r\n    <tr>\r\n        <th><a href=\"/RhymeBinder/ChangeListDisplay?change=title\">Title</a></th>\r\n    </tr>\r\n\r\n");
#nullable restore
#line 16 "C:\Users\millr\source\repos\RhymeBinder_CleanBuild\RhymeBinder\RhymeBinder\Views\RhymeBinder\ScrapStack.cshtml"
     foreach (var text in Model)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <tr>\r\n            <td><a");
            BeginWriteAttribute("href", " href=\"", 384, "\"", 454, 2);
            WriteAttributeValue("", 391, "/RhymeBinder/ReviewScrappedText?textHeaderID=", 391, 45, true);
#nullable restore
#line 19 "C:\Users\millr\source\repos\RhymeBinder_CleanBuild\RhymeBinder\RhymeBinder\Views\RhymeBinder\ScrapStack.cshtml"
WriteAttributeValue("", 436, text.TextHeaderId, 436, 18, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">");
#nullable restore
#line 19 "C:\Users\millr\source\repos\RhymeBinder_CleanBuild\RhymeBinder\RhymeBinder\Views\RhymeBinder\ScrapStack.cshtml"
                                                                                     Write(text.Title);

#line default
#line hidden
#nullable disable
            WriteLiteral("</a></td>\r\n            <td><a");
            BeginWriteAttribute("href", " href=\"", 496, "\"", 559, 2);
            WriteAttributeValue("", 503, "/RhymeBinder/RestoreText?textHeaderID=", 503, 38, true);
#nullable restore
#line 20 "C:\Users\millr\source\repos\RhymeBinder_CleanBuild\RhymeBinder\RhymeBinder\Views\RhymeBinder\ScrapStack.cshtml"
WriteAttributeValue("", 541, text.TextHeaderId, 541, 18, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">Restore</a></td>\r\n\r\n        </tr>\r\n");
#nullable restore
#line 23 "C:\Users\millr\source\repos\RhymeBinder_CleanBuild\RhymeBinder\RhymeBinder\Views\RhymeBinder\ScrapStack.cshtml"

    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n</table>\r\n");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<DisplayTextHeader>> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
