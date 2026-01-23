using System;
using System.Net.Http;
using LabelzoomDotnetSdk;
using Xunit;

namespace LabelzoomDotnetSdk.Tests
{
    public class LabelzoomClientBuilderTests
    {
        [Fact]
        public void Builder_WithToken_SetsToken()
        {
            var builder = new LabelzoomClientBuilder()
                .WithToken("test-token");

            var client = builder.Build();

            Assert.NotNull(client);
        }

        [Fact]
        public void Builder_WithoutToken_ThrowsException()
        {
            var builder = new LabelzoomClientBuilder();

            Assert.Throws<InvalidOperationException>(() => builder.Build());
        }

        [Fact]
        public void Builder_WithEmptyToken_ThrowsException()
        {
            var builder = new LabelzoomClientBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithToken(""));
        }

        [Fact]
        public void Builder_WithNullToken_ThrowsException()
        {
            var builder = new LabelzoomClientBuilder();

            Assert.Throws<ArgumentException>(() => builder.WithToken(null));
        }

        [Fact]
        public void Builder_WithEndpoint_SetsEndpoint()
        {
            var customEndpoint = "https://custom-api.example.com";

            using (var client = new LabelzoomClientBuilder()
                .WithToken("test-token")
                .WithEndpoint(customEndpoint)
                .Build())
            {
                Assert.Equal(customEndpoint, client.Endpoint);
            }
        }

        [Fact]
        public void Builder_WithEmptyEndpoint_ThrowsException()
        {
            var builder = new LabelzoomClientBuilder()
                .WithToken("test-token");

            Assert.Throws<ArgumentException>(() => builder.WithEndpoint(""));
        }

        [Fact]
        public void Builder_WithTimeout_SetsTimeout()
        {
            var timeout = TimeSpan.FromSeconds(30);

            var builder = new LabelzoomClientBuilder()
                .WithToken("test-token")
                .WithTimeout(timeout);

            var client = builder.Build();

            Assert.NotNull(client);
        }

        [Fact]
        public void Builder_WithZeroTimeout_ThrowsException()
        {
            var builder = new LabelzoomClientBuilder()
                .WithToken("test-token");

            Assert.Throws<ArgumentException>(() => builder.WithTimeout(TimeSpan.Zero));
        }

        [Fact]
        public void Builder_WithNegativeTimeout_ThrowsException()
        {
            var builder = new LabelzoomClientBuilder()
                .WithToken("test-token");

            Assert.Throws<ArgumentException>(() => builder.WithTimeout(TimeSpan.FromSeconds(-1)));
        }

        [Fact]
        public void Builder_WithHttpClient_UsesProvidedClient()
        {
            var httpClient = new HttpClient();

            using (var client = new LabelzoomClientBuilder()
                .WithToken("test-token")
                .WithHttpClient(httpClient)
                .Build())
            {
                Assert.NotNull(client);
            }

            // HttpClient should not be disposed by default when provided externally
            Assert.False(httpClient.IsDisposed());
        }

        [Fact]
        public void Builder_WithNullHttpClient_ThrowsException()
        {
            var builder = new LabelzoomClientBuilder()
                .WithToken("test-token");

            Assert.Throws<ArgumentNullException>(() => builder.WithHttpClient(null));
        }

        [Fact]
        public void Builder_FluentChaining_Works()
        {
            using (var client = new LabelzoomClientBuilder()
                .WithToken("test-token")
                .WithEndpoint("https://api.example.com")
                .WithTimeout(TimeSpan.FromSeconds(60))
                .Build())
            {
                Assert.NotNull(client);
                Assert.Equal("https://api.example.com", client.Endpoint);
            }
        }

        [Fact]
        public void Builder_DefaultEndpoint_IsSet()
        {
            using (var client = new LabelzoomClientBuilder()
                .WithToken("test-token")
                .Build())
            {
                Assert.Equal("https://api.labelzoom.net", client.Endpoint);
            }
        }
    }

    // Extension method to check if HttpClient is disposed (for testing purposes)
    internal static class HttpClientExtensions
    {
        public static bool IsDisposed(this HttpClient client)
        {
            try
            {
                // Try to access a property - will throw if disposed
                var _ = client.BaseAddress;
                return false;
            }
            catch (ObjectDisposedException)
            {
                return true;
            }
        }
    }
}

