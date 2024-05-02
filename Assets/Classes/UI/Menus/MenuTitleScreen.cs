using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Xml;

public class MenuTitleScreen : Menu
{
    [SerializeField] private GameObject logo;
    [SerializeField] private GameObject tagline;
    [SerializeField] private Image pressEnterImage;
    [SerializeField] private GameObject leftFighter;
    [SerializeField] private GameObject rightFighter;
    [SerializeField] private UIAnimationController leftFighterAnimator;
    [SerializeField] private UIAnimationController rightFighterAnimator;

    private bool performInput;

    public override void OnInputEvent(InputValue inputValue, bool isHeld)
    {
        if (performInput)
        {
            if (inputValue == InputValue.ENTER_PRESS && !isHeld)
            {
                StartCoroutine(LoadStage());
            }

            if (inputValue == InputValue.DOWN_PRESS)
            {

            }
        }
    }

    public override void LoadMenu()
    {
        StartCoroutine("TitleScreenLoad");
    }

    public override void ExitMenu()
    {
        
    }

    private IEnumerator LoadStage()
    {
        AudioHandler.PlaySound("Confirm");
        Log.Alert("Enter has been pressed.");

        performInput = false;

        AudioHandler.FadeOutMusic();

        Metacombat.FadeOut();

        yield return new WaitForSeconds(2f);

        GameHandler.LoadStage("City_Hall");
        HideMenu();

        yield return null;
    }

    private IEnumerator TitleScreenLoad()
    {
        AudioHandler.PlayMusic("TitleScreen_Intro");

        LeanTween.moveLocalY(logo, 180, 1f).setEaseOutBack();

        yield return new WaitForSeconds(3.5f);

        LeanTween.moveLocalY(tagline, 90, 0.5f).setEaseOutBack();

        yield return new WaitForSeconds(0.25f);

        LeanTween.moveLocalY(logo, 220, 0.25f).setEaseOutBack();

        yield return new WaitForSeconds(0.25f);

        LeanTween.moveLocalY(logo, 180, 0.25f).setEaseOutBack();

        yield return new WaitForSeconds(0.75f);

        AudioHandler.PlayMusic("TitleScreen_Loop");

        LeanTween.moveLocalX(leftFighter, -375f, 0.75f).setEaseOutBack();
        LeanTween.moveLocalX(rightFighter, 375f, 0.75f).setEaseOutBack();

        leftFighterAnimator.StartAnimation("Fighter_Idle", true);
        rightFighterAnimator.StartAnimation("Fighter_Idle", true);

        StartCoroutine("BlinkPressEnter");

        performInput = true;

        yield return null;
    }

    private IEnumerator BlinkPressEnter()
    {
        while (true)
        {
            if (pressEnterImage != null)
            {
                pressEnterImage.enabled = !pressEnterImage.enabled;
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
