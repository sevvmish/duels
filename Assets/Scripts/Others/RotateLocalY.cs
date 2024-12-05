using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLocalY : MonoBehaviour
{
    [SerializeField] private float initialAngle = 45f;
    [SerializeField] private Axis axis = Axis.axisY;

    // Start is called before the first frame update
    void Start()
    {
        int rnd = UnityEngine.Random.Range(0, (int)(360/initialAngle));

        switch(axis)
        {
            case Axis.axisX:
                transform.localEulerAngles = new Vector3(initialAngle * rnd, transform.localEulerAngles.y, transform.localEulerAngles.z);
                break;

            case Axis.axisY:
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, initialAngle * rnd, transform.localEulerAngles.z);
                break;

            case Axis.axisZ:
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, initialAngle * rnd);
                break;
        }

        
    }

}

public enum Axis
{
    axisX, axisY, axisZ
}
