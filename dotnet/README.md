![LabelZoom Logo](../docs/LabelZoom_Logo_f_400px.png)

# LabelZoom .NET SDK

A full-featured .NET SDK for converting documents and images to ZPL format using the LabelZoom API.

## Features

- **Fluent API**: Modern, discoverable API with method chaining (e.g., `client.Convert().FromPdf().ToZpl()`)
- **Builder Pattern**: Configure clients with `LabelzoomClientBuilder` for advanced scenarios
- **PDF to ZPL Conversion**: Convert PDF documents to ZPL format
- **PNG to ZPL Conversion**: Convert PNG images to ZPL format
- **Streaming Support**: Efficiently handle large multi-page documents with streaming callbacks
- **.NET Standard 2.0**: Compatible with .NET Framework 4.6.1+, .NET Core 2.0+, and .NET 5+
- **Cancellation Token Support**: Cancel long-running operations gracefully
- **Extensible Design**: Easy to add new format conversions without breaking existing code
- **Backward Compatible**: Existing `PdfToZpl()` methods continue to work

## Installation

### From Source

```bash
git clone https://github.com/labelzoom/labelzoom-zpl-sdk.git
cd labelzoom-zpl-sdk/dotnet
dotnet build LabelzoomDotnetSdk.sln
```

### NuGet Package

*(Coming soon)*

## Quick Start

### Fluent API (Recommended)

#### Basic PDF to ZPL Conversion

```csharp
using LabelzoomDotnetSdk;

using (var client = new LabelzoomClient("YOUR_AUTH_TOKEN"))
{
    var zpl = await client.Convert()
        .FromPdf("path/to/document.pdf")
        .ToZpl()
        .ExecuteAsync();

    Console.WriteLine(zpl);
}
```

#### Streaming PDF to ZPL (for large documents)

```csharp
using LabelzoomDotnetSdk;

using (var client = new LabelzoomClient("YOUR_AUTH_TOKEN"))
{
    await client.Convert()
        .FromPdf("path/to/large-document.pdf")
        .ToZpl()
        .StreamAsync(async (zpl) =>
        {
            // Process each label as it's received
            Console.WriteLine($"Received label: {zpl.Substring(0, 50)}...");
            // Send to printer, save to file, etc.
        });
}
```

#### PNG to ZPL Conversion

```csharp
using LabelzoomDotnetSdk;

using (var client = new LabelzoomClient("YOUR_AUTH_TOKEN"))
{
    var zpl = await client.Convert()
        .FromPng("path/to/image.png")
        .ToZpl()
        .ExecuteAsync();

    Console.WriteLine(zpl);
}
```

### Using the Builder Pattern

For advanced configuration scenarios:

```csharp
using LabelzoomDotnetSdk;

using (var client = new LabelzoomClientBuilder()
    .WithToken("YOUR_AUTH_TOKEN")
    .WithEndpoint("https://custom-api.example.com")
    .WithTimeout(TimeSpan.FromSeconds(30))
    .Build())
{
    var zpl = await client.Convert()
        .FromPdf("path/to/document.pdf")
        .ToZpl()
        .ExecuteAsync();

    Console.WriteLine(zpl);
}
```

### Legacy API (Still Supported)

The original API methods continue to work for backward compatibility:

```csharp
using LabelzoomDotnetSdk;

using (var client = new LabelzoomClient("YOUR_AUTH_TOKEN"))
{
    // Simple conversion
    var zpl = await client.PdfToZpl("path/to/document.pdf");
    Console.WriteLine(zpl);

    // Streaming conversion
    await client.PdfToZplAsync("path/to/document.pdf", async (zpl) =>
    {
        Console.WriteLine($"Received label: {zpl.Substring(0, 50)}...");
    });
}
```

## API Reference

### LabelzoomClientBuilder

The builder pattern for creating configured `LabelzoomClient` instances.

#### Methods

##### WithToken(string token)
Sets the authentication token for the LabelZoom API.

