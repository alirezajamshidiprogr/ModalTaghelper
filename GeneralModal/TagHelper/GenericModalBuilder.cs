using Microsoft.AspNetCore.Html;
using GeneralModal.Models;
using System.Text;
using GeneralModal.Controllers;

namespace GeneralModal.TagHelper
{
        public class GenericModalBuilder
        {
            private readonly HomeController.GenericModalModel _model;
            private bool _enableValidation = true;

            public GenericModalBuilder(string id)
            {
                _model = new HomeController.GenericModalModel { Id = id };
            }

            public GenericModalBuilder EnableValidation(bool enable = true)
            {
                _enableValidation = enable;
                return this;
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
                    _ => ""
                };

                // Header
                string headerHtml = "";
                if (!string.IsNullOrEmpty(_model.HeaderHtml))
                {
                    headerHtml = $"<div class='modal-header'>{_model.HeaderHtml}<button type='button' class='close' onclick=\"closeModal('{_model.Id}')\">&times;</button></div>";
                }
                else if (!string.IsNullOrEmpty(_model.Title))
                {
                    headerHtml = $"<div class='modal-header'><h5 class='modal-title'>{_model.Title}</h5><button type='button' class='close' onclick=\"closeModal('{_model.Id}')\">&times;</button></div>";
                }

                // Footer
                string footerHtml = "";
                if (!string.IsNullOrEmpty(_model.FooterHtml))
                {
                    footerHtml = $"<div class='modal-footer'>{_model.FooterHtml}</div>";
                }

                // Body
                string bodyHtml = $"<div class='modal-body'>{_model.BodyHtml}</div>";

                // --- Script ولیدیشن + Open/Close Bootstrap 4 compatible ---
                string script = $@"
<script>
document.addEventListener('DOMContentLoaded', function () {{
    var modal = document.getElementById('{_model.Id}');
    if (!modal) return;

    var fields = modal.querySelectorAll('input[isrequired], textarea[isrequired], select[isrequired]');
    fields.forEach(function(field) {{
        field.addEventListener('blur', function() {{ validateField(field); }});
        field.addEventListener('input', function() {{
            if(field.classList.contains('is-invalid')) validateField(field);
        }});
    }});

    function validateField(field) {{
        if(!field.checkValidity()) {{
            field.classList.add('is-invalid');
            field.classList.remove('is-valid');
            return false;
        }}
        field.classList.remove('is-invalid');
        field.classList.add('is-valid');
        return true;
    }}
}});

// ---------- Submit خودکار با اعتبارسنجی ----------
function submitModalForm() {{
    {(_enableValidation ? $@"
    var modal = document.getElementById('{_model.Id}');
    if (!modal) return;

    var fields = modal.querySelectorAll('input[isrequired], textarea[isrequired], select[isrequired]');
    var formIsValid = true;

    fields.forEach(function(field) {{
        if (!field.checkValidity()) {{
            field.classList.add('is-invalid');
            field.classList.remove('is-valid');
            formIsValid = false;
        }} else {{
            field.classList.remove('is-invalid');
            field.classList.add('is-valid');
        }}
    }});

    if (!formIsValid) {{
        alert('لطفاً تمام فیلدهای اجباری را کامل کنید!');
        return;
    }}
    alert('فرم معتبر است، می‌توانید ادامه دهید!');
    " : "")}
}}

// ---------- Bootstrap 4 Open / Close ----------
function openModal(id){{
    var modal = document.getElementById(id);
    if(!modal) return;

    var oldBackdrop = document.querySelector('.modal-backdrop');
    if(oldBackdrop) oldBackdrop.remove();

    modal.style.display = 'block';
    modal.classList.add('in','show');
    document.body.style.overflow = 'hidden';

    var backdrop = document.createElement('div');
    backdrop.className = 'modal-backdrop fade in';
    backdrop.id = id + '_backdrop';
    document.body.appendChild(backdrop);
}}

function closeModal(id){{
    var modal = document.getElementById(id);
    if(!modal) return;

    modal.classList.remove('in','show');
    modal.style.display = 'none';

    var backdrop = document.getElementById(id + '_backdrop');
    if(backdrop) backdrop.remove();

    document.body.style.overflow = '';
}}
</script>";

                string html = $@"
<div class='modal fade' id='{_model.Id}' tabindex='-1' aria-hidden='true'>
    <div class='modal-dialog {modalSizeClass} modal-dialog-centered'>
        <div class='modal-content'>
            {headerHtml}
            {bodyHtml}
            {footerHtml}
        </div>
    </div>
</div>
{script}";

                return new HtmlString(html);
            }
        }
    }
