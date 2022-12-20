using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {
    [SerializeField] private LettersDescriptor descriptor;
    [SerializeField] private ObjectsSpawner spawner;
    [SerializeField] private Character[] characters;

    private List<IWishContainer> wishContainers;
    private void Awake() {
        wishContainers = new List<IWishContainer>(characters);
        spawner.Initialize(descriptor, wishContainers.ToArray());

        foreach (var character in characters) {
            //character.SetWishDescriptor(descriptor);
            character.OnUpdateRequest += OnIdRequest;
        }
    }

    private void OnIdRequest(Character character) {
         var id = descriptor.getRandomUniqueID(wishContainers.ToArray());
         character.UpdateWish(id, descriptor.getSpriteByID(id));
    }
}