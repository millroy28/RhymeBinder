#pragma checksum "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3258d727b96c06bb8045261fd7d273638d86779c"
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3258d727b96c06bb8045261fd7d273638d86779c", @"/Views/RhymeBinder/ListTexts.cshtml")]
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
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "3258d727b96c06bb8045261fd7d273638d86779c5376", async() => {
                WriteLiteral("\r\n    <input type=\"hidden\" name=\"View.SavedViewId\"");
                BeginWriteAttribute("value", " value=\"", 201, "\"", 232, 1);
#nullable restore
#line 10 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 209, Model.View.SavedViewId, 209, 23, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"View.UserId\"");
                BeginWriteAttribute("value", " value=\"", 281, "\"", 307, 1);
#nullable restore
#line 11 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 289, Model.View.UserId, 289, 18, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"View.SetValue\"");
                BeginWriteAttribute("value", " value=\"", 358, "\"", 386, 1);
#nullable restore
#line 12 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 366, Model.View.SetValue, 366, 20, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" id=\"view_set_value\" />\r\n    <input type=\"hidden\" name=\"View.SortValue\"");
                BeginWriteAttribute("value", " value=\"", 458, "\"", 487, 1);
#nullable restore
#line 13 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 466, Model.View.SortValue, 466, 21, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" id=\"view_sort_value\" />\r\n    <input type=\"hidden\" name=\"View.Descending\"");
                BeginWriteAttribute("value", " value=\"", 561, "\"", 602, 1);
#nullable restore
#line 14 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 569, Model.View.Descending.ToString(), 569, 33, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" id=\"view_descending\" />\r\n    <input type=\"hidden\" name=\"View.ViewName\"");
                BeginWriteAttribute("value", " value=\"", 674, "\"", 702, 1);
#nullable restore
#line 15 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 682, Model.View.ViewName, 682, 20, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" id=\"view_view_name\" />\r\n    <input type=\"hidden\" name=\"View.Default\"");
                BeginWriteAttribute("value", " value=\"", 772, "\"", 810, 1);
#nullable restore
#line 16 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 780, Model.View.Default.ToString(), 780, 30, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" id=\"view_default\" />\r\n    <input type=\"hidden\" name=\"View.Saved\"");
                BeginWriteAttribute("value", " value=\"", 876, "\"", 912, 1);
#nullable restore
#line 17 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 884, Model.View.Saved.ToString(), 884, 28, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" id=\"view_saved\" />\r\n    <input type=\"hidden\" name=\"View.LastView\"");
                BeginWriteAttribute("value", " value=\"", 979, "\"", 1018, 1);
#nullable restore
#line 18 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 987, Model.View.LastView.ToString(), 987, 31, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" id=\"view_last_view\" />\r\n    <input type=\"hidden\" name=\"View.LastModified\"");
                BeginWriteAttribute("value", " value=\"", 1093, "\"", 1136, 1);
#nullable restore
#line 19 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 1101, Model.View.LastModified.ToString(), 1101, 35, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" id=\"view_last_modified\" />\r\n    <input type=\"hidden\" name=\"View.LastModifiedBy\"");
                BeginWriteAttribute("value", " value=\"", 1217, "\"", 1262, 1);
#nullable restore
#line 20 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 1225, Model.View.LastModifiedBy.ToString(), 1225, 37, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" id=\"view_last_modified_by\" />\r\n    <input type=\"hidden\" name=\"View.Created\"");
                BeginWriteAttribute("value", " value=\"", 1339, "\"", 1377, 1);
#nullable restore
#line 21 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 1347, Model.View.Created.ToString(), 1347, 30, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" id=\"view_created\" />\r\n    <input type=\"hidden\" name=\"View.CreatedBy\"");
                BeginWriteAttribute("value", " value=\"", 1447, "\"", 1487, 1);
#nullable restore
#line 22 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 1455, Model.View.CreatedBy.ToString(), 1455, 32, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" id=\"view_created_by\" />\r\n    <input type=\"hidden\" name=\"View.VisionNumber\"");
                BeginWriteAttribute("value", " value=\"", 1563, "\"", 1606, 1);
#nullable restore
#line 23 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 1571, Model.View.VisionNumber.ToString(), 1571, 35, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" id=\"view_vision_number\" />\r\n    <input type=\"hidden\" name=\"View.RevisionStatus\"");
                BeginWriteAttribute("value", " value=\"", 1687, "\"", 1732, 1);
#nullable restore
#line 24 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 1695, Model.View.RevisionStatus.ToString(), 1695, 37, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" id=\"revision_status\" />\r\n    <input type=\"hidden\" name=\"View.User.UserId\"");
                BeginWriteAttribute("value", " value=\"", 1807, "\"", 1838, 1);
#nullable restore
#line 25 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 1815, Model.View.User.UserId, 1815, 23, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" id=\"view_userID\" />\r\n    <input type=\"hidden\" name=\"View.User.AspNetUserId\"");
                BeginWriteAttribute("value", " value=\"", 1915, "\"", 1952, 1);
#nullable restore
#line 26 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 1923, Model.View.User.AspNetUserId, 1923, 29, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" />\r\n    <input type=\"hidden\" name=\"View.User.UserName\"");
                BeginWriteAttribute("value", " value=\"", 2008, "\"", 2041, 1);
#nullable restore
#line 27 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 2016, Model.View.User.UserName, 2016, 25, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" id=\"view_user_name\" />\r\n    <input type=\"submit\" name=\"action\" class=\"button\" value=\"LastView\" id=\"bttn_sub\" />\r\n");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            BeginWriteTagHelperAttribute();
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __tagHelperExecutionContext.AddHtmlAttribute("hidden", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.Minimized);
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
            WriteLiteral(@"

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
                <a class=""menu-bar-dropdown-item"" onclick=""hidden_view_form_submit('LastView', 'view_created', this.id, 'created_col')"" id=""toggle_created"">? Created</a>
                <a class=""menu-bar-dropdown-item"" onclick=""hidden_view_for");
            WriteLiteral(@"m_submit('LastView', 'view_created_by', this.id, 'created_by_col')"" id=""toggle_created_by"">? Created By</a>
                <a class=""menu-bar-dropdown-item"" onclick=""hidden_view_form_submit('LastView', 'view_vision_number', this.id, 'vision_col')"" id=""toggle_vision_no"">? Vision No</a>
                <a class=""menu-bar-dropdown-item"" onclick=""hidden_view_form_submit('LastView', 'revision_status', this.id, 'revision_col')"" id=""toggle_revision"">? Revision</a>
            </div>
        </div>
        <div class=""menu-bar-dropdown"">
            <div class=""menu-bar-button-dropdown"">
                Groups ▼
            </div>
            <div class=""menu-bar-dropdown-content"">
                <a class=""menu-bar-dropdown-item"" onclick=""grouping_view_form_submit('LastView', 'view_set_value', this.id)"" id=""set_value_all"">All</a>
                <a class=""menu-bar-dropdown-item"" onclick=""grouping_view_form_submit('LastView', 'view_set_value', this.id)"" id=""set_value_active"">Active</a>
                <a");
            WriteLiteral(@" class=""menu-bar-dropdown-item"" onclick=""grouping_view_form_submit('LastView', 'view_set_value', this.id)"" id=""set_value_scrapped"">Scrapped</a>
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
    </div>
    <div class=""table-view-table-container"">
        <table>
            <tr>
                <th name=""title_col"" id=""Title"" onclick=""col_header_sort_change(this.id, 'view_sort_value', 'view_descending');""><a>Title</a></th>
                <th name=""last_modified_col"" id=""Last Modified"" onclick=""col_header_sort_change(this.id, 'view_sort_value', 'view_descending');""");
            WriteLiteral(@"><a>Last Modified</a></th>
                <th name=""last_modified_by_col"" id=""Last Modified By"" onclick=""col_header_sort_change(this.id, 'view_sort_value', 'view_descending');""><a>Last Modified By</a></th>
                <th name=""created_col"" id=""Created"" onclick=""col_header_sort_change(this.id, 'view_sort_value', 'view_descending');""><a>Created</a></th>
                <th name=""created_by_col"" id=""Created By"" onclick=""col_header_sort_change(this.id, 'view_sort_value', 'view_descending');""><a>Created By</a></th>
                <th name=""vision_col"" id =""Vision Number"" onclick=""col_header_sort_change(this.id, 'view_sort_value', 'view_descending');""><a>Vision Number</a></th>
                <th name=""revision_col"" id =""Revision"" onclick=""col_header_sort_change(this.id, 'view_sort_value', 'view_descending');""><a>Revision Status</a></th>
            </tr>

");
#nullable restore
#line 83 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
             foreach (var text in Model.TextHeaders)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <tr>\r\n                    <td><a");
            BeginWriteAttribute("href", " href=\"", 6230, "\"", 6290, 2);
            WriteAttributeValue("", 6237, "/RhymeBinder/EditText?textHeaderID=", 6237, 35, true);
#nullable restore
#line 86 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
WriteAttributeValue("", 6272, text.TextHeaderId, 6272, 18, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">");
#nullable restore
#line 86 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
                                                                                   Write(text.Title);

#line default
#line hidden
#nullable disable
            WriteLiteral("</a></td>\r\n                    <td name=\"last_modified_col\">");
#nullable restore
#line 87 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
                                            Write(text.LastModified);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td name=\"last_modified_by_col\">");
#nullable restore
#line 88 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
                                               Write(text.ModifyByName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td name=\"created_col\">");
#nullable restore
#line 89 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
                                      Write(text.Created);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td name=\"created_by_col\">");
#nullable restore
#line 90 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
                                         Write(text.CreatedByName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td name=\"vision_col\">");
#nullable restore
#line 91 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
                                     Write(text.VisionNumber);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td name=\"revision_col\">");
#nullable restore
#line 92 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"
                                       Write(text.RevisionStatus);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                </tr>\r\n");
#nullable restore
#line 94 "C:\Users\millr\source\repos\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListTexts.cshtml"

            }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </table>\r\n    </div>\r\n</div>\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "3258d727b96c06bb8045261fd7d273638d86779c23449", async() => {
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
