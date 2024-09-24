using UnityEngine;

public interface IMovable
{
    public void Move(Vector2Int target);
    public void Knockback(EDirection dir, int amount);
}
