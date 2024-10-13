using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed of movement

    void Update()
    {
        // Get input from horizontal (A/D or Left/Right arrow keys) and vertical (W/S or Up/Down arrow keys)
        float horizontalInput = Input.GetAxis("Horizontal"); // Left (-1) to Right (+1)
        float verticalInput = Input.GetAxis("Vertical");     // Backward (-1) to Forward (+1)

        // Calculate movement direction
        Vector3 movement = new Vector3(horizontalInput, 0, verticalInput) * moveSpeed * Time.deltaTime;

        // Move the player in the desired direction
        transform.Translate(movement);
    }
}
