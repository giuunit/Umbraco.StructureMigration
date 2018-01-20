using System;
using Umbraco.Core;

namespace GiuUnit.Umbraco.StructureMigration
{
    public class TextstringToLabelMigration : MigrationProcessBase, IMigrationProcess
    {
        public override string Destination => Constants.PropertyEditors.NoEditAlias;

        public override string Origin => Constants.PropertyEditors.TextboxAlias;
    }
}
