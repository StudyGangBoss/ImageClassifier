using System.Net.Mime;
using Application;
using Domain;
using FluentAssertions;
using Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Tests;

public class Tests
{
    private ServiceProvider sp = null!;

    [SetUp]
    public void Setup()
    {
        var configuration = new Configuration();
        var sc = new ServiceCollection();
        sc.ConfigureApplication(configuration);
        sc.ConfigureInfrastructure(configuration);
        sp = sc.BuildServiceProvider();
    }

    [Test]
    public async Task Test1()
    {
        var sender=sp.GetRequiredService<ISender>();
        var user=await sender.Send(new AddUserCommand(1));
        user.ChatId.Should().Be(1);
    }
    
    [Test]
    public async Task Test2()
    {
        var sender=sp.GetRequiredService<IReadRepository<Image>>();
    }
    
    [Test]
    public async Task Test3()
    {
        var sender=sp.GetRequiredService<IReadRepository<UserInfo>>();
    }
}