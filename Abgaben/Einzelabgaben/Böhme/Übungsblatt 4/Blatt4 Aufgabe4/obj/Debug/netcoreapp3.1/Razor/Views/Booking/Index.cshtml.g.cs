#pragma checksum "D:\MSV Projekte\Blatt4 Aufgabe4\Views\Booking\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "17f43d676989f2b036e3ffd7addd26f62054573a"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Booking_Index), @"mvc.1.0.view", @"/Views/Booking/Index.cshtml")]
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
#line 1 "D:\MSV Projekte\Blatt4 Aufgabe4\Views\_ViewImports.cshtml"
using Blatt3_Aufgabe4;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\MSV Projekte\Blatt4 Aufgabe4\Views\_ViewImports.cshtml"
using Blatt3_Aufgabe4.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"17f43d676989f2b036e3ffd7addd26f62054573a", @"/Views/Booking/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d0559f2165a21d4fac4f48e9fdfad1b08ea59c19", @"/Views/_ViewImports.cshtml")]
    public class Views_Booking_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Create", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "D:\MSV Projekte\Blatt4 Aufgabe4\Views\Booking\Index.cshtml"
  
    ViewData["Titel"] = "Booking";
    

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<h1>Buchungen</h1>\r\n<p style=\"display:inline\">Übersicht aller Buchugen:</p>\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "17f43d676989f2b036e3ffd7addd26f62054573a3624", async() => {
                WriteLiteral("<button class=\"btn btn-primary\" style=\"display:inline; float:right\">Neue Buchung anlegen</button>");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n");
#nullable restore
#line 10 "D:\MSV Projekte\Blatt4 Aufgabe4\Views\Booking\Index.cshtml"
 if (Model == null)
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <p>Noch keine Buchungen vorhanden!</p>\r\n");
#nullable restore
#line 13 "D:\MSV Projekte\Blatt4 Aufgabe4\Views\Booking\Index.cshtml"
}
else
{

#line default
#line hidden
#nullable disable
            WriteLiteral("<table class=\"table table-hover table-striped\">\r\n    <tr>\r\n        <th>Charge Status</th>\r\n        <th>Distance</th>\r\n        <th>Start Time</th>\r\n        <th>End Time</th>\r\n    </tr>\r\n\r\n\r\n");
#nullable restore
#line 24 "D:\MSV Projekte\Blatt4 Aufgabe4\Views\Booking\Index.cshtml"
     foreach (Booking item in Model)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <tr>\r\n            <td>\r\n                ");
#nullable restore
#line 28 "D:\MSV Projekte\Blatt4 Aufgabe4\Views\Booking\Index.cshtml"
           Write(item.chargeStatus);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n            </td>\r\n            <td>\r\n                ");
#nullable restore
#line 32 "D:\MSV Projekte\Blatt4 Aufgabe4\Views\Booking\Index.cshtml"
           Write(item.distance);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
#nullable restore
#line 35 "D:\MSV Projekte\Blatt4 Aufgabe4\Views\Booking\Index.cshtml"
           Write(item.startTime);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n            <td>\r\n                ");
#nullable restore
#line 38 "D:\MSV Projekte\Blatt4 Aufgabe4\Views\Booking\Index.cshtml"
           Write(item.endTime);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n            </td>\r\n        </tr>\r\n");
#nullable restore
#line 41 "D:\MSV Projekte\Blatt4 Aufgabe4\Views\Booking\Index.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</table>");
#nullable restore
#line 42 "D:\MSV Projekte\Blatt4 Aufgabe4\Views\Booking\Index.cshtml"
        }

#line default
#line hidden
#nullable disable
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
