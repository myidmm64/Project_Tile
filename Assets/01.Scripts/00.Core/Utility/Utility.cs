using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    private static readonly string _skillDataPath = "SkillDataSO";
    private static readonly string _playerWeaponPath = "PlayerWeaponDataSO";

    public static Vector2Int EDirectionToVector(EDirection direction) => direction switch
    {
        EDirection.None => Vector2Int.zero,
        EDirection.Left => Vector2Int.left,
        EDirection.Right => Vector2Int.right,
        EDirection.Up => Vector2Int.up,
        EDirection.Down => Vector2Int.down,
        EDirection.LeftUp => Vector2Int.left + Vector2Int.up,
        EDirection.RightUp => Vector2Int.right + Vector2Int.up,
        EDirection.LeftDown => Vector2Int.left + Vector2Int.down,
        EDirection.RightDown => Vector2Int.right + Vector2Int.down,
        _ => Vector2Int.zero,
    };

    public static int GetMaxCountWithDirection(EDirection direction, Vector2Int mapSize) => direction switch
    {
        EDirection.None => 0,
        EDirection.Left => mapSize.x,
        EDirection.Right => mapSize.x,
        EDirection.Up => mapSize.y,
        EDirection.Down => mapSize.y,
        EDirection.LeftUp => Mathf.Min(mapSize.x, mapSize.y),
        EDirection.RightUp => Mathf.Min(mapSize.x, mapSize.y),
        EDirection.LeftDown => Mathf.Min(mapSize.x, mapSize.y),
        EDirection.RightDown => Mathf.Min(mapSize.x, mapSize.y),
        _ => 0,
    };

    public static EDirection GetReflectedDirection(EDirection direction) => direction switch
    {
        EDirection.None => EDirection.None,
        EDirection.Left => EDirection.Right,
        EDirection.Right => EDirection.Left,
        EDirection.Up => EDirection.Down,
        EDirection.Down => EDirection.Up,
        EDirection.LeftUp => EDirection.RightDown,
        EDirection.RightUp => EDirection.LeftDown,
        EDirection.LeftDown => EDirection.RightUp,
        EDirection.RightDown => EDirection.LeftUp,
        _ => EDirection.None,
    };

    // 우측 기준
    public static float GetZRotate(EDirection direction) => direction switch
    {
        EDirection.None => 0f,
        EDirection.Left => 180f,
        EDirection.Right => 0f,
        EDirection.Up => 90f,
        EDirection.Down => -90f,
        EDirection.LeftUp => 135f,
        EDirection.RightUp => 45f,
        EDirection.LeftDown => -135f,
        EDirection.RightDown => -45f,
        _ => 0f,
    };

    public static Vector2Int GetRotatedVector(Vector2Int start, EDirection dir)
    {
        float rot = GetZRotate(dir);

        // 각도를 라디안으로 변환
        float radians = rot * Mathf.Deg2Rad;

        // 회전 행렬을 사용해 벡터를 회전
        float cos = Mathf.Cos(radians);
        float sin = Mathf.Sin(radians);

        // 새 x, y 값 계산
        int newX = Mathf.RoundToInt(start.x * cos - start.y * sin);
        int newY = Mathf.RoundToInt(start.x * sin + start.y * cos);

        // 새로운 벡터 반환
        return new Vector2Int(newX, newY);
    }

    public static PlayerWeaponDataSO GetPlayerWeaponDataSO(int weaponID)
    {
        string resourcePath = $"{_playerWeaponPath}/{weaponID}";
        return LoadResource<PlayerWeaponDataSO>(resourcePath);
    }

    public static SkillDataSO GetSkillDataSO(int skillID)
    {
        string resourcePath = $"{_skillDataPath}/{skillID}";
        return LoadResource<SkillDataSO>(resourcePath);
    }

    private static T LoadResource<T>(string path) where T : Object
    {
        T data = Resources.Load<T>(path);
        if (data == null)
        {
            Debug.LogError($"리소스를 찾지 못함. path : {path}");
            return null;
        }
        return data;
    }

    public static void SetLocalScaleByDirection(Transform trm, EDirection direction)
    {
        Vector3 localScale = trm.transform.localScale;
        localScale.x = Mathf.Abs(localScale.x);
        localScale.x *= DirectionToXMiltiflier(direction);
        trm.transform.localScale = localScale;
    }

    public static float DirectionToXMiltiflier(EDirection direction)
    {
        return direction == EDirection.Right ? 1f : -1f;
    }
}
