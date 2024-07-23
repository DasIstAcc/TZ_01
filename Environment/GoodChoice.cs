using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GoodChoice : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            other.gameObject.GetComponent<PlayerController>().ChangeMoney(other.gameObject.GetComponent<PlayerController>().GetThreshold());
        }
    }
}
