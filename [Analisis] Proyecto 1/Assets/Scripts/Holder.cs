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
    private static float tileSize, tileSpace, width, height;

    public static void setCurrentNonogramBoard(NonogramBoard nonogramBoard)
    {
        CurrentNonogramBoard = nonogramBoard;
    }

    public static NonogramBoard getCurrentNonogramBoard()
    {
        return CurrentNonogramBoard;
    }
    public void returnButton() 
    {
        SceneManager.LoadScene("Menu");
    }

    public void startButton()
    {
        CurrentNonogramBoard.TimedBacktracking();
        drawGrid(CurrentNonogramBoard);
        CurrentNonogramBoard.print();
        if (CurrentNonogramBoard.isSolvable())
            GameObject.Find("Text Execution Time").GetComponent<TextMeshProUGUI>().text = "SOLVED IN:\n" + CurrentNonogramBoard.getSolvingTime().ToString() + " ms";
        else
            GameObject.Find("Text Execution Time").GetComponent<TextMeshProUGUI>().text = "No solution \nfound";
    }

    public void generateGrid(NonogramBoard CurrentNonogramBoard)
    {
        GameObject gridHolder = GameObject.Find("GridHolder");
        RectTransform gridRect = gridHolder.GetComponent<RectTransform>();
        Grid = new GameObject[CurrentNonogramBoard.getRows(), CurrentNonogramBoard.getColumns()];
        if (CurrentNonogramBoard.getRows() > CurrentNonogramBoard.getColumns())
            tileSize = gridRect.rect.width / CurrentNonogramBoard.getRows();
        else
            tileSize = gridRect.rect.height / CurrentNonogramBoard.getColumns();
        tileSpace = tileSize / 4;
        tileSize -= tileSpace;
        width = CurrentNonogramBoard.getColumns() * tileSpace;
        height = CurrentNonogramBoard.getRows() * tileSpace;
    }

    public void drawGrid(NonogramBoard CurrentNonogramBoard)
    {
        GameObject emptyTileRef = (GameObject)Instantiate(Resources.Load("TileEmpty"));
        GameObject markTileRef = (GameObject)Instantiate(Resources.Load("TileMark"));
        GameObject gridHolder = GameObject.Find("GridHolder");
        for (int row = 0; row < CurrentNonogramBoard.getRows(); row++)
        {
            for (int col = 0; col < CurrentNonogramBoard.getColumns(); col++)
            {
                GameObject tile;
                if (CurrentNonogramBoard.getMatrix()[row, col] != 1)
                    tile = (GameObject)Instantiate(emptyTileRef, gridHolder.transform);
                else
                    tile = (GameObject)Instantiate(markTileRef, gridHolder.transform);
                float posX = col * tileSpace;
                float posY = row * -tileSpace;
                tile.transform.localScale = new Vector2(tileSize, tileSize);
                tile.transform.position = new Vector3(posX + 1000 - width / 2 + tileSpace / 2, posY + height / 2 - tileSpace / 2, 1010);
                Destroy(Grid[row, col]);
                Grid[row, col] = tile;
            }
        }
        Destroy(emptyTileRef);
        Destroy(markTileRef);
    }

    public static void changeTile(bool mark, int row, int column)
    {
        GameObject emptyTileRef = (GameObject)Instantiate(Resources.Load("TileEmpty"));
        GameObject markTileRef = (GameObject)Instantiate(Resources.Load("TileMark"));
        GameObject gridHolder = GameObject.Find("GridHolder");
        GameObject currentTile = Grid[row, column];
        GameObject tile;
        if (mark)
            tile = (GameObject)Instantiate(markTileRef, gridHolder.transform);
        else
            tile = (GameObject)Instantiate(emptyTileRef, gridHolder.transform);
        tile.transform.localScale = currentTile.transform.localScale;
        tile.transform.position = currentTile.transform.position;
        Destroy(Grid[row, column]);
        Destroy(emptyTileRef);
        Destroy(markTileRef);
        Grid[row, column] = tile;
    }

    void Start()
    {
        generateGrid(CurrentNonogramBoard);
        drawGrid(CurrentNonogramBoard);
    }

}
