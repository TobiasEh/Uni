#pragma checksum "F:\C#\Projekte\Blatt3\Blatt03\Views\Booking\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "28258bb83c4141c665bbe3fb4aded3ac2f6a368e"
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
#line 1 "F:\C#\Projekte\Blatt3\Blatt03\Views\_ViewImports.cshtml"
using Blatt03;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "F:\C#\Projekte\Blatt3\Blatt03\Views\_ViewImports.cshtml"
using Blatt03.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"28258bb83c4141c665bbe3fb4aded3ac2f6a368e", @"/Views/Booking/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fc81e1eddc654ceebcbf0f513ebbcac455da1f82", @"/Views/_ViewImports.cshtml")]
    public class Views_Booking_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "F:\C#\Projekte\Blatt3\Blatt03\Views\Booking\Index.cshtml"
  
    ViewData["Title"] = "Booking";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<h1 class=""display-4"">Bookings</h1>
<br/>
<div class=""table-responsive"">
    <table class=""table table-striped table-borderless table-hover"">
        <caption>List of bookings</caption>
        <thead class=""thead-dark"">
            <tr>
                <th>Current Charge</th>
                <th>Requested Distance in km</th>
                <th>Start Time</th>
                <th>End Time</th>
            </tr>

        </thead>
        
");
#nullable restore
#line 21 "F:\C#\Projekte\Blatt3\Blatt03\Views\Booking\Index.cshtml"
         foreach (Booking booking in Model)
        {

#line default
#line hidden
#nullable disable
            WriteLiteral("    <tr>\r\n        <td>");
#nullable restore
#line 24 "F:\C#\Projekte\Blatt3\Blatt03\Views\Booking\Index.cshtml"
       Write(booking.currentCharge);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n        <td>");
#nullable restore
#line 25 "F:\C#\Projekte\Blatt3\Blatt03\Views\Booking\Index.cshtml"
       Write(booking.requiredDistance);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n        <td>");
#nullable restore
#line 26 "F:\C#\Projekte\Blatt3\Blatt03\Views\Booking\Index.cshtml"
       Write(booking.start);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n        <td>");
#nullable restore
#line 27 "F:\C#\Projekte\Blatt3\Blatt03\Views\Booking\Index.cshtml"
       Write(booking.end);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n    </tr>\r\n");
#nullable restore
#line 29 "F:\C#\Projekte\Blatt3\Blatt03\Views\Booking\Index.cshtml"
        }

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    </table>\r\n</div>\r\n\r\n\r\n");
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
