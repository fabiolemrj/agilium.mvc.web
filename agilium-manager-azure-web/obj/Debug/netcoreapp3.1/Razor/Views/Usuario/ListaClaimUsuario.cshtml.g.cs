#pragma checksum "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Usuario\ListaClaimUsuario.cshtml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "525b0972c3546425ccfc4d8085599f3955962fdbfadc946629479b76c3bd2d13"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCoreGeneratedDocument.Views_Usuario_ListaClaimUsuario), @"mvc.1.0.view", @"/Views/Usuario/ListaClaimUsuario.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"Sha256", @"525b0972c3546425ccfc4d8085599f3955962fdbfadc946629479b76c3bd2d13", @"/Views/Usuario/ListaClaimUsuario.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"Sha256", @"f8451ba42770157f1c112c708bc4dea4ed0e6fd8050564d03155ec11226e2f5d", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    internal sealed class Views_Usuario_ListaClaimUsuario : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<
#nullable restore
#line 2 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Usuario\ListaClaimUsuario.cshtml"
       agilium.webapp.manager.mvc.ViewModels.ClaimsPorUsuarioViewModel

#line default
#line hidden
#nullable disable
    >
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/dist/plugins/jquery/jquery.min.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", "hidden", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("idUserAspNet"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("control-label"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("labelnome"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("form-control"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("text-danger"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_7 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("frmEdit"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.LabelTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.ValidationMessageTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "525b0972c3546425ccfc4d8085599f3955962fdbfadc946629479b76c3bd2d137079", async() => {
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
            WriteLiteral("\r\n");
            WriteLiteral("\r\n");
#nullable restore
#line 4 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Usuario\ListaClaimUsuario.cshtml"
  

    ViewData["Title"] = "Claims por Usuario";

    TempData.Keep("lista");

