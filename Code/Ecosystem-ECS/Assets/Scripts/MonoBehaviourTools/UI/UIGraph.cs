using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Entities;
using UnityEngine.UI;

//maybe we need these function later on, this function can modify the number of red line in the graph
//now only apply in 5 line in X aixs and 5 line in y axis not sure need to updata,discuss monday
public class UIGraph : MonoBehaviour
{
    public SimulationManager simulationManager;

    [SerializeField] private Sprite circleSprite;
    [SerializeField] Vector2 Line = new Vector2(5, 5);

    private float XPos;
    private float RabbitNumber;
    private float yMaximum;
    private float xMaximum;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;
    private float graphHeight;
    private float graphWidth;
    private float parentWidth;
    private float parentHeight;

    private void Awake()
    {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        parentWidth = graphContainer.rect.width;
        parentHeight = graphContainer.rect.height;
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        dashTemplateX = graphContainer.Find("dashTemplateX").GetComponent<RectTransform>();
        
        dashTemplateY = graphContainer.Find("dashTemplateY").GetComponent<RectTransform>();

        dashTemplateX.sizeDelta = new Vector2(parentWidth, 3f);
        dashTemplateY.sizeDelta = new Vector2(parentHeight, 3f);
        //List<int> valueList = new List<int>() { 5, 98, 56, 45, 30, 22, 17, 15, 13, 17, 25, 37, 40, 36, 33 };
        //ShowGraph(valueList);
        
    }
    private void Start()
    {
        RabbitNumber = simulationManager.RabbitSpawn();
        yMaximum = RabbitNumber * 5;
        xMaximum = 100f;

        graphHeight = graphContainer.sizeDelta.y;
        graphWidth = graphContainer.sizeDelta.x;

        // create line in awake based on vector2 Line input from inspector
        Create(Line);
    }
    private void Update()
    {
        
        if (RabbitNumber / 8 * 10 > yMaximum)
        {
            //yMaximum = 2 * yMaximum;
            yMaximum = RabbitNumber / 8 * 10;
            UpdataYAxis();
            //Line = new Vector2(Line.x, Line.y * 2);
            //DecreaseY();
            //DestoryLineY();
            //Create(Line);

        }
        print(XPos);
        if (XPos >= xMaximum)
        {
            //xMaximum = XPos * 2;
            //UpdataXAxis();

            xMaximum = 2 * xMaximum;
            UpdataXAxis();
            DecreaseX();
            //DestoryLineX();
            //Create(Line);
        }
        RabbitNumber = simulationManager.RabbitPopulation();
        XPos += 1f * Time.deltaTime;    // X aixs is same with turn number in time counter system 
        ShowGraph(XPos,RabbitNumber);
    }
    void UpdataYAxis()
    {
        Transform[] AllGameObject = graphContainer.GetComponentsInChildren<Transform>();
        foreach (Transform Child in AllGameObject)
        {
            if (Child.name.Contains("labelTemplateY"))
            {
                Destroy(Child.gameObject);
            }
        }

        for (int i = 1; i <= Line.y; i++)
        {
            int YText = (int)(yMaximum / Line.y * i);
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            labelY.anchoredPosition = new Vector2(-7f, graphHeight / Line.y * i);    //####
            labelY.GetComponent<Text>().text = YText.ToString();
        }

    }


    void UpdataXAxis()
    {
        Transform[] AllGameObject = graphContainer.GetComponentsInChildren<Transform>();
        foreach (Transform Child in AllGameObject)
        {
            if (Child.name.Contains("labelTemplateX"))
            {
                Destroy(Child.gameObject);
            }
        }

        for (int i = 1; i <= Line.x; i++)
        {
            int XText = (int)(xMaximum / Line.x * i);
            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(graphWidth / Line.x * i * 1f, -7f);
            labelX.GetComponent<Text>().text = XText.ToString();
        }

    }

    void Create(Vector2 Line)
    {

        for (int i = 1; i <= Line.x; i++)
        {

            int XText = (int)(xMaximum/ Line.x * i);
           
            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(graphWidth / Line.x * i * 1f, -7f);
            labelX.GetComponent<Text>().text = XText.ToString();

            RectTransform DashY = Instantiate(dashTemplateY);
            DashY.SetParent(graphContainer, false);
            DashY.gameObject.SetActive(true);
            
            DashY.anchoredPosition = new Vector2(graphWidth / Line.x * i * 1f, dashTemplateY.anchoredPosition.y);
        }


        for (int i = 1; i <= Line.y; i++)
        {
            int YText = (int)(yMaximum / Line.y * i);
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            labelY.anchoredPosition = new Vector2(-7f, graphHeight / Line.y * i );    //####
            labelY.GetComponent<Text>().text = YText.ToString();

            RectTransform DashX = Instantiate(dashTemplateX);
            DashX.SetParent(graphContainer, false);
            DashX.gameObject.SetActive(true);
            
            DashX.anchoredPosition = new Vector2(dashTemplateX.anchoredPosition.x, graphHeight / Line.y * i);     //####
        }
    }
    private GameObject CreateCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(3, 3);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }
    //if we need to modify the line number, just leave this here,reason in the line 6
    //private void DecreaseY()
    //{
    //    RectTransform[] AllGameObject = graphContainer.GetComponentsInChildren<RectTransform>();
    //    foreach (RectTransform Child in AllGameObject)
    //    {
    //        if (Child.name.Contains("circle"))
    //        {
    //            Child.anchoredPosition = new Vector2 (Child.anchoredPosition.x, Child.anchoredPosition.y / 2);
    //        }
    //    }
    //}
    private void DecreaseX()
    {
        RectTransform[] AllGameObject = graphContainer.GetComponentsInChildren<RectTransform>();
        foreach (RectTransform Child in AllGameObject)
        {
            if (Child.name.Contains("circle"))
            {
                Child.anchoredPosition = new Vector2(Child.anchoredPosition.x / 2, Child.anchoredPosition.y);
            }
        }

    }

    private void ShowGraph(float xValue,float yValue)
    {
        //float yMaximum = 500f;
        //GameObject circleGameObject = null;
        //float xPosition = xSize + i * xSize;
        float yPosition = (yValue / yMaximum) * graphHeight;
        float xPosition = (xValue / xMaximum) * graphWidth;
        CreateCircle(new Vector2(xPosition, yPosition));
    }

    //just leave code here, not sure useful later reason in the line 6
    //private void DestoryLineY()
    //{
    //    Transform[] AllGameObject = graphContainer.GetComponentsInChildren<Transform>();
    //    foreach (Transform Child in AllGameObject)
    //    {
    //        if (Child.name.Contains("labelTemplateY"))
    //        {
    //            Destroy(Child.gameObject);
    //        }
    //        if (Child.name.Contains("dashTemplateX"))
    //        {
    //            Destroy(Child.gameObject);
    //        }
    //    }
    //}
    //private void DestoryLineX()
    //{
    //    Transform[] AllGameObject = graphContainer.GetComponentsInChildren<Transform>();
    //    foreach (Transform Child in AllGameObject)
    //    {
    //        if (Child.name.Contains("labelTemplateX"))
    //        {
    //            Destroy(Child.gameObject);
    //        }
    //        if (Child.name.Contains("dashTemplateY"))
    //        {
    //            Destroy(Child.gameObject);
    //        }
    //    }
    //}
}
