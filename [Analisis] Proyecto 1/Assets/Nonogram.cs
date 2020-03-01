using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Nonogram : MonoBehaviour
{
    class NonogramBoard
    {
        private int[][] HorizontalHints;
        private int[][] VerticalHints;
        private byte[,] Matrix;

        public void Print() 
        {
            string printResult = "";
            int maxRowHints = 0;
            int maxColumnHints = 0;
            foreach (int[] Hints in this.getVerticalHints())
            {
                if (Hints.Length > maxColumnHints)
                    maxColumnHints = Hints.Length;
            }
            foreach (int[] Hints in this.getHorizontalHints())
            {
                if (Hints.Length > maxRowHints)
                    maxRowHints = Hints.Length;
            }
            for (int cont = maxColumnHints; cont!=0 ; cont--) {
                for (int i = 0; i < maxRowHints; i++) 
                {
                    printResult += "---"; 
                }
                printResult += "-";
                foreach (int[] hint in this.getVerticalHints()) 
                {
                    if (hint.Length >= cont)
                    {
                        printResult += ("-" + hint[cont - 1].ToString() + "-");
                    }
                    else
                    {
                        printResult += "---";
                    }
                }
                printResult += "\n";
            }
            for (int cont = this.getHorizontalHints().Length; cont != 0; cont--) 
            {
                int[] hint = this.getHorizontalHints()[this.getHorizontalHints().Length - cont];
                for (int i = maxRowHints; i != 0; i--)
                {
                    if (hint.Length >= i)
                    {
                        printResult += ("-" + hint[i - 1].ToString() + "-") ;
                    }
                    else
                    {
                        printResult += "---";
                    }
                }
                printResult += "|";
                for (int cont2 = 0; cont2 < this.getVerticalHints().Length; cont2++) 
                {
                    printResult += "-" + (this.getMatrix()[this.getHorizontalHints().Length - cont, cont2].ToString() +"-");
                }
                printResult += "\n";
            }
            Debug.Log(printResult);
            return;
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
            int[][] HorizontalHints = new int[rows][];
            line = file.ReadLine();
            int i = 0;
            while ((line = file.ReadLine()) != "COLUMNAS")
            {
                string[] hints = line.Split(',');
                HorizontalHints[i] = new int [hints.Length];
                for (int cont = 0; cont < hints.Length; cont++) 
                {
                    HorizontalHints[i][cont] = Convert.ToInt32(hints[cont].Trim());
                }
                i++;
            }
            int[][] VerticalHints = new int[columns][];
            i = 0;
            while ((line = file.ReadLine()) != null)
            {
                string[] hints = line.Split(',');
                VerticalHints[i] = new int[hints.Length];
                for (int cont = 0; cont < hints.Length; cont++)
                {
                    VerticalHints[i][cont] = Convert.ToInt32(hints[cont].Trim());
                }
                i++;
            }
            board.setVerticalHints(VerticalHints);
            board.setHorizontalHints(HorizontalHints);
            board.setMatrix(Matrix);
            return board;
        }

        public void setVerticalHints(int[][] pVerticalHints)
        {
            this.VerticalHints = pVerticalHints;
            return;
        }

        public int[][] getVerticalHints()
        {
            return this.VerticalHints;
        }

        public void setHorizontalHints(int[][] pHorizontalHints)
        {
            this.HorizontalHints = pHorizontalHints;
            return;
        }

        public int[][] getHorizontalHints()
        {
            return this.HorizontalHints;
        }

        public void setMatrix(byte[,] pMatrix)
        {
            this.Matrix = pMatrix;
            return;
        }

        public byte[,] getMatrix()
        {
            return this.Matrix;
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        NonogramBoard board = NonogramBoard.LoadNonogramBoard("Prueba.txt");
        board.Print();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
