using System.ComponentModel;
using System.Runtime.InteropServices;

namespace PowerSuggestion
{
    internal partial class OptionsProvider
    {
        // Register the options with this attribute on your package class:
        // [ProvideOptionPage(typeof(OptionsProvider.ConnectionParametersOptions), "PowerSuggestion", "ConnectionParameters", 0, 0, true, SupportsProfiles = true)]
        [ComVisible(true)]
        public class ConnectionParametersOptions : BaseOptionPage<ConnectionParameters> { }
    }

    public class ConnectionParameters : BaseOptionModel<ConnectionParameters>
    {
       
        [Category("Connection")]
        [DisplayName("CRM Enviroment Url")]
        [Description("CRM URL.")]
        [DefaultValue(true)]
        public string EnviromentUrl { get; set; } = "";

        [Category("Connection")]
        [DisplayName("CRM Username")]
        [Description("Username of CRM User to connect to.")]
        [DefaultValue(true)]
        public string Username { get; set; } = "";

        [Category("Connection")]
        [DisplayName("CRM Password")]
        [Description("Password of CRM User to connect to.")]
        [DefaultValue(true)]
        [PasswordPropertyText(true)]
        public string Password { get; set; } = "";
    }
}
