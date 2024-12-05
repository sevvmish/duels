using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class CameraControl : MonoBehaviour
{    
    private Camera _camera;
    private PlayerControl pc;

    private Transform mainPlayer;
    private Transform mainCamera;
    private Transform mainCamTransformForRaycast;
    private Transform outerCamera;
    private Vector3 outerCameraShiftVector = Vector3.zero;
    private Vector3 baseCameraBodyPosition = Vector3.zero;

    private float currentZoom;
    private float zoomTimer;
    private bool isZooming;

    private float xLimitUp = 40;
    private float xLimitDown = 270;
    private float defaultCameraDistance;

    public void SetData(Camera _camera)
    {        
        this._camera = _camera;
        pc = GetComponent<PlayerControl>();
        mainPlayer = transform;

        mainCamTransformForRaycast = _camera.transform;
        mainCamera = _camera.transform.parent;
        
        outerCamera = mainCamera.parent;
        mainCamera.localPosition = Globals.BasePosition;
        mainCamera.localEulerAngles = Globals.BaseRotation;
        baseCameraBodyPosition = Globals.BasePosition;

        currentZoom = Globals.MainPlayerData.Zoom;
        Zoom(Globals.MainPlayerData.Zoom);


        outerCamera.eulerAngles += new Vector3(-25, 0, 0);
        defaultCameraDistance = (mainPlayerPoint - mainCamTransformForRaycast.position).magnitude;
    }

    private Vector3 mainPlayerPoint => mainPlayer.position + Vector3.up * 1.2f;

    public void ChangeCameraAngleX(float angleX)
    {
        if (!Globals.IsMobile && Mathf.Abs(angleX) > 5) return;


        if (angleX > 0 && outerCamera.localEulerAngles.x > (xLimitUp - 10) && outerCamera.localEulerAngles.x < xLimitUp) return;
        if (angleX < 0 && outerCamera.localEulerAngles.x < (xLimitDown + 10) && outerCamera.localEulerAngles.x > xLimitDown) return;


        outerCamera.localEulerAngles = new Vector3(outerCamera.localEulerAngles.x + angleX * 2, outerCamera.localEulerAngles.y, outerCamera.localEulerAngles.z);

    }

    public void Zoom(float koeff)
    {
        _camera.fieldOfView = koeff;
    }

    public void ChangeZoom(float koeff)
    {
        if (koeff < 0 && Globals.MainPlayerData.Zoom <= Globals.ZOOM_LIMIT_HIGH)
        {
            Globals.MainPlayerData.Zoom += Globals.ZOOM_DELTA;
        }
        else if (koeff > 0 && Globals.MainPlayerData.Zoom > -Globals.ZOOM_LIMIT_LOW)
        {
            Globals.MainPlayerData.Zoom -= Globals.ZOOM_DELTA;
        }

        checkCorrectZoom();
    }

    private void checkCorrectZoom()
    {
        if (Globals.MainPlayerData.Zoom > Globals.ZOOM_LIMIT_HIGH)
        {
            Globals.MainPlayerData.Zoom = Globals.ZOOM_LIMIT_HIGH;
            Zoom(Globals.MainPlayerData.Zoom);
        }
        else if (Globals.MainPlayerData.Zoom < Globals.ZOOM_LIMIT_LOW)
        {
            Globals.MainPlayerData.Zoom = Globals.ZOOM_LIMIT_LOW;
            Zoom(Globals.MainPlayerData.Zoom);
        }
        else
        {
            Zoom(Globals.MainPlayerData.Zoom);
        }
    }

    void Update()
    {
        mainCamera.localPosition = Globals.BasePosition;
        baseCameraBodyPosition = Globals.BasePosition;


        if (zoomTimer > 10)
        {
            zoomTimer = 0;

            if (currentZoom != Globals.MainPlayerData.Zoom)
            {
                currentZoom = Globals.MainPlayerData.Zoom;
                SaveLoadManager.Save();
            }
        }
        else
        {
            zoomTimer += Time.deltaTime;
        }


        outerCamera.position = mainPlayer.position;
        outerCamera.eulerAngles = new Vector3(outerCamera.eulerAngles.x, pc.angleYForMobile, outerCamera.eulerAngles.z);

    }
}
