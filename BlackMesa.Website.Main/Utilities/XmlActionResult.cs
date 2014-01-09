using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace BlackMesa.Website.Main.Utilities
{
    public class XmlActionResult : ActionResult
    {
        public XmlActionResult(string xml, string fileName,
            EncodingType encoding = EncodingType.UTF8,
            LoadOptions loadOptions = System.Xml.Linq.LoadOptions.None)
        {
            XmlContent = xml;
            FileName = fileName;
            Encoding = encoding;
            LoadOptions = loadOptions;
        }

        public string FileName
        {
            get;
            set;
        }

        public string XmlContent
        {
            get;
            set;
        }

        public EncodingType Encoding
        {
            get;
            set;
        }

        public LoadOptions LoadOptions
        {
            get;
            set;
        }

        public XmlDocument ToXmlDocument(XDocument xdoc)
        {
            var xmldoc = new XmlDocument();
            xmldoc.Load(xdoc.CreateReader());
            return xmldoc;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            XDocument doc = XDocument.Parse(XmlContent, this.LoadOptions);
            context.HttpContext.Response.ContentType = "text/xml";
            context.HttpContext.Response.AddHeader("content-disposition",
              string.Format("attachment; filename={0}", FileName));

            XmlDocument xmldoc = ToXmlDocument(doc);
            // Create an XML declaration. 
            XmlDeclaration xmldecl;
            xmldecl = xmldoc.CreateXmlDeclaration("1.0", null, null);

            switch (Encoding)
            {
                case EncodingType.UTF8:
                    doc.Declaration = new XDeclaration("1.0", "utf-8", null);
                    context.HttpContext.Response.Charset = "utf-8";
                    xmldecl.Encoding = "UTF-8";
                    XmlElement root = xmldoc.DocumentElement;
                    xmldoc.InsertBefore(xmldecl, root);
                    context.HttpContext.Response.BinaryWrite(
                      System.Text.UTF8Encoding.Default.GetBytes(xmldoc.OuterXml));
                    break;
                case EncodingType.UTF16:
                    doc.Declaration = new XDeclaration("1.0", "utf-16", null);
                    context.HttpContext.Response.Charset = "utf-16";
                    xmldecl.Encoding = "UTF-16";
                    root = xmldoc.DocumentElement;
                    xmldoc.InsertBefore(xmldecl, root);
                    context.HttpContext.Response.BinaryWrite(
                      System.Text.UnicodeEncoding.Default.GetBytes(xmldoc.OuterXml));
                    break;
                case EncodingType.UTF32:
                    doc.Declaration = new XDeclaration("1.0", "utf-32", null);
                    context.HttpContext.Response.Charset = "utf-32";
                    xmldecl.Encoding = "UTF-32";
                    root = xmldoc.DocumentElement;
                    xmldoc.InsertBefore(xmldecl, root);
                    context.HttpContext.Response.BinaryWrite(
                      System.Text.UTF32Encoding.Default.GetBytes(xmldoc.OuterXml));
                    break;
            }

            context.HttpContext.Response.End();
        }
    }

    public enum EncodingType
    {
        UTF8,
        UTF16,
        UTF32
    }


}