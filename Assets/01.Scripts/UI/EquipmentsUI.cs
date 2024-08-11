using UnityEngine;

public class EquipmentsUI : MainUIElement
{
    public RectTransform gridTrm { get; private set; }

    public override void BindUI()
    {
        gridTrm = transform.FindComponentInChildren<RectTransform>("Grid");
    }

}
