# User manual

This manual helps a user to get familiar with the ```Peachpie.Blazor``` library. See also generated library documentation for more detailed information about function parameters and return types.

### Get Started

The library contains Blazor components and helper classes enabling navigation and execution of PHP scripts, defined in a Peachpie project, inside a browser. It is available as a NuGet package, which can be found in the attachment. There are two ways how to get started. We created templates, which can be adjusted to fit your goal. The second way creates a new Blazor project referencing PHP scripts. We recommend beginning with the first (easier) way.   

##### Prerequisites

- .NET 5.0 SDK (https://dotnet.microsoft.com/download/dotnet/5.0)
- Visual Studio 2019 (Optional) (https://visualstudio.microsoft.com/vs/)
- *Peachpie.Blazor.1.0.0-alpha.nupkg* - A part of the attachment
- *Peachpie.Blazor.Templates.1.0.2.nupkg* - A part of the attachment (Required by the first approach of creating a website)

#### From Template

- **Step 1** - Install the templates by ```dotnet new -i "Path\To\Peachpie.Blazor.Templates.1.0.2.nupkg"```.
- **Step 2** - Add a new source, where ```Peachpie.Blazor.1.0.0-alpha.nupkg``` can be find by the package manager, to the ```NuGet.Config``` file. It is usually located in ```%appdata%\NuGet\NuGet.Config``` on Windows or ```~/.nuget/NuGet/NuGet.Config``` on Mac.
- **Step 3** - Choose a template depending on your intention:
  - *Peachpie Blazor Web* - A simple PHP website running in browser
  - *Peachpie Blazor Hybrid* - A simple Blazor website combining PHP and Razor Pages.
- **Step 4** - Create the project by ```dotnet new blazor-hybrid``` or ```dotnet new blazor-web```.
- **Step 5** - Modify the ```BlazorApp.Client``` or ```PHPScripts``` (Optional).
- **Step 6** - Navigate to ```BlazorApp\Server\``` and run ```dotnet run```.
- **Step 7** - Access https://localhost:5001 .

#### From Blazor project

**Step 1** - Create Blazor WebAssembly project with ASP.NET Hosting and targeting .NET 5.0.

**Step 2** - Install Peachpie Visual Studio extension (https://www.peachpie.io/getstarted).

**Step 3** - Create Peachpie Class library project.

**Step 4** -  Add a new source, where ```Peachpie.Blazor.1.0.0-alpha.nupkg``` can be find by the package manager.

**Step 5** - Add references to the library in ```BlazorApp.Client```, ```BlazorApp.Server``` and Peachpie projects.

**Step 6** - Add ```<script src="_content/Peachpie.Blazor/php.js"></script>``` to head of index.html located in ```BlazorApp.Client\wwwroot.```

**Step 7** - Add assemblies containing PHP script to ```WebAssemblyHostBuilder``` (located in ```BlazorApp.Client\Pragram.cs```) using the ```AddPhp``` extension method.

**Step 8** - Modify projects (optional).

**Step 9** - Launch the server by ```dotnet run``` in ```BlazorApp\Server\``` folder. 

**Step 10** - Access https://localhost:5001 .

## API

This section picks useful classes and methods up, which are used to provide PHP scripts defined in the Peachpie project. The library provides two approaches, how to use PHP in Blazor. The first one creates a Blazor component by inheriting the ```PhpComponent``` class. The second approach inserts the ```PhpScriptProvider``` component into a Razor page or sets it up as a root component. This component navigates the PHP scripts, evaluates them, and generates page content based on their output. There are also helper classes representing HTML entities, which are useful in the first approach, where the builder has a complex API for generating page content. There are functions handling file management and interoperability between PHP and Javascript.

#### PhpComponent

This class can be inherited in a script to provide Blazor component API in PHP. Every output initiated by echo or similar functions is copied into a browser console. The virtual method of the Blazor component can be freely overridden. The inherited class has to implement the ```BuildRenderTree``` method. It generates the page content using ```PhpTreeBuilder```, which is a wrapper of the original ```RenderTreeBuilder``` and provides adjusted API for PHP use. 

```php
#[\Microsoft\AspNetCore\Components\RouteAttribute("/Asteroids")]
class AsteroidsComponent extends \Peachpie\Blazor\PhpComponent
{  
	...
  public function BuildRenderTree($builder) : void { ... }

  public function OnInitialized() : void { ... }
}
```

##### Helper classes representing HTML

The library provides a collection of helper classes representing HTML entities because ```RenderTreeBuilder```, used in Blazor, is complicated for writing an HTML page. You can see an example of usage in the figure below.

```php
$button= new \Peachpie\Blazor\Tag("button");
$button->attributes["style"]["position"] = "absolute";
$button->attributes["style"]["top"] = "700px";
$button->attributes["style"]["left"] = "100px";
$button->attributes["class"]->add("mybutton");        
        
$button->attributes->addEvent("onclick", function($seq, $builder) {$builder->AddEventMouseCallback($seq, "onclick", function($e) {$this->HandleFire();});});
        
$button->content[] = new \Peachpie\Blazor\Text("Fire");
```
- **BlazorWritable** - Interface defines ```writeWithTreeBuilder``` generating an HTML output. 
- **Tag** - Class represents an HTML tag comprising its name, attributes and inner content. It implements ```BlazorWritable```.  
- **AttributeCollection** - Helper class manages tag attributes in array style. It is used by the ```Tag``` class.
- **ClassBuilder** - Helper class adds and removes class names from the *class* attribute. It is used by the ```AttributeCollection``` class.  
- **CssBuilder** - Helper class builds css style format used in the *style* attribute. It is used by the ```AttributeCollection``` class.  
- **Text** - Class represents an HTML text element. It implements ```BlazorWritable```.  

#### PhpScriptProvider

The component navigates PHP scripts in Peachpie project. It has three modes:

- **Router** - Handles navigation and script routing (Used when it is set as a root component).

- **ScriptProvider** - Provides script routing based on URL (Can be used with the default Blazor router). 
- **Script** - Always navigates one script defined by its parameter. 

The component can preserve Peachpie context to next evaluation. This behaviour is determined by the ```ContextLifetime``` parameter having two modes.

- **OnNavigationChanged** - Every navigation causes a new context, which is a standard behaviour of PHP.
- **Preserved** - The context is preserved until the component is not disposed. 

You can see an example of usage in the figure below. It is a Razor page containing the provider.

```html
@page "/php/{*sth}"
@using Peachpie.Blazor

<PhpScriptProvider ContextLifetime="@SessionLifetime.OnNavigationChanged" Type="@PhpScriptProviderType.ScriptProvider">
    <Navigating>
        <p>Navigating to script</p>
    </Navigating>
    <NotFound>
        <p>Script not found</p>
    </NotFound>
</PhpScriptProvider>

@code
{
    [Parameter]
    public string sth { get; set; }
}
```

##### BlazorContext

The context inherits the original PHP context and adds functionality for managing Blazor. The  functionality includes handling *$GET*, *$POST*, and *$_FILES* superglobals. It includes evaluating HTML forms on a client side, where forms are not sent to a server but it is parsed and handled by PHP scripts inside a browser. It also provides file management like reading uploaded files by forms or creating a new one, which can be downloaded back to a client.  For this purpose, there are the following functions and structure:

- **BrowserFile** - Represents a file, which can be downloaded to a client.

- **DownloadFile** - Downloads a file stored in memory with a specified ID.
- **CreateFile** - Creates a new file with given content, name, and type.
- **GetBrowserFileContent** - Reads a content of a specified file.
- **CreateUrlObject** - Creates an object, which can be navigated by returned URL.

#### Helper classes 

- **Timer** - Wrapper of the .NET ```Timer``` class. It enables to handle the ```Elapsed``` event.
- **addSimpleEventListenner** - Function for adding a simple event listener (does not have any parameters) to C# objects in PHP.
- **addEventListenner** - Function for adding an event listener (has parameters) to C# objects in PHP.

#### Interoperability

- **window.php.callPhp** - Calls a PHP function by given name and parameters passed as JSON. The context has to be presented in Javascript , meaning that the page has to contain one of the components mentioned earlier, and the context has to already know the function (requires at least one script execution).
- **CallJSVoid** - Calls a Javascript void function.
- **CallJS** - Calls a Javascript function.
- **CallJSVoidAsync** - Calls a Javascript void function asynchronously.
- **CallJSAsync** - Calls a Javascript function asynchronously.
