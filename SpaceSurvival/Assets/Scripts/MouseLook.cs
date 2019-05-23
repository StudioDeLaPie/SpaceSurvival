using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public class MouseLook
{
    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
    public bool clampVerticalRotation = true;
    public float MinimumX = -90F;
    public float MaximumX = 90F;
    public bool lockCursor = true;

    private Quaternion _CharacterTargetRot;
    private Quaternion _CameraTargetRot;
    private bool _cursorIsLocked = true;

    public void Init(Transform character, Transform camera)
    {
        _CharacterTargetRot = character.localRotation;
        _CameraTargetRot = camera.localRotation;
    }

    public void LookRotation(Transform character, Transform camera)
    {
        if (_cursorIsLocked) //Tourne la caméra seulement si le cursor est lock
        {

            float yRot = Input.GetAxis("Mouse X") * XSensitivity;
            float xRot = Input.GetAxis("Mouse Y") * YSensitivity;

            _CameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

            if (clampVerticalRotation)
                _CameraTargetRot = ClampRotationAroundXAxis(_CameraTargetRot);

            character.Rotate(character.up, yRot, Space.World);
            camera.localRotation = _CameraTargetRot;

        }
        UpdateCursorLock();
    }

    private void UpdateCursorLock()
    {
        //if the user set "lockCursor" we check & properly lock the cursor
        if (lockCursor)
            InternalLockUpdate();
    }

    private void InternalLockUpdate()
    {
        if (_cursorIsLocked && Input.GetButtonUp("Escape"))
        {
            _cursorIsLocked = false;
        }
        else if ((Input.GetMouseButtonUp(0) && !IsPointerOverUIObject()) || Input.GetButtonUp("Cancel") || (!_cursorIsLocked && Input.GetButtonUp("Escape")))
        {
            _cursorIsLocked = true;
        }

        if (_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            //escapeMenu.SetActive(false);
            //reticule.SetActive(true);
        }
        else if (!_cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            //escapeMenu.SetActive(true);
            //reticule.SetActive(false);
        }
    }

    //When Touching UI
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
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
