using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

using CMS.DocumentEngine;
using CMS.Helpers;

using Kentico.Web.Mvc;

namespace MvcDemo.Web.Helpers
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Returns HTML input element with a label and validation fields for each property in the object that is represented by the System.Linq.Expressions.Expression expression.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="html">The HTML helper instance that this method extends</param>
        /// <param name="expression">An expression that identifies the object that contains the properties to display</param>
        public static MvcHtmlString ValidatedEditorFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            var label = html.LabelFor(expression).ToString();
            var editor = html.EditorFor(expression).ToString();
            var message = html.ValidationMessageFor(expression).ToString();

            var generatedHtml = string.Format(@"
<div class=""form-group"">
    <div class=""form-group-label"">{0}</div>
    <div class=""form-group-input"">{1}</div>
    <div class=""message message-error"">{2}</div>
</div>", label, editor, message);

            return MvcHtmlString.Create(generatedHtml);
        }


        /// <summary>
        /// Generates A tag with "mailto" link.
        /// </summary>
        /// <param name="htmlHelper">HTML helper</param>
        /// <param name="email">Email address</param>
        public static MvcHtmlString MailTo(this HtmlHelper htmlHelper, string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return MvcHtmlString.Empty;
            }

            var link = string.Format("<a href=\"mailto:{0}\">{1}</a>", HTMLHelper.EncodeForHtmlAttribute(email), HTMLHelper.HTMLEncode(email));

            return MvcHtmlString.Create(link);
        }


        /// <summary>
        /// Generates IMG tag for attachment.
        /// </summary>
        /// <param name="htmlHelper">HTML helper</param>
        /// <param name="attachment">Attachment object</param>
        /// <param name="title">Title</param>
        /// <param name="cssClassName">CSS class</param>
        /// <param name="constraint">Size constraint</param>
        public static MvcHtmlString AttachmentImage(this HtmlHelper htmlHelper, Attachment attachment, string title = "", string cssClassName = "", SizeConstraint? constraint = null)
        {
            if (attachment == null)
            {
                return MvcHtmlString.Empty;
            }

            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            var image = new TagBuilder("img");
            image.MergeAttribute("src", urlHelper.Kentico().Attachment(attachment, constraint.GetValueOrDefault(SizeConstraint.Empty)));
            image.AddCssClass(cssClassName);
            image.MergeAttribute("alt", title);
            image.MergeAttribute("title", title);

            return MvcHtmlString.Create(image.ToString(TagRenderMode.SelfClosing));
        }
    }
}