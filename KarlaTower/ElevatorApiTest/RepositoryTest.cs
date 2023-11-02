using KarlaTower;
using KarlaTower.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ElevatorApiTest;

public class RepositoryTest
{
    [Fact]
    public void TestGet()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddServices();
        var provider = serviceCollection.BuildServiceProvider();

        var musicRepository = provider.GetService<IMusicRepository>() ?? throw new NullReferenceException();
        
        Assert.NotEmpty(musicRepository.Songs);
    }
}