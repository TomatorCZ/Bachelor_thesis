window.php = {
    phpContext: undefined,

    init: function (phpContext) { window.php.phpContext = phpContext; }
};

window.php.interop = {
    callPhpVoid: function (method) {
        window.php.phpContext.invokeMethod("CallPhpVoid", method);
    },
    callPhpString: function (method, data) {
        window.php.phpContext.invokeMethod("CallPhpString", method, data);
    }
}

window.php.fileUtils = {
    files: {},
    nextFileId: 0,

    getFilesInfo: function (elementId)
    {
        let files = document.getElementById(elementId).files;

        let result = [];
        for (let i = 0; i < files.length; i++) {
            this.files[this.nextFileId] = files[i];
            result.push(
                {
                    'id': this.nextFileId,
                    'name': this.files[this.nextFileId].name,
                    'size': this.files[this.nextFileId].size,
                    'type': this.files[this.nextFileId].type
                });
            this.nextFileId++;
        }

        return result;
    },

    getData: function (fileId, callback)
    {
        window.internalUtils.toBase64(this.files[fileId]).then(result => {
            window.php.interop.callPhpString(callback, result);
        }); 
    },

    createUrlObject: function (data, contentType)
    {
        let byteCharacters = atob(data);

        let byteNumbers = new Array(byteCharacters.length);
        for (let i = 0; i < byteCharacters.length; i++) {
            byteNumbers[i] = byteCharacters.charCodeAt(i);
        }

        let byteArray = new Uint8Array(byteNumbers);


        let blob = new Blob([byteArray], { type: contentType });

        return URL.createObjectURL(blob);
    },

    destroyUrlObject: function (url)
    {
        URL.revokeObjectURL(url);
    },

    setUrlTo: function (elementId, attribute, value)
    {
        document.getElementById(elementId).setAttribute(attribute, value);
    },

    downloadData: function (data, contentType, filename)
    {
        console.log(filename);
        var a = document.createElement('a');
        a.href = this.createUrlObject(data, contentType);
        a.download = filename;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
    }
}

window.internalUtils = {
    toBase64: function (file) {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = () => resolve(reader.result);
            reader.onerror = error => reject(error);
        })
    }
}