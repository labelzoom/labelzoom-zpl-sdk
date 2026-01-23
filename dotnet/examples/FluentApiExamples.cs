using System;
using System.Threading.Tasks;
using LabelzoomDotnetSdk;

namespace LabelzoomDotnetSdk.Examples
{
    /// <summary>
    /// Examples demonstrating the new fluent API and builder pattern.
    /// </summary>
    public class FluentApiExamples
    {
        /// <summary>
        /// Example 1: Using the LabelzoomClientBuilder
        /// </summary>
        public static async Task BuilderPatternExample()
        {
            // Build a client with custom configuration
            using (var client = new LabelzoomClientBuilder()
                .WithToken("YOUR_AUTH_TOKEN")
                .WithEndpoint("https://api.labelzoom.net")
                .WithTimeout(TimeSpan.FromSeconds(30))
                .Build())
            {
                var zpl = await client.Convert()
                    .FromPdf("path/to/document.pdf")
                    .ToZpl()
                    .ExecuteAsync();

                Console.WriteLine(zpl);
            }
        }

        /// <summary>
        /// Example 2: Simple PDF to ZPL conversion using fluent API
        /// </summary>
        public static async Task SimplePdfToZplExample()
        {
            using (var client = new LabelzoomClient("YOUR_AUTH_TOKEN"))
            {
                // New fluent API
                var zpl = await client.Convert()
                    .FromPdf("path/to/document.pdf")
                    .ToZpl()
                    .ExecuteAsync();

                Console.WriteLine(zpl);
            }
        }

        /// <summary>
        /// Example 3: Streaming PDF to ZPL for large documents
        /// </summary>
        public static async Task StreamingPdfToZplExample()
        {
            using (var client = new LabelzoomClient("YOUR_AUTH_TOKEN"))
            {
                int labelCount = 0;

                await client.Convert()
                    .FromPdf("path/to/large-document.pdf")
                    .ToZpl()
                    .StreamAsync(async (zpl) =>
                    {
                        labelCount++;
                        Console.WriteLine($"Received label {labelCount}");
                        // Process each label as it arrives
                        // Send to printer, save to file, etc.
                    });

                Console.WriteLine($"Total labels processed: {labelCount}");
            }
        }

        /// <summary>
        /// Example 4: PNG to ZPL conversion
        /// </summary>
        public static async Task PngToZplExample()
        {
            using (var client = new LabelzoomClient("YOUR_AUTH_TOKEN"))
            {
                var zpl = await client.Convert()
                    .FromPng("path/to/image.png")
                    .ToZpl()
                    .ExecuteAsync();

                Console.WriteLine(zpl);
            }
        }

        /// <summary>
        /// Example 5: Using a custom HttpClient (for dependency injection scenarios)
        /// </summary>
        public static async Task CustomHttpClientExample()
        {
            var httpClient = new System.Net.Http.HttpClient();
            // Configure your HttpClient as needed (e.g., with HttpClientFactory)

            using (var client = new LabelzoomClientBuilder()
                .WithToken("YOUR_AUTH_TOKEN")
                .WithHttpClient(httpClient)
                .WithDisposeHttpClient(false) // We'll manage the HttpClient lifecycle ourselves
                .Build())
            {
                var zpl = await client.Convert()
                    .FromPdf("document.pdf")
                    .ToZpl()
                    .ExecuteAsync();

                Console.WriteLine(zpl);
            }

            // Clean up our HttpClient
            httpClient.Dispose();
        }

        /// <summary>
        /// Example 6: Backward compatibility - old API still works
        /// </summary>
        public static async Task BackwardCompatibilityExample()
        {
            using (var client = new LabelzoomClient("YOUR_AUTH_TOKEN"))
            {
                // Old API - still works!
                var zpl = await client.PdfToZpl("path/to/document.pdf");
                Console.WriteLine(zpl);

                // Old streaming API - still works!
                await client.PdfToZplAsync("path/to/document.pdf", async (label) =>
                {
                    Console.WriteLine($"Label: {label.Substring(0, 50)}...");
                });
            }
        }

        /// <summary>
        /// Example 7: Error handling with the fluent API
        /// </summary>
        public static async Task ErrorHandlingExample()
        {
            using (var client = new LabelzoomClient("YOUR_AUTH_TOKEN"))
            {
                try
                {
                    var zpl = await client.Convert()
                        .FromPdf("path/to/document.pdf")
                        .ToZpl()
                        .ExecuteAsync();

                    Console.WriteLine("Conversion successful!");
                }
                catch (System.IO.FileNotFoundException ex)
                {
                    Console.WriteLine($"File not found: {ex.Message}");
                }
                catch (System.Net.Http.HttpRequestException ex)
                {
                    Console.WriteLine($"API request failed: {ex.Message}");
                }
            }
        }
    }
}

