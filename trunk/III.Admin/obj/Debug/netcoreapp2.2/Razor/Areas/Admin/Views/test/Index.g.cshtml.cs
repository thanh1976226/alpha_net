#pragma checksum "C:\Users\Administrator\Desktop\Project_Net_Alpha\Project_Net_Alpha\trunk\III.Admin\Areas\Admin\Views\test\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f70d072411291e42dde40d28d379b652a9194d86"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Admin_Views_test_Index), @"mvc.1.0.view", @"/Areas/Admin/Views/test/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Areas/Admin/Views/test/Index.cshtml", typeof(AspNetCore.Areas_Admin_Views_test_Index))]
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
#line 1 "C:\Users\Administrator\Desktop\Project_Net_Alpha\Project_Net_Alpha\trunk\III.Admin\Areas\Admin\Views\_ViewImports.cshtml"
using ESEIM;

#line default
#line hidden
#line 2 "C:\Users\Administrator\Desktop\Project_Net_Alpha\Project_Net_Alpha\trunk\III.Admin\Areas\Admin\Views\_ViewImports.cshtml"
using Hot.Models.AccountViewModels;

#line default
#line hidden
#line 1 "C:\Users\Administrator\Desktop\Project_Net_Alpha\Project_Net_Alpha\trunk\III.Admin\Areas\Admin\Views\test\Index.cshtml"
using ESEIM.Utils;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f70d072411291e42dde40d28d379b652a9194d86", @"/Areas/Admin/Views/test/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"baab20c8b3ee70a393938928849dc6b0f0db8ab8", @"/Areas/Admin/Views/_ViewImports.cshtml")]
    public class Areas_Admin_Views_test_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
#line 2 "C:\Users\Administrator\Desktop\Project_Net_Alpha\Project_Net_Alpha\trunk\III.Admin\Areas\Admin\Views\test\Index.cshtml"
  
    ViewData["Title"] = "Trình độ";
    Layout = "~/Areas/Admin/Views/Shared/_Layout_Admin.cshtml";

#line default
#line hidden
            BeginContext(129, 343, true);
            WriteLiteral(@"
<style>
    /*th {
        background: linear-gradient(#04A3ED,rgba(21,73,155,1)) !important;
        color: white;
    }*/
</style>
<div id=""contentMain"" ng-app=""App_ESEIM"" ng-controller=""Ctrl_ESEIM"">
    <div style=""min-height:100px"" ng-view></div>
</div>
<script src=""/views/test/controller.js"" type=""text/javascript""></script>
");
            EndContext();
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
