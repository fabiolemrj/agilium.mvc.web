using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System;

namespace SeuProjeto.TagHelpers
{
    [HtmlTargetElement("input", Attributes = ForAttributeName)]
    public class MoneyInputTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var type = For.ModelExplorer.ModelType;

            // Se for double, double? ou decimal, aplica a classe "money"
            if (type == typeof(double) || type == typeof(double?) ||
                type == typeof(decimal) || type == typeof(decimal?))
            {
                if (output.Attributes.TryGetAttribute("class", out var classAttr))
                {
                    // adiciona sem sobrescrever outras classes
                    var newClassValue = classAttr.Value.ToString();
                    if (!newClassValue.Contains("money"))
                        output.Attributes.SetAttribute("class", newClassValue + " money");
                }
                else
                {
                    output.Attributes.Add("class", "money");
                }
            }
        }
    }
}
