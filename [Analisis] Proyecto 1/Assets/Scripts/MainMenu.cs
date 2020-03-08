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

    public static Holder.NonogramBoard LoadFile() 
    {
        string file = "";
        OpenFileDialog openFileDialog1;
        openFileDialog1 = new OpenFileDialog();
        openFileDialog1.Title = "Select text file to load the nonogram";
        openFileDialog1.DefaultExt = "txt";
        openFileDialog1.CheckFileExists = true;
        openFileDialog1.CheckPathExists = true;
        DialogResult result = openFileDialog1.ShowDialog();
        Holder.NonogramBoard newNonogramBoard = null;
        if (result == DialogResult.OK)
        {
            file = openFileDialog1.FileName;
            newNonogramBoard = Holder.NonogramBoard.LoadNonogramBoard(file);
        }
        return newNonogramBoard;
    }

    public void LoadButton() 
    {
        Holder.setCurrentNonogramBoard(LoadFile());
    }

    public void SolveButton() 
    {
        if (Holder.getCurrentNonogramBoard() == null) 
        {
            return;
        }
        GameObject ToggleAnimate = GameObject.Find("Toggle Animate");
        Holder.getCurrentNonogramBoard().setAnimated(ToggleAnimate.GetComponent<Toggle>().isOn);
        SceneManager.LoadScene("Solver");
    }

     // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }
}
