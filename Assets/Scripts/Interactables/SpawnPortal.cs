using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPortal : MonoBehaviour
{    
    private float cooldown = 20f;
    [SerializeField] private GameObject visual;

    private BoxCollider _collider;

    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<BoxCollider>();
        _collider.enabled = true;
        visual.SetActive(true);
    }

    public void StartCooldown()
    {
        visual.SetActive(false);
        _collider.enabled = false;
        StartCoroutine(playCooldown());
    }
    private IEnumerator playCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        visual.SetActive(true);
        _collider.enabled = true;
    }

    public int GetCost(int squadAmount)
    {
        switch(squadAmount)
        {
            case 0: return 0;
            case 1: return 3; //3
            case 2: return 5; //2
            case 3: return 10; //5
            case 4: return 15; //5
            case 5: return 23; //8
            case 6: return 32; //9
            case 7: return 42; // 10
            case 8: return 50; // 8
            default: return 50;
        }
    }
}
