using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using System.Linq;

namespace GiuUnit.Umbraco.StructureMigration
{
    public class ContentBaseMigrationService : IStructureMigrationService
    {
        private ServiceContext _serviceContext;
        private bool _isMemberType;

        internal ContentBaseMigrationService(ServiceContext serviceContext, bool isMemberType)
        {
            _serviceContext = serviceContext;
            _isMemberType = isMemberType;
        }

        public IEnumerable<IContentBase> GetContentBaseList(string contentTypeAlias)
        {
            if (_isMemberType)
                return _serviceContext.MemberService.GetMembersByMemberType(contentTypeAlias);

            else
            {
                //looks like it's not possible from content service to find content by content type alias, weird
                var contentType = _serviceContext.ContentTypeService.GetContentType(contentTypeAlias);
                return _serviceContext.ContentService.GetContentOfContentType(contentType.Id);
            }
        }

        public IContentTypeBase GetContentBaseType(string contentTypeAlias)
        {
            if (_isMemberType)
                return _serviceContext.MemberTypeService.Get(contentTypeAlias);

            else
                return _serviceContext.ContentTypeService.GetContentType(contentTypeAlias);
        }

        public void Save(IContentTypeBase contentTypeBase)
        {
            if(_isMemberType)
            {
                var sTyped = contentTypeBase as IMemberType;
                if (sTyped == null)
                    throw new System.Exception("wrong type");

                _serviceContext.MemberTypeService.Save(sTyped);
            }

            else
            {
                var sTyped = contentTypeBase as IContentType;
                if (sTyped == null)
                    throw new System.Exception("wrong type");

                _serviceContext.ContentTypeService.Save(sTyped);
            }
        }

        public void SaveContentBaseList(IEnumerable<IContentBase> list)
        {
            if (_isMemberType)
            {
                var sTypedList = list.Select(x => x as IMember);
                if (sTypedList.Any(x => x == null))
                    throw new System.Exception("wrong type");

                _serviceContext.MemberService.Save(sTypedList);
            }

            else
            {
                var sTypedList = list.Select(x => x as IContent);
                if (sTypedList.Any(x => x == null))
                    throw new System.Exception("wrong type");

                _serviceContext.ContentService.Save(sTypedList);
            }
        }

        public IContentTypeBase SetAccessibilityProperties(IContentTypeBase contentTypeBase, string transitionAlias, string oldPropAlias)
        {
            if (_isMemberType)
            {
                var memberType = contentTypeBase as IMemberType;

                if (memberType == null)
                    throw new System.Exception("wrong type");

                memberType.SetMemberCanEditProperty(transitionAlias, memberType.MemberCanEditProperty(oldPropAlias));
                memberType.SetMemberCanViewProperty(transitionAlias, memberType.MemberCanViewProperty(oldPropAlias));

                return memberType;
            }

            //for content there is no need to set accessibility properties
            else
            {
                if (!(contentTypeBase is IContentType))
                    throw new System.Exception("wrong type");

                return contentTypeBase;
            }
        }
    }
}
