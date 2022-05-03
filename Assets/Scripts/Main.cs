using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public int[] tryPosition;
    Cube[] cubeTree = new Cube[27];
    public int[,,] colorCube = new int[3, 3, 3];
    int[] colorArray = { 4, 5, 6, 8, 3, 1, 7, 5, 4, 3, 1, 9, 2, 6, 7, 4, 3, 9, 1, 7, 2, 9, 8, 5, 6, 2, 8 };
    int levelTree = 0;

    // Start is called before the first frame update
    void Start()
    {
        Solve();
    }

    public int assignedColor(int _cubeIndex)
    {
        return colorArray[_cubeIndex];
    }

    public void AssignColor(int[] _position, int _cubeIndex)
    {
        colorCube[_position[0], _position[1], _position[2]] = assignedColor(_cubeIndex);
    }
    public void UnassignColor(int[] _position)
    {
        colorCube[_position[0], _position[1], _position[2]] = 0;
    }

    public void UnassignCubeTree()
    {
        cubeTree[levelTree] = null;
    }

    public void Solve()
    {
        CubeSolver cubeSolver = new CubeSolver();
        cubeTree = InitialValues(cubeTree, colorCube);

        GoUp();

    }

    private Cube[] InitialValues(Cube[] _cubeTree, int[,,] _colorCube)
    {
        CubeSolver cubeSolver = new CubeSolver();

        _cubeTree[0] = new Cube();

        _cubeTree[0].Color = 4;
        _cubeTree[0].position = tryPosition;
        _cubeTree[0].posPos = cubeSolver.FindMoves(_cubeTree[0].position, _colorCube);
        _cubeTree[0].posVisited = 0;

        AssignColor(_cubeTree[0].position, 0);
        levelTree += 1;
        tryPosition = _cubeTree[0].posPos[_cubeTree[0].posVisited];

        return _cubeTree;
    }

    public void GoUp()
    {
        Cube currentCube = new Cube();
        CubeSolver cubeSolver = new CubeSolver();
        bool isValid = true;

        currentCube.Color = assignedColor(levelTree);
        currentCube.position = tryPosition;
        currentCube.posVisited = 0;

        if (cubeSolver.isValidInTermsOfColors(cubeTree, levelTree, currentCube))
        {
            if (currentCube.posPos == null)//possible error
            {
                currentCube.posPos = cubeSolver.FindMoves(currentCube.position, colorCube);

                if (currentCube.posPos.Count == 0)
                {
                    isValid = false;
                }
            }
        }
        else
        {
             isValid = false;
        }
        if (isValid)
        {
            cubeTree[levelTree] = currentCube;
            NextCube();
        }
        else
        {
            GoDownPostVisited();
        }
    }

    private void GoDownPostVisited()
    {
        Debug.Log("down");
        UnassignColor(cubeTree[levelTree].position);
        UnassignCubeTree();
        levelTree--;
        cubeTree[levelTree].posVisited++;
        if (cubeTree[levelTree].posPos.Count != cubeTree[levelTree].posVisited)
        {
            NextCube();
            GoUp();
        }
        else
        {
            GoDownPostVisited();
        }
        
    }

    private void NextCube()
    {
        
        tryPosition = cubeTree[levelTree].posPos[cubeTree[levelTree].posVisited];
        
        AssignColor(cubeTree[levelTree].position, levelTree);
        levelTree++;
        GoUp();
    }
}
