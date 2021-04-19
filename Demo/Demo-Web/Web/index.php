﻿<?php
    require("/headers/defaultHeader.php");
    CallJsVoid("window.alert", "Hello from PHP script.");
?>

<h1>Index</h1>

<h3>Easy Configuration</h3>
<p>This static web was configured just by 2 lines of code.</p>
<pre>
public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        // Configure logging
        builder.Logging.SetMinimumLevel(LogLevel.Debug); // Debug does not work

        // Add PHP
        <b>builder.AddPhp(new[] { typeof(force).Assembly });</b>
        // builder.RootComponents.Add(typeof(PhpScriptProvider), "#app", ParameterView.FromDictionary(new Dictionary<string, object>() { { "ContextLifetime", SessionLifetime.Persistant} }));
        <b>builder.RootComponents.Add(typeof(PhpScriptProvider), "#app");</b>
            
        await builder.Build().RunAsync();
    }
</pre>

<h3>Offline</h3>
<p>You can try the offline mode by disabling it in the developer tools.</p>
<p>You can navigate through the website, but external resources (like images) can not be loaded in offline mode.</p>
<img src="Web/images/offlineMode.png" width="600" height="200"/>
<h3>Compatible with PhpComponents</h3>
<p>The context is shared between the component and the provider, which renders the component, when the context mode is persistant.
We demonstrate it by creating variable $msg &et; "Hello from provider" and printing it in the component</p>

Try navigating to the <a href="/simpleComponent">component</a>

<?php
    $msg="Hello from provider";
    require("/footers/defaultFooter.php");
?>