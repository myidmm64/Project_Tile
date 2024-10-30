using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/RangeData")]
public class RangeDataSO : ScriptableObject
{
    public ECenterType centerType; // ������ �ν��� �߾� ���ϱ�
    public Vector2Int centerPosKey; // centerType�� PosKey�� �� ����� center
    public ETeam targetType; // �������� �� �ǵ�??
    public ESearchType searchType; // ��� �˻��� ����??
    public bool includeOwner; // ���� �� ����?
     
    [Header("���� ����")]
    [SerializeField]
    private List<LineRange> lineRanges = new List<LineRange>();
    [SerializeField]
    private List<CrossRange> crossRanges = new List<CrossRange>();
    [SerializeField]
    private List<SquareRange> squareRanges = new List<SquareRange>();
    [SerializeField]
    private List<RactangleRange> ractangleRanges = new List<RactangleRange>();
    [SerializeField]
    private List<StrRange> strRanges = new List<StrRange>();

    public List<RangeOption> GetRangeOptions() // �߰��Ǹ� ���⵵ �߰�
    {
        List<RangeOption> result = new List<RangeOption>();
        result.AddRange(lineRanges);
        result.AddRange(crossRanges);
        result.AddRange(squareRanges);
        result.AddRange(ractangleRanges);
        result.AddRange(strRanges);
        return result;
    }
}

[System.Serializable]
public abstract class RangeOption
{
    public EAddType addType = EAddType.Add;
    public abstract IEnumerable<Vector2Int> GetPosKeys(Vector2Int centerPos);

    protected int GetMaxCount(EDirection direction)
    {
        Vector2Int mapSize = GetMapSize();
        if (direction == EDirection.Left || direction == EDirection.Right) return mapSize.x;
        else return mapSize.y;
    }

    protected Vector2Int GetMapSize()
    {
        return MapManager.Inst.GetCurrentMapData().mapData.mapSize;
    }
}

[System.Serializable]
public class LineRange : RangeOption
{
    public EDirection direction = EDirection.None;
    public bool isMaxCount = false;
    public int count;
    bool plusReflect;

    public override IEnumerable<Vector2Int> GetPosKeys(Vector2Int centerPos)
    {
        int realCount = isMaxCount ? GetMaxCount(direction) : count;
        return PosKeyUtil.Line(centerPos, direction, realCount, plusReflect);
    }
}

[System.Serializable]
public class CrossRange : RangeOption
{
    public bool isMaxCount = false;
    public int count;
    public bool xCross;
    public override IEnumerable<Vector2Int> GetPosKeys(Vector2Int centerPos)
    {
        Vector2Int mapSize = GetMapSize();
        int realCount = isMaxCount ? Mathf.Max(mapSize.x, mapSize.y) : count;
        if (xCross) return PosKeyUtil.XCross(centerPos, realCount);
        else return PosKeyUtil.Cross(centerPos, realCount);
    }
}

[System.Serializable]
public class SquareRange : RangeOption
{
    public bool isMaxCount = false;
    public int size;
    public bool isBorder;
    public bool isRotated;
    public override IEnumerable<Vector2Int> GetPosKeys(Vector2Int centerPos)
    {
        Vector2Int mapSize = GetMapSize();
        int realSize = isMaxCount ? Mathf.Max(mapSize.x, mapSize.y) : size;
        if (isRotated) return PosKeyUtil.RotatedSquare(centerPos, realSize, isBorder);
        else return PosKeyUtil.Square(centerPos, realSize, isBorder);
    }
}

[System.Serializable]
public class RactangleRange : RangeOption
{
    public bool isMaxCount = false;
    public int width;
    public int height;
    public bool isBorder;

    public override IEnumerable<Vector2Int> GetPosKeys(Vector2Int centerPos)
    {
        Vector2Int mapSize = GetMapSize();
        int realWidth = isMaxCount ? mapSize.x : width;
        int realHeight = isMaxCount ? mapSize.y : height;
        return PosKeyUtil.Rectangle(centerPos, realWidth, realHeight, isBorder);
    }
}

[System.Serializable]
public class StrRange : RangeOption
{
    [TextArea]
    public string strPattern;
    public override IEnumerable<Vector2Int> GetPosKeys(Vector2Int centerPos)
    {
        return PosKeyUtil.StrPattern(centerPos, strPattern);
    }
}

public enum EAddType
{
    None,
    Add,
    Sub
}

public enum ECenterType
{
    None,
    Owner,
    Player,
    MapCenter,
    PosKey
}

public enum ESearchType
{
    None,
    Nearest, // ���� ����� ��
    All, // ��� �༮
}