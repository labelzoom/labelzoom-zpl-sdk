using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelzoomDotnetSdk.Tests
{
    public abstract class TestBase: IDisposable
    {
        private bool disposedValue;

        protected readonly LabelzoomClient _client;

        protected TestBase()
        {
            var token = Environment.GetEnvironmentVariable("LABELZOOM_API_TOKEN") ?? throw new ArgumentException("LABELZOOM_API_TOKEN environment variable was not defined");
            var endpoint = Environment.GetEnvironmentVariable("LABELZOOM_ENDPOINT") ?? throw new ArgumentException("LABELZOOM_ENDPOINT environment variable was not defined");

            _client = new LabelzoomClientBuilder()
                .WithToken(token)
                .WithEndpoint(endpoint)
                .Build();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _client.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
