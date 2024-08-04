using UnityEngine;

public interface IDamagable
{
    protected int MaxHP { get; set; }
    protected int CurHP { get; set; }

    public void Damage(int damage);
}
