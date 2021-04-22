using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

/* Custom data structure : 
 * save positions dara (vector3)
*/
public class JsonOutputForm{
    public Vector3[] positionData;
    public JsonOutputForm(int dataCount)
    {
        positionData = new Vector3[dataCount];  
    }
    
}
/* Manage function :
 * 1. Output data (custom data structure) with json object data type
 * 2. Read json file from direct path (default is desktop)
*/
[RequireComponent(typeof(DrawLine))]
public class DataManager : MonoBehaviour
{
    DrawLine drawLine;
    public string fileName = "positionData.json";//need to edit by json file
    public string readPath;
    // Start is called before the first frame update
    void Start()
    {
        drawLine=GetComponent<DrawLine>();
        readPath=(readPath=="")?Environment.GetFolderPath(Environment.SpecialFolder.Desktop) +"/"+ fileName:readPath;
        
    }

    public Vector3[] ReadDataFromTarget()
    {
        GameObject lineObject = drawLine.currentLine;
        LineRenderer lineData = lineObject.GetComponent<LineRenderer>();
        JsonOutputForm newFile=new JsonOutputForm(lineData.positionCount);
        lineData.GetPositions(newFile.positionData);
        return newFile.positionData;

    }
    public void OutputData()
    {
        JsonOutputForm newFile=new JsonOutputForm(0);
        newFile.positionData=ReadDataFromTarget();
        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) +"/"+ fileName;
        SaveArrayAsJson(newFile, path);
    }

    public JsonOutputForm ReadDataFromPath(string path)
    {
        try
        {
            StreamReader file = File.OpenText(path);
            string data = file.ReadToEnd();
            JsonOutputForm positions = (JsonOutputForm)JsonConvert.DeserializeObject(data, typeof(JsonOutputForm));
            return positions;
        }
        catch
        {
            return null;
        }
        
    }

    private static void SaveArrayAsJson(JsonOutputForm arrayToSave, string fileName)
    {
        try
        {
            using (StreamWriter file = new StreamWriter(fileName))
            {
                string data = JsonConvert.SerializeObject(arrayToSave);
                file.Write(data);
                file.Close();
            }
        }
        finally
        {

        }
    }
}
