using System;
using System.Collections.Generic;
using Umbraco.Core.Models;

namespace GiuUnit.Umbraco.StructureMigration
{
    interface IStructureMigrationService
    {
        IContentTypeBase GetContentBaseType(string contentTypeAlias);
        void Save(IContentTypeBase contentTypeBase);
        IContentTypeBase SetAccessibilityProperties(IContentTypeBase contentTypeBase, string transitionAlias, string oldPropAlias);
        IEnumerable<IContentBase> GetContentBaseList(string contentTypeAlias);
        void SaveContentBaseList(IEnumerable<IContentBase> list);
    }
}
