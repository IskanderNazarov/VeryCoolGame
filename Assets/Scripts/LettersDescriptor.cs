using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LettersDescriptor : MonoBehaviour, IWishDescriptor {
    [SerializeField] private int wrongObjectsInRowThreshold = 3;
    [SerializeField] private Sprite[] sprites;

    private int counterInARow;

    public int getRandomID() {
        return Random.Range(0, sprites.Length);
    }

    public int getRandomID(IWishContainer[] containers) {
        if (counterInARow == wrongObjectsInRowThreshold) {
            counterInARow = 0;
            return containers[Random.Range(0, containers.Length)].WishID();
        }

        var randId = Random.Range(0, sprites.Length);
        if (isIdForAWish(randId, containers)) {
            counterInARow = 0;
        }
        else {
            counterInARow++;
        }

        return randId;
    }

    public int getRandomUniqueID(IWishContainer[] containers) {
        while (true) {
            var randId = getRandomID();
            var isRandIdUnique = true;
            foreach (var wishContainer in containers) {
                isRandIdUnique &= wishContainer.WishID() != randId;
            }

            if (isRandIdUnique) {
                return randId;
            }
        }
    }

    public Sprite getSpriteByID(int i) {
        return sprites[i];
    }

    private bool isIdForAWish(int id, IWishContainer[] containers) {
        foreach (var wishContainer in containers) {
            if (wishContainer.WishID() == id) {
                return true;
            }
        }

        return false;
    }
}