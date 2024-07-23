using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : Collectable
{

    /// <summary>
    /// Might be a negative value
    /// </summary>
    [SerializeField]
    int amount = 0;

    public int GetAmount()
    {
        return amount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerController>() != null)
        {
            other.gameObject.GetComponent<PlayerController>().ChangeMoney(amount);
        }

        Destroy(gameObject);
    }
}
