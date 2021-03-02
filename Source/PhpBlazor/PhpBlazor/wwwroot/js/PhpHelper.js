window.php = {
    phpContext: undefined,

    init: function (phpContext) { window.php.phpContext = phpContext; }
};

window.php.forms = {
    turnFormToClientSide: function (form) {
        form.addEventListener("submit", (event) => {
            //TODO: save forms
            //TODO: navigate to action

            event.preventDefault();
        });
    },

    turnFormsToClientSide: function () {
        let forms = document.getElementsByTagName("form");

        for (var i = 0; i < forms.length; i++) {
            this.turnFormToClientSide(forms[i]);
        }
    }
}