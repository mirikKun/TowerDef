using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemView : EnemyView
{
    public void OnDieAnimationFinished()
    {
        _enemy.Recycle();
    }
}
