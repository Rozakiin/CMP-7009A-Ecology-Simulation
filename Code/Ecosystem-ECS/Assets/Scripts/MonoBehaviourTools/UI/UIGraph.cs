﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

//maybe we need these function later on, this function can modify the number of red line in the graph
//now only apply in 5 line in X aixs and 5 line in y axis not sure need to update,discuss monday
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
        private int rabbitNumber;
        private int foxNumber;
        private int grassNumber;
        
        private float nextTime;
        private float yMaximum;
        private float xMaximum;
        private float inyMaximum;
        private float graphHeight;
        private float graphWidth;

        private RectTransform graphContainer;
        private RectTransform labelTemplateX;
        private RectTransform labelXContainer;
        private RectTransform labelYContainer;
        private RectTransform labelTemplateY;
        private RectTransform dashTemplateX;
        private RectTransform dashTemplateY;
        private RectTransform circleContainer;

        private readonly List<int> graphRabbitsList = new List<int>();
        private readonly List<int> graphFoxesList = new List<int>();
        private readonly List<int> graphGrassList = new List<int>();
        private readonly List<float> graphTime = new List<float>();

        private void Awake()
        {
            graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
            var sizeDelta = graphContainer.sizeDelta;
            graphHeight = sizeDelta.y;
            graphWidth = sizeDelta.x;

            labelXContainer = graphContainer.Find("LabelXContainer").GetComponent<RectTransform>();
            labelYContainer = graphContainer.Find("LabelYContainer").GetComponent<RectTransform>();
            labelTemplateX = labelXContainer.Find("labelTemplateX").GetComponent<RectTransform>();
            labelTemplateY = labelYContainer.Find("labelTemplateY").GetComponent<RectTransform>();

            dashTemplateX = graphContainer.Find("dashTemplateX").GetComponent<RectTransform>();
            dashTemplateY = graphContainer.Find("dashTemplateY").GetComponent<RectTransform>();
            
            circleContainer = graphContainer.Find("CircleContainer").GetComponent<RectTransform>();

            dashTemplateX.sizeDelta = new Vector2(graphWidth, 3f);
            dashTemplateY.sizeDelta = new Vector2(graphHeight, 3f);
        }
        private void Start()
        {
            rabbitNumber = simulationManager.RabbitSpawn();
            foxNumber = simulationManager.FoxSpawn();
            grassNumber = simulationManager.GrassSpawn();
            
            graphRabbitsList.Add(rabbitNumber);
            graphFoxesList.Add(foxNumber);
            graphGrassList.Add(grassNumber);
            graphTime.Add(1);
            ShowGraph(1);
            
            inyMaximum = Mathf.Max(rabbitNumber, foxNumber, grassNumber) * 5;
            yMaximum = Mathf.Max(rabbitNumber, foxNumber, grassNumber) * 5;
            xMaximum = 100f;
            input = 100;

            // create line in awake based on vector2 Line input from inspector
            Create(line);
            submit.onClick.AddListener(ShowTime);
            save.onClick.AddListener(SaveFile);
        }
        private void Update()
        {
            //catch to not run if paused
            if (UITimeControl.instance.GetPause())
            {
                return;
            }
            
            rabbitNumber = simulationManager.RabbitPopulation();
            foxNumber = simulationManager.FoxPopulation();
            grassNumber = simulationManager.GrassPopulation();

            // add last one to graphList or, in just add one number every second in normal situation
            graphRabbitsList.Add(rabbitNumber);
            graphFoxesList.Add(foxNumber);
            graphGrassList.Add(grassNumber);
            graphTime.Add(Time.timeSinceLevelLoad);
            
            int graphLength = graphRabbitsList.Count;
            if (Mathf.Max(rabbitNumber, foxNumber, grassNumber) / 8 * 10 > yMaximum)
            {
                inyMaximum = yMaximum;
                yMaximum = Mathf.Max(rabbitNumber, foxNumber, grassNumber) / 8 * 10;
                UpdateYAxis();
                DecreaseY();
            }
            
            if (graphRabbitsList.Count <= 100)
            {
                ShowGraph(graphLength);
            }
            else if (graphRabbitsList.Count > 100)
            {
                if (input >= 100)
                {
                    ShowInputGraph(input,graphLength);
                }
                else
                {
                    if (Time.timeSinceLevelLoad >= nextTime)
                    {
                        ShowAllGraph(graphLength);

                        int updateSecond = Mathf.FloorToInt(graphRabbitsList.Count / 100);
                        nextTime += updateSecond;
                    }
                }
            }
            
        }

        private void ShowTime()
        {
            int lastInput = int.Parse(inputField.text);
            nextTime = Time.timeSinceLevelLoad;

            // only GraphRabbitList.Count must high than 100, input will work
            if (graphRabbitsList.Count <= 100) return;
            input = lastInput < graphRabbitsList.Count ? lastInput : 1;
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
                    for (var i = 0; i < graphTime.Count; i++) 
                    {
                        sw.WriteLine(graphTime[i] + "," + graphRabbitsList[i] + "," + graphFoxesList[i] + "," + graphGrassList[i]);
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

        private void UpdateYAxis()
        {
            Transform[] allGameObject = labelYContainer.GetComponentsInChildren<Transform>();
            foreach (Transform child in allGameObject)
            {
                if (child.name.Contains("Label"))
                {
                    continue;
                }
                int labelNumber = int.Parse(child.name);
                int labelText = (int)(yMaximum / line.y * labelNumber);
                child.GetComponent<Text>().text = labelText.ToString();
            }
        }

        private void UpdateXAxis(int number, int value)
        {
            Transform[] allGameObject = labelXContainer.GetComponentsInChildren<Transform>();
            foreach (Transform child in allGameObject)
            {
                if (child.name.Contains("Label"))
                {
                    continue;
                }

                int labelNumber = int.Parse(child.name);
                int labelText = (labelNumber - 1) * (value / (int)line.x) + number;
                child.GetComponent<Text>().text = labelText.ToString();
            }
        }

        private void Create(Vector2 lineNumber)
        {
            for (var i = 1; i <= lineNumber.x; i++)
            {
                var xText = (int)(xMaximum / lineNumber.x * i);
                RectTransform labelX = Instantiate(labelTemplateX, labelXContainer, false);
                labelX.gameObject.SetActive(true);
                labelX.anchoredPosition = new Vector2(graphWidth / lineNumber.x * i * 1f, -7f);
                labelX.GetComponent<Text>().text = xText.ToString();
                labelX.name = i.ToString();

                RectTransform dashY = Instantiate(dashTemplateY, graphContainer, false);
                dashY.gameObject.SetActive(true);
                dashY.anchoredPosition = new Vector2(graphWidth / lineNumber.x * i * 1f, dashTemplateY.anchoredPosition.y);
            }


            for (var i = 1; i <= lineNumber.y; i++)
            {
                var yText = (int)(yMaximum / lineNumber.y * i);
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

        private void CreateDots(Vector2 anchoredPosition, string objectName)
        {
            GameObject gameObject = new GameObject("circle", typeof(Image));
            gameObject.transform.SetParent(circleContainer, false);
            gameObject.GetComponent<Image>().sprite = circleSprite;
            if (objectName != "Rabbit")
            {
                gameObject.GetComponent<Image>().color = (objectName == "Fox") ? Color.red : Color.green;
            }
            gameObject.name = objectName + graphRabbitsList.Count;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = new Vector2(5, 5);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
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

        private void ShowGraph(int listLength)
        {
            float xPosition = (listLength) * graphWidth / 100;
            float yPosition = (graphRabbitsList[listLength-1] / yMaximum) * graphHeight;
            float yPosition1 = (graphFoxesList[listLength-1] / yMaximum) * graphHeight;
            float yPosition2 = (graphGrassList[listLength-1] / yMaximum) * graphHeight;
            CreateDots(new Vector2(xPosition, yPosition), "Rabbit");
            CreateDots(new Vector2(xPosition, yPosition1), "Fox");
            CreateDots(new Vector2(xPosition, yPosition2), "Grass");
        }

        private void ShowInputGraph(int value, int listLength)
        {
            // Dont understand the logic behind the position calculation but objects are moved now rather than deleted and recreated
            RectTransform[] allGameObject = circleContainer.GetComponentsInChildren<RectTransform>();
            int iR = 1;
            int iF = 1;
            int iG = 1;

            foreach (RectTransform child in allGameObject)
            {
                if (child.gameObject.name.Contains("Rabbit"))
                {
                    int a = Mathf.RoundToInt(value * iR / 100);
                    float yPosition = (graphRabbitsList[listLength - value + a - 1] / yMaximum) * graphHeight;
                    float xPosition = iR * graphWidth / 100;

                    child.anchoredPosition = new Vector2(xPosition, yPosition);
                    iR++;
                }
                else if (child.gameObject.name.Contains("Fox"))
                {
                    int a = Mathf.RoundToInt(value * iF / 100);
                    float yPosition1 = (graphFoxesList[listLength - value + a - 1] / yMaximum) * graphHeight;
                    float xPosition = iF * graphWidth / 100;

                    child.anchoredPosition = new Vector2(xPosition, yPosition1);
                    iF++;
                }
                else if (child.gameObject.name.Contains("Grass"))
                {
                    int a = Mathf.RoundToInt(value * iG / 100);
                    float yPosition2 = (graphGrassList[listLength - value + a - 1] / yMaximum) * graphHeight;
                    float xPosition = iG * graphWidth / 100;

                    child.anchoredPosition = new Vector2(xPosition, yPosition2);
                    iG++;
                }
            }

            var number = listLength - (value / 5 * 4);
            UpdateXAxis(number, value);
        }

        private void ShowAllGraph(int listLength)
        {
            // Dont understand the logic behind the position calculation but objects are moved now rather than deleted and recreated
            RectTransform[] allGameObject = circleContainer.GetComponentsInChildren<RectTransform>();
            int iR = 1;
            int iF = 1;
            int iG = 1;

            foreach (RectTransform child in allGameObject)
            {
                if (child.gameObject.name.Contains("Rabbit"))
                {
                    int a = Mathf.RoundToInt(listLength * iR / 100);
                    float yPosition = (graphRabbitsList[a - 1] / yMaximum) * graphHeight;
                    float xPosition = iR * graphWidth / 100;

                    child.anchoredPosition = new Vector2(xPosition, yPosition);
                    iR++;
                }
                else if (child.gameObject.name.Contains("Fox"))
                {
                    int a = Mathf.RoundToInt(listLength * iF / 100);
                    float yPosition1 = (graphFoxesList[a - 1] / yMaximum) * graphHeight;
                    float xPosition = iF * graphWidth / 100;

                    child.anchoredPosition = new Vector2(xPosition, yPosition1);
                    iF++;
                }
                else if (child.gameObject.name.Contains("Grass"))
                {
                    int a = Mathf.RoundToInt(listLength * iG / 100);
                    float yPosition2 = (graphGrassList[a - 1] / yMaximum) * graphHeight;
                    float xPosition = iG * graphWidth / 100;

                    child.anchoredPosition = new Vector2(xPosition, yPosition2);
                    iG++;
                }
            }
            
            var number = listLength / 5;
            UpdateXAxis(number, listLength);
        }
    }
}
