using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using UnityEngine.Networking;

public class XML_Loader
{
    private string[] names;

    private XmlDocument database = new XmlDocument();
    Dictionary<string, string> target_path = new Dictionary<string, string>(){
        { "Image", "Tracking/ImageTarget"}

    };
    public XmlDocument LoadXMLDatabase(string path, string targetType)
    {
        database = new XmlDocument();
        if (Application.platform == RuntimePlatform.Android)
        {
            var webrequest = UnityWebRequest.Get("jar:file://" + Application.dataPath + "!/assets/" + path + ".xml");
            webrequest.SendWebRequest();
            while (!webrequest.isDone) ;
            return loadData(webrequest.downloadHandler.text, targetType, true);
        }

        else return loadData("Assets/StreamingAssets/" + path + ".xml", targetType);
    }

    private XmlDocument loadData(string path, string targetType,  bool mode = false)
    {
        Debug.Log(path);
        if (mode) database.LoadXml(path);
        else database.Load(path);
        XmlElement root = database.DocumentElement;
        XmlNodeList nodes = root.SelectNodes(target_path[targetType]);
        int max = nodes.Count;
        names = new string[max];
        int temp = 0;
        foreach (XmlNode node in nodes)
        {
            names[temp] = node.Attributes["name"].InnerText;
            Debug.Log(names[temp]);
            temp++;
        }
        return database;
    }


    public XmlDocument GetDatabase()
    {
        return database;
    }

    public string[] GetItemNames()
    {
        return names;
    }
}
