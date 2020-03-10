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
    private static GameObject[,] Grid;

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

    //clean up
    public void generateGrid(NonogramBoard CurrentNonogramBoard)
    {
        GameObject emptyTileRef = (GameObject)Instantiate(Resources.Load("TileEmpty"));
        GameObject markTileRef = (GameObject)Instantiate(Resources.Load("TileMark"));
        GameObject gridHolder = GameObject.Find("GridHolder");
        RectTransform gridRect = gridHolder.GetComponent<RectTransform>();
        Grid = new GameObject[CurrentNonogramBoard.getRows(), CurrentNonogramBoard.getColumns()];
        float tileSize;
        if (CurrentNonogramBoard.getRows() > CurrentNonogramBoard.getColumns())
            tileSize = gridRect.rect.width / CurrentNonogramBoard.getRows();
        else
            tileSize = gridRect.rect.height / CurrentNonogramBoard.getColumns();
        float tileSpace = tileSize / 4;
        tileSize -= tileSpace;
        float width = CurrentNonogramBoard.getColumns() * tileSpace;
        float height = CurrentNonogramBoard.getRows() * tileSpace;
        for (int row = 0; row < CurrentNonogramBoard.getRows(); row++)
        {
            for (int col = 0; col < CurrentNonogramBoard.getColumns(); col++)
            {
                GameObject emptyTile = (GameObject)Instantiate(emptyTileRef, gridHolder.transform);
                float posX = col * tileSpace;
                float posY = row * -tileSpace;
                emptyTile.transform.localScale = new Vector2(tileSize, tileSize);
                emptyTile.transform.position = new Vector3(posX + 1000 - width / 2 + tileSpace / 2, posY + height / 2 - tileSpace / 2, 1010);
                Grid[row, col] = emptyTile;
            }
        }
        Destroy(emptyTileRef);
        Destroy(markTileRef);
    }

    void Start()
    {
        generateGrid(CurrentNonogramBoard);
        //CurrentNonogramBoard.TimedBacktracking();
        //CurrentNonogramBoard.print();
        //if (CurrentNonogramBoard.isSolvable())
        //    GameObject.Find("Text Execution Time").GetComponent<TextMeshProUGUI>().text = "SOLVED IN:\n" + CurrentNonogramBoard.getSolvingTime().ToString() + " ms";
        //else
        //    GameObject.Find("Text Execution Time").GetComponent<TextMeshProUGUI>().text = "No solution \nfound";
    }

}
