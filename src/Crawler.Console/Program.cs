using System;
using System.Collections.Generic;
using System.Text;
using Crawler.Console.Model;

namespace Crawler.Console
{
  public class Program
  {
    public static void Main(String[] args)
    {
      try
      {
        System.Console.WriteLine("Enter the uri to crawl");
        var uri = System.Console.ReadLine();
        var crawler = new Logic.Crawler();
        var link = new Link(uri);
        var task = crawler.CrawlLink(link);
        task.Wait();

        var output = PrintTree(link.Links);
        System.IO.File.WriteAllText(".\\output.txt", output);

        System.Console.WriteLine("Output saved to .\\output.txt");
        System.Console.WriteLine($@"Found a total of {printCache.Count} links");
      }
      catch (Exception exception)
      {
        System.Console.WriteLine(exception.ToString());
      }
      System.Console.WriteLine("Press any key to exit");
      System.Console.ReadKey();
    }

    private static List<String> printCache;

    private static String PrintTree(List<Link> links, Int32 level = 0)
    {
      if (level == 0)
        printCache = new List<String>();

      var stringBuilder = new StringBuilder();
      if (links != null && links.Count > 0)
      {
        foreach (var link in links)
        {
          stringBuilder.AppendFormat("{0}Uri:  {1}\r\n{0}Text: {2}\r\n{0}Links: {3}\r\n{0}Node: {4}\r\n{0}----\r\n", "".PadLeft(2 * level, ' '), link.Uri.AbsoluteUri, link.AssociatedText, link.Links?.Count ?? 0, link.NodeName);

          if (!printCache.Contains(link.Uri.AbsoluteUri))
          {
            printCache.Add(link.Uri.AbsoluteUri);
            stringBuilder.Append(PrintTree(link.Links, level + 1));
          }
        }
      }
      return stringBuilder.ToString();
    }
  }
}