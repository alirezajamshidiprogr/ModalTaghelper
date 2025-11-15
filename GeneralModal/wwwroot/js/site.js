function initModalValidation(modalId) {
    const modal = document.getElementById(modalId);
    if (!modal) return;

    const fields = modal.querySelectorAll("input[isrequired], textarea[isrequired], select[isrequired]");

    fields.forEach(field => {

        // رویداد blur → هنگام leave
        field.addEventListener("blur", function () {
            validateField(field);
        });

        // اگر خواستی هنگام تایپ هم چک شود
        field.addEventListener("input", function () {
            if (field.classList.contains("is-invalid")) {
                validateField(field);
            }
        });

    });
}

function validateField(field) {
    // چک با HTML5
    if (!field.checkValidity()) {
        field.classList.add("is-invalid");
        field.classList.remove("is-valid");
        return false;
    }

    // سالم
    field.classList.remove("is-invalid");
    field.classList.add("is-valid");
    return true;
}

function IsValidForm(modalName) {
    debugger
    var modal = document.getElementById(modalName);
    if (!modal)
        return;

    var fields = modal.querySelectorAll('input[isrequired], textarea[isrequired], select[isrequired]');
    var formIsValid = true;

    fields.forEach(function (field) {
        if (!field.checkValidity()) {
            field.classList.add('is-invalid');
            field.classList.remove('is-valid');
            formIsValid = false;
        } else {
            field.classList.remove('is-invalid');
            field.classList.add('is-valid');
        }
    });

    return formIsValid;
    
}