#pragma checksum "D:\MSV Projekte\Blatt6 Aufgabe4\Views\Booking\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "9c7409eb493347dd275f20c85fe25ac14ea78adc"
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
#line 1 "D:\MSV Projekte\Blatt6 Aufgabe4\Views\_ViewImports.cshtml"
using Blatt3_Aufgabe4;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "D:\MSV Projekte\Blatt6 Aufgabe4\Views\_ViewImports.cshtml"
using Blatt3_Aufgabe4.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"9c7409eb493347dd275f20c85fe25ac14ea78adc", @"/Views/Booking/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"d0559f2165a21d4fac4f48e9fdfad1b08ea59c19", @"/Views/_ViewImports.cshtml")]
    public class Views_Booking_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/lib/jquery/dist/jquery.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Data", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "upload", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("enctype", new global::Microsoft.AspNetCore.Html.HtmlString("multipart/form-data"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "D:\MSV Projekte\Blatt6 Aufgabe4\Views\Booking\Index.cshtml"
  
    ViewData["Titel"] = "Booking";
    

#line default
#line hidden
#nullable disable
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "9c7409eb493347dd275f20c85fe25ac14ea78adc5138", async() => {
                WriteLiteral("$(document).ready(function () { });");
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n<div");
            BeginWriteAttribute("class", " class=\"", 141, "\"", 149, 0);
            EndWriteAttribute();
            WriteLiteral(">\r\n    <h1>Buchungen</h1>\r\n    <p style=\"display:inline\">Übersicht aller Buchugen:</p>\r\n    <input type=\"button\" class=\"btn btn-primary\" style=\"display:inline; float:right\" value=\"Neue Buchung anlegen\"");
            BeginWriteAttribute("onclick", " onclick=\"", 351, "\"", 409, 3);
            WriteAttributeValue("", 361, "location.href=\'", 361, 15, true);
#nullable restore
#line 9 "D:\MSV Projekte\Blatt6 Aufgabe4\Views\Booking\Index.cshtml"
WriteAttributeValue("", 376, Url.Action("Create", "Booking"), 376, 32, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 408, "\'", 408, 1, true);
            EndWriteAttribute();
            WriteLiteral(" />\r\n\r\n");
#nullable restore
#line 11 "D:\MSV Projekte\Blatt6 Aufgabe4\Views\Booking\Index.cshtml"
     if (Model == null)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <br />\r\n        <p>Noch keine Buchungen vorhanden!</p>\r\n");
#nullable restore
#line 15 "D:\MSV Projekte\Blatt6 Aufgabe4\Views\Booking\Index.cshtml"

    }
    else
    {

#line default
#line hidden
#nullable disable
            WriteLiteral(@"<table class=""table table-hover table-striped"">
            <tr>
                <th>Charge Status</th>
                <th>Distance</th>
                <th>Start Time</th>
                <th>End Time</th>
                <th>Connector Type</th>
            </tr>


");
#nullable restore
#line 28 "D:\MSV Projekte\Blatt6 Aufgabe4\Views\Booking\Index.cshtml"
             foreach (Booking item in Model)
            {

#line default
#line hidden
#nullable disable
            WriteLiteral("                <tr>\r\n                    <td>\r\n                        ");
#nullable restore
#line 32 "D:\MSV Projekte\Blatt6 Aufgabe4\Views\Booking\Index.cshtml"
                   Write(item.chargeStatus);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n\r\n                    </td>\r\n                    <td>\r\n                        ");
#nullable restore
#line 36 "D:\MSV Projekte\Blatt6 Aufgabe4\Views\Booking\Index.cshtml"
                   Write(item.distance);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td>\r\n                        ");
#nullable restore
#line 39 "D:\MSV Projekte\Blatt6 Aufgabe4\Views\Booking\Index.cshtml"
                   Write(item.startTime);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td>\r\n                        ");
#nullable restore
#line 42 "D:\MSV Projekte\Blatt6 Aufgabe4\Views\Booking\Index.cshtml"
                   Write(item.endTime);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                    <td>\r\n                        ");
#nullable restore
#line 45 "D:\MSV Projekte\Blatt6 Aufgabe4\Views\Booking\Index.cshtml"
                   Write(item.connectorType);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n                    </td>\r\n                </tr>\r\n");
#nullable restore
#line 48 "D:\MSV Projekte\Blatt6 Aufgabe4\Views\Booking\Index.cshtml"
            }

#line default
#line hidden
#nullable disable
            WriteLiteral("        </table>\r\n        <input type=\"button\" class=\"btn btn-primary\" value=\"Export\" style=\"display:inline; float:left\"");
            BeginWriteAttribute("onclick", " onclick=\"", 1532, "\"", 1622, 3);
            WriteAttributeValue("", 1542, "location.href=\'", 1542, 15, true);
#nullable restore
#line 50 "D:\MSV Projekte\Blatt6 Aufgabe4\Views\Booking\Index.cshtml"
WriteAttributeValue("", 1557, Url.Action("exportData", "Data", new { cacheKey = "bookings" }), 1557, 64, false);

#line default
#line hidden
#nullable disable
            WriteAttributeValue("", 1621, "\'", 1621, 1, true);
            EndWriteAttribute();
            WriteLiteral(" />\r\n");
#nullable restore
#line 51 "D:\MSV Projekte\Blatt6 Aufgabe4\Views\Booking\Index.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("    <button type=\"button\" class=\"btn btn-primary\" data-toggle=\"modal\" data-target=\"#import_modal\" style=\"display:inline; float:right\">Import</button>\r\n    \r\n");
#nullable restore
#line 54 "D:\MSV Projekte\Blatt6 Aufgabe4\Views\Booking\Index.cshtml"
     if (ViewBag.Message != null)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <div class=\"alert alert-success\">\r\n            <p>");
#nullable restore
#line 57 "D:\MSV Projekte\Blatt6 Aufgabe4\Views\Booking\Index.cshtml"
          Write(ViewBag.Message);

#line default
#line hidden
#nullable disable
            WriteLiteral("</p>\r\n        </div>\r\n");
#nullable restore
#line 59 "D:\MSV Projekte\Blatt6 Aufgabe4\Views\Booking\Index.cshtml"
    }

