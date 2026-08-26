namespace Fighters.UI
{
    public class ConsoleIO : IConsoleIO
    {
        public void WriteLine( string message ) => Console.WriteLine( message );
        public string? ReadLine() => Console.ReadLine();
    }
}
