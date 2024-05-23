using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateIfFPS : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (Globals.IsLowFPS) Destroy(gameObject);
    }

    private void Update()
    {
        if (Globals.IsLowFPS && gameObject.activeSelf) gameObject.SetActive(false);
    }

}
