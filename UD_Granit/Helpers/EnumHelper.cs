using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace System.Web.Mvc.Html
{
    // Помогает отображать перечисления в представлениях
    public static class EnumHelper
    {
        // Добавляет метод к перечислениям по считыванию декоратора Description
        public static string ToDescription(this Enum value)
        {
            if (value == null)
                return string.Empty;

            var attributes = (DescriptionAttribute[])value.GetType().GetField(
                Convert.ToString(value)).GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attributes.Length > 0 ? attributes[0].Description : Convert.ToString(value);
        }

        // Добавляет метод по выводу выпадающего списка из перечисления
        public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TEnum>> expression, object attributes = null)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            IEnumerable<TEnum> values = Enum.GetValues(typeof(TEnum)).Cast<TEnum>();

            IEnumerable<SelectListItem> items =
                values.Select(value => new SelectListItem
                {
                    Text = (value as Enum).ToDescription(),
                    Value = value.ToString(),
                    Selected = value.Equals(metadata.Model)
                });

            return htmlHelper.DropDownListFor(expression, items);
        }
    }
}