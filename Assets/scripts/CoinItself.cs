using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinItself : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.tag == "player")
        {
            Debug.Log(hitInfo);
            Destroy(gameObject);
        }
    }
}
