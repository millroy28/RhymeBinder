#pragma checksum "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditBinder.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "fe49515ee6583585ace760ae4f60f1da2977bd9d"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_RhymeBinder_EditBinder), @"mvc.1.0.view", @"/Views/RhymeBinder/EditBinder.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fe49515ee6583585ace760ae4f60f1da2977bd9d", @"/Views/RhymeBinder/EditBinder.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"09a45f068354f87046b22aebe54e24a87c312eff", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_RhymeBinder_EditBinder : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<DisplayBinder>
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("action", new global::Microsoft.AspNetCore.Html.HtmlString("CreateBinder"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("<div class=\"menu-bar-main\">\r\n    <div class=\"menu-bar-title\">\r\n        Edit Binder: ");
#nullable restore
#line 7 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditBinder.cshtml"
                Write(Model.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral(": \r\n    </div>\r\n</div>\r\n\r\n<h1>");
#nullable restore
#line 11 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditBinder.cshtml"
Write(Model.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral(":</h1>\r\n<ul>\r\n<li>Created: ");
#nullable restore
#line 13 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditBinder.cshtml"
        Write(Model.Created);

#line default
#line hidden
#nullable disable
            WriteLiteral("</li>\r\n<li>Created By: ");
#nullable restore
#line 14 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditBinder.cshtml"
           Write(Model.CreatedBy);

#line default
#line hidden
#nullable disable
            WriteLiteral("</li>\r\n<li>Last Modified: ");
#nullable restore
#line 15 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditBinder.cshtml"
              Write(Model.LastModified);

#line default
#line hidden
#nullable disable
            WriteLiteral("</li>\r\n<li>Last Modified By: ");
#nullable restore
#line 16 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditBinder.cshtml"
                 Write(Model.LastModifiedBy);

#line default
#line hidden
#nullable disable
            WriteLiteral("</li>\r\n<li>Number of Pages: ");
#nullable restore
#line 17 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditBinder.cshtml"
                Write(Model.PageCount);

#line default
#line hidden
#nullable disable
            WriteLiteral(" </li>\r\n<li>Number of Groups: ");
#nullable restore
#line 18 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditBinder.cshtml"
                 Write(Model.GroupCount);

#line default
#line hidden
#nullable disable
            WriteLiteral("</li>\r\n</ul>\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "fe49515ee6583585ace760ae4f60f1da2977bd9d6183", async() => {
                WriteLiteral("\r\n    Binder Name:\r\n    <input type=\"text\" name=\"Name\"");
                BeginWriteAttribute("placeholder", " placeholder=\"", 657, "\"", 682, 1);
#nullable restore
#line 22 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditBinder.cshtml"
WriteAttributeValue("", 671, Model.Name, 671, 11, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    Description:\r\n    <input type=\"text\" name=\"Desription\"");
                BeginWriteAttribute("placeholder", " placeholder=\"", 746, "\"", 778, 1);
#nullable restore
#line 24 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditBinder.cshtml"
WriteAttributeValue("", 760, Model.Description, 760, 18, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral("/>\r\n    <input type=\"hidden\" name=\"BinderId\"");
                BeginWriteAttribute("value", " value=\"", 823, "\"", 846, 1);
#nullable restore
#line 25 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditBinder.cshtml"
WriteAttributeValue("", 831, Model.BinderId, 831, 15, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(@" />
    <input type=""submit"" name=""action"" class=""button"" value=""Edit"" id=""bttn_sub"" />
    <hr />
    <p>
    Clear Binder: <i>This will remove all groups and pages from the binder. Pages will appear in 'Loose Pages' Binder.</i>
    <input type=""submit"" name=""action"" class=""button"" value=""Clear"" id=""bttn_sub"" />
    </p>
    <p>
    Delete Binder: <i>This will remove this binder. Pages will appear in 'Loose Pages' Binder.</i>
    <input type=""submit"" name=""action"" class=""button"" value=""Delete"" id=""bttn_sub"" />
    </p>
    <p>
    Delete Binder and all Contents: <i>This will remove this binder AND delete all pages!</i>
    <input type=""submit"" name=""action"" class=""button"" value=""DeleteAll"" id=""bttn_sub"" />
    </p>

");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<DisplayBinder> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591