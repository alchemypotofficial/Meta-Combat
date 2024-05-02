using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : Singleton<GameHandler>
{
    [SerializeField] private CameraHandler cameraHandler;
    [SerializeField] private GameObject fightingUI;
    [SerializeField] private GameObject playerFighter;
    [SerializeField] private GameObject npcFighter;
    [SerializeField] private Vector3 leftFighterSpawn;
    [SerializeField] private Vector3 rightFighterSpawn;
    [SerializeField] private Stage[] stages;
    [SerializeField] private Image leftFighterHealthImage;
    [SerializeField] private Image rightFighterHealthImage;
    [SerializeField] private Sprite[] numbers;
    [SerializeField] public MenuTitleScreen menuTitleScreen;

    private GameObject leftFighter;
    private FighterController leftFighterController;
    private int leftFighterWins = 0;
    private float leftFighterHealthValue;
    [SerializeField] private Image leftFighterCounter1;
    [SerializeField] private Image leftFighterCounter2;

    [SerializeField] private Image timerTensImage;
    [SerializeField] private Image timerOnesImage;
    private int timerTens = 6;
    private int timerOnes = 0;

    private GameObject rightFighter;
    private FighterController rightFighterController;
    private int rightFighterWins = 0;
    private float rightFighterHealthValue;
    [SerializeField] private Image rightFighterCounter1;
    [SerializeField] private Image rightFighterCounter2;

    private Stage currentStage;
    private bool roundStarted;
    private int round = 1;

    private Coroutine timerCoroutine;

    private void Update()
    {
        if (roundStarted)
        {
            UpdateFacing();
            PushFighters();

            if (leftFighterController.isDead)
            {
                StopCoroutine(timerCoroutine);

                rightFighterWins++;

                UpdateWinCounter();

                if (rightFighterWins == 2)
                {
                    EndMatch();
                }
                else
                {
                    StartNextRound();
                }
            }
            else if (rightFighterController.isDead)
            {
                StopCoroutine(timerCoroutine);

                leftFighterWins++;

                UpdateWinCounter();

                if (leftFighterWins == 2)
                {
                    EndMatch();
                }
                else
                {
                    StartNextRound();
                }
            }
            else if (timerTens == 0 && timerOnes == 0)
            {
                StopCoroutine(timerCoroutine);

                if (leftFighterController.health > rightFighterController.health)
                {
                    leftFighterWins++;

                    UpdateWinCounter();

                    if (leftFighterWins == 2)
                    {
                        EndMatch();
                    }
                    else
                    {
                        StartNextRound();
                    }
                }
                else if (leftFighterController.health > rightFighterController.health)
                {
                    rightFighterWins++;

                    UpdateWinCounter();

                    if (rightFighterWins == 2)
                    {
                        EndMatch();
                    }
                    else
                    {
                        StartNextRound();
                    }
                }
                else
                {
                    StartNextRound();
                }
            }
        }

        if (leftFighterController != null && rightFighterController != null)
        {
            leftFighterHealthValue = Mathf.Lerp((float)leftFighterHealthValue, (float)leftFighterController.health, 0.2f);
            rightFighterHealthValue = Mathf.Lerp((float)rightFighterHealthValue, (float)rightFighterController.health, 0.2f);

            float normalLeftFighterHealth = (leftFighterHealthValue / 100);
            float normalRightFighterHealth = (rightFighterHealthValue / 100);

            leftFighterHealthImage.fillAmount = normalLeftFighterHealth;
            rightFighterHealthImage.fillAmount = normalRightFighterHealth;
        }
    }

    public static void LoadStage(string name)
    {
        foreach (Stage stage in instance.stages)
        {
            if (stage.stageName == name)
            {
                instance.InitializeStage(stage);

                return;
            }
        }

        Log.Error("Stage \"" + name + "\" could not be found.");
    }

    public void UpdateWinCounter()
    {
        if (leftFighterWins == 1)
        {
            leftFighterCounter1.enabled = true;
            leftFighterCounter2.enabled = false;
        }
        else if (leftFighterWins == 2)
        {
            leftFighterCounter1.enabled = true;
            leftFighterCounter2.enabled = true;
        }
        else
        {
            leftFighterCounter1.enabled = false;
            leftFighterCounter2.enabled = false;
        }

        if (rightFighterWins == 1)
        {
            rightFighterCounter1.enabled = true;
            rightFighterCounter2.enabled = false;
        }
        else if (rightFighterWins == 2)
        {
            rightFighterCounter1.enabled = true;
            rightFighterCounter2.enabled = true;
        }
        else
        {
            rightFighterCounter1.enabled = false;
            rightFighterCounter2.enabled = false;
        }
    }

    public GameObject LoadPlayerFighter()
    {
        return Instantiate(playerFighter);
    }

    public GameObject LoadNPCFighter()
    {
        return Instantiate(npcFighter);
    }

    private void UpdateFacing()
    {
        if (leftFighterController.IsOnGround())
        {
            if ((leftFighter.transform.position - rightFighter.transform.position).x > 0)
            {
                leftFighterController.isFacingRight = false;
                leftFighter.transform.rotation = new Quaternion(0, 180, 0, 0);
            }
            else
            {
                leftFighterController.isFacingRight = true;
                leftFighter.transform.rotation = new Quaternion(0, 0, 0, 0);
            }
        }

        if (rightFighterController.IsOnGround())
        {
            if ((leftFighter.transform.position - rightFighter.transform.position).x > 0)
            {
                rightFighterController.isFacingRight = true;
                rightFighter.transform.rotation = new Quaternion(0, 0, 0, 0);
            }
            else
            {
                rightFighterController.isFacingRight = false;
                rightFighter.transform.rotation = new Quaternion(0, 180, 0, 0);
            }
        }
    }

    private void PushFighters()
    {
        if (!leftFighterController.isJumping && !rightFighterController.isJumping)
        {
            if (leftFighterController.isFacingRight && leftFighter.transform.position.x > rightFighter.transform.position.x - 0.75f)
            {
                Vector3 leftFighterPosition = leftFighter.transform.position;

                leftFighter.transform.position = new Vector3(rightFighter.transform.position.x - 0.75f, leftFighterPosition.y, leftFighterPosition.z);
            }
            else if (!leftFighterController.isFacingRight && leftFighter.transform.position.x < rightFighter.transform.position.x + 0.75f)
            {
                Vector3 leftFighterPosition = leftFighter.transform.position;

                leftFighter.transform.position = new Vector3(rightFighter.transform.position.x + 0.75f, leftFighterPosition.y, leftFighterPosition.z);
            }
        }

        if (!rightFighterController.isJumping && !leftFighterController.isJumping)
        {
            if (rightFighterController.isFacingRight && rightFighter.transform.position.x > leftFighter.transform.position.x - 0.75f)
            {
                Vector3 rightFighterPosition = rightFighter.transform.position;

                rightFighter.transform.position = new Vector3(leftFighter.transform.position.x - 0.75f, rightFighterPosition.y, rightFighterPosition.z);
            }
            else if (!rightFighterController.isFacingRight && rightFighter.transform.position.x < leftFighter.transform.position.x + 0.75f)
            {
                Vector3 rightFighterPosition = rightFighter.transform.position;

                rightFighter.transform.position = new Vector3(leftFighter.transform.position.x + 0.75f, rightFighterPosition.y, rightFighterPosition.z);
            }
        }
    }

    private void InitializeStage(Stage stage)
    {
        StartCoroutine(InitStage(stage));
    }

    private void StartNextRound()
    {
        StartCoroutine(NextRound());
    }

    private void EndMatch()
    {
        StartCoroutine(EndFight());
    }

    private IEnumerator EndFight()
    {
        instance.roundStarted = false;
        instance.leftFighterController.isControllable = false;
        instance.rightFighterController.isControllable = false;

        while (true)
        {
            if (leftFighterController.IsOnGround() && rightFighterController.IsOnGround())
            {
                break;
            }

            yield return null;
        }

        yield return new WaitForSeconds(3f);

        AudioHandler.PlaySound("Announcer_Victory");

        yield return new WaitForSeconds(3f);

        AudioHandler.FadeOutMusic();

        Metacombat.FadeOut();

        yield return new WaitForSeconds(2f);

        Destroy(currentStage.gameObject);
        Destroy(leftFighter);
        Destroy(rightFighter);

        fightingUI.SetActive(false);

        menuTitleScreen.ShowMenu();

        Metacombat.FadeIn();

        yield return new WaitForSeconds(2f);

        menuTitleScreen.LoadMenu();

        yield return null;
    }

    private IEnumerator NextRound()
    {
        instance.roundStarted = false;
        instance.leftFighterController.isControllable = false;
        instance.rightFighterController.isControllable = false;

        while (true)
        {
            if (leftFighterController.IsOnGround() && rightFighterController.IsOnGround())
            {
                break;
            }

            yield return null;
        }

        round++;

        yield return new WaitForSeconds(1.25f);

        AudioHandler.PlaySound("Announcer_Winner");

        yield return new WaitForSeconds(3f);

        Metacombat.FadeOut();

        yield return new WaitForSeconds(2f);

        timerTens = 6;
        timerOnes = 0;
        timerTensImage.sprite = GetNumber(timerTens);
        timerOnesImage.sprite = GetNumber(timerOnes);

        instance.leftFighter.transform.position = instance.leftFighterSpawn;
        instance.leftFighterController.isFacingRight = true;
        instance.leftFighterController.health = instance.leftFighterController.maxHealth;
        instance.leftFighterController.stun = 0;
        instance.leftFighterController.isDead = false;
        instance.leftFighterController.SetStartingState();

        instance.rightFighter.transform.position = instance.rightFighterSpawn;
        instance.rightFighterController.isFacingRight = false;
        instance.rightFighterController.health = instance.rightFighterController.maxHealth;
        instance.rightFighterController.stun = 0;
        instance.rightFighterController.isDead = false;
        instance.rightFighterController.SetStartingState();

        instance.UpdateFacing();

        Metacombat.FadeIn();

        if (!Metacombat.IsQuickStart)
        {
            yield return new WaitForSeconds(2f);

            if (round == 1)
            {
                AudioHandler.PlaySound("Announcer_Round_One");
            }
            else if (round == 2)
            {
                AudioHandler.PlaySound("Announcer_Round_Two");
            }
            else if (round == 3)
            {
                AudioHandler.PlaySound("Announcer_Round_Final");
            }

            yield return new WaitForSeconds(1.75f);

            AudioHandler.PlaySound("Announcer_Fight");

            yield return new WaitForSeconds(0.75f);
        }

        instance.roundStarted = true;
        instance.leftFighterController.isControllable = true;
        instance.rightFighterController.isControllable = true;

        timerCoroutine = StartCoroutine(CountDownTimer());

        yield return null;
    }

    private IEnumerator InitStage(Stage stage)
    {
        fightingUI.SetActive(true);

        GameObject stageObject = Instantiate(stage.gameObject, Metacombat.instance.transform);
        instance.currentStage = stageObject.GetComponent<Stage>();
        instance.currentStage.transform.parent = transform;

        instance.leftFighter = instance.LoadPlayerFighter();
        instance.leftFighter.transform.parent = transform;
        instance.leftFighterController = instance.leftFighter.GetComponent<FighterController>();
        instance.leftFighter.transform.position = instance.leftFighterSpawn;
        instance.leftFighterController.isFacingRight = true;

        instance.rightFighter = instance.LoadNPCFighter();
        instance.rightFighter.transform.parent = transform;
        instance.rightFighter.transform.position = instance.rightFighterSpawn;
        instance.rightFighterController = instance.rightFighter.GetComponent<FighterController>();
        instance.rightFighterController.isFacingRight = false;

        instance.UpdateFacing();

        instance.cameraHandler.transform1 = instance.leftFighter.transform;
        instance.cameraHandler.transform2 = instance.rightFighter.transform;

        timerTens = 6;
        timerOnes = 0;
        timerTensImage.sprite = GetNumber(timerTens);
        timerOnesImage.sprite = GetNumber(timerOnes);

        AudioHandler.PlayMusic(stage.stageMusic);

        Metacombat.FadeIn();

        if (!Metacombat.IsQuickStart)
        {
            yield return new WaitForSeconds(2f);

            if (round == 1)
            {
                AudioHandler.PlaySound("Announcer_Round_One");
            }
            else if (round == 2)
            {
                AudioHandler.PlaySound("Announcer_Round_Two");
            }
            else if (round == 3)
            {
                AudioHandler.PlaySound("Announcer_Round_Three");
            }
            else if (round == 4)
            {
                AudioHandler.PlaySound("Announcer_Round_Final");
            }

            yield return new WaitForSeconds(1.75f);

            AudioHandler.PlaySound("Announcer_Fight");

            yield return new WaitForSeconds(0.75f);
        }

        instance.roundStarted = true;
        instance.leftFighterController.isControllable = true;
        instance.rightFighterController.isControllable = true;

        timerCoroutine = StartCoroutine(CountDownTimer());

        yield return null;
    }

    private IEnumerator CountDownTimer()
    {
        while (true)
        {
            if (timerOnes == 0)
            {
                if (timerTens > 0)
                {
                    timerTens--;
                    timerOnes = 9;
                }
            }
            else
            {
                timerOnes--;
            }

            if (timerTens == 0 && timerOnes == 0)
            {
                break;
            }

            timerTensImage.sprite = GetNumber(timerTens);
            timerOnesImage.sprite = GetNumber(timerOnes);

            yield return new WaitForSeconds(1f);
        }

        yield return null;
    }

    private Sprite GetNumber(int number)
    {
        return numbers[number];
    }

    public static FighterController GetLeftFighter()
    {
        return instance.leftFighterController;
    }

    public static FighterController GetRightFighter()
    {
        return instance.rightFighterController;
    }
}
