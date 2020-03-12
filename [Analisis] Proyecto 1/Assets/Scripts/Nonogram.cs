using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;
using System.Diagnostics;

public class NonogramBoard
{
    private List<List<int>> HorizontalHints;
    private List<List<int>> VerticalHints;
    private int rows;
    private int columns;
    private byte[,] Matrix;
    private long SolvingTime = 0;
    private bool Animated = false;
    private bool Solvable = false;

    public List<List<int>> getVerticalHints()
    {
        return this.VerticalHints;
    }

    public List<List<int>> getHorizontalHints()
    {
        return this.HorizontalHints;
    }

    public void setVerticalHints(List<List<int>> VerticalHints)
    {
        this.VerticalHints = VerticalHints;
        this.columns = VerticalHints.Count;
    }

    public void setHorizontalHints(List<List<int>> HorizontalHints)
    {
        this.HorizontalHints = HorizontalHints;
        this.rows = HorizontalHints.Count;
    }

    public byte[,] getMatrix()
    {
        return Matrix;
    }

    public int getRows()
    {
        return this.rows;
    }

    public int getColumns()
    {
        return this.columns;
    }

    public void setMatrix(byte[,] Matrix)
    {
        this.Matrix = Matrix;
    }

    public double getSolvingTime()
    {
        return this.SolvingTime;
    }

    public void setAnimated(bool Animated)
    {
        this.Animated = Animated;
    }

    public bool isAnimated()
    {
        return this.Animated;
    }

    public bool isSolvable()
    {
        return this.Solvable;
    }

    public void print()
    {
        string res = "";
        for (int i = 0; i < rows; i++)
        {
            res += "\n";
            for (int j = 0; j < columns; j++)
            {
                if (Matrix[i,j] == 1)
                    res += "▓";
                else
                    res += "▒";
            }
        }
        UnityEngine.Debug.Log(res);
    }

    public void printNumerical()
    {
        string res = "";
        for (int i = 0; i < rows; i++)
        {
            res += "\n";
            for (int j = 0; j < columns; j++)
            {
                res += Matrix[i,j].ToString();
            }
        }
        UnityEngine.Debug.Log(res);
    }

    public void printComplete()
    {
        string printResult = "";
        int maxRowHints = 0;
        int maxColumnHints = 0;
        foreach (List<int> Hints in this.VerticalHints)
        {
            if (Hints.Count > maxColumnHints)
                maxColumnHints = Hints.Count;
        }
        foreach (List<int> Hints in this.HorizontalHints)
        {
            if (Hints.Count > maxRowHints)
                maxRowHints = Hints.Count;
        }
        for (int cont = maxColumnHints; cont != 0; cont--)
        {
            for (int i = 0; i < maxRowHints; i++)
            {
                printResult += "---";
            }
            printResult += "-";
            foreach (List<int> hint in this.VerticalHints)
            {
                if (hint.Count >= cont)
                {
                    printResult += ("-" + hint[hint.Count - cont].ToString() + "-");
                }
                else
                {
                    printResult += "---";
                }
            }
            printResult += "\n";
        }
        for (int cont = this.HorizontalHints.Count; cont != 0; cont--)
        {
            List<int> hint = this.HorizontalHints[this.HorizontalHints.Count - cont];
            for (int i = maxRowHints; i != 0; i--)
            {
                if (hint.Count >= i)
                {
                    printResult += ("-" + hint[hint.Count - i].ToString() + "-");
                }
                else
                {
                    printResult += "---";
                }
            }
            printResult += "|";
            for (int cont2 = 0; cont2 < this.VerticalHints.Count; cont2++)
            {
                if (this.Matrix[this.HorizontalHints.Count - cont, cont2] == 1)
                {
                    printResult += ("-#-");
                }
                else
                {
                    printResult += ("---");
                }
            }
            printResult += "\n";
        }
        UnityEngine.Debug.Log(printResult);
    }

    public static NonogramBoard LoadNonogramBoard(string fileName)
    {
        string line;
        NonogramBoard board = new NonogramBoard();
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
        board.setVerticalHints(VerticalHints);
        board.setHorizontalHints(HorizontalHints);
        board.setMatrix(Matrix);
        return board;
    }

    public bool backtracking(Tuple<int, int, bool> indexes)
    {
        if (indexes.Item3)
            return true;
        int indexX = indexes.Item1;
        int indexY = indexes.Item2;
        for (byte i = 1; i <= 2; i++)
        {
            if (isValid(indexX, indexY, i))
            {
                Matrix[indexX, indexY] = i;
                if (backtracking(findEmptySquare(indexX, indexY)))
                    return true;
                Matrix[indexX, indexY] = 0;
            }
        }
        return false;
    }

