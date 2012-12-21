using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Configuration;
using System.Reflection;

using EsriUK.NETPortalAPI.REST;

namespace EsriUK.NETPortalAPI
{
    /*
     * This object stores Portal connection details, including authentication
     * information (i.e. username/password and token)
     * 
     * A PortalConnection object is typically required to construct any object in
     * EsriUK.NETPortalAPI.REST 
     */ 
    public class PortalConnection
    {
        public string password { get; private set; }
        public string username { get; private set; }
        public string portalDomainName { get; private set; }
        public string clientReferer { get; private set; }
        public bool alwaysUseSSL { get; private set; }
        public string token = null;

        // Construct a Portal Connection object using details from App.config
        public PortalConnection()
        {
            String assemblyLocation = Assembly.GetEntryAssembly().Location;
            Configuration appConfig = ConfigurationManager.OpenExeConfiguration(assemblyLocation);
            password = appConfig.AppSettings.Settings["PortalPassword"].Value;
            username = appConfig.AppSettings.Settings["PortalUser"].Value;
            portalDomainName = appConfig.AppSettings.Settings["PortalDomainName"].Value;
            clientReferer = appConfig.AppSettings.Settings["ClientReferer"].Value;
            alwaysUseSSL = Convert.ToBoolean(appConfig.AppSettings.Settings["AlwaysUseSSL"].Value);

            // Generate a token when this object is constructed
            // TODO: auto-refresh this token on timeout
            token = GenerateToken();
        }

        // TODO: Create a constructor that allows a user to create a Portal Connection
        // without using the App.config file
        /*
        public PortalConnection(string username, string password, string clientReferer)
        {
            this.username = username;
            this.password = password;
            this.clientReferer = clientReferer;
        }*/

        // Generate a token
        // TODO: Error-handling
        private string GenerateToken()
        {
            GenerateToken generateToken = new GenerateToken(this);
            generateToken.makeRequest();
            return generateToken.response.token;
        }

    }
}
