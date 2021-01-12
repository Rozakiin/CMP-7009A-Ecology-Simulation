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
    private float GrassNumber;
    private float yMaximum;
    private float xMaximum;
    private float InyMaximum;
    private float InxMaximum;
    private float graphHeight;
    private float graphWidth;
    private float parentWidth;
    private float parentHeight;
    private int input;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform LabelXContainer;
    private RectTransform LabelYContainer;
    private RectTransform labelTemplateY;
    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;
    private RectTransform CircleContainer;
    private readonly List<float> GraphRabbitsList = new List<float> ();
    private readonly List<float> GraphFoxesList = new List<float>();
    private readonly List<float> GraphGrassList = new List<float>();

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
        GrassNumber = simulationManager.GrassSpawn();

        InyMaximum = Mathf.Max(RabbitNumber, FoxNumber, GrassNumber) * 5;
        yMaximum = Mathf.Max(RabbitNumber, FoxNumber, GrassNumber) * 5;
        xMaximum = 100f;
        InxMaximum = 100f;
        nextTime = 1;
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
            GraphRabbitsList.Add(RabbitNumber);
            GraphFoxesList.Add(FoxNumber);
            GraphGrassList.Add(GrassNumber);

            RabbitNumber = simulationManager.RabbitPopulation();
            FoxNumber = simulationManager.FoxPopulation();
            GrassNumber = simulationManager.GrassPopulation();

            XPos = Time.time;   
            nextTime += 1;

            if (Mathf.Max(RabbitNumber, FoxNumber, GrassNumber) / 8 * 10 > yMaximum) 
            {
                InyMaximum = yMaximum;
                yMaximum = Mathf.Max(RabbitNumber, FoxNumber, GrassNumber) / 8 * 10;
                UpdataYAxis();
                DecreaseY();
            }

            if (GraphRabbitsList.Count <= 100)
            {
                ShowGraph(XPos, RabbitNumber, FoxNumber, GrassNumber);
            }
            else if (GraphRabbitsList.Count > 100)
            {
                if (input >= 100)
                {
                    ShowGraphList(input);
                }
                else
                {
                    if (Time.time >= nextTime2)
                    {
                        ShowAllGraph();

                        int a = (int)Mathf.Floor(GraphRabbitsList.Count / 100);
                        nextTime2 += a;
                    }
                }
            }
        }
    }

    void ShowTime()
    {
        int lastInput = int.Parse(inputField.text);
        nextTime2 = Time.time;

        // only GraphRabbitList.Count must high than 100, input will work
        if (GraphRabbitsList.Count > 100)
        {
            if (lastInput < GraphRabbitsList.Count)
            {
                input = lastInput;
            }
            else
            {
                input = 1;
            }
        }
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
                sw.WriteLine("Seconds"+","+"Rabbit"+","+"Fox"+","+"Grass");
                for (int i = 0; i < GraphRabbitsList.Count; i++) 
                {
                    sw.WriteLine((i+1)+","+GraphRabbitsList[i]+","+ GraphFoxesList[i]+","+ GraphGrassList[i]);
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
                int XText = (int)(GraphRabbitsList.Count / Line.x * labelNumber);
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
            labelY.anchoredPosition = new Vector2(-7f, graphHeight / Line.y * i );   
            labelY.GetComponent<Text>().text = YText.ToString();
            labelY.name = i.ToString();

            RectTransform DashX = Instantiate(dashTemplateX);
            DashX.SetParent(graphContainer, false);
            DashX.gameObject.SetActive(true);
            DashX.anchoredPosition = new Vector2(dashTemplateX.anchoredPosition.x, graphHeight / Line.y * i);     
        }
    }

    private GameObject CreateRabbit(Vector2 anchoredPosition)
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

    private GameObject CreateFox(Vector2 anchoredPosition)
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

    private GameObject CreateGrass(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(CircleContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        gameObject.GetComponent<Image>().color = Color.green;
        gameObject.name = "Grass" + (Mathf.Round((int)Time.time)).ToString();
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(5, 5);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

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

    private void ShowGraph(float xValue,float yValue,float yValue1, float yValue2)
    {
        float xPosition = (xValue / xMaximum) * graphWidth;
        float yPosition = (yValue / yMaximum) * graphHeight;
        float yPosition1 = (yValue1 / yMaximum) * graphWidth;
        float yPosition2 = (yValue2 / yMaximum) * graphWidth;
        CreateRabbit(new Vector2(xPosition, yPosition));
        CreateFox(new Vector2(xPosition, yPosition1));
        CreateGrass(new Vector2(xPosition, yPosition2));
    }

    private void ShowGraphList(int value)
    {
        DestoryPoint();
        if (value <= 100 && value >= 60)
        {
            int number;
            for (int i = 1; i <= value; i++)
            {
                float yPosition = (GraphRabbitsList[GraphRabbitsList.Count - value + i - 1] / yMaximum) * graphHeight;
                float yPosition1 = (GraphFoxesList[GraphRabbitsList.Count - value + i - 1] / yMaximum) * graphHeight;
                float yPosition2 = (GraphGrassList[GraphRabbitsList.Count - value + i - 1] / yMaximum) * graphHeight;
                float xPosition = i * graphWidth / value;
                CreateRabbit(new Vector2(xPosition, yPosition));
                CreateFox(new Vector2(xPosition, yPosition1));
                CreateGrass(new Vector2(xPosition, yPosition2));
                if (i == (int)Mathf.Round(value / Line.x))
                {
                    number = GraphRabbitsList.Count - value + i - 1;
                    UpdataGraphListXAxis(number, value);
                }
            }
        }
        else if (value > 100) 
        {
            int number;
            for (int i = 1; i <= 100; i++)
            {
                int a = (int)Mathf.Round(value * i / 100);
                float yPosition = (GraphRabbitsList[GraphRabbitsList.Count - value + a-1] / yMaximum) * graphHeight;
                float yPosition1 = (GraphFoxesList[GraphRabbitsList.Count - value + a-1] / yMaximum) * graphHeight;
                float yPosition2 = (GraphGrassList[GraphRabbitsList.Count - value + a - 1] / yMaximum) * graphHeight;
                float xPosition = i * graphWidth / 100;
                CreateRabbit(new Vector2(xPosition, yPosition));
                CreateFox(new Vector2(xPosition, yPosition1));
                CreateGrass(new Vector2(xPosition, yPosition2));
            }
            number = (int)GraphRabbitsList.Count - (value / 5 * 4);
            UpdataGraphListXAxis(number, value);
        }
    }

    private void ShowAllGraph()
    {
        DestoryPoint();
        for (int i = 1; i <= 100; i++)
        {
            int a = (int)Mathf.Round(GraphRabbitsList.Count * i / 100);
            float yPosition = (GraphRabbitsList[a-1] / yMaximum) * graphHeight;
            float yPosition1 = (GraphFoxesList[a - 1] / yMaximum) * graphHeight;
            float yPosition2 = (GraphGrassList[a - 1] / yMaximum) * graphHeight;
            float xPosition = i * graphWidth / 100;
            CreateRabbit(new Vector2(xPosition, yPosition));
            CreateFox(new Vector2(xPosition, yPosition1));
            CreateGrass(new Vector2(xPosition, yPosition2));
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
