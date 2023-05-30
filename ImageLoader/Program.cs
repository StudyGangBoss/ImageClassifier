// See https://aka.ms/new-console-template for more information

using Application;
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

var imageInfos=(await sender.Send(new GetImageInfoCommand())).ToArray();
for (var i = 0; i< imageInfos.Length;i++ )
{
    var image=await sender.Send(new GetImageCommand(imageInfos[i].ImageId));
    if( imageInfos[i].Classes.All(c=>c.Value==0))
    {
        var pathTo = Path.Join(path, "пусто");
        if (!Directory.Exists(pathTo))
            Directory.CreateDirectory(pathTo!);
        using var ms = new MemoryStream(image.ImageData);
        await using var fs = new FileStream(Path.Join(pathTo, $"{i}.jpg"), FileMode.Create);
        ms.WriteTo(fs);
    }
    foreach (var @class in imageInfos[i].Classes)
    {
        var pathTo = Path.Join(path, $"{@class.Type}");
        Console.WriteLine($"saving image to {pathTo}");
        if(@class.Value==1)
        {
            if (!Directory.Exists(pathTo))
                Directory.CreateDirectory(pathTo!);
            using var ms = new MemoryStream(image.ImageData);
            await using var fs = new FileStream(Path.Join(pathTo,$"{i}.jpg"), FileMode.Create);
            ms.WriteTo(fs);
        }
    }
}
