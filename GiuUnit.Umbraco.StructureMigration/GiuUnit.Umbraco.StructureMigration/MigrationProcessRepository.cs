using System.Collections.Generic;
using Umbraco.Core.Models;
using System.Linq;
using GiuUnit.Umbraco.StructureMigration.ViewModels;

namespace GiuUnit.Umbraco.StructureMigration
{
    public static class MigrationProcessRepository
    {
        public static IEnumerable<IMigrationProcess> GetRepository()
        {
            return new List<IMigrationProcess>
            {
                new LabelToTextstringMigration(),
                new TextstringToLabelMigration()
            };
        }

        public static IEnumerable<PropertyTypeViewModel> Filter(IEnumerable<IContentTypeBase> list)
        {
            var repository = GetRepository();

            var distinctOrigins = repository.Select(x => x.Origin).Distinct();

            var res = list.SelectMany(x => x.PropertyTypes
            
            //we only select the properties with the proper origin prop type
            .Where(y => distinctOrigins.Contains(y.PropertyEditorAlias))
            //we don't display locked properties
            .Where(y => !Helpers.Constants.LockedPropertyAliasList.ToList().Contains(y.Alias))

            //we add the parent content type alias
            , (x, y) => new PropertyTypeViewModel()
            {
                Alias = y.Alias,
                Name = y.Name,
                ContentTypeAlias = x.Alias,
                PropertyEditorAlias = y.PropertyEditorAlias,
                IsMemberType = x is IMemberType
            });

            return res;

        }
    }
}
