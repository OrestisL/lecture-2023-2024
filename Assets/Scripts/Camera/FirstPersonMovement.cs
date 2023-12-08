using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lecture;

public class FirstPersonMovement : MonoBehaviour
{
    public float inputSensitivity;
    private Camera mainCam;
    private float rotX, rotY;
    [SerializeField] private float clampAngle;
    private void Awake()
    {
        mainCam = Camera.main;
    }

    public void SetStartingValues(float x, float y)
    {
        rotX = x;
        rotY = y;
    }
    public void CameraRotation(Vector2 delta)
    {
        rotY += delta.x * inputSensitivity * Time.deltaTime;
        rotX -= delta.y * inputSensitivity * Time.deltaTime;

        //Clamp rotation x between -clampAngle and clampAngle
        rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);

        Quaternion localRot = Quaternion.Euler(rotX, rotY, 0.0f);
        mainCam.transform.localRotation = localRot;
    }
}
