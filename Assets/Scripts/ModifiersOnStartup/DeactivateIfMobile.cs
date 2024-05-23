using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateIfMobile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Globals.IsMobile) Destroy(gameObject);
    }

}
