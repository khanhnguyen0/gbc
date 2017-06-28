using UnityEngine;
using System.Collections.Generic;

public static class EntityStore
{
    private const string _path = "Entities/";
    private static bool _initialized;

    private static Dictionary<int, Entity> _entities;

    public static void initialize()
    {
        if(_initialized)
        {
            //throw new System.Exception("Already initialized");
        }
        else
        {
            _initialized = true;
            _entities = new Dictionary<int, Entity>();
            loadEntities(_path);
        }
    }

    private static void loadEntities(string path)
    {
        UnityEngine.Object[] data = Resources.LoadAll(path);

        Debug.Log("Loading " + data.Length + " entities.");

        for(int i = 0; i < data.Length; i++)
        {
            Debug.Log(data[i].GetType());
            Entity entity = ((GameObject)data[i]).GetComponent<Entity>();
            _entities.Add(entity.id, entity);
        }
    }

    public static Entity getEntity(int identifier)
    {
        return _entities[identifier];
    }

    public static Entity createEntity(int identifier)
    {
        Entity entity = getEntity(identifier);

        Entity instance = GameObject.Instantiate<Entity>(entity);
        return instance;
    }
}