#pragma checksum "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Usuario\_AutorizarEmpresa.cshtml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "10aaacc1e4484160aa02b0e4deee8d52ef48c0055ef76b0c4835d65675539fad"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCoreGeneratedDocument.Views_Usuario__AutorizarEmpresa), @"mvc.1.0.view", @"/Views/Usuario/_AutorizarEmpresa.cshtml")]
namespace AspNetCoreGeneratedDocument
{
    #line default
    using global::System;
    using global::System.Collections.Generic;
    using global::System.Linq;
    using global::System.Threading.Tasks;
    using global::Microsoft.AspNetCore.Mvc;
    using global::Microsoft.AspNetCore.Mvc.Rendering;
    using global::Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\_ViewImports.cshtml"
using agilium.webapp.manager.mvc

#nullable disable
    ;
#nullable restore
#line 2 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\_ViewImports.cshtml"
using agilium.webapp.manager.mvc.Models

#line default
#line hidden
#nullable disable
    ;
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"Sha256", @"10aaacc1e4484160aa02b0e4deee8d52ef48c0055ef76b0c4835d65675539fad", @"/Views/Usuario/_AutorizarEmpresa.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"Sha256", @"f8451ba42770157f1c112c708bc4dea4ed0e6fd8050564d03155ec11226e2f5d", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    internal sealed class Views_Usuario__AutorizarEmpresa : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<
#nullable restore
#line 1 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Usuario\_AutorizarEmpresa.cshtml"
       agilium.webapp.manager.mvc.ViewModels.EmpresasAssociadasViewModel

#line default
#line hidden
#nullable disable
    >
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", "hidden", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", "checkbox", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "AssociarEmpresa", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("method", "post", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("formEmpresaAuth"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.LabelTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Usuario\_AutorizarEmpresa.cshtml"
  
    Layout = null;
    var inclusao = ViewBag.operacao == "I";
    var operacao = "Autorização de Empresa";
    ViewData["Title"] = operacao;

#line default
#line hidden
#nullable disable

            WriteLiteral("\r\n\r\n<div class=\"modal-header\">\r\n    <h5 class=\"modal-title\">");
            Write(
#nullable restore
#line 12 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Usuario\_AutorizarEmpresa.cshtml"
                             ViewData["Title"]

#line default
#line hidden
#nullable disable
            );
            WriteLiteral("</h5>\r\n    <button type=\"button\" class=\"close\" data-dismiss=\"modal\">\r\n        <span aria-hidden=\"true\">×</span><span class=\"sr-only\">Fechar</span>\r\n    </button>\r\n</div>\r\n\r\n<div class=\"row\">\r\n    <div class=\"col-md-12\">\r\n        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "10aaacc1e4484160aa02b0e4deee8d52ef48c0055ef76b0c4835d65675539fad6570", async() => {
                WriteLiteral(@"
            <vc:Summary></vc:Summary>
            <div class=""card card-success"">
                <div class=""card-header"">
                    <h3 class=""card-title"">Empresas Associadas</h3>
                </div>
                <div class=""card-body"">
                    <div class=""callout callout-success"">
                        <h5><i class=""fas fa-user mr-1""></i> ");
                Write(
#nullable restore
#line 28 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Usuario\_AutorizarEmpresa.cshtml"
                                                              Model.UsuarioSelecionado

#line default
#line hidden
#nullable disable
                );
                WriteLiteral("</h5>\r\n                    </div>\r\n");
#nullable restore
#line 30 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Usuario\_AutorizarEmpresa.cshtml"
                     if (!Model.EmpresasSelecao.Any())
                    {

#line default
#line hidden
#nullable disable

                WriteLiteral("                        <h5 class=\"text-danger\">Não existe(m) Empresa(s) cadastrada(s)</h5>\r\n");
#nullable restore
#line 33 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Usuario\_AutorizarEmpresa.cshtml"
                    }
                    else
                    {
                        for (int i = 0; i < Model.EmpresasSelecao.Count(); i++)
                        {

#line default
#line hidden
#nullable disable

                WriteLiteral("                            <div class=\"form-group clearfix\">\r\n                                <div class=\"icheck-primary d-inline\">\r\n                                    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "10aaacc1e4484160aa02b0e4deee8d52ef48c0055ef76b0c4835d65675539fad8589", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_0.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => 
#nullable restore
#line 40 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Usuario\_AutorizarEmpresa.cshtml"
                                                                   Model.EmpresasSelecao[i].IDEMPRESA

#line default
#line hidden
#nullable disable
                );
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n                                    ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "10aaacc1e4484160aa02b0e4deee8d52ef48c0055ef76b0c4835d65675539fad10489", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_0.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => 
#nullable restore
#line 41 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Usuario\_AutorizarEmpresa.cshtml"
                                                                   Model.EmpresasSelecao[i].IDUSUARIO

#line default
#line hidden
#nullable disable
                );
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n\r\n                                    <div class=\"form-group clearfix\">\r\n                                        <div class=\"icheck-primary d-inline\">\r\n                                            ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagOnly, "10aaacc1e4484160aa02b0e4deee8d52ef48c0055ef76b0c4835d65675539fad12560", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_1.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => 
#nullable restore
#line 45 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Usuario\_AutorizarEmpresa.cshtml"
                                                                             Model.EmpresasSelecao[i].Selecionado

#line default
#line hidden
#nullable disable
                );
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n                                            ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("label", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "10aaacc1e4484160aa02b0e4deee8d52ef48c0055ef76b0c4835d65675539fad14482", async() => {
                    WriteLiteral("\r\n                                                ");
                    Write(
#nullable restore
#line 47 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Usuario\_AutorizarEmpresa.cshtml"
                                                 Model.EmpresasSelecao[i].NomeEmpresa

#line default
#line hidden
#nullable disable
                    );
                    WriteLiteral("\r\n                                            ");
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.LabelTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => 
#nullable restore
#line 46 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Usuario\_AutorizarEmpresa.cshtml"
                                                             Model.EmpresasSelecao[i].Selecionado

#line default
#line hidden
#nullable disable
                );
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n                                        </div>\r\n                                    </div>\r\n                                </div>\r\n                            </div>\r\n");
#nullable restore
#line 53 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Usuario\_AutorizarEmpresa.cshtml"
                        }
                    }

#line default
#line hidden
#nullable disable

                WriteLiteral("\r\n\r\n");
                WriteLiteral(@"
                    <div class=""form-group"">
                        <input type=""submit"" value=""Salvar"" class=""btn btn-primary"" style=""visibility:hidden"" id=""btnSendForm"" />
                    </div>

                </div>
            </div>
            <div class=""modal-footer justify-content-end"">

                <a class=""nav-link btn btn-outline-danger"" data-dismiss=""modal"" aria-label=""Close"" id=""fechar"" title=""Cancelar operação"">
                    <i class=""fas fa-window-close""></i>
                </a>

                <button type=""submit"" class=""nav-link btn btn-outline-success"" id=""btnSalvarModal"" title=""Confirmar operação"">
                    <i class=""fas fa-save""></i>
                </button>
            </div>
        ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.Action = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-returnurl", "Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
            WriteLiteral(
#nullable restore
#line 20 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Usuario\_AutorizarEmpresa.cshtml"
                                                                 ViewData["ReturnUrl"]

#line default
#line hidden
#nullable disable
            );
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.RouteValues["returnurl"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-returnurl", __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper.RouteValues["returnurl"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
            WriteLiteral("\r\n    </div>\r\n</div>\r\n<script>\r\n      \r\n</script>");
        }
        #pragma warning restore 1998
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; } = default!;
        #nullable disable
        #nullable restore
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<agilium.webapp.manager.mvc.ViewModels.EmpresasAssociadasViewModel> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
