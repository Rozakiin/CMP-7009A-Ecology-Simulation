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
    public static void ReadInMapFromFile(string filePath, ref List<List<TerrainCost>> mapList)
    {
        try 
        {
            // Create an instance of StreamReader to read from a file.
            // The using statement also closes the StreamReader.
            using (StreamReader reader = new StreamReader(filePath)) 
            {
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

                mapList.Reverse(); // reverse YList to match how it looks on the map since starts at bottom left in simulation

                // Debugging
                // string row = words[0].Length.ToString(); // length of one row string
                // string column = words.Length.ToString(); // number of row strings in array
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

    // Reads in from file a text representation of the map grid with terrain types as numeric value of the TerrainCost enum.
    // This is stored in mapList which is a List of lists passed by reference.
    public static bool ReadInMapFromString(string map)
    {
        try
        {
            List<List<TerrainCost>> mapList = new List<List<MapReader.TerrainCost>>();
            char[] charSeparators = { ' ', '\n' }; // delimeters of space and newline
            string[] words = map.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries); // split file into array of strings, remove emplty entries

            int mapListIndex = 0;
            foreach (string word in words) //iterate over each string in the words array
            {
                mapList.Add(new List<TerrainCost>()); // add a list of TerrainCost to end of mapList
                words[mapListIndex] = word.Trim(); // trim whitespace off start and end of string
                foreach (char cost in words[mapListIndex]) // iterate over each char the string at words[mapListIndex] (each row)
                {
                    //Debug.Log(cost + ", " + Char.GetNumericValue(cost));
                    mapList[mapListIndex].Add((TerrainCost)Char.GetNumericValue(cost)); // convert the char to numericvalue of char to terraincost and add to list
                }
                mapListIndex++; //increment the index position accessing the words array
            }

            mapList.Reverse(); // reverse YList to match how it looks on the map since starts at bottom left in simulation
            return true;
        }
        catch (Exception e)
        {
            // Let the user know what went wrong.
            Debug.Log("The file could not be read:");
            Debug.Log(e.Message);
        }
        return false;
    }
}