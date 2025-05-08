using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;

    [SerializeField] private float baseSpeed;
    private float speed;
    public bool canMove = true;

    private void OnSpeedChange(float scale)
    {
        speed = baseSpeed * scale;
    }

    private void Start()
    {
        speed = baseSpeed;
    }

    void Update()
    {
        if (canMove)
        {
            float deltaX = Input.GetAxis("Horizontal");
            float deltaZ = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(deltaX, 0, deltaZ);
            movement.Normalize();
            movement *= speed;

            rigidBody.MovePosition(transform.position + movement);
        }
    }
}
