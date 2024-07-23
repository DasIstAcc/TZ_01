using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float roadPosition = 0.5f;

    [SerializeField]
    GameObject playerPrefab;

    [SerializeField]
    List<GameObject> appearances;

    [SerializeField]
    int change_threshold = 20;
    [SerializeField]
    int max_money = 200;
    int curr_money = 0;

    string[] wealths = new string[] { "poor", "casual", "middle", "buisiness", "rich", "millionare" };

    void Start()
    {
        foreach (var appearance in appearances)
        {
            appearance.SetActive(false);
        }
        appearances[0].SetActive(true);
    }



    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            roadPosition -= 0.5f * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D))
        {
            roadPosition += 0.5f * Time.deltaTime;
        }

        if (roadPosition > 1) roadPosition = 1;
        if (roadPosition < 0) roadPosition = 0;

        playerPrefab.transform.localPosition = new Vector3((roadPosition - 0.5f) * 4, 0, 0);
        GetComponent<BoxCollider>().center = playerPrefab.transform.localPosition;
    }

    public void Reset()
    {
        curr_money = 0;
        roadPosition = 0.5f;
        foreach (var appearance in appearances)
        {
            appearance.SetActive(false);
        }
        appearances[0].SetActive(true);
    }


    /// <summary>
    /// Accepts negative values
    /// </summary>
    /// <param name="amount"> Might be negative </param>
    public void ChangeMoney(int amount)
    {
        MoneyChanged();
        GameManager.Default.AddToPlusCounter(amount);
        if (curr_money + amount > max_money) { amount = max_money - curr_money; }
        if (curr_money + amount < 0) { curr_money = 0; amount = 0; }
        MoneyCheck(amount);
        curr_money += amount;
        GameManager.Default.SetupWealth(wealths[GetCurrLevel()]);
        if (curr_money == 0) { curr_money = 0; GameManager.Default.Defeat(); }
        
    }

    public int GetCurrMoney()
    {
        return curr_money;
    }

    public float GetMoneyPercentage()
    {
        return (float)curr_money / max_money;
    }

    public int GetThreshold()
    {
        return change_threshold;
    }

    /// <summary>
    /// Can be replaced with any desired function
    /// </summary>
    /// <returns></returns>
    private int GetCurrThreshold()
    {
        return (int)(curr_money / change_threshold) * change_threshold;
    }

    private int GetCurrLevel()
    {
        return (int)GetCurrThreshold() / change_threshold;
    }

    private int GetNextThreshold()
    {
        return (int)(curr_money / change_threshold + 1) * change_threshold;
    }

    private void MoneyCheck(int amount)
    {
        if (curr_money + amount < GetCurrThreshold())
        {
            GetComponentInChildren<Animator>().ResetTrigger("Downed");
            GetComponentInChildren<Animator>().SetTrigger("Downed");
            ChangeAppearance(GetCurrLevel() - 1);
        }
        if (curr_money + amount >= GetNextThreshold())
        {
            GetComponentInChildren<Animator>().ResetTrigger("Spin");
            GetComponentInChildren<Animator>().SetTrigger("Spin");
            ChangeAppearance(GetCurrLevel() + 1);
        }
    }

    private void ChangeAppearance(int type)
    {
        foreach (var appearance in appearances)
        {
            appearance.SetActive(false);
        }
        if (type > appearances.Count - 1)
            appearances[appearances.Count - 1].SetActive(true);
        else if (type > 0)
            appearances[type].SetActive(true);
        else
            appearances[0].SetActive(true);
    }

    #region Events
    public static event Action MoneyChanged = delegate { };



    #endregion
}
