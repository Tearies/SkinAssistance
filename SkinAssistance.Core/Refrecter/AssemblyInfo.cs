#region NS

using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using SkinAssistance.Core.Extensions;

#endregion

namespace SkinAssistance.Core.Refrecter
{
    #region Reference

    #endregion

    [HostProtection(SecurityAction.LinkDemand, Resources = HostProtectionResource.ExternalProcessMgmt)]
    internal class AssemblyInfos
    {
        #region Properties

        private readonly Assembly m_Assembly;
        private string m_CompanyName;
        private string m_Copyright;
        private string m_Description;
        private string m_ProductName;
        private string m_Title;
        private string m_Trademark;
        private string m_VersionInfoString;

        public string Description
        {
            get
            {
                if (m_Description == null)
                    if (m_Assembly != null)
                    {
                        var descriptionAttribute =
                            m_Assembly.GetAssemblyCustermAttribute<AssemblyDescriptionAttribute>();
                        m_Description = descriptionAttribute != null ? descriptionAttribute.Description : "";
                    }
                return m_Description;
            }
        }

        public string CompanyName
        {
            get
            {
                if (m_CompanyName == null)
                    if (m_Assembly != null)
                    {
                        var companyAttribute = m_Assembly.GetAssemblyCustermAttribute<AssemblyCompanyAttribute>();
                        m_CompanyName = companyAttribute != null ? companyAttribute.Company : "";
                    }
                return m_CompanyName;
            }
        }

        public string Title
        {
            get
            {
                if (m_Title == null)
                    if (m_Assembly != null)
                    {
                        var assemblyTitleAttribute = m_Assembly.GetAssemblyCustermAttribute<AssemblyTitleAttribute>();
                        m_Title = assemblyTitleAttribute != null ? assemblyTitleAttribute.Title : "";
                    }
                return m_Title;
            }
        }

        public string VersionInfoString
        {
            get
            {
                if (m_VersionInfoString == null)
                    if (m_Assembly != null)
                    {
                        var assemblyTitleAttribute = m_Assembly.GetAssemblyCustermAttribute<AssemblyFileVersionAttribute>();
                        m_VersionInfoString = assemblyTitleAttribute != null ? assemblyTitleAttribute.Version : "";
                    }
                return m_VersionInfoString;
            }
        }

        public string Copyright
        {
            get
            {
                if (m_Copyright == null)
                    if (m_Assembly != null)
                    {
                        var copyrightAttribute = m_Assembly.GetAssemblyCustermAttribute<AssemblyCopyrightAttribute>();
                        m_Copyright = copyrightAttribute != null ? copyrightAttribute.Copyright : "";
                    }
                return m_Copyright;
            }
        }

        public string Trademark
        {
            get
            {
                if (m_Trademark == null)
                    if (m_Assembly != null)
                    {
                        var trademarkAttribute = m_Assembly.GetAssemblyCustermAttribute<AssemblyTrademarkAttribute>();
                        m_Trademark = trademarkAttribute != null ? trademarkAttribute.Trademark : "";
                    }
                return m_Trademark;
            }
        }

        public string ProductName
        {
            get
            {
                if (m_ProductName == null)
                    if (m_Assembly != null)
                    {
                        var productAttribute = m_Assembly.GetAssemblyCustermAttribute<AssemblyProductAttribute>();
                        m_ProductName = productAttribute != null ? productAttribute.Product : "";
                    }
                    else
                    {
                        m_ProductName = "UITest";
                    }

                return m_ProductName;
            }
        }

        public Version Version
        {
            get { return m_Assembly == null ? new Version(1, 0, 0, 0) : (m_Assembly.GetAssemblyCustermAttribute<AssemblyDistinationVersionAttribute>().Version); }
        }

        public string AssemblyName
        {
            get { return m_Assembly == null ? "" : m_Assembly.GetName().Name; }
        }

        public string DirectoryPath
        {
            get { return m_Assembly == null ? "" : Path.GetDirectoryName(m_Assembly.Location); }
        }

        public string StackTrace
        {
            get { return Environment.StackTrace; }
        }

        public long WorkingSet
        {
            get { return Environment.WorkingSet; }
        }


        public string ProductId
        {
            get { return m_Assembly != null ? m_Assembly.GetAssemblyCustermAttribute<GuidAttribute>().Value : ""; }
        }

        #endregion

        #region Method

        protected AssemblyInfos(Assembly currentAssembly)
        {
            m_Description = null;
            m_Title = null;
            m_ProductName = null;
            m_CompanyName = null;
            m_Trademark = null;
            m_Copyright = null;
            m_Assembly = currentAssembly;
        }

        #endregion
    }
}