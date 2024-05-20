using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class InputControl : MonoBehaviour
{
    [Inject] private Joystick joystick;

    public Vector2 Direction { get; private set; }


    // Update is called once per frame
    void Update()
    {
        if (joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            Direction = joystick.Direction;
        }
        else if (Input.anyKey)
        {
            float x = 0;
            float y = 0;

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                x = -1;
            }
            else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                x = 1;
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                y = 1;
            }
            else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                y = -1;
            }

            Direction = new Vector2(x, y);
        }
        else
        {
            Direction = Vector2.zero;
        }
    }
}
