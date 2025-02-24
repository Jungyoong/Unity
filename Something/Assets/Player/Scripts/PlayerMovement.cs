using UnityEngine;

public class PlayerMovement : Movement
{
    void FixedUpdate()
    {
        DragCalculation();
    }

    // Update is called once per frame
    void Update()
    {
        if (isUsing)
        {
            moveDirection = (transform.forward * Input.GetAxisRaw("Vertical")) + (transform.right * Input.GetAxisRaw("Horizontal"));
            moveDirection = moveDirection.normalized;

            if (Input.GetKeyDown(KeyCode.Space) && isUsing && Grounded)
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }


}
