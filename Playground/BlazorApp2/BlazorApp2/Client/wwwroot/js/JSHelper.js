window.eventManager = {
    eventHandler: undefined,

    setEventHandler: function (dotnetHelper) { window.eventManager.eventHandler = dotnetHelper; },

    callEvent: function (method, ...args) {
        window.eventManager.eventHandler.invokeMethod("CallHandler", method, args);
    }
};