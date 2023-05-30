// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;
using Application;
using Domain;
using Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("Hello, World!");
var sc= new ServiceCollection();
var conf = new Configuration();
sc.ConfigureInfrastructure(conf);
sc.ConfigureApplication(conf);
var sp=sc.BuildServiceProvider();
var sender=sp.GetRequiredService<ISender>();
var path = args[0];

await sender.Send(new AddUserCommand(0, Role.Admin));

var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
    .Where(p=>Regex.IsMatch(p, @"\.jpg$|\.png$|\.gif$"));

foreach (var file in files)
{
    var memoryStream = new MemoryStream();

    // Read the file into the memory stream
    await using (var fileStream = new FileStream(file, FileMode.Open))
    {
        fileStream.CopyTo(memoryStream);
    }
    await sender.Send(new AddImageCommand(0, memoryStream));
    Console.WriteLine($"добавлено изображение {file}");
}
