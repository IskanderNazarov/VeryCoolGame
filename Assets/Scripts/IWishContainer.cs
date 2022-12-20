using UnityEngine;

public interface IWishContainer {
    int WishID();
    void OnWishDropped(WishObject obj);
    Transform GetWishTarget();
    void OnWishSatisfied();
}