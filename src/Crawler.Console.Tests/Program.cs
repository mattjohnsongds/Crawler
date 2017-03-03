using System;
using Xunit;

namespace Crawler.Console.Tests
{
  public class Program
  {
    [Fact]
    public void Crawl_IndeedHomePageOnly_24LinksFound()
    {
      // Arrange
      var crawler = new Crawler();
      // Act
      var results = crawler.Crawl("https://www.indeed.co.uk/");
      // Assert
      Assert.NotNull(results);
      Assert.Equal(24, results.Count);
    }
  }
}