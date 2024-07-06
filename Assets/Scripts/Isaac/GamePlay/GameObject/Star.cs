using MarkFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public GameModel model;
    private bool triggered;
    private void Start()
    {
        triggered = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!triggered)
        {
            GameModel.Instance.StarCount++;
            triggered = true;
        }

        Destroy(gameObject);
    }
}
