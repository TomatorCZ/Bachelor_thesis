window.eventManager = {
    events: {},

    assignEvent: function (method, dotnetHelper) {
        window.eventManager.events[method] = function () { dotnetHelper.invokeMethod("CallHandler",method); }
    },

    callEvent: function (method) {
        window.eventManager.events[method]();
    }
};