using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Entities;
using UnityEngine.UI;
using SFB;
using System.IO;
using System;

//maybe we need these function later on, this function can modify the number of red line in the graph
//now only apply in 5 line in X aixs and 5 line in y axis not sure need to updata,discuss monday
public class UIGraph : MonoBehaviour
{
    public SimulationManager simulationManager;

    [SerializeField] private Sprite circleSprite;
    [SerializeField] Vector2 Line = new Vector2(5, 5);

    [SerializeField] InputField inputField;
    [SerializeField] Button Submit;
    [SerializeField] Button Save;

    public TimeControlSystem timeControlSystem;

    private float nextTime;
    private float nextTime2;
    private float XPos;
    private float RabbitNumber;
    private float FoxNumber;
    private float yMaximum;
    private float xMaximum;
    private float InyMaximum;
    private float InxMaximum;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform LabelXContainer;
    private RectTransform LabelYContainer;
    private RectTransform labelTemplateY;
    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;
    private RectTransform CircleContainer;
    private float graphHeight;
    private float graphWidth;
    private float parentWidth;
    private float parentHeight;
    private int input;
    List<float> GraphRabbitList = new List<float> ();
    List<float> GraphFoxList = new List<float>();

    private void Awake()
    {
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        parentWidth = graphContainer.rect.width;
        parentHeight = graphContainer.rect.height;

        LabelXContainer = graphContainer.Find("LabelXContainer").GetComponent<RectTransform>();
        LabelYContainer = graphContainer.Find("LabelYContainer").GetComponent<RectTransform>();
        labelTemplateX = LabelXContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = LabelYContainer.Find("labelTemplateY").GetComponent<RectTransform>();

        dashTemplateX = graphContainer.Find("dashTemplateX").GetComponent<RectTransform>();
        dashTemplateY = graphContainer.Find("dashTemplateY").GetComponent<RectTransform>();

        CircleContainer = graphContainer.Find("CircleContainer").GetComponent<RectTransform>();

        dashTemplateX.sizeDelta = new Vector2(parentWidth, 3f);
        dashTemplateY.sizeDelta = new Vector2(parentHeight, 3f);
    }
    private void Start()
    {
        RabbitNumber = simulationManager.RabbitSpawn();
        FoxNumber = simulationManager.FoxSpawn();
        InyMaximum = simulationManager.RabbitSpawn() * 5;
        yMaximum = Mathf.Max(RabbitNumber, FoxNumber) * 5;
        xMaximum = 50f;
        InxMaximum = 100f;
        nextTime = 0;
        input = 100;

        graphHeight = graphContainer.sizeDelta.y;
        graphWidth = graphContainer.sizeDelta.x;

        // create line in awake based on vector2 Line input from inspector
        Create(Line);
        Submit.onClick.AddListener(ShowTime);
        Save.onClick.AddListener(SaveFile);
    }
    private void Update()
    {
        if (Time.time >= nextTime)
        {
            RabbitNumber = simulationManager.RabbitPopulation();
            FoxNumber = simulationManager.FoxPopulation();
            XPos = Time.time;   
            GraphRabbitList.Add(RabbitNumber);
            GraphFoxList.Add(FoxNumber);
            nextTime += 1;

            if (input >= 60 && input <= 300)
            {
                if (Mathf.Max(RabbitNumber,FoxNumber) / 8 * 10 > yMaximum)
                {
                    InyMaximum = yMaximum;
                    yMaximum = Mathf.Max(RabbitNumber, FoxNumber) / 8 * 10;
                    UpdataYAxis();
                    DecreaseY();
                }

                if (XPos <= input - 1)
                {
                    if (XPos >= xMaximum)
                    {
                        InxMaximum = xMaximum;
                        xMaximum += 1;
                        UpdataXAxis();
                        DecreaseX();
                    }
                    ShowGraph(XPos, RabbitNumber,FoxNumber);
                }
                else
                {
                    ShowGraphList(input);
                }
            }
            else
            {
                if (Time.time >= nextTime2)
                {
                    ShowAllGraph();

                    if (Mathf.Max(RabbitNumber, FoxNumber) / 8 * 10 > yMaximum)
                    {
                        InyMaximum = yMaximum;
                        yMaximum = Mathf.Max(RabbitNumber, FoxNumber) / 8 * 10;
                        UpdataYAxis();
                        DecreaseY();
                    }

                    int a = (int)Mathf.Floor(GraphRabbitList.Count / 100);
                    nextTime2 += a;
                }
            }
        }
    }

    void ShowTime()
    {
        input = int.Parse(inputField.text);
        nextTime2 = Time.time;
    }

    void SaveFile()
    {
        timeControlSystem.Pause();

        string path = StandaloneFileBrowser.SaveFilePanel("Save File", "", "", "csv");
        try
        {
            // Create a new file     
            using (StreamWriter sw = File.CreateText(path))
            {
                for (int i = 0; i < GraphRabbitList.Count; i++) 
                {
                    sw.WriteLine(GraphRabbitList[i]+","+ GraphFoxList[i]);
                }
                sw.Close();
            }
        }
        catch (Exception Ex)
        {
            Console.WriteLine(Ex.ToString());
        }

        timeControlSystem.Play();
    }

    void UpdataYAxis()
    {
        Transform[] AllGameObject = LabelYContainer.GetComponentsInChildren<Transform>();
        foreach (Transform Child in AllGameObject)
        {
            if (Child.name.Contains("Label"))
            {
                continue;
            }
            else
            {
                int labelNumber = int.Parse(Child.name.ToString());
                int YText = (int)(yMaximum / Line.y * labelNumber);
                Child.GetComponent<Text>().text = YText.ToString();
            }
        }
    }


    void UpdataXAxis()
    {
        Transform[] AllGameObject = LabelXContainer.GetComponentsInChildren<Transform>();
        foreach (Transform Child in AllGameObject)
        {
            if (Child.name.Contains("Label"))
            {
                continue;
            }
            else
            {
                int labelNumber = int.Parse(Child.name.ToString());
                int XText = (int)(GraphRabbitList.Count / Line.x * labelNumber);
                Child.GetComponent<Text>().text = XText.ToString();
            }
        }
    }


    void UpdataGraphListXAxis(int number, int value)
    {
        Transform[] AllGameObject = LabelXContainer.GetComponentsInChildren<Transform>();
        foreach (Transform Child in AllGameObject)
        {
            if (Child.name.Contains("Label"))
            {
                continue;
            }
            else
            { 
                int labelNumber = int.Parse(Child.name.ToString());
                int b = (labelNumber - 1) * (value / (int)Line.x) + number;
                Child.GetComponent<Text>().text = b.ToString();
            }
        }
    }

    void Create(Vector2 Line)
    {

        for (int i = 1; i <= Line.x; i++)
        {

            int XText = (int)(xMaximum/ Line.x * i);
           
            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(LabelXContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(graphWidth / Line.x * i * 1f, -7f);
            labelX.GetComponent<Text>().text = XText.ToString();
            labelX.name = i.ToString();

            RectTransform DashY = Instantiate(dashTemplateY);
            DashY.SetParent(graphContainer, false);
            DashY.gameObject.SetActive(true);
            
            DashY.anchoredPosition = new Vector2(graphWidth / Line.x * i * 1f, dashTemplateY.anchoredPosition.y);
        }


        for (int i = 1; i <= Line.y; i++)
        {
            int YText = (int)(yMaximum / Line.y * i);
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(LabelYContainer, false);
            labelY.gameObject.SetActive(true);
            labelY.anchoredPosition = new Vector2(-7f, graphHeight / Line.y * i );    //####
            labelY.GetComponent<Text>().text = YText.ToString();
            labelY.name = i.ToString();

            RectTransform DashX = Instantiate(dashTemplateX);
            DashX.SetParent(graphContainer, false);
            DashX.gameObject.SetActive(true);
            DashX.anchoredPosition = new Vector2(dashTemplateX.anchoredPosition.x, graphHeight / Line.y * i);     //####
        }
    }
    private GameObject CreateCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(CircleContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        gameObject.name = "Rabbit" + (Mathf.Round((int)Time.time)).ToString();
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(5, 5);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    private GameObject CreateCircle1(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(CircleContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        gameObject.GetComponent<Image>().color = Color.red;
        gameObject.name = "Fox" + (Mathf.Round((int)Time.time)).ToString();
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(5, 5);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    //if we need to modify the line number, just leave this here,reason in the line 6
    private void DecreaseY()
    {
        RectTransform[] AllGameObject = CircleContainer.GetComponentsInChildren<RectTransform>();
        foreach (RectTransform Child in AllGameObject)
        {
             Child.anchoredPosition = new Vector2(Child.anchoredPosition.x, Child.anchoredPosition.y / (yMaximum /InyMaximum));
        }
    }

    private void DecreaseX()
    {
        RectTransform[] AllGameObject = CircleContainer.GetComponentsInChildren<RectTransform>();
        foreach (RectTransform Child in AllGameObject)
        {
            Child.anchoredPosition = new Vector2(Child.anchoredPosition.x / (xMaximum / InxMaximum), Child.anchoredPosition.y);
        }

    }

    private void ShowGraph(float xValue,float yValue,float yValue1)
    {
        float yPosition = (yValue / yMaximum) * graphHeight;
        float xPosition = (xValue / xMaximum) * graphWidth;
        float yPosition1 = (yValue1 / yMaximum) * graphWidth;
        CreateCircle(new Vector2(xPosition, yPosition));
        CreateCircle1(new Vector2(xPosition, yPosition1));
    }

    private void ShowGraphList(int value)
    {
        DestoryPoint();
        if (value <= 300 && value >= 60)
        {
            int number;
            for (int i = 1; i <= value; i++)
            {
                float yPosition = (GraphRabbitList[GraphRabbitList.Count - value + i - 1] / yMaximum) * graphHeight;
                float yPosition1 = (GraphFoxList[GraphRabbitList.Count - value + i - 1] / yMaximum) * graphHeight;
                float xPosition = i * graphWidth / value;
                CreateCircle(new Vector2(xPosition, yPosition));
                CreateCircle1(new Vector2(xPosition, yPosition1));
                if (i == (int)Mathf.Round(value / Line.x))
                {
                    number = GraphRabbitList.Count - value + i - 1;
                    UpdataGraphListXAxis(number,value);
                }
            }
        }
    }

    private void ShowAllGraph()
    {
        DestoryPoint();
        for (int i = 1; i <= 100; i++)
        {
            int a = (int)Mathf.Round(GraphRabbitList.Count * i / 100);
            float yPosition = (GraphRabbitList[a-1] / yMaximum) * graphHeight;
            float yPosition1 = (GraphFoxList[a - 1] / yMaximum) * graphHeight;
            float xPosition = i * graphWidth / 100;
            CreateCircle(new Vector2(xPosition, yPosition));
            CreateCircle1(new Vector2(xPosition, yPosition1));
        }
        UpdataXAxis();
    }

    private void DestoryPoint()
    {
        Transform[] AllGameObject = CircleContainer.GetComponentsInChildren<Transform>();
        for (int i=1;i< AllGameObject.Length;i++)
        {
                Destroy(AllGameObject[i].gameObject);
        }
    }
}
