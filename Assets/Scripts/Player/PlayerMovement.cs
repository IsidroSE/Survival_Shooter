using Assets.Scripts.Manager;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidbody;
    int floorMask;
    float camRayLength = 100f;
    public VirtualJoystick JoystickMove;
    public VirtualJoystick JoystickRotate;

    void Awake ()
    {
        anim = GetComponent<Animator>();
        playerRigidbody = GetComponent<Rigidbody>();
        floorMask = LayerMask.GetMask("Floor");
    }

    void FixedUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        //Move(h, v);

        float hJoystick = JoystickMove.Horizontal();
        float vJoystick = JoystickMove.Vertical();

        Move(h, v);
        Animating(h, v);

        //Turning();
        //Animating(h, v);

        Move(hJoystick, vJoystick);
        JoystickRotate.RotateTarget(transform);
        Animating(hJoystick, vJoystick);

    }

    void Move(float h, float v)
    {
        movement.Set(h, 0f, v);
        movement = movement.normalized * speed * Time.deltaTime;
        playerRigidbody.MovePosition(transform.position + movement);
    }

    void Turning()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit floorHit;
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            playerRigidbody.MoveRotation(newRotation);
        }
    }

    void Animating (float h, float v)
    {
        bool idle = (v == 0 && h == 0);
        anim.SetBool("IsWalking", !idle);
    }

}
