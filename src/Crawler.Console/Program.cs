using System;

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
        var crawler = new Crawler();
        var task = crawler.Crawl(uri);
        task.Wait();
        var results = task.Result;
        foreach (var result in results)
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