using UnityEngine;

public interface IWishDescriptor {
        int getRandomID();
        public int getRandomID(IWishContainer[] containers);
        public int getRandomUniqueID(IWishContainer[] containers);
        Sprite getSpriteByID(int i);
}