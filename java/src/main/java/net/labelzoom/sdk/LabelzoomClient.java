package net.labelzoom.sdk;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.nio.charset.StandardCharsets;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.function.Consumer;

/**
 * LabelZoom API client for converting PDF documents to ZPL format.
 * <p>
 * This client provides both synchronous and asynchronous (streaming) methods
 * for converting PDF documents to ZPL format using the LabelZoom API.
 * </p>
 * <p>
 * Example usage:
 * <pre>{@code
 * try (LabelzoomClient client = new LabelzoomClient("YOUR_AUTH_TOKEN")) {
 *     String zpl = client.pdfToZpl("path/to/document.pdf");
 *     System.out.println(zpl);
 * }
 * }</pre>
 * </p>
 */
public class LabelzoomClient implements AutoCloseable {
    
    private final HttpClient httpClient;
    private final String token;
    private String endpoint = "https://api.labelzoom.net";
    
    /**
     * Creates a new LabelZoom client with the specified authentication token.
     *
     * @param token the API authentication token
     * @throws IllegalArgumentException if token is null or empty
     */
    public LabelzoomClient(String token) {
        if (token == null || token.trim().isEmpty()) {
            throw new IllegalArgumentException("Token cannot be null or empty");
        }
        this.token = token;
        this.httpClient = HttpClient.newBuilder()
                .version(HttpClient.Version.HTTP_1_1)
                .build();
    }
    
    /**
     * Gets the API endpoint URL.
     *
     * @return the endpoint URL
     */
    public String getEndpoint() {
        return endpoint;
    }
    
    /**
     * Sets the API endpoint URL.
     *
     * @param endpoint the endpoint URL to use
     */
    public void setEndpoint(String endpoint) {
        this.endpoint = endpoint;
    }
    
    /**
     * Converts a PDF document to a single ZPL string.
     * <p>
     * Best used for smaller documents with fewer pages.
     * </p>
     *
     * @param pdfPath path to the PDF file
     * @return ZPL string containing all labels
     * @throws LabelzoomException if the conversion fails
     * @throws IOException if there's an error reading the file
     */
    public String pdfToZpl(String pdfPath) throws LabelzoomException, IOException {
        Path path = Paths.get(pdfPath);
        byte[] pdfBytes = Files.readAllBytes(path);
        
        HttpRequest request = HttpRequest.newBuilder()
                .uri(URI.create(endpoint + "/api/v2/convert/pdf/to/zpl"))
                .header("Authorization", "Bearer " + token)
                .header("Content-Type", "application/pdf")
                .POST(HttpRequest.BodyPublishers.ofByteArray(pdfBytes))
                .build();
        
        try {
            HttpResponse<String> response = httpClient.send(request, HttpResponse.BodyHandlers.ofString());
            
            if (response.statusCode() != 200) {
                throw new LabelzoomException(
                    "API request failed with status code " + response.statusCode() + ": " + response.body()
                );
            }
            
            return response.body();
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
            throw new LabelzoomException("Request was interrupted", e);
        }
    }
    
    /**
     * Converts a PDF document to ZPL and streams the response one label at a time.
     * <p>
     * Best used for larger documents with many pages. Each label returned will
     * invoke the callback function.
     * </p>
     *
     * @param pdfPath path to the PDF file
     * @param onLabel callback function invoked for each label
     * @throws LabelzoomException if the conversion fails
     * @throws IOException if there's an error reading the file
     * @throws IllegalArgumentException if onLabel is null
     */
    public void pdfToZplAsync(String pdfPath, Consumer<String> onLabel) 
            throws LabelzoomException, IOException {
        if (onLabel == null) {
            throw new IllegalArgumentException("onLabel callback cannot be null");
        }
        
        Path path = Paths.get(pdfPath);
        byte[] pdfBytes = Files.readAllBytes(path);
        
        HttpRequest request = HttpRequest.newBuilder()
                .uri(URI.create(endpoint + "/api/v2.5/convert/pdf/to/zpl"))
                .header("Authorization", "Bearer " + token)
                .header("Content-Type", "application/pdf")
                .POST(HttpRequest.BodyPublishers.ofByteArray(pdfBytes))
                .build();
        
        try {
            HttpResponse<InputStream> response = httpClient.send(request, HttpResponse.BodyHandlers.ofInputStream());
            
            if (response.statusCode() != 200) {
                String errorBody = new String(response.body().readAllBytes(), StandardCharsets.UTF_8);
                throw new LabelzoomException(
                    "API request failed with status code " + response.statusCode() + ": " + errorBody
                );
            }
            
            try (BufferedReader reader = new BufferedReader(
                    new InputStreamReader(response.body(), StandardCharsets.US_ASCII))) {
                String line;
                while ((line = reader.readLine()) != null) {
                    if (!line.isEmpty()) {
                        onLabel.accept(line);
                    }
                }
            }
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
            throw new LabelzoomException("Request was interrupted", e);
        }
    }
    
    @Override
    public void close() {
        // HttpClient doesn't require explicit cleanup in Java 11+
        // This method is provided for consistency with AutoCloseable pattern
    }
}