    private Tuple<int, int, bool> findEmptySquare(int indexX, int indexY)
    {
        if (indexX == (rows - 1))
        {
            if (indexY == (columns - 1))
                return Tuple.Create(indexX, indexY, true);
            indexX = 0;
            indexY++;
        }
        else
            indexX++;
        return Tuple.Create(indexX, indexY, false);
    }

    private bool isValid(int indexX, int indexY, int value)
    {
        if (value == 1)
            return (isValidMarkRow(indexX, indexY) && isValidMarkColumn(indexY, indexX));
        else
            return (isValidBlankRow(indexX) && isValidBlankColumn(indexY));
    }

    private bool isValidMarkRow(int indexX, int indexY)
    {
        List<int> rowMarks = getRowMarks(indexX);
        for (int i = 0; i < rowMarks.Count; i++)
        {
            if (rowMarks[i] != HorizontalHints[indexX][i])
            {
                if (rowMarks[i] != 0 || indexY == 0 || Matrix[indexX, indexY - 1] != 1)
                    return true;
                else
                    return false;
            }
        }
        return false;
    }

    private bool isValidMarkColumn(int indexY, int indexX)
    {
        List<int> colMarks = getColumnMarks(indexY);
        for (int i = 0; i < colMarks.Count; i++)
        {
            if (colMarks[i] != VerticalHints[indexY][i])
            {
                if (colMarks[i] != 0 || indexX == 0 || Matrix[indexX - 1, indexY] != 1)
                    return true;
                else
                    return false;
            }
        }
        return false;
    }

    private List<int> getRowMarks(int indexX)
    {
        List<int> rowMarks = new List<int> { };
        for (int i = 0; i < HorizontalHints[indexX].Count; i++)
            rowMarks.Add(0);
        int indexClues = 0;
        for (int i = 0; i <= columns; i++)
        {
            if (Matrix[indexX, i] == 1)
            {
                rowMarks[indexClues] += 1;
                if (i + 1 < columns && Matrix[indexX, i + 1] != 1)
                    indexClues++;
            }
            else if (Matrix[indexX, i] == 0)
                break;
        }
        return rowMarks;
    }

    private List<int> getColumnMarks(int indexY)
    {
        List<int> colMarks = new List<int> { };
        for (int i = 0; i < VerticalHints[indexY].Count; i++)
            colMarks.Add(0);
        int indexClues = 0;
        for (int i = 0; i < rows; i++)
        {
            if (Matrix[i, indexY] == 1)
            {
                colMarks[indexClues] += 1;
                if (i + 1 < rows && Matrix[i + 1, indexY] != 1)
                    indexClues++;
            }
            else if (Matrix[i, indexY] == 0)
                break;
        }
        return colMarks;
    }

    private bool isValidBlankRow(int indexX)
    {
        List<int> rowMarks = getRowMarks(indexX);
        int rowBlanksMax = columns;
        for (int i = 0; i < HorizontalHints[indexX].Count; i++)
            rowBlanksMax -= HorizontalHints[indexX][i];
        int rowBlanks = 0;
        for (int i = 0; i < columns; i++)
        {
            if (Matrix[indexX, i] == 2)
                rowBlanks++;
            else if (Matrix[indexX, i] == 0)
                break;
        }
        if (rowBlanks < rowBlanksMax)
        {
            for (int i = 0; i < rowMarks.Count; i++)
            {
                if (rowMarks[i] != HorizontalHints[indexX][i])
                {
                    if (rowMarks[i] == 0)
                        return true;
                    else
                        return false;
                }
            }
            return true;
        }
        return false;
    }

    private bool isValidBlankColumn(int indexY)
    {
        List<int> colMarks = getColumnMarks(indexY);
        int colBlanksMax = rows;
        for (int i = 0; i < VerticalHints[indexY].Count; i++)
            colBlanksMax -= VerticalHints[indexY][i];
        int colBlanks = 0;
        for (int i = 0; i < rows; i++)
        {
            if (Matrix[i, indexY] == 2)
                colBlanks++;
            else if (Matrix[i, indexY] == 0)
                break;
        }
        if (colBlanks < colBlanksMax)
        {
            for (int i = 0; i < colMarks.Count; i++)
            {
                if (colMarks[i] != VerticalHints[indexY][i])
                {
                    if (colMarks[i] == 0)
                        return true;
                    else
                        return false;
                }
            }
            return true;
        }
        return false;
    }

    public void TimedBacktracking()
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        this.Solvable = backtracking(Tuple.Create(0, 0, false));
        stopwatch.Stop();
        this.SolvingTime = stopwatch.ElapsedMilliseconds;
    }
}
