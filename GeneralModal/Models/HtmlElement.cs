namespace GeneralModal.Models
{
    public abstract class HtmlElement
    {
        public string Id { get; set; } = "";
        public string Class { get; set; } = "";
        public string Style { get; set; } = "";
        public string Name { get; set; } = "";
        public string Placeholder { get; set; } = "";
        public string LabelText { get; set; } = ""; // متن label خودکار

        // ویژگی‌های Validation
        public bool Required { get; set; } = false;
        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }
        public string Pattern { get; set; } = "";
        public string ValidationMessage { get; set; } = "";

        public virtual string RenderValidationAttributes()
        {
            string attrs = "";
            if (Required) attrs += " required";
            if (MinLength.HasValue) attrs += $" minlength='{MinLength.Value}'";
            if (MaxLength.HasValue) attrs += $" maxlength='{MaxLength.Value}'";
            if (!string.IsNullOrEmpty(Pattern)) attrs += $" pattern='{Pattern}'";
            if (!string.IsNullOrEmpty(ValidationMessage)) attrs += $" title='{ValidationMessage}'";
            return attrs;
        }

        public virtual string RenderAttributes() => $"id='{Id}' name='{Name}' class='{Class}' style='{Style}' placeholder='{Placeholder}'";

        // رندر المان و Label داخل یک div
        public virtual string Render()
        {
            return RenderWrapper(RenderElementHtml());
        }

        protected virtual string RenderWrapper(string elementHtml)
        {
            string labelHtml = !string.IsNullOrEmpty(LabelText) ? $"<label for='{Id}' class='form-label'>{LabelText}</label>" : "";
            string wrapperClass = this is CheckBox || this is RadioButton ? "form-check mb-3" : "mb-3";
            return $"<div class='{wrapperClass}'>{labelHtml}{elementHtml}</div>";
        }

        protected abstract string RenderElementHtml();
    }

    public class TextBox : HtmlElement
    {
        public string Value { get; set; } = "";

        protected override string RenderElementHtml()
        {
            string cls = string.IsNullOrWhiteSpace(Class) ? "form-control" : Class;
            return $"<input type='text' value='{Value}' {RenderAttributes()} class='{cls}' {RenderValidationAttributes()} />";
        }
    }

    public class PasswordBox : HtmlElement
    {
        public string Value { get; set; } = "";

        protected override string RenderElementHtml()
        {
            string cls = string.IsNullOrWhiteSpace(Class) ? "form-control" : Class;
            return $"<input type='password' value='{Value}' {RenderAttributes()} class='{cls}' {RenderValidationAttributes()} />";
        }
    }

    public class TextArea : HtmlElement
    {
        public string Value { get; set; } = "";
        public int Rows { get; set; } = 3;

        protected override string RenderElementHtml()
        {
            string cls = string.IsNullOrWhiteSpace(Class) ? "form-control" : Class;
            return $"<textarea rows='{Rows}' {RenderAttributes()} class='{cls}' {RenderValidationAttributes()}>{Value}</textarea>";
        }
    }

    public class Select : HtmlElement
    {
        public string InnerHtml { get; set; } = "";
        public bool UseSelect2 { get; set; } = false;

        protected override string RenderElementHtml()
        {
            string cls = string.IsNullOrWhiteSpace(Class) ? "form-select" : Class;
            if (UseSelect2) cls += " select2";
            return $"<select {RenderAttributes()} class='{cls}' {RenderValidationAttributes()}>{InnerHtml}</select>";
        }
    }

    public class CheckBox : HtmlElement
    {
        public bool Checked { get; set; } = false;
        public string Value { get; set; } = "1";

        protected override string RenderElementHtml()
        {
            string cls = string.IsNullOrWhiteSpace(Class) ? "form-check-input" : Class;
            string isChecked = Checked ? "checked" : "";
            return $"<input type='checkbox' value='{Value}' {RenderAttributes()} class='{cls}' {isChecked} {RenderValidationAttributes()} />";
        }
    }

    public class RadioButton : HtmlElement
    {
        public string Value { get; set; } = "";
        public bool Checked { get; set; } = false;

        protected override string RenderElementHtml()
        {
            string cls = string.IsNullOrWhiteSpace(Class) ? "form-check-input" : Class;
            string isChecked = Checked ? "checked" : "";
            return $"<input type='radio' value='{Value}' {RenderAttributes()} class='{cls}' {isChecked} {RenderValidationAttributes()} />";
        }
    }

    public class Button : HtmlElement
    {
        public string Type { get; set; } = "button";
        public string Text { get; set; } = "Button";
        public string OnClick { get; set; } = "";

        protected override string RenderElementHtml()
        {
            string cls = string.IsNullOrWhiteSpace(Class) ? "btn btn-primary" : Class;
            string onclickAttr = string.IsNullOrWhiteSpace(OnClick) ? "" : $"onclick='{OnClick}'";
            return $"<button type='{Type}' class='{cls}' {onclickAttr}>{Text}</button>";
        }
    }

    public class DatePicker : HtmlElement
    {
        public string Value { get; set; } = "";

        protected override string RenderElementHtml()
        {
            string cls = string.IsNullOrWhiteSpace(Class) ? "form-control datepicker" : Class;
            return $"<input type='text' value='{Value}' {RenderAttributes()} class='{cls}' {RenderValidationAttributes()} />";
        }
    }

    public class FileInput : HtmlElement
    {
        protected override string RenderElementHtml()
        {
            string cls = string.IsNullOrWhiteSpace(Class) ? "form-control" : Class;
            return $"<input type='file' {RenderAttributes()} class='{cls}' {RenderValidationAttributes()} />";
        }
    }

    // رندر همزمان چند المان
    public static class HtmlElementExtensions
    {
        public static string RenderAll(params HtmlElement[] elements)
        {
            string html = "";
            foreach (var e in elements)
            {
                html += e.Render();
            }
            return html;
        }
    }
}
