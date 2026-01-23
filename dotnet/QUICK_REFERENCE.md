# LabelZoom .NET SDK - Quick Reference

## Installation

```bash
dotnet add package LabelzoomDotnetSdk
```

## Basic Usage

### Simple Client Creation

```csharp
using LabelzoomDotnetSdk;

using (var client = new LabelzoomClient("YOUR_TOKEN"))
{
    // Use client...
}
```

### Client with Builder

```csharp
using (var client = new LabelzoomClientBuilder()
    .WithToken("YOUR_TOKEN")
    .WithEndpoint("https://api.labelzoom.net")
    .WithTimeout(TimeSpan.FromSeconds(30))
    .Build())
{
    // Use client...
}
```

## Conversions

### PDF to ZPL (Complete)

```csharp
var zpl = await client.Convert()
    .FromPdf("document.pdf")
    .ToZpl()
    .ExecuteAsync();
```

### PDF to ZPL (Streaming)

```csharp
await client.Convert()
    .FromPdf("large-document.pdf")
    .ToZpl()
    .StreamAsync(async (label) => {
        Console.WriteLine(label);
    });
```

### PNG to ZPL

```csharp
var zpl = await client.Convert()
    .FromPng("image.png")
    .ToZpl()
    .ExecuteAsync();
```

### Using Streams

```csharp
using (var stream = File.OpenRead("document.pdf"))
{
    var zpl = await client.Convert()
        .FromPdf(stream)
        .ToZpl()
        .ExecuteAsync();
}
```

### With Cancellation Token

```csharp
var cts = new CancellationTokenSource();
var zpl = await client.Convert()
    .FromPdf("document.pdf")
    .ToZpl()
    .ExecuteAsync(cts.Token);
```

## Legacy API (Still Supported)

### Simple Conversion

```csharp
var zpl = await client.PdfToZpl("document.pdf");
```

### Streaming Conversion

```csharp
await client.PdfToZplAsync("document.pdf", async (label) => {
    Console.WriteLine(label);
});
```

## Builder Options

| Method | Description |
|--------|-------------|
| `WithToken(string)` | Set authentication token (required) |
| `WithEndpoint(string)` | Set custom API endpoint |
| `WithTimeout(TimeSpan)` | Set HTTP request timeout |
| `WithHttpClient(HttpClient)` | Provide custom HttpClient |
| `WithDisposeHttpClient(bool)` | Control HttpClient disposal |
| `Build()` | Create the configured client |

## Conversion Methods

### Source Formats

| Method | Description |
|--------|-------------|
| `FromPdf(string path)` | PDF file path |
| `FromPdf(Stream stream)` | PDF stream |
| `FromPng(string path)` | PNG file path |
| `FromPng(Stream stream)` | PNG stream |
| `FromZpl(string content)` | ZPL content (future) |

### Target Formats

| Method | Description |
|--------|-------------|
| `ToZpl()` | Convert to ZPL |
| `ToPdf()` | Convert to PDF (future) |
| `ToPng()` | Convert to PNG (future) |

### Execution Methods

| Method | Description |
|--------|-------------|
| `ExecuteAsync(CancellationToken)` | Get complete result |
| `StreamAsync(Func<string, Task>, CancellationToken)` | Stream results |

## Error Handling

```csharp
try
{
    var zpl = await client.Convert()
        .FromPdf("document.pdf")
        .ToZpl()
        .ExecuteAsync();
}
catch (FileNotFoundException ex)
{
    Console.WriteLine($"File not found: {ex.Message}");
}
catch (HttpRequestException ex)
{
    Console.WriteLine($"API error: {ex.Message}");
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Configuration error: {ex.Message}");
}
```

## Common Patterns

### Dependency Injection

```csharp
// Startup.cs
services.AddSingleton<IHttpClientFactory, HttpClientFactory>();
services.AddScoped<LabelzoomClient>(sp => {
    var httpClient = sp.GetService<IHttpClientFactory>().CreateClient();
    return new LabelzoomClientBuilder()
        .WithToken(Configuration["LabelZoom:Token"])
        .WithHttpClient(httpClient)
        .WithDisposeHttpClient(false)
        .Build();
});
```

### Batch Processing

```csharp
var files = Directory.GetFiles("pdfs", "*.pdf");
foreach (var file in files)
{
    var zpl = await client.Convert()
        .FromPdf(file)
        .ToZpl()
        .ExecuteAsync();
    
    await File.WriteAllTextAsync(
        Path.ChangeExtension(file, ".zpl"), 
        zpl
    );
}
```

### Streaming to Printer

```csharp
await client.Convert()
    .FromPdf("labels.pdf")
    .ToZpl()
    .StreamAsync(async (label) => {
        await SendToPrinter(label);
    });
```

## Best Practices

1. **Use `using` statements** to ensure proper disposal
2. **Reuse client instances** for multiple conversions
3. **Use streaming** for large documents (>10 pages)
4. **Use cancellation tokens** for long-running operations
5. **Handle exceptions** appropriately
6. **Use builder pattern** for complex configurations

## Links

- [Full Documentation](README.md)
- [Migration Guide](MIGRATION_GUIDE.md)
- [Architecture](ARCHITECTURE.md)
- [Examples](examples/)

