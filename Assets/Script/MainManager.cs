using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MainManagerData
{
    public float speed;
    public float frameRate;
}
public enum Mode{draw, autoDraw}
public class MainManager : MonoBehaviour
{
    
    public Mode mode=Mode.autoDraw;
    
    public float speed=1;
    public float frameRate=24;
    float nextDrawTime=0;
    float guiTimer = -1;
    Vector3 currentPosition=Vector3.zero;

    DrawLine drawLine;
    DataManager dataManager;
    JsonOutputForm pastPainting;

    struct AutoDrawData{
        public Vector3 currentPos;
        public int nowIndex;
        public int nextIndex;
    }
    AutoDrawData autoDrawData;

    // Start is called before the first frame update
    void Start()
    {
        drawLine = GetComponent<DrawLine>();
        dataManager = GetComponent<DataManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time < nextDrawTime)return;
        nextDrawTime=(1/frameRate)+Time.time;

        if(mode==Mode.draw)drawLine.draw();
        else if(mode==Mode.autoDraw)AutoDraw();
    }
    public void SwtichMode(){
        /*drawLine.ClearLine();*/

        if(mode == Mode.draw)
            mode = Mode.autoDraw;
        else if(mode == Mode.autoDraw)
            mode = Mode.draw;

        pastPainting=null;
        autoDrawData.nowIndex=0;
    }

    void AutoDraw()
    {
        //read data from data manager
        if (pastPainting == null)
        {
            pastPainting = dataManager.ReadDataFromPath(dataManager.readPath);
            if (pastPainting == null)
            {
                guiTimer = 15;
                SwtichMode();
                return;
            }
        }
        //draw first point
        if(autoDrawData.nowIndex > pastPainting.positionData.Length-1 || autoDrawData.nowIndex<=0){
            Vector3 point = pastPainting.positionData[0];
            drawLine.CreateLine(point);
            autoDrawData.nowIndex=1;
            autoDrawData.currentPos=pastPainting.positionData[0];
            currentPosition=autoDrawData.currentPos;
        }

        //calculate currentPosition
        if (Vector3.Distance(pastPainting.positionData[autoDrawData.nowIndex], currentPosition) <= 0.1f)
        {
            autoDrawData.nowIndex++;
        }
        else
        {
            Vector3 lastPosition = autoDrawData.nowIndex == 0 ? pastPainting.positionData[autoDrawData.nowIndex] : pastPainting.positionData[autoDrawData.nowIndex - 1];
            Vector3 nextPosition = pastPainting.positionData[autoDrawData.nowIndex];

            Vector3 deltaMoveDirection = (nextPosition - lastPosition).normalized * speed * Time.deltaTime;
            currentPosition += deltaMoveDirection;

            //check the position won't go too far away
            float startToCurrent = Vector3.Distance(lastPosition, currentPosition);
            float startToEnd = Vector3.Distance(lastPosition, nextPosition);
            while (startToCurrent > startToEnd )
            {

                autoDrawData.currentPos = currentPosition;
                drawLine.UpdateLine(nextPosition);
                autoDrawData.nowIndex++;
                if (autoDrawData.nowIndex > pastPainting.positionData.Length - 1) return;

                lastPosition = pastPainting.positionData[autoDrawData.nowIndex - 1];
                nextPosition = pastPainting.positionData[autoDrawData.nowIndex];

                startToCurrent = Vector3.Distance(lastPosition, currentPosition);
                startToEnd = Vector3.Distance(lastPosition, nextPosition);

                currentPosition = (nextPosition - lastPosition).normalized*startToCurrent + lastPosition;
            }
            autoDrawData.currentPos = currentPosition;
            drawLine.UpdateLine(autoDrawData.currentPos);
        }
    }
    void OnGUI()
    {
        if(guiTimer >= 0)
        {
            guiTimer-= Time.deltaTime;
            GUI.Box(new Rect(900, 500, 400, 50), "Can't find position data so automatically switch to draw mode");
        }
    }
}
