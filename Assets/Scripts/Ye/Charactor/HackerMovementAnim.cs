﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

// enum Direction { UP, DOWN, LEFT, RIGHT, UPLEFT, UPRIGHT, DOWNLEFT, DOWNRIGHT };

[RequireComponent(typeof(DeviceReceiver))]

public class HackerMovementAnim: MonoBehaviour {

    bool moveBool;

    Animator anim;
    InputDevice myInputDevice;
    Vector2 moveVector = new Vector2(0, 0);
    AudioClip clip;

    // Use this for initialization
    void Start ()
    {
        clip = GetComponent<AudioSource>().clip;
        anim = GetComponent<Animator>();
	}

    // translate a Direction enum to a normalized Vector3
    Direction Vector2Direction(Vector2 vec)
    {
        
        if (vec.magnitude == 0f)
        {
            Debug.Log("Warning: vec.magnitude == 0f");
            return Direction.RIGHT;
        }
        
        Vector2 rightVector = new Vector2(1f, 0f);

        float angle = Vector2.Angle(rightVector, vec);

        if (vec.y < 0f)
        {
            angle = 360f - angle;
        }
        // play "going upright" animation if angle between 22.5° and 67.5°
        if (angle > 22.5f && angle <= 67.5f)
        {
            return Direction.UPRIGHT;// up
        }
        // play "going up" animation if angle between 67.5° and 112.5°
        else if (angle > 67.5f && angle <= 112.5f)
        {
            return Direction.UP;// left
        }
        // play "going upleft" animation if angle between 225° and 315°
        else if (angle > 112.5f && angle <= 157.5f)
        {
            return Direction.UPLEFT;// down
        }
        else if (angle > 157.5f && angle <= 202.5f)
        {
            return Direction.LEFT;
        }
        else if (angle > 202.5f && angle <= 247.5f)
        {
            return Direction.DOWNLEFT;
        }
        else if (angle > 247.5f && angle <= 292.5f)
        {
            return Direction.DOWN;
        }
        else if (angle > 292.5f && angle <= 357.5f)
        {
            return Direction.DOWNRIGHT;
        }
        else
        {
            return Direction.RIGHT;
        }

    }


    // Update is called once per frame
    void Update()
    {
        // moveBool = anim.GetBool("Moving");
        myInputDevice = GetComponent<DeviceReceiver>().GetDevice();

        if (myInputDevice == null)
        {
            return;
        }
        float horizontal = myInputDevice.LeftStickX;
        float vertical = myInputDevice.LeftStickY;

        moveVector = new Vector2(horizontal, vertical);

        // If magnitude > 1, normalize
        if (moveVector.magnitude > 1f)
        {
            moveVector.Normalize();
        }

        // Magnitude != 0, set moving 
        if ( moveVector.magnitude != 0f )
        {
            // GetComponent<FacingSpriteSwitcher>().enabled = false;
            // Set moving
            anim.SetBool("Moving", true);

            Direction dir = Vector2Direction(moveVector);
            switch (dir)
            {
                case Direction.DOWN:
                    anim.SetFloat("SpeedX", 0);
                    anim.SetFloat("SpeedY", -1);
                    break;
                case Direction.UP:
                    anim.SetFloat("SpeedX", 0);
                    anim.SetFloat("SpeedY", 1);
                    break;
                case Direction.LEFT:
                    anim.SetFloat("SpeedX", -1);
                    anim.SetFloat("SpeedY", 0);
                    break;
                case Direction.RIGHT:
                    anim.SetFloat("SpeedX", 1);
                    anim.SetFloat("SpeedY", 0);
                    break;
                case Direction.DOWNRIGHT:
                    anim.SetFloat("SpeedX", 1);
                    anim.SetFloat("SpeedY", -1);
                    break;
                case Direction.DOWNLEFT:
                    anim.SetFloat("SpeedX", -1);
                    anim.SetFloat("SpeedY", -1);
                    break;
                case Direction.UPLEFT:
                    anim.SetFloat("SpeedX", -1);
                    anim.SetFloat("SpeedY", 1);
                    break;
                case Direction.UPRIGHT:
                    anim.SetFloat("SpeedX", 1);
                    anim.SetFloat("SpeedY", 1);
                    break;
            }
            
        }
        if ( moveVector.magnitude == 0 )
        {
            GetComponent<FacingSpriteSwitcher>().enabled = true;
            GetComponent<FacingSpriteSwitcher>().facing = new Vector3(anim.GetFloat("SpeedX"),anim.GetFloat("SpeedY"),0);
            anim.SetBool("Moving", false);
        }
    }
}