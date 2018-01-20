namespace GiuUnit.Umbraco.StructureMigration.ViewModels
{
    public class MigrationFormResults
    {
        public PropertyTypeViewModel SelectedProperty { get; set; }
        public MigrationProcessViewModel SelectedMigration { get; set; }
        //used for step 2
        public TransitionProperty TransitionProperty { get; set; }
    }
}
