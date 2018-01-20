namespace GiuUnit.Umbraco.StructureMigration
{
    public interface IMigrationProcess
    {
        string Destination { get; }
        string Origin { get; }
    }
}
