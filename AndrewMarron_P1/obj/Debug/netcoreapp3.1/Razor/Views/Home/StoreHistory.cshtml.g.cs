#pragma checksum "C:\Users\ajmarron\Desktop\Revature\repoDev\AndrewMarron\RevatureP1\Views\Home\StoreHistory.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "02c37fdf37142786e72fd40ee1d4ac4c217d3baa"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Home_StoreHistory), @"mvc.1.0.view", @"/Views/Home/StoreHistory.cshtml")]
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
#line 1 "C:\Users\ajmarron\Desktop\Revature\repoDev\AndrewMarron\RevatureP1\Views\_ViewImports.cshtml"
using RevatureP1;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\ajmarron\Desktop\Revature\repoDev\AndrewMarron\RevatureP1\Views\_ViewImports.cshtml"
using RevatureP1.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"02c37fdf37142786e72fd40ee1d4ac4c217d3baa", @"/Views/Home/StoreHistory.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f565dcdcde1a0045080f19d0b46ca871d629d2d9", @"/Views/_ViewImports.cshtml")]
    public class Views_Home_StoreHistory : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<RevatureP1.Models.OrderItemViewModel>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#nullable restore
#line 5 "C:\Users\ajmarron\Desktop\Revature\repoDev\AndrewMarron\RevatureP1\Views\Home\StoreHistory.cshtml"
   ViewData["Title"] = "Store History"; 

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<div class=\"borderdiv\">\r\n    <h1>Order History for ");
#nullable restore
#line 8 "C:\Users\ajmarron\Desktop\Revature\repoDev\AndrewMarron\RevatureP1\Views\Home\StoreHistory.cshtml"
                     Write(ViewData["storeaddress"]);

#line default
#line hidden
#nullable disable
            WriteLiteral("</h1>\r\n\r\n");
            WriteLiteral("    <table class=\"table\">\r\n        <thead>\r\n            <tr>\r\n                <th>\r\n                    Product Name\r\n");
            WriteLiteral(@"                </th>
                <th>
                    Number Ordered
                </th>
                <th>
                    Total Price
                </th>
                <th>
                    Ordered By
                </th>
                <th>
                    Ordered On
                </th>
                <th></th>
            </tr>
        </thead>
        <tbody>
");
#nullable restore
#line 36 "C:\Users\ajmarron\Desktop\Revature\repoDev\AndrewMarron\RevatureP1\Views\Home\StoreHistory.cshtml"
             foreach (var item in Model)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <tr>\r\n                    <td>\r\n                        ");
#nullable restore
#line 40 "C:\Users\ajmarron\Desktop\Revature\repoDev\AndrewMarron\RevatureP1\Views\Home\StoreHistory.cshtml"
                   Write(Html.DisplayFor(modelItem => item.ProductName));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td>\r\n                        ");
#nullable restore
#line 43 "C:\Users\ajmarron\Desktop\Revature\repoDev\AndrewMarron\RevatureP1\Views\Home\StoreHistory.cshtml"
                   Write(Html.DisplayFor(modelItem => item.OrderCount));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td>\r\n                        $");
#nullable restore
#line 46 "C:\Users\ajmarron\Desktop\Revature\repoDev\AndrewMarron\RevatureP1\Views\Home\StoreHistory.cshtml"
                    Write(Html.DisplayFor(modelItem => item.TotalPriceWhenOrdered));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td>\r\n                        ");
#nullable restore
#line 49 "C:\Users\ajmarron\Desktop\Revature\repoDev\AndrewMarron\RevatureP1\Views\Home\StoreHistory.cshtml"
                   Write(Html.DisplayFor(modelItem => item.CustomerName));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td>\r\n                        ");
#nullable restore
#line 52 "C:\Users\ajmarron\Desktop\Revature\repoDev\AndrewMarron\RevatureP1\Views\Home\StoreHistory.cshtml"
                   Write(Html.DisplayFor(modelItem => item.DateOrdered));

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                </tr>\r\n");
#nullable restore
#line 55 "C:\Users\ajmarron\Desktop\Revature\repoDev\AndrewMarron\RevatureP1\Views\Home\StoreHistory.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </tbody>\r\n    </table>\r\n");
#nullable restore
#line 58 "C:\Users\ajmarron\Desktop\Revature\repoDev\AndrewMarron\RevatureP1\Views\Home\StoreHistory.cshtml"
       if (Model.Count() == 0)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("            <h3>No Prior orders</h3> ");
#nullable restore
#line 60 "C:\Users\ajmarron\Desktop\Revature\repoDev\AndrewMarron\RevatureP1\Views\Home\StoreHistory.cshtml"
                                     } 

#line default
#line hidden
#nullable disable
            WriteLiteral("    <a class=\"btn btn-primary\" href=\"javascript:history.go(-1)\">Back</a>\r\n</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<RevatureP1.Models.OrderItemViewModel>> Html { get; private set; }
    }
}
#pragma warning restore 1591
