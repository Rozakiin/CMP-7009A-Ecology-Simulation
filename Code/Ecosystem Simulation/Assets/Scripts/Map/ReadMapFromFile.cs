using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using UnityEditor;

public class ReadMapFromFile : MonoBehaviour
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