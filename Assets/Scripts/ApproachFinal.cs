using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApproachFinal : MonoBehaviour
{

    
    public List<GameObject> cubeList = new List<GameObject>();

    const int cubeLength = 3;
    int[,,] colourCube = new int[cubeLength, cubeLength, cubeLength];
    int[] cubeArray = { 8, 2, 6, 5, 8, 9, 2, 7, 1, 9, 3, 4, 7, 6, 2, 9, 1, 3, 4, 5, 7, 1, 3, 8, 6, 5, 4 };


    public void Solve()
    {
        bool success = false;
        bool deadEnd = false;
        int[] currentPosition = new int[cubeLength] { 0, 0, 0 };
        int[] attemptPosition = new int[cubeLength];
        int[,] cubePosition = new int[(int)Mathf.Pow(cubeLength, 3), cubeLength];
        int currentCube = 0;

        List<List<int[]>> attemptsTree = new List<List<int[]>>();
        for (int i = 0; i < cubeArray.Length; i++)
        {
            attemptsTree.Add(new List<int[]>());
        }

        colourCube[currentPosition[0], currentPosition[1], currentPosition[2]] = cubeArray[currentCube];

        while (!success)
        {
            //Get available neighbours list
            if (attemptsTree[currentCube].Count == 0 && !deadEnd)
            {
                FindAttempts();
                deadEnd = attemptsTree[currentCube].Count == 0 ? true : false;
            }

            //If no possible attempts, move back in tree until we find other remaining attempts or get to the root

            if (attemptsTree[currentCube].Count == 0 && currentCube != cubeArray.Length - 1 && deadEnd)
            {
                MoveBackInTree();
                if (attemptsTree[currentCube].Count > 0)
                { 
                    deadEnd = false; 
                }
                else 
                {
                    break;
                }
            }

            
            attemptPosition = attemptsTree[currentCube][attemptsTree[currentCube].Count - 1];

            // Check that the attempted position follows the games rules: no two squares of the same colour in any face of the cube
            bool isValid = IsValidAttempt();

            if (isValid) // If it's a valid attempt, 
            {
                if (currentCube == cubeArray.Length - 1)
                {
                    success = true;
                    return;
                }
                else
                {
                    cubePosition[currentCube + 1, 0] = attemptPosition[0];
                    cubePosition[currentCube + 1, 1] = attemptPosition[1];
                    cubePosition[currentCube + 1, 2] = attemptPosition[2];

                    MoveStepForwardInTree();
                }
            }
            else 
            {
                // Failed attempt, remove it and see if there are other attempts left
                attemptsTree[currentCube].RemoveAt(attemptsTree[currentCube].Count - 1);
                if (attemptsTree[currentCube].Count == 0)
                {
                    deadEnd = true;
                }
            }
        }     

        void FindAttempts()
        {
            for (int i = 0; i < 3; i++) // Try finding in every axis
            {
                //Towards lower coordinate
                TryAddAttempt(i, 0);
                //Towards bigger coordinate
                TryAddAttempt(i, cubeLength - 1);
            }
        }
        void MoveStepForwardInTree()
        {

            colourCube[attemptPosition[0], attemptPosition[1], attemptPosition[2]] = cubeArray[currentCube + 1];

            currentCube++;
            currentPosition = attemptPosition;
            Debug.Log("MovingForward  " + attemptPosition[0] + attemptPosition[1] + attemptPosition[2]);
            Debug.Log("Colour  " + colourCube[attemptPosition[0], attemptPosition[1], attemptPosition[2]]);
            deadEnd = false;
        }
        void MoveStepBackInTree()
        {
            colourCube[cubePosition[currentCube, 0], cubePosition[currentCube, 1], cubePosition[currentCube, 2]] = 0;

            currentCube--;
            currentPosition[0] = cubePosition[currentCube, 0];
            currentPosition[1] = cubePosition[currentCube, 1];
            currentPosition[2] = cubePosition[currentCube, 2];
            attemptsTree[currentCube].RemoveAt(attemptsTree[currentCube].Count - 1);
        }
        void MoveBackInTree()
        {
            while (attemptsTree[currentCube].Count == 0 && currentCube > 0)
            {
                MoveStepBackInTree();
            }
        }
        void TryAddAttempt(int coordinate, int boundary)
        {
            if (currentPosition[coordinate] != boundary)
            {
                int coordinateIncrement = 2 * (int)Mathf.Clamp01(boundary) - 1; // The increment is negative if we are in the maximum boundary and pos if we are on the minimum (on a 5x1 its m x x x M)

                currentPosition[coordinate] += coordinateIncrement; // Getting the position to check if the candidate place is empty

                if (colourCube[currentPosition[0], currentPosition[1], currentPosition[2]] == 0)
                {
                    attemptsTree[currentCube].Add(new int[] { currentPosition[0], currentPosition[1], currentPosition[2] });
                }
                currentPosition[coordinate] -= coordinateIncrement; // Getting the position back to it's original coordinates
            }
        }
        bool IsValidAttempt()
        {
            int a, b, c;
            for (int i = currentCube; i >= 0; i--) //Check all of the previously placed cubes
            {
                if (cubeArray[i] == cubeArray[currentCube + 1]) // If same colour
                {
                    a = cubePosition[i, 0] - attemptPosition[0];
                    b = cubePosition[i, 1] - attemptPosition[1];
                    c = cubePosition[i, 2] - attemptPosition[2];

                    if (a * b * c == 0) // If any coordinate is the same this will be true. Invalid attempt
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }

    public void InstantiateCube()
    {
        for (int i = 0; i < cubeLength; i++)
        {
            for (int j = 0; j < cubeLength; j++)
            {
                for (int k = 0; k < cubeLength; k++)
                {
                    Instantiate(cubeList[colourCube[i, j, k]], new Vector3(i, j, k), Quaternion.identity);
                }
            }
        }
    }
}
