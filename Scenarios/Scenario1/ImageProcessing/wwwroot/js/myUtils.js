window.myUtils = {
    loadedImg: undefined,

    saveImg: function () {
    },

    loadImg: function (element) {
        window.myUtils.loadedImg = element.files[0];
        console.log(window.myUtils.loadedImg);
    },

    processImg: function (callback) {
        var reader = new FileReader();
        reader.onloadend = function () {
            console.log(reader.result);
            window.php.callCallbackVoid(callback, reader.result);
        }
        reader.readAsDataURL(window.myUtils.loadedImg);
    },

    createUrl: function (stringImg) {
        var image = new Image();
        image.src = stringImg;
        return URL.createObjectURL(image);
    }
};