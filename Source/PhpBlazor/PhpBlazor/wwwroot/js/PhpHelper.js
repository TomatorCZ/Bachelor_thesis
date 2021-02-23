window.php = {
    phpContext: undefined,

    setContext: function (phpContext) { window.php.phpContext = phpContext; },

    callCallbackVoid: function (method, ...args)
    {
        window.php.phpContext.invokeMethod("CallFromJS", method, args);
    },

    callCallback: function (method, ...args)
    {
        return window.php.phpContext.invokeMethod("CallFromJS", method, args);
    }
};