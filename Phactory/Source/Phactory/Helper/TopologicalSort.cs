using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Project.Helper
{
    public class TopologicalSort
    {
        int N;
        bool[,] matrix;
        int[] tasks;
        int edgesNum;

        public struct PairValue
        {
            public int A;
            public int B;

            public PairValue(int A, int B)
            {
                this.A = A;
                this.B = B;
            }
        }
       
        public List<int> Sort(List<PairValue> pairValues)
        {
            N = pairValues.Count;
            matrix = new bool[N + 1, N + 1];

            Dictionary<int, int> dictionary = new Dictionary<int, int>();
            int id = 0;

            edgesNum = N;
            tasks = new int[N*2];
            int index = 0;
            foreach( PairValue pairValue in pairValues)
            {
                int id1 = id;
                if (dictionary.ContainsKey(pairValue.A))
                {
                    dictionary.TryGetValue(pairValue.A, out id1);
                }
                else
                {
                    dictionary.Add(pairValue.A, id);
                    id++;
                }

                int id2 = id;
                if (dictionary.ContainsKey(pairValue.B))
                {
                    dictionary.TryGetValue(pairValue.B, out id2);
                }
                else
                {
                    dictionary.Add(pairValue.B, id);
                    id++;
                }

                tasks[index] = id2;
                tasks[index+1] = id1;
                index+=2;
            }

            TasksIntoAdjacencyMatrix();

            List<int> internalSortedNodes = new List<int>();
            Kahn_TopologicalSort(internalSortedNodes);

            List<int> sortedNodes = new List<int>();
            foreach( int sortedOffset in internalSortedNodes)
            {
                foreach (KeyValuePair<int, int> pair in dictionary)
                {
                    if ( pair.Value == sortedOffset )
                    {
                        sortedNodes.Add(pair.Key);
                    }
                }
            }

            return sortedNodes;
        }

        private void Kahn_TopologicalSort(List<int> sortedNodes)
        {
            //list of nodes  that have no fathers
            Queue<int> freeNodes = new Queue<int>();
            FindFirstFreeNodes(freeNodes);

            while (freeNodes.Count > 0)
            {
                int curr = freeNodes.Dequeue();
                sortedNodes.Add(curr);

                for (int i = 1; i <= N; i++)
                {
                    if (matrix[curr, i] == true)
                    {
                        matrix[curr, i] = false;
                        edgesNum--;

                        bool IsFreeAlready = true;
                        for (int k = 1; k <= N; k++)
                        {
                            if (matrix[k, i] == true)
                            {
                                IsFreeAlready = false;
                                break;
                            }
                        }
                        if (IsFreeAlready == true)
                        {
                            freeNodes.Enqueue(i);
                        }
                    }
                }
            }

            if (edgesNum > 0)
            {
                // Console.WriteLine("There are cycles in the graph!");
                return;
            }
        }

        private void FindFirstFreeNodes(Queue<int> freeNodesList)
        {
            for (int i = 1; i <= N; i++)
            {
                if (matrix[0, i] == false)
                {
                    freeNodesList.Enqueue(i);
                }
            }
        }

        private void TasksIntoAdjacencyMatrix()
        {
            for (int i = 0; i < edgesNum; i++)
            {
                int parent = tasks[(i*2)];
                int child = tasks[(i*2)+1];
                matrix[parent, child] = true;

                // the first row is used to mark the nodes that has parents
                matrix[0, child] = true;
            }
        }
    }
}
