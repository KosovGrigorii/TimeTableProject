#pragma checksum "/Users/grigorijkosov/Documents/GitHub/TimeTableProject/Application/UserInterface/Views/Shared/_SingleSpecifiedFilter.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "8966b88fbc24f1be627fbf8a88dd6bac58d4aeab"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Views_Shared__SingleSpecifiedFilter), @"mvc.1.0.view", @"/Views/Shared/_SingleSpecifiedFilter.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"8966b88fbc24f1be627fbf8a88dd6bac58d4aeab", @"/Views/Shared/_SingleSpecifiedFilter.cshtml")]
    public class Views_Shared__SingleSpecifiedFilter : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<IEnumerable<string>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\n");
#nullable restore
#line 3 "/Users/grigorijkosov/Documents/GitHub/TimeTableProject/Application/UserInterface/Views/Shared/_SingleSpecifiedFilter.cshtml"
  
    var modelNameId = "filters[" + ViewBag.Index + "].Name";
    var modelHoursId = "filters[" + ViewBag.Index + "].Days";

#line default
#line hidden
#nullable disable
            WriteLiteral("\n<input list=\"filtered\" type=\"text\" placeholder=\"Name\"");
            BeginWriteAttribute("name", " name=\"", 210, "\"", 229, 1);
#nullable restore
#line 8 "/Users/grigorijkosov/Documents/GitHub/TimeTableProject/Application/UserInterface/Views/Shared/_SingleSpecifiedFilter.cshtml"
WriteAttributeValue("", 217, modelNameId, 217, 12, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("/>\n<datalist id=\"filtered\">\n");
#nullable restore
#line 10 "/Users/grigorijkosov/Documents/GitHub/TimeTableProject/Application/UserInterface/Views/Shared/_SingleSpecifiedFilter.cshtml"
     foreach (var item in Model)
    {

#line default
#line hidden
#nullable disable
            WriteLiteral("        <option");
            BeginWriteAttribute("value", " value=\"", 312, "\"", 325, 1);
#nullable restore
#line 12 "/Users/grigorijkosov/Documents/GitHub/TimeTableProject/Application/UserInterface/Views/Shared/_SingleSpecifiedFilter.cshtml"
WriteAttributeValue("", 320, item, 320, 5, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral("></option>\n");
#nullable restore
#line 13 "/Users/grigorijkosov/Documents/GitHub/TimeTableProject/Application/UserInterface/Views/Shared/_SingleSpecifiedFilter.cshtml"
    }

#line default
#line hidden
#nullable disable
            WriteLiteral("</datalist>\n<br>\n<input type=\"number\" step=\"1\" min=\"0\"");
            BeginWriteAttribute("name", " name=\"", 397, "\"", 417, 1);
#nullable restore
#line 16 "/Users/grigorijkosov/Documents/GitHub/TimeTableProject/Application/UserInterface/Views/Shared/_SingleSpecifiedFilter.cshtml"
WriteAttributeValue("", 404, modelHoursId, 404, 13, false);

#line default
#line hidden
#nullable disable
            EndWriteAttribute();
            WriteLiteral(" placeholder=\"Hours amount\"/>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<string>> Html { get; private set; }
    }
}
#pragma warning restore 1591
