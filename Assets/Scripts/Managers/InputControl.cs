using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;


public class InputControl : MonoBehaviour
{
    private Camera _camera;
    private Joystick joystick;    
    private PointerDownOnly jumpButton;
    private PointerDownOnly attackButton;
    private PointerMoveOnly moverSurface;

    PlayerControl playerControl;
    private readonly float XLimit = 10;
    CameraControl cameraControl;

    public void SetData(Camera _camera)
    {
        this._camera = _camera;
        playerControl = GetComponent<PlayerControl>();
        cameraControl = GetComponent<CameraControl>();

        if (!Globals.IsMobile)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Destroy(GameObject.Find("Mobile Controls"));
            Globals.WORKING_DISTANCE = 30;
        }
        else
        {
            Globals.WORKING_DISTANCE = 20;
            moverSurface = GameObject.Find("MoverSurface").GetComponent<PointerMoveOnly>();
            jumpButton = GameObject.Find("JumpButton").GetComponent<PointerDownOnly>();
            attackButton = GameObject.Find("AttackButton").GetComponent<PointerDownOnly>();
            joystick = GameObject.Find("Floating Joystick").GetComponent<FloatingJoystick>();

            moverSurface.gameObject.SetActive(true);
            jumpButton.gameObject.SetActive(true);
            attackButton.gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Globals.IsMobile)
        {
            forMobile();
        }
        else
        {
            forPC();
        }
    }

    private void forMobile()
    {

        //playerControl.SetHorizontal(joystick.Horizontal);
        //playerControl.SetVertical(joystick.Vertical);
        playerControl.SetVectorMovements(new Vector2(joystick.Horizontal, joystick.Vertical));

        //TODEL        
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (!Globals.IsOptions && (horizontal != 0 || vertical != 0))
        {
            playerControl.SetVectorMovements(new Vector2(horizontal, vertical));
        }

        if (!Globals.IsOptions && (Input.GetKeyDown(KeyCode.Space) || jumpButton.IsPressed))
        {            
            playerControl.JumpActivator?.Invoke();
        }
        else if (!Globals.IsOptions && (Input.GetMouseButtonDown(0) || attackButton.IsPressed))
        {
            playerControl.AttackActivator?.Invoke();
        }

        if (Globals.IsOptions) return;

        Vector2 delta2 = moverSurface.DeltaPosition;
        Vector2 delta = delta2.normalized;


        if (delta2.x > 0 || delta2.x < 0)
        {

            int sign = delta2.x > 0 ? 1 : -1;
            playerControl.SetRotationAngle(4 * sign/*200 * sign * Time.deltaTime*/);

        }
        else if (delta2.x == 0)
        {
            playerControl.SetRotationAngle(0);
        }

        if (Mathf.Abs(delta.y) > 0)
        {
            cameraControl.ChangeCameraAngleX(delta.y * -70 * Time.deltaTime);
        }
    }

    private void forPC()
    {
        if (Input.mouseScrollDelta.magnitude > 0)
        {
            cameraControl.ChangeZoom(Input.mouseScrollDelta.y);
        }


        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (!Globals.IsOptions && (horizontal != 0 || vertical != 0))
        {
            playerControl.SetVectorMovements(new Vector2(horizontal, vertical));
        }


        if (!Globals.IsOptions && Input.GetMouseButtonDown(0))
        {
            playerControl.AttackActivator?.Invoke();
        }
        else if (!Globals.IsOptions && Input.GetKeyDown(KeyCode.Space))
        {            
            playerControl.JumpActivator?.Invoke();
        }
        

        if (Globals.IsOptions) return;

        Vector3 mouseDelta = new Vector3(
            Input.GetAxis("Mouse X") * Globals.MOUSE_X_SENS * Time.deltaTime,
            Input.GetAxis("Mouse Y") * Globals.MOUSE_Y_SENS * Time.deltaTime, 0);


        if ((mouseDelta.x > 0) || (mouseDelta.x < 0))
        {
            float koeff = mouseDelta.x * 20 * Time.deltaTime;

            if (koeff > XLimit)
            {
                koeff = XLimit;
            }
            else if (koeff < -XLimit)
            {
                koeff = -XLimit;
            }


            playerControl.SetRotationAngle(koeff);
        }
        else
        {
            playerControl.SetRotationAngle(0);
        }


        if (Mathf.Abs(mouseDelta.y) > 0)
        {
            cameraControl.ChangeCameraAngleX(mouseDelta.y * -7 * Time.deltaTime);
        }
    }

}
