using UnityEngine;

public class TutoSkeletonEnemy : BossEnemy
{
    protected override void Update()
    {
        base.Update();
    }

    protected override void BindPattern()
    {
        _patterns.Add(new TutoSkeleton_TestPattern(this));
    }
}
