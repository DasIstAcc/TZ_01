using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class MoneySlider : MonoBehaviour
{
    Slider main;

    private void Awake()
    {
        main = GetComponent<Slider>();
        main.onValueChanged.AddListener(delegate { ValueChanged(); });
    }

    // Update is called once per frame
    void Update()
    {
        main.value = GameManager.Default.GetCurrentMoneyPercent();
    }

    public void ValueChanged()
    {
        if (main.value < 0.35f)
        {
            main.fillRect.gameObject.GetComponent<Image>().color = Color.red;
        }
        else if (main.value < 0.6f)
        {
            main.fillRect.gameObject.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            main.fillRect.gameObject.GetComponent<Image>().color = Color.green;
        }
    }
}
