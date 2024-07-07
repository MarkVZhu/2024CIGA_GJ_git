using MarkFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    public int AddScore;
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
            GameModel.Instance.Score += AddScore;
            triggered = true;
        }

        Destroy(gameObject);
    }
}
