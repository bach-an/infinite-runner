using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    [SerializeField] private Vector2 sensitivity;
    [SerializeField] private GameObject player;
    [SerializeField] private Vector2 acceleration;
    // period to wait until resetting input val
    [SerializeField] private float inputLagPeriod;

    // clamp rotation between -90 and 90
    private Vector2 velocity; // current velocity
    private Vector2 rotation; // current rotation
    private Vector2 lastInputEvent; // last received non-zero input value
    private float inputLagTimer; // time since last received non-zero input value

    private Vector2 GetInput()
    {
        inputLagTimer += Time.deltaTime;
        Vector2 input =
            new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        // if neither axes of the input are close to 0 or if timer is greater
        // than lag period (user not giving any input)
        if ((Mathf.Approximately(0, input.x) &&
            Mathf.Approximately(0, input.y)) == false
            || inputLagTimer >= inputLagPeriod)
        {
            // set the last input event to input (even if 0)
            lastInputEvent = input;
            inputLagTimer = 0; // reset timer
        }

        return lastInputEvent;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 wantedVelocity = GetInput() * sensitivity;
        velocity = new Vector2(
            // current value, desired value, max change to apply 
            Mathf.MoveTowards(velocity.x, wantedVelocity.x, acceleration.x * Time.deltaTime),
            Mathf.MoveTowards(velocity.y, wantedVelocity.y, acceleration.y * Time.deltaTime));

        rotation += velocity * Time.deltaTime;
        rotation.y = Mathf.Clamp(rotation.y, -90f, 90f);
        transform.localEulerAngles = new Vector3(rotation.y, 0, 0);

        // remove one axis of mouse look
        //player.transform.localEulerAngles = new Vector3(0, rotation.x, 0);
    }
}
