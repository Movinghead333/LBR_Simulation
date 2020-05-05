using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Text;

public class Plan
{
    public PickAndPlaceTask[] tasks;
    public GameObject[] objects;

    public Plan(string filePath)
    {
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
        XmlDocument xmlDoc = new XmlDocument();

        TextAsset xmlFile = (TextAsset)Resources.Load(filePath, typeof(TextAsset));
        xmlDoc.LoadXml(xmlFile.text);

        //xmlDoc.Load(filePath);

        tasks = new PickAndPlaceTask[xmlDoc.DocumentElement.ChildNodes.Count];
        int index = 0;
        foreach (XmlNode xmlNode in xmlDoc.DocumentElement.ChildNodes)
        {
            float height = float.Parse(xmlNode.Attributes["object_height"].Value);

            Vector3 from = new Vector3(
                float.Parse(xmlNode.Attributes["fx"].Value),
                float.Parse(xmlNode.Attributes["fy"].Value) + height,
                float.Parse(xmlNode.Attributes["fz"].Value));
            Vector3 to = new Vector3(
                float.Parse(xmlNode.Attributes["tx"].Value),
                float.Parse(xmlNode.Attributes["ty"].Value) + height,
                float.Parse(xmlNode.Attributes["tz"].Value));


            string name = xmlNode.Attributes["name"].Value;

            tasks[index] = new PickAndPlaceTask(from, to, height, name);
            //Debug.Log("Vectors are:");
            //Debug.Log(from);
            //Debug.Log(to);
            index++;
        }
    }
}
