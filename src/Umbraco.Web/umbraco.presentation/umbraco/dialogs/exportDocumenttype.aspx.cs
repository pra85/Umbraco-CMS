using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using umbraco.cms.businesslogic.web;
using System.Xml;
using Umbraco.Core;

namespace umbraco.presentation.dialogs
{
	/// <summary>
	/// Summary description for exportDocumenttype.
	/// </summary>
	public class exportDocumenttype : BasePages.UmbracoEnsuredPage
	{
	    public exportDocumenttype()
	    {
            CurrentApp = BusinessLogic.DefaultApps.settings.ToString();

	    }
		private void Page_Load(object sender, System.EventArgs e)
		{
			int documentTypeId = int.Parse(helper.Request("nodeID"));
			if (documentTypeId > 0) 
			{
				var dt = new cms.businesslogic.web.DocumentType(documentTypeId);
                var folderNames = string.Empty;
                if (dt.Level != 1)
                {
                    var folders = new List<string>();

                    var current = dt.Parent;
                    while (current.Level >= 1)
                    {
                        if (current.nodeObjectType == Constants.ObjectTypes.DocumentTypeContainerGuid)
                            folders.Add(HttpUtility.UrlEncode(current.Text));

                        if (current.Level == 1)
                            break;
                        current = current.Parent;
                    }

                    folderNames = string.Join("/", folders.ToArray().Reverse());
                }

                Response.AddHeader("Content-Disposition", "attachment;filename=" + dt.Alias + ".udt");
                Response.ContentType = "application/octet-stream";

                XmlDocument doc = new XmlDocument();
                doc.AppendChild(dt.ToXml(doc, folderNames));


                XmlWriterSettings writerSettings = new XmlWriterSettings();
                writerSettings.Indent = true;

                XmlWriter xmlWriter = XmlWriter.Create(Response.OutputStream, writerSettings);
                doc.Save(xmlWriter);

                //Response.Write(editDataType.ToXml(new XmlDocument()).OuterXml);
            }
        }

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion
	}
}
