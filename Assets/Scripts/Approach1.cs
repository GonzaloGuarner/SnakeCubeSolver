using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Approach1 : MonoBehaviour
{
    int[,,] colourCube = new int[3, 3, 3];
    int[] cubeArray = { 4, 5, 6, 8, 3, 1, 7, 5, 4, 3, 1, 9, 2, 6, 7, 4, 3, 9, 1, 7, 2, 9, 8, 5, 6, 2, 8 };
    int[,,] colourCube2 = new int[2, 2, 2];
    int[] cubeArray2 = { 4, 1, 2, 3, 1, 4, 3, 2 };
    public int cubeDimension = 3;
    public List<GameObject> cubeList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        Solve(cubeDimension);
    }

    void Solve(int cubeDimension)
    {
        bool success = false;
        bool finished = false;
        int[] currentPosition = { 0, 0, 0 };
        int[] currentPosition2 = new int[3]{ 0, 0, 0 };
        int[] attemptPosition2 = new int[3];
        int[,] cubePosition = new int[27,3];
        int currentCube = 0;
        int a, b, c = 0;
        List<List<int[]>> attempts = new List<List<int[]>>();
        for (int i = 0; i < cubeArray2.Length; i++)
        {
            attempts.Add(new List<int[]>());
        }
        var watch = System.Diagnostics.Stopwatch.StartNew();
        float elapsedMs = watch.ElapsedMilliseconds;

        colourCube2[currentPosition2[0], currentPosition2[1], currentPosition2[2]] = cubeArray2[currentCube];
        while (!success && elapsedMs < 100f)
        {
            Debug.Log(currentCube +"  " + success);
            //Get available neighbours list
            if (attempts[currentCube].Count == 0)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (currentPosition2[i] != 0)
                    {
                        currentPosition2[i]--;
                        if (colourCube2[currentPosition2[0], currentPosition2[1], currentPosition2[2]] == 0)
                        {
                            attempts[currentCube].Add(new int[] { currentPosition2[0], currentPosition2[1], currentPosition2[2] });
                            Debug.Log("Adding Attempt "+ currentCube+":  " + currentPosition2[0] + " " + currentPosition2[1] + " " + currentPosition2[2]);
                        }
                        currentPosition2[i]++;
                    }
                    if (currentPosition2[i] != cubeDimension - 1)
                    {
                        currentPosition2[i]++;
                        if (colourCube2[currentPosition2[0], currentPosition2[1], currentPosition2[2]] == 0)
                        {
                            attempts[currentCube].Add(new int[] { currentPosition2[0], currentPosition2[1], currentPosition2[2] });
                            Debug.Log("Adding Attempt " + currentCube + ":  " + currentPosition2[0] + " " + currentPosition2[1] + " " + currentPosition2[2]);
                        }
                        currentPosition2[i]--;
                    }
                }     
            }
            Debug.Log("Attempts Size "+ currentCube+":   "+ attempts[currentCube].Count);

            //Traverse the attempts tree
            //If no neighbours/no attempts
            if (attempts[currentCube].Count == 0 && currentCube != cubeArray2.Length - 1)
            {
                while (attempts[currentCube].Count == 0 && currentCube >= 0)
                {
                    //Go to previous cube
                    currentCube--;
                    attempts[currentCube].RemoveAt(attempts[currentCube].Count - 1);  
                } 
            }

            attemptPosition2 = attempts[currentCube][attempts[currentCube].Count - 1]; //posible fuente de error
          
            cubePosition[currentCube+1, 0] = attemptPosition2[0];
            cubePosition[currentCube+1, 1] = attemptPosition2[1];
            cubePosition[currentCube+1, 2] = attemptPosition2[2];

            finished = true;
            for (int i = currentCube; i >= 0; i--)
            {
                if (cubeArray2[i] == cubeArray2[currentCube+1])
                {
                    a = cubePosition[i, 0] - cubePosition[currentCube+1, 0];
                    b = cubePosition[i, 1] - cubePosition[currentCube+1, 1];
                    c = cubePosition[i, 2] - cubePosition[currentCube+1, 2];
                    if (a*b*c == 0)
                    {
                        //Failed attempt, go to last successful position and see if there are other attempts left
                        attempts[currentCube].RemoveAt(attempts[currentCube].Count-1);
                        cubePosition[currentCube + 1, 0] = 0;
                        cubePosition[currentCube + 1, 1] = 0;
                        cubePosition[currentCube + 1, 2] = 0;
                        finished = false;
                        break;
                    }
                }
            }
            if (finished)
            {
                //colourCube[currentPosition[0], currentPosition[1], currentPosition[2]] = cubeArray[currentCube];
                colourCube2[attemptPosition2[0], attemptPosition2[1], attemptPosition2[2]] = cubeArray2[currentCube + 1];
                Debug.Log("Current colour:   " + cubeArray2[currentCube] +"   Position " + currentCube + ":  " + attemptPosition2[0] + " " + attemptPosition2[1] + " " + attemptPosition2[2]);
                if (currentCube == cubeArray2.Length - 2)
                {
                    success = true;
                }
                else
                {
                    currentCube++;
                    currentPosition2 = attemptPosition2;
                }
            } 

            elapsedMs = watch.ElapsedMilliseconds;
        }
        for (int i = 0; i < cubeDimension; i++)
        {
            for (int j = 0; j < cubeDimension; j++)
            {
                for (int k = 0; k < cubeDimension; k++)
                {
                    Vector3 position = new Vector3(i, j, k);
                    Instantiate(cubeList[colourCube2[i, j, k]], position, Quaternion.identity);
                }
            }
        }
        watch.Stop();
    }
}
