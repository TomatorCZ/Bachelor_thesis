window.example = {
    component: undefined,
    files: {},
    nextFileId: 0,

    init: function (component) { this.component = component; },

    callCS: function (method, ...args) { this.component.invokeMethod(method, ...args); },

    getFilesInfo: function (idElement) {
        let files = document.getElementById(idElement).files;

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

    getData: function (idFile, callback) {
        const toBase64 = file => new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.readAsDataURL(file);
            reader.onload = () => resolve(reader.result);
            reader.onerror = error => reject(error);
        });

        toBase64(this.files[idFile]).then(result => {
            this.callCS(callback, result);
        });
    },

    loadImg: function ()
    {
        document.getElementById('img1').setAttribute("src", URL.createObjectURL(this.files[0]));

    }
}