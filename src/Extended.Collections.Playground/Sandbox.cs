using Serilog;

namespace Extended.Collections.Playground
{
    public abstract class Sandbox
    {
        public ILogger Logger { get; }

        protected Sandbox()
        {
            Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
        }

        protected abstract void Run();
    }
}
