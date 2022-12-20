using System;
using DG.Tweening;
using UnityEngine;

public class Character : MonoBehaviour, IWishContainer {
    [SerializeField] private Transform wishDropPlace;
    [SerializeField] private SpriteRenderer spriteRenderer;


    private Collider2D collider;

    private int id;

    //private IWishDescriptor wishDescriptor;
    public Action<Character> OnUpdateRequest;


    private void Start() {
        collider = GetComponent<Collider2D>();
        OnUpdateRequest?.Invoke(this);
    }

    public void UpdateWish(int id, Sprite sprite) {
        this.id = id;
        spriteRenderer.sprite = sprite;
    }

    public int WishID() {
        return id;
    }

    public virtual bool CanDropWish(WishObject obj) {
        return obj.ID == id;
    }

    public Transform GetWishTarget() {
        return wishDropPlace;
    }

    public void OnWishDropped(WishObject obj) {
        collider.enabled = false;
    }

    public void OnWishSatisfied() {
        collider.enabled = true;
        var tr = spriteRenderer.transform;
        var originScale = tr.localScale;
        const float duration = 0.5f;

        tr.DOScale(originScale / 2, duration);
        spriteRenderer.DOFade(0, duration).OnComplete(delegate {
            OnUpdateRequest?.Invoke(this);

            tr.DOScale(originScale, duration);
            spriteRenderer.DOFade(1, duration);
        });
    }
}