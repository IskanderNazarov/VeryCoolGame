public class BonusWishObject : WishObject {
    protected override bool CanBePlacedOn(IWishContainer container) {
        return true;
    }
}