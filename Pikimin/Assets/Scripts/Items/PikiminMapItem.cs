using static Map.PikiMap;

namespace Items
{
    public class PikiminMapItem : Item
    {
        #region Overrides of Item

        public override void OnUse()
        {
            CurrentMap.SwapMapOnOff();
        }

        #endregion
    }
}
