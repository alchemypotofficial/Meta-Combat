using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Metacombat : Game<Metacombat>
{
    [SerializeField] private bool quickStart;

    [SerializeField] private GameObject fade;
    [SerializeField] private Image demoImage;
    [SerializeField] public MenuTitleScreen menuTitleScreen;

    public static bool IsQuickStart;

    public override void PreInit()
    {
        LeanTween.reset();

        IsQuickStart = quickStart;
    }

    public override void Init()
    {
        InputHandler.Initialize();
    }

    public override void PostInit()
    {
        StartCoroutine(BlinkDemo());

        if (IsQuickStart)
        {
            menuTitleScreen.HideMenu();
            GameHandler.LoadStage("City_Hall");
        }
        else
        {
            menuTitleScreen.LoadMenu();
        }
    }

    private void Update()
    {
        InputHandler.SendInputs();
    }

    public static void FadeOut()
    {
        instance.StartCoroutine(instance.FadeOutBlack());
    }

    public static void FadeIn()
    {
        instance.StartCoroutine(instance.FadeInBlack());
    }

    private IEnumerator FadeOutBlack()
    {
        Color objectColor = fade.GetComponent<Image>().color;
        float fadeAmount;

        while (objectColor.a < 1)
        {
            fadeAmount = objectColor.a + (1f * Time.deltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            fade.GetComponent<Image>().color = objectColor;

            yield return null;
        }

        yield return null;
    }

    private IEnumerator FadeInBlack()
    {
        Color objectColor = fade.GetComponent<Image>().color;
        float fadeAmount;

        while (fade.GetComponent<Image>().color.a > 0)
        {
            fadeAmount = objectColor.a - (1f * Time.deltaTime);

            objectColor = new Color(objectColor.r, objectColor.g, objectColor.b, fadeAmount);
            fade.GetComponent<Image>().color = objectColor;

            yield return null;
        }

        yield return null;
    }

    public IEnumerator BlinkDemo()
    {
        while (true)
        {
            if (demoImage != null)
            {
                demoImage.enabled = !demoImage.enabled;
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
}
