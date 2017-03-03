using System;
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
        foreach (var result in link.Links)
        {
          System.Console.WriteLine(result);
        }
      }
      catch (Exception exception)
      {
        System.Console.WriteLine(exception.ToString());
      }
      System.Console.WriteLine("Press any key to exit");
      System.Console.ReadKey();
    }
  }
}