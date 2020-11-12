using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Utilities
{
    ///<summary>
    /// General purpose utility functions
    ///</summary>
    public static class Utilities
    {
        /**
            Takes a file path as a string and reads the file into another string.
            @param path (string) - the file path
            @return inputLine (string) - the raw contents of the file
        **/
        public static string readTextFile(string path)
        {
            string inputLine = "";
            StreamReader inputStream = new StreamReader(path);
            while(!inputStream.EndOfStream)
            {
                inputLine += inputStream.ReadLine();
            }
            inputStream.Close();
            return inputLine;
        }


        /** 
            Makes a 2D array from a 1D array. Both arrays must be the same template type.
            @param T[] input (template) an array of some type
            @param height (int) - height of the 2D array
            @param width (int) - width of the 2D array
            @return output T[,] (template) - the new 2D array
        **/
        public static T[,] make2DArray<T>(T[]input, int height, int width)
        {
            T[,] output = new T[height, width];
            for(int i = 0; i < height; i++)
            {
                for(int j = 0; j < width; j++)
                {
                    output[i,j] = input[i * width + j];
                }
            }
            return output;
        }

        /** 
            Converts a string into an array of integers.
            The string must be comma delimited in order to be split.
            @param inputString (string) - the string that will be parsed for integers
            @return output (int[]) - an array of integers
        **/
        public static int[] stringToIntArray(string inputString)
        {
            // inputString must contain the folowing delimeters inorder to split the string.
            char[] delimeterChars = {','};
            string[] splitOnDelimeter = inputString.Split(delimeterChars);
            int[]output = new int[splitOnDelimeter.Length];
            for(int i = 0; i < output.Length; i++)
            {
                output[i] = int.Parse(splitOnDelimeter[i]);
            }
            return output;
        }

        /**
            A wrapper function for converting a text file into a 2D array
            @param path (string) - the path to the file to be read
            @param width (int) - the width of the 2D array
            @param height (int) - the height of the 2D array
            @return int[,] - a 2D array of integers
        **/
        public static int[,] build2DArrayFromFile(string path, int width, int height)
        {
            return make2DArray<int>(stringToIntArray(readTextFile(path)), width, height);
        }
    }

    ///<summary>
    /// Implementation of text mesh to draw grids
    ///</summary>
    public static class TextMeshUtilities
    {
        public const int sortingOrderDefault = 5000;

        public static TextMesh CreateWorldText(string text, Transform parent = null, Vector3 localPosition = default(Vector3), int fontSize = 40, Color? color = null, 
            TextAnchor textAnchor = TextAnchor.UpperLeft, TextAlignment textAlignment = TextAlignment.Left, int sortingOrder = sortingOrderDefault)
        {
            if(color == null) color = Color.white;
            return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
        }

        public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, TextAnchor textAnchor, TextAlignment textAlignment, int sortingOrder)
        {
            GameObject gameObject = new GameObject("World Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPosition;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.text = text;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            return textMesh;
        }
    }
}
