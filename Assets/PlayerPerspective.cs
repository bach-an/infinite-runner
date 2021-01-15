using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPerspective : MonoBehaviour
{
    [SerializeField] private Transform cameraParent;
    private float cameraDistance = 15;
    private float cameraHeight = 2;
    public bool isThirdPerson = false;

    // Update is called once per frame
    void Update()
    {

        // rotate by y 180
        float px = cameraParent.localPosition.x;
        float py = cameraParent.localPosition.y;
        float pz = cameraParent.localPosition.z;

        if (Input.GetKeyDown("p"))
        {
            isThirdPerson = !isThirdPerson;

            if(isThirdPerson)
            {
                ChangeToThirdPerson(px, py, pz);
            }
            else
            {
                ChangeToFirstPerson(px, py, pz);
            }

        }
    }

    private void ChangeToThirdPerson(float x, float y, float z)
    {
        cameraParent.localPosition = new Vector3(x + cameraDistance, y + cameraHeight, z);
        cameraParent.localEulerAngles -= new Vector3(0, 90, 0);
        cameraParent.localEulerAngles = new Vector3(0, cameraParent.localEulerAngles.y, cameraParent.localEulerAngles.z);
    }

    private void ChangeToFirstPerson(float x, float y, float z)
    {
        cameraParent.localPosition = new Vector3(x - cameraDistance, y - cameraHeight, z);
        cameraParent.localEulerAngles -= new Vector3(0, -90, 0);
        cameraParent.localEulerAngles = new Vector3(0, cameraParent.localEulerAngles.y, cameraParent.localEulerAngles.z);
    }
}
