using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    // Start is called before the first frame update

    private Vector2 inputDirection;
    public static Vector2 moveDirectionInput;
    public static Vector2 faceDirectionInput;
    public static bool shootInput;
    public static bool interactInput;
    public static bool switchInput;

    public Joystick joystick;

    void  Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        InputManager();
    }

    void InputManager()
    {

        inputDirection = Vector2.zero;

        inputDirection = joystick.Direction;


        /*
         * Not used under current movement
         * 
        if (Input.GetKey(KeyCode.W))
        {
            inputDirection += Vector2.up;
        }

        if (Input.GetKey(KeyCode.A))
        {
            inputDirection += Vector2.left;
        }

        if (Input.GetKey(KeyCode.S))
        {
            inputDirection += Vector2.down;
        }

        if (Input.GetKey(KeyCode.D))
        {
            inputDirection += Vector2.right;
        }
        */

        if (Input.GetKey(KeyCode.W))
        {
            inputDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        }

        if (Input.GetMouseButtonDown(0))
        {
            shootInput = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            interactInput = true;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            switchInput = true;
        }

        moveDirectionInput = inputDirection;

        // Distinguish between PC / Mouse and Mobile / Joystick
        if (joystick.isActiveAndEnabled)
        {
            faceDirectionInput = moveDirectionInput;
        }
        else
        {
            faceDirectionInput = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        }
        
    }

    public void shootButton()
    {
        shootInput = true;
    }

    public void interactButton()
    {
        interactInput = true;
    }

    public void switchButton()
    {
        switchInput = true;
    }

}
