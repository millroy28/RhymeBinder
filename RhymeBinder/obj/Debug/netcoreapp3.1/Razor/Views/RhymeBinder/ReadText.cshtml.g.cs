#pragma checksum "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ReadText.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8021174e17f2d7c62dbf91d28f59e72e8006a84d"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_RhymeBinder_ReadText), @"mvc.1.0.view", @"/Views/RhymeBinder/ReadText.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8021174e17f2d7c62dbf91d28f59e72e8006a84d", @"/Views/RhymeBinder/ReadText.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"09a45f068354f87046b22aebe54e24a87c312eff", @"/Views/_ViewImports.cshtml")]
    public class Views_RhymeBinder_ReadText : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<TextHeaderBodyUserRecord>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ReadText.cshtml"
  
    ViewData["Title"] = "ReadText";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"wrapper\">\r\n\r\n<div class=\"a font-control-header\">\r\n    Reading:\r\n</div>\r\n\r\n<div class=\"b font-weight-bold\">\r\n    ");
#nullable restore
#line 16 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ReadText.cshtml"
Write(Model.TextHeader.Title);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n</div>\r\n\r\n<div class=\"d\">\r\n    <button");
            BeginWriteAttribute("onclick", " onclick=\"", 275, "\"", 376, 5);
            WriteAttributeValue("", 285, "window.location.href", 285, 20, true);
            WriteAttributeValue(" ", 305, "=", 306, 2, true);
            WriteAttributeValue(" ", 307, "\'/RhymeBinder/EditText?textHeaderID=", 308, 37, true);
#nullable restore
#line 20 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ReadText.cshtml"
WriteAttributeValue("", 344, Model.TextHeader.TextHeaderId, 344, 30, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 374, "\';", 374, 2, true);
            EndWriteAttribute();
            WriteLiteral(">\r\n        Edit\r\n    </button>\r\n</div>\r\n<div class=\"e\">\r\n    <button");
            BeginWriteAttribute("onclick", " onclick=\"", 445, "\"", 548, 5);
            WriteAttributeValue("", 455, "window.location.href", 455, 20, true);
            WriteAttributeValue(" ", 475, "=", 476, 2, true);
            WriteAttributeValue(" ", 477, "\'/RhymeBinder/HideHeader?textHeaderID=", 478, 39, true);
#nullable restore
#line 25 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ReadText.cshtml"
WriteAttributeValue("", 516, Model.TextHeader.TextHeaderId, 516, 30, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 546, "\';", 546, 2, true);
            EndWriteAttribute();
            WriteLiteral(">\r\n        Delete\r\n    </button>\r\n</div>\r\n<div class=\"f\">\r\n    <button");
            BeginWriteAttribute("onclick", " onclick=\"", 619, "\"", 723, 5);
            WriteAttributeValue("", 629, "window.location.href", 629, 20, true);
            WriteAttributeValue(" ", 649, "=", 650, 2, true);
            WriteAttributeValue(" ", 651, "\'/RhymeBinder/AddRevision?textHeaderID=", 652, 40, true);
#nullable restore
#line 30 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ReadText.cshtml"
WriteAttributeValue("", 691, Model.TextHeader.TextHeaderId, 691, 30, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 721, "\';", 721, 2, true);
            EndWriteAttribute();
            WriteLiteral(">\r\n        Re-vision\r\n    </button>\r\n</div>\r\n\r\n<div class=\"g font-control-label\">\r\n    Revision Status:\r\n</div>\r\n<div class=\"h\">\r\n    ");
#nullable restore
#line 39 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ReadText.cshtml"
Write(Model.CurrentRevisionStatus);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n</div>\r\n<div class=\"i font-control-label\">\r\n    Last Modified by:\r\n</div>\r\n<div class=\"j\">\r\n    ");
#nullable restore
#line 45 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ReadText.cshtml"
Write(Model.User.UserName);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n</div>\r\n<div class=\"k font-control-label\">\r\n    Last Modified on:\r\n</div>\r\n<div class=\"l\">\r\n    ");
#nullable restore
#line 51 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ReadText.cshtml"
Write(Model.TextHeader.LastModified);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n</div>\r\n<div class=\"m font-control-label\">\r\n    Last Read by:\r\n</div>\r\n<div class=\"n\">\r\n    ");
#nullable restore
#line 57 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ReadText.cshtml"
Write(Model.User.UserName);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n</div>\r\n<div class=\"o font-control-label\">\r\n    Last Read on:\r\n</div>\r\n<div class=\"p\">\r\n    ");
#nullable restore
#line 63 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ReadText.cshtml"
Write(Model.TextHeader.LastRead);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n</div>\r\n\r\n<div class=\"z title-box\">\r\n    <textarea rows=\"1\" cols=\"100\" readonly =\"true\"> ");
#nullable restore
#line 67 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ReadText.cshtml"
                                               Write(Model.TextHeader.Title);

#line default
#line hidden
#nullable disable
            WriteLiteral(" </textarea>\r\n</div>\r\n\r\n<div class=\"y editor\">\r\n    <textarea rows=\"30\" cols=\"100\" readonly=\"true\"> ");
#nullable restore
#line 71 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ReadText.cshtml"
                                               Write(Model.Text.TextBody);

#line default
#line hidden
#nullable disable
            WriteLiteral(" </textarea>\r\n</div>\r\n</div>\r\n\r\n");
            WriteLiteral("\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<TextHeaderBodyUserRecord> Html { get; private set; }
    }
}
#pragma warning restore 1591
