<h1 align="center">
Client-side execution of PHP applications compiled to .NET
</h1>
### Introduction

Blazor is a new technology enabling to run .NET applications directly in the browser using WebAssembly, a recently created binary instruction format adopted by major web browsers. Whilst PHP is the most popular language in the realm of web applications, it cannot run directly in the browser. The PeachPie compiler provides a way to compile projects written in PHP into Common Intermediate Language (CIL), enabling them to run on the .NET platform.
This thesis aims to design and implement a convenient interface between Blazor and compiled PHP, enabling developers to create client-side PHP applications. These applications would be able to utilize the specifics of the client-side paradigm, such as fast response times, the possibility to preserve the application state between the requests more efficiently and the direct access to the Document Object Model (DOM) of the page. To demonstrate the usability of the implementation and the specific benefits of the solution, a pilot interactive application will be created.

### API & Architecture

To be determined

### Demo

I created four demo, which demonstrates common usage of using the library.

|     Demo     | Description                                                  |
| :----------: | ------------------------------------------------------------ |
|     Web      | We want to write whole web application in PHP, but the application should run on the client side. |
|  OneScript   | We have a part of application, which is better to write in PHP (Existing library, PHP syntax, etc..). So we want a opportunity to plug this part into an existing Blazor application. |
| PhpComponent | We have a part of application, which we want to write in PHP, but is heavy and the rendering is crucial. So we want more control above the rendering in other to make rendering more effective. |
| AllTogether  | We want to put together the previous scenarios.              |

#### Demo-Web

The web is placed in standalone project **Web**. You can see that there is no special constructs. 

<img src="E:\OwnCode\Github\Bachelor_thesis\Documents\Pictures\web.png" alt="VS project" style="zoom:33%;" />

The benefit of creation this web in this manner is client side execution and navigation of the scrips. You can try it yourself. The magic of this is hide in *PhpScriptProvider* class. 

It is necessary to do 3 steps to make this working.

**Step 1** - Configure server's *StartUp* class. This code serves additional javascript and resources like images.

```c#
// Add helper js code for php.
var fileProvider = new ManifestEmbeddedFileProvider(typeof(PhpBlazor.BlazorContext).Assembly);
app.UseStaticFiles(new StaticFileOptions() { FileProvider = fileProvider });

//If you have external resources like images, you can add them in this way
app.UseStaticFiles(new StaticFileOptions {
	FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName, "Web\\wwwroot")),
RequestPath = "/Web"
});
```

**Step 2** - Configure client's *Main* method.

```c#
public static async Task Main(string[] args)
{
	var builder = WebAssemblyHostBuilder.CreateDefault(args);

	// Add PHP
	builder.AddPhp(new[] { typeof(force).Assembly });
	builder.RootComponents.Add(typeof(PhpScriptProvider), "#app");
        
	await builder.Build().RunAsync();
}  
```
**Step 3** - Add link to *index.html*.

```html
<head>
...
<script src="js/php.js"></script>
</head>
```

#### Demo-OneScript

It demonstrates how to add php scripts to Blazor as a part. You can navigate to them. There are necessary 4 steps to make it possible.

**Step 1** - Configure server's *StartUp* class. This code serves additional javascript and resources like images. Same as previous

**Step 2** - Configure client's *Main* method.
```c#
public static async Task Main(string[] args)
{
	var builder = WebAssemblyHostBuilder.CreateDefault(args);
	builder.RootComponents.Add<App>("#app");
	
	// Add PHP
	builder.AddPhp(new[] { typeof(force).Assembly });
	    
	await builder.Build().RunAsync();
}  
```
**Step 3** - Add link to *index.html*. Same as previous.

**Step 4** - Add *PhpScriptProvider* into Razor page. There are several options how to set the provider. If you want to navigate more than one script, change *PhpScriptProviderType* to *ScriptProvider*.   

```Razor
@page "/phpstandalone"
@using PhpBlazor

<PhpScriptProvider ContextLifetime="@SessionLifetime.Persistant" Type="@PhpScriptProviderType.Script" ScriptName="fileManagment/index.php">
    <Navigating>
        <p>Navigating</p>
    </Navigating>
    <NotFound>
        <p>Script not found</p>
    </NotFound>
</PhpScriptProvider>
```

#### Demo-PhpComponent

If you want to have full control of rendering, you can inherit *PhpComponent*. 

```php
#[\Microsoft\AspNetCore\Components\RouteAttribute("/Asteroids")]
class AsteroidsComponent extends \PhpBlazor\PhpComponent
{	
	private $app;
	private \PhpBlazor\Timer $timer;

	public function BuildRenderTree($builder) : void 
	{
		$this->app->writeWithTreeBuilder($builder, 0);
	}
}
```

You have to make same modification of client's *Main*, *index.html* and server's *StartUp* as in the previous examples.

#### Demo-AllTogether

At the end, the components are designed to be able to be put together. So you can easily have static web in Blazor web. 

![web](E:\OwnCode\Github\Bachelor_thesis\Documents\Pictures\webInweb.png)

### Project structure

The project is divided into 4 folders

- **Playground** - There are projects, which can be broken and they are used for experiments.
- **Demo** - There are projects which represents individual scenarios. See the section <a href="#scenarios">Scenarios</a>.
- **Source** - There is a project(s) which is shared by scenarios and are used for supporting to use PHP in Blazor.
- **Documents** - There are documents, which are related to BT.

### Status

| Date      | Action                                                       |
| :-------- | :----------------------------------------------------------- |
| 20.2.2021 | Scenario 2 is functional and can be reviewed for the first time. |
| 22.2.2021 | Scenario 1 was set and it is in progress.                    |
| 28.2.2021 | Scenario 1 is functional and can be reviewed for the first time. |
| 4.3.2021  | Scenario 3 is almost done.                                   |
| 5.3.2021  | Scenario 1,2,3 is done and can be reviewed.                  |
| 10.3.2021 | Scenarios are recreated as *Demo-Web*, *Demo-OneScript*, *Demo-PhpComponent*, *Demo-AllTogether* |

### Feature work

- [ ] [FEATURE] Make rendering, which is implemented by Tag class in [blazorUtilities.php](https://github.com/TomatorCZ/Bachelor_thesis/blob/main/Scenarios/Scenario2/Asteroids/Php/blazorUtilities.php) smarter.
- [ ] [FEATURE] Think of a better way how to supply static files in Peachpie libraries.
- [ ] [FEATURE] Make file up/downloading as streams
- [ ] [FEATURE] Lazy loading assemblies