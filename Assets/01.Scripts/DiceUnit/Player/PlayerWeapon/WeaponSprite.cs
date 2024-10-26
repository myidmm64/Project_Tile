using AllIn1SpriteShader;
using DG.Tweening;
using UnityEngine;

public class WeaponSprite : MonoBehaviour
{
    public SpriteRenderer spriteRenderer { get; private set; }
    public Animator animator { get; private set; }

    [SerializeField]
    private PlayerWeapon _weapon = null;

    [SerializeField]
    private float _fadeMax = 0.46f;
    [SerializeField]
    private float _fadeinDuration = 0.1f;
    [SerializeField]
    private float _fadeoutDuration = 0.1f;
    private Sequence _fadeSeq = null;

    private void Awake()
    {
        if(_weapon == null)
            _weapon = transform.GetComponentInParent<PlayerWeapon>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void Spawn()
    {
        // _weapon.SpawnAttackObj();
    }

    public void FadeIn()
    {
        Fade(_fadeMax, 0f, _fadeinDuration);
    }

    public void FadeOut()
    {
        Fade(0f, _fadeMax, _fadeoutDuration);
    }

    public void Fade(float start, float target, float duration)
    {
        if(_fadeSeq != null && _fadeSeq.active)
        {
            _fadeSeq.Kill();
        }

        spriteRenderer.material.SetFloat("_FadeAmount", start);
        _fadeSeq = DOTween.Sequence();
        _fadeSeq.Append(DOTween.To(() => start, 
            x=> 
            {
                spriteRenderer.material.SetFloat("_FadeAmount", x); 
            }, target, duration));
    }
}
