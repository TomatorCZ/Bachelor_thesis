# Overview

I decided to aim at 3 scenarios.

- Scenario 1. - We have some heavy application (like a game) written in PHP and we want to add it as a part of an existing website in Blazor.
- Scenario 2. - We want to move whole PHP web (static) to the browser. 
- Scenario 3. - We have a part of PHP web using database and we want to securely access it from a client through the server.

## Scenario 1.

We want to develop a component, which offers a possibility to render a page by PHP in comfortable way. The component should be the best choice, because we want a part, which is rendered by PHP, to be inserted into Blazor(by router or just an another page). There are two ways how to achieve this. 

We make a custom component, which will seek out scripts to be evaluated. Advantage of this approach is eliminate Blazor's interface from a programmer. The programmer just render the page by echo (or php interleaving).



The ComponentBase can be derived in PHP due to Peachpie and we can call RenderBuilderTree by myself. Advantage of this is a full control of rendering.



Also It will be nice to have a choice call PHP functions from Blazor and Javascript.

### Demo

Simple game of type Space Invaders. There will be a version for each approach. Each version will contain a way how to call Javascript a how Javascript can call PHP. 

## Scenario 2.

There is a several conditions, which we have to consider. We already have some functional PHP web (e.g. implemented by Front Controller pattern ) and we want to deploy it to browser. Or we have just PHP pages and wants to glue it together. And we want a opportunity somehow to interop with Javascript. 

So we want to develop a Router, which will serves these pages and a mechanism, which can call PHP functions by Javascript.    

### Demo

There will be two versions. The first will use the Router as an bridge between C# and PHP. The second will use a Router as Front Controller. Each version will contain a way how to call Javascript a how Javascript can call PHP. 

## Scenario 3.

We want some PHP helper class, which will offer an API for sending and receiving http request from browser.  

### Demo

There will be a demo, which will a server like a middleware between client and database.