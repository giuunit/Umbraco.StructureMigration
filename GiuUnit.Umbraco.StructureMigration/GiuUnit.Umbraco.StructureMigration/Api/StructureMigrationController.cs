using Umbraco.Web.WebApi;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using System.Web.Http;
using System.Net.Http;
using GiuUnit.Umbraco.StructureMigration.ViewModels;
using System;

namespace GiuUnit.Umbraco.StructureMigration.Api
{
    public class StructureMigrationController : UmbracoAuthorizedApiController
    {
        [HttpGet]
        public object RenderViewData()
        {
            //possible migration processes allowed
            var processList = MigrationProcessRepository.GetRepository();

            if (!processList.Any())
                return null;

            var contentTypes = Services.ContentTypeService.GetAllContentTypes();
            var memberTypes = Services.MemberTypeService.GetAll();

            var contentTypeBaseList = new List<IContentTypeBase>();
            contentTypeBaseList.AddRange(contentTypes);
            contentTypeBaseList.AddRange(memberTypes);

            //no content type, nothing to migrate
            if (!contentTypes.Any())
                return null;

            //only select contentTypes with propertyType we can process and only select those propertyTypes
            var cpropertyTypeListViewModel = MigrationProcessRepository.Filter(contentTypeBaseList);

            if (!cpropertyTypeListViewModel.Any())
                return null;

            return new
            {
                migrations= processList,
                propertyTypes= cpropertyTypeListViewModel
            };
        }

        [HttpPost]
        public object AddTransitionProperty(MigrationFormResults obj)
        {
            //rules for aliases: must start with a letter, must be lowercase, no dash, no special char
            var transitionAlias = "a" + Guid.NewGuid().ToString().Replace("-", string.Empty).ToLowerInvariant();
            var transitionName = "a" + Guid.NewGuid().ToString().Replace("-", string.Empty).ToLowerInvariant();

            var sMigrationService = new ContentBaseMigrationService(Services, obj.SelectedProperty.IsMemberType);
            
            var contentTypeBase = sMigrationService.GetContentBaseType(obj.SelectedProperty.ContentTypeAlias);
            var property = contentTypeBase.PropertyTypes.FirstOrDefault(x => x.Alias == obj.SelectedProperty.Alias);

            var propertyGroup = contentTypeBase.PropertyGroups.FirstOrDefault(x => x.PropertyTypes.Select(y => y.Alias).Contains(property.Alias))?.Name;

            if (property == null)
                throw new Exception("the property is not in the system");

            if (property.PropertyEditorAlias != obj.SelectedProperty.PropertyEditorAlias)
                throw new Exception("the property editor alias does not match");

            if (property.PropertyEditorAlias != obj.SelectedMigration.Origin)
                throw new Exception("the property editor alias does not match");

            //if we reach here, we good
            var transitionPropType = new PropertyType(new DataTypeDefinition(obj.SelectedMigration.Destination))
            {
                Alias = transitionAlias,
                Name = transitionName,
                SortOrder = property.SortOrder
            };

            if (string.IsNullOrWhiteSpace(propertyGroup))
                contentTypeBase.AddPropertyType(transitionPropType);
                
            else
                contentTypeBase.AddPropertyType(transitionPropType, propertyGroup);

            //accessibility properties - for members only
            sMigrationService.SetAccessibilityProperties(contentTypeBase, transitionAlias, property.Alias);

            sMigrationService.Save(contentTypeBase);

            return new
            {
                transitionAlias=transitionAlias,
                transitionName=transitionName
            };
        }

        [HttpPost]
        public HttpResponseMessage MigrateData(MigrationFormResults obj)
        {
            var sMigrationService = new ContentBaseMigrationService(Services, obj.SelectedProperty.IsMemberType);

            var contentBaseList = sMigrationService.GetContentBaseList(obj.SelectedProperty.ContentTypeAlias);

            foreach(var contentBase in contentBaseList)
            {
                var oldValue = contentBase.GetValue(obj.SelectedProperty.Alias);
                contentBase.SetValue(obj.TransitionProperty.TransitionAlias, oldValue);
            }

            sMigrationService.SaveContentBaseList(contentBaseList);

            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        }

        [HttpPost]
        public HttpResponseMessage RenameDelete(MigrationFormResults obj)
        {
            var sMigrationService = new ContentBaseMigrationService(Services, obj.SelectedProperty.IsMemberType);

            var contentTypeBase = sMigrationService.GetContentBaseType(obj.SelectedProperty.ContentTypeAlias);

            var oldProperty = contentTypeBase.PropertyTypes.FirstOrDefault(x => x.Alias == obj.SelectedProperty.Alias);
            var newProperty = contentTypeBase.PropertyTypes.FirstOrDefault(x => x.Alias == obj.TransitionProperty.TransitionAlias);

            contentTypeBase.RemovePropertyType(obj.SelectedProperty.Alias);
            newProperty.Alias = obj.SelectedProperty.Alias;
            newProperty.Name = obj.SelectedProperty.Name;

            sMigrationService.Save(contentTypeBase);

            return new HttpResponseMessage(System.Net.HttpStatusCode.OK);
        }
    }
}
