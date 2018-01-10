namespace Petroineos.PowerReport.Interfaces
{
    public interface IRunSettings
    {
        string Filename { get; }
        string Path { get; }
        int IntervalInMinutes { get; }

        void Refresh();
    }
}
