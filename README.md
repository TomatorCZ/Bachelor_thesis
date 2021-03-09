

## Status

20.2.2021 - Scenario 2 is functional and can be reviewed for the first time.

22.2.2021 - Scenario 1 was set and it is in progress. 

28.2.2021 - Scenario 1 is functional and can be reviewed for the first time

4.3.2021 - Scenario 3 is almost done. 

5.3.2021 - Scenario 1,2,3 is done and can be reviewed.

## Project structure

The project is divided into 3 folders

- Playground - There are projects, which can be broken and they are used for experiments.
- Scenarios - There are projects which represents individual scenarios. See the section <a href="#scenarios">Scenarios</a>.
- Source - There is a project(s) which is shared by scenarios and are used for supporting to use PHP in Blazor.

## API & Architecture

To be determined.

## TODO

- [ ] [FEATURE] Make rendering, which is implemented by Tag class in [blazorUtilities.php](https://github.com/TomatorCZ/Bachelor_thesis/blob/main/Scenarios/Scenario2/Asteroids/Php/blazorUtilities.php) smarter.
- [ ] [FEATURE] Think of a better way how to supply static files in Peachpie libraries.
- [ ] [FEATURE] Make file up/downloading as streams
- [ ] [FEATURE] Lazy loading assemblies

## Scenarios

I made five possible scenarios, which demonstrates possible ways how to use PHP in Blazor. The first three scenarios are the point of interest. It will be determined later, if the next two scenarios are necessary to show.

| Scenario | Description                                                  |
| -------- | ------------------------------------------------------------ |
| 1        | We have a part of application, which is better to write in PHP (Existing library, PHP syntax, etc..). So we want a opportunity to plug this part into an existing Blazor application. |
| 2        | We have a part of application, which is heavy and the rendering is crucial. So we want more control above the rendering in PHP scripts and has an opportunity to plug it into an existing Blazor application. |
| 3        | We want to write while web application in PHP, but the application should run on the client side. |
| 4        | We have some common structures in PHP on the client side, which should be serialized, sent to server, and deserialized. |
| 5        | We have a classical 3 layer application (Client-Server-Database) and we  want to securely work with the database on the client side. |

It will be created demo in other to try and evaluate the approaches above.

### Scenario 1

We want to develop a custom Blazor component, which will offer a way how to navigate scripts, which are defined by a Peachpie assembly and render them. The component should be the best choice because we want a part, which is written in PHP, to be inserted into Blazor(by router or just an another page). The rendering will be provided by PHP interleaving and echo function. This approach has an advantage that the scripts are just common PHP scripts. The programmer of these scripts can have no information about Blazor.

Purpose of this scenario is to provide a simple and fast way, how to write a part of Blazor application in PHP.

Demo will use PHP library (gd2 graphics) to process an image, which will be uploaded by client.

#### Known issues

- Image uploading is slow due to converting whole file into base64 string. Can be improved by streams...
- Calling custom js functions is limited due to strong type of Blazor(params object[] doesn't work properly)
- Unable to insert url with builder as addMarkUp()...

### Scenario 2

We want to have more control above the rendering. So we can derive ComponentBase in PHP and build the Render tree by ourselves. There is an advantage of faster rendering than in scenario 1. A disadvantage of this is a harder implementation of correct usage of RenderTreeBuilder and  having a knowledge about how Blazor works. Helper classes will be created in other to make it easy for PHP programmer.

Purpose of this scenario is to provide a way, how to render an application which rendering is crucial.

Demo will contain a game in PHP (like Space Invaders) and will execute it in the real time. There will be a comparison between scenario 1 and 2.

### Scenario 3

We want to write whole web application in PHP. So we create a special router for navigating these pages in Peachpie assembly. This router can behaves in two different ways. The context of PHP will be persistent or it will be reset after each navigation.

Purpose of this scenario is to move PHP web to client and save the resources of the server.

Demo will contain a simple web, which will demonstrate usage of the router.

### Scenario 4

will be determined later.

### Scenario 5

will be determined later.

### Appendix

Also It will be nice to have a choice call PHP functions from Blazor or Javascript. And make a option to call Javascript from PHP and react to events 