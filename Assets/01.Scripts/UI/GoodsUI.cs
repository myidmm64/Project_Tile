using DG.Tweening;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class GoodsUI : MainUIElement
{
    [Header("·çºñ")]
    public TextMeshProUGUI lubyAmountText { get; private set; }
    public RectTransform lubyImageTrm { get; private set; }

    private int _lubyAmount = 0;
    private Sequence lubyTextSeq = null;

    public override void BindUI()
    {
        lubyAmountText = transform.FindComponentInChildren<TextMeshProUGUI>("Luby/AmountText");
        lubyImageTrm = transform.FindComponentInChildren<RectTransform>("Luby/Image");
    }

    public void SetLubyAmount(int amount, float duration)
    {
        lubyTextSeq?.Kill();
        lubyTextSeq = DOTween.Sequence();

        lubyTextSeq.Append(DOTween.To(() => _lubyAmount,
            x =>
            {
                lubyAmountText.SetText(x.ToString());
            },
            amount, duration)).SetEase(Ease.Linear);
    }    
}
