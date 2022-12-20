using System;
using System.Collections;
using UnityEngine;

public class ObjectsSpawner : MonoBehaviour {
    [SerializeField] private float objectsSpeed = 2;
    [SerializeField] private float timerInterval = 1;

    private IWishDescriptor wishDescriptor;
    private IWishContainer[] wishContainers;
    private Camera camera;
    private Vector2 startObjectPos;
    private Vector2 endObjectPos;

    private void Start() {
        camera = Camera.main;
        var camH = camera.orthographicSize * 2;
        var camW = camera.aspect * camH;
        startObjectPos = new Vector2(camW / 2, transform.position.y);
        endObjectPos = new Vector2(-camW / 2, transform.position.y);

        StartCoroutine(StartSpawning());
    }
    
    public void Initialize(IWishDescriptor descriptor, IWishContainer[] containers) {
        wishContainers = containers;
        wishDescriptor = descriptor;
    }

    public IEnumerator StartSpawning() {
        if (wishDescriptor == null) {
            throw new InvalidOperationException(
                "The 'entityDescriptor' field cannot be null, use 'SetEntityDescriptor' to initialize it");
        }


        while (true) {
            yield return new WaitWhile(() => Pool.shared.GetPooledObject() == null);


            var obj = Pool.shared.GetPooledObject().GetComponent<WishObject>();
            var id = wishDescriptor.getRandomID(wishContainers);
            var size = obj.GetComponent<SpriteRenderer>().bounds.size;
            obj.gameObject.SetActive(true);
            obj.ReinitializeItem(id, wishDescriptor.getSpriteByID(id));
            obj.MoveOnConveyor(startObjectPos + Vector2.right * size.x, endObjectPos + Vector2.left * size.x,
                objectsSpeed);


            yield return new WaitForSeconds(timerInterval);
        }
    }
}