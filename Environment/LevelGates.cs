using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelGates : MonoBehaviour
{
    [SerializeField]
    TextMeshPro ScoreNeededText;

    [SerializeField]
    int ScoreNeeded = 100;

    [SerializeField]
    GameObject levelStopTrigger;
    

    // Start is called before the first frame update
    void Start()
    {
        ScoreNeededText.text = "Need\n" + ScoreNeeded;
        levelStopTrigger.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            int money = other.GetComponent<PlayerController>().GetCurrMoney();
            if (money >= ScoreNeeded)
            {
                GetComponentInChildren<Animator>().SetTrigger("Open");
                levelStopTrigger.SetActive(false);
            }
        }
    }
}
