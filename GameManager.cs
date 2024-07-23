using ButchersGames;
using PathCreation;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Singletone
    private static GameManager _default;
    public static GameManager Default { get => _default; }
    public GameManager() => _default = this;
    #endregion

    
    [SerializeField]
    PlayerController player;

    bool is_playing = false;

    // Start is called before the first frame update
    void Start()
    {
        DisableUI();
        DefeatScreen.SetActive(false);
        VictoryScreen.SetActive(false);
        PlayerController.MoneyChanged += ShowPlusScore;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (is_playing)
        {
            if (pathCreator != null)
                MovePlayer();
            else 
                NoPathLevel();
            UpdateUI();
        }
    }

    #region LevelManagement

    public void StartGame()
    {
        LevelManager.Default.Init();
        is_playing = true;
        player.ChangeMoney(10);
        EnableGameUI();
    }

    private void NoPathLevel()
    {
        Debug.Log("There is no path!");
        player.GetComponentInChildren<Animator>().SetTrigger("Spin");
    }

    public void Defeat()
    {
        ShowDefeatScreen();
        is_playing = false;
        distanceTravelled = 0;
        player.GetComponentInChildren<Animator>().SetFloat("Speed", 0);
    }

    public void LevelComplete()
    {
        ShowVictoryScreen();
        is_playing = false;
        distanceTravelled = 0;
        player.GetComponentInChildren<Animator>().SetFloat("Speed", 0);
        player.GetComponentInChildren<Animator>().SetTrigger("Spin");
    }


    public void NextLevel()
    {
        player.Reset();
        LevelManager.Default.NextLevel();
        is_playing = true;
        player.ChangeMoney(10);
        EnableGameUI();
    }

    public void RetryLevel()
    {
        player.Reset();
        LevelManager.Default.RestartLevel();
        is_playing = true;
        player.ChangeMoney(10);
        EnableGameUI();
    }

    #endregion

    public float GetCurrentMoneyPercent()
    {
        return player.GetMoneyPercentage();
    }

    #region Movement

    PathCreator pathCreator;

    public void SetupPathCreator(PathCreator pathCreator)
    {
        this.pathCreator = pathCreator;
    }

    [SerializeField]
    float max_speed = 4;

    float distanceTravelled = 0;
    float speed = 0;

    void MovePlayer()
    {
        speed++;
        if (speed > max_speed) speed = max_speed;

        distanceTravelled += speed * Time.deltaTime;

        player.transform.rotation = pathCreator.path.GetRotationAtDistance(distanceTravelled);
        player.transform.position = pathCreator.path.GetPointAtDistance(distanceTravelled);
        player.transform.rotation = Quaternion.Euler(player.transform.rotation.eulerAngles.x, player.transform.rotation.eulerAngles.y, player.transform.rotation.eulerAngles.z + 90f);


        player.GetComponentInChildren<Animator>().SetFloat("Speed", speed / max_speed);
    }


    #endregion

    #region UI

    [SerializeField]
    GameObject UICanvas;
    [SerializeField]
    GameObject MenuCanvas;

    [SerializeField]
    TextMeshProUGUI levelText;
    [SerializeField]
    TextMeshProUGUI ScoreText;
    [SerializeField]
    TextMeshProUGUI PlusScoreText;
    [SerializeField]
    TextMeshProUGUI WealthText;

    [SerializeField]
    GameObject DefeatScreen;
    [SerializeField]
    Image redScreen;
    [SerializeField]
    Image DefeatText;
    [SerializeField]
    GameObject VictoryScreen;
    [SerializeField]
    Image VictoryText;

    private void EnableGameUI()
    {
        UICanvas.SetActive(true);
        MenuCanvas.SetActive(false);
        DefeatScreen.SetActive(false);
        VictoryScreen.SetActive(false);
    }

    private void DisableUI()
    {
        UICanvas.SetActive(false);
    }

    private void UpdateUI()
    {
        levelText.text = "Level " + LevelManager.Default.CurrentLevelIndex;
        ScoreText.text = player.GetCurrMoney().ToString();
        if (plus_counter > 0)
        {
            PlusScoreText.color = Color.green;
            PlusScoreText.text = "+" + plus_counter.ToString();
        }
        else
        {
            PlusScoreText.color = Color.red;
            PlusScoreText.text = plus_counter.ToString();
        }
        if (fade_counter > 0) fade_counter--;
        else { PlusScoreText.gameObject.SetActive(false); plus_counter = 0; }
    }

    int fade_counter = 0;
    int plus_counter = 0;

    public void AddToPlusCounter(int amount)
    {
        plus_counter += amount;
    }

    public void SetupWealth(string text)
    {
        WealthText.text = text;
    }

    private void ShowPlusScore()
    {
        PlusScoreText.gameObject.SetActive(true);
        fade_counter = 120;
        StartCoroutine(Pulse());
    }

    private IEnumerator Pulse()
    {
        for (float i = 1; i <= 1.2f; i += 0.05f)
        {
            PlusScoreText.rectTransform.localScale = new Vector3(i, i, i);
            ScoreText.rectTransform.localScale = new Vector3(i,i,i);
            yield return new WaitForEndOfFrame();
        }

        PlusScoreText.rectTransform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        ScoreText.rectTransform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

        for (float i = 1.2f; i >= 1; i -= 0.05f)
        {
            PlusScoreText.rectTransform.localScale = new Vector3(i, i, i);
            ScoreText.rectTransform.localScale = new Vector3(i, i, i);
            yield return new WaitForEndOfFrame();
        }

        PlusScoreText.rectTransform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        ScoreText.rectTransform.localScale = new Vector3(1f, 1f, 1f);
    }

    private void ShowDefeatScreen() 
    {
        DisableUI();
        DefeatScreen.SetActive(true);
        StartCoroutine(TurnScreenRed());
        StartCoroutine(PopDefeatText());
    }

    private void ShowVictoryScreen()
    {
        DisableUI();
        VictoryScreen.SetActive(true);
        StartCoroutine(PopVictoryText());
    }

    private IEnumerator TurnScreenRed()
    {
        redScreen.rectTransform.anchoredPosition = new Vector3(0, 250);
        redScreen.rectTransform.sizeDelta = new Vector2(redScreen.rectTransform.sizeDelta.x, 0);

        for (int i = 0; i < 100; i++)
        {
            redScreen.rectTransform.anchoredPosition -= new Vector2(0, 2.5f);
            redScreen.rectTransform.sizeDelta += new Vector2(0, 5);
            //redScreen.rectTransform;m_SizeDelta.xm_AnchoredPosition.y

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator PopDefeatText()
    {
        for (float i = 0; i < 1; i += 0.005f)
        {
            DefeatText.rectTransform.localScale = new Vector3(i, i);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator PopVictoryText()
    {
        for (float i = 0; i < 1; i += 0.005f)
        {
            VictoryText.rectTransform.localScale = new Vector3(i, i);
            yield return new WaitForEndOfFrame();
        }
    }

    #endregion

}
