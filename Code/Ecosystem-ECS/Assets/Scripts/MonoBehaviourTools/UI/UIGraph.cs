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

    [SerializeField] InputField inputField;
    [SerializeField] Button Submit;


    private float nextTime;
    private float nextTime2;
    private float XPos;
    private float RabbitNumber;
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
    List<float> GraphList = new List<float> ();

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
        InyMaximum = simulationManager.RabbitSpawn() * 5;
        yMaximum = RabbitNumber * 5;
        xMaximum = 50f;
        InxMaximum = 100f;
        nextTime = 0;
        input = 100;

        graphHeight = graphContainer.sizeDelta.y;
        graphWidth = graphContainer.sizeDelta.x;

        // create line in awake based on vector2 Line input from inspector
        Create(Line);
        Submit.onClick.AddListener(ShowTime);
    }
    private void Update()
    {
        if (Time.time >= nextTime)
        {
            RabbitNumber = simulationManager.RabbitPopulation();
            XPos = Time.time;    // X aixs is same with turn number in time counter system 
            GraphList.Add(RabbitNumber);
            nextTime += 1;

            if (input >= 60 && input <= 300)
            {
                if (RabbitNumber / 8 * 10 > yMaximum)
                {
                    InyMaximum = yMaximum;
                    yMaximum = RabbitNumber / 8 * 10;
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
                    ShowGraph(XPos, RabbitNumber);
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

                    if (RabbitNumber / 8 * 10 > yMaximum)
                    {
                        InyMaximum = yMaximum;
                        yMaximum = RabbitNumber / 8 * 10;
                        UpdataYAxis();
                        DecreaseY();
                    }

                    int a = (int)Mathf.Floor(GraphList.Count / 100);

                    nextTime2 += a;

                    print("I am update duration"+a);
                    print("I am total time"+Time.time);

                }
            }
        }
    }

    void ShowTime()
    {
        input = int.Parse(inputField.text);
        nextTime2 = Time.time;
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
                int XText = (int)(GraphList.Count / Line.x * labelNumber);
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
        gameObject.name = "circle" + nextTime.ToString();
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

    private void ShowGraph(float xValue,float yValue)
    {
        float yPosition = (yValue / yMaximum) * graphHeight;
        float xPosition = (xValue / xMaximum) * graphWidth;
        CreateCircle(new Vector2(xPosition, yPosition));
    }

    private void ShowGraphList(int value)
    {
        DestoryPoint();
        if (value <= 300 && value >= 60)
        {
            int number;
            for (int i = 1; i <= value; i++)
            {
                float yPosition = (GraphList[GraphList.Count - value + i - 1] / yMaximum) * graphHeight;
                float xPosition = i * graphWidth / value;
                CreateCircle(new Vector2(xPosition, yPosition));

                if (i == (int)Mathf.Round(value / Line.x))
                {
                    number = GraphList.Count - value + i - 1;
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
            int a = (int)Mathf.Round(GraphList.Count * i / 100);
            float yPosition = (GraphList[a-1] / yMaximum) * graphHeight;
            float xPosition = i * graphWidth / 100;
            CreateCircle(new Vector2(xPosition, yPosition));
        }
        UpdataXAxis();
    }

    private void DestoryPoint()
    {
        Transform[] AllGameObject = graphContainer.GetComponentsInChildren<Transform>();
        foreach (Transform Child in AllGameObject)
        {
            if (Child.name.Contains("circle"))
            {
                Destroy(Child.gameObject);
            }
        }
        print("I am destroy Point"+Time.time);
    }
}
