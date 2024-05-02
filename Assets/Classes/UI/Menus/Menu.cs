using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Menu : MonoBehaviour
{
    public abstract void LoadMenu();
    public abstract void ExitMenu();
    public abstract void OnInputEvent(InputValue inputValue, bool isHeld);

    private void OnEnable()
    {
        InputHandler.OnInputEvent += OnInputEvent;
    }

    private void OnDisable()
    {
        InputHandler.OnInputEvent -= OnInputEvent;
    }

    public void ShowMenu()
    {
        gameObject.SetActive(true);
        InputHandler.OnInputEvent += OnInputEvent;
    }

    public void HideMenu()
    {
        InputHandler.OnInputEvent -= OnInputEvent;
        gameObject.SetActive(false);
    }
}
