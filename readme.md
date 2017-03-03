# Crawler

## Steps to run

 * Install DotNetCore
 * `dotnet restore`
 * `dotnet build`
 * `dotnet run` for the `/src/Crawler.Console` directory

## Steps to test

 * `dotnet test` for the `/src/Crawler.Console.Tests` directory

## Steps for coverage

 * `/src/Crawler.Console.Tests/DoCoverage.ps1` script - it will work in Windows, have not yet cross-compiled to OSX/Linux.

## Caveats

 * I've only ran this on Windows (sorry it's what I had access to at the time)
 * I built it in C# .Net Core - so it should be portable
 * Python, Rust or Java were my preferences but the laptop I had with me just had my Windows tool chain stuff on it

## Walkthrough

I built this aganst a single URL first to ensure that it would give the structure of data I wanted in a repeatable manner, I then tested this against a few different URLs. I eventually added recursive link checking with a default timeout of 5 seconds and some semi-realistic browser headers. It seems to work pretty well.

I didn't expand the number of libraries or depth/abstraction of the code because:
 * Keep It Simple principle
 * Single responsibility principle
 * Portability principle
 * Time constraints

This is obviously not designed for large sites since there is a fairly dumb check on recursiveness (it's just a flat in-memory structure to check the URL). And it will only recurse down links that are in the same domain anyway. Additionally, there's no "maximum-depth" setting - I thought about adding it, and I thought about making a few more things configurable, however I wanted to keep within the constraints of the given requirements and not over-engineer it.

This is not a large project, it relies on HtmlAgilityPack to process HTML into an IXPathNavigable document so I can execute an XPATH expression to find any elements with `src` or `href` attributes. I could've used a library to do CSS pathing over the elements but there are performance considerations and XPATH would work just as well. I could have used Regular Expressions on the raw response, but I wanted some assurance that the downloaded content was in fact SGML based and Regular Expressions can get messy (fast).

Only a few peices of information for each link are then gathered:
 * `Uri` - The URI to the resource
 * `Links` - A list of other linked resources from this resource
 * `StatusCode` - To test for dead-links - I know this isn't a requirement but I've been meaning to build something that can check link statuses for XML sitemap generation (for another project)
 * `NodeName` - Initially I had just an enum with Script/Style/Image/Html values but it meant accounting for all the rules at build time. It's probably going to be more useful to determine what the link is from the output (i.e. favicon.ico, is it an image?)
 * `Attributes` - I figured that combined with `NodeName` it would be possible to work out what the function of the linked resource is
 * `AssociatedText` - Just for the sake of pretty output, and context when reading it