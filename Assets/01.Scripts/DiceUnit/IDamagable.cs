public interface IDamagable
{
    public int CurHP { get; set; }
    public int MaxHP { get; set; }

    public void Damage(int damage);
}