using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Animal
{
    public SceneSetter scene;
    private float xPos, zPos;
    private float currentXPos, currentZPos;
    public float moveSpeed;
    private System.Random rnd;
    private float tileSize;
    private float leftLimit, upLimit, rightLimit, downLimit;
    private enum Directions
    {
        Left, Up, Right, Down
    }
    private Directions currentDirection;
    // Start is called before the first frame update
    void Start()
    {
        xPos = transform.position.x;
        zPos = transform.position.z;
        moveSpeed = 25f;
        rnd = new System.Random();
        GetLimits();
        RandomizeDirection();
        tileSize = scene.GetTileSize();   
        transform.localScale = new Vector3(3f, 3f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        float distanceMoved;

        Move();
        currentXPos = transform.position.x;
        currentZPos = transform.position.z;
        if (currentDirection == Directions.Left || currentDirection == Directions.Right)
        {
            distanceMoved = CalculateDistanceMoved(xPos, currentXPos);
        }
        else
        {
            distanceMoved = CalculateDistanceMoved(zPos, currentZPos);
        }
        if (distanceMoved >= tileSize)
        {
            xPos = currentXPos;
            zPos = currentZPos;
            RandomizeDirection();
        }
        //print("X Pos: " + currentXPos);
        //print("Z Pos: " + currentZPos);
        //print(distanceMoved);
    }

    void RandomizeDirection()
    {
        bool canMove = false;
        do
        {
            int directionsCounter = Directions.GetNames(typeof(Directions)).Length;
            int rnd = new System.Random().Next(0, directionsCounter);
            switch (rnd)
            {
                case 0:
                    currentDirection = Directions.Left;
                    break;
                case 1:
                    currentDirection = Directions.Up;
                    break;
                case 2:
                    currentDirection = Directions.Right;
                    break;
                case 3:
                    currentDirection = Directions.Down;
                    break;
            }
            if ((currentXPos >= rightLimit && currentDirection == Directions.Right) ||
                (currentXPos <= leftLimit && currentDirection == Directions.Left) ||
                (currentZPos <= downLimit && currentDirection == Directions.Down) ||
                (currentZPos >= upLimit && currentDirection == Directions.Up))
                canMove = false;
            else canMove = true;
            //canMove = true;
        } while (canMove == false);
        print(currentDirection.ToString());
    }

    private float CalculateDistanceMoved(float startPos, float currentPos)
    {
        return System.Math.Abs(currentPos - startPos);
    }

    private void Move()
    {
        if (currentDirection == Directions.Left)
        {
            transform.position += new Vector3(-moveSpeed * Time.deltaTime, 0f, 0f);
        }
        else if (currentDirection == Directions.Right)
        {
            transform.position += new Vector3(moveSpeed * Time.deltaTime, 0f, 0f);
        }
        else if (currentDirection == Directions.Up)
        {
            transform.position += new Vector3(0f, 0f, moveSpeed * Time.deltaTime);
        }
        else
        {
            transform.position += new Vector3(0f, 0f, -moveSpeed * Time.deltaTime);
        }
    }

    private void GetLimits()
    {
        leftLimit = scene.GetLeftLimit();
        upLimit = scene.GetUpLimit();
        rightLimit = scene.GetRightLimit();
        downLimit = scene.GetDownLimit();
    }
}
