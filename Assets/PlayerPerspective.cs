using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPerspective : MonoBehaviour
{
    [SerializeField] private Transform camera;
    [SerializeField] private CharacterController playerController;

    // Update is called once per frame
    void Update()
    {
        // rotate by y 180
        float playerZ = camera.localPosition.z;
        float playerX = camera.localPosition.x;
        float playerY = camera.localPosition.y;

        if (Input.GetKeyDown("p"))
        {

            camera.localPosition = new Vector3(playerX + 5, playerY, playerZ);
            camera.localEulerAngles -= new Vector3(0, 90, 0);
        }
    }
}
