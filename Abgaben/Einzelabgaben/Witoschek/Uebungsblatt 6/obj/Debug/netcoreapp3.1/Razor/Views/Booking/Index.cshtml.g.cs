#pragma checksum "F:\Git\Softwareprojekt\tutorium-d-team-17\Abgaben\Einzelabgaben\Witoschek\Uebungsblatt 5\Views\Booking\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7c1ccd10fe4d9c3f2dc02ed8e4f7d96e791a58c4"
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
#line 1 "F:\Git\Softwareprojekt\tutorium-d-team-17\Abgaben\Einzelabgaben\Witoschek\Uebungsblatt 5\Views\_ViewImports.cshtml"
using Blatt03;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "F:\Git\Softwareprojekt\tutorium-d-team-17\Abgaben\Einzelabgaben\Witoschek\Uebungsblatt 5\Views\_ViewImports.cshtml"
using Blatt03.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7c1ccd10fe4d9c3f2dc02ed8e4f7d96e791a58c4", @"/Views/Booking/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"fc81e1eddc654ceebcbf0f513ebbcac455da1f82", @"/Views/_ViewImports.cshtml")]
    public class Views_Booking_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "F:\Git\Softwareprojekt\tutorium-d-team-17\Abgaben\Einzelabgaben\Witoschek\Uebungsblatt 5\Views\Booking\Index.cshtml"
  
    ViewData["Title"] = "Booking";

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
<div class=""d-flex"">
    <div>
        <p class=""h4 d-inline-block"">Übersicht aller Buchungen</p>
    </div>
    <div class=""ml-auto"">
        <input class=""float-right col btn btn-primary btn-lg"" type=""button"" name=""createBooking"" value=""Neue Buchung""");
            BeginWriteAttribute("onclick", " onclick=\"", 304, "\"", 362, 3);
            WriteAttributeValue("", 314, "location.href=\'", 314, 15, true);
#nullable restore
#line 11 "F:\Git\Softwareprojekt\tutorium-d-team-17\Abgaben\Einzelabgaben\Witoschek\Uebungsblatt 5\Views\Booking\Index.cshtml"
WriteAttributeValue("", 329, Url.Action("create", "Booking"), 329, 32, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 361, "\'", 361, 1, true);
            EndWriteAttribute();
            WriteLiteral(" />\r\n    </div>\r\n    \r\n</div>\r\n\r\n<br />\r\n\r\n");
#nullable restore
#line 18 "F:\Git\Softwareprojekt\tutorium-d-team-17\Abgaben\Einzelabgaben\Witoschek\Uebungsblatt 5\Views\Booking\Index.cshtml"
 if (Model != null)
{

#line default
#line hidden
#nullable disable
            WriteLiteral(@"    <div class=""table-responsive"">
        <table class=""table table-striped table-borderless table-hover"">
            <caption>List of bookings</caption>
            <thead class=""thead-dark"">
                <tr>
                    <th>Current Charge</th>
                    <th>Requested Distance in km</th>
                    <th>Start Time</th>
                    <th>End Time</th>
                    <th>Plug Type</th>
                </tr>

            </thead>

");
#nullable restore
#line 34 "F:\Git\Softwareprojekt\tutorium-d-team-17\Abgaben\Einzelabgaben\Witoschek\Uebungsblatt 5\Views\Booking\Index.cshtml"
             foreach (Booking booking in Model)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <tr>\r\n                    <td>");
#nullable restore
#line 37 "F:\Git\Softwareprojekt\tutorium-d-team-17\Abgaben\Einzelabgaben\Witoschek\Uebungsblatt 5\Views\Booking\Index.cshtml"
                   Write(booking.currentCharge);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 38 "F:\Git\Softwareprojekt\tutorium-d-team-17\Abgaben\Einzelabgaben\Witoschek\Uebungsblatt 5\Views\Booking\Index.cshtml"
                   Write(booking.requiredDistance);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 39 "F:\Git\Softwareprojekt\tutorium-d-team-17\Abgaben\Einzelabgaben\Witoschek\Uebungsblatt 5\Views\Booking\Index.cshtml"
                   Write(booking.start);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 40 "F:\Git\Softwareprojekt\tutorium-d-team-17\Abgaben\Einzelabgaben\Witoschek\Uebungsblatt 5\Views\Booking\Index.cshtml"
                   Write(booking.end);

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                    <td>");
#nullable restore
#line 41 "F:\Git\Softwareprojekt\tutorium-d-team-17\Abgaben\Einzelabgaben\Witoschek\Uebungsblatt 5\Views\Booking\Index.cshtml"
                   Write(booking.connectorType.Description());

#line default
#line hidden
#nullable disable
            WriteLiteral("</td>\r\n                </tr>\r\n");
#nullable restore
#line 43 "F:\Git\Softwareprojekt\tutorium-d-team-17\Abgaben\Einzelabgaben\Witoschek\Uebungsblatt 5\Views\Booking\Index.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </table>\r\n    </div>\r\n");
#nullable restore
#line 46 "F:\Git\Softwareprojekt\tutorium-d-team-17\Abgaben\Einzelabgaben\Witoschek\Uebungsblatt 5\Views\Booking\Index.cshtml"
}
else
{

#line default
#line hidden
#nullable disable
            WriteLiteral("    <p class=\"text-center col\">Noch keine Buchungen vorhanden!</p>\r\n");
#nullable restore
#line 50 "F:\Git\Softwareprojekt\tutorium-d-team-17\Abgaben\Einzelabgaben\Witoschek\Uebungsblatt 5\Views\Booking\Index.cshtml"
}

#line default
#line hidden
#nullable disable
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
