using UnityEngine;

namespace IsoRush.Level.Scrollable
{
    public class PositionScrollable : BaseScrollable
    {
        protected override void SetScroll(float scroll)
        {
            transform.localPosition = transform.right * scroll;
        }
    }
}
