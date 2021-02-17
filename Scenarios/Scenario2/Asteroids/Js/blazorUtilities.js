window.eventManager = {
    eventHandlers: {},

    setEventHandler: function (identifier, dotnetHelper) { window.eventManager.eventHandler[identifier] = dotnetHelper; },

    callEvent: function (identifier, method, ...args) {
        window.eventManager.eventHandler[identifier].invokeMethod(method, args);
    }
};