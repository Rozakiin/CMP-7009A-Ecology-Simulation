using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace MonoBehaviourTools.Map
{
    public class MapFileGenerator : MonoBehaviour
    {
        public int width;
        public int height;

        public string seed;
        public bool useRandomSeed;
        public Button mapGeneratorButton;
        public Button saveFileButton;
        public Slider widthSlider;
        public Slider heightSlider;
        public Slider waterFillSlider;
        public Text widthSliderText;
        public Text heightSliderText;
        public Text waterFillSliderText;
        public InputField seedInput;

        [Range(0, 100)]
        public int waterFillPercent;

        int[,] map;

        //Tile Sprite rendering
        public Texture2D grassTex;
        public Texture2D waterTex;
        public Texture2D sandTex;
        public Texture2D rockTex;

        public GameObject tilePrefab;
        private GameObject[,] tiles;


        private void Start()
        {
            mapGeneratorButton.onClick.AddListener(TaskOnClick);
            saveFileButton.onClick.AddListener(SaveFile);

            widthSlider.onValueChanged.AddListener(SetWidth);
            heightSlider.onValueChanged.AddListener(SetHeight);
            waterFillSlider.onValueChanged.AddListener(SetWaterFillPercent);

            seedInput.onEndEdit.AddListener(SetSeed);
        }

        private void SetSeed(string value)
        {
            seed = value;
        }

        private void SetWaterFillPercent(float value)
        {
            waterFillPercent = (int)value;
            waterFillSliderText.text = value.ToString();
        }

        private void SetHeight(float value)
        {
            height = (int)value;
            heightSliderText.text = value.ToString();
        }

        private void SetWidth(float value)
        {
            width = (int)value;
            widthSliderText.text = value.ToString();
        }

        void TaskOnClick()
        {
            if (useRandomSeed == true)
            {
                seed = DateTime.Now.ToString();
                GenerateMap(seed);
            }
            else
            {
                GenerateMap(seed);
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
                    for (int j = 0; j < map.GetUpperBound(1); j++)
                    {
                        for (int i = 0; i < map.GetUpperBound(0); i++)
                        {
                            output += map[i, j].ToString();
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
            map = new int[width, height];

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
            for (int i = 1; i < width; i++)
            {
                for (int j = 1; j < height - 1; j++)
                {
                    if (map[i, j] == 0)
                    {
                        map[i - 1, j] = (map[i - 1, j] == 0) ? 0 : 2;
                        map[i + 1, j] = (map[i + 1, j] == 0) ? 0 : 2;
                        map[i, j + 1] = (map[i, j + 1] == 0) ? 0 : 2;
                        map[i, j - 1] = (map[i, j - 1] == 0) ? 0 : 2;
                    }
                }
            }
        }


        private void RandomFillMap(string seed)
        {
            System.Random pseudoRandom = new System.Random(seed.GetHashCode());

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (x == 0 || x == width - 1 || y == 0 || y == height - 1)
                    {
                        map[x, y] = 1;
                    }
                    else
                    {
                        map[x, y] = (pseudoRandom.Next(0, 100) < waterFillPercent) ? 1 : 0;
                    }
                }
            }
        }

        private void SmoothMap()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    int neighbourWallTiles = GetSurroundingWallCount(x, y);

                    if (neighbourWallTiles > 4)
                        map[x, y] = 1;
                    else if (neighbourWallTiles < 4)
                        map[x, y] = 0;
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
                    if (neighbourX >= 0 && neighbourX < width && neighbourY >= 0 && neighbourY < height)
                    {
                        if (neighbourX != gridX || neighbourY != gridY)
                        {
                            wallCount += map[neighbourX, neighbourY];
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
            if (tiles == null)
            {
                tiles = new GameObject[width, height];
            }

            if (map != null && tiles != null)
            {
                // Destroy old gameobjects
                foreach (GameObject tile in tiles)
                {
                    Destroy(tile);
                }
                tiles = new GameObject[width, height];

                float tileWidth = tilePrefab.GetComponent<RectTransform>().rect.width;
                float tileHeight = tilePrefab.GetComponent<RectTransform>().rect.height;
          
                for (int x = 0; x < map.GetUpperBound(0); x++)
                {
                    for (int y = 0; y < map.GetUpperBound(1); y++)
                    {
                        var pos = new Vector3(x * tileWidth, y * tileHeight, 0);
                        tiles[x, y] = Instantiate(tilePrefab); //Instantiate tile
                        tiles[x, y].transform.SetParent(this.transform); // set parent as this
                        tiles[x, y].transform.position = pos; // set position
                        // change colour based on value
                        switch (map[x, y])
                        {
                            case 0:
                                tiles[x, y].GetComponent<Image>().color = Color.blue;
                                break;
                            case 1:
                                tiles[x, y].GetComponent<Image>().color = Color.green;
                                break;
                            case 2:
                                tiles[x, y].GetComponent<Image>().color = Color.yellow;
                                break;
                            case 3:
                                tiles[x, y].GetComponent<Image>().color = Color.grey;
                                break;
                            default:
                                tiles[x, y].GetComponent<Image>().color = Color.black;
                                break;
                        }
                    }
                }
            }
        }
    }
}