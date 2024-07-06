using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayerStates
{
    Idle,
    Moving,
}
public class Player : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D m_rigidbody;
    [SerializeField]
    private float m_maxVelocity;
    [SerializeField]
    private float m_acceleration;

    private PlayerStates m_curPlayerState;
    
    private void Start()
    {
        m_curPlayerState = PlayerStates.Moving;
        m_rigidbody = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        HandleMove();
    }
    private void HandleMove()
    {
        if (m_curPlayerState != PlayerStates.Moving)
        {
            return;
        }
        if(Mathf.Abs(m_rigidbody.velocity.x) > m_maxVelocity)
        {
            return;
        }
        m_rigidbody.AddForce(Vector2.right * m_acceleration * Time.deltaTime);
    }
}
