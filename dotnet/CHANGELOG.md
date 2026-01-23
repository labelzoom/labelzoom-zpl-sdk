# Changelog

All notable changes to the LabelZoom .NET SDK will be documented in this file.

## [Unreleased]

### Added - Builder Pattern & Fluent API

#### New Classes
- **LabelzoomClientBuilder** - Fluent builder for creating configured client instances
- **LabelzoomClientOptions** - Configuration options for the client
- **ConversionRequestBuilder** - Entry point for fluent conversion API
- **PdfSourceBuilder** - Handles PDF source conversions
- **PngSourceBuilder** - Handles PNG source conversions
- **ZplSourceBuilder** - Handles ZPL source conversions (placeholder for future)
- **ZplTargetBuilder** - Executes conversions to ZPL format
- **PdfTargetBuilder** - Placeholder for PDF target conversions
- **PngTargetBuilder** - Placeholder for PNG target conversions

#### New Features
- Fluent API for conversions: `client.Convert().FromPdf().ToZpl().ExecuteAsync()`
- Builder pattern for client configuration
- Support for Stream-based conversions (in addition to file paths)
- PNG to ZPL conversion support
- Configurable HttpClient for dependency injection scenarios
- Improved HttpClient lifecycle management
- Enhanced XML documentation throughout

#### New Documentation
- **ARCHITECTURE.md** - Detailed architecture and design patterns
- **MIGRATION_GUIDE.md** - Guide for migrating from legacy API
- **QUICK_REFERENCE.md** - Quick reference card for common operations
- **IMPLEMENTATION_SUMMARY.md** - Summary of implementation details
- **examples/FluentApiExamples.cs** - Comprehensive usage examples
- Updated **README.md** with fluent API documentation

#### New Tests
- **LabelzoomClientBuilderTests.cs** - Tests for builder pattern
- **FluentApiTests.cs** - Tests for fluent conversion API

### Changed
- **LabelzoomClient** - Added internal constructor for builder pattern
- **LabelzoomClient** - Added `Convert()` method for fluent API
- **LabelzoomClient** - Improved disposal logic for HttpClient
- **LabelzoomClient** - Enhanced XML documentation

### Backward Compatibility
- ✅ All existing methods (`PdfToZpl`, `PdfToZplAsync`) remain unchanged
- ✅ No breaking changes to public API
- ✅ Existing code continues to work without modification

## API Comparison

### Before (Legacy API - Still Works!)
```csharp
using (var client = new LabelzoomClient("TOKEN"))
{
    var zpl = await client.PdfToZpl("document.pdf");
}
```

### After (New Fluent API - Recommended)
```csharp
using (var client = new LabelzoomClient("TOKEN"))
{
    var zpl = await client.Convert()
        .FromPdf("document.pdf")
        .ToZpl()
        .ExecuteAsync();
}
```

### With Builder Pattern
```csharp
using (var client = new LabelzoomClientBuilder()
    .WithToken("TOKEN")
    .WithEndpoint("https://api.labelzoom.net")
    .WithTimeout(TimeSpan.FromSeconds(30))
    .Build())
{
    var zpl = await client.Convert()
        .FromPdf("document.pdf")
        .ToZpl()
        .ExecuteAsync();
}
```

## Benefits of New API

1. **Type Safety** - Compiler enforces correct method chains
2. **Discoverability** - IntelliSense guides through available options
3. **Extensibility** - Easy to add new formats without breaking changes
4. **Clarity** - Explicit separation of source and target formats
5. **Flexibility** - Support for both files and streams
6. **Configuration** - Advanced client configuration via builder

## Migration Path

No migration required! The legacy API continues to work. Use the new fluent API for new code and migrate existing code gradually.

See [MIGRATION_GUIDE.md](MIGRATION_GUIDE.md) for detailed migration examples.

## Future Enhancements

Planned features (placeholders already in place):
- ZPL to PDF conversion
- ZPL to PNG conversion
- Additional image format support
- IAsyncEnumerable support for streaming (when targeting .NET Core 3.0+)

## Links

- [README](README.md) - Full documentation
- [Architecture](ARCHITECTURE.md) - Design patterns and architecture
- [Migration Guide](MIGRATION_GUIDE.md) - Migration from legacy API
- [Quick Reference](QUICK_REFERENCE.md) - Quick reference card
- [Examples](examples/) - Code examples

