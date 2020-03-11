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

    public bool isSolvable()
    {
        return this.Solvable;
    }

    public void print()
    {
        string res = "";

        for (int i = 0; i < HorizontalHints.Count; i++)
        {

            res += "\n";
            for (int j = 0; j < VerticalHints.Count; j++)
            {
                if (Matrix[i,j] == 1)
                    res += "▓";
                else
                    res += "▒";
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

    public bool backtracking(byte[,] board, List<List<int>> rowClues, List<List<int>> colClues)
    {
        Tuple<bool, int, int> emptySquare = NonogramBoard.findEmptySquare(board);
        if (!emptySquare.Item1)
            return true;
        int indexX = emptySquare.Item2;
        int indexY = emptySquare.Item3;
        for (byte i = 1; i <= 2; i++)
        {
            if (NonogramBoard.isValid(board, rowClues, colClues, indexX, indexY, i))
            {
                board[indexX, indexY] = i;
                if (backtracking(board, rowClues, colClues))
                    return true;
                board[indexX, indexY] = 0;
            }
        }
        return false;
    }

    public bool animatedBacktracking(byte[,] board, List<List<int>> rowClues, List<List<int>> colClues)
    {
        Tuple<bool, int, int> emptySquare = NonogramBoard.findEmptySquare(board);
        if (!emptySquare.Item1)
            return true;
        int indexX = emptySquare.Item2;
        int indexY = emptySquare.Item3;
        for (byte i = 1; i <= 2; i++)
        {
            if (NonogramBoard.isValid(board, rowClues, colClues, indexX, indexY, i))
            {
                board[indexX, indexY] = i;
                if (i == 1)
                    Holder.changeTile(true, indexX, indexY);
                else
                    Holder.changeTile(false, indexX, indexY);
                if (backtracking(board, rowClues, colClues))
                    return true;
                board[indexX, indexY] = 0;
                Holder.changeTile(false, indexX, indexY);
            }
        }
        return false;
    }

    private static Tuple<bool, int, int> findEmptySquare(byte[,] board)
    {
        for (int i = 0; i <= board.GetUpperBound(0); i++)
            for (int j = 0; j <= board.GetUpperBound(1); j++)
                if (board[i, j] == 0)
                    return Tuple.Create(true, i, j);
        return Tuple.Create(false, 0, 0);
    }

    private static bool isValid(byte[,] board, List<List<int>> rowClues, List<List<int>> colClues, int indexX, int indexY, int value)
    {
        List<int> rowMarks = getRowMarks(board, rowClues, indexX);
        List<int> colMarks = getColumnMarks(board, colClues, indexY);
        if (value == 1)
            return (isValidMarkRow(board, rowClues, indexX, indexY, rowMarks) && isValidMarkColumn(board, colClues, indexY, indexX, colMarks));
        else
            return (isValidBlankRow(board, rowClues, indexX, rowMarks) && isValidBlankColumn(board, colClues, indexY, colMarks));
    }

    private static bool isValidMarkRow(byte[,] board, List<List<int>> rowClues, int indexX, int indexY, List<int> rowMarks)
    {
        for (int i = 0; i < rowMarks.Count; i++)
        {
            if (rowMarks[i] != rowClues[indexX][i])
            {
                if (rowMarks[i] != 0 || indexY == 0 || board[indexX, indexY - 1] != 1)
                    return true;
                else
                    return false;
            }
        }
        return false;
    }

    private static bool isValidMarkColumn(byte[,] board, List<List<int>> colClues, int indexY, int indexX, List<int> colMarks)
    {
        for (int i = 0; i < colMarks.Count; i++)
        {
            if (colMarks[i] != colClues[indexY][i])
            {
                if (colMarks[i] != 0 || indexX == 0 || board[indexX - 1, indexY] != 1)
                    return true;
                else
                    return false;
            }
        }
        return false;
    }

    private static List<int> getRowMarks(byte[,] board, List<List<int>> rowClues, int indexX)
    {
        List<int> rowMarks = new List<int> { };
        for (int i = 0; i < rowClues[indexX].Count; i++)
            rowMarks.Add(0);
        int indexClues = 0;
        bool skip = true;
        for (int i = 0; i <= board.GetUpperBound(1); i++)
        {
            if (board[indexX, i] == 1)
            {
                skip = false;
                rowMarks[indexClues] += 1;
            }
            else if (!skip && board[indexX, i - 1] == 1)
                indexClues++;
        }
        return rowMarks;
    }

    private static List<int> getColumnMarks(byte[,] board, List<List<int>> colClues, int indexY)
    {
        List<int> colMarks = new List<int> { };
        for (int i = 0; i < colClues[indexY].Count; i++)
            colMarks.Add(0);
        int indexClues = 0;
        bool skip = true;
        for (int i = 0; i <= board.GetUpperBound(0); i++)
        {
            if (board[i, indexY] == 1)
            {
                skip = false;
                colMarks[indexClues] += 1;
            }
            else if (!skip && board[i - 1, indexY] == 1)
                indexClues++;
        }
        return colMarks;
    }

    private static bool isValidBlankRow(byte[,] board, List<List<int>> rowClues, int indexX, List<int> rowMarks)
    {
        int rowBlanksMax = board.GetLength(1);
        for (int i = 0; i < rowClues[indexX].Count; i++)
            rowBlanksMax -= rowClues[indexX][i];
        int rowBlanks = 0;
        for (int i = 0; i < board.GetLength(1); i++)
            if (board[indexX, i] == 2)
                rowBlanks++;
        if (rowBlanks < rowBlanksMax)
        {
            for (int i = 0; i < rowMarks.Count; i++)
            {
                if (rowMarks[i] != rowClues[indexX][i])
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

    private static bool isValidBlankColumn(byte[,] board, List<List<int>> colClues, int indexY, List<int> colMarks)
    {
        int colBlanksMax = board.GetLength(0);
        for (int i = 0; i < colClues[indexY].Count; i++)
            colBlanksMax -= colClues[indexY][i];
        int colBlanks = 0;
        for (int i = 0; i < board.GetLength(0); i++)
            if (board[i, indexY] == 2)
                colBlanks++;
        if (colBlanks < colBlanksMax)
        {
            for (int i = 0; i < colMarks.Count; i++)
            {
                if (colMarks[i] != colClues[indexY][i])
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
        if (!Animated)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            this.Solvable = backtracking(this.Matrix, this.HorizontalHints, this.VerticalHints);
            stopwatch.Stop();
            this.SolvingTime = stopwatch.ElapsedMilliseconds;
        }
        else
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            this.Solvable = animatedBacktracking(this.Matrix, this.HorizontalHints, this.VerticalHints);
            stopwatch.Stop();
            this.SolvingTime = stopwatch.ElapsedMilliseconds;
        }
    }
}
