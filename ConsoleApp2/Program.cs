using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace program
{
    class program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            string path = Directory.GetCurrentDirectory() + @"\log.txt";
            float[,] table = {{4f, 2, 3}, 
                              {6f, 2, 1},
                              {3f, 0, 3},
                              {2f, 3, 0},
                              {0f,-7,-5}};

            float[] result = new float[2];
            float[,] table_result;

            program S = new program(table);
            table_result = S.SimplexMetod(result);
            Console.WriteLine("F = 7x1 + 5x2 -> max" + "\t\tF'= -( 7x1 + 5x2 ) -> min" + "\n2x1 + 3x2 <= 4" + "\t\t\t2x1 + 3x2 + x3 = 4" + "\n2x1 + 1x2 <= 6" + "\t\t\t2x1 + 1x2 + x4 = 6" + "\n3x2 <= 3" + "\t\t\t3x2+ x5 = 3" + "\n3x1 <= 2" + "\t\t\t3x1 + x6 = 2" + "\n" + "\t\nx1,x2 => 0" + "\n" + "x3,x4,x5,x6 - Любое");
            Console.WriteLine("\nРешенная симплекс-таблица:");
            for (int i = 0; i < table_result.GetLength(0); i++)
            {

                for (int j = 0; j < table_result.GetLength(1); j++)

                    Console.Write("\t" + Math.Round(table_result[i, j], 2) + "\t|");

                Console.WriteLine();

            }
            Console.WriteLine("\nОтвет:");
            Console.WriteLine("X[1] = " + Math.Round(result[0], 5));
            Console.WriteLine("X[2] = " + Math.Round(result[1], 5));
            Console.WriteLine("F = " + table_result[4,0]);
            Console.WriteLine("Программу выполнил:" + " Рученин Артем");
            Console.ReadLine();
        }

        //симплекс таблица
        float[,] table; 

        //Размерность
        int m, n;

        //список базисных переменных
        List<int> basis;
        //Добавление фиктивных переменных
        public program(float[,] source)
        {
            m = source.GetLength(0);
            n = source.GetLength(1);
            table = new float[m, n + m - 1];
            basis = new List<int>();


            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < table.GetLength(1); j++)
                {
                    if (j < n)
                        table[i, j] = source[i, j];
                    else
                        table[i, j] = 0;
                }
                if ((n + i) < table.GetLength(1))
                {
                    table[i, n + i] = 1;
                    basis.Add(n + i);
                }
            }
            n = table.GetLength(1);
        }
        //цикл с решением
        public float[,] SimplexMetod(float[] result)
        {
            //ведущие столбец и строка
            int mainCol, mainRow;

            while (!IsItEnd())
            {
                mainCol = findMainCol();
                mainRow = findMainRow(mainCol);
                basis[mainRow] = mainCol;

                float[,] new_table = new float[m, n];

                for (int j = 0; j < n; j++)
                    new_table[mainRow, j] = table[mainRow, j] / table[mainRow, mainCol];

                for (int i = 0; i < m; i++)
                {
                    if (i == mainRow)
                        continue;

                    for (int j = 0; j < n; j++)
                        new_table[i, j] = table[i, j] - table[i, mainCol] * new_table[mainRow, j];
                }
                table = new_table;
            }


            for (int i = 0; i < result.Length; i++)
            {
                int k = basis.IndexOf(i + 1);
                if (k != -1)
                    result[i] = table[k, 0];
                else
                    result[i] = 0;
            }

            return table;
        }
        //Завершение цикла
        private bool IsItEnd()
        {
            bool flag = true;
            for (int j = 1; j < n; j++)
            {
                if (table[m - 1, j] < 0)
                {
                    flag = false;
                    break;
                }
            }

            return flag;
        }
        //Поиск разрешающего столбца
        private int findMainCol()
        {
            int mainCol = 1;

            for (int j = 2; j < n; j++)
                if (table[m - 1, j] < table[m - 1, mainCol])
                    mainCol = j;

            return mainCol;
        }
        //Поиск разрешающей строки
        private int findMainRow(int mainCol)
        {
            int mainRow = 0;
            for (int i = 0; i < m - 1; i++)
                if (table[i, mainCol] > 0)
                {
                    mainRow = i; break;
                }

            for (int i = mainRow + 1; i < m - 1; i++)
                if ((table[i, mainCol] > 0) && ((table[i, 0] / table[i, mainCol]) < (table[mainRow, 0] / table[mainRow, mainCol])))
                    mainRow = i;
            return mainRow;
        }
    }
}
