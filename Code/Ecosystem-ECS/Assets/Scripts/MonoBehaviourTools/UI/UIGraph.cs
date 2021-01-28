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
        [SerializeField] private Button _save;
        [SerializeField] private Button _submit;
        [SerializeField] private Sprite _circleSprite;
        [SerializeField] private InputField _inputField;
        
        private int _input = 100;
        private int _rabbitNumber;
        private int _foxNumber;
        private int _grassNumber;
        private readonly int _plotNumber = 100;
        private readonly int _yMaximumMultiplier = 2;
        
        private float _nextTime;
        private float _yMaximum;
        private readonly float _xMaximum = 100f;
        private float _lastYMaximum;

        private readonly Vector2 _line = new Vector2(5, 5); 
        private Vector2 _graphContainerSize;

        private RectTransform _graphContainer;
        private RectTransform _labelTemplateX;
        private RectTransform _labelXContainer;
        private RectTransform _labelYContainer;
        private RectTransform _labelTemplateY;
        private RectTransform _dashTemplateX;
        private RectTransform _dashTemplateY;
        private RectTransform _circleContainer;
        
        private readonly List<int> _graphRabbitsList = new List<int>();
        private readonly List<int> _graphFoxesList = new List<int>();
        private readonly List<int> _graphGrassList = new List<int>();
        
        private readonly List<int> _graphNewRabbitsList = new List<int>();
        private readonly List<int> _graphNewFoxesList = new List<int>();
        private readonly List<int> _graphNewGrassList = new List<int>();
        private readonly List<float> _graphTime = new List<float>();

        private void Awake()
        {
            _graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
            _graphContainerSize = _graphContainer.sizeDelta;

            _labelXContainer = _graphContainer.Find("LabelXContainer").GetComponent<RectTransform>();
            _labelYContainer = _graphContainer.Find("LabelYContainer").GetComponent<RectTransform>();
            _labelTemplateX = _labelXContainer.Find("labelTemplateX").GetComponent<RectTransform>();
            _labelTemplateY = _labelYContainer.Find("labelTemplateY").GetComponent<RectTransform>();

            _dashTemplateX = _graphContainer.Find("dashTemplateX").GetComponent<RectTransform>();
            _dashTemplateY = _graphContainer.Find("dashTemplateY").GetComponent<RectTransform>();
            
            _circleContainer = _graphContainer.Find("CircleContainer").GetComponent<RectTransform>();

            _dashTemplateX.sizeDelta = new Vector2(_graphContainerSize.x, 3f);
            _dashTemplateY.sizeDelta = new Vector2(_graphContainerSize.y, 3f);
        }
        private void Start()
        {
            _rabbitNumber = SimulationManager.Instance.NumberOfRabbitsToSpawn;
            _foxNumber = SimulationManager.Instance.NumberOfFoxesToSpawn;
            _grassNumber = SimulationManager.Instance.NumberOfGrassToSpawn;

            _yMaximum = Mathf.Max(_rabbitNumber, _foxNumber, _grassNumber) * _yMaximumMultiplier;
            
            // create line in awake based on vector2 line
            Create(_line);
            
            _submit.onClick.AddListener(ShowTime);
            _save.onClick.AddListener(SaveFile);
        }
        private void Update()
        {
            //catch to not run if paused
            if (UITimeControl.Instance.GetPause())
            {
                return;
            }

            _rabbitNumber = SimulationManager.Instance.RabbitPopulation;
            _foxNumber = SimulationManager.Instance.FoxPopulation;
            _grassNumber = SimulationManager.Instance.GrassPopulation;
            
            //this four new list will update every frame, the purpose of this four list is to keep to CSV file to research
            //because the data of update every frame is more precisely.
            _graphNewRabbitsList.Add(_rabbitNumber);
            _graphNewFoxesList.Add(_foxNumber);
            _graphNewGrassList.Add(_grassNumber);
            _graphTime.Add(Time.timeSinceLevelLoad);
            
            //update every second
            if ((int) Time.timeSinceLevelLoad > _graphRabbitsList.Count)
            {
                var numberOfNew = (int) Time.timeSinceLevelLoad - _graphRabbitsList.Count;
                //check the period of one frame is less than a internal seconds
                if (numberOfNew>1)
                {
                    // for example three internal seconds per frame, so just add three number in this frame
                    //It is a kind of data interpolation, for example, last frame is 12th second(100 rabbit)
                    //this frame is 15th second(200 rabbits), so graphRabbitList will like this [100,125,150,175,200].
                    // here will add 125 150 175 to graphRabbitList, add 200 in line 133, reason is to make sure record data every internal seconds
                    var rabbit = (_rabbitNumber - _graphRabbitsList.Last()) / numberOfNew;
                    var fox = (_foxNumber - _graphFoxesList.Last()) / numberOfNew;
                    var grass = (_grassNumber - _graphGrassList.Last()) / numberOfNew;
                    for (var i = 1; i < numberOfNew; i++)
                    {
                        _graphRabbitsList.Add(_graphRabbitsList.Last()+Mathf.RoundToInt(rabbit*i));
                        _graphFoxesList.Add(_graphFoxesList.Last()+Mathf.RoundToInt(fox*i));
                        _graphGrassList.Add(_graphGrassList.Last()+Mathf.RoundToInt(grass*i));
                    }
                }
                
                // add last one to graphList or, in just add one number every second in normal situation
                _graphRabbitsList.Add(_rabbitNumber);
                _graphFoxesList.Add(_foxNumber);
                _graphGrassList.Add(_grassNumber);
                var graphLength = _graphRabbitsList.Count;
                
                // Y Value can't bigger than 80 percent of YMaximum Value or YMaximum Value will increase as Y Value increase
                if (Mathf.Max(_rabbitNumber, _foxNumber, _grassNumber) / 0.8f > _yMaximum)
                {
                    _lastYMaximum = _yMaximum;
                    _yMaximum = Mathf.Max(_rabbitNumber, _foxNumber, _grassNumber) / 0.8f;
                    UpdateYAxis();
                }
                
                // every second plot one dot
                if (_graphRabbitsList.Count <= _plotNumber)
                {
                    ShowGraph(graphLength,numberOfNew);
                }
                else if (_graphRabbitsList.Count > _plotNumber)
                {
                    if (_input >= _plotNumber) // when user input is 200, show a trend of last 200 seconds,
                    {
                        //change existing dots position
                        UpdateDotsPositionByInput(_input,graphLength);
                    }
                    else // show overall graph from 0s to now
                    {
                        // update rate increase as time goes by
                        if (Time.timeSinceLevelLoad >= _nextTime)
                        {
                            UpdateAllDotsPosition(graphLength);
                            
                            //updateSecond must be 2s, 3s, 4s, increase by graphRabbitsList.Count
                            var updateSecond = Mathf.FloorToInt(_graphRabbitsList.Count / _plotNumber);
                            _nextTime += updateSecond;
                        }
                    }
                }
            }
        }

        //button onclick method
        private void ShowTime()
        {
            var lastInput = int.Parse(_inputField.text);
            _nextTime = Time.timeSinceLevelLoad;
            
            if (_graphRabbitsList.Count <= _plotNumber) return;
            // if lastInput(user input) more than graphRabbitsList.Count, let input = 1 that show overall graph
            _input = lastInput < _graphRabbitsList.Count ? lastInput : 1;
        }

        //save fox,rabbit,grass number to csv file
        private void SaveFile()
        {
            UITimeControl.Instance.Pause();

            var path = StandaloneFileBrowser.StandaloneFileBrowser.SaveFilePanel("Save File", "", "", "csv");
            try
            {
                // Create a new file     
                using (var sw = File.CreateText(path))
                {
                    sw.WriteLine("Seconds" + "," + "Rabbit" + "," + "Fox" + "," + "Grass");
                    for (var i = 0; i< _graphTime.Count;i++)
                    {
                        // save every frame of data and time more precisely
                        sw.WriteLine(_graphTime[i] + "," + _graphNewRabbitsList[i] + "," + _graphNewFoxesList[i] + "," + _graphNewGrassList[i]);
                    }
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            UITimeControl.Instance.Play();
        }

        //Decrease Y position when Y value more than 80 percent of Y maximum
        private void UpdateYAxis()
        {
            var allLabel = _labelYContainer.GetComponentsInChildren<Transform>();
            foreach (var child in allLabel)
            {
                if (child.name.Contains("Label"))
                {
                    continue;
                }
                var labelNumber = int.Parse(child.name);
                var labelText = (int)(_yMaximum / _line.y * labelNumber);
                child.GetComponent<Text>().text = labelText.ToString();
            }
            
            var allDots = _circleContainer.GetComponentsInChildren<RectTransform>();
            foreach (var child in allDots)
            {
                var anchoredPosition = child.anchoredPosition;
                anchoredPosition = new Vector2(anchoredPosition.x, anchoredPosition.y / (_yMaximum / _lastYMaximum));
                child.anchoredPosition = anchoredPosition;
            }
        }

        //update X label value based on different user input and time
        private void UpdateXAxis(int number, int value)
        {
            var allGameObject = _labelXContainer.GetComponentsInChildren<Transform>();
            foreach (var child in allGameObject)
            {
                if (child.name.Contains("Label"))
                {
                    continue;
                }

                var labelNumber = int.Parse(child.name);
                var labelText = (labelNumber - 1) * (value / (int)_line.x) + number;
                child.GetComponent<Text>().text = labelText.ToString();
            }
        }

        //Create the line and label of X and Y
        private void Create(Vector2 lineNumber)
        {
            for (var i = 1; i <= lineNumber.x; i++)
            {
                var xText = (int)(_xMaximum / lineNumber.x * i);
                var labelX = Instantiate(_labelTemplateX, _labelXContainer, false);
                labelX.gameObject.SetActive(true);
                labelX.anchoredPosition = new Vector2(_graphContainerSize.x / lineNumber.x * i * 1f, -7f);
                labelX.GetComponent<Text>().text = xText.ToString();
                labelX.name = i.ToString();

                var dashY = Instantiate(_dashTemplateY, _graphContainer, false);
                dashY.gameObject.SetActive(true);
                dashY.anchoredPosition = new Vector2(_graphContainerSize.x / lineNumber.x * i * 1f, _dashTemplateY.anchoredPosition.y);
            }
            
            for (var i = 1; i <= lineNumber.y; i++)
            {
                var yText = (int)(_yMaximum / lineNumber.y * i);
                var labelY = Instantiate(_labelTemplateY, _labelYContainer, false);
                labelY.gameObject.SetActive(true);
                labelY.anchoredPosition = new Vector2(-7f, _graphContainerSize.y / lineNumber.y * i);
                labelY.GetComponent<Text>().text = yText.ToString();
                labelY.name = i.ToString();

                var dashX = Instantiate(_dashTemplateX, _graphContainer, false);
                dashX.gameObject.SetActive(true);
                dashX.anchoredPosition = new Vector2(_dashTemplateX.anchoredPosition.x, _graphContainerSize.y / lineNumber.y * i);
            }
        }
        
        private void CreateDots(Vector2 anchoredPosition, string objectName)
        {
            var gameObject = new GameObject("circle", typeof(Image));
            gameObject.transform.SetParent(_circleContainer, false);
            gameObject.GetComponent<Image>().sprite = _circleSprite;
            if (objectName != "Rabbit")
            {
                //Fox is red color dots Grass is green color, rabbit is white(default)
                gameObject.GetComponent<Image>().color = objectName == "Fox" ? Color.red : Color.green;
            }
            gameObject.name = objectName + _graphRabbitsList.Count;
            var rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = new Vector2(5, 5);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
        }

        // draw dots between 0s and 100s
        private void ShowGraph(int listLength,int addNumber)
        {
            for (var i = addNumber; i >= 1; i--)
            {
                var xPosition = (listLength-i+1) * _graphContainerSize.x / 100;
                var yPosition = _graphRabbitsList[listLength-i] / _yMaximum * _graphContainerSize.y;
                var yPosition1 = _graphFoxesList[listLength-i] / _yMaximum * _graphContainerSize.y;
                var yPosition2 = _graphGrassList[listLength-i] / _yMaximum * _graphContainerSize.y;
                CreateDots(new Vector2(xPosition, yPosition), "Rabbit");
                CreateDots(new Vector2(xPosition, yPosition1), "Fox");
                CreateDots(new Vector2(xPosition, yPosition2), "Grass");
            }
        }

        // Update Dots Position by user input
        private void UpdateDotsPositionByInput(int value, int listLength)
        {
            var allGameObject = _circleContainer.GetComponentsInChildren<RectTransform>();
            var iR = 1;
            var iF = 1;
            var iG = 1;

            foreach (var child in allGameObject)
            {
                if (child.gameObject.name.Contains("Rabbit"))
                {
                    var a = Mathf.RoundToInt(value * iR / _plotNumber);
                    var yPosition = _graphRabbitsList[listLength - value + a - 1] / _yMaximum * _graphContainerSize.y;
                    var xPosition = iR * _graphContainerSize.x / _plotNumber;

                    child.anchoredPosition = new Vector2(xPosition, yPosition);
                    iR++;
                }
                else if (child.gameObject.name.Contains("Fox"))
                {
                    var a = Mathf.RoundToInt(value * iF / _plotNumber);
                    var yPosition = _graphFoxesList[listLength - value + a - 1] / _yMaximum * _graphContainerSize.y;
                    var xPosition = iF * _graphContainerSize.x / _plotNumber;

                    child.anchoredPosition = new Vector2(xPosition, yPosition);
                    iF++;
                }
                else if (child.gameObject.name.Contains("Grass"))
                {
                    var a = Mathf.RoundToInt(value * iG / _plotNumber);
                    var yPosition = _graphGrassList[listLength - value + a - 1] / _yMaximum * _graphContainerSize.y;
                    var xPosition = iG * _graphContainerSize.x / _plotNumber;

                    child.anchoredPosition = new Vector2(xPosition, yPosition);
                    iG++;
                }
            }

            var number = Mathf.RoundToInt(listLength - value / _line.x * (_line.x-1));
            UpdateXAxis(number, value);
        }

        // Update all Dots Position
        private void UpdateAllDotsPosition(int listLength)
        {
            // Dont understand the logic behind the position calculation but objects are moved now rather than deleted and recreated
            var allGameObject = _circleContainer.GetComponentsInChildren<RectTransform>();
            var iR = 1;
            var iF = 1;
            var iG = 1;

            foreach (var child in allGameObject)
            {
                if (child.gameObject.name.Contains("Rabbit"))
                {
                    var a = Mathf.RoundToInt(listLength * iR / _plotNumber);
                    var yPosition = _graphRabbitsList[a - 1] / _yMaximum * _graphContainerSize.y;
                    var xPosition = iR * _graphContainerSize.x / _plotNumber;

                    child.anchoredPosition = new Vector2(xPosition, yPosition);
                    iR++;
                }
                else if (child.gameObject.name.Contains("Fox"))
                {
                    var a = Mathf.RoundToInt(listLength * iF / _plotNumber);
                    var yPosition = _graphFoxesList[a - 1] / _yMaximum * _graphContainerSize.y;
                    var xPosition = iF * _graphContainerSize.x / _plotNumber;

                    child.anchoredPosition = new Vector2(xPosition, yPosition);
                    iF++;
                }
                else if (child.gameObject.name.Contains("Grass"))
                {
                    var a = Mathf.RoundToInt(listLength * iG / _plotNumber);
                    var yPosition = _graphGrassList[a - 1] / _yMaximum * _graphContainerSize.y;
                    var xPosition = iG * _graphContainerSize.x / _plotNumber;

                    child.anchoredPosition = new Vector2(xPosition, yPosition);
                    iG++;
                }
            }
            
            var number = Mathf.RoundToInt(listLength / _line.x);
            UpdateXAxis(number, listLength);
        }
    }
}
