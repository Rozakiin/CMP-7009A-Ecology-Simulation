using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using UnityEditor;

public class MapReader
{
    // code snippet adapted from https://support.unity3d.com/hc/en-us/articles/115000341143-How-do-I-read-and-write-data-from-a-text-file-
    /*static void WriteString(string path)
    {
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine("Test");
        writer.Close();

        //Re-import the file to update the reference in the editor
        AssetDatabase.ImportAsset(path);
        TextAsset asset = Resources.Load("test");

        //Print the text from the file
        Debug.Log(asset.text);
    }*/

/*    // code snippet adapted from https://support.unity3d.com/hc/en-us/articles/115000341143-How-do-I-read-and-write-data-from-a-text-file-
    static void ReadString(string path)
    {
        //Read the text from directly from the test.txt file
        StreamReader reader = new StreamReader(path);
        Debug.Log(reader.ReadToEnd());
        reader.Close();
    }*/

    enum TerrainCost
    {
        Water, 
        Grass,
        Sand,
        Rock
    }
//ref List<List<TerrainCost>> mapList
    public static void ReadMap(string filePath)
    {
        try 
        {
            // Create an instance of StreamReader to read from a file.
            // The using statement also closes the StreamReader.
            using (StreamReader reader = new StreamReader(filePath)) 
            {
                string line;
                string column;
                string row;
                char[] charSeparators = {' ', '\n'}; // delimeters of space and newline
                string[] words = reader.ReadToEnd().Split(charSeparators, StringSplitOptions.RemoveEmptyEntries); // split file into array of strings, remove emplty entries
                List<List<TerrainCost>> mapList = new List<List<TerrainCost>>();                

                int mapListIndex = 0;
                foreach(string word in words) //iterate over each string in the words array
                {
                    mapList.Add(new List<TerrainCost>()); // add a list of TerrainCost to end of mapList
                    words[mapListIndex] = word.Trim(); // trim whitespace off start and end of string
                    foreach(char cost in words[mapListIndex]) // iterate over each char the string at words[mapListIndex] (each row)
                    {
                        Debug.Log(cost + ", " + Char.GetNumericValue(cost));
                        mapList[mapListIndex].Add((TerrainCost)Char.GetNumericValue(cost)); // convert the char to numericvalue of char to terraincost and add to list
                    }
                    mapListIndex++; //increment the index position accessing the words array
                }

                row = words[0].Length.ToString(); // length of one row string
                column = words.Length.ToString(); // number of row strings in array


                // Debugging
                Debug.Log("Row: "+row);
                Debug.Log("Column"+column);
                Debug.Log("Printing mapList");
                foreach(List<TerrainCost> rowList in mapList)
                {
                    foreach(TerrainCost cost in rowList)
                    {
                        Debug.Log(cost.ToString());
                    }
                }
            }
        } 
        catch (Exception e) 
        {
            // Let the user know what went wrong.
            Debug.Log("The file could not be read:");
            Debug.Log(e.Message);
        }
    }

    // converted from lukaz's cpp code
    // enum ETerrainCost
    // {
    //     Clear = 1,
    //     Water = 2,
    //     Wood = 3,
    //     Wall = 0
    // };

    // void readMap(string fileName, vector<vector<ETerrainCost>>& mapVector)
    // {
    //     // cpp code
    //     ifstream infile(fileName);

    //     //Run as long as there are more lines in the text file
    //     if (infile.good())
    //     {
    //         string sLine;   //A single line from the text file
    //         string column;  //The first word in the first line says how many x coords the map has
    //         string row;     //The first word in the first line says how many y coords the map has
    //         getline(infile, sLine);     //Read the first line from the text file
    //         stringstream ssin(sLine);
    //         ssin >> column >> row;      //Assign column and row values
    //         int columnNum = stoi(column);   //Convert string into int used in the loops
    //         int rowNum = stoi(row);
    //         //Push data onto a vector of vectors of ETerrainCost type, of the width and height equal to columnNum and rowNum
    //         for (int i = 0; i < columnNum; i++)
    //         {
    //             mapVector.push_back(vector<ETerrainCost>());
    //             for (int j = 0; j < rowNum; j++)
    //             {
    //                 mapVector[i].push_back(ETerrainCost(0));    //Fill the rows with 0's, setting up vectors maximum length
    //             }
    //         }

    //         //Replace 0's in the vector with data read from the text file. However, as the u axis has to be inverted, we start from the
    //         //rowNum - 1 decrementing i with each loop, so that the first line of the text file is in the last row of the vector of vectors.
    //         for (int i = rowNum - 1; i >= 0; i--)
    //         {
    //             getline(infile, sLine);
    //             int characterInLine = 0;
    //             for (int j = 0; j < columnNum; j++)
    //             {
    //                 mapVector[j][i] = ETerrainCost(sLine.at(characterInLine) - '0');
    //                 characterInLine++;
    //             }
    //         }
    //     }
    //     infile.close();
    // }
}

//public class OpenFilePanelExample : EditorWindow
//{
//    [MenuItem("Example/Overwrite Texture")]
//    static void Apply()
//    {
//        Texture2D texture = Selection.activeObject as Texture2D;
//        if (texture == null)
//        {
//            EditorUtility.DisplayDialog("Select Texture", "You must select a texture first!", "OK");
//            return;
//        }

//        string path = EditorUtility.OpenFilePanel("Overwrite with png", "", "png");
//        if (path.Length != 0)
//        {
//            var fileContent = File.ReadAllBytes(path);
//            texture.LoadImage(fileContent);
//        }
//    }
//}