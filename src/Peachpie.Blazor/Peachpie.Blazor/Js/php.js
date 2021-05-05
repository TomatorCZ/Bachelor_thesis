window.php = {
    phpContext: undefined,

    postPresented: false,

    filesPresented: false,

    init: function (phpContext) { window.php.phpContext = phpContext; },

    isPost: function () { return this.postPresented; },

    isFiles: function () { return this.filesPresented; },

    callPHP: function (method, data) { this.phpContext.invokeMethod("CallPHP", method, JSON.stringify(data));}
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
                if (Object.keys(this.getData).length > 0)
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

            window.php.internal.navigateFormTo(url);
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
        let result = [];

        for (var key in this.filesData) {
            if (this.filesData.hasOwnProperty(key)) {
                let struct = window.php.files.createStructure(this.filesData[key])
                struct["fieldName"] = key;
                result.push(struct);
            }
        }

        this.filesData = {};
        window.php.filesPresented = false;

        return result;
    }
};

window.php.files = {
    files: {},

    urlObjects: {},

    nextFileId: 0,

    addFile: function (file) {
        this.files[this.nextFileId] = file;
        return this.nextFileId++;
    },

    createStructure: function (fileId) {
        let file = this.files[fileId];
        return {
            "id": fileId,
            "name": file.name,
            "size": file.size,
            "type": file.type
        };
    },

    readAllFileAsBase64: function (fileId) {
        return new Promise((resolve, reject) => {

            const reader = new FileReader();
            reader.onload = function (e) {
                resolve(window.php.internal.arrayBufferToBase64(e.target.result));
            };

            reader.onerror = reject;

            reader.readAsArrayBuffer(this.files[fileId]);
        });
    },

    createUrlObject: function (fileId) {
        if (!(fileId in this.urlObjects))
            this.urlObjects[fileId] = URL.createObjectURL(this.files[fileId]); 

        return this.urlObjects[fileId];
    },

    downloadFile: function (fileId) {
        var a = document.createElement('a');
        a.href = this.createUrlObject(fileId);
        a.download = this.files[fileId].name;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
    },

    createFile: function (data, type, name) {
        let file = new File([data], name, {
            type: type,
        });

        return this.createStructure(this.addFile(file));
    }
};

window.php.internal = {
    navigateFormTo: function (url) {
        var a = document.createElement('a');
        a.href = url;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
    },

    arrayBufferToBase64: function (buffer) {
        var binary = '';
        var bytes = new Uint8Array(buffer);
        var len = bytes.byteLength;
        for (var i = 0; i < len; i++) {
            binary += String.fromCharCode(bytes[i]);
        }
        return window.btoa(binary);
    }
};