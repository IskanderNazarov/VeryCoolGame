using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour {
    public static Pool shared { get; private set; }

    [SerializeField] private GameObject objectToPool;
    [SerializeField] private int amountToPool;
    
    private List<GameObject> pooledObjects;


    private void Awake() {
        if (shared == null) {
            shared = this;
        } else if (shared != this) {
            Destroy(this);
        }
        
        
        pooledObjects = new List<GameObject>();
        for (var i = 0; i < amountToPool; i++) {
            var obj = Instantiate(objectToPool);
            obj.gameObject.SetActive(false);
            pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject() {
        return pooledObjects.Find(o => !o.gameObject.activeSelf);
    }
}