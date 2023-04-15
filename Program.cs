// See https://aka.ms/new-console-template for more information
using PuppeteerSharp;
using RazorLight;

Console.WriteLine("Hello, World!");

var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");

var engine = new RazorLightEngineBuilder()
    .UseEmbeddedResourcesProject(typeof(Program))
    .UseMemoryCachingProvider()
    .UseFileSystemProject(path)
    .Build();
var model = new TestModel{ Title = "Title", IncludeTitle = "IncludeTitle" };
var html = await engine.CompileRenderAsync("Test.cshtml", model);


Console.WriteLine(html);
var pathHtml = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.html");
await File.WriteAllTextAsync(pathHtml, html);

using var browserFetcher = new BrowserFetcher();
await browserFetcher.DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
await using var browser = await Puppeteer.LaunchAsync(
    new LaunchOptions { Headless = true });


await using var page = await browser.NewPageAsync();
await page.SetViewportAsync(new ViewPortOptions
{
    Width = 800,
    Height = 800
});
await page.GoToAsync(pathHtml);

var pathScreenshot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "screenshot.png");
await page.ScreenshotAsync(pathScreenshot);

Console.WriteLine("Нажмите любую клавишу для завершения работы");
Console.ReadLine();
await page.CloseAsync();
await browser.CloseAsync();
public class TestModel
{
    public string? Title { get; init; }
    public string? IncludeTitle { get; init; }
}