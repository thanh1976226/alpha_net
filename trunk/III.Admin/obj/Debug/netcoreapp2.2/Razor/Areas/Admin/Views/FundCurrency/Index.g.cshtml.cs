#pragma checksum "C:\Users\Administrator\Desktop\Project_Net_Alpha\Project_Net_Alpha\trunk\III.Admin\Areas\Admin\Views\FundCurrency\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "66f3a3d19fb47c232f7c49587fb39b9cb0422b8b"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Areas_Admin_Views_FundCurrency_Index), @"mvc.1.0.view", @"/Areas/Admin/Views/FundCurrency/Index.cshtml")]
[assembly:global::Microsoft.AspNetCore.Mvc.Razor.Compilation.RazorViewAttribute(@"/Areas/Admin/Views/FundCurrency/Index.cshtml", typeof(AspNetCore.Areas_Admin_Views_FundCurrency_Index))]
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
#line 1 "C:\Users\Administrator\Desktop\Project_Net_Alpha\Project_Net_Alpha\trunk\III.Admin\Areas\Admin\Views\FundCurrency\Index.cshtml"
using ESEIM.Utils;

#line default
#line hidden
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"66f3a3d19fb47c232f7c49587fb39b9cb0422b8b", @"/Areas/Admin/Views/FundCurrency/Index.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"baab20c8b3ee70a393938928849dc6b0f0db8ab8", @"/Areas/Admin/Views/_ViewImports.cshtml")]
    public class Areas_Admin_Views_FundCurrency_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/views/admin/fundCurrency/controller.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", new global::Microsoft.AspNetCore.Html.HtmlString("text/javascript"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
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
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            BeginContext(20, 2, true);
            WriteLiteral("\r\n");
            EndContext();
#line 3 "C:\Users\Administrator\Desktop\Project_Net_Alpha\Project_Net_Alpha\trunk\III.Admin\Areas\Admin\Views\FundCurrency\Index.cshtml"
  
    ViewData["Title"] = "Quản lý loại tiền tệ";
    Layout = "~/Areas/Admin/Views/Shared/_Layout_Admin.cshtml";

#line default
#line hidden
            DefineSection("plugin_script", async() => {
                BeginContext(167, 367, true);
                WriteLiteral(@"
    <script src=""../../lib/assets/global/plugins/angularjs-datatable/js/polyline.js""></script>
    <script src=""../../lib/assets/global/plugins/angularjs-datatable/js/render_view_ol3.js""></script>
    <script src=""https://maps.googleapis.com/maps/api/js?key=AIzaSyAn-5Fd7KH4e78m1X7SNj5gayFcJKDoUow&libraries=places""
            type=""text/javascript""></script>
");
                EndContext();
            }
            );
            BeginContext(537, 104, true);
            WriteLiteral("<div id=\"contentMain\" ng-app=\"App_ESEIM\" ng-controller=\"Ctrl_ESEIM\">\r\n    <div  ng-view></div>\r\n</div>\r\n");
            EndContext();
            BeginContext(641, 87, false);
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "66f3a3d19fb47c232f7c49587fb39b9cb0422b8b5464", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            EndContext();
            BeginContext(728, 2, true);
            WriteLiteral("\r\n");
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
