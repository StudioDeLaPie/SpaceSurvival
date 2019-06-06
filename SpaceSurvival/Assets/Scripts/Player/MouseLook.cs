using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MouseLook
{
    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
    public bool clampVerticalRotation = true;
    public float MinimumX = -90F;
    public float MaximumX = 90F;
    public bool cursorLocked = true;

    private Quaternion _CharacterTargetRot;
    private Quaternion _CameraTargetRot;

    public void Init(Transform character, Transform camera)
    {
        _CharacterTargetRot = character.localRotation;
        _CameraTargetRot = camera.localRotation;
    }

    public void LookRotation(Transform character, Transform camera)
    {
        if (cursorLocked) //Tourne la caméra seulement si le cursor est lock
        {

            float yRot = Input.GetAxis("Mouse X") * XSensitivity;
            float xRot = Input.GetAxis("Mouse Y") * YSensitivity;

            _CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

            if (clampVerticalRotation)
                _CameraTargetRot = ClampRotationAroundXAxis(_CameraTargetRot);

            character.Rotate(character.up, yRot, Space.World);
            camera.localRotation = _CameraTargetRot;

        }
    }

    private Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

}
