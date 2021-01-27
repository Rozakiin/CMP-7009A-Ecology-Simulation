using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace MonoBehaviourTools.Map
{
    public class MapFileGenerator : MonoBehaviour
    {
        public int Width;
        public int Height;

        public string Seed;
        public bool UseRandomSeed;
        public Button MapGeneratorButton;
        public Button SaveFileButton;
        public Slider WidthSlider;
        public Slider HeightSlider;
        public Slider WaterFillSlider;
        public Text WidthSliderText;
        public Text HeightSliderText;
        public Text WaterFillSliderText;
        public InputField SeedInput;

        [Range(0, 100)]
        public int WaterFillPercent;

        int[,] _map;

        //Tile Sprite rendering
        public Texture2D GrassTex;
        public Texture2D WaterTex;
        public Texture2D SandTex;
        public Texture2D RockTex;

        public GameObject TilePrefab;
        private GameObject[,] _tiles;


        private void Start()
        {
            MapGeneratorButton.onClick.AddListener(TaskOnClick);
            SaveFileButton.onClick.AddListener(SaveFile);

            WidthSlider.onValueChanged.AddListener(SetWidth);
            HeightSlider.onValueChanged.AddListener(SetHeight);
            WaterFillSlider.onValueChanged.AddListener(SetWaterFillPercent);

            SeedInput.onEndEdit.AddListener(SetSeed);
        }

        private void SetSeed(string value)
        {
            Seed = value;
        }

        private void SetWaterFillPercent(float value)
        {
            WaterFillPercent = (int)value;
            WaterFillSliderText.text = value.ToString();
        }

        private void SetHeight(float value)
        {
            Height = (int)value;
            HeightSliderText.text = value.ToString();
        }

        private void SetWidth(float value)
        {
            Width = (int)value;
            WidthSliderText.text = value.ToString();
        }

        void TaskOnClick()
        {
            if (UseRandomSeed == true)
            {
                Seed = DateTime.Now.ToString();
                GenerateMap(Seed);
            }
            else
            {
                GenerateMap(Seed);
            }
        }


        private void SaveFile()
        {
            string path = StandaloneFileBrowser.StandaloneFileBrowser.SaveFilePanel("Save File", "", "", "txt");
            if (path != string.Empty)
            {
                FileGenerate(path);
            }
            else
            {
                Debug.Log("No Path Insert");
            }

        }

        private void FileGenerate(string path)
        {
            Debug.Log("File Generate Yeah!");
            try
            {
                // Create a new file     
                using (StreamWriter sw = File.CreateText(path))
                {
                    string output = "";
                    for (int j = _map.GetUpperBound(1); j > 0; j--)
                    {
                        for (int i = 0; i < _map.GetUpperBound(0); i++)
                        {
                            output += _map[i, j].ToString();
                        }
                        sw.WriteLine(output);
                        output = "";
                    }
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }


        private void GenerateMap(string seed)
        {
            _map = new int[Width, Height];

            RandomFillMap(seed);

            for (int i = 0; i < 5; i++)
            {
                SmoothMap();
                CreateMoreTile();
            }

            DrawTiles();
        }

        private void CreateMoreTile()
        {
            for (int i = 1; i < Width; i++)
            {
                for (int j = 1; j < Height - 1; j++)
                {
                    if (_map[i, j] == 0)
                    {
                        _map[i - 1, j] = (_map[i - 1, j] == 0) ? 0 : 2;
                        _map[i + 1, j] = (_map[i + 1, j] == 0) ? 0 : 2;
                        _map[i, j + 1] = (_map[i, j + 1] == 0) ? 0 : 2;
                        _map[i, j - 1] = (_map[i, j - 1] == 0) ? 0 : 2;
                    }
                }
            }
        }


        private void RandomFillMap(string seed)
        {
            System.Random pseudoRandom = new System.Random(seed.GetHashCode());

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (x == 0 || x == Width - 1 || y == 0 || y == Height - 1)
                    {
                        _map[x, y] = 1;
                    }
                    else
                    {
                        _map[x, y] = (pseudoRandom.Next(0, 100) < WaterFillPercent) ? 1 : 0;
                    }
                }
            }
        }

        private void SmoothMap()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    int neighbourWallTiles = GetSurroundingWallCount(x, y);

                    if (neighbourWallTiles > 4)
                        _map[x, y] = 1;
                    else if (neighbourWallTiles < 4)
                        _map[x, y] = 0;
                }
            }
        }

        private int GetSurroundingWallCount(int gridX, int gridY)
        {
            int wallCount = 0;
            for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX++)
            {
                for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY++)
                {
                    if (neighbourX >= 0 && neighbourX < Width && neighbourY >= 0 && neighbourY < Height)
                    {
                        if (neighbourX != gridX || neighbourY != gridY)
                        {
                            wallCount += _map[neighbourX, neighbourY];
                        }
                    }
                    else
                    {
                        wallCount++;
                    }
                }
            }

            return wallCount;
        }


        private void DrawTiles()
        {
            if (_tiles == null)
            {
                _tiles = new GameObject[Width, Height];
            }

            if (_map != null && _tiles != null)
            {
                // Destroy old gameobjects
                foreach (GameObject tile in _tiles)
                {
                    Destroy(tile);
                }
                _tiles = new GameObject[Width, Height];

                float tileWidth = TilePrefab.GetComponent<RectTransform>().rect.width;
                float tileHeight = TilePrefab.GetComponent<RectTransform>().rect.height;
          
                for (int x = 0; x < _map.GetUpperBound(0); x++)
                {
                    for (int y = 0; y < _map.GetUpperBound(1); y++)
                    {
                        var pos = new Vector3(x * tileWidth, y * tileHeight, 0);
                        _tiles[x, y] = Instantiate(TilePrefab); //Instantiate tile
                        _tiles[x, y].transform.SetParent(this.transform); // set parent as this
                        _tiles[x, y].transform.position = pos; // set position
                        // change colour based on value
                        switch (_map[x, y])
                        {
                            case 0:
                                _tiles[x, y].GetComponent<Image>().color = Color.blue;
                                break;
                            case 1:
                                _tiles[x, y].GetComponent<Image>().color = Color.green;
                                break;
                            case 2:
                                _tiles[x, y].GetComponent<Image>().color = Color.yellow;
                                break;
                            case 3:
                                _tiles[x, y].GetComponent<Image>().color = Color.grey;
                                break;
                            default:
                                _tiles[x, y].GetComponent<Image>().color = Color.black;
                                break;
                        }
                    }
                }
            }
        }
    }
}