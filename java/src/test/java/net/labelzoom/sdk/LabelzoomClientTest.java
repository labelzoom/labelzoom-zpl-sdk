package net.labelzoom.sdk;

import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.io.TempDir;

import java.io.IOException;
import java.nio.file.Files;
import java.nio.file.Path;
import java.util.concurrent.atomic.AtomicInteger;

import static org.junit.jupiter.api.Assertions.*;

/**
 * Unit tests for LabelzoomClient.
 */
class LabelzoomClientTest {
    
    @Test
    void testConstructorWithNullToken() {
        assertThrows(IllegalArgumentException.class, () -> new LabelzoomClient(null));
    }
    
    @Test
    void testConstructorWithEmptyToken() {
        assertThrows(IllegalArgumentException.class, () -> new LabelzoomClient(""));
    }
    
    @Test
    void testConstructorWithValidToken() {
        assertDoesNotThrow(() -> {
            try (LabelzoomClient client = new LabelzoomClient("test-token")) {
                assertNotNull(client);
            }
        });
    }
    
    @Test
    void testGetEndpoint() {
        try (LabelzoomClient client = new LabelzoomClient("test-token")) {
            assertEquals("https://api.labelzoom.net", client.getEndpoint());
        }
    }
    
    @Test
    void testSetEndpoint() {
        try (LabelzoomClient client = new LabelzoomClient("test-token")) {
            client.setEndpoint("https://custom-api.example.com");
            assertEquals("https://custom-api.example.com", client.getEndpoint());
        }
    }
    
    @Test
    void testPdfToZplAsyncWithNullCallback(@TempDir Path tempDir) throws IOException {
        Path testPdf = tempDir.resolve("test.pdf");
        Files.write(testPdf, new byte[]{0x25, 0x50, 0x44, 0x46}); // PDF header
        
        try (LabelzoomClient client = new LabelzoomClient("test-token")) {
            assertThrows(IllegalArgumentException.class, 
                () -> client.pdfToZplAsync(testPdf.toString(), null));
        }
    }
    
    // Note: The following tests require a valid API token and test PDF file
    // Uncomment and configure when running integration tests
    
    /*
    @Test
    void testConvertPdfToZplReturnsValidZpl() throws Exception {
        Path pdfPath = Path.of("src/test/resources/TestData/4x6_document.pdf");
        assertTrue(Files.exists(pdfPath), "Missing test PDF: " + pdfPath);
        
        try (LabelzoomClient client = new LabelzoomClient("")) {
            String zpl = client.pdfToZpl(pdfPath.toString());
            assertNotNull(zpl);
            assertTrue(zpl.contains("^XA"));
            assertTrue(zpl.contains("^XZ"));
        }
    }
    
    @Test
    void testConvertPdfToZplAsyncReturnsValidZpl() throws Exception {
        Path pdfPath = Path.of("src/test/resources/TestData/4x6_document.pdf");
        assertTrue(Files.exists(pdfPath), "Missing test PDF: " + pdfPath);
        
        try (LabelzoomClient client = new LabelzoomClient("")) {
            AtomicInteger labelCount = new AtomicInteger(0);
            StringBuilder zplBuffer = new StringBuilder();
            
            client.pdfToZplAsync(pdfPath.toString(), zpl -> {
                labelCount.incrementAndGet();
                zplBuffer.append(zpl);
            });
            
            String result = zplBuffer.toString();
            assertNotNull(result);
            assertTrue(result.contains("^XA"));
            assertTrue(result.contains("^XZ"));
            assertEquals(12, labelCount.get());
        }
    }
    */
}

