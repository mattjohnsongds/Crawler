using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crawler.Console.Model;
using HtmlAgilityPack;

namespace Crawler.Console.Logic
{
  public class Crawler
  {
    private Dictionary<String, Link> cache = new Dictionary<String, Link>();

    public async Task CrawlLink(Link link, Uri parentUri = null)
    {
      if (cache.ContainsKey(link.Uri.AbsoluteUri))
        return;

      cache.Add(link.Uri.AbsoluteUri, link);

      try
      {
        var webClient = new System.Net.Http.HttpClient();
        webClient.DefaultRequestHeaders.Add("Cache-Control", "no-cache");
        webClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36");
        webClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
        webClient.DefaultRequestHeaders.Add("Referer", "https://www.google.co.uk/");
        webClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.8");
        webClient.Timeout = TimeSpan.FromSeconds(5);

        var response = await webClient.GetAsync(link.Uri);
        link.StatusCode = (Int32)response.StatusCode;
        if (link.NodeName == "a" && (parentUri == null || parentUri.Host == link.Uri.Host))
        {
          var responseRaw = await response.Content.ReadAsStringAsync();
          if (!String.IsNullOrWhiteSpace(responseRaw))
          {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(responseRaw);
            if (htmlDocument.DocumentNode != null && htmlDocument.DocumentNode.HasChildNodes)
            {
              link.Links = htmlDocument.DocumentNode.SelectNodes(@"//*[(@src or @href)]").Select(n => ConvertLink(link.Uri, n)).Where(l => l != null).ToList();

              Task.WaitAll(link.Links.Select(l => CrawlLink(l, link.Uri)).ToArray());
            }
          }
        }
      }
      catch (TaskCanceledException)
      {
        link.StatusCode = 408;
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

          if (cache.ContainsKey(link.Uri.AbsoluteUri))
            return cache[link.Uri.AbsoluteUri];

          link.AssociatedText = (node.InnerText ?? String.Empty).Trim();
          link.NodeName = (node.Name ?? String.Empty).ToLowerInvariant();
          link.Attributes = node.Attributes.ToDictionary(a => a.Name, a => a.Value);

          return link;
        }
      }
      return null;
    }
  }
}