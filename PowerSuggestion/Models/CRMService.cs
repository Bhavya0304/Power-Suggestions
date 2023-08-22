using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerSuggestion.Models
{
    public class CRMService
    {
        public static IOrganizationService Service { get; set; } = null;

        public void ConnectToCRM(string Username,string Password,string Url)
        {
            try
            {
                string ConnectionString = $@" Url ={Url};AuthType=OAuth;UserName ={Username}; Password={Password};AppId=51f81489-12ee-4a9e-aaae-a2591f45987d;RedirectUri=app://58145B91-0C36-4500-8554-080854F2AC97;LoginPrompt=Auto;RequireNewInstance=True;";
                CrmServiceClient conn = new CrmServiceClient(ConnectionString);
                if (conn.IsReady)
                {
                    CRMService.Service = (IOrganizationService)conn;
                    VS.StatusBar.ShowMessageAsync("Connected To CRM");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
