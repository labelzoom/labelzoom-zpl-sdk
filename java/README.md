![LabelZoom Logo](../docs/LabelZoom_Logo_f_400px.png)

# LabelZoom Java SDK

A full-featured Java SDK for converting documents and images to ZPL format using the LabelZoom API.

## Features

- **PDF to ZPL Conversion**: Convert PDF documents to ZPL format
- **Synchronous and Asynchronous APIs**: Choose the best approach for your use case
- **Streaming Support**: Efficiently handle large multi-page documents with streaming callbacks
- **Java 11+**: Compatible with Java 11 and later versions
- **Zero External Dependencies**: Uses Java's built-in HttpClient (Java 11+)
- **AutoCloseable Support**: Proper resource management with try-with-resources
- **Configurable Endpoint**: Override the default API endpoint if needed

## Requirements

- Java 11 or later
- Maven 3.6+ (for building from source)

## Installation

### From Source

```bash
git clone https://github.com/labelzoom/labelzoom-zpl-sdk.git
cd labelzoom-zpl-sdk/java
mvn clean install
```

### Maven Dependency

*(Coming soon)*

```xml
<dependency>
    <groupId>net.labelzoom</groupId>
    <artifactId>labelzoom-java-sdk</artifactId>
    <version>1.0.0</version>
</dependency>
```

### Gradle Dependency

*(Coming soon)*

```gradle
implementation 'net.labelzoom:labelzoom-java-sdk:1.0.0'
```

## Quick Start

### Basic PDF to ZPL Conversion

```java
import net.labelzoom.sdk.LabelzoomClient;

try (LabelzoomClient client = new LabelzoomClient("YOUR_AUTH_TOKEN")) {
    String zpl = client.pdfToZpl("path/to/document.pdf");
    System.out.println(zpl);
} catch (Exception e) {
    e.printStackTrace();
}
```

### Streaming PDF to ZPL (for large documents)

```java
import net.labelzoom.sdk.LabelzoomClient;

try (LabelzoomClient client = new LabelzoomClient("YOUR_AUTH_TOKEN")) {
    client.pdfToZplAsync("path/to/document.pdf", zpl -> {
        // Process each label as it's received
        System.out.println("Received label: " + zpl.substring(0, Math.min(50, zpl.length())) + "...");
        // Send to printer, save to file, etc.
    });
} catch (Exception e) {
    e.printStackTrace();
}
```

### Custom Endpoint

```java
import net.labelzoom.sdk.LabelzoomClient;

try (LabelzoomClient client = new LabelzoomClient("YOUR_AUTH_TOKEN")) {
    client.setEndpoint("https://custom-api.example.com");
    String zpl = client.pdfToZpl("path/to/document.pdf");
    System.out.println(zpl);
} catch (Exception e) {
    e.printStackTrace();
}
```

## API Reference

### LabelzoomClient

#### Constructor

```java
public LabelzoomClient(String token)
```

Creates a new instance of the LabelZoom client with the specified authentication token.

**Parameters:**
- `token` - The API authentication token

**Throws:**
- `IllegalArgumentException` - if token is null or empty

#### Properties

- `String getEndpoint()` - Gets the API endpoint URL (default: `https://api.labelzoom.net`)
- `void setEndpoint(String endpoint)` - Sets the API endpoint URL

#### Methods

##### pdfToZpl

```java
public String pdfToZpl(String pdfPath) throws LabelzoomException, IOException
```

Converts a PDF document to a single ZPL string. Best used for smaller documents with fewer pages.

**Parameters:**
- `pdfPath` - Path to the PDF file

**Returns:** ZPL string containing all labels

**Throws:**
- `LabelzoomException` - if the conversion fails
- `IOException` - if there's an error reading the file

##### pdfToZplAsync

```java
public void pdfToZplAsync(String pdfPath, Consumer<String> onLabel) 
    throws LabelzoomException, IOException
```

Converts a PDF document to ZPL and streams the response one label at a time. Best used for larger documents with many pages.

**Parameters:**
- `pdfPath` - Path to the PDF file
- `onLabel` - Callback function invoked for each label

**Throws:**
- `LabelzoomException` - if the conversion fails
- `IOException` - if there's an error reading the file
- `IllegalArgumentException` - if onLabel is null

## Building

```bash
cd java
mvn clean package
```

## Running Tests

```bash
cd java
mvn test
```

## Authentication

You need a LabelZoom API token to use this SDK. Contact [LabelZoom](https://www.labelzoom.net) to obtain an API token.

## Code Snippets

Additional code snippets and examples can be found in the `snippets/` directory.

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For issues, questions, or contributions, please visit the [GitHub repository](https://github.com/labelzoom/labelzoom-zpl-sdk).