##### WithEndpoint(string endpoint)
Sets a custom API endpoint URL (default: `https://api.labelzoom.net`).

##### WithTimeout(TimeSpan timeout)
Sets the HTTP request timeout.

##### WithHttpClient(HttpClient httpClient)
Provides a custom `HttpClient` instance (useful for dependency injection scenarios).

##### WithDisposeHttpClient(bool dispose)
Controls whether the client should dispose the `HttpClient` when disposed.

##### Build()
Builds and returns a configured `LabelzoomClient` instance.

### LabelzoomClient

#### Constructors

```csharp
public LabelzoomClient(string token)
```

Creates a new instance with the specified authentication token.

#### Properties

- `string Endpoint` - The API endpoint URL (default: `https://api.labelzoom.net`)

#### Fluent API Methods

##### Convert()

```csharp
public ConversionRequestBuilder Convert()
```

Returns a fluent conversion request builder for specifying source and target formats.

**Example:**
```csharp
var zpl = await client.Convert().FromPdf("doc.pdf").ToZpl().ExecuteAsync();
```

##### Source Format Methods

- `FromPdf(string path)` - Specify a PDF file as the source
- `FromPdf(Stream stream)` - Specify a PDF stream as the source
- `FromPng(string path)` - Specify a PNG file as the source
- `FromPng(Stream stream)` - Specify a PNG stream as the source
- `FromZpl(string content)` - Specify ZPL content as the source (for future conversions)

##### Target Format Methods

- `ToZpl()` - Convert to ZPL format
  - `.ExecuteAsync(CancellationToken ct = default)` - Execute and return complete result
  - `.StreamAsync(Func<string, Task> callback, CancellationToken ct = default)` - Stream results

#### Legacy Methods (Backward Compatible)

##### PdfToZpl

```csharp
public async Task<string> PdfToZpl(string pdfPath, CancellationToken ct = default)
```

Converts a PDF document to a single ZPL string. Best used for smaller documents with fewer pages.

**Parameters:**
- `pdfPath` - Path to the PDF file
- `ct` - Optional cancellation token

**Returns:** ZPL string containing all labels

##### PdfToZplAsync

```csharp
public async Task PdfToZplAsync(string pdfPath, Func<string, Task> onLabelAsync, CancellationToken ct = default)
```

Converts a PDF document to ZPL and streams the response one label at a time. Best used for larger documents with many pages.

**Parameters:**
- `pdfPath` - Path to the PDF file
- `onLabelAsync` - Callback function invoked for each label
- `ct` - Optional cancellation token

## Design Benefits

### Why the Fluent API?

The new fluent API provides several advantages:

1. **Type Safety**: The compiler ensures you specify both source and target formats
2. **Discoverability**: IntelliSense guides you through available options
3. **Extensibility**: New formats can be added without method explosion
4. **Separation of Concerns**: Input and output formats are independently configurable
5. **Clean Code**: Reads naturally like English: "Convert from PDF to ZPL"

### Comparison

**Old API:**
```csharp
var zpl = await client.PdfToZpl("document.pdf");
```

**New Fluent API:**
```csharp
var zpl = await client.Convert().FromPdf("document.pdf").ToZpl().ExecuteAsync();
```

Both work! The new API is more verbose but scales better as more formats are added.

## Building

```bash
cd dotnet
dotnet build LabelzoomDotnetSdk.sln
```

## Running Tests

```bash
cd dotnet
dotnet test
```

## Authentication

You need a LabelZoom API token to use this SDK. Contact [LabelZoom](https://www.labelzoom.net) to obtain an API token.

## Examples

Comprehensive examples demonstrating the fluent API and builder pattern can be found in the `examples/` directory.

Additional code snippets can be found in the `snippets/` directory.

## License

This project is licensed under the BSD 3-Clause License - see the [LICENSE](../LICENSE) file for details.

## Support

For questions or support, visit [LabelZoom](https://www.labelzoom.net) or contact the LabelZoom team.

