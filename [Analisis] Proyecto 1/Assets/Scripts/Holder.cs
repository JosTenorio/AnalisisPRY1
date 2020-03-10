using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Diagnostics;
using UnityEngine.UI;
using TMPro;

public class Holder : MonoBehaviour
{
    private static NonogramBoard CurrentNonogramBoard = null;

    public static void setCurrentNonogramBoard(NonogramBoard nonogramBoard)
    {
        CurrentNonogramBoard = nonogramBoard;
    }

    public static NonogramBoard getCurrentNonogramBoard()
    {
        return CurrentNonogramBoard;
    }

    public void ReturnButton() 
    {
        SceneManager.LoadScene("Menu");
    }

    void Start()
    {
        CurrentNonogramBoard.TimedBacktracking();
        CurrentNonogramBoard.Print();
        if (CurrentNonogramBoard.isSolvable())
            GameObject.Find("Text Execution Time").GetComponent<TextMeshProUGUI>().text = "Solved in:\n" + CurrentNonogramBoard.getSolvingTime().ToString() + " ms";
        else
            GameObject.Find("Text Execution Time").GetComponent<TextMeshProUGUI>().text = "No solution \nfound";
    }

}
