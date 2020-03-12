using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Diagnostics;
using UnityEngine.UI;
using TMPro;
using System.Threading;

public class Holder : MonoBehaviour
{
    private static NonogramBoard CurrentNonogramBoard = null;
    private static GameObject[,] Grid;
    private static float tileSize, tileSpace, width, height;
    private bool animateThread, threadDone;

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
        Holder.setCurrentNonogramBoard(null);
        SceneManager.LoadScene("Menu");
    }

    public void startButton()
    {
        if (CurrentNonogramBoard.isAnimated())
        {
            Thread thread = new Thread(animatedBacktrackingThread);
            thread.Start();
        }
        else
        {
            CurrentNonogramBoard.TimedBacktracking();
            drawGrid(CurrentNonogramBoard, (GameObject)Instantiate(Resources.Load("TileEmpty")), (GameObject)Instantiate(Resources.Load("TileMark")), GameObject.Find("GridHolder"));
            updateText();
            CurrentNonogramBoard.print();
        }
    }

    public static void updateHints()
    {
        string VerticalHintsText = GameObject.Find("Vertical Hints").GetComponentInChildren<TextMeshProUGUI>().text;
        string HorizontalHintsText = GameObject.Find("Horizontal Hints").GetComponentInChildren<TextMeshProUGUI>().text;
        int i = 1;
        foreach (List<int> HintList in Holder.getCurrentNonogramBoard().getVerticalHints())
        {
            VerticalHintsText += "(" + i.ToString() + ") ";
            foreach (int Hint in HintList)
            {
                VerticalHintsText += Hint.ToString() + " "; 
            }
            VerticalHintsText += "\n";
            i++;
        }
        i = 1;
        foreach (List<int> HintList in Holder.getCurrentNonogramBoard().getHorizontalHints())
        {
            HorizontalHintsText += "(" + i.ToString() + ") ";
            foreach (int Hint in HintList)
            {
                HorizontalHintsText += Hint.ToString() + " ";
            }
            HorizontalHintsText += "\n";
            i++;
        }
        GameObject.Find("Vertical Hints").GetComponentInChildren<TextMeshProUGUI>().text = VerticalHintsText;
        GameObject.Find("Horizontal Hints").GetComponentInChildren<TextMeshProUGUI>().text =  HorizontalHintsText;
    }

    public void updateText()
    {
        if (CurrentNonogramBoard.isSolvable())
            GameObject.Find("Text Execution Time").GetComponent<TextMeshProUGUI>().text = "Solved in:\n" + CurrentNonogramBoard.getSolvingTime().ToString() + " ms";
        else
            GameObject.Find("Text Execution Time").GetComponent<TextMeshProUGUI>().text = "No solution \nfound";
    }

    public void animatedBacktrackingThread()
    {
        animateThread = true;
        CurrentNonogramBoard.TimedBacktracking();
        threadDone = true;
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

    public void drawGrid(NonogramBoard CurrentNonogramBoard, GameObject emptyTileRef, GameObject markTileRef, GameObject gridHolder)
    {
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

    private void Start()
    {
        Holder.updateHints();
        threadDone = animateThread = false;
        generateGrid(CurrentNonogramBoard);
        drawGrid(CurrentNonogramBoard, (GameObject)Instantiate(Resources.Load("TileEmpty")), (GameObject)Instantiate(Resources.Load("TileMark")), GameObject.Find("GridHolder"));
    }

    private void FixedUpdate()
    {
        if (animateThread)
        {
            if (threadDone)
            {
                updateText();
                CurrentNonogramBoard.print();
                animateThread = false;
            }
            drawGrid(CurrentNonogramBoard, (GameObject)Instantiate(Resources.Load("TileEmpty")), (GameObject)Instantiate(Resources.Load("TileMark")), GameObject.Find("GridHolder"));
        }
    }

}