#line default
#line hidden
#nullable disable

            WriteLiteral("\r\n\r\n<h4>");
            Write(
#nullable restore
#line 12 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Usuario\ListaClaimUsuario.cshtml"
     ViewData["Title"]

#line default
#line hidden
#nullable disable
            );
            WriteLiteral(@"</h4>

<hr />
<section class=""barra-de-menu-principal"">
    <div class=""barra-de-botoes-menu-principal"">

        <a href=""javascript:history.go(-1)"" type=""button"" as title=""Voltar"" id=""btnVoltar""><span class=""fas fa-reply sns-tool-action"" id=""btnReturn""></span></a>
        <a href=""#"" type=""button"" title=""Salvar dados"" id=""btnSalvar""><span class=""fa fa-save sns-tool-action"" id=""btnSalvar"" onclick=""Salvar()""></span></a>

        <a href=""#"" type=""button"" title=""Precisa de Ajuda?"" id=""btnAjuda""><span class=""fa fa-question sns-tool-action""></span></a>
    </div>

    <article>
        <div class=""barra-de-posicao-atual"" id=""breadcrumb"">
            <a");
            BeginWriteAttribute("href", " href=\"", 919, "\"", 953, 1);
            WriteAttributeValue("", 926, 
#nullable restore
#line 26 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Usuario\ListaClaimUsuario.cshtml"
                      Url.Action("Index","Home")

#line default
#line hidden
#nullable disable
            , 926, 27, false);
            EndWriteAttribute();
            WriteLiteral("><i class=\"fa fa-dashboard\"></i> Home</a> /\r\n            <a");
            BeginWriteAttribute("href", " href=\"", 1013, "\"", 1063, 1);
            WriteAttributeValue("", 1020, 
#nullable restore
#line 27 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Usuario\ListaClaimUsuario.cshtml"
                      Url.Action("ObterTodosUsuarios","Usuario")

#line default
#line hidden
#nullable disable
            , 1020, 43, false);
            EndWriteAttribute();
            WriteLiteral("><i class=\"fa fa-dashboard\"></i> Usuarios </a> /\r\n            <a href=\"#\">Associar Usuario a Claims</a>\r\n        </div>\r\n    </article>\r\n</section>\r\n<br />\r\n<div class=\"row\">\r\n    <div class=\"col-md-12\">\r\n        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("form", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "525b0972c3546425ccfc4d8085599f3955962fdbfadc946629479b76c3bd2d1310666", async() => {
                WriteLiteral("\r\n            <vc:Summary></vc:Summary>\r\n            ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "525b0972c3546425ccfc4d8085599f3955962fdbfadc946629479b76c3bd2d1311002", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_1.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.
#nullable restore
#line 37 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Usuario\ListaClaimUsuario.cshtml"
                                          id

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
                WriteLiteral("\r\n            ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "525b0972c3546425ccfc4d8085599f3955962fdbfadc946629479b76c3bd2d1312830", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_1.Value;
                __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.
#nullable restore
#line 38 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Usuario\ListaClaimUsuario.cshtml"
                                          idUserAspNet

#line default
#line hidden
#nullable disable
                );
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral(@"
            <div class=""card"" id=""labelcardGerais"">
                <div class=""card-header"">
                    <h3 class=""card-title"">Usuario Selecionado</h3>
                    <div class=""card-tools"">
                        <ul class=""nav nav-pills ml-auto"">
                            <li class=""nav-item"">
                                <a class=""nav-link active"" href=""#revenue-chart"" data-toggle=""tab"">Selecione</a>
                            </li>
                        </ul>
                    </div>
                </div>
                <div class=""card-body"">
                    <div class=""row"">
                        <div class=""col-md-4"">
                            <div class=""form-group"">
                                ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("label", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "525b0972c3546425ccfc4d8085599f3955962fdbfadc946629479b76c3bd2d1315538", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.LabelTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.
#nullable restore
#line 54 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Usuario\ListaClaimUsuario.cshtml"
                                                Nome

#line default
#line hidden
#nullable disable
                );
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_LabelTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n                                ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "525b0972c3546425ccfc4d8085599f3955962fdbfadc946629479b76c3bd2d1317358", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.
#nullable restore
#line 55 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Usuario\ListaClaimUsuario.cshtml"
                                                Nome

#line default
#line hidden
#nullable disable
                );
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-for", __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
                BeginWriteTagHelperAttribute();
                __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
                __tagHelperExecutionContext.AddHtmlAttribute("readonly", Html.Raw(__tagHelperStringValueBuffer), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.Minimized);
                await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
                if (!__tagHelperExecutionContext.Output.IsContentModified)
                {
                    await __tagHelperExecutionContext.SetOutputContentAsync();
                }
                Write(__tagHelperExecutionContext.Output);
                __tagHelperExecutionContext = __tagHelperScopeManager.End();
                WriteLiteral("\r\n                                ");
                __tagHelperExecutionContext = __tagHelperScopeManager.Begin("span", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "525b0972c3546425ccfc4d8085599f3955962fdbfadc946629479b76c3bd2d1319408", async() => {
                }
                );
                __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.ValidationMessageTagHelper>();
                __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper);
                __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.
#nullable restore
#line 56 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Usuario\ListaClaimUsuario.cshtml"
                                                          Nome

#line default
#line hidden
#nullable disable
                );
                __tagHelperExecutionContext.AddTagHelperAttribute("asp-validation-for", __Microsoft_AspNetCore_Mvc_TagHelpers_ValidationMessageTagHelper.For, global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
                __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_6);
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
                    <div class=""row"">
                        <table class=""table table-hover"" id=""divGridResultado"">
                            <thead class=""table thead-dark"">
                                <tr>
                                    <th>
                                        Claim
                                    </th>
                                    <th>
                                        Ações
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
");
#nullable restore
#line 73 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Usuario\ListaClaimUsuario.cshtml"
                                 foreach (var item in Model.ClaimsSelecionadas)
                                {


#line default
#line hidden
#nullable disable

                WriteLiteral("                                    <tr>\r\n                                        <td>\r\n                                            <dt>");
                Write(
#nullable restore
#line 78 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Usuario\ListaClaimUsuario.cshtml"
                                                 Html.DisplayFor(modelItem => item.claim)

#line default
#line hidden
#nullable disable
                );
                WriteLiteral("</dt> \r\n                                        </td>\r\n                                        <td>\r\n                                            <div class=\"row\">\r\n");
#nullable restore
#line 82 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Usuario\ListaClaimUsuario.cshtml"
                                                 foreach (var acoes in item.ClaimValue)
                                                {


#line default
#line hidden
#nullable disable

                WriteLiteral(@"                                                    <div class=""col-md-2"">
                                                        <div class=""info-box mb-2 bg-light"">
                                                            <span class=""info-box-icon""><i class=""fas fa-check-circle""></i></span>
                                                            <div class=""info-box-content"">
                                                                <span class=""info-box-text"">");
                Write(
#nullable restore
#line 89 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Usuario\ListaClaimUsuario.cshtml"
                                                                                             Html.DisplayFor(acao => acoes)

#line default
#line hidden
#nullable disable
                );
                WriteLiteral("</span>\r\n                                                            </div>\r\n\r\n                                                        </div>\r\n                                                    </div>\r\n");
#nullable restore
#line 94 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Usuario\ListaClaimUsuario.cshtml"



                                                }

#line default
#line hidden
#nullable disable

                WriteLiteral("                                            </div>\r\n                                            </td>\r\n\r\n                                    </tr>\r\n");
#nullable restore
#line 102 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Usuario\ListaClaimUsuario.cshtml"
                                }

#line default
#line hidden
#nullable disable

                WriteLiteral("                            </tbody>\r\n                        </table>\r\n                    </div>\r\n\r\n\r\n                </div>\r\n            </div>\r\n        ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.FormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_FormTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.RenderAtEndOfFormTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_RenderAtEndOfFormTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_7);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n    </div>\r\n</div>\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<agilium.webapp.manager.mvc.ViewModels.ClaimsPorUsuarioViewModel> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
