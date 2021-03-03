window.php = {
    phpContext: undefined,

    postPresented: false,

    filesPresented: false,

    init: function (phpContext) { window.php.phpContext = phpContext; },

    isPost: function () { return this.postPresented; },

    isFiles: function () { return this.filesPresented; }
};

window.php.forms = {
    postData: {},
    getData: {},
    filesData: {},

    turnFormToClientSide: function (form) {
        form.addEventListener("submit", (event) => {
            this.postData = {};
            this.getData = {};
            this.filesData = {};
            window.php.postPresented = false;
            window.php.filesPresented = false;

            let url = event.target.getAttribute("action");

            let formData = new FormData(form);

            this.processFormsData(formData, form.getAttribute("method"));

            if (form.getAttribute("method") == "get") {
                if (this.getData != undefined)
                    url = url + "?" + new URLSearchParams(this.getData);;
            }
            else if (form.getAttribute("method") == "post") {
                if (this.postData != undefined) 
                    window.php.postPresented = true;
            }
            else {
                return;
            }

            if (this.filesData != undefined)
                window.php.filesPresented = true;

            window.php.internal.navigateTo(url);
            event.preventDefault();
            event.stopPropagation();
        });
    },

    turnFormsToClientSide: function () {
        let forms = document.getElementsByTagName("form");

        for (var i = 0; i < forms.length; i++) {
            this.turnFormToClientSide(forms[i]);
        }
    },

    processFormsData: function (formData, method) {
        for (var pair of formData.entries()) {
            if (pair[1] instanceof File) {
                this.filesData[pair[0]] = window.php.files.addFile(pair[1]);
            }
            else {
                if (method == "get") {
                    this.getData[pair[0]] = pair[1];
                }
                else if (method == "post")
                    this.postData[pair[0]] = pair[1];
            }
        }
    },

    getPostData: function () {
        return this.postData;
    },

    getFilesData: function () {

    }
};

window.php.files = {
    files: {},

    nextFileId: 0,

    addFile: function (file) {
        this.files[this.nextFileId] = file;
        return this.nextFileId++;
    }
}

window.php.internal = {
    navigateTo: function (url) {
        var a = document.createElement('a');
        a.href = url;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
    }
};