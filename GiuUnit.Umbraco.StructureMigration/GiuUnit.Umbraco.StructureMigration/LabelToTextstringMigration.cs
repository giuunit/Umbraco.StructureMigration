using System;
using Umbraco.Core;

namespace GiuUnit.Umbraco.StructureMigration
{
    public class LabelToTextstringMigration : MigrationProcessBase, IMigrationProcess
    {
        public override string Destination => Constants.PropertyEditors.TextboxAlias;

        public override string Origin => Constants.PropertyEditors.NoEditAlias;
    }
}
