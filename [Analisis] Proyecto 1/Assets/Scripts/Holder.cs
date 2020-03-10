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
    private float tileSize = 8;

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

    public static void generateGrid(NonogramBoard CurrentNonogramBoard, float tileSize)
    {
        GameObject emptyTileRef = (GameObject)Instantiate(Resources.Load("TileEmpty"));
        GameObject markTileRef = (GameObject)Instantiate(Resources.Load("TileMark"));
        GameObject gridHolder = GameObject.Find("GridHolder");
        float gridW = CurrentNonogramBoard.getColumns() * tileSize;
        float gridH = CurrentNonogramBoard.getRows() * tileSize;
        for (int row = 0; row < CurrentNonogramBoard.getRows(); row++)
        {
            for (int col = 0; col < CurrentNonogramBoard.getColumns(); col++)
            {
                GameObject emptyTile = (GameObject)Instantiate(emptyTileRef, gridHolder.transform);
                float posX = col * tileSize;
                float posY = row * -tileSize;
                emptyTile.transform.localScale = new Vector2(20, 20);
                emptyTile.transform.position = new Vector3(posX + 1000 - gridW / 2 + tileSize / 2, posY + gridH / 2 - tileSize / 2, 1010);
            }
        }
        Destroy(emptyTileRef);
        Destroy(markTileRef);
    }

    void Start()
    {
        generateGrid(CurrentNonogramBoard, tileSize);
        //CurrentNonogramBoard.TimedBacktracking();
        //CurrentNonogramBoard.print();
        //if (CurrentNonogramBoard.isSolvable())
        //    GameObject.Find("Text Execution Time").GetComponent<TextMeshProUGUI>().text = "SOLVED IN:\n" + CurrentNonogramBoard.getSolvingTime().ToString() + " ms";
        //else
        //    GameObject.Find("Text Execution Time").GetComponent<TextMeshProUGUI>().text = "No solution \nfound";
    }

}
