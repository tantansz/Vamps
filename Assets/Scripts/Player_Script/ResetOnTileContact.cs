using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetOnTileContact : MonoBehaviour
{
    public Transform respawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Agua"))
        {
            Debug.Log("Ta funcionando");
            transform.position = respawnPoint.position;
        }
    }
}
