# LabelZoom .NET SDK Architecture

## Overview

The LabelZoom .NET SDK uses a fluent builder pattern to provide a clean, type-safe API for converting between different document formats.

## Architecture Diagram

```
┌─────────────────────────────────────────────────────────────┐
│                    LabelzoomClientBuilder                    │
│  ┌────────────────────────────────────────────────────────┐ │
│  │ .WithToken(string)                                     │ │
│  │ .WithEndpoint(string)                                  │ │
│  │ .WithTimeout(TimeSpan)                                 │ │
│  │ .WithHttpClient(HttpClient)                            │ │
│  │ .Build() → LabelzoomClient                             │ │
│  └────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                      LabelzoomClient                         │
│  ┌────────────────────────────────────────────────────────┐ │
│  │ .Convert() → ConversionRequestBuilder                  │ │
│  │ .PdfToZpl() [Legacy]                                   │ │
│  │ .PdfToZplAsync() [Legacy]                              │ │
│  └────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                 ConversionRequestBuilder                     │
│  ┌────────────────────────────────────────────────────────┐ │
│  │ .FromPdf(string|Stream) → PdfSourceBuilder             │ │
│  │ .FromPng(string|Stream) → PngSourceBuilder             │ │
│  │ .FromZpl(string) → ZplSourceBuilder                    │ │
│  └────────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
                            │
              ┌─────────────┼─────────────┐
              ▼             ▼             ▼
    ┌─────────────┐ ┌─────────────┐ ┌─────────────┐
    │PdfSource    │ │PngSource    │ │ZplSource    │
    │Builder      │ │Builder      │ │Builder      │
    │             │ │             │ │             │
    │.ToZpl()     │ │.ToZpl()     │ │.ToPdf()     │
    │.ToPdf()     │ │             │ │.ToPng()     │
    └─────────────┘ └─────────────┘ └─────────────┘
              │             │             │
              └─────────────┼─────────────┘
                            ▼
              ┌─────────────────────────┐
              │   Target Builders       │
              ├─────────────────────────┤
              │ ZplTargetBuilder        │
              │  .ExecuteAsync()        │
              │  .StreamAsync()         │
              │                         │
              │ PdfTargetBuilder        │
              │  .ExecuteAsync()        │
              │                         │
              │ PngTargetBuilder        │
              │  .ExecuteAsync()        │
              └─────────────────────────┘
```

## Design Patterns

### 1. Builder Pattern (LabelzoomClientBuilder)

The builder pattern is used to construct `LabelzoomClient` instances with various configuration options:

```csharp
var client = new LabelzoomClientBuilder()
    .WithToken("token")
    .WithEndpoint("https://api.example.com")
    .WithTimeout(TimeSpan.FromSeconds(30))
    .Build();
```

**Benefits:**
- Fluent, readable configuration
- Optional parameters without constructor overload explosion
- Validation at build time
- Immutable client once built

### 2. Fluent Interface (Conversion API)

The conversion API uses a fluent interface to separate source format from target format:

```csharp
var result = await client.Convert()
    .FromPdf("input.pdf")
    .ToZpl()
    .ExecuteAsync();
```

**Benefits:**
- Type-safe: compiler enforces correct method chains
- Discoverable: IntelliSense guides the user
- Extensible: new formats don't require new top-level methods
- Readable: code reads like natural language

### 3. Strategy Pattern (Format Conversion)

Each source/target combination represents a different conversion strategy:

- `PdfSourceBuilder` → `ZplTargetBuilder` = PDF to ZPL conversion
- `PngSourceBuilder` → `ZplTargetBuilder` = PNG to ZPL conversion
- `ZplSourceBuilder` → `PdfTargetBuilder` = ZPL to PDF conversion (future)

**Benefits:**
- Each conversion path is independent
- Easy to add new conversions
- Clear separation of concerns

## Class Responsibilities

### LabelzoomClientBuilder
- Validates configuration options
- Creates configured `LabelzoomClient` instances
- Manages `HttpClient` lifecycle options

### LabelzoomClient
- Entry point for all API operations
- Manages HTTP communication
- Provides both fluent and legacy APIs
- Implements `IDisposable` for resource cleanup

### ConversionRequestBuilder
- Entry point for fluent conversion API
- Routes to appropriate source builders
- Validates input parameters

### Source Builders (PdfSourceBuilder, PngSourceBuilder, ZplSourceBuilder)
- Hold source data (file path or stream)
- Provide methods to select target format
- Create appropriate target builders

### Target Builders (ZplTargetBuilder, PdfTargetBuilder, PngTargetBuilder)
- Execute the actual API calls
- Handle both streaming and non-streaming responses
- Manage resource cleanup (streams, etc.)

## Extension Points

### Adding a New Source Format

1. Create a new source builder (e.g., `ImageSourceBuilder.cs`)
2. Add a `FromImage()` method to `ConversionRequestBuilder`
3. Implement target format methods in the source builder

### Adding a New Target Format

1. Create a new target builder (e.g., `SvgTargetBuilder.cs`)
2. Add `ToSvg()` methods to relevant source builders
3. Implement the conversion logic in the target builder

## Backward Compatibility

The SDK maintains backward compatibility by keeping the original methods:

```csharp
// Old API - still works
var zpl = await client.PdfToZpl("document.pdf");

// New API - recommended
var zpl = await client.Convert().FromPdf("document.pdf").ToZpl().ExecuteAsync();
```

Both APIs use the same underlying HTTP client and configuration, ensuring consistent behavior.

## Thread Safety

- `LabelzoomClient` is thread-safe for read operations
- Multiple concurrent conversions can be performed with the same client instance
- `HttpClient` is shared and reused across requests (following best practices)

## Resource Management

- `LabelzoomClient` implements `IDisposable`
- Automatically disposes `HttpClient` unless configured otherwise
- File streams are properly disposed after use
- Supports `using` statements for automatic cleanup

