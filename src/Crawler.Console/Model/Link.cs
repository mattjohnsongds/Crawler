using System;
using System.Collections.Generic;

namespace Crawler.Console.Model
{
  public class Link
  {
    public Link() { }

    public Link(String uri)
    {
      Uri = new Uri(uri);
      NodeName = "a";
    }

    public Uri Uri { get; set; }
    public List<Link> Links { get; set; }
    public Int32 StatusCode { get; set; }
    public String NodeName { get; set; }
    public Dictionary<String, String> Attributes { get; set; }
    public String AssociatedText { get; set; }
  }
}