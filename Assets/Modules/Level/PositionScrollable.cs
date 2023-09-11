using UnityEngine;

namespace IsoRush.Level
{
    public class PositionScrollable : Scrollable
    {
        protected override void SetScroll(float scroll)
        {
            transform.localPosition = transform.right * scroll;
        }
    }
}
