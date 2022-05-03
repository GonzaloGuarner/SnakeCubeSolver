using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Approach2 : MonoBehaviour
{
    int[,,] colourCube = new int[3, 3, 3];
    //int[] cubeArray = { 4, 5, 6, 8, 3, 1, 7, 5, 4, 3, 1, 9, 2, 6, 7, 4, 3, 9, 1, 7, 2, 9, 8, 5, 6, 2, 8 };
    int[] cubeArray = { 8, 2, 6, 5, 8, 9, 2, 7, 1, 9, 3, 4, 7, 6, 2, 9, 1, 3, 4, 5, 7, 1, 3, 8, 6, 5, 4 };
    public int cubeDimension = 3;
    public List<GameObject> cubeList = new List<GameObject>();
    int m = 0;
    // Start is called before the first frame update
    void Start()
    {
        Solve(cubeDimension);       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Debug.Log("Number tries: "+ m);
        } 
    }

    void Solve(int cubeDimension)
    {
        bool success = false;
        bool finished = false;
        bool deadEnd = false;
        int[] currentPosition = new int[3] { 0, 0, 0 };
        int[] attemptPosition = new int[3];
        int[,] cubePosition = new int[27,3];
        int currentCube = 0;
        int maxCube = 0;
        int a, b, c = 0;

        List<List<int[]>> attempts = new List<List<int[]>>();
        for (int i = 0; i < cubeArray.Length; i++)
        {
            attempts.Add(new List<int[]>());
        }
        var watch = System.Diagnostics.Stopwatch.StartNew();
        float elapsedMs = watch.ElapsedMilliseconds;

        colourCube[currentPosition[0], currentPosition[1], currentPosition[2]] = cubeArray[currentCube];
        while (!success && elapsedMs < 100000f)
        {
            if (currentCube > maxCube) maxCube = currentCube;
            //Debug.Log(currentCube +"  " + maxCube + "  " + success +"   "+ currentPosition[0] + " " + currentPosition[1] + " " + currentPosition[2]);
            Debug.Log(currentCube + "  " + success + "   " + currentPosition[0] + " " + currentPosition[1] + " " + currentPosition[2]);

            //Get available neighbours list
            if (attempts[currentCube].Count == 0 && !deadEnd)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (currentPosition[i] != 0)
                    {
                        currentPosition[i]--;
                        if (colourCube[currentPosition[0], currentPosition[1], currentPosition[2]] == 0)
                        {
                            attempts[currentCube].Add(new int[] { currentPosition[0], currentPosition[1], currentPosition[2] });
                            //Debug.Log("Adding Attempt "+ currentCube+":  " + currentPosition[0] + " " + currentPosition[1] + " " + currentPosition[2]);
                        }
                        currentPosition[i]++;
                    }
                    if (currentPosition[i] != cubeDimension - 1)
                    {
                        currentPosition[i]++;
                        if (colourCube[currentPosition[0], currentPosition[1], currentPosition[2]] == 0)
                        {
                            attempts[currentCube].Add(new int[] { currentPosition[0], currentPosition[1], currentPosition[2] });
                           // Debug.Log("Adding Attempt " + currentCube + ":  " + currentPosition[0] + " " + currentPosition[1] + " " + currentPosition[2]);
                        }
                        currentPosition[i]--;
                    }
                }
                if (attempts[currentCube].Count == 0) deadEnd = true;
            }
            //Debug.Log("Attempts Size "+ currentCube+":   "+ attempts[currentCube].Count);

            //Traverse the attempts tree
            //If no neighbours/no attempts
            
            if (attempts[currentCube].Count == 0 && currentCube != cubeArray.Length - 1 && deadEnd)
            {
                //Debug.Log("Primer " + (attempts[currentCube-1].Count - 1) + "  " + currentCube + "  " + deadEnd);
                while (attempts[currentCube].Count == 0 && currentCube > 0)
                {
                    colourCube[cubePosition[currentCube, 0], cubePosition[currentCube, 1], cubePosition[currentCube, 2]] = 0;
                    //Go to previous cube
                    currentCube--;
                    //Debug.Log("Primer2 " + (attempts[currentCube].Count) + "  " + currentCube + "  " + deadEnd);
                    currentPosition[0] = cubePosition[currentCube, 0];
                    currentPosition[1] = cubePosition[currentCube, 1];
                    currentPosition[2] = cubePosition[currentCube, 2];
                    attempts[currentCube].RemoveAt(attempts[currentCube].Count - 1);  
                }
                deadEnd = false;
            }
           // Debug.Log("Segon " +(attempts[currentCube].Count - 1) + "  " + currentCube + "  " + deadEnd);
            attemptPosition = attempts[currentCube][attempts[currentCube].Count - 1]; //posible fuente de error
          
            cubePosition[currentCube+1, 0] = attemptPosition[0];
            cubePosition[currentCube+1, 1] = attemptPosition[1];
            cubePosition[currentCube+1, 2] = attemptPosition[2];

            finished = true;
            for (int i = currentCube; i >= 0; i--)
            {
                if (cubeArray[i] == cubeArray[currentCube+1])
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
                        if (attempts[currentCube].Count == 0)
                        {
                            deadEnd = true;
                            m++;
                            if (currentCube > 23)
                            {
                                for (int l = 0; l < cubeDimension; l++)
                                {
                                    for (int j = 0; j < cubeDimension; j++)
                                    {
                                        for (int k = 0; k < cubeDimension; k++)
                                        {
                                            Vector3 position = new Vector3(l + 4 * m, j, k);
                                            Instantiate(cubeList[colourCube[l, j, k]], position, Quaternion.identity);
                                        }
                                    }
                                }  
                            }
                        }
                        break;
                    }
                }
            }
            if (finished)
            {
                //colourCube[currentPosition[0], currentPosition[1], currentPosition[2]] = cubeArray[currentCube];
                colourCube[attemptPosition[0], attemptPosition[1], attemptPosition[2]] = cubeArray[currentCube + 1];
                //Debug.Log("Current colour:   " + cubeArray[currentCube + 1] +"   Position " + currentCube + ":  " + attemptPosition[0] + " " + attemptPosition[1] + " " + attemptPosition[2]);
                if (currentCube == cubeArray.Length - 2)
                {
                    success = true;
                }
                else
                {
                    currentCube++;
                    currentPosition = attemptPosition;
                    deadEnd = false;
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
                    Instantiate(cubeList[colourCube[i, j, k]], position, Quaternion.identity);
                }
            }
        }
        watch.Stop();
    }
}
