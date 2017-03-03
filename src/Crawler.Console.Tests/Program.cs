using System.Threading.Tasks;
using Crawler.Console.Model;
using System.Linq;
using Xunit;

namespace Crawler.Console.Tests
{
  public class Program
  {
    [Fact]
    public async Task Crawl_IndeedHomePageOnly_32AnyLinksFound()
    {
      // Arrange
      var crawler = new Logic.Crawler();
      var link = new Link("https://www.indeed.co.uk/");
      // Act
      await crawler.CrawlLink(link);
      // Assert
      Assert.NotNull(link.Links);
      Assert.Equal(31, link.Links.Count());
    }

    [Fact]
    public async Task Crawl_IndeedHomePageOnly_24AnchorLinksFound()
    {
      // Arrange
      var crawler = new Logic.Crawler();
      var link = new Link("https://www.indeed.co.uk/");
      // Act
      await crawler.CrawlLink(link);
      // Assert
      Assert.NotNull(link.Links);
      Assert.Equal(23, link.Links.Count(a => a.NodeName == "a"));
    }

    [Fact]
    public async Task Crawl_IndeedHomePageOnly_9ExternalLinksFound()
    {
      // Arrange
      var crawler = new Logic.Crawler();
      var link = new Link("https://www.indeed.co.uk/");
      // Act
      await crawler.CrawlLink(link);
      // Assert
      Assert.NotNull(link.Links);
      Assert.Equal(9, link.Links.Count(a => link.Uri.Host != a.Uri.Host));
    }
  }
}