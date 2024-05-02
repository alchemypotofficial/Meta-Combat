using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class InputController : MonoBehaviour
{
    public enum InputValue
    {
        NONE = 0,
        RIGHT_PRESS = 1,
        LEFT_PRESS = 2,
        CROUCH_PRESS = 3,
        JUMP_PRESS = 4,
        LIGHT_PRESS = 5,
        MEDIUM_PRESS = 6,
        HEAVY_PRESS = 7,
        RIGHT_RELEASE = -1,
        LEFT_RELEASE = -2,
        CROUCH_RELEASE = -3,
        JUMP_RELEASE = -4,
        LIGHT_RELEASE = -5,
        MEDIUM_RELEASE = -6,
        HEAVY_RELEASE = -7,
    }

    private PlayerControls _playerControls;
    private int[] _socdRightLeft = new int[4] { 1, 2, 0, 0 }; /* Simultaneous Opposing Cardinal Directions
                                                              * (To determine left or right precedence)
                                                              * Where [0] = (int)Input of [2], [1] = (int)Input of [3],
                                                              * [2] = Right, [3] = Left
                                                              * Where the value: -1 = physically pressed but release in code,
                                                              * 0 = released, 1 = pressed */
    private int[] _socdDownUp = new int[4] { 3, 4, 0, 0 };    // Input order: Crouch , Jump


    // Event System: (passes the inputs to the states)
    public delegate void InputEventHandler(object sender, InputValue input);
    public static event InputEventHandler OnInputEvent;

    protected virtual void Awake()
    {
        _playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
    }

    private void OnDisable()
    {
        _playerControls.Disable();
    }


    private void DoInput(InputValue input)
    {
        //DO INPUT assuming correct SOCD

        // Create new event
        if(OnInputEvent != null)
        {
            OnInputEvent(this, input);
        }
    }

    private void EvaluateInput(InputValue input, int[] socd = null)
    {
        if (socd == null)
        {
            DoInput(input);
        }
        else
        {
            LastInputControlledSocd(input, socd);
            //NeutralSocd(input, socd);
        }
    }

    // SOCD styles
    private void NeutralSocd(InputValue input, int[] socd)
    {
        // NOTE: Comments written for the specific case of right and left cleaning
        int firstInput = socd[0]; // right
        int secondInput = socd[1]; // left

        if ((int)input == firstInput) // If right is being pressed
        {
            if (socd[3] == 1) // If left is pressed (both right and left are pressed)
            {
                DoInput((InputValue)(-secondInput)); // if left is being held then unpress left

                // Unpress both right and left
                socd[2] = 0; socd[3] = 0;
            }
            else // left is unpressed (only right is being pressed)
            {
                socd[2] = 1; //set right to pressed
                DoInput(input); // press right
            }
        }
        else if ((int)input == -firstInput) // If right is being unpressed
        {
            if (socd[2] == 1) // if right was being held
            {
                socd[2] = 0; //set right to unpressed
                DoInput(input); // unpress right
            }
            // else, nothing else was being pressed since any form of simultaneous buttons would become neutral
        }
        else if ((int)input == secondInput) // If left is being pressed
        {
            if (socd[2] == 1) // If right is pressed (both right and left are pressed)
            {
                DoInput((InputValue)(-firstInput)); // if right is being held then unpress right

                // Unpress both right and left
                socd[2] = 0; socd[3] = 0;
            }
            else // right is unpressed (only left is being pressed)
            {
                socd[3] = 1; //set left to pressed
                DoInput(input); // press left
            }
        }
        else if ((int)input == -secondInput) // If left is being unpressed
        {
            if (socd[3] == 1) // if left was being held
            {
                socd[3] = 0; //set left to unpressed
                DoInput(input); // unpress left
            }
            // else, nothing else was being pressed since any form of simultaneous buttons would become neutral
        }
    }
    private void LastInputControlledSocd(InputValue input, int[] socd)
    {
        // NOTE: Comments written for the specific case of right and left cleaning
        int firstInput = socd[0]; // right
        int secondInput = socd[1]; // left

        if ((int)input == firstInput) // If right is being pressed
        {
            socd[2] = 1; //set right to pressed
            if (socd[3] == 1) // If left is pressed
            {
                DoInput((InputValue)(-secondInput)); // if left is being held then unpress left
                socd[3] = -1; //change status to physically held but unpressed
            }
            DoInput(input); // press right
        }
        else if ((int)input == -firstInput) // If right is being unpressed
        {
            socd[2] = 0; //set right to unpressed
            if (socd[3] == -1) // if direction was right
            {
                DoInput(input); //release right
                socd[3] = 1; // Set left to pressed
                DoInput((InputValue)secondInput); // press left
            }
            else if (socd[3] == 0) // if left was not pressed at all
            {
                DoInput(input); // simply release right (stop moving)
            }
            //if (socd[3] == 1) { } // if direction was left, do nothing as it has already been released
        }
        else if ((int)input == secondInput) // If left is being pressed
        {
            socd[3] = 1; // Set left to pressed
            if (socd[2] == 1) // If right is pressed
            {
                DoInput((InputValue)(-firstInput)); // Unpress right
                socd[2] = -1; // set right to held but unpressed
            }
            DoInput(input); // press left
        }
        else if ((int)input == -secondInput) // If left is being unpressed
        {
            socd[3] = 0; // Set left to unpressed
            if (socd[2] == -1) // If right is held but not pressed
            {
                DoInput(input); // Release left
                socd[2] = 1; // Set right to pressed
                DoInput((InputValue)firstInput); // Press right
            }
            else if (socd[2] == 0) //If right is unpressed
            {
                DoInput(input); // Unpress Left
            }
            //if (socd[2] == 1) { }
        }
    }
}
