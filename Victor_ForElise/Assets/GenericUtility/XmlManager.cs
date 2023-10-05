using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml.Serialization;
using System.IO;


// ---------------------------------------------
//
// v1.0 ('20.04.13)
//  - 스크립트 작성
//
// ---------------------------------------------


/// <summary>
/// <para>Document : https://docs.microsoft.com/ko-kr/dotnet/api/system.xml.serialization.xmlserializer?view=netframework-4.8</para>
/// <para># Serialize 또는 Deserialize 할 수 없는 것들</para>
/// <para> - ArrayList 배열</para>
/// <para> - List<T> 배열</T></para>
/// <para> - [Obsolte]로 표시된 개체 (.NET Framework 3.5 이상부터)</para>
/// </summary>
public static class _XmlManager
{
    public static string Serialize<T>(T obj)
    {
        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        using (StringWriter textWriter = new StringWriter())
        {
            xmlSerializer.Serialize(textWriter, obj);
            return textWriter.ToString();
        }
    }

    public static T Deserialize<T>(string xml)
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        serializer.UnknownNode += new XmlNodeEventHandler(serializer_UnknownNode);
        serializer.UnknownAttribute += new XmlAttributeEventHandler(serializer_UnknownAttribute);

        using (StringReader textReader = new StringReader(xml))
        {
            return (T)serializer.Deserialize(textReader);
        }
    }

    private static void serializer_UnknownNode(object sender, XmlNodeEventArgs e)
    {
        Debug.Log("Unknown Node:" + e.Name + "\t" + e.Text);
    }

    private static void serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
    {
        System.Xml.XmlAttribute attr = e.Attr;
        Debug.Log("Unknown attribute " +
        attr.Name + "='" + attr.Value + "'");
    }
}