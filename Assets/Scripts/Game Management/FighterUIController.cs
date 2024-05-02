using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterUIController : MonoBehaviour
{
    [SerializeField] private GameObject fighter1UI;
    [SerializeField] private GameObject fighter2UI;

    public FighterController fighter1;
    public FighterController fighter2;

    private float fighter1HealthValue;
    private float fighter2HealthValue;

    private Image healthImage1;
    private Image healthImage2;

    private void Start()
    {
        healthImage1 = fighter1UI.transform.GetChild(1).GetComponent<Image>();
        healthImage2 = fighter2UI.transform.GetChild(1).GetComponent<Image>();
    }

    public void SetupUI()
    {
        fighter1HealthValue = fighter1.maxHealth;
        fighter2HealthValue = fighter2.maxHealth;
    }

    public void Update()
    {
        if (fighter1 != null && fighter2 != null)
        {
            fighter1HealthValue = Mathf.Lerp(fighter1HealthValue, fighter1.health, 0.025f);
            fighter2HealthValue = Mathf.Lerp(fighter2HealthValue, fighter2.health, 0.025f);

            float normalFighter1Health = (fighter1HealthValue / 100);
            float normalFighter2Health = (fighter2HealthValue / 100);

            healthImage1.fillAmount = normalFighter1Health;
            healthImage2.fillAmount = normalFighter2Health;
        }
    }
}
