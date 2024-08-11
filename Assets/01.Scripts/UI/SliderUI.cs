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

    [SerializeField, Header("���м� ����")]
    private int _segmentCount = 0;
    [SerializeField, Header("Fill ������Ʈ�� RectTrm")]
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
            Debug.LogWarning("�� 0���� ���弼��");
            return;
        }

        float segmentWidth = 1.0f / _segmentCount;  // ���м� ������ ����

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
        SetValueImmediate(sliderMaxValue);
    }

    public void SetValueImmediate(float value)
    {
        _animationSeq?.Kill();
        SetUI(value);
    }

    public void SetUI(float value)
    {
        _slider.value = value;
        _amountText.SetText($"{(int)value}/{_slider.maxValue}");
    }

    public void SetValueWithAnimation(int value, float duration)
    {
        _animationSeq?.Kill();
        _animationSeq = DOTween.Sequence();

        float floatValue = Mathf.Clamp((float)value, 0, _slider.maxValue);

        _animationSeq.Append(DOTween.To(() => _slider.value, 
            x => 
            {
                SetUI(x);
            },
            floatValue, duration)).SetEase(Ease.Linear);
    }
}
