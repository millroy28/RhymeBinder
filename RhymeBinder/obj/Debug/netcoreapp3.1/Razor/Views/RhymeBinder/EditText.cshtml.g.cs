#pragma checksum "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "96f5248716eef256fcfe565a12188a4c187d0b0a"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_RhymeBinder_EditText), @"mvc.1.0.view", @"/Views/RhymeBinder/EditText.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"96f5248716eef256fcfe565a12188a4c187d0b0a", @"/Views/RhymeBinder/EditText.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"09a45f068354f87046b22aebe54e24a87c312eff", @"/Views/_ViewImports.cshtml")]
    public class Views_RhymeBinder_EditText : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<TextHeaderBodyUserRecord>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("action", new global::Microsoft.AspNetCore.Html.HtmlString("EditText"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
  
    ViewData["Title"] = "EditText";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h2>Editing text: </h2>\r\n\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "96f5248716eef256fcfe565a12188a4c187d0b0a4323", async() => {
                WriteLiteral("\r\n    <input type=\"hidden\" name=\"TextHeader.TextHeaderId\"");
                BeginWriteAttribute("value", " value=\"", 210, "\"", 248, 1);
#nullable restore
#line 12 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 218, Model.TextHeader.TextHeaderId, 218, 30, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"TextHeader.TextGroupId\"");
                BeginWriteAttribute("value", " value=\"", 308, "\"", 345, 1);
#nullable restore
#line 13 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 316, Model.TextHeader.TextGroupId, 316, 29, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"TextHeader.TextId\"");
                BeginWriteAttribute("value", " value=\"", 400, "\"", 432, 1);
#nullable restore
#line 14 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 408, Model.TextHeader.TextId, 408, 24, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n");
                WriteLiteral("    <input type=\"hidden\" name=\"TextHeader.Created\"");
                BeginWriteAttribute("value", " value=\"", 577, "\"", 610, 1);
#nullable restore
#line 16 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 585, Model.TextHeader.Created, 585, 25, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"TextHeader.CreatedBy\"");
                BeginWriteAttribute("value", " value=\"", 668, "\"", 703, 1);
#nullable restore
#line 17 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 676, Model.TextHeader.CreatedBy, 676, 27, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"TextHeader.LastModified\"");
                BeginWriteAttribute("value", " value=\"", 764, "\"", 802, 1);
#nullable restore
#line 18 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 772, Model.TextHeader.LastModified, 772, 30, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"TextHeader.LastModifiedBy\"");
                BeginWriteAttribute("value", " value=\"", 865, "\"", 905, 1);
#nullable restore
#line 19 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 873, Model.TextHeader.LastModifiedBy, 873, 32, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"TextHeader.LastRead\"");
                BeginWriteAttribute("value", " value=\"", 962, "\"", 996, 1);
#nullable restore
#line 20 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 970, Model.TextHeader.LastRead, 970, 26, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"TextHeader.LastReadBy\"");
                BeginWriteAttribute("value", " value=\"", 1055, "\"", 1091, 1);
#nullable restore
#line 21 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 1063, Model.TextHeader.LastReadBy, 1063, 28, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n");
                WriteLiteral("    <input type=\"hidden\" name=\"TextHeader.VisionNumber\"");
                BeginWriteAttribute("value", " value=\"", 1273, "\"", 1311, 1);
#nullable restore
#line 23 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 1281, Model.TextHeader.VisionNumber, 1281, 30, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"TextHeader.VersionOf\"");
                BeginWriteAttribute("value", " value=\"", 1369, "\"", 1404, 1);
#nullable restore
#line 24 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 1377, Model.TextHeader.VersionOf, 1377, 27, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n");
                WriteLiteral("\r\n    <input type=\"hidden\" name=\"User.UserId\"");
                BeginWriteAttribute("value", " value=\"", 1724, "\"", 1750, 1);
#nullable restore
#line 29 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 1732, Model.User.UserId, 1732, 18, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"User.UserName\"");
                BeginWriteAttribute("value", " value=\"", 1801, "\"", 1829, 1);
#nullable restore
#line 30 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 1809, Model.User.UserName, 1809, 20, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"User.AspNetUserId\"");
                BeginWriteAttribute("value", " value=\"", 1884, "\"", 1916, 1);
#nullable restore
#line 31 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 1892, Model.User.AspNetUserId, 1892, 24, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n\r\n    <input type=\"hidden\" name=\"Text.TextId\"");
                BeginWriteAttribute("value", " value=\"", 1967, "\"", 1993, 1);
#nullable restore
#line 33 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 1975, Model.Text.TextId, 1975, 18, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"Text.Created\"");
                BeginWriteAttribute("value", " value=\"", 2043, "\"", 2070, 1);
#nullable restore
#line 34 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 2051, Model.Text.Created, 2051, 19, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <select name=\"TextHeader.TextRevisionStatusID\">\r\n");
#nullable restore
#line 36 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
         foreach (var revisionStatus in Model.RevisionStatuses)
        {
            if (revisionStatus.TextRevisionStatusId == @Model.TextHeader.TextRevisionStatusId)
            {

#line default
#line hidden
#nullable disable
                WriteLiteral("                ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "96f5248716eef256fcfe565a12188a4c187d0b0a12450", async() => {
#nullable restore
#line 40 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
                                                                         Write(revisionStatus.TextRevisionStatus1);

#line default
#line hidden
#nullable disable
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                BeginWriteTagHelperAttribute();
#nullable restore
#line 40 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
                   WriteLiteral(revisionStatus.TextRevisionStatusId);

#line default
#line hidden
#nullable disable
                __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = __tagHelperStringValueBuffer;
                __tagHelperExecutionContext.AddTagHelperAttribute("value", __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                BeginWriteTagHelperAttribute();
                __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                __tagHelperExecutionContext.AddHtmlAttribute("selected", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.Minimized);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n");
#nullable restore
#line 41 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
            }
            else
            {

#line default
#line hidden
#nullable disable
                WriteLiteral("                ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "96f5248716eef256fcfe565a12188a4c187d0b0a14989", async() => {
#nullable restore
#line 44 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
                                                                Write(revisionStatus.TextRevisionStatus1);

#line default
#line hidden
#nullable disable
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.OptionTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper);
                BeginWriteTagHelperAttribute();
#nullable restore
#line 44 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
                   WriteLiteral(revisionStatus.TextRevisionStatusId);

#line default
#line hidden
#nullable disable
                __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value = __tagHelperStringValueBuffer;
                __tagHelperExecutionContext.AddTagHelperAttribute("value", __Microsoft_AspNetCore_Mvc_TagHelpers_OptionTagHelper.Value, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n");
#nullable restore
#line 45 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
            }
        }

#line default
#line hidden
#nullable disable
                WriteLiteral("    </select>\r\n\r\n    <input type=\"text\" name=\"TextHeader.Title\"");
                BeginWriteAttribute("value", " value=\"", 2692, "\"", 2723, 1);
#nullable restore
#line 49 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 2700, Model.TextHeader.Title, 2700, 23, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"text\" name=\"Text.TextBody\"");
                BeginWriteAttribute("value", " value=\"", 2772, "\"", 2800, 1);
#nullable restore
#line 50 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 2780, Model.Text.TextBody, 2780, 20, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"submit\" value=\"Save and Exit\" />\r\n");
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
            WriteLiteral("\r\n\r\n\r\n");
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
