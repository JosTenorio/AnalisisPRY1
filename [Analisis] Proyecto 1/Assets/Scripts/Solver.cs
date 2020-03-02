using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solver : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        List<List<int>> rowClues = new List<List<int>> { new List<int> { 3 }, new List<int> { 2, 1 }, new List<int> { 3, 2 }, new List<int> { 2, 2 }, new List<int> { 6 }, new List<int> { 1, 5 }, new List<int> { 6 }, new List<int> { 1 }, new List<int> { 2 } };
        List<List<int>> colClues = new List<List<int>> { new List<int> { 1, 2 }, new List<int> { 3, 1 }, new List<int> { 1, 5 }, new List<int> { 7, 1 }, new List<int> { 5 }, new List<int> { 3 }, new List<int> { 4 }, new List<int> { 3 } };
        int[,] board = new int[rowClues.Count, colClues.Count];
        initBoard(board);
        bool result = backtracking(board, rowClues, colClues);
        printArray(board);
        Console.WriteLine(result.ToString());
    }

    static void printArray(int[,] array)
    {
        for (int i = 0; i <= array.GetUpperBound(0); i++)
        {
            for (int j = 0; j <= array.GetUpperBound(1); j++)
            {
                if (array[i, j] == 1)
                    Console.Write("#");
                else
                    Console.Write("-");
            }
            Console.WriteLine();
        }
    }

    static void initBoard(int[,] board)
    {
        for (int i = 0; i <= board.GetUpperBound(0); i++)
            for (int j = 0; j <= board.GetUpperBound(1); j++)
                board[i, j] = 0;
    }

    static Tuple<bool, int, int> findEmptySquare(int[,] board)
    {
        for (int i = 0; i <= board.GetUpperBound(0); i++)
            for (int j = 0; j <= board.GetUpperBound(1); j++)
                if (board[i, j] == 0)
                    return Tuple.Create(true, i, j);
        return Tuple.Create(false, 0, 0);
    }

    static bool isValid(int[,] board, List<List<int>> rowClues, List<List<int>> colClues, int indexX, int indexY, int value)
    {
        List<int> rowMarks = getRowMarks(board, rowClues, indexX);
        List<int> colMarks = getColumnMarks(board, colClues, indexY);
        if (value == 1)
            return (isValidMarkRow(board, rowClues, indexX, indexY, rowMarks) && isValidMarkColumn(board, colClues, indexY, indexX, colMarks));
        else
            return (isValidBlankRow(board, rowClues, indexX, rowMarks) && isValidBlankColumn(board, colClues, indexY, colMarks));
    }

    static bool isValidMarkRow(int[,] board, List<List<int>> rowClues, int indexX, int indexY, List<int> rowMarks)
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

    static bool isValidMarkColumn(int[,] board, List<List<int>> colClues, int indexY, int indexX, List<int> colMarks)
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

    static List<int> getRowMarks(int[,] board, List<List<int>> rowClues, int indexX)
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

    static List<int> getColumnMarks(int[,] board, List<List<int>> colClues, int indexY)
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

    static bool isValidBlankRow(int[,] board, List<List<int>> rowClues, int indexX, List<int> rowMarks)
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

    static bool isValidBlankColumn(int[,] board, List<List<int>> colClues, int indexY, List<int> colMarks)
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

    static bool backtracking(int[,] board, List<List<int>> rowClues, List<List<int>> colClues)
    {
        Tuple<bool, int, int> emptySquare = findEmptySquare(board);
        if (!emptySquare.Item1)
            return true;
        int indexX = emptySquare.Item2;
        int indexY = emptySquare.Item3;
        for (int i = 1; i <= 2; i++)
        {
            if (isValid(board, rowClues, colClues, indexX, indexY, i))
            {
                board[indexX, indexY] = i;
                printArray(board);
                Console.WriteLine();
                if (backtracking(board, rowClues, colClues))
                    return true;
                board[indexX, indexY] = 0;
            }
        }
        return false;
    }
}




