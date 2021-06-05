using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMovement : MonoBehaviour
{
    public bool CanMove { get; set; }
    public float MovementSpeed = 2.0f;
    private Rigidbody m_rigibody;
    private void Awake()
    {
        m_rigibody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!CanMove)
        {
            return;
        }
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        float diagonalMultiplier = horizontal != 0 && vertical != 0 ? 0.75f : 1f;
        m_rigibody.velocity = new Vector3(horizontal, 0, vertical) * MovementSpeed * Time.fixedDeltaTime * diagonalMultiplier;
    }
}
