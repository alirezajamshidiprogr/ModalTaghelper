using Microsoft.AspNetCore.Html;
using GeneralModal.Models;
using System.Text;
using GeneralModal.Controllers;

namespace GeneralModal.TagHelper
{
    public class GenericModalBuilder
    {
        private readonly HomeController.GenericModalModel _model;

        public GenericModalBuilder(string id)
        {
            _model = new HomeController.GenericModalModel { Id = id };
        }

        public GenericModalBuilder Title(string title)
        {
            _model.Title = title;
            return this;
        }

        public GenericModalBuilder Size(string size)
        {
            _model.Size = size;
            return this;
        }

        public GenericModalBuilder HeaderHtml(string html)
        {
            _model.HeaderHtml = html;
            return this;
        }

        public GenericModalBuilder FooterHtml(string html)
        {
            _model.FooterHtml = html;
            return this;
        }

        // پذیرش چند HtmlElement
        public GenericModalBuilder BodyHtml(params HtmlElement[] elements)
        {
            var sb = new StringBuilder();
            foreach (var el in elements)
            {
                sb.AppendLine(el.Render());
            }
            _model.BodyHtml = sb.ToString();
            return this;
        }

        public IHtmlContent Build()
        {
            string modalSizeClass = _model.Size switch
            {
                "sm" => "modal-sm",
                "lg" => "modal-lg",
                "xl" => "modal-xl",
                "xxl" => "modal-xxl",
                _ => "" // md پیش‌فرض
            };

            // Header
            string headerHtml = "";
            if (!string.IsNullOrEmpty(_model.HeaderHtml))
            {
                headerHtml = $"<div class='modal-header'>{_model.HeaderHtml}<button type='button' class='btn-close' data-bs-dismiss='modal'></button></div>";
            }
            else if (!string.IsNullOrEmpty(_model.Title))
            {
                headerHtml = $"<div class='modal-header'><h5 class='modal-title'>{_model.Title}</h5><button type='button' class='btn-close' data-bs-dismiss='modal'></button></div>";
            }

            // Footer
            string footerHtml = "";
            if (!string.IsNullOrEmpty(_model.FooterHtml))
            {
                footerHtml = $"<div class='modal-footer'>{_model.FooterHtml}</div>";
            }

            // Body
            string bodyHtml = $"<div class='modal-body'>{_model.BodyHtml}</div>";

            string html = $@"
<div class='modal fade' id='{_model.Id}' tabindex='-1' aria-hidden='true' data-bs-backdrop='static' data-bs-keyboard='false'>
    <div class='modal-dialog {modalSizeClass} modal-dialog-centered'>
        <div class='modal-content'>
            {headerHtml}
            {bodyHtml}
            {footerHtml}
        </div>
    </div>
</div>";

            return new HtmlString(html);
        }
    }
}
