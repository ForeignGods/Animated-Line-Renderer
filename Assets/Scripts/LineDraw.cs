
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDraw: MonoBehaviour
{
    private LineRenderer lineRenderer;
    public GameObject drawingPrefab;
    public Material material;
    private Color randomColor;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject drawing = Instantiate(drawingPrefab);
            lineRenderer = drawing.GetComponent<LineRenderer>();
            lineRenderer.startWidth = 0.15f;
            lineRenderer.endWidth = 0.15f;
            Randomize();
            lineRenderer.startColor = randomColor;
            lineRenderer.endColor = randomColor;
        }

        if (Input.GetMouseButton(0))
        {
            FreeDraw();
        }  

    }

    void Randomize()
    {
        int randomInt = Random.Range(1, 5); 

        switch (randomInt)
        {
            case 4:
                material.SetFloat("width", 0.5f); 
                material.SetFloat("heigth", 0.1f); 
                randomColor = Color.yellow;
                break;
            case 3:
                material.SetFloat("width", 0.5f); 
                material.SetFloat("heigth", 1f); 
                randomColor = Color.cyan;
                break;
            case 2:
                material.SetFloat("width", 0.75f); 
                material.SetFloat("heigth", 0.1f); 
                randomColor = Color.green;
                break;      
            case 1:
                material.SetFloat("width", 0.4f); 
                material.SetFloat("heigth", 0.8f); 
                randomColor = Color.red;
                break;
        }
     
    }

    void FreeDraw()
    {

        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f);
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, Camera.main.ScreenToWorldPoint(mousePos));

    }
}

