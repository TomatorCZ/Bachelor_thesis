<p>You can add a script as Blazor component.</p>
<pre>
&lt;PhpScriptProvider ContextLifetime="@SessionLifetime.Persistant" Type="@PhpScriptProviderType.Script" ScriptName="index.php"&gt;
    &lt;Navigating&gt;
        &lt;PhpScriptProvider ContextLifetime="@SessionLifetime.OnNavigationChanged" Type="@PhpScriptProviderType.Script" ScriptName="navigating.php"/&gt;
    &lt;/Navigating&gt;
    &lt;NotFound&gt;
        &lt;p&gt;Script not found&lt;/p&gt;
    &lt;/NotFound&gt;
&lt;/PhpScriptProvider&gt;
</pre>
<?php