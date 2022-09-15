// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.Desktop.XmlNamespaceHelper
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Microsoft.Band.Admin.Desktop
{
  internal class XmlNamespaceHelper : XmlNamespaceHelperBase
  {
    private XmlNamespaceManager xmlnm;
    private Dictionary<string, string> namespaces;

    private XmlNamespaceHelper()
    {
    }

    public XmlNamespaceHelper(XmlDocument doc)
    {
      this.xmlnm = new XmlNamespaceManager(doc.NameTable);
      this.namespaces = this.GenerateNamespaceList(doc);
      foreach (KeyValuePair<string, string> keyValuePair in this.namespaces)
        this.xmlnm.AddNamespace(keyValuePair.Key, keyValuePair.Value);
    }

    public override string ResolveNodeWithNamespace(XmlNode node, string xpath)
    {
      XmlNode xmlNode = node.SelectSingleNode(xpath, this.xmlnm);
      if (xmlNode == null)
        return (string) null;
      StringBuilder stringBuilder = new StringBuilder();
      foreach (XmlNode childNode in xmlNode.ChildNodes)
      {
        string str = childNode.InnerText.Trim();
        if (str.Length > 0)
        {
          if (stringBuilder.Length > 0)
            stringBuilder.AppendLine();
          stringBuilder.Append(str);
        }
      }
      return stringBuilder.ToString();
    }

    public override void RemoveDefaultNamespace(XmlDocument doc) => doc.LoadXml(doc.OuterXml.Replace("xmlns=\"", "xmlns:msbwt=\""));

    private Dictionary<string, string> GenerateNamespaceList(XmlDocument doc)
    {
      Dictionary<string, string> dictionary = new Dictionary<string, string>();
      Queue<XmlElement> xmlElementQueue = new Queue<XmlElement>();
      xmlElementQueue.Enqueue(doc.DocumentElement);
      while (xmlElementQueue.Count != 0)
      {
        XmlElement xmlElement = xmlElementQueue.Dequeue();
        foreach (XmlAttribute attribute in (XmlNamedNodeMap) xmlElement.Attributes)
        {
          if (attribute.Prefix == "xmlns" && !dictionary.ContainsKey(attribute.LocalName))
            dictionary.Add(attribute.LocalName, attribute.Value);
        }
        foreach (XmlNode childNode in xmlElement.ChildNodes)
        {
          if (childNode.GetType() == typeof (XmlElement))
            xmlElementQueue.Enqueue(childNode as XmlElement);
        }
      }
      return dictionary;
    }
  }
}
