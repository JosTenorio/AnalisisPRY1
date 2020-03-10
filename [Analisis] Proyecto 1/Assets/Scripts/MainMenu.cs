using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Diagnostics;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    [DllImport("user32.dll")]
    private static extern void OpenFileDialog();

    public static NonogramBoard CreateInstanceFromFile() 
    {
        string file = "";
        OpenFileDialog openFileDialog1;
        openFileDialog1 = new OpenFileDialog();
        openFileDialog1.Title = "Select text file to load the nonogram";
        openFileDialog1.DefaultExt = "txt";
        openFileDialog1.CheckFileExists = true;
        openFileDialog1.CheckPathExists = true;
        DialogResult result = openFileDialog1.ShowDialog();
        NonogramBoard newNonogramBoard = null;
        if (result == DialogResult.OK)
        {
            file = openFileDialog1.FileName;
            newNonogramBoard = NonogramBoard.LoadNonogramBoard(file);
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
