using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using UnityEditor;

public class MapReader
{
    public enum TerrainCost
    {
        Water, 
        Grass,
        Sand,
        Rock
    }

    // Reads in from file a text representation of the map grid with terrain types as numeric value of the TerrainCost enum.
    // This is stored in mapList which is a List of lists passed by reference.
    public static void ReadInMap(string filePath, ref List<List<TerrainCost>> mapList)
    {
        try 
        {
            // Create an instance of StreamReader to read from a file.
            // The using statement also closes the StreamReader.
            using (StreamReader reader = new StreamReader(filePath)) 
            {
                string column;
                string row;
                char[] charSeparators = {' ', '\n'}; // delimeters of space and newline
                string[] words = reader.ReadToEnd().Split(charSeparators, StringSplitOptions.RemoveEmptyEntries); // split file into array of strings, remove emplty entries

                int mapListIndex = 0;
                foreach(string word in words) //iterate over each string in the words array
                {
                    mapList.Add(new List<TerrainCost>()); // add a list of TerrainCost to end of mapList
                    words[mapListIndex] = word.Trim(); // trim whitespace off start and end of string
                    foreach(char cost in words[mapListIndex]) // iterate over each char the string at words[mapListIndex] (each row)
                    {
                        //Debug.Log(cost + ", " + Char.GetNumericValue(cost));
                        mapList[mapListIndex].Add((TerrainCost)Char.GetNumericValue(cost)); // convert the char to numericvalue of char to terraincost and add to list
                    }
                    mapListIndex++; //increment the index position accessing the words array
                }

                row = words[0].Length.ToString(); // length of one row string
                column = words.Length.ToString(); // number of row strings in array


                // Debugging
                // Debug.Log("Row: "+row);
                // Debug.Log("Column"+column);
                // Debug.Log("Printing mapList");
                // foreach(List<TerrainCost> rowList in mapList)
                // {
                //     foreach(TerrainCost cost in rowList)
                //     {
                //         Debug.Log(cost.ToString());
                //     }
                // }
            }
        } 
        catch (Exception e) 
        {
            // Let the user know what went wrong.
            Debug.Log("The file could not be read:");
            Debug.Log(e.Message);
        }
    }
}