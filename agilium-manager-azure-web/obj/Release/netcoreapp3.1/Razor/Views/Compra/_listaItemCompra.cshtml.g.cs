#pragma checksum "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Compra\_listaItemCompra.cshtml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "a2823a549a1f95c639548b7552996a043ae7aed61779caef45ef1ac0b767b0ca"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCoreGeneratedDocument.Views_Compra__listaItemCompra), @"mvc.1.0.view", @"/Views/Compra/_listaItemCompra.cshtml")]
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
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"Sha256", @"a2823a549a1f95c639548b7552996a043ae7aed61779caef45ef1ac0b767b0ca", @"/Views/Compra/_listaItemCompra.cshtml")]
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"Sha256", @"f8451ba42770157f1c112c708bc4dea4ed0e6fd8050564d03155ec11226e2f5d", @"/Views/_ViewImports.cshtml")]
    #nullable restore
    internal sealed class Views_Compra__listaItemCompra : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<
#nullable restore
#line 1 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Compra\_listaItemCompra.cshtml"
       IEnumerable<agilium.webapp.manager.mvc.ViewModels.CompraViewModel.CompraItemViewModel>

#line default
#line hidden
#nullable disable
    >
    #nullable disable
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn btn-default"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_1 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "EditarItemModal", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_2 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("title", new global::Microsoft.AspNetCore.Html.HtmlString("Editar dados básicos do item"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_3 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("data-modal-local", new global::Microsoft.AspNetCore.Html.HtmlString("0"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_4 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn btn-warning"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_5 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "EditarItem", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_6 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("title", new global::Microsoft.AspNetCore.Html.HtmlString("Editar item Completo"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_7 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("class", new global::Microsoft.AspNetCore.Html.HtmlString("btn btn-danger"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_8 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("asp-action", "Apagar", global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_9 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("title", new global::Microsoft.AspNetCore.Html.HtmlString("Apagar Item"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
        private global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 3 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Compra\_listaItemCompra.cshtml"
  
    var importada = false;
    importada = (ViewBag.importada != null || ViewData["importada"] != null)? (ViewBag.importada || (bool)ViewData["importada"] == true): false;
 

#line default
#line hidden
#nullable disable

            WriteLiteral(@"<div>
    <table class=""table table-hover"">
        <thead class=""table thead-dark"">
            <tr>
                <th>Cod Produto</th>
                <th style=""width:300px"">Produto</th>
                <th>Estoque</th>
                <th>Quant.</th>
                <th>Unid.</th>
                <th>Rel Compra/venda</th>
                <th>Valor Unitário</th>
                <th>Valor Total</th>
                <th>Preço Venda</th>
                <th>CFOP</th>
                <th>Estoque</th>
                <th>Descr. NF</th>
                <th>CEST</th>
                <th>NCM</th>
                <th>EAN</th>
                <th>CST ICMS</th>
                <th>CST Cofins</th>
                <th>CST PIS</th>
                <th>CST IPI</th>
                <th>Impostos</th>
");
            WriteLiteral("                <th></th>\r\n            </tr>\r\n        </thead>\r\n        <tbody>\r\n");
#nullable restore
#line 49 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Compra\_listaItemCompra.cshtml"
             foreach (var item in Model.OrderBy(x => x.CodigoProdutoFornecedor))
            {
                var valorAliquotaIcms = item.ValorAliquotaIcms.HasValue ? item.ValorAliquotaIcms.Value : 0;
                var valorAliquotaCofins = item.ValorAliquotaCofins.HasValue ? item.ValorAliquotaCofins.Value : 0;
                var valorAliquotaIpi = item.ValorAliquotaIpi.HasValue ? item.ValorAliquotaIpi.Value : 0;
                var valorAliquotaPis = item.ValorAliquotaPis.HasValue ? item.ValorAliquotaPis.Value : 0;
                var valorBaseCalculoCofins = item.ValorBaseCalculoCofins.HasValue ? item.ValorBaseCalculoCofins.Value : 0;
                var valorBaseCalculoIcms = item.ValorBaseCalculoIcms.HasValue ? item.ValorBaseCalculoIcms.Value : 0;
                var valorBaseCalculoIpi = item.ValorBaseCalculoIpi.HasValue ? item.ValorBaseCalculoIpi.Value : 0;
                var valorBaseCalculoPis = item.ValorBaseCalculoPis.HasValue ? item.ValorBaseCalculoPis.Value : 0;
                var valorBaseRetido = item.ValorBaseRetido.HasValue ? item.ValorBaseRetido.Value : 0;
                var valorCofins = item.ValorCofins.HasValue ? item.ValorCofins.Value : 0;
                var valorIcms = item.ValorIcms.HasValue ? item.ValorIcms.Value : 0;
                var valorIpi = item.ValorIpi.HasValue ? item.ValorIpi.Value : 0;
                var valorNovoPrecoVenda = item.ValorNovoPrecoVenda.HasValue ? item.ValorNovoPrecoVenda.Value : 0;
                var valorUnitario = item.ValorUnitario.HasValue ? item.ValorUnitario.Value : 0;
                var valorTotal = item.ValorTotal.HasValue ? item.ValorTotal.Value : 0;
                var valorPis = item.ValorPis.HasValue ? item.ValorPis.Value : 0;
                var valorOUTROS = item.ValorOUTROS.HasValue ? item.ValorOUTROS.Value : 0;

                var impostos = valorAliquotaIcms + valorAliquotaCofins + valorAliquotaIpi + valorAliquotaPis + valorAliquotaPis + valorBaseCalculoCofins +
                valorBaseCalculoIcms + valorBaseCalculoIpi + valorBaseCalculoPis + valorBaseRetido + valorCofins + valorIcms + valorIcms + valorIpi +
                valorPis;


#line default
#line hidden
#nullable disable

            WriteLiteral("                <tr>\r\n                    <td> ");
            Write(
#nullable restore
#line 74 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Compra\_listaItemCompra.cshtml"
                          Html.DisplayFor(modelItem => item.CodigoProdutoFornecedor)

#line default
#line hidden
#nullable disable
            );
            WriteLiteral("</td>\r\n                    <td>");
            Write(
#nullable restore
#line 75 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Compra\_listaItemCompra.cshtml"
                         Html.DisplayFor(modelItem => item.NomeProduto)

#line default
#line hidden
#nullable disable
            );
            WriteLiteral("</td>\r\n                    <td> ");
            Write(
#nullable restore
#line 76 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Compra\_listaItemCompra.cshtml"
                          Html.DisplayFor(modelItem => item.NomeEstoque)

#line default
#line hidden
#nullable disable
            );
            WriteLiteral("</td>\r\n                    <td> ");
            Write(
#nullable restore
#line 77 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Compra\_listaItemCompra.cshtml"
                          Html.DisplayFor(modelItem => item.Quantidade)

#line default
#line hidden
#nullable disable
            );
            WriteLiteral("</td>\r\n                    <td> ");
            Write(
#nullable restore
#line 78 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Compra\_listaItemCompra.cshtml"
                          Html.DisplayFor(modelItem => item.SGUN)

#line default
#line hidden
#nullable disable
            );
            WriteLiteral("</td>\r\n                    <td> ");
            Write(
#nullable restore
#line 79 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Compra\_listaItemCompra.cshtml"
                          Html.DisplayFor(modelItem => item.Relacao)

#line default
#line hidden
#nullable disable
            );
            WriteLiteral("</td>\r\n                    <td> ");
            Write(
#nullable restore
#line 80 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Compra\_listaItemCompra.cshtml"
                          Html.Raw(valorUnitario.ToString("N"))

#line default
#line hidden
#nullable disable
            );
            WriteLiteral("</td>\r\n                    <td> ");
            Write(
#nullable restore
#line 81 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Compra\_listaItemCompra.cshtml"
                          Html.Raw(valorTotal.ToString("N"))

#line default
#line hidden
#nullable disable
            );
            WriteLiteral("</td>\r\n                    <td> ");
            Write(
#nullable restore
#line 82 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Compra\_listaItemCompra.cshtml"
                          Html.Raw(valorNovoPrecoVenda.ToString("N"))

#line default
#line hidden
#nullable disable
            );
            WriteLiteral("</td>\r\n                    <td> ");
            Write(
#nullable restore
#line 83 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Compra\_listaItemCompra.cshtml"
                          Html.DisplayFor(modelItem => item.NumeroCFOP)

#line default
#line hidden
#nullable disable
            );
            WriteLiteral("</td>\r\n                    <td> ");
            Write(
#nullable restore
#line 84 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Compra\_listaItemCompra.cshtml"
                          Html.DisplayFor(modelItem => item.NomeEstoque)

#line default
#line hidden
#nullable disable
            );
            WriteLiteral("</td>\r\n                    <td> ");
            Write(
#nullable restore
#line 85 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Compra\_listaItemCompra.cshtml"
                          Html.DisplayFor(modelItem => item.DescricaoProdutoCompra)

#line default
#line hidden
#nullable disable
            );
            WriteLiteral("</td>\r\n                    <td> ");
            Write(
#nullable restore
#line 86 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Compra\_listaItemCompra.cshtml"
                          Html.DisplayFor(modelItem => item.CodigoCEST)

#line default
#line hidden
#nullable disable
            );
            WriteLiteral("</td>\r\n                    <td> ");
            Write(
#nullable restore
#line 87 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Compra\_listaItemCompra.cshtml"
                          Html.DisplayFor(modelItem => item.CodigoNCM)

#line default
#line hidden
#nullable disable
            );
            WriteLiteral("</td>\r\n                    <td> ");
            Write(
#nullable restore
#line 88 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Compra\_listaItemCompra.cshtml"
                          Html.DisplayFor(modelItem => item.CodigoEan)

#line default
#line hidden
#nullable disable
            );
            WriteLiteral("</td>\r\n                    <td> ");
            Write(
#nullable restore
#line 89 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Compra\_listaItemCompra.cshtml"
                          Html.DisplayFor(modelItem => item.CodigoCstIcms)

#line default
#line hidden
#nullable disable
            );
            WriteLiteral("</td>\r\n                    <td> ");
            Write(
#nullable restore
#line 90 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Compra\_listaItemCompra.cshtml"
                          Html.DisplayFor(modelItem => item.CodigoCstCofins)

#line default
#line hidden
#nullable disable
            );
            WriteLiteral("</td>\r\n                    <td> ");
            Write(
#nullable restore
#line 91 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Compra\_listaItemCompra.cshtml"
                          Html.DisplayFor(modelItem => item.CodigoCstPis)

#line default
#line hidden
#nullable disable
            );
            WriteLiteral("</td>\r\n                    <td> ");
            Write(
#nullable restore
#line 92 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Compra\_listaItemCompra.cshtml"
                          Html.DisplayFor(modelItem => item.CodigoCstPis)

#line default
#line hidden
#nullable disable
            );
            WriteLiteral("</td>\r\n                    <td> ");
            Write(
#nullable restore
#line 93 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Compra\_listaItemCompra.cshtml"
                          Html.Raw(impostos.ToString("N"))

#line default
#line hidden
#nullable disable
            );
            WriteLiteral("</td>\r\n");
            WriteLiteral("                    <td>\r\n                        ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "a2823a549a1f95c639548b7552996a043ae7aed61779caef45ef1ac0b767b0ca17855", async() => {
                WriteLiteral("<spam class=\"fas fa-edit\"></spam>");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_1.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_1);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
            WriteLiteral(
#nullable restore
#line 108 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Compra\_listaItemCompra.cshtml"
                                                                                               item.Id

#line default
#line hidden
#nullable disable
            );
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_2);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_3);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                       \r\n");
#nullable restore
#line 110 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Compra\_listaItemCompra.cshtml"
                         if (!importada)
                        {

#line default
#line hidden
#nullable disable

            WriteLiteral("                            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "a2823a549a1f95c639548b7552996a043ae7aed61779caef45ef1ac0b767b0ca20739", async() => {
                WriteLiteral("<spam class=\"fa fa-pencil-alt\"></spam>");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_4);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_5.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_5);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
            WriteLiteral(
#nullable restore
#line 112 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Compra\_listaItemCompra.cshtml"
                                                                                              item.Id

#line default
#line hidden
#nullable disable
            );
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_6);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n                            ");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("a", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "a2823a549a1f95c639548b7552996a043ae7aed61779caef45ef1ac0b767b0ca23239", async() => {
                WriteLiteral("<spam class=\"fas fa-window-close\"></spam>");
            }
            );
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_7);
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.Action = (string)__tagHelperAttribute_8.Value;
            __tagHelperExecutionContext.AddTagHelperAttribute(__tagHelperAttribute_8);
            if (__Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues == null)
            {
                throw new InvalidOperationException(InvalidTagHelperIndexerAssignment("asp-route-id", "Microsoft.AspNetCore.Mvc.TagHelpers.AnchorTagHelper", "RouteValues"));
            }
            BeginWriteTagHelperAttribute();
            WriteLiteral(
#nullable restore
#line 113 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Compra\_listaItemCompra.cshtml"
                                                                                         item.Id

#line default
#line hidden
#nullable disable
            );
            __tagHelperStringValueBuffer = EndWriteTagHelperAttribute();
            __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"] = __tagHelperStringValueBuffer;
            __tagHelperExecutionContext.AddTagHelperAttribute("asp-route-id", __Microsoft_AspNetCore_Mvc_TagHelpers_AnchorTagHelper.RouteValues["id"], global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_9);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral("\r\n");
#nullable restore
#line 114 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Compra\_listaItemCompra.cshtml"
                        }

#line default
#line hidden
#nullable disable

            WriteLiteral("                    </td>\r\n                </tr>\r\n");
#nullable restore
#line 117 "C:\ProjetoAgilium\agilium_base\agilium-manager-azure-web\Views\Compra\_listaItemCompra.cshtml"
            }

#line default
#line hidden
#nullable disable

            WriteLiteral("        </tbody>\r\n    </table>\r\n</div>\r\n\r\n");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<IEnumerable<agilium.webapp.manager.mvc.ViewModels.CompraViewModel.CompraItemViewModel>> Html { get; private set; } = default!;
        #nullable disable
    }
}
#pragma warning restore 1591
