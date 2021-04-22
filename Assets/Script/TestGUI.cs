using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGUI : MonoBehaviour
{
    public float version=1.0f;
    public float drawSpeed = 210f;
    public MainManager mainManager;
    public DataManager dataManager;
    public DrawLine drawLine;
    public bool isUseGUI=true;
    private void Start() {
        GUIStart();
    }
    public void GUIStart(){
        drawLine = GetComponent<DrawLine>();
        dataManager = GetComponent<DataManager>();
        mainManager = GetComponent<MainManager>();
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)){
            isUseGUI= !isUseGUI;
        }
    }

    private void OnGUI() {
        if(!isUseGUI)return;

        int i=1;
        int width = mainManager.mode == Mode.draw ? 6 : 5;
        GUI.Box(new Rect(50, 50, 300, 50 * width), "");
        Rect versionLable = new Rect(50, 50*i++, 150, 50);
        string versionText = "version : " + version;
        GUI.Label(versionLable, versionText);

        Rect timeLable = new Rect(50,50*i++,150,50);
        string time="Current TIme : " + Time.time;
        GUI.Label(timeLable, time);

        drawSpeed=GUI.HorizontalSlider(new Rect(175, 50 * i, 150, 50), drawSpeed,1.0f,1000f);
        mainManager.speed = drawSpeed;
        Rect speedLable = new Rect(50, 50 * i++, 150, 50);
        string speedText = "speed : " + (int)drawSpeed;
        GUI.Label(speedLable, speedText);

        if(mainManager.mode== Mode.draw)
        {
            Rect outputFIle = new Rect(50, 50 * i++, 150, 50);
            if (GUI.Button(outputFIle, "output jason"))
            {
                if (dataManager != null)
                    dataManager.OutputData();
            }

            Rect clearLineButton = new Rect(50, 50 * i++, 150, 50);
            if (GUI.Button(clearLineButton, "clear"))
            {
                if (drawLine != null)
                    drawLine.ClearLine();
            }
        }
        else
        {
            Rect filePathLable = new Rect(50, 50 * i++, 300, 50);
            string filePathText = "File Path : " + dataManager.readPath;
            GUI.Label(filePathLable, filePathText);
        }

        Rect switchModeButton = new Rect(50,50 * i++, 150,50);
        if(GUI.Button(switchModeButton, "switch mode")){
            if(mainManager!=null){
                mainManager.SwtichMode();
            }
        }
        

    }
}
