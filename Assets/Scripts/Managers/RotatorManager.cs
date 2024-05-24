using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatorManager : MonoBehaviour
{
    private float yAxis;
    [SerializeField] private float speed = 60;

    private List<Transform> clients = new List<Transform>();

    private void Awake()
    {
        gameObject.name = "RotatorManager";
    }

    // Update is called once per frame
    void Update()
    {
        yAxis += Time.deltaTime * speed;
        if (yAxis >= 360) yAxis = 0;

        for (int i = 0; i < clients.Count; i++)
        {
            clients[i].localEulerAngles = new Vector3(clients[i].localEulerAngles.x, yAxis, clients[i].localEulerAngles.z);
        }
    }

    public void Add(Transform t)
    {
        if (!clients.Contains(t)) clients.Add(t);
    }

    public void Remove(Transform t)
    {
        if (clients.Contains(t)) clients.Remove(t);
    }
}
