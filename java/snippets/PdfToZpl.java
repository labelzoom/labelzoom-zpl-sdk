import java.io.IOException;
import java.net.URI;
import java.net.http.HttpClient;
import java.net.http.HttpRequest;
import java.net.http.HttpResponse;
import java.nio.file.Files;
import java.nio.file.Path;
import java.nio.file.Paths;

public class PdfToZpl {
    
    public static String pdfToZpl(String pdfPath) throws IOException, InterruptedException {
        Path path = Paths.get(pdfPath);
        byte[] pdfBytes = Files.readAllBytes(path);
        
        HttpClient client = HttpClient.newBuilder()
                .version(HttpClient.Version.HTTP_1_1)
                .build();
        
        HttpRequest request = HttpRequest.newBuilder()
                .uri(URI.create("https://api.labelzoom.net/api/v2.5/convert/pdf/to/zpl"))
                .header("Authorization", "Bearer <YOUR AUTH TOKEN HERE>")
                .header("Content-Type", "application/pdf")
                .POST(HttpRequest.BodyPublishers.ofByteArray(pdfBytes))
                .build();
        
        HttpResponse<String> response = client.send(request, HttpResponse.BodyHandlers.ofString());
        
        if (response.statusCode() != 200) {
            throw new RuntimeException("API returned status " + response.statusCode());
        }
        
        return response.body();
    }
}

