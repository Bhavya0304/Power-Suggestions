using Microsoft.Crm.Sdk.Messages;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using PowerSuggestion.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Services.Description;
using static PowerSuggestion.Helpers.Enums;

namespace PowerSuggestion.Actions
{
    public class WebResourceAction
    {

        public Guid UploadWebResources(string resourcesPath, Guid? WebRresourceId = null, string Description = null, string Name = null, string DisplayName = null, string SolutionName = null, string Prefix = "_")
        {

            try
            {
                string ResourceName = System.IO.Path.GetFileName(resourcesPath);
                string webResourceName = resourcesPath + System.IO.Path.GetFileName(resourcesPath);
                var content = getEncodedFileContent(resourcesPath);
                Entity wr = new Entity("webresource");
                wr["content"] = content;
                wr["displayname"] = DisplayName ?? ResourceName;
                wr["description"] = Description ?? "Uploaded File";
                if (SolutionName == "Default")
                {
                    wr["name"] = "_" + RemoveSpecialChars(Name ?? ResourceName);

                }
                else
                {
                    wr["name"] = Prefix + RemoveSpecialChars(Name ?? ResourceName);

                }
                bool createWr = true;
                switch (System.IO.Path.GetExtension(ResourceName))
                {
                    case ".js":

                        wr["webresourcetype"] = new OptionSetValue((int)WEBRESOURCETYPE.JS);
                        break;
                    case ".gif":
                        wr["webresourcetype"] = new OptionSetValue((int)WEBRESOURCETYPE.GIF);
                        break;
                    case ".css":
                        wr["webresourcetype"] = new OptionSetValue((int)WEBRESOURCETYPE.CSS);
                        break;
                    case ".html":
                        wr["webresourcetype"] = new OptionSetValue((int)WEBRESOURCETYPE.HTML);
                        break;
                    default:
                        createWr = false;
                        break;
                }
                if (createWr)
                {
                    if (WebRresourceId != null)
                    {
                        wr.Id = new Guid(WebRresourceId.ToString());

                        CRMService.Service.Update(wr);
                        return wr.Id;
                    }
                    else
                    {
                        QueryByAttribute qba = new QueryByAttribute("webresource");
                        qba.ColumnSet = new ColumnSet(true);
                        qba.AddAttributeValue("name", webResourceName);
                        if (CRMService.Service.RetrieveMultiple(qba).Entities.FirstOrDefault() == null)
                        {
                            wr.Id = CRMService.Service.Create(wr);
                            if (SolutionName != "Default")
                            {
                                AddSolutionComponentRequest scRequest = new AddSolutionComponentRequest();
                                scRequest.ComponentType = (int)SolutionComponentType.WebResource;
                                scRequest.SolutionUniqueName = SolutionName;
                                scRequest.ComponentId = wr.Id;
                                var response = (AddSolutionComponentResponse)CRMService.Service.Execute(scRequest);
                            }
                            return wr.Id;
                        }
                        else
                        {
                            return Guid.Empty;
                        }
                    }

                }
                return Guid.Empty;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public string RemoveSpecialChars(string input)
        {
            return Regex.Replace(input, @"[^0-9a-zA-Z\._]", string.Empty);
        }


        public bool PublishFile(Guid webResourceId)
        {
            try
            {
                string webResctag = "<webresource>" + webResourceId + "</webresource>";

                string webrescXml = "<importexportxml><webresources>" + webResctag + "</webresources></importexportxml>";

                PublishXmlRequest publishxmlrequest = new PublishXmlRequest

                {

                    ParameterXml = String.Format(webrescXml)

                };
                var Response = CRMService.Service.Execute(publishxmlrequest);
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        static public string getEncodedFileContent(String pathToFile)
        {
            FileStream fs = new FileStream(pathToFile, FileMode.Open, FileAccess.Read);
            byte[] binaryData = new byte[fs.Length];
            long bytesRead = fs.Read(binaryData, 0, (int)fs.Length);
            fs.Close();
            return System.Convert.ToBase64String(binaryData, 0, binaryData.Length);
        }

        public List<SolutionMetadata> GetAllSolutions()
        {
            try
            {
                List<SolutionMetadata> Data = new List<SolutionMetadata>();
                QueryExpression SolutionExp = new QueryExpression("solution");
                SolutionExp.ColumnSet = new ColumnSet("uniquename", "publisherid");

                SolutionExp.LinkEntities.Add(new LinkEntity()
                {
                    LinkToEntityName = "publisher",
                    LinkFromAttributeName = "publisherid",
                    LinkToAttributeName = "publisherid",
                    Columns = new ColumnSet("customizationprefix"),
                    EntityAlias = "pub"
                });

                EntityCollection Solutions = CRMService.Service.RetrieveMultiple(SolutionExp);
                Data.AddRange(Solutions.Entities.ToList().ConvertAll(x => new SolutionMetadata
                {
                    UniqueName = x.GetAttributeValue<string>("uniquename"),
                    Prefix = x.GetAttributeValue<AliasedValue>("pub.customizationprefix").Value.ToString(),
                }));
                return Data;
            }
            catch (Exception)
            {
                return new List<SolutionMetadata>();
            }
        }
        public WebResourceMetedata GetWebResourceFromId(Guid Id)
        {
            try
            {
                Entity WR = CRMService.Service.Retrieve("webresource", Id, new ColumnSet("name", "displayname", "webresourceid", "description"));
                return new WebResourceMetedata
                {
                    Name = WR.Contains("wr.name") ? WR.GetAttributeValue<AliasedValue>("wr.name").Value.ToString() : "",
                    DisplayName = WR.Contains("wr.displayname") ? WR.GetAttributeValue<AliasedValue>("wr.displayname").Value.ToString() : "",
                    Id = WR.Contains("wr.webresourceid") ? (Guid)WR.GetAttributeValue<AliasedValue>("wr.webresourceid").Value : Guid.Empty,
                    Description = WR.Contains("wr.description") ? WR.GetAttributeValue<AliasedValue>("wr.description").Value.ToString() : ""
                };
            }
            catch (Exception)
            {

                return new WebResourceMetedata();
            }
        }

        public List<WebResourceMetedata> getAllWebResourceFile(string SolutionName = "Default")
        {
            List<WebResourceMetedata> WRdata = new List<WebResourceMetedata>();
            try
            {
                QueryExpression componentsQuery = new QueryExpression
                {
                    EntityName = "solutioncomponent",
                    ColumnSet = new ColumnSet("objectid"),
                    Criteria = new FilterExpression(),
                };
                LinkEntity solutionLink = new LinkEntity("solutioncomponent", "solution", "solutionid", "solutionid", JoinOperator.Inner);
                solutionLink.LinkCriteria = new FilterExpression();
                solutionLink.LinkCriteria.AddCondition(new ConditionExpression("uniquename", ConditionOperator.Equal, SolutionName));
                componentsQuery.LinkEntities.Add(solutionLink);
                componentsQuery.Criteria.AddCondition(new ConditionExpression("componenttype", ConditionOperator.Equal, (int)SolutionComponentType.WebResource));

                LinkEntity WebResource = new LinkEntity()
                {
                    LinkFromEntityName = "solutioncomponent",
                    LinkToEntityName = "webresource",
                    LinkFromAttributeName = "objectid",
                    LinkToAttributeName = "webresourceid",
                    Columns = new ColumnSet("name", "displayname", "webresourceid", "description"),
                    EntityAlias = "wr"
                };
                WebResource.LinkCriteria.AddCondition("webresourcetype", ConditionOperator.Equal, (int)WEBRESOURCETYPE.JS);
                componentsQuery.LinkEntities.Add(WebResource);
                EntityCollection ComponentsResult = CRMService.Service.RetrieveMultiple(componentsQuery);



                WRdata.AddRange(ComponentsResult.Entities.ToList().ConvertAll(x => new WebResourceMetedata
                {
                    Name = x.Contains("wr.name") ? x.GetAttributeValue<AliasedValue>("wr.name").Value.ToString() : "",
                    DisplayName = x.Contains("wr.displayname") ? x.GetAttributeValue<AliasedValue>("wr.displayname").Value.ToString() : "",
                    Id = x.Contains("wr.webresourceid") ? (Guid)x.GetAttributeValue<AliasedValue>("wr.webresourceid").Value : Guid.Empty,
                    Description = x.Contains("wr.description") ? x.GetAttributeValue<AliasedValue>("wr.description").Value.ToString() : ""
                }));
                return WRdata;
            }
            catch (Exception)
            {

                return WRdata;
            }
        }

    }
}
