using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChomperView : EnemyView
{
    public void OnDieAnimationFinished()
    {
        _enemy.Recycle();
    }
}
