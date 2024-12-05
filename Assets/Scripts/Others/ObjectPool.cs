using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private GameObject example;
    private Transform storage;
    private int index;

    private Queue<GameObject> poolOfObjects;
    public ObjectPool(int Index, GameObject Example, Transform Storage)
    {
        example = Example;
        storage = Storage;
        index = Index;

        poolOfObjects = new Queue<GameObject>();


        for (int i = 0; i < Index; i++)
        {
            GameObject _object = Instantiate(Example, Vector3.zero, Quaternion.identity, Storage);
            _object.name = Example.name;
            _object.SetActive(false);
            poolOfObjects.Enqueue(_object);
        }
    }

    public void AddAdditionalLimit(int amount)
    {        
        index += amount;

        for (int i = 0; i < amount; i++)
        {
            GameObject _object = Instantiate(example, Vector3.zero, Quaternion.identity, storage);
            _object.name = example.name;
            _object.SetActive(false);
            poolOfObjects.Enqueue(_object);
        }
    }

    public int AmountOfActive()
    {
        return (index - poolOfObjects.Count);
    }

    public int OverallAmount => index;


    public ObjectPool(int Index, GameObject Example)
    {
        example = Example;
        storage = null;
        index = Index;

        poolOfObjects = new Queue<GameObject>();


        for (int i = 0; i < Index; i++)
        {
            GameObject _object = Instantiate(Example);
            _object.name = Example.name;
            _object.SetActive(false);
            poolOfObjects.Enqueue(_object);
        }
    }

    public void Clear()
    {
        if (poolOfObjects.Count == 0) return;

        while (poolOfObjects.Count > 0)
        {
            GameObject g = poolOfObjects.Dequeue();
            Destroy(g);
        }

    }

    public GameObject GetObject()
    {
        if (poolOfObjects.Count > 0 && !poolOfObjects.Peek().activeSelf)
        {
            return poolOfObjects.Dequeue();
            
        }
            

        //print("instantiated new object of type: queue is full - " + example.name);
        index++;
        GameObject _object = null;
        if (storage == null)
        {
            _object = Instantiate(example);
            _object.name = example.name;
        }
        else
        {
            _object = Instantiate(example, storage);
            _object.name = example.name;
        }

        _object.SetActive(false);

        return _object;
    }

    public GameObject GetObject(bool isActiveBeforeTaken)
    {
        if (poolOfObjects.Count > 0)
        {
            GameObject result = poolOfObjects.Dequeue();
            result.SetActive(isActiveBeforeTaken);
            return result;
        }


        //print("instantiated new object of type: queue is full");
        index++;
        GameObject _object = null;
        if (storage == null)
        {
            Instantiate(example);
        }
        else
        {
            Instantiate(example, storage);
        }

        _object.SetActive(isActiveBeforeTaken);

        return _object;
    }

    public void ReturnObject(GameObject _object)
    {
        if (_object == null) return;

        _object.transform.DOKill();

        if (storage != null) _object.transform.SetParent(storage);
        _object.transform.localScale = Vector3.one;
        _object.transform.position = Vector3.zero;
        _object.transform.eulerAngles = Vector3.zero;
                
        _object.SetActive(false);
        poolOfObjects.Enqueue(_object);
    }

}
