using Garage2._0;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.TestModels.ConferencePlanner;

namespace TestProject;

public class WebTests
    : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    
    public WebTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    
    
    private DbContext GetContext()
    {
        var options = new DbContextOptionsBuilder().
            UseSqlite("Data Source = nameOfYourDatabase.db")
            .Options;
        
        var db = new DbContext(options);

        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();

        return db;
    }
    
  /*  
    [Fact]
    public void Test1()
    {
        // Arrange
        
        // Act
        
        // Assert
    }
*/
    [Theory]
    [InlineData("/")]
    public async Task GetEndpointsReturnSucessful(string url)
    {
        // Arrange
        var client = _factory.CreateClient();
        var db = GetContext();

        // Act
        var response = await client.GetAsync(url);
        
        // Assert
        response.EnsureSuccessStatusCode(); // Status Code 200-299
        Assert.Equal("text/html; charset=utf-8", 
            response.Content.Headers.ContentType.ToString());
    }
}