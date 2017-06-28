using UnityEngine;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.Serialization;

[System.Serializable]
public class Entity : MonoBehaviour, ISaveable
{
    [SerializeField]
    private int _id;

    public int id { get { return _id; } }

    public virtual void loadData(StreamReader reader)
    {
        _id = int.Parse(reader.ReadLine());
    }

    public virtual void saveData(StreamWriter writer)
    {
        writer.WriteLine(_id);
    }

    void Awake()
    {

    }

    void Start()
    {

    }

    void Update()
    {

    }
}