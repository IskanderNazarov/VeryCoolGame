using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class WishObject : MonoBehaviour {
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D collider;
    [SerializeField] private float dragScaleFactor = 1.25f;
    [SerializeField] private float shiftUp = 0.8f;
    [SerializeField] private float wishDropAnimDuration = 0.5f;
    [FormerlySerializedAs("wrongDropMoveDuration")] [SerializeField] private float wrongDropMoveSpeed = 0.8f;

    public int ID { get; private set; }

    private Camera camera;
    private const int TOP_ORDER = 20;
    private Vector2 originScale;

    private void Start() {
        camera = Camera.main;
        originScale = transform.localScale;
    }

    public void ReinitializeItem(int id, Sprite newSprite) {
        ID = id;
        spriteRenderer.sprite = newSprite;
    }

    public void MoveOnConveyor(Vector2 startPosition, Vector2 endPosition, float speed) {
        transform.position = startPosition;
        transform.DOMove(endPosition, speed).SetSpeedBased().SetEase(Ease.Linear).OnComplete(
            delegate { gameObject.SetActive(false); });
    }

    private void OnMouseDown() {
        var mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        transform.DOKill();
        spriteRenderer.sortingOrder = TOP_ORDER;

        transform.localScale = originScale * dragScaleFactor;
        mousePos.z = 0;
        transform.position = mousePos + Vector3.up * shiftUp;
    }

    private void OnMouseDrag() {
        var mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        transform.position = mousePos + Vector3.up * shiftUp;
    }

    private void OnMouseUp() {
        var mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        collider.enabled = false;

        var hit = Physics2D.Raycast(mousePos, Vector2.zero);
        if (hit.collider != null) {
            IWishContainer wishContainer = null;
            var components = hit.collider.GetComponents<MonoBehaviour>();
            foreach (var component in components) {
                if (component is IWishContainer container) {
                    wishContainer = container;
                    break;
                }
            }

            if (wishContainer != null && CanBePlacedOn(wishContainer)) {
                wishContainer.OnWishDropped(this);
                MoveToTarget(wishContainer);
            }
            else {
                OnWrongDrop();
            }
        }
        else {
            OnWrongDrop();
        }


        //gameObject.SetActive(false);
    }

    protected virtual bool CanBePlacedOn(IWishContainer container) {
        return ID == container.WishID();
    }

    private void MoveToTarget(IWishContainer wishContainer) {
        var tr = transform;
        //var originScale = transform.localScale;
        tr.DOScale(Vector3.zero, wishDropAnimDuration);
        tr.DOMove(wishContainer.GetWishTarget().position, wishDropAnimDuration).OnComplete(delegate {
            wishContainer.OnWishSatisfied();

            Reset();
        });
    }

    private void OnWrongDrop() {
        var cameraHeight = camera.orthographicSize * 2;
        transform.DOMoveY(-cameraHeight / 2 - spriteRenderer.bounds.size.y, wrongDropMoveSpeed).SetEase(Ease.InSine).SetSpeedBased()
            .OnComplete(Reset);
    }

    private void Reset() {
        transform.localScale = originScale;
        gameObject.SetActive(false);
        collider.enabled = true;
    }
}