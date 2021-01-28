using System;
using System.Collections.Generic;
using System.IO;
using Components;
using UnityEngine;

namespace MonoBehaviourTools.Map
{
    public class MapReader
    {

        /*
         * Reads in from file a text representation of the map grid with terrain types as numeric value of the TerrainCost enum.
         * This is stored in mapList which is a List of lists passed by reference.
         */
        public static bool ReadInMapFromFile(string filePath, ref List<List<TerrainTypeData.TerrainType>> mapList)
        {
            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (var reader = new StreamReader(filePath))
                {
                    char[] charSeparators = { ' ', '\n' }; // delimiters of space and newline
                    var words = reader.ReadToEnd().Split(charSeparators, StringSplitOptions.RemoveEmptyEntries); // split file into array of strings, remove empty entries

                    var mapListIndex = 0;
                    foreach (var word in words) //iterate over each string in the words array
                    {
                        mapList.Add(new List<TerrainTypeData.TerrainType>()); // add a list of TerrainCost to end of mapList
                        words[mapListIndex] = word.Trim(); // trim whitespace off start and end of string
                        foreach (var cost in words[mapListIndex]) // iterate over each char the string at words[mapListIndex] (each row)
                        {
                            mapList[mapListIndex].Add((TerrainTypeData.TerrainType)char.GetNumericValue(cost)); // convert the char to numeric value of char to terrain cost and add to list
                        }
                        mapListIndex++; //increment the index position accessing the words array
                    }

                    mapList.Reverse(); // reverse YList to match how it looks on the map since starts at bottom left in simulation
                    return true;
                }
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Debug.Log("The file could not be read:");
                Debug.Log(e.Message);
            }
            return false;
        }

        /*
         * Reads in from string a text representation of the map grid with terrain types as numeric value of the TerrainCost enum.
         * This is stored in mapList which is a List of lists passed by reference.
         */
        public static bool ReadInMapFromString(string map, ref List<List<TerrainTypeData.TerrainType>> mapList)
        {
            try
            {
                char[] charSeparators = { ' ', '\n' }; // delimiters of space and newline
                var words = map.Split(charSeparators, StringSplitOptions.RemoveEmptyEntries); // split file into array of strings, remove empty entries

                var mapListIndex = 0;
                foreach (var word in words) //iterate over each string in the words array
                {
                    mapList.Add(new List<TerrainTypeData.TerrainType>()); // add a list of TerrainCost to end of mapList
                    words[mapListIndex] = word.Trim(); // trim whitespace off start and end of string
                    foreach (var cost in words[mapListIndex]) // iterate over each char the string at words[mapListIndex] (each row)
                    {
                        mapList[mapListIndex].Add((TerrainTypeData.TerrainType)char.GetNumericValue(cost)); // convert the char to numeric value of char to terrain cost and add to list
                    }
                    mapListIndex++; //increment the index position accessing the words array
                }

                mapList.Reverse(); // reverse YList to match how it looks on the map since starts at bottom left in simulation
                return true;
            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Debug.Log("The String could not be read:");
                Debug.Log(e.Message);
            }
            return false;
        }
    }
}