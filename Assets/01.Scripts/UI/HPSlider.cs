using System;
using UnityEngine;
using UnityEngine.UI;

public class HPSlider : MonoBehaviour
{
    private Slider _slider = null;
    private int maxHP = 0;

    [SerializeField, Header("구분선 갯수")]
    private int _segmentCount = 0;
    [SerializeField, Header("Fill 오브젝트의 RectTrm")]
    private RectTransform _fillRectTrm = null;

    private void Start()
    {
        if (_segmentCount == 0) return;

        SetSegment(_segmentCount);
    }

    private GameObject GetBarSegment()
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("UI/BarSegment"));
        return obj;
    }

    public void SetSegment(int segmentCount)
    {
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

    public void Initialize(int maxHP)
    {
        if(_slider == null)
        {
            _slider = GetComponent<Slider>();
        }
        _slider.maxValue = maxHP;
        _slider.value = maxHP;
    }


}
