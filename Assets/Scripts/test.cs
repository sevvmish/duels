using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out CharacterManager c))
        {
            print("erjhfgbrbjhgrhg");
            print(c.Character.Name);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CharacterManager c))
        {
            print("22222222222");
            print(c.Character.Name);
        }
    }
}
