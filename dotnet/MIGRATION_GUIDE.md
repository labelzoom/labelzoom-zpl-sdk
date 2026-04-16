# Migration Guide: Legacy API to Fluent API

This guide helps you migrate from the legacy API to the new fluent API. **Note: The legacy API is still fully supported**, so you can migrate at your own pace.

## Why Migrate?

The new fluent API provides:
- Better IntelliSense support and discoverability
- Type safety at compile time
- Easier to extend with new formats
- More consistent API across different conversion types
- Cleaner separation between source and target formats

## Quick Reference

| Legacy API | New Fluent API |
|------------|----------------|
| `client.PdfToZpl(path)` | `client.Convert().FromPdf(path).ToZpl().ExecuteAsync()` |
| `client.PdfToZplAsync(path, callback)` | `client.Convert().FromPdf(path).ToZpl().StreamAsync(callback)` |

## Migration Examples

### Example 1: Simple PDF to ZPL Conversion

**Before (Legacy API):**
```csharp
using (var client = new LabelzoomClient("YOUR_TOKEN"))
{
    var zpl = await client.PdfToZpl("document.pdf");
    Console.WriteLine(zpl);
}
```

**After (Fluent API):**
```csharp
using (var client = new LabelzoomClient("YOUR_TOKEN"))
{
    var zpl = await client.Convert()
        .FromPdf("document.pdf")
        .ToZpl()
        .ExecuteAsync();
    
    Console.WriteLine(zpl);
}
```

### Example 2: Streaming PDF to ZPL

**Before (Legacy API):**
```csharp
using (var client = new LabelzoomClient("YOUR_TOKEN"))
{
    await client.PdfToZplAsync("large-document.pdf", async (zpl) =>
    {
        Console.WriteLine($"Received: {zpl}");
        await ProcessLabel(zpl);
    });
}
```

**After (Fluent API):**
```csharp
using (var client = new LabelzoomClient("YOUR_TOKEN"))
{
    await client.Convert()
        .FromPdf("large-document.pdf")
        .ToZpl()
        .StreamAsync(async (zpl) =>
        {
            Console.WriteLine($"Received: {zpl}");
            await ProcessLabel(zpl);
        });
}
```

### Example 3: Custom Endpoint

**Before (Legacy API):**
```csharp
using (var client = new LabelzoomClient("YOUR_TOKEN"))
{
    client.Endpoint = "https://custom-api.example.com";
    var zpl = await client.PdfToZpl("document.pdf");
}
```

**After (Fluent API with Builder):**
```csharp
using (var client = new LabelzoomClientBuilder()
    .WithToken("YOUR_TOKEN")
    .WithEndpoint("https://custom-api.example.com")
    .Build())
{
    var zpl = await client.Convert()
        .FromPdf("document.pdf")
        .ToZpl()
        .ExecuteAsync();
}
```

### Example 4: With Cancellation Token

**Before (Legacy API):**
```csharp
var cts = new CancellationTokenSource();
using (var client = new LabelzoomClient("YOUR_TOKEN"))
{
    var zpl = await client.PdfToZpl("document.pdf", cts.Token);
}
```

**After (Fluent API):**
```csharp
var cts = new CancellationTokenSource();
using (var client = new LabelzoomClient("YOUR_TOKEN"))
{
    var zpl = await client.Convert()
        .FromPdf("document.pdf")
        .ToZpl()
        .ExecuteAsync(cts.Token);
}
```

## New Capabilities

The fluent API also enables new conversion types that weren't available before:

### PNG to ZPL Conversion

```csharp
using (var client = new LabelzoomClient("YOUR_TOKEN"))
{
    var zpl = await client.Convert()
        .FromPng("image.png")
        .ToZpl()
        .ExecuteAsync();
}
```

### Using Streams Instead of File Paths

```csharp
using (var client = new LabelzoomClient("YOUR_TOKEN"))
using (var stream = GetPdfStream())
{
    var zpl = await client.Convert()
        .FromPdf(stream)
        .ToZpl()
        .ExecuteAsync();
}
```

## Advanced Configuration with Builder

The new `LabelzoomClientBuilder` provides more configuration options:

```csharp
using (var client = new LabelzoomClientBuilder()
    .WithToken("YOUR_TOKEN")
    .WithEndpoint("https://api.labelzoom.net")
    .WithTimeout(TimeSpan.FromMinutes(5))
    .Build())
{
    // Use client...
}
```

### Dependency Injection Scenarios

```csharp
// In your DI container setup
services.AddSingleton<HttpClient>();

// When creating the client
var httpClient = serviceProvider.GetService<HttpClient>();
using (var client = new LabelzoomClientBuilder()
    .WithToken("YOUR_TOKEN")
    .WithHttpClient(httpClient)
    .WithDisposeHttpClient(false) // Don't dispose the shared HttpClient
    .Build())
{
    // Use client...
}
```

## Gradual Migration Strategy

You don't need to migrate everything at once. Here's a recommended approach:

1. **Phase 1**: Keep using the legacy API for existing code
2. **Phase 2**: Use the fluent API for all new code
3. **Phase 3**: Gradually refactor existing code during maintenance
4. **Phase 4**: (Optional) Complete migration when convenient

## Breaking Changes

**There are NO breaking changes.** The legacy API remains fully functional and supported.

## Performance Considerations

Both APIs use the same underlying implementation, so there is **no performance difference** between them.

## Need Help?

If you encounter any issues during migration:
1. Check the [examples](examples/) directory for working code samples
2. Review the [API Reference](README.md#api-reference) in the README
3. Contact LabelZoom support at [https://www.labelzoom.net](https://www.labelzoom.net)

