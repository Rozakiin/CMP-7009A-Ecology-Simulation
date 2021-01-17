using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

//maybe we need these function later on, this function can modify the number of red line in the graph
//now only apply in 5 line in X aixs and 5 line in y axis not sure need to updata,discuss monday
namespace MonoBehaviourTools.UI
{
    public class UIGraph : MonoBehaviour
    {
        [SerializeField] private SimulationManager simulationManager;
        [SerializeField] private Sprite circleSprite;
        [SerializeField] private Vector2 line = new Vector2(5, 5);
        [SerializeField] private InputField inputField;
        [SerializeField] private Button submit;
        [SerializeField] private Button save;
        [SerializeField] private UITimeControl uITimeControl;

        private int input;
        private float nextTime;
        private float nextTime2;
        private float xPos;
        private float rabbitNumber;
        private float foxNumber;
        private float grassNumber;
        private float yMaximum;
        private float xMaximum;
        private float inyMaximum;
        private float graphHeight;
        private float graphWidth;
        private float parentWidth;
        private float parentHeight;

        private RectTransform graphContainer;
        private RectTransform labelTemplateX;
        private RectTransform labelXContainer;
        private RectTransform labelYContainer;
        private RectTransform labelTemplateY;
        private RectTransform dashTemplateX;
        private RectTransform dashTemplateY;
        private RectTransform circleContainer;

        private readonly List<float> graphRabbitsList = new List<float>();
        private readonly List<float> graphFoxesList = new List<float>();
        private readonly List<float> graphGrassList = new List<float>();

        private void Awake()
        {
            graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
            var rect = graphContainer.rect;
            parentWidth = rect.width;
            parentHeight = rect.height;

            labelXContainer = graphContainer.Find("LabelXContainer").GetComponent<RectTransform>();
            labelYContainer = graphContainer.Find("LabelYContainer").GetComponent<RectTransform>();
            labelTemplateX = labelXContainer.Find("labelTemplateX").GetComponent<RectTransform>();
            labelTemplateY = labelYContainer.Find("labelTemplateY").GetComponent<RectTransform>();

            dashTemplateX = graphContainer.Find("dashTemplateX").GetComponent<RectTransform>();
            dashTemplateY = graphContainer.Find("dashTemplateY").GetComponent<RectTransform>();

            circleContainer = graphContainer.Find("CircleContainer").GetComponent<RectTransform>();

            dashTemplateX.sizeDelta = new Vector2(parentWidth, 3f);
            dashTemplateY.sizeDelta = new Vector2(parentHeight, 3f);
        }
        private void Start()
        {
            rabbitNumber = simulationManager.RabbitSpawn();
            foxNumber = simulationManager.FoxSpawn();
            grassNumber = simulationManager.GrassSpawn();

            inyMaximum = Mathf.Max(rabbitNumber, foxNumber, grassNumber) * 5;
            yMaximum = Mathf.Max(rabbitNumber, foxNumber, grassNumber) * 5;
            xMaximum = 100f;
            nextTime = 1;
            input = 100;

            var sizeDelta = graphContainer.sizeDelta;
            graphHeight = sizeDelta.y;
            graphWidth = sizeDelta.x;

            // create line in awake based on vector2 Line input from inspector
            Create(line);
            submit.onClick.AddListener(ShowTime);
            save.onClick.AddListener(SaveFile);
        }
        private void Update()
        {
            //catch to not run if paused
            if (UITimeControl.Instance.GetPause())
            {
                return;
            }
            if (Time.timeSinceLevelLoad >= nextTime)
            {
                graphRabbitsList.Add(rabbitNumber);
                graphFoxesList.Add(foxNumber);
                graphGrassList.Add(grassNumber);

                rabbitNumber = simulationManager.RabbitPopulation();
                foxNumber = simulationManager.FoxPopulation();
                grassNumber = simulationManager.GrassPopulation();

                xPos = Time.timeSinceLevelLoad;
                nextTime += 1;

                if (Mathf.Max(rabbitNumber, foxNumber, grassNumber) / 8 * 10 > yMaximum)
                {
                    inyMaximum = yMaximum;
                    yMaximum = Mathf.Max(rabbitNumber, foxNumber, grassNumber) / 8 * 10;
                    UpdateLabel("Y");
                    DecreaseY();
                }

                if (graphRabbitsList.Count <= 100)
                {
                    ShowGraph(xPos, rabbitNumber, foxNumber, grassNumber);
                }
                else if (graphRabbitsList.Count > 100)
                {
                    if (input >= 100)
                    {
                        ShowGraphList(input);
                    }
                    else
                    {
                        if (Time.timeSinceLevelLoad >= nextTime2)
                        {
                            ShowAllGraph();

                            int a = (int)Mathf.Floor(graphRabbitsList.Count / 100);
                            nextTime2 += a;
                        }
                    }
                }
            }
        }

        private void ShowTime()
        {
            int lastInput = int.Parse(inputField.text);
            nextTime2 = Time.timeSinceLevelLoad;

            // only GraphRabbitList.Count must high than 100, input will work
            if (graphRabbitsList.Count > 100)
            {
                if (lastInput < graphRabbitsList.Count)
                {
                    input = lastInput;
                }
                else
                {
                    input = 1;
                }
            }
        }

        private void SaveFile()
        {
            uITimeControl.Pause();

            string path = StandaloneFileBrowser.StandaloneFileBrowser.SaveFilePanel("Save File", "", "", "csv");
            try
            {
                // Create a new file     
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("Seconds" + "," + "Rabbit" + "," + "Fox" + "," + "Grass");
                    for (var i = 0; i < graphRabbitsList.Count; i++)
                    {
                        sw.WriteLine((i + 1) + "," + graphRabbitsList[i] + "," + graphFoxesList[i] + "," + graphGrassList[i]);
                    }
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            uITimeControl.Play();
        }

        private void UpdateLabel(string labelName)
        {
            Transform[] allGameObject = (labelName == "X")
                ? labelXContainer.GetComponentsInChildren<Transform>()
                : labelYContainer.GetComponentsInChildren<Transform>();
            foreach (Transform child in allGameObject)
            {
                if (child.name.Contains("Label"))
                {
                    continue;
                }
                int labelNumber = int.Parse(child.name);
                int labelText = (labelName == "X")
                    ? (int)(graphRabbitsList.Count / line.x * labelNumber)
                    : (int)(yMaximum / line.y * labelNumber);
                child.GetComponent<Text>().text = labelText.ToString();


            }
        }

        private void UpdateGraphListXAxis(int number, int value)
        {
            Transform[] allGameObject = labelXContainer.GetComponentsInChildren<Transform>();
            foreach (Transform child in allGameObject)
            {
                if (child.name.Contains("Label"))
                {
                    continue;
                }

                int labelNumber = int.Parse(child.name.ToString());
                int labelText = (labelNumber - 1) * (value / (int)line.x) + number;
                child.GetComponent<Text>().text = labelText.ToString();
            }
        }

        private void Create(Vector2 lineNumber)
        {

            for (int i = 1; i <= lineNumber.x; i++)
            {

                int xText = (int)(xMaximum / lineNumber.x * i);

                RectTransform labelX = Instantiate(labelTemplateX, labelXContainer, false);
                labelX.gameObject.SetActive(true);
                labelX.anchoredPosition = new Vector2(graphWidth / lineNumber.x * i * 1f, -7f);
                labelX.GetComponent<Text>().text = xText.ToString();
                labelX.name = i.ToString();

                RectTransform dashY = Instantiate(dashTemplateY, graphContainer, false);
                dashY.gameObject.SetActive(true);
                dashY.anchoredPosition = new Vector2(graphWidth / lineNumber.x * i * 1f, dashTemplateY.anchoredPosition.y);
            }


            for (int i = 1; i <= lineNumber.y; i++)
            {
                int yText = (int)(yMaximum / lineNumber.y * i);
                RectTransform labelY = Instantiate(labelTemplateY, labelYContainer, false);
                labelY.gameObject.SetActive(true);
                labelY.anchoredPosition = new Vector2(-7f, graphHeight / lineNumber.y * i);
                labelY.GetComponent<Text>().text = yText.ToString();
                labelY.name = i.ToString();

                RectTransform dashX = Instantiate(dashTemplateX, graphContainer, false);
                dashX.gameObject.SetActive(true);
                dashX.anchoredPosition = new Vector2(dashTemplateX.anchoredPosition.x, graphHeight / lineNumber.y * i);
            }
        }

        private GameObject CreateDots(Vector2 anchoredPosition, string objectName)
        {
            GameObject gameObject = new GameObject("circle", typeof(Image));
            gameObject.transform.SetParent(circleContainer, false);
            gameObject.GetComponent<Image>().sprite = circleSprite;
            if (objectName != "Rabbit")
            {
                gameObject.GetComponent<Image>().color = (objectName == "Fox") ? Color.red : Color.green;
            }
            gameObject.name = objectName + (Mathf.Round((int)Time.timeSinceLevelLoad));
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = new Vector2(5, 5);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            return gameObject;
        }

        private void DecreaseY()
        {
            RectTransform[] allGameObject = circleContainer.GetComponentsInChildren<RectTransform>();
            foreach (RectTransform child in allGameObject)
            {
                var anchoredPosition = child.anchoredPosition;
                anchoredPosition = new Vector2(anchoredPosition.x, anchoredPosition.y / (yMaximum / inyMaximum));
                child.anchoredPosition = anchoredPosition;
            }
        }

        private void ShowGraph(float xValue, float yValue, float yValue1, float yValue2)
        {
            float xPosition = (xValue / xMaximum) * graphWidth;
            float yPosition = (yValue / yMaximum) * graphHeight;
            float yPosition1 = (yValue1 / yMaximum) * graphWidth;
            float yPosition2 = (yValue2 / yMaximum) * graphWidth;
            CreateDots(new Vector2(xPosition, yPosition), "Rabbit");
            CreateDots(new Vector2(xPosition, yPosition1), "Fox");
            CreateDots(new Vector2(xPosition, yPosition2), "Grass");
        }

        private void ShowGraphList(int value)
        {
            DestroyPoint();

            for (int i = 1; i <= 100; i++)
            {
                int a = (int)Mathf.Round(value * i / 100);
                float yPosition = (graphRabbitsList[graphRabbitsList.Count - value + a - 1] / yMaximum) * graphHeight;
                float yPosition1 = (graphFoxesList[graphRabbitsList.Count - value + a - 1] / yMaximum) * graphHeight;
                float yPosition2 = (graphGrassList[graphRabbitsList.Count - value + a - 1] / yMaximum) * graphHeight;
                float xPosition = i * graphWidth / 100;
                CreateDots(new Vector2(xPosition, yPosition), "Rabbit");
                CreateDots(new Vector2(xPosition, yPosition1), "Fox");
                CreateDots(new Vector2(xPosition, yPosition2), "Grass");
            }
            var number = (int)graphRabbitsList.Count - (value / 5 * 4);
            UpdateGraphListXAxis(number, value);
        }

        private void ShowAllGraph()
        {
            DestroyPoint();
            for (int i = 1; i <= 100; i++)
            {
                int a = (int)Mathf.Round(graphRabbitsList.Count * i / 100);
                float yPosition = (graphRabbitsList[a - 1] / yMaximum) * graphHeight;
                float yPosition1 = (graphFoxesList[a - 1] / yMaximum) * graphHeight;
                float yPosition2 = (graphGrassList[a - 1] / yMaximum) * graphHeight;
                float xPosition = i * graphWidth / 100;
                CreateDots(new Vector2(xPosition, yPosition), "Rabbit");
                CreateDots(new Vector2(xPosition, yPosition1), "Fox");
                CreateDots(new Vector2(xPosition, yPosition2), "Grass");
            }
            UpdateLabel("X");
        }

        private void DestroyPoint()
        {
            Transform[] allGameObject = circleContainer.GetComponentsInChildren<Transform>();
            for (int i = 1; i < allGameObject.Length; i++)
            {
                Destroy(allGameObject[i].gameObject);
            }
        }

        public int GetGraphListCount()
        {
            return graphRabbitsList.Count;
        }
    }
}
