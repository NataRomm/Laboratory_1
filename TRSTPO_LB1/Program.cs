using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TRSTPO_LB1
{
    class Program
    {
        public static int[] generateMas(int begin, int end, int n)
        {
            //                                заповнення масиву випадковими числами
            Random r = new Random();
            int[] arr = new int[n];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = r.Next(begin, end + 1);
            }
            return arr;
        }

        public static void writeToFile(int[] arr, string fileName)
        {
            //                                 запис масиву у файл
            using (StreamWriter st = new StreamWriter(@fileName))
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    st.Write(arr[i] + "\n");
                }
            }
        }

        public static int[] readFromFile(string fileName)
        {
            //                         зчитування масиву із файлу
            using (StreamReader st = new StreamReader(@fileName))
            {
                return st.ReadToEnd().TrimEnd().Split('\n').Select(int.Parse).ToArray();
            }
        }

        public static void bubbleSort(ref int[] arr)
        {
            //                           сортування обміном (бульбашкою) за зростанням
            int s = 1; int temp = 0;
            for (int j = 0; j < arr.Length; j++)
            {
                for (int i = 0; i < arr.Length - 1; i++)
                {
                    if (arr[i] * s > arr[i + 1] * s)
                    {
                        temp = arr[i];
                        arr[i] = arr[i + 1];
                        arr[i + 1] = temp;
                    }
                }
            }
        }

        public static int[] threadSort(int[] arr)      
        {
            //                           розділяє масив на 2 частини
            int[] left = new int[arr.Length/2], right = new int[arr.Length - arr.Length / 2];
            for (int i = 0; i < arr.Length/2; ++i) left[i] = arr[i];
            for (int i = arr.Length / 2, j = 0; i < arr.Length; ++i, ++j) right[j] = arr[i];
            //                           створює потоки
            Thread thread1 = new Thread(()=>bubbleSort(ref left));
            Thread thread2 = new Thread(()=> bubbleSort(ref right));
            //                           запускає потік
            thread1.Start();
            thread2.Start();
            //                           чекає на завершення потоку
            thread1.Join();
            thread2.Join();
            //                           зєднує 2 частини потоків сортуючи їх
            int[] sortedArr = left.Concat(right).OrderBy(x => x).ToArray();
            return sortedArr;
        }
            
        static void Main(string[] args)
        {
            var arr = generateMas(0, 5000, 28000);
            writeToFile(arr, "input.txt");

            Stopwatch watch = Stopwatch.StartNew();
            bubbleSort(ref arr);
            watch.Stop();
            Console.WriteLine("Normal Time: {0} s", watch.ElapsedMilliseconds/1000.0);

            writeToFile(arr, "output.txt");


            var arrToSort = readFromFile("input.txt");

            watch.Restart();
            arr = threadSort(arrToSort);
            watch.Stop();
            Console.WriteLine("Thread Time: {0} s", watch.ElapsedMilliseconds / 1000.0);

            writeToFile(arr, "outThread.txt");

            Console.ReadKey();
        }
    }
}
