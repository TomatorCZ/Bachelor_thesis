window.php = {
    phpContext: undefined,

    init: function (phpContext) { window.php.phpContext = phpContext; }
};

window.php.forms = {
    turnFormToClientSide: function (form) {
        form.addEventListener("submit", (event) => {
            //TODO: save forms

            window.php.internal.navigateTo(event.target.getAttribute("action"));
            event.stopPropagation()
        });
    },

    turnFormsToClientSide: function () {
        let forms = document.getElementsByTagName("form");

        for (var i = 0; i < forms.length; i++) {
            this.turnFormToClientSide(forms[i]);
        }
    }
};

window.php.internal = {
    navigateTo: function (url) {
        var a = document.createElement('a');
        a.href = url;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
    }
};