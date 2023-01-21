#pragma checksum "C:\Users\millr\source\repos\RhymeBinder_CleanBuild\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListBinders.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "2347b83edcbdb7aa62177e6b254f00ceea386bb4"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_RhymeBinder_ListBinders), @"mvc.1.0.view", @"/Views/RhymeBinder/ListBinders.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"2347b83edcbdb7aa62177e6b254f00ceea386bb4", @"/Views/RhymeBinder/ListBinders.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"09a45f068354f87046b22aebe54e24a87c312eff", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    public class Views_RhymeBinder_ListBinders : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<DisplayBinder>>
    #nullable disable
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 1 "C:\Users\millr\source\repos\RhymeBinder_CleanBuild\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListBinders.cshtml"
  
    ViewData["Title"] = "RhymeBinder: List Binders";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
#nullable restore
#line 8 "C:\Users\millr\source\repos\RhymeBinder_CleanBuild\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListBinders.cshtml"
   int index = 0; 

#line default
#line hidden
#nullable disable
            WriteLiteral(@"

    <div class=""table-view-wrapper"">
        <div class=""list-texts-menu-bar-main"">
            <div class=""menu-bar-title"" id=""menu_bar_title"">
                Your Binders:
            </div>

            <div class=""menu-bar-dropdown"">
                <div class=""menu-bar-button-dropdown"">
                    Actions ▼
                </div>
                <div class=""menu-bar-dropdown-content"">
                    <a class=""menu-bar-dropdown-item"" href=""/RhymeBinder/CreateBinder"">New Binder</a>
                 </div>
            </div>
        </div>

        <div class=""table-view-table-container"">
            <table>
                <tr>
                    <th name=""selected_col"" id=""Selected"" align=""center"">Current</th>
                    <th name=""name_col"" id=""Name"" align=""center""><a>Name</a></th>
                    <th name=""text_count_col"" id=""Text Count"" align=""center""><a>Text Count</a></th>
                    <th name=""group_count_col"" id=""Group Count"" align=""cente");
            WriteLiteral(@"r""><a>Group Count</a></th>
                    <th name=""last_modified_col"" id=""Last Modified"" align=""center""><a>Last Modified</a></th>
                    <th name=""last_modified_by_col"" id=""Last Modified By"" align=""center"" ><a>Last Modified By</a></th>
                    <th name=""created_col"" id=""Created"" align=""center""><a>Created</a></th>
                    <th name=""created_by_col"" id=""Created By"" align=""center""><a>Created By</a></th>
                    <th name=""description_col"" id=""Description"" align=""center""><a>Description</a></th>
                </tr>

");
#nullable restore
#line 41 "C:\Users\millr\source\repos\RhymeBinder_CleanBuild\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListBinders.cshtml"
                 foreach (var binder in Model)
                {


#line default
#line hidden
#nullable disable
            WriteLiteral("                <tr>\r\n\r\n");
#nullable restore
#line 46 "C:\Users\millr\source\repos\RhymeBinder_CleanBuild\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListBinders.cshtml"
                     if (binder.Name != "Trash" && binder.Name != "Loose Pages")
                    {
                        

#line default
#line hidden
#nullable disable
#nullable restore
#line 48 "C:\Users\millr\source\repos\RhymeBinder_CleanBuild\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListBinders.cshtml"
                         if (binder.Selected == true)
                            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                <td><a");
            BeginWriteAttribute("href", " href=\"", 2046, "\"", 2102, 2);
            WriteAttributeValue("", 2053, "/RhymeBinder/EditBinder?binderID=", 2053, 33, true);
#nullable restore
#line 50 "C:\Users\millr\source\repos\RhymeBinder_CleanBuild\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListBinders.cshtml"
WriteAttributeValue("", 2086, binder.BinderId, 2086, 16, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">Manage </a>\r\n                                    ► \r\n                                </td>\r\n");
#nullable restore
#line 53 "C:\Users\millr\source\repos\RhymeBinder_CleanBuild\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListBinders.cshtml"
                            }
                            else{

#line default
#line hidden
#nullable disable
            WriteLiteral("                                <td><a");
            BeginWriteAttribute("href", " href=\"", 2300, "\"", 2356, 2);
            WriteAttributeValue("", 2307, "/RhymeBinder/EditBinder?binderID=", 2307, 33, true);
#nullable restore
#line 55 "C:\Users\millr\source\repos\RhymeBinder_CleanBuild\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListBinders.cshtml"
WriteAttributeValue("", 2340, binder.BinderId, 2340, 16, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">Manage </a>\r\n\r\n                                </td>\r\n");
#nullable restore
#line 58 "C:\Users\millr\source\repos\RhymeBinder_CleanBuild\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListBinders.cshtml"
                        }

#line default
#line hidden
#nullable disable
#nullable restore
#line 58 "C:\Users\millr\source\repos\RhymeBinder_CleanBuild\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListBinders.cshtml"
                         

                    }
                    else
                    {

#line default
#line hidden
#nullable disable
            WriteLiteral("                        <td></td>\r\n");
#nullable restore
#line 64 "C:\Users\millr\source\repos\RhymeBinder_CleanBuild\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListBinders.cshtml"
                    }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                        <td><a");
            BeginWriteAttribute("href", " href=\"", 2603, "\"", 2659, 2);
            WriteAttributeValue("", 2610, "/RhymeBinder/OpenBinder?binderID=", 2610, 33, true);
#nullable restore
#line 66 "C:\Users\millr\source\repos\RhymeBinder_CleanBuild\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListBinders.cshtml"
WriteAttributeValue("", 2643, binder.BinderId, 2643, 16, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(">");
#nullable restore
#line 66 "C:\Users\millr\source\repos\RhymeBinder_CleanBuild\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListBinders.cshtml"
                                                                                   Write(binder.Name);

#line default
#line hidden
#nullable disable
            WriteLiteral("</a></td>\r\n                        <td name=\"text_count_col\" align=\"center\">");
#nullable restore
#line 67 "C:\Users\millr\source\repos\RhymeBinder_CleanBuild\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListBinders.cshtml"
                                                            Write(binder.PageCount);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                        <td name=\"group_count_col\" align=\"center\">");
#nullable restore
#line 68 "C:\Users\millr\source\repos\RhymeBinder_CleanBuild\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListBinders.cshtml"
                                                             Write(binder.GroupCount);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                        <td name=\"last_modified_col \"align=\"center\">");
#nullable restore
#line 69 "C:\Users\millr\source\repos\RhymeBinder_CleanBuild\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListBinders.cshtml"
                                                               Write(binder.LastModified);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                        <td name=\"last_modified_by_col\" align=\"center\">");
#nullable restore
#line 70 "C:\Users\millr\source\repos\RhymeBinder_CleanBuild\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListBinders.cshtml"
                                                                  Write(binder.LastModifiedBy);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                        <td name=\"created_col\" align=\"center\">");
#nullable restore
#line 71 "C:\Users\millr\source\repos\RhymeBinder_CleanBuild\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListBinders.cshtml"
                                                         Write(binder.Created);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                        <td name=\"created_by_col\" align=\"center\">");
#nullable restore
#line 72 "C:\Users\millr\source\repos\RhymeBinder_CleanBuild\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListBinders.cshtml"
                                                            Write(binder.CreatedBy);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                        <td name=\"description_col\">");
#nullable restore
#line 73 "C:\Users\millr\source\repos\RhymeBinder_CleanBuild\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListBinders.cshtml"
                                              Write(binder.Description);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    </tr>\r\n");
#nullable restore
#line 75 "C:\Users\millr\source\repos\RhymeBinder_CleanBuild\RhymeBinder\RhymeBinder\Views\RhymeBinder\ListBinders.cshtml"
                }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </table>\r\n        </div>\r\n    </div>\r\n\r\n\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<DisplayBinder>> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
