#pragma checksum "C:\Users\Ibrahim Mansour\source\repos\E-CommerceAPI\AdminPanel\Views\Order\Details.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3764aef144f8843862b7011c4d0cb265eb87609b"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Order_Details), @"mvc.1.0.view", @"/Views/Order/Details.cshtml")]
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
#line 1 "C:\Users\Ibrahim Mansour\source\repos\E-CommerceAPI\AdminPanel\Views\_ViewImports.cshtml"
using AdminPanel;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Ibrahim Mansour\source\repos\E-CommerceAPI\AdminPanel\Views\_ViewImports.cshtml"
using AdminPanel.Models;

#line default
#line hidden
#nullable disable
#nullable restore
#line 8 "C:\Users\Ibrahim Mansour\source\repos\E-CommerceAPI\AdminPanel\Views\Order\Details.cshtml"
using ViewModels.Order;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3764aef144f8843862b7011c4d0cb265eb87609b", @"/Views/Order/Details.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"88cb09a286142a0af4f033f761ef1537daca5904", @"/Views/_ViewImports.cshtml")]
    public class Views_Order_Details : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<GetOrderViewModel>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "C:\Users\Ibrahim Mansour\source\repos\E-CommerceAPI\AdminPanel\Views\Order\Details.cshtml"
  
    ViewData["Title"] = "Details";
    Layout = "~/Views/Shared/_Layout.cshtml";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n");
            WriteLiteral(@"
<div class=""m-auto"">

    <div class=""container  "">
        <div class=""row"">
            <div class=""col-12"">
                <table class=""w-75 m-auto"">
                    <tr >
                        <td><label for=""CustomerName"" class=""form-label p-1"">Customer Name :  </label></td>
                        <td><input type=""text"" readonly id=""CustomerName""");
            BeginWriteAttribute("value", " value=\"", 519, "\"", 540, 1);
#nullable restore
#line 19 "C:\Users\Ibrahim Mansour\source\repos\E-CommerceAPI\AdminPanel\Views\Order\Details.cshtml"
WriteAttributeValue("", 527, ViewBag.Name, 527, 13, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"form-control\"></td>\r\n                    </tr>\r\n                    <tr >\r\n                        <td><label for=\"Status\" class=\"form-label p-1\">Status :  </label></td>\r\n                        <td><input type=\"text\" readonly id=\"Status\"");
            BeginWriteAttribute("value", " value=\"", 787, "\"", 808, 1);
#nullable restore
#line 23 "C:\Users\Ibrahim Mansour\source\repos\E-CommerceAPI\AdminPanel\Views\Order\Details.cshtml"
WriteAttributeValue("", 795, Model.Status, 795, 13, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" class=\"form-control\"></td>\r\n                    </tr>\r\n                    <tr >\r\n                        <td><label for=\"Date\" class=\"form-label p-1\">Date :  </label></td>\r\n                        <td><input type=\"text\" readonly id=\"Date\"");
            BeginWriteAttribute("value", " value=\"", 1049, "\"", 1073, 1);
#nullable restore
#line 27 "C:\Users\Ibrahim Mansour\source\repos\E-CommerceAPI\AdminPanel\Views\Order\Details.cshtml"
WriteAttributeValue("", 1057, Model.OrderDate, 1057, 16, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@" class=""form-control""></td>
                    </tr>
                    <tr >
                        <td><label for=""TotalPrice"" class=""form-label p-1"">TotalPrice :  </label></td>
                        <td><input type=""text"" readonly id=""TotalPrice""");
            BeginWriteAttribute("value", " value=\"", 1332, "\"", 1357, 1);
#nullable restore
#line 31 "C:\Users\Ibrahim Mansour\source\repos\E-CommerceAPI\AdminPanel\Views\Order\Details.cshtml"
WriteAttributeValue("", 1340, Model.TotalPrice, 1340, 17, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(@" class=""form-control""></td>
                    </tr>
                    <tr class=""mt-5"">
                        <td colspan=""2"" >
                            <table class=""table table-dark"">
                                <tr>
                                    <th>Product</th>
                                    <th>Amout</th>
                                    <th>Price</th>
                                </tr>
");
#nullable restore
#line 41 "C:\Users\Ibrahim Mansour\source\repos\E-CommerceAPI\AdminPanel\Views\Order\Details.cshtml"
                                 foreach (var item in Model.Items)
                                {

#line default
#line hidden
#nullable disable
            WriteLiteral("                                    <tr>\r\n                                        <td>");
#nullable restore
#line 44 "C:\Users\Ibrahim Mansour\source\repos\E-CommerceAPI\AdminPanel\Views\Order\Details.cshtml"
                                       Write(item.ProductName);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                        <td>");
#nullable restore
#line 45 "C:\Users\Ibrahim Mansour\source\repos\E-CommerceAPI\AdminPanel\Views\Order\Details.cshtml"
                                       Write(item.Amount);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                        <td>");
#nullable restore
#line 46 "C:\Users\Ibrahim Mansour\source\repos\E-CommerceAPI\AdminPanel\Views\Order\Details.cshtml"
                                       Write(item.Price);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                                    </tr>\r\n");
#nullable restore
#line 48 "C:\Users\Ibrahim Mansour\source\repos\E-CommerceAPI\AdminPanel\Views\Order\Details.cshtml"
                                }

#line default
#line hidden
#nullable disable
            WriteLiteral("                            </table>\r\n                        </td>\r\n                    </tr>                \r\n\r\n                </table>\r\n            </div>\r\n        </div>\r\n    </div>\r\n</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<GetOrderViewModel> Html { get; private set; }
    }
}
#pragma warning restore 1591
