using System;
using System.Linq;
using System.Threading.Tasks;
using Crawler.Console.Model;
using HtmlAgilityPack;

namespace Crawler.Console.Logic
{
  public class Crawler
  {
    public async Task CrawlLink(Link link)
    {
      try
      {
        var webClient = new System.Net.Http.HttpClient();
        webClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
        webClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36");
        webClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
        webClient.DefaultRequestHeaders.Add("Referer", "https://www.google.co.uk/");
        webClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.8");

        var response = await webClient.GetAsync(link.Uri);
        link.StatusCode = (Int32)response.StatusCode;
        var responseRaw = await response.Content.ReadAsStringAsync();
        var htmlDocument = new HtmlDocument();
        htmlDocument.LoadHtml(responseRaw);
        link.Links = htmlDocument.DocumentNode.SelectNodes(@"//*[(@src or @href)]").Select(n => ConvertLink(link.Uri, n)).Where(l => l != null).ToList();
      }
      catch (Exception exception)
      {
        System.Console.WriteLine(exception.ToString());
      }
    }

    public Link ConvertLink(Uri parentUri, HtmlNode node)
    {
      if (node != null && node.Attributes.Count > 0)
      {
        var linkValue = String.Empty;
        if (node.Attributes.Any(a => a != null && a.Name == "src"))
          linkValue = node.Attributes["src"].Value;
        else if (node.Attributes.Any(a => a != null && a.Name == "href"))
          linkValue = node.Attributes["href"].Value;
        if (!String.IsNullOrWhiteSpace(linkValue))
        {
          var link = new Link();
          if (linkValue.Contains("://") || linkValue.ToLowerInvariant().StartsWith("data:"))
            link.Uri = new Uri(linkValue);
          else if (linkValue.StartsWith("/"))
            link.Uri = new Uri($@"{parentUri.Scheme}://{parentUri.Host}{linkValue}");
          else
            link.Uri = new Uri(parentUri.AbsoluteUri.Substring(0, parentUri.AbsoluteUri.LastIndexOf("/", StringComparison.Ordinal)) + linkValue);

          link.AssociatedText = node.InnerText;
          link.NodeName = (node.Name ?? String.Empty).ToLowerInvariant();
          link.Attributes = node.Attributes.ToDictionary(a => a.Name, a => a.Value);

          return link;
        }
      }
      return null;
    }
  }
}