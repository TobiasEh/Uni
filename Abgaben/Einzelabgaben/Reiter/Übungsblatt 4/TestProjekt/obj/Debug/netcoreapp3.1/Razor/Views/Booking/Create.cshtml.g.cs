#pragma checksum "G:\Uni Git\tutorium-d-team-17\Abgaben\Einzelabgaben\Reiter\Übungsblatt 4\TestProjekt\Views\Booking\Create.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "3a9965c6da2ae54999a0b85f9be887a3571738bc"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Booking_Create), @"mvc.1.0.view", @"/Views/Booking/Create.cshtml")]
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
#line 1 "G:\Uni Git\tutorium-d-team-17\Abgaben\Einzelabgaben\Reiter\Übungsblatt 4\TestProjekt\Views\_ViewImports.cshtml"
using TestProjekt;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "G:\Uni Git\tutorium-d-team-17\Abgaben\Einzelabgaben\Reiter\Übungsblatt 4\TestProjekt\Views\_ViewImports.cshtml"
using TestProjekt.Models;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"3a9965c6da2ae54999a0b85f9be887a3571738bc", @"/Views/Booking/Create.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"4c69b4715194f03c7bdfe8019551deb50cc11e96", @"/Views/_ViewImports.cshtml")]
    public class Views_Booking_Create : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-controller", "Booking", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 2 "G:\Uni Git\tutorium-d-team-17\Abgaben\Einzelabgaben\Reiter\Übungsblatt 4\TestProjekt\Views\Booking\Create.cshtml"
  
    ViewData["Title"] = "Create";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n<p class=\"h3\">Neue Buchung</p>\r\n<div>\r\n    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "3a9965c6da2ae54999a0b85f9be887a3571738bc4542", async() => {
                WriteLiteral("\r\n        <div class=\"form-group\">\r\n            <label for=\"in_currentCharge\">Ladestand (aktuell)</label>\r\n            <input type=\"number\" class=\"form-control\"");
                BeginWriteAttribute("id", " id=\"", 312, "\"", 337, 1);
#nullable restore
#line 11 "G:\Uni Git\tutorium-d-team-17\Abgaben\Einzelabgaben\Reiter\Übungsblatt 4\TestProjekt\Views\Booking\Create.cshtml"
WriteAttributeValue("", 317, Model.currentCharge, 317, 20, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(@" placeholder=""Current charge in %"" name=""currentCharge"" min=""0"" max=""100"" required>
        </div>
        <div class=""form-group"">
            <label for=""in_requiredDistance"">Anzahl benötigter KM</label>
            <input type=""number"" class=""form-control""");
                BeginWriteAttribute("id", " id=\"", 601, "\"", 629, 1);
#nullable restore
#line 15 "G:\Uni Git\tutorium-d-team-17\Abgaben\Einzelabgaben\Reiter\Übungsblatt 4\TestProjekt\Views\Booking\Create.cshtml"
WriteAttributeValue("", 606, Model.requiredDistance, 606, 23, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(@" placeholder=""Required distance in km"" name=""requiredDistance"" min=""1"" max=""1000"" required>
        </div>
        <div class=""form-group input-append date form_datetime"">
            <label for=""in_start"">Startzeitpunkt</label>
            <input type=""datetime-local"" class=""form-control""");
                BeginWriteAttribute("id", " id=\"", 924, "\"", 941, 1);
#nullable restore
#line 19 "G:\Uni Git\tutorium-d-team-17\Abgaben\Einzelabgaben\Reiter\Übungsblatt 4\TestProjekt\Views\Booking\Create.cshtml"
WriteAttributeValue("", 929, Model.start, 929, 12, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" name=\"start\" placeholder=\"Start timestamp\" required >\r\n        </div>\r\n        <div class=\"form-group input-append date form_datetime\">\r\n            <label for=\"in_end\">Endezeitpunkt</label>\r\n            <input type=\"datetime-local\" class=\"form-control\"");
                BeginWriteAttribute("id", " id=\"", 1196, "\"", 1211, 1);
#nullable restore
#line 23 "G:\Uni Git\tutorium-d-team-17\Abgaben\Einzelabgaben\Reiter\Übungsblatt 4\TestProjekt\Views\Booking\Create.cshtml"
WriteAttributeValue("", 1201, Model.end, 1201, 10, false);

#line default
#line hidden
#nullable disable
                EndWriteAttribute();
                WriteLiteral(" name=\"end\" placeholder=\"End timestamp\" required>\r\n        </div>\r\n\r\n        <button type=\"submit\" class=\"btn btn-primary\">Buchung erstellen</button>\r\n    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Controller = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Method = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n\r\n</div>");
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
