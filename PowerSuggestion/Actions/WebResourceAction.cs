using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using PowerSuggestion.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using static PowerSuggestion.Helpers.Enums;

namespace PowerSuggestion.Actions
{
    public class WebResourceAction
    {

        public Guid UploadWebResources(string resourcesPath, Guid? WebRresourceId = null, string Description = null, string Name = null, string DisplayName = null, string SoltuionName = null)
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
                wr["name"] = Name ?? webResourceName;
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
            catch (Exception)
            {

                return Guid.Empty;
            }
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

        public List<WebResourceMetedata> getAllWebResourceFile(string FileType, Guid? webResourceId = null)
        {
            List<WebResourceMetedata> WRdata = new List<WebResourceMetedata>();

            return WRdata;
        }

    }
}
