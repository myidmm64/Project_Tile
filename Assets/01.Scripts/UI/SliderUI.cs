using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderUI : MonoBehaviour
{
    private Slider _slider = null;
    private TextMeshProUGUI _amountText = null;
    private Sequence _animationSeq = null;

    [SerializeField, Header("구분선 갯수")]
    private int _segmentCount = 0;
    [SerializeField, Header("Fill 오브젝트의 RectTrm")]
    private RectTransform _fillRectTrm = null;

    private void Awake()
    {
        SetSegment(_segmentCount);
    }

    private GameObject GetBarSegment()
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("UI/BarSegment"));
        return obj;
    }

    public void SetSegment(int segmentCount)
    {
        if (segmentCount == 0)
        {
            Debug.LogWarning("왜 0개를 만드세용");
            return;
        }

        float segmentWidth = 1.0f / _segmentCount;  // 구분선 사이의 간격

        for (int i = 1; i < segmentCount; i++)
        {
            GameObject barSegmentObj = GetBarSegment();
            RectTransform segmentRectTrm = barSegmentObj.GetComponent<RectTransform>();
            segmentRectTrm.SetParent(_fillRectTrm);

            segmentRectTrm.anchorMin = new Vector2(segmentWidth * i, 0f);
            segmentRectTrm.anchorMax = new Vector2(segmentWidth * i, 1f);

            segmentRectTrm.anchoredPosition3D = Vector3.zero;
        }
    }

    public void Initialize(int sliderMaxValue)
    {
        if(_slider == null)
        {
            _slider = GetComponent<Slider>();
        }
        if(_amountText == null)
        {
            _amountText = transform.Find("AmountText").GetComponent<TextMeshProUGUI>();
        }

        _slider.maxValue = sliderMaxValue;
        _slider.minValue = 0;
        SetValue(sliderMaxValue);
    }

    public void SetValue(int value)
    {
        _animationSeq?.Kill();

        _slider.value = value;
        _amountText.SetText($"{value}/{_slider.maxValue}");
    }

    public void SetValueWithAnimation(int value, float duration)
    {
        _animationSeq?.Kill();
        _animationSeq = DOTween.Sequence();

        value = Mathf.Clamp(value, 0, (int)_slider.maxValue);
        _animationSeq.Append(DOTween.To(() => (int)_slider.value, 
            x => 
            {
                SetValue(x);
            },
            value, duration)).SetEase(Ease.Linear);
    }
}
