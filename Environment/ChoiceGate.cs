using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoiceGate : MonoBehaviour
{
    [SerializeField]
    GameObject goodChoice;
    [SerializeField]
    GameObject badChoice;

    private void Awake()
    {
        goodChoice.AddComponent<GoodChoice>();
        badChoice.AddComponent<BadChoice>();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
