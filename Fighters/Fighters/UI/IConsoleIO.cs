namespace Fighters.UI
{
    public interface IConsoleIO
    {
        public void WriteLine( string message );
        public string? ReadLine();
    }
}
