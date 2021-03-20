# Thesis sketch

Contains the most up-to-date outline.

## Outline draft

1. Introduction 

   - Short introduction into web applications, give a high level overview of the problem (PHP Blazor interop, PHP client side, two languages for writing a web app)
   - Existing projects (pib, maybe SilverLight??)
   - Overview of thesis

2. Existing technologies (maybe 15 - 20 pages)

   - Explain shortly project [pib](https://github.com/oraoto/pib?fbclid=IwAR3KZKXWCC3tlgQf886PF3GT_Hc8pmfCMI1-43gdQEdE5wYgpv070bRwXqI) 

   1. WebAssembly
      - Explain what it is
      - How it works (loading, executing)
   2. Mono
      - Explain what it is
      - Compilation Mono to WebAssembly
      - Describe two modes of compilation (Everything, without .dll)
   3. Blazor
      - Explain what it is
      - Describe two hosting models (Server hosted vs. Blazor WebAssembly)
      - High-level description of  how Blazor WebAssembly works (Mono, WebAssembly, Kestrel??) - what technologies it uses.
      - Describe how Blazor WebAssembly client serves the pages (Pages, Components, Razor, Router,  Navigation, Process of rendering, RenderTreeBuilder, Diff algorithm,..)
   4. PeachPie
      - Explain what it is
      - Describe PHP to .NET compilation
      - interoperability
      - Creating libraries
      - Structs is not supported
      - Custom attributes
   5. PHP
      - Give a short overview of language specification (paradigm, dynamic language, HTML interleaving, Globals, No async functions)
      
      - Standards of writing PHP Web application (Forms, HTML interleaving, Front controller, Database, Sessions, HTTP, JavaScript??)

3. Problem analysis
   - Describe a high-level problem with client side (Two languages for writing Web app, Interesting collaboration with Blazor)
   - Changing paradigm of PHP (Persistent vs. changing context, Session)
   - How to navigate scripts/websites in PHP
   - How to insert PHP into component in C#
   - Speed of rendering
   - No async function in PHP, but Blazor is full of async methods (point to working with files)
   - How to add client dynamic interaction to PHP(Forms, JS interaction).
   - How to handle query, forms, post, get, files...
   - Database connection 

4. Solution

   - Two components

   - Files management

   - Forms transformation

   - Own navigation

   - JS interop

   - query handling and rendering

5. Examples

   - Describe scenarios which fits to my solution

   - Demo 1,...4

6. Experiments

   - Show differences between rendering with both components
   - Compare my solution and [pib](https://github.com/oraoto/pib?fbclid=IwAR3KZKXWCC3tlgQf886PF3GT_Hc8pmfCMI1-43gdQEdE5wYgpv070bRwXqI) (No JSinterop??, Collaboration with Blazor, WebAPI??)
   - Some other metrics like compilation time, time to load page, number of changes to make simple PHP web application client-side 

7. Conclusion

- My solution is good enough to get A