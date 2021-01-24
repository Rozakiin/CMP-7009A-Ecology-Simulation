using System;
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
        [SerializeField] private Button save;
        [SerializeField] private Button submit;
        [SerializeField] private Sprite circleSprite;
        [SerializeField] private InputField inputField;
        [SerializeField] private UITimeControl uITimeControl;
        [SerializeField] private SimulationManager simulationManager;
        
        private int input = 100;
        private int rabbitNumber;
        private int foxNumber;
        private int grassNumber;
        private readonly int plotNumber = 100;
        private readonly int yMaximumMultiplier = 2;
        
        private float nextTime;
        private float yMaximum;
        private readonly float xMaximum = 100f;
        private float lastYMaximum;

        private readonly Vector2 line = new Vector2(5, 5); 
        private Vector2 graphContainerSize;

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
        
        private readonly List<int> graphNewRabbitsList = new List<int>();
        private readonly List<int> graphNewFoxesList = new List<int>();
        private readonly List<int> graphNewGrassList = new List<int>();
        private readonly List<float> graphTime = new List<float>();

        private void Awake()
        {
            graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
            graphContainerSize = graphContainer.sizeDelta;

            labelXContainer = graphContainer.Find("LabelXContainer").GetComponent<RectTransform>();
            labelYContainer = graphContainer.Find("LabelYContainer").GetComponent<RectTransform>();
            labelTemplateX = labelXContainer.Find("labelTemplateX").GetComponent<RectTransform>();
            labelTemplateY = labelYContainer.Find("labelTemplateY").GetComponent<RectTransform>();

            dashTemplateX = graphContainer.Find("dashTemplateX").GetComponent<RectTransform>();
            dashTemplateY = graphContainer.Find("dashTemplateY").GetComponent<RectTransform>();
            
            circleContainer = graphContainer.Find("CircleContainer").GetComponent<RectTransform>();

            dashTemplateX.sizeDelta = new Vector2(graphContainerSize.x, 3f);
            dashTemplateY.sizeDelta = new Vector2(graphContainerSize.y, 3f);
        }
        private void Start()
        {
            rabbitNumber = SimulationManager.Instance.numberOfRabbitsToSpawn;
            foxNumber = SimulationManager.Instance.numberOfFoxesToSpawn;
            grassNumber = SimulationManager.Instance.numberOfGrassToSpawn;

            yMaximum = Mathf.Max(rabbitNumber, foxNumber, grassNumber) * yMaximumMultiplier;
            
            // create line in awake based on vector2 line
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

            rabbitNumber = simulationManager.RabbitPopulation();
            foxNumber = simulationManager.FoxPopulation();
            grassNumber = simulationManager.GrassPopulation();
            
            //this four new list will update every frame, the purpose of this four list is to keep to CSV file to research
            //because the data of update every frame is more precisely.
            graphNewRabbitsList.Add(rabbitNumber);
            graphNewFoxesList.Add(foxNumber);
            graphNewGrassList.Add(grassNumber);
            graphTime.Add(Time.timeSinceLevelLoad);
            
            //update every second
            if ((int) Time.timeSinceLevelLoad > graphRabbitsList.Count)
            {
                var numberOfNew = (int) Time.timeSinceLevelLoad - graphRabbitsList.Count;
                //check the period of one frame is less than a internal seconds
                if (numberOfNew>1)
                {
                    // for example three internal seconds per frame, so just add three number in this frame
                    //It is a kind of data interpolation, for example, last frame is 12th second(100 rabbit)
                    //this frame is 15th second(200 rabbits), so graphRabbitList will like this [100,125,150,175,200].
                    // here will add 125 150 175 to graphRabbitList, add 200 in line 133, reason is to make sure record data every internal seconds
                    var rabbit = (rabbitNumber - graphRabbitsList.Last()) / numberOfNew;
                    var fox = (foxNumber - graphFoxesList.Last()) / numberOfNew;
                    var grass = (grassNumber - graphGrassList.Last()) / numberOfNew;
                    for (var i = 1; i < numberOfNew; i++)
                    {
                        graphRabbitsList.Add(graphRabbitsList.Last()+Mathf.RoundToInt(rabbit*i));
                        graphFoxesList.Add(graphFoxesList.Last()+Mathf.RoundToInt(fox*i));
                        graphGrassList.Add(graphGrassList.Last()+Mathf.RoundToInt(grass*i));
                    }
                }
                
                // add last one to graphList or, in just add one number every second in normal situation
                graphRabbitsList.Add(rabbitNumber);
                graphFoxesList.Add(foxNumber);
                graphGrassList.Add(grassNumber);
                int graphLength = graphRabbitsList.Count;
                
                // Y Value can't bigger than 80 percent of YMaximum Value or YMaximum Value will increase as Y Value increase
                if (Mathf.Max(rabbitNumber, foxNumber, grassNumber) / 0.8f > yMaximum)
                {
                    lastYMaximum = yMaximum;
                    yMaximum = Mathf.Max(rabbitNumber, foxNumber, grassNumber) / 0.8f;
                    UpdateYAxis();
                    DecreaseY();
                }
                
                // every second plot one dot
                if (graphRabbitsList.Count <= plotNumber)
                {
                    ShowGraph(graphLength,numberOfNew);
                }
                else if (graphRabbitsList.Count > plotNumber)
                {
                    if (input >= plotNumber) // when user input is 200, show a trend of last 200 seconds,
                    {
                        //change existing dots position
                        UpdateDotsPositionByInput(input,graphLength);
                    }
                    else // show overall graph from 0s to now
                    {
                        // update rate increase as time goes by
                        if (Time.timeSinceLevelLoad >= nextTime)
                        {
                            UpdateAllDotsPosition(graphLength);
                            
                            //updateSecond must be 2s, 3s, 4s, increase by graphRabbitsList.Count
                            var updateSecond = Mathf.FloorToInt(graphRabbitsList.Count / plotNumber);
                            nextTime += updateSecond;
                        }
                    }
                }
            }
        }

        private void ShowTime()
        {
            int lastInput = int.Parse(inputField.text);
            nextTime = Time.timeSinceLevelLoad;
            
            if (graphRabbitsList.Count <= plotNumber) return;
            // if lastInput(user input) more than graphRabbitsList.Count, let input = 1 that show overall graph
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
                    for (var i = 0; i< graphTime.Count;i++)
                    {
                        // save every frame of data and time more precisely
                        sw.WriteLine(graphTime[i] + "," + graphNewRabbitsList[i] + "," + graphNewFoxesList[i] + "," + graphNewGrassList[i]);
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
                labelX.anchoredPosition = new Vector2(graphContainerSize.x / lineNumber.x * i * 1f, -7f);
                labelX.GetComponent<Text>().text = xText.ToString();
                labelX.name = i.ToString();

                RectTransform dashY = Instantiate(dashTemplateY, graphContainer, false);
                dashY.gameObject.SetActive(true);
                dashY.anchoredPosition = new Vector2(graphContainerSize.x / lineNumber.x * i * 1f, dashTemplateY.anchoredPosition.y);
            }
            
            for (var i = 1; i <= lineNumber.y; i++)
            {
                var yText = (int)(yMaximum / lineNumber.y * i);
                RectTransform labelY = Instantiate(labelTemplateY, labelYContainer, false);
                labelY.gameObject.SetActive(true);
                labelY.anchoredPosition = new Vector2(-7f, graphContainerSize.y / lineNumber.y * i);
                labelY.GetComponent<Text>().text = yText.ToString();
                labelY.name = i.ToString();

                RectTransform dashX = Instantiate(dashTemplateX, graphContainer, false);
                dashX.gameObject.SetActive(true);
                dashX.anchoredPosition = new Vector2(dashTemplateX.anchoredPosition.x, graphContainerSize.y / lineNumber.y * i);
            }
        }

        private void CreateDots(Vector2 anchoredPosition, string objectName)
        {
            GameObject gameObject = new GameObject("circle", typeof(Image));
            gameObject.transform.SetParent(circleContainer, false);
            gameObject.GetComponent<Image>().sprite = circleSprite;
            if (objectName != "Rabbit")
            {
                //Fox is red color dots Grass is green color, rabbit is white(default)
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
                anchoredPosition = new Vector2(anchoredPosition.x, anchoredPosition.y / (yMaximum / lastYMaximum));
                child.anchoredPosition = anchoredPosition;
            }
        }

        private void ShowGraph(int listLength,int addNumber)
        {
            for (var i = addNumber; i >= 1; i--)
            {
                float xPosition = (listLength-i+1) * graphContainerSize.x / 100;
                float yPosition = (graphRabbitsList[listLength-i] / yMaximum) * graphContainerSize.y;
                float yPosition1 = (graphFoxesList[listLength-i] / yMaximum) * graphContainerSize.y;
                float yPosition2 = (graphGrassList[listLength-i] / yMaximum) * graphContainerSize.y;
                CreateDots(new Vector2(xPosition, yPosition), "Rabbit");
                CreateDots(new Vector2(xPosition, yPosition1), "Fox");
                CreateDots(new Vector2(xPosition, yPosition2), "Grass");
            }
        }

        private void UpdateDotsPositionByInput(int value, int listLength)
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
                    int a = Mathf.RoundToInt(value * iR / plotNumber);
                    float yPosition = (graphRabbitsList[listLength - value + a - 1] / yMaximum) * graphContainerSize.y;
                    float xPosition = iR * graphContainerSize.x / plotNumber;

                    child.anchoredPosition = new Vector2(xPosition, yPosition);
                    iR++;
                }
                else if (child.gameObject.name.Contains("Fox"))
                {
                    int a = Mathf.RoundToInt(value * iF / plotNumber);
                    float yPosition = (graphFoxesList[listLength - value + a - 1] / yMaximum) * graphContainerSize.y;
                    float xPosition = iF * graphContainerSize.x / plotNumber;

                    child.anchoredPosition = new Vector2(xPosition, yPosition);
                    iF++;
                }
                else if (child.gameObject.name.Contains("Grass"))
                {
                    int a = Mathf.RoundToInt(value * iG / plotNumber);
                    float yPosition = (graphGrassList[listLength - value + a - 1] / yMaximum) * graphContainerSize.y;
                    float xPosition = iG * graphContainerSize.x / plotNumber;

                    child.anchoredPosition = new Vector2(xPosition, yPosition);
                    iG++;
                }
            }

            var number = Mathf.RoundToInt(listLength - value / line.x * (line.x-1));
            UpdateXAxis(number, value);
        }

        private void UpdateAllDotsPosition(int listLength)
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
                    int a = Mathf.RoundToInt(listLength * iR / plotNumber);
                    float yPosition = (graphRabbitsList[a - 1] / yMaximum) * graphContainerSize.y;
                    float xPosition = iR * graphContainerSize.x / plotNumber;

                    child.anchoredPosition = new Vector2(xPosition, yPosition);
                    iR++;
                }
                else if (child.gameObject.name.Contains("Fox"))
                {
                    int a = Mathf.RoundToInt(listLength * iF / plotNumber);
                    float yPosition = (graphFoxesList[a - 1] / yMaximum) * graphContainerSize.y;
                    float xPosition = iF * graphContainerSize.x / plotNumber;

                    child.anchoredPosition = new Vector2(xPosition, yPosition);
                    iF++;
                }
                else if (child.gameObject.name.Contains("Grass"))
                {
                    int a = Mathf.RoundToInt(listLength * iG / plotNumber);
                    float yPosition = (graphGrassList[a - 1] / yMaximum) * graphContainerSize.y;
                    float xPosition = iG * graphContainerSize.x / plotNumber;

                    child.anchoredPosition = new Vector2(xPosition, yPosition);
                    iG++;
                }
            }
            
            var number = Mathf.RoundToInt(listLength / line.x);
            UpdateXAxis(number, listLength);
        }
    }
}
