using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputValue
{
    NONE = 0,
    RIGHT_PRESS = 1,
    LEFT_PRESS = 2,
    DOWN_PRESS = 3,
    UP_PRESS = 4,
    LIGHT_PRESS = 5,
    MEDIUM_PRESS = 6,
    HEAVY_PRESS = 7,
    ENTER_PRESS = 8,
    RIGHT_RELEASE = -1,
    LEFT_RELEASE = -2,
    DOWN_RELEASE = -3,
    UP_RELEASE = -4,
    LIGHT_RELEASE = -5,
    MEDIUM_RELEASE = -6,
    HEAVY_RELEASE = -7,
    ENTER_RELEASE = -8
};

public static class InputHandler
{
    public static PlayerControls playerControls;

    public delegate void InputEventHandler(InputValue input, bool isHeld);
    public static event InputEventHandler OnInputEvent;

    public static void SendInputs()
    {
        if (playerControls.Player.Up.IsPressed())
        {
            if (playerControls.Player.Up.WasPressedThisFrame())
            {
                OnInputEvent(InputValue.UP_PRESS, false);
            }
            else
            {
                OnInputEvent(InputValue.UP_PRESS, true);
            }
        }
        else if (playerControls.Player.Up.WasReleasedThisFrame())
        {
            OnInputEvent(InputValue.UP_RELEASE, false);
        }

        if (playerControls.Player.Down.IsPressed())
        {
            if (playerControls.Player.Down.WasPressedThisFrame())
            {
                OnInputEvent(InputValue.DOWN_PRESS, false);
            }
            else
            {
                OnInputEvent(InputValue.DOWN_PRESS, true);
            }
        }
        else if (playerControls.Player.Down.WasReleasedThisFrame())
        {
            OnInputEvent(InputValue.DOWN_RELEASE, false);
        }

        if (playerControls.Player.Left.IsPressed())
        {
            if (playerControls.Player.Left.WasPressedThisFrame())
            {
                OnInputEvent(InputValue.LEFT_PRESS, false);
            }
            else
            {
                OnInputEvent(InputValue.LEFT_PRESS, true);
            }
        }
        else if (playerControls.Player.Left.WasReleasedThisFrame())
        {
            OnInputEvent(InputValue.LEFT_RELEASE, false);
        }

        if (playerControls.Player.Right.IsPressed())
        {
            if (playerControls.Player.Right.WasPressedThisFrame())
            {
                OnInputEvent(InputValue.RIGHT_PRESS, false);
            }
            else
            {
                OnInputEvent(InputValue.RIGHT_PRESS, true);
            }
        }
        else if (playerControls.Player.Right.WasReleasedThisFrame())
        {
            OnInputEvent(InputValue.RIGHT_RELEASE, false);
        }

        if (playerControls.Player.Enter.IsPressed())
        {
            if (playerControls.Player.Enter.WasPressedThisFrame())
            {
                OnInputEvent(InputValue.ENTER_PRESS, false);
            }
            else
            {
                OnInputEvent(InputValue.ENTER_PRESS, true);
            }
        }
        else if (playerControls.Player.Enter.WasReleasedThisFrame())
        {
            OnInputEvent(InputValue.ENTER_RELEASE, false);
        }

        if (playerControls.Player.LightAttack.IsPressed())
        {
            if (playerControls.Player.LightAttack.WasPressedThisFrame())
            {
                OnInputEvent(InputValue.LIGHT_PRESS, false);
            }
            else
            {
                OnInputEvent(InputValue.LIGHT_PRESS, true);
            }
        }
        else if (playerControls.Player.LightAttack.WasReleasedThisFrame())
        {
            OnInputEvent(InputValue.LIGHT_RELEASE, false);
        }

        if (playerControls.Player.MediumAttack.IsPressed())
        {
            if (playerControls.Player.MediumAttack.WasPressedThisFrame())
            {
                OnInputEvent(InputValue.MEDIUM_PRESS, false);
            }
            else
            {
                OnInputEvent(InputValue.MEDIUM_PRESS, true);
            }
        }
        else if (playerControls.Player.MediumAttack.WasReleasedThisFrame())
        {
            OnInputEvent(InputValue.MEDIUM_RELEASE, false);
        }

        if (playerControls.Player.HeavyAttack.IsPressed())
        {
            if (playerControls.Player.HeavyAttack.WasPressedThisFrame())
            {
                OnInputEvent(InputValue.HEAVY_PRESS, false);
            }
            else
            {
                OnInputEvent(InputValue.HEAVY_PRESS, true);
            }
        }
        else if (playerControls.Player.HeavyAttack.WasReleasedThisFrame())
        {
            OnInputEvent(InputValue.HEAVY_RELEASE, false);
        }
    }

    public static void Initialize()
    {
        playerControls = new PlayerControls();
        playerControls.Enable();
    }
}
