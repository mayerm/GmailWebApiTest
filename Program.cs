using WebApplicationTest;

Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(configure: static webBuilder =>
{
    webBuilder.UseStartup<Startup>();
}).Build().RunAsync().GetAwaiter().GetResult();
