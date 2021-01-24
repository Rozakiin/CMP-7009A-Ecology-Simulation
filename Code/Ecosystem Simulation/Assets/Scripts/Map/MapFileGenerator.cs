using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Text;
using UnityEngine.UI;

public class MapFileGenerator : MonoBehaviour
{

	public int width;
	public int height;

	public string seed;
	public bool useRandomSeed;
	public Button MapGenerator;
	public Button FileGenerator;

	[Range(0, 100)]
	public int randomFillPercent;

	public MapFileGenerator(int randomFillPercent)
	{
		this.randomFillPercent = randomFillPercent;
	}

	int[,] map;
	void Start()
	{
		GenerateMap("seed");
		//FileGenerate();
		Button btn = MapGenerator.GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
		Button btn1 = FileGenerator.GetComponent<Button>();
		btn1.onClick.AddListener(FileGenerate);
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

	void FileGenerate()
	{
        Debug.Log("File Generate Yeah!");
		string file = Application.dataPath;
		string fileName = file + "/Scripts/Map/Map.txt";
		try
		{
			// Check if file already exists. If yes, delete it.     
			if (File.Exists(fileName))
			{
				File.Delete(fileName);
			}

			// Create a new file     
			using (StreamWriter sw = File.CreateText(fileName))
			{
				string output = "";
				for (int i = 0; i < map.GetUpperBound(0); i++)
				{
					for (int j = 0; j < map.GetUpperBound(1); j++)
					{
						output += map[i, j].ToString();
					}
					sw.WriteLine(output);
					output = "";
				}
				sw.Close();
			}
		}
		catch (Exception Ex)
		{
			Console.WriteLine(Ex.ToString());
		}
	}


	void GenerateMap(string seed)
	{
		map = new int[width, height];
		
		RandomFillMap(seed);

		for (int i = 0; i < 5; i++)
		{
			SmoothMap();
			CreateMoreTile();
		}

	}

	void CreateMoreTile()
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


	void RandomFillMap(string seed)
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
					map[x, y] = (pseudoRandom.Next(0, 100) < randomFillPercent) ? 1 : 0;
				}
			}
		}
	}

	void SmoothMap()
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

	int GetSurroundingWallCount(int gridX, int gridY)
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


	void OnDrawGizmos()
	{
		if (map != null)
		{
			for (int x = 0; x < width; x++)
			{
				for (int y = 0; y < height; y++)
				{
					if (map[x, y] == 1)
					{
						Gizmos.color = Color.green;
					}
					if (map[x, y] == 0)
					{
						Gizmos.color = Color.blue;
					}
					if (map[x, y] == 2)
					{
						Gizmos.color = Color.black;
					}
					Vector3 pos = new Vector3(-width / 2 + x + .5f, 0, -height / 2 + y + .5f);
					Gizmos.DrawCube(pos, Vector3.one);
				}
			}
		}
	}

}