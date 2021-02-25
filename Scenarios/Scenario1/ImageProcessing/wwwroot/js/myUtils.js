window.myUtils = {
    loadedImg: undefined,

    saveImg: function () {
    },

    loadImg: function (element) {
        window.myUtils.loadedImg = element.files[0];
    },

    processImg: function (callback) {
        var reader = new FileReader();
        reader.onloadend = function () {
            window.php.callCallbackVoid(callback, reader.result);
        }
        reader.readAsDataURL(window.myUtils.loadedImg);
    },

    createUrl: function (stringImg) {
        var image = new Image();
        image.src = stringImg;
        let blob = new Blob(image, { type: 'image/jpg' });
        return URL.createObjectURL(blob);
    }
};