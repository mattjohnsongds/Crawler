using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crawler.Console
{
  public class Crawler
  {
    public async Task<List<String>> Crawl(String uri)
    {
      try
      {
        var webClient = new System.Net.Http.HttpClient();
        var response = await webClient.GetAsync(uri);
        var responseRaw = await response.Content.ReadAsStringAsync();
        var htmlDocument = new HtmlAgilityPack.HtmlDocument();
        htmlDocument.LoadHtml(responseRaw);
        return htmlDocument.DocumentNode.SelectNodes(@"//a").Select(node => node.Attributes["href"].Value).ToList();
      }
      catch (Exception exception)
      {
        System.Console.WriteLine(exception.ToString());
        return null;
      }
    }
  }
}