﻿using System;
using System.Configuration;


namespace Vega.USiteBuilder.Configuration
{
    /// <summary>
    /// Holds uSiteBuilder configuration settings (read from web.config)
    /// </summary>
    static class USiteBuilderConfiguration
    {
        /// <summary>
        /// Get's username of umbraco user whose account is used with Umbraco API
        /// </summary>
        public static string ApiUser 
        {
            get
            {
                var retVal = "admin";

                var user = umbraco.helper.GetCurrentUmbracoUser();

                if (user != null && !string.IsNullOrWhiteSpace(user.LoginName))
                {
                    retVal = user.LoginName;
                }
                else
                {
                    var configUser = ConfigurationManager.AppSettings["siteBuilderUserLoginName"];

                    if (!string.IsNullOrWhiteSpace(configUser))
                    {
                        retVal = configUser;
                    }
                }

                return retVal;
            }
        }

        /// <summary>
        /// Get's username of umbraco user whose account is used with Umbraco API
        /// </summary>
        public static bool SuppressSynchronization
        {
            get
            {
                bool retVal = true;
                
                string isSuppressed = 
                    ConfigurationManager.AppSettings["siteBuilderSuppressSynchronization"]
                    // this value is kept here to maintain backwards compatibility
                    ?? ConfigurationManager.AppSettings["siteBuilderSupressSynchronization"]; 

                if (!string.IsNullOrEmpty(isSuppressed))
                {
                    retVal = Convert.ToBoolean(isSuppressed);
                }

                return retVal;
            }
        }


        /// <summary>
        /// Gets the ';' seperated list of assemblies to scan, if empty all assemblies will be scanned
        /// </summary>
        public static string Assemblies
        {
            get
            {
                string retVal = ConfigurationManager.AppSettings["siteBuilderAssembly"];

                if (String.IsNullOrEmpty(retVal))
                {
                    retVal = "";
                }

                return retVal;
            }
        }
    
    
    
    }
}
