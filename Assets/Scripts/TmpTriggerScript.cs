using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TmpTriggerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject pieces;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            pieces.SetActive(true);
        }
    }
}
