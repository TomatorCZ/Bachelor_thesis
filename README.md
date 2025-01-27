<h1 align="center">
Client-side execution of PHP applications compiled to .NET
</h1>

### Introduction

Blazor is a new technology enabling to run .NET applications directly in the browser using WebAssembly, a recently created binary instruction format adopted by major web browsers. Whilst PHP is the most popular language in the realm of web applications, it cannot run directly in the browser. The PeachPie compiler provides a way to compile projects written in PHP into Common Intermediate Language (CIL), enabling them to run on the .NET platform.
This thesis aims to design and implement a convenient interface between Blazor and compiled PHP, enabling developers to create client-side PHP applications. These applications would be able to utilize the specifics of the client-side paradigm, such as fast response times, the possibility to preserve the application state between the requests more efficiently and the direct access to the Document Object Model (DOM) of the page. To demonstrate the usability of the implementation and the specific benefits of the solution, a pilot interactive application will be created.

### More information

See  docs/thesis/thesis.pdf to get more information.