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
            newNonogramBoard = NonogramBoard.LoadNonogramBoard(filePath);
        }
        return newNonogramBoard;
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
