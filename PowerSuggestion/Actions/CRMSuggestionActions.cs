using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using PowerSuggestion.Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Services.Description;

namespace PowerSuggestion.Actions
{
    public class CRMSuggestionActions
    {
        public List<PowerSuggestion.Models.EntityMetadata> GetAllEntities()
        {
            try
            {
                List<PowerSuggestion.Models.EntityMetadata> entityMetadata = new List<Models.EntityMetadata>();
                RetrieveAllEntitiesRequest retrieveAll = new RetrieveAllEntitiesRequest
                {
                    EntityFilters = EntityFilters.Entity
                };
                RetrieveAllEntitiesResponse retrieveAllEntityResponse = (RetrieveAllEntitiesResponse)CRMService.Service.Execute(retrieveAll);

                if (retrieveAllEntityResponse != null)
                {
                    entityMetadata.AddRange(retrieveAllEntityResponse.EntityMetadata.ToList().ConvertAll(x => new Models.EntityMetadata
                    {
                        DisplayName = x.DisplayName.UserLocalizedLabel == null ? x.LogicalName.ToString() : x.DisplayName.UserLocalizedLabel.Label.ToString(),
                        LogicalName = x.LogicalName.ToString()
                    }));
                }
                return entityMetadata;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Models.AttributeMetadata> GetEntityWithAttributes(string LogicalName)
        {
            try
            {
                List<Models.AttributeMetadata> attributes = new List<Models.AttributeMetadata>();
                RetrieveEntityRequest retrieveSingle = new RetrieveEntityRequest
                {
                    EntityFilters = EntityFilters.All,
                    LogicalName = LogicalName
                };
                RetrieveEntityResponse retrieveEntityResponse = (RetrieveEntityResponse)CRMService.Service.Execute(retrieveSingle);

                if (retrieveEntityResponse != null)
                {
                    attributes.AddRange(retrieveEntityResponse.EntityMetadata.Attributes.ToList().ConvertAll(x => new Models.AttributeMetadata
                    {
                        DisplayName = x.DisplayName.UserLocalizedLabel == null ? x.LogicalName.ToString() : x.DisplayName.UserLocalizedLabel.Label.ToString(),
                        LogicalName = x.LogicalName.ToString(),
                        SchemaName = x.SchemaName.ToString(),
                    }));

                }
                return attributes;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Models.AttributeMetadata GetAttributeMetadata(string EntityLogicalName, string AttributeLogicalName)
        {
            try
            {
                RetrieveAttributeRequest attributeRequest = new RetrieveAttributeRequest
                {
                    EntityLogicalName = EntityLogicalName,
                    LogicalName = AttributeLogicalName,
                    RetrieveAsIfPublished = false
                };
                RetrieveAttributeResponse response = CRMService.Service.Execute(attributeRequest) as RetrieveAttributeResponse;
                EnumAttributeMetadata attributeData = response.AttributeMetadata is EnumAttributeMetadata ? (EnumAttributeMetadata)response.AttributeMetadata : null;
                Models.AttributeMetadata Attribute = new Models.AttributeMetadata()
                {
                    DisplayName = response.AttributeMetadata.DisplayName.UserLocalizedLabel == null ? response.AttributeMetadata.LogicalName.ToString() : response.AttributeMetadata.DisplayName.UserLocalizedLabel.Label.ToString(),
                    LogicalName = response.AttributeMetadata.LogicalName,
                    AttributeType = response.AttributeMetadata.AttributeType.ToString(),
                    SchemaName = response.AttributeMetadata.SchemaName,
                    Options = attributeData != null && attributeData.OptionSet != null ? (from option in attributeData.OptionSet.Options
                                                       select new OptionSet()
                                                       {
                                                           OptionSetValue = option.Value.ToString(),
                                                           OptionSetName = option.Label.UserLocalizedLabel.Label

                                                       }).ToList() : null

                };

                return Attribute;
            }
            catch (Exception ex) { throw ex; }

        }

        public Models.EntityMetadata GetEntityMetadata(string EntityLogicalName)
        {
            try
            {
                RetrieveEntityRequest entityRequest = new RetrieveEntityRequest
                {
                    LogicalName = EntityLogicalName,
                    RetrieveAsIfPublished = false
                };
                RetrieveEntityResponse response = CRMService.Service.Execute(entityRequest) as RetrieveEntityResponse;

                Models.EntityMetadata Entity = new Models.EntityMetadata()
                {
                    DisplayName = response.EntityMetadata.DisplayName.UserLocalizedLabel == null ? response.EntityMetadata.LogicalName.ToString() : response.EntityMetadata.DisplayName.UserLocalizedLabel.Label.ToString(),
                    LogicalName = response.EntityMetadata.LogicalName,
                    SchemaName = response.EntityMetadata.SchemaName,
                    PluralName = response.EntityMetadata.LogicalCollectionName,
                };

                return Entity;
            }
            catch (Exception ex) { throw ex; }

        }
    }
}
