#pragma checksum "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "77a2b55e8d9c27de5dc6e2d0941922c3abad6d4b"
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"77a2b55e8d9c27de5dc6e2d0941922c3abad6d4b", @"/Views/RhymeBinder/EditText.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"09a45f068354f87046b22aebe54e24a87c312eff", @"/Views/_ViewImports.cshtml")]
    public class Views_RhymeBinder_EditText : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<TextHeaderBodyUserRecord>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("editor-view-wrapper"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("action", new global::Microsoft.AspNetCore.Html.HtmlString("EditText"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("edit"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("language", new global::Microsoft.AspNetCore.Html.HtmlString("Javascript"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/autosave.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/editor.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
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
            WriteLiteral("\r\n\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "77a2b55e8d9c27de5dc6e2d0941922c3abad6d4b6213", async() => {
                WriteLiteral("\r\n    <input type=\"hidden\" name=\"TextHeader.TextHeaderId\"");
                BeginWriteAttribute("value", " value=\"", 223, "\"", 261, 1);
#nullable restore
#line 11 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 231, Model.TextHeader.TextHeaderId, 231, 30, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"TextHeader.TextGroupId\"");
                BeginWriteAttribute("value", " value=\"", 321, "\"", 358, 1);
#nullable restore
#line 12 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 329, Model.TextHeader.TextGroupId, 329, 29, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"TextHeader.TextId\"");
                BeginWriteAttribute("value", " value=\"", 413, "\"", 445, 1);
#nullable restore
#line 13 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 421, Model.TextHeader.TextId, 421, 24, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"TextHeader.Created\"");
                BeginWriteAttribute("value", " value=\"", 501, "\"", 534, 1);
#nullable restore
#line 14 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 509, Model.TextHeader.Created, 509, 25, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"TextHeader.CreatedBy\"");
                BeginWriteAttribute("value", " value=\"", 592, "\"", 627, 1);
#nullable restore
#line 15 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 600, Model.TextHeader.CreatedBy, 600, 27, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"TextHeader.LastModified\"");
                BeginWriteAttribute("value", " value=\"", 688, "\"", 726, 1);
#nullable restore
#line 16 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 696, Model.TextHeader.LastModified, 696, 30, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"TextHeader.LastModifiedBy\"");
                BeginWriteAttribute("value", " value=\"", 789, "\"", 829, 1);
#nullable restore
#line 17 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 797, Model.TextHeader.LastModifiedBy, 797, 32, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"TextHeader.LastRead\"");
                BeginWriteAttribute("value", " value=\"", 886, "\"", 920, 1);
#nullable restore
#line 18 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 894, Model.TextHeader.LastRead, 894, 26, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"TextHeader.LastReadBy\"");
                BeginWriteAttribute("value", " value=\"", 979, "\"", 1015, 1);
#nullable restore
#line 19 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 987, Model.TextHeader.LastReadBy, 987, 28, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"TextHeader.VisionNumber\"");
                BeginWriteAttribute("value", " value=\"", 1076, "\"", 1114, 1);
#nullable restore
#line 20 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 1084, Model.TextHeader.VisionNumber, 1084, 30, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"TextHeader.VersionOf\"");
                BeginWriteAttribute("value", " value=\"", 1172, "\"", 1207, 1);
#nullable restore
#line 21 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 1180, Model.TextHeader.VersionOf, 1180, 27, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"User.UserId\"");
                BeginWriteAttribute("value", " value=\"", 1256, "\"", 1282, 1);
#nullable restore
#line 22 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 1264, Model.User.UserId, 1264, 18, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"User.UserName\"");
                BeginWriteAttribute("value", " value=\"", 1333, "\"", 1361, 1);
#nullable restore
#line 23 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 1341, Model.User.UserName, 1341, 20, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"User.AspNetUserId\"");
                BeginWriteAttribute("value", " value=\"", 1416, "\"", 1448, 1);
#nullable restore
#line 24 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 1424, Model.User.AspNetUserId, 1424, 24, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"Text.TextId\"");
                BeginWriteAttribute("value", " value=\"", 1497, "\"", 1523, 1);
#nullable restore
#line 25 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 1505, Model.Text.TextId, 1505, 18, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"Text.Created\"");
                BeginWriteAttribute("value", " value=\"", 1573, "\"", 1600, 1);
#nullable restore
#line 26 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 1581, Model.Text.Created, 1581, 19, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"EditWindowProperty.CursorPosition\" id=\"cursor_position\"");
                BeginWriteAttribute("value", " value=\"", 1692, "\"", 1740, 1);
#nullable restore
#line 27 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 1700, Model.EditWindowProperty.CursorPosition, 1700, 40, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"EditWindowProperty.ActiveElement\" id=\"form_focus\"");
                BeginWriteAttribute("value", " value=\"", 1826, "\"", 1873, 1);
#nullable restore
#line 28 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 1834, Model.EditWindowProperty.ActiveElement, 1834, 39, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"EditWindowProperty.ShowLineCount\" id=\"show_line_count\"");
                BeginWriteAttribute("value", " value=\"", 1964, "\"", 2011, 1);
#nullable restore
#line 29 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 1972, Model.EditWindowProperty.ShowLineCount, 1972, 39, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"EditWindowProperty.ShowParagraphCount\" id=\"show_paragraph_count\"");
                BeginWriteAttribute("value", " value=\"", 2112, "\"", 2164, 1);
#nullable restore
#line 30 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 2120, Model.EditWindowProperty.ShowParagraphCount, 2120, 44, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"EditWindowProperty.UserID\"");
                BeginWriteAttribute("value", " value=\"", 2227, "\"", 2253, 1);
#nullable restore
#line 31 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 2235, Model.User.UserId, 2235, 18, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"EditWindowProperty.TextHeaderID\"");
                BeginWriteAttribute("value", " value=\"", 2322, "\"", 2360, 1);
#nullable restore
#line 32 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 2330, Model.TextHeader.TextHeaderId, 2330, 30, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(@" />

    <div class=""menu-bar-main"">
        <div class=""menu-bar-title"">
            Editing:
        </div>
        <div class=""menu-bar-item"">
            Revision Status:
        </div>
        <div class=""menu-bar-item"">
            <select name=""TextHeader.TextRevisionStatusID"" id=""revision_status"">
");
#nullable restore
#line 43 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
                 foreach (var revisionStatus in Model.AllRevisionStatuses)
                {
                    if (revisionStatus.TextRevisionStatusId == @Model.TextHeader.TextRevisionStatusId)
                    {

#line default
#line hidden
#nullable disable
                WriteLiteral("                        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "77a2b55e8d9c27de5dc6e2d0941922c3abad6d4b17510", async() => {
#nullable restore
#line 47 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
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
#line 47 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
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
#line 48 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
                    }
                    else
                    {

#line default
#line hidden
#nullable disable
                WriteLiteral("                        ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("option", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "77a2b55e8d9c27de5dc6e2d0941922c3abad6d4b20097", async() => {
#nullable restore
#line 51 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
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
#line 51 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
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
#line 52 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
                    }
                }

#line default
#line hidden
#nullable disable
                WriteLiteral(@"            </select>
        </div>
        <div class=""menu-bar-item-spacer"">

        </div>
        <div class=""menu-bar-dropdown"">
            <div class=""button menu-bar-button-dropdown"">
                View ▼
            </div>
            <div class=""menu-bar-dropdown-content"">
                <a class=""menu-bar-dropdown-item"" onclick=""toggle_hide_element('show_line_count', this.id, 'line_count')"" id=""toggle_line_count"">? Line Count</a>
                <a class=""menu-bar-dropdown-item"" onclick=""toggle_hide_element('show_paragraph_count', this.id, 'paragraph_count')"" id=""toggle_paragraph_count"">? Stanza Count</a>
            </div>

        </div>
    </div>



    <div class=""menu-bar-escape"">
        <div class=""menu-bar-item"">
            <input type=""submit"" name=""action"" class=""button menu-bar-button"" value=""Return"" />
        </div>
        <div class=""menu-bar-item"">
            <input type=""submit"" name=""action"" class=""button menu-bar-button"" value=""Save"" id=""save"" />
");
                WriteLiteral(@"        </div>
        <div class=""menu-bar-item-right-final"">
            <input type=""submit"" name=""action"" class=""button menu-bar-button"" value=""Revision"" id=""save"" />
        </div>
    </div>

    <div class=""title-bar"">
        <textarea name=""TextHeader.Title"" class=""title-box"" id=""title_edit_field"">");
#nullable restore
#line 86 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
                                                                             Write(Model.TextHeader.Title);

#line default
#line hidden
#nullable disable
                WriteLiteral("</textarea>\r\n    </div>\r\n\r\n    <div class=\"editor-container\">\r\n        <textarea name=\"Text.TextBody\" class=\"editor-box\" id=\"body_edit_field\" onscroll=\"sync_scroll()\">");
#nullable restore
#line 90 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
                                                                                                   Write(Model.Text.TextBody);

#line default
#line hidden
#nullable disable
                WriteLiteral(@"</textarea>
    </div>
    <div class=""ruler-container-a"">
        <textarea class=""ruler-box ruler-box-b"" id=""paragraph_count"" readonly></textarea>
    </div>
    <div class=""ruler-container-b"">
        <textarea class=""ruler-box"" id=""line_count"" readonly></textarea>
    </div>

    <div class=""left-sidebar"">
        <div class=""left-sidebar-item-heading"">");
#nullable restore
#line 100 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
                                          Write(Model.TextHeader.Title);

#line default
#line hidden
#nullable disable
                WriteLiteral("</div>\r\n        <div class=\"left-sidebar-item\">Vision No. ");
#nullable restore
#line 101 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
                                             Write(Model.TextHeader.VisionNumber);

#line default
#line hidden
#nullable disable
                WriteLiteral("</div>\r\n");
#nullable restore
#line 102 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
         if (Model.PreviousTexts.Count() > 0)
        {

#line default
#line hidden
#nullable disable
                WriteLiteral("            <div class=\"left-sidebar-item\">Previous visions:</div>\r\n");
#nullable restore
#line 105 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
            foreach (var prev in Model.PreviousTexts)
            {


#line default
#line hidden
#nullable disable
                WriteLiteral("                <div class=\"left-sidebar-item-heading\">No. ");
#nullable restore
#line 108 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
                                                      Write(prev.VisionNumber);

#line default
#line hidden
#nullable disable
                WriteLiteral(": <a href=\"#\" data-drawer-trigger");
                BeginWriteAttribute("aria-controls", " aria-controls=\"", 5615, "\"", 5649, 1);
#nullable restore
#line 108 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 5631, prev.VisionNumber, 5631, 18, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" aria-expanded=\"false\">");
#nullable restore
#line 108 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
                                                                                                                                                                   Write(prev.Title);

#line default
#line hidden
#nullable disable
                WriteLiteral("</a></div>\r\n                <div class=\"left-sidebar-item\">Status: ");
#nullable restore
#line 109 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
                                                  Write(prev.Status);

#line default
#line hidden
#nullable disable
                WriteLiteral("</div>\r\n                <div class=\"left-sidebar-item\">Created ");
#nullable restore
#line 110 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
                                                  Write(prev.Created.Value.ToString("d"));

#line default
#line hidden
#nullable disable
                WriteLiteral(" by ");
#nullable restore
#line 110 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
                                                                                       Write(prev.CreatedBy);

#line default
#line hidden
#nullable disable
                WriteLiteral("</div>\r\n                <div class=\"left-sidebar-item\">Last Modified ");
#nullable restore
#line 111 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
                                                        Write(prev.LastModified.Value.ToString("d"));

#line default
#line hidden
#nullable disable
                WriteLiteral(" by ");
#nullable restore
#line 111 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
                                                                                                  Write(prev.LastModifiedBy);

#line default
#line hidden
#nullable disable
                WriteLiteral("</div>\r\n");
#nullable restore
#line 112 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
            }
        }

#line default
#line hidden
#nullable disable
                WriteLiteral("    </div>\r\n\r\n    <div class=\"right-sidebar\">\r\n        <div class=\"right-sidebar-item-heading\">Last Modified ");
#nullable restore
#line 117 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
                                                         Write(Model.TextHeader.LastModified.Value.ToString("d"));

#line default
#line hidden
#nullable disable
                WriteLiteral("</div>\r\n        <div class=\"right-sidebar-item\">by ");
#nullable restore
#line 118 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
                                      Write(Model.LastModifiedByUser.UserName);

#line default
#line hidden
#nullable disable
                WriteLiteral("</div>\r\n        <div class=\"right-sidebar-item-heading\">Created ");
#nullable restore
#line 119 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
                                                   Write(Model.TextHeader.Created.Value.ToString("d"));

#line default
#line hidden
#nullable disable
                WriteLiteral("</div>\r\n        <div class=\"right-sidebar-item\">by ");
#nullable restore
#line 120 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
                                      Write(Model.CreatedByUser.UserName);

#line default
#line hidden
#nullable disable
                WriteLiteral("</div>\r\n    </div>\r\n\r\n\r\n\r\n\r\n\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n");
#nullable restore
#line 130 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
 foreach (var prev in Model.PreviousTexts)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <section class=\"drawer drawer--left\"");
            BeginWriteAttribute("id", " id=\"", 6628, "\"", 6651, 1);
#nullable restore
#line 132 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
WriteAttributeValue("", 6633, prev.VisionNumber, 6633, 18, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" data-drawer-target>\r\n        <div class=\"drawer__wrapper\">\r\n            <div class=\"drawer__header\">\r\n                <div class=\"drawer__title\">\r\n                    ");
#nullable restore
#line 136 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
               Write(prev.Title);

#line default
#line hidden
#nullable disable
            WriteLiteral(" (Vision No.: ");
#nullable restore
#line 136 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
                                        Write(prev.VisionNumber);

#line default
#line hidden
#nullable disable
            WriteLiteral(")\r\n                </div>\r\n                <button class=\"drawer__close\" data-drawer-close aria-label=\"Close Drawer\"></button>\r\n            </div>\r\n            <textarea class=\"drawer__content\">\r\n                ");
#nullable restore
#line 141 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
           Write(prev.TextBody);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </textarea>\r\n        </div>\r\n    </section>\r\n");
#nullable restore
#line 145 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\EditText.cshtml"
}

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div id=\"textWidthSample\" class=\"text-sample\" hidden>X</div>\r\n\r\n\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "77a2b55e8d9c27de5dc6e2d0941922c3abad6d4b34491", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "77a2b55e8d9c27de5dc6e2d0941922c3abad6d4b35614", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_6);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n<br />\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n");
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
