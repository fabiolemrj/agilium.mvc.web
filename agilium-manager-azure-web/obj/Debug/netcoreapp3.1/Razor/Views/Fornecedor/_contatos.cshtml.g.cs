#pragma checksum "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Fornecedor\_contatos.cshtml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "d06690ae1238136a5fcfde1d2fff72e5bff4a61e7c6b2e08404a46907df632b7"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCoreGeneratedDocument.Views_Fornecedor__contatos), @"mvc.1.0.view", @"/Views/Fornecedor/_contatos.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"Sha256", @"d06690ae1238136a5fcfde1d2fff72e5bff4a61e7c6b2e08404a46907df632b7", @"/Views/Fornecedor/_contatos.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"Sha256", @"f8451ba42770157f1c112c708bc4dea4ed0e6fd8050564d03155ec11226e2f5d", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    internal sealed class Views_Fornecedor__contatos : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<
#nullable restore
#line 1 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Fornecedor\_contatos.cshtml"
       agilium.webapp.manager.mvc.ViewModels.Fornecedor.FornecedorViewModel

#line default
#line hidden
#nullable disable
    >
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("type", "hidden", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("nav-link btn btn-default"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "AdicionarContato", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("title", new global::Microsoft.AspNetCore.Html.HtmlString("Cadastrar Novo Contato"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("data-modal", new global::Microsoft.AspNetCore.Html.HtmlString(""), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("id", new global::Microsoft.AspNetCore.Html.HtmlString("btnaddContato"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn btn-warning"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_7 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "EditarContato", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_8 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("title", new global::Microsoft.AspNetCore.Html.HtmlString("Editar"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper;
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n\r\n<div class=\"card card card-outline card-primary\">\r\n    <div class=\"card-header\">\r\n        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "d06690ae1238136a5fcfde1d2fff72e5bff4a61e7c6b2e08404a46907df632b76872", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.
#nullable restore
#line 6 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Fornecedor\_contatos.cshtml"
                                      Endereco.Id

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
            WriteLiteral(@"
        <h5 class=""card-title"">
            <i class=""fas fa-address-book""></i>
            Contato
        </h5>
    </div>
    <div class=""card-body"">
        <nav class=""navbar navbar-expand navbar-light"">
            <ul class=""navbar-nav"">
                <li class=""nav-item d-none d-sm-inline-block"">
                    <h3 class=""card-title"">
                        <i class=""fas fa-id-card""></i>
                        Contato
                    </h3>

                </li>
            </ul>
            <ul class=""navbar-nav ml-auto"">

                <li class=""nav-item"">
                    ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "d06690ae1238136a5fcfde1d2fff72e5bff4a61e7c6b2e08404a46907df632b79261", async() => {
                WriteLiteral("\r\n                        <span class=\"fa fa-plus-square sns-tool-action\"></span>\r\n                    ");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_1);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_2.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_2);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-idFornecedor", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
            WriteLiteral(
#nullable restore
#line 26 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Fornecedor\_contatos.cshtml"
                                                                                                               Model.Id

#line default
#line hidden
#nullable disable
            );
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["idFornecedor"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-idFornecedor", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["idFornecedor"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_5);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                </li>\r\n            </ul>\r\n        </nav>\r\n        <div id=\"divGridResultadoContato\">\r\n            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("input", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, "d06690ae1238136a5fcfde1d2fff72e5bff4a61e7c6b2e08404a46907df632b712145", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.InputTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper);
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.InputTypeName = (string)__tagHelperAttribute_0.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_InputTagHelper.For = ModelExpressionProvider.CreateModelExpression(ViewData, __model => __model.
#nullable restore
#line 33 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Fornecedor\_contatos.cshtml"
                                          Id

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
            WriteLiteral(@"
            <table class=""table table-hover"">
                <thead class=""table thead-dark"">
                    <tr>
                        <th>
                            Tipo
                        </th>
                        <th>
                            Descrição
                        </th>
                        <th>
                            Descrição Auxiliar
                        </th>
                        <th class=""colunaacao""></th>
                    </tr>
                </thead>
                <tbody>
");
#nullable restore
#line 50 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Fornecedor\_contatos.cshtml"
                     foreach (var item in Model.Contatos)
                    {

#line default
#line hidden
#nullable disable

            WriteLiteral("                        <tr>\r\n                            <td>\r\n                                ");
            Write(
#nullable restore
#line 54 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Fornecedor\_contatos.cshtml"
                                 Html.DisplayFor(modelItem => item.Contato.TPCONTATO)

#line default
#line hidden
#nullable disable
            );
            WriteLiteral("\r\n                            </td>\r\n                            <td>\r\n                                ");
            Write(
#nullable restore
#line 57 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Fornecedor\_contatos.cshtml"
                                 Html.DisplayFor(modelItem => item.Contato.DESCR1)

#line default
#line hidden
#nullable disable
            );
            WriteLiteral("\r\n                            </td>\r\n                            <td>\r\n                                ");
            Write(
#nullable restore
#line 60 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Fornecedor\_contatos.cshtml"
                                 Html.DisplayFor(modelItem => item.Contato.DESCR2)

#line default
#line hidden
#nullable disable
            );
            WriteLiteral("\r\n                            </td>\r\n                            <td class=\"text-right\">\r\n                                ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "d06690ae1238136a5fcfde1d2fff72e5bff4a61e7c6b2e08404a46907df632b716155", async() => {
                WriteLiteral("<spam class=\"fa fa-pencil-alt\"></spam>");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_6);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_7.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_7);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-idFornecedor", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
            WriteLiteral(
#nullable restore
#line 63 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Fornecedor\_contatos.cshtml"
                                                                                                               item.IDFORN

#line default
#line hidden
#nullable disable
            );
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["idFornecedor"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-idFornecedor", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["idFornecedor"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            BeginWriteTagHelperAttribute();
            WriteLiteral(
#nullable restore
#line 63 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Fornecedor\_contatos.cshtml"
                                                                                                                                                  item.IDCONTATO

#line default
#line hidden
#nullable disable
            );
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["idContato"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-idContato", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["idContato"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_8);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                                <a class=\"btn btn-danger delete\" id=\"btnDeleteContato\" title=\"Apagar\" id=\"btnedtContato\"\r\n                                   data-idContato=\"");
            Write(
#nullable restore
#line 65 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Fornecedor\_contatos.cshtml"
                                                    item.IDCONTATO

#line default
#line hidden
#nullable disable
            );
            WriteLiteral("\" data-idFornecedor=\"");
            Write(
#nullable restore
#line 65 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Fornecedor\_contatos.cshtml"
                                                                                        item.IDFORN

#line default
#line hidden
#nullable disable
            );
            WriteLiteral("\" data-tpcontato=\"");
            Write(
#nullable restore
#line 65 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Fornecedor\_contatos.cshtml"
                                                                                                                      item.Contato.TPCONTATO

#line default
#line hidden
#nullable disable
            );
            WriteLiteral("\" data-descr1=\"");
            Write(
#nullable restore
#line 65 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Fornecedor\_contatos.cshtml"
                                                                                                                                                            item.Contato.DESCR1

#line default
#line hidden
#nullable disable
            );
            WriteLiteral("\" id=\"btndeleteContato\"><spam class=\"fa fa-trash\"></spam></a>\r\n                            </td>\r\n                        </tr>\r\n");
#nullable restore
#line 68 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Fornecedor\_contatos.cshtml"
                    }

#line default
#line hidden
#nullable disable

            WriteLiteral("                </tbody>\r\n            </table>\r\n        </div>\r\n    </div>\r\n</div>\r\n<script>\r\n\r\n</script>\r\n\r\n\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<agilium.webapp.manager.mvc.ViewModels.Fornecedor.FornecedorViewModel> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
