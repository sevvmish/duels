using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVControl : MonoBehaviour
{
    [SerializeField] private Transform fovClose;
    [SerializeField] private Transform fovFar;

    public void SetFOV()
    {
        if (Globals.IsMobile)
        {
            if (Globals.IsLowFPS)
            {
                fovFar.localScale = new Vector3(1.2f, 1, 0.95f);
                fovFar.localPosition = new Vector3(0, 5, 65);
                fovFar.localEulerAngles = new Vector3(180, 0, 0);

                fovClose.localScale = new Vector3(0.6f, 0.3f, 0.45f);
                fovClose.localPosition = new Vector3(0, 2, 35);
                fovClose.localEulerAngles = new Vector3(180, 0, 0);
            }
            else
            {
                fovFar.localScale = new Vector3(1.1f, 1, 1.2f);
                fovFar.localPosition = new Vector3(0, 5, 85);
                fovFar.localEulerAngles = new Vector3(180, 0, 0);

                fovClose.localScale = new Vector3(0.65f, 0.3f, 0.55f);
                fovClose.localPosition = new Vector3(0, 5, 42);
                fovClose.localEulerAngles = new Vector3(180, 0, 0);
            }
            
        }
        else
        {
            fovFar.localScale = new Vector3(1.1f, 1, 1.3f);
            fovFar.localPosition = new Vector3(0, 5, 90);
            fovFar.localEulerAngles = new Vector3(180, 0, 0);

            fovClose.localScale = new Vector3(0.65f, 0.3f, 0.6f);
            fovClose.localPosition = new Vector3(0, 5, 42);
            fovClose.localEulerAngles = new Vector3(180, 0, 0);
        }
    }
}
