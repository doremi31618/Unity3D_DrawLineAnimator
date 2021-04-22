using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DrawLine : MonoBehaviour
{
    [Header("LineRender Attribute")]
    public GameObject linePrefab;
    public Color lineColor;
    public float lineWidth=0.15f;

    [Header("Drawing Attribute")]
    public GameObject target;
   
    public List<Vector2> fingerPos;
    [HideInInspector]public GameObject currentLine;
    [HideInInspector]public LineRenderer lineRenderer;
    [HideInInspector]public EdgeCollider2D edgeCollider;
    

	

    private void Update()
    {
        // WARN!!! : processing in MainManager script
        // draw();
    }

    public void draw(){
        var mousePos = Input.mousePosition;
        mousePos.z = 10;
        Vector3 tempFingerPos = Camera.main.ScreenToWorldPoint(mousePos);

        //when presse mouse button 
        if (Input.GetMouseButtonDown(0))
            if(currentLine==null || fingerPos.Count==0)
                CreateLine(tempFingerPos);
            
        if (Input.GetMouseButton(0)){
            if(fingerPos.Count<1)return;
            if (Vector2.Distance(tempFingerPos,fingerPos[fingerPos.Count - 1]) > .1f){
                UpdateLine(tempFingerPos);
            }
        }
    }


    public void CreateLine(Vector3 point)
    {
        //if havent create Line renderer , instantiate one from prefab
        if(currentLine==null){
            currentLine = Instantiate(linePrefab,  target.transform);

            lineRenderer = currentLine.GetComponent<LineRenderer>();
            lineRenderer.SetColors(lineColor,lineColor);
            lineRenderer.SetWidth(lineWidth,lineWidth);

            // edgeCollider = currentLine.GetComponent<EdgeCollider2D>();
        }

        fingerPos.Clear();
        fingerPos.Add(point);
        fingerPos.Add(point);
        lineRenderer.positionCount=2;
        lineRenderer.SetPosition(0, fingerPos[0]);
        lineRenderer.SetPosition(1, fingerPos[1]);

        // edgeCollider.points = fingerPos.ToArray();
    }

    
    public void UpdateLine(Vector2 newFingerPos)
    {
        if(lineRenderer.positionCount<=0 || fingerPos.Count==0 )return;
        if(fingerPos[fingerPos.Count-1]==newFingerPos )return;

        fingerPos.Add(newFingerPos);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, newFingerPos);
        // edgeCollider.points = fingerPos.ToArray();
    } 

    public void ClearLine()
    {
        fingerPos.Clear();
        lineRenderer.positionCount=0;
    }
    
}