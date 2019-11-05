using UnityEngine;

public class Player : MonoBehaviour
{
    string moveAxisName = "Vertical";
    string turnAxisName = "Horizontal";

    float moveSpeed = 10f;
    float turnSpeed = 240f;
    float moveAxis, turnAxis;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        moveAxis = Input.GetAxis(moveAxisName);
        turnAxis = Input.GetAxis(turnAxisName);

        if (Input.GetKeyDown(KeyCode.Mouse0))
            GetComponent<ShootBehaviour>().Shoot();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + transform.forward * moveAxis * moveSpeed * Time.deltaTime);
        Quaternion rotation = Quaternion.Euler(0, turnAxis * turnSpeed * Time.deltaTime, 0);
        rb.MoveRotation(transform.rotation * rotation);
    }
}
