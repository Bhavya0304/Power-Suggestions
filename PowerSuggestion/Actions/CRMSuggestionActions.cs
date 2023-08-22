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
            catch (Exception)
            {

                return null;
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
            catch (Exception)
            {

                return null;
            }
        }
    }
}
