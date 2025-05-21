using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace agilum.mvc.web.Extensions
{
    public static class HtmlExtensions
    {
        public static IEnumerable<SelectListItem> GetEnumValueSelectList<TEnum>(this IHtmlHelper htmlHelper) where TEnum : struct
        {
            return new SelectList(Enum.GetValues(typeof(TEnum)).OfType<Enum>()
                .Select(x =>
                    new SelectListItem
                    {
                        Text = x.GetType().GetField(x.ToString()).GetCustomAttribute<DisplayAttribute>()?.Name,
                        Value = x.ToString()
                    }), "Value", "Text");
        }
    }

}