using UnityEngine;

public interface IMovable
{
    public bool Move(Vector2Int target);
    public bool Knockback(EDirection dir, int amount);
}
