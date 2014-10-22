using System.Diagnostics;

namespace CodeChurnLoader.Tests
{
    public class TestLogger : ILogger
    {
        public void Log(string message)
        {
            Trace.WriteLine(message);
        }
    }
}
