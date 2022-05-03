using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSolver
{
    
    public List<int[]> FindMoves(int[] _currentPosition, int[,,] _colorCube)
    {
        List<int[]> nextPositions = new List<int[]>();
        int[] nextPosition = new int[3];

        for (int i = 0; i < 3; i++)
        {
            if (_currentPosition[i] > 0)
            {
                nextPosition[i] =  _currentPosition[i] - 1;
                if (_colorCube[nextPosition[0], nextPosition[1], nextPosition[2]] == 0)
                {
                    nextPositions.Add(nextPosition); 
                }
            }
            if (_currentPosition[i] < 2)
            {
                nextPosition[i] = _currentPosition[i] + 1;
               
                if (_colorCube[nextPosition[0], nextPosition[1], nextPosition[2]] == 0)
                {
                    nextPositions.Add(nextPosition);
                }

            }

            nextPosition = new int[] { _currentPosition[0], _currentPosition[1], _currentPosition[2]};
        }

        return nextPositions;
    }

    public int[,,] AddToColorCube(int _color, int[] _position, int[,,] _colorCube)
    {
        _colorCube[_position[0], _position[1], _position[2]] = _color;
        return _colorCube;
    }

    public int[,,] RemoveToColorCube(int[] _position, int[,,] _colorCube)
    {
        _colorCube[_position[0], _position[1], _position[2]] = 0;
        return _colorCube;
    }

    public bool isValidInTermsOfColors(Cube[] _cubeTree, int _cubeIndex, Cube _cube) {

        bool isValid = true;
        for (int i = _cubeIndex - 1; i >= 0; i--)
        {
            if (_cubeTree[i].Color == _cube.Color)
            {
                int a = _cubeTree[i].position[0] - _cube.position[0];
                int b = _cubeTree[i].position[1] - _cube.position[1];
                int c = _cubeTree[i].position[2] - _cube.position[2];

                if (a * b * c == 0)
                {
                    //Failed attempt, go to last successful position and see if there are other attempts left
                    isValid = false;
                    break;
                }
            }
        }

        return isValid;
    }
}
