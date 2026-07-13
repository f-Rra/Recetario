// Validación cliente con decimales es-AR: jQuery Validation solo acepta
// punto decimal por defecto; estos overrides aceptan también la coma (guía 07).
if (window.jQuery && $.validator) {
    $.validator.methods.number = function (value, element) {
        return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:\.\d{3})+)?(?:,\d+)?$|^-?\d*\.?\d+$/.test(value);
    };

    $.validator.methods.range = function (value, element, param) {
        const valor = parseFloat(String(value).replace(",", "."));
        return this.optional(element) || (valor >= param[0] && valor <= param[1]);
    };
}
