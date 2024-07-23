using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Collectable : MonoBehaviour
{
    [SerializeField]
    GameObject m_Prefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Spin();
    }

    void Spin()
    {
        m_Prefab.transform.eulerAngles += new Vector3(0, 0.1f, 0);
    }
}