#line default
#line hidden
#nullable disable
#nullable restore
#line 60 "D:\MSV Projekte\Blatt6 Aufgabe4\Views\Booking\Index.cshtml"
     if (ViewBag.ErrorMessage != null)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <div class=\"alter alert-danger\" style=\"margin-top:20px\">\r\n            ");
#nullable restore
#line 63 "D:\MSV Projekte\Blatt6 Aufgabe4\Views\Booking\Index.cshtml"
       Write(ViewBag.ErrorMessage);

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </div>\r\n");
#nullable restore
#line 65 "D:\MSV Projekte\Blatt6 Aufgabe4\Views\Booking\Index.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
</div>

  <!--Modal-->  
  <div class=""modal fade"" id=""import_modal"" role=""dialog"" aria-labelledby=""exampleModalLabel"" aria-hidden=""true"">
      <div class=""modal-dialog"" role=""document"">
      <div class=""modal-content"">
          <div class=""modal-header"">
              <h2 class=""modal-title"" id=""import_modalLabel"">Select File to import</h2>
              <button txpe=""button"" class=""close"" data-dismiss=""modal"" aria-label=""Close"">
                  <span aria-hidden=""true"">&times;</span>
              </button>
          </div>
          ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "9c7409eb493347dd275f20c85fe25ac14ea78adc12831", async() => {
                WriteLiteral(@"
              <div class=""modal-body"">
                  <div class=""custom-file"">
                      <input type=""file"" class=""custom-file-input"" id=""file"" name=""file"" accept="".json"">
                      <label class=""custom-file-label"" for=""customFile"">Choose file to import</label>
                  </div>

              </div>
              <div class=""modal-footer"">
                  <button type=""button"" class=""btn btn-secondary"" data-dismiss=""modal"">Close</button>
                  <button type=""submit"" class=""btn btn-primary"">Import</button>

              </div>
          ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Controller = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_3.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_3);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"
      </div>
      </div>
  </div>  
  <script>

    $(""custom-file-lable"").on(""change"", function() {
        var filename2 = $(this).val().split(""\\"").pop();
        $(this).siblings("".custom-file-lable"").addClass(""selected"").html(filename2);
     });
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
