using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    public static NonogramBoard CreateInstanceFromFile() 
    {
        string filePath = "";
        filePath = EditorUtility.OpenFilePanel("Select a text file from which to load the nonogram", "", "txt");
        NonogramBoard newNonogramBoard = null;
        if (filePath != "")
        {
            newNonogramBoard = LoadNonogramBoard(filePath);
        }
        return newNonogramBoard;
    }
    public static NonogramBoard LoadNonogramBoard(string fileName)
    {
        string line;
        System.IO.StreamReader file = new System.IO.StreamReader(fileName);
        string[] dimensions = (line = file.ReadLine()).Split(',');
        int rows = Convert.ToInt32(dimensions[0]);
        int columns = Convert.ToInt32(dimensions[1].Trim());
        byte[,] Matrix = new byte[rows, columns];
        List<List<int>> HorizontalHints = new List<List<int>>();
        line = file.ReadLine();
        while ((line = file.ReadLine()) != "COLUMNAS")
        {
            List<int> IntHints = new List<int>();
            string[] hints = line.Split(',');
            for (int cont = 0; cont < hints.Length; cont++)
            {

                IntHints.Add(Convert.ToInt32(hints[cont].Trim()));
            }
            HorizontalHints.Add(IntHints);
        }
        List<List<int>> VerticalHints = new List<List<int>>();
        while ((line = file.ReadLine()) != null)
        {
            List<int> IntHints = new List<int>();
            string[] hints = line.Split(',');
            for (int cont = 0; cont < hints.Length; cont++)
            {
                IntHints.Add(Convert.ToInt32(hints[cont].Trim()));
            }
            VerticalHints.Add(IntHints);
        }
        NonogramBoard board = new NonogramBoard(Matrix, VerticalHints, HorizontalHints);
        return board;
    }

    public void LoadButton() 
    {
        try
        {
            Holder.setCurrentNonogramBoard(CreateInstanceFromFile());
        } 
        catch (FormatException e)
        {
            Holder.setCurrentNonogramBoard(null);
            Console.WriteLine("Invalid format exception caught.", e);
        }
    }

    public void SolveButton() 
    {
        if (Holder.getCurrentNonogramBoard() != null) 
        {
            GameObject ToggleAnimate = GameObject.Find("Toggle Animate");
            Holder.getCurrentNonogramBoard().setAnimated(ToggleAnimate.GetComponent<Toggle>().isOn);
            SceneManager.LoadScene("Solver");
        }
    }
}
