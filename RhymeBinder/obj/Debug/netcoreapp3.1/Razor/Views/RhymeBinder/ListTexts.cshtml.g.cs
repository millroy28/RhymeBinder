#pragma checksum "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "d10df4f2c5e13a2a187be24cfcb1a67fe19d8d2e"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_RhymeBinder_ListTexts), @"mvc.1.0.view", @"/Views/RhymeBinder/ListTexts.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d10df4f2c5e13a2a187be24cfcb1a67fe19d8d2e", @"/Views/RhymeBinder/ListTexts.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"09a45f068354f87046b22aebe54e24a87c312eff", @"/Views/_ViewImports.cshtml")]
    public class Views_RhymeBinder_ListTexts : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<DisplayTextHeadersAndSavedView>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("action", new global::Microsoft.AspNetCore.Html.HtmlString("ListTexts"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("view"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("language", new global::Microsoft.AspNetCore.Html.HtmlString("Javascript"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/js/site.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
  
    ViewData["Title"] = "ListTexts";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 8 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
   int index = 0; 

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "d10df4f2c5e13a2a187be24cfcb1a67fe19d8d2e5563", async() => {
                WriteLiteral("\r\n    <input type=\"hidden\" name=\"View.SavedViewId\"");
                BeginWriteAttribute("value", " value=\"", 216, "\"", 247, 1);
#nullable restore
#line 11 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 224, Model.View.SavedViewId, 224, 23, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"View.UserId\"");
                BeginWriteAttribute("value", " value=\"", 296, "\"", 322, 1);
#nullable restore
#line 12 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 304, Model.View.UserId, 304, 18, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"View.SetValue\"");
                BeginWriteAttribute("value", " value=\"", 373, "\"", 401, 1);
#nullable restore
#line 13 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 381, Model.View.SetValue, 381, 20, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" id=\"view_set_value\" />\r\n    <input type=\"hidden\" name=\"View.SortValue\"");
                BeginWriteAttribute("value", " value=\"", 473, "\"", 502, 1);
#nullable restore
#line 14 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 481, Model.View.SortValue, 481, 21, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" id=\"view_sort_value\" />\r\n    <input type=\"hidden\" name=\"View.Descending\"");
                BeginWriteAttribute("value", " value=\"", 576, "\"", 617, 1);
#nullable restore
#line 15 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 584, Model.View.Descending.ToString(), 584, 33, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" id=\"view_descending\" />\r\n    <input type=\"hidden\" name=\"View.ViewName\"");
                BeginWriteAttribute("value", " value=\"", 689, "\"", 717, 1);
#nullable restore
#line 16 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 697, Model.View.ViewName, 697, 20, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" id=\"view_view_name\" />\r\n    <input type=\"hidden\" name=\"View.Default\"");
                BeginWriteAttribute("value", " value=\"", 787, "\"", 825, 1);
#nullable restore
#line 17 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 795, Model.View.Default.ToString(), 795, 30, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" id=\"view_default\" />\r\n    <input type=\"hidden\" name=\"View.Saved\"");
                BeginWriteAttribute("value", " value=\"", 891, "\"", 927, 1);
#nullable restore
#line 18 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 899, Model.View.Saved.ToString(), 899, 28, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" id=\"view_saved\" />\r\n    <input type=\"hidden\" name=\"View.LastView\"");
                BeginWriteAttribute("value", " value=\"", 994, "\"", 1033, 1);
#nullable restore
#line 19 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 1002, Model.View.LastView.ToString(), 1002, 31, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" id=\"view_last_view\" />\r\n    <input type=\"hidden\" name=\"View.LastModified\"");
                BeginWriteAttribute("value", " value=\"", 1108, "\"", 1151, 1);
#nullable restore
#line 20 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 1116, Model.View.LastModified.ToString(), 1116, 35, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" id=\"view_last_modified\" />\r\n    <input type=\"hidden\" name=\"View.LastModifiedBy\"");
                BeginWriteAttribute("value", " value=\"", 1232, "\"", 1277, 1);
#nullable restore
#line 21 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 1240, Model.View.LastModifiedBy.ToString(), 1240, 37, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" id=\"view_last_modified_by\" />\r\n    <input type=\"hidden\" name=\"View.Created\"");
                BeginWriteAttribute("value", " value=\"", 1354, "\"", 1392, 1);
#nullable restore
#line 22 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 1362, Model.View.Created.ToString(), 1362, 30, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" id=\"view_created\" />\r\n    <input type=\"hidden\" name=\"View.CreatedBy\"");
                BeginWriteAttribute("value", " value=\"", 1462, "\"", 1502, 1);
#nullable restore
#line 23 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 1470, Model.View.CreatedBy.ToString(), 1470, 32, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" id=\"view_created_by\" />\r\n    <input type=\"hidden\" name=\"View.VisionNumber\"");
                BeginWriteAttribute("value", " value=\"", 1578, "\"", 1621, 1);
#nullable restore
#line 24 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 1586, Model.View.VisionNumber.ToString(), 1586, 35, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" id=\"view_vision_number\" />\r\n    <input type=\"hidden\" name=\"View.RevisionStatus\"");
                BeginWriteAttribute("value", " value=\"", 1702, "\"", 1747, 1);
#nullable restore
#line 25 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 1710, Model.View.RevisionStatus.ToString(), 1710, 37, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" id=\"revision_status\" />\r\n    <input type=\"hidden\" name=\"View.User.UserId\"");
                BeginWriteAttribute("value", " value=\"", 1822, "\"", 1853, 1);
#nullable restore
#line 26 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 1830, Model.View.User.UserId, 1830, 23, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" id=\"view_userID\" />\r\n    <input type=\"hidden\" name=\"View.User.AspNetUserId\"");
                BeginWriteAttribute("value", " value=\"", 1930, "\"", 1967, 1);
#nullable restore
#line 27 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 1938, Model.View.User.AspNetUserId, 1938, 29, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"View.User.UserName\"");
                BeginWriteAttribute("value", " value=\"", 2023, "\"", 2056, 1);
#nullable restore
#line 28 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 2031, Model.View.User.UserName, 2031, 25, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(@" id=""view_user_name"" />
    <input type=""submit"" hidden name=""action"" value=""LastView"" id=""bttn_sub"" />
    <input type=""hidden"" name=""groupID"" value=""-1"" id=""group_id"" />

    <div class=""table-view-wrapper"">
        <div class=""list-texts-menu-bar-main"">
            <div class=""menu-bar-title"" id=""menu_bar_title"">
                Your Binder:
            </div>

            <div class=""menu-bar-dropdown"">
                <div class=""menu-bar-button-dropdown"">
                    Columns ▼
                </div>
                <div class=""menu-bar-dropdown-content"">
                    <a class=""menu-bar-dropdown-item"" onclick=""hidden_view_form_submit('LastView', 'view_last_modified', this.id, 'last_modified_col')"" id=""toggle_last_modified"">? Last Modified</a>
                    <a class=""menu-bar-dropdown-item"" onclick=""hidden_view_form_submit('LastView', 'view_last_modified_by', this.id, 'last_modified_by_col')"" id=""toggle_last_modified_by"">? Last Modified By</a>
                    <a c");
                WriteLiteral(@"lass=""menu-bar-dropdown-item"" onclick=""hidden_view_form_submit('LastView', 'view_created', this.id, 'created_col')"" id=""toggle_created"">? Created</a>
                    <a class=""menu-bar-dropdown-item"" onclick=""hidden_view_form_submit('LastView', 'view_created_by', this.id, 'created_by_col')"" id=""toggle_created_by"">? Created By</a>
                    <a class=""menu-bar-dropdown-item"" onclick=""hidden_view_form_submit('LastView', 'view_vision_number', this.id, 'vision_col')"" id=""toggle_vision_no"">? Vision No</a>
                    <a class=""menu-bar-dropdown-item"" onclick=""hidden_view_form_submit('LastView', 'revision_status', this.id, 'revision_col')"" id=""toggle_revision"">? Revision</a>
                </div>
            </div>
            <div class=""menu-bar-dropdown"">
                <div class=""menu-bar-button-dropdown"">
                    Groups ▼
                </div>
                <div class=""menu-bar-dropdown-content"">
                    <a class=""menu-bar-dropdown-item"" onclick=""gr");
                WriteLiteral(@"ouping_view_form_submit('LastView', 'view_set_value', this.id)"" id=""set_value_all"">All</a>
                    <a class=""menu-bar-dropdown-item"" onclick=""grouping_view_form_submit('LastView', 'view_set_value', this.id)"" id=""set_value_active"">Active</a>
                    <a class=""menu-bar-dropdown-item"" onclick=""grouping_view_form_submit('LastView', 'view_set_value', this.id)"" id=""set_value_scrapped"">Scrapped</a>
                </div>
            </div>
            <div class=""menu-bar-dropdown"">
                <div class=""menu-bar-button-dropdown"">
                    Views ▼
                </div>
                <div class=""menu-bar-dropdown-content"">
                    <a class=""menu-bar-dropdown-item"">Default</a>
                    <a class=""menu-bar-dropdown-item"" onclick=""sub_form('SaveDefault');"">Set Default</a>
                    <a class=""menu-bar-dropdown-item"">Add new...</a>
                </div>
            </div>
            <div class=""menu-bar-dropdown"">
               ");
                WriteLiteral(@" <div class=""menu-bar-button-dropdown"">
                    Actions ▼
                </div>
                <div class=""menu-bar-dropdown-content"">
                    <a class=""menu-bar-dropdown-item"" onclick=""selected_action_form_submit('Scrap', '-1')"">Move to Scrap Stack</a>
                    <a class=""menu-bar-dropdown-item"">Add to Group... </a>
");
#nullable restore
#line 78 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
                     foreach (var group in Model.Groups)
                    {

#line default
#line hidden
#nullable disable
                WriteLiteral("                        <a class=\"menu-bar-dropdown-item\"");
                BeginWriteAttribute("onclick", " onclick=\"", 5628, "\"", 5697, 3);
                WriteAttributeValue("", 5638, "selected_action_form_submit(\'GroupAdd\',", 5638, 39, true);
#nullable restore
#line 80 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue(" ", 5677, group.TextGroupId, 5678, 18, false);

#line default
#line hidden
#nullable disable
                WriteAttributeValue("", 5696, ")", 5696, 1, true);
                EndWriteAttribute();
                WriteLiteral(">  + ");
#nullable restore
#line 80 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
                                                                                                                               Write(group.GroupTitle);

#line default
#line hidden
#nullable disable
                WriteLiteral("</a>\r\n");
#nullable restore
#line 81 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
                    }

#line default
#line hidden
#nullable disable
                WriteLiteral("                    <a class=\"menu-bar-dropdown-item\">Remove from Group... </a>\r\n");
#nullable restore
#line 83 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
                     foreach (var group in Model.Groups)
                    {

#line default
#line hidden
#nullable disable
                WriteLiteral("                        <a class=\"menu-bar-dropdown-item\"");
                BeginWriteAttribute("onclick", " onclick=\"", 5968, "\"", 6040, 3);
                WriteAttributeValue("", 5978, "selected_action_form_submit(\'GroupRemove\',", 5978, 42, true);
#nullable restore
#line 85 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue(" ", 6020, group.TextGroupId, 6021, 18, false);

#line default
#line hidden
#nullable disable
                WriteAttributeValue("", 6039, ")", 6039, 1, true);
                EndWriteAttribute();
                WriteLiteral(">  - ");
#nullable restore
#line 85 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
                                                                                                                                  Write(group.GroupTitle);

#line default
#line hidden
#nullable disable
                WriteLiteral("</a>\r\n");
#nullable restore
#line 86 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
                    }

#line default
#line hidden
#nullable disable
                WriteLiteral(@"                </div>
            </div>
        </div>
        <div class=""table-view-table-container"">
            <table>
                <tr>
                    <th name=""selected_col"" id=""Selected""></th>
                    <th name=""title_col"" id=""Title"" onclick=""col_header_sort_change(this.id, 'view_sort_value', 'view_descending');""><a>Title</a></th>
                    <th name=""last_modified_col"" id=""Last Modified"" onclick=""col_header_sort_change(this.id, 'view_sort_value', 'view_descending');""><a>Last Modified</a></th>
                    <th name=""last_modified_by_col"" id=""Last Modified By"" onclick=""col_header_sort_change(this.id, 'view_sort_value', 'view_descending');""><a>Last Modified By</a></th>
                    <th name=""created_col"" id=""Created"" onclick=""col_header_sort_change(this.id, 'view_sort_value', 'view_descending');""><a>Created</a></th>
                    <th name=""created_by_col"" id=""Created By"" onclick=""col_header_sort_change(this.id, 'view_sort_value', 'view_descend");
                WriteLiteral(@"ing');""><a>Created By</a></th>
                    <th name=""vision_col"" id=""Vision Number"" onclick=""col_header_sort_change(this.id, 'view_sort_value', 'view_descending');""><a>Vision Number</a></th>
                    <th name=""revision_col"" id=""Revision"" onclick=""col_header_sort_change(this.id, 'view_sort_value', 'view_descending');""><a>Revision Status</a></th>
                </tr>

");
#nullable restore
#line 103 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
                 foreach (var text in Model.TextHeaders)
                {


#line default
#line hidden
#nullable disable
                WriteLiteral("                    <tr>\r\n\r\n                        <td>\r\n                            <input type=\"hidden\"");
                BeginWriteAttribute("name", " name=\"", 7695, "\"", 7734, 3);
                WriteAttributeValue("", 7702, "TextHeaders[", 7702, 12, true);
#nullable restore
#line 109 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 7714, index, 7714, 6, false);

#line default
#line hidden
#nullable disable
                WriteAttributeValue("", 7720, "].TextHeaderId", 7720, 14, true);
                EndWriteAttribute();
                BeginWriteAttribute("value", " value=\"", 7735, "\"", 7761, 1);
#nullable restore
#line 109 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 7743, text.TextHeaderId, 7743, 18, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n                            <input type=\"checkbox\"");
                BeginWriteAttribute("name", " name=\"", 7817, "\"", 7852, 3);
                WriteAttributeValue("", 7824, "TextHeaders[", 7824, 12, true);
#nullable restore
#line 110 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 7836, index, 7836, 6, false);

#line default
#line hidden
#nullable disable
                WriteAttributeValue("", 7842, "].Selected", 7842, 10, true);
                EndWriteAttribute();
                WriteLiteral(" value=\"true\" />\r\n                        </td>\r\n                        <td><a");
                BeginWriteAttribute("href", " href=\"", 7932, "\"", 7992, 2);
                WriteAttributeValue("", 7939, "/RhymeBinder/EditText?textHeaderID=", 7939, 35, true);
#nullable restore
#line 112 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 7974, text.TextHeaderId, 7974, 18, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(">");
#nullable restore
#line 112 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
                                                                                       Write(text.Title);

#line default
#line hidden
#nullable disable
                WriteLiteral("</a></td>\r\n                        <td name=\"last_modified_col\">");
#nullable restore
#line 113 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
                                                Write(text.LastModified);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                        <td name=\"last_modified_by_col\">");
#nullable restore
#line 114 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
                                                   Write(text.ModifyByName);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                        <td name=\"created_col\">");
#nullable restore
#line 115 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
                                          Write(text.Created);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                        <td name=\"created_by_col\">");
#nullable restore
#line 116 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
                                             Write(text.CreatedByName);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                        <td name=\"vision_col\">");
#nullable restore
#line 117 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
                                         Write(text.VisionNumber);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                        <td name=\"revision_col\">");
#nullable restore
#line 118 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
                                           Write(text.RevisionStatus);

#line default
#line hidden
#nullable disable
                WriteLiteral("</td>\r\n                    </tr>\r\n");
#nullable restore
#line 120 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
                    { index++; }
                }

#line default
#line hidden
#nullable disable
                WriteLiteral("\r\n            </table>\r\n        </div>\r\n    </div>\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "d10df4f2c5e13a2a187be24cfcb1a67fe19d8d2e29114", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
<script language=""Javascript"">
    window.onload = toggle_hide_element('view_last_modified', 'toggle_last_modified', 'last_modified_col', 'init');
    window.onload = toggle_hide_element('view_last_modified_by', 'toggle_last_modified_by', 'last_modified_by_col', 'init');
    window.onload = toggle_hide_element('view_created', 'toggle_created', 'created_col', 'init');
    window.onload = toggle_hide_element('view_created_by', 'toggle_created_by', 'created_by_col', 'init');
    window.onload = toggle_hide_element('view_vision_number', 'toggle_vision_no', 'vision_col', 'init');
    window.onload = toggle_hide_element('revision_status', 'toggle_revision', 'revision_col', 'init');
    window.onload = on_start_mark_sorted_column('view_sort_value', 'view_descending');
    window.onload = on_start_get_form_sub_button('bttn_sub');
</script>
");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<DisplayTextHeadersAndSavedView> Html { get; private set; }
    }
}
#pragma warning restore 1591
