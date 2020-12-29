using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBodyDetector : MonoBehaviour
{
    public PartOfBody partOfBody;

    private Enemy enemy;

    public enum PartOfBody
    {
        Head,
        Torso,
        Arms,
        Legs
    }

    void Update ()
    {
        enemy = (Enemy)GetComponentInParent(typeof(Enemy));
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Bullet bullet))
        {
            enemy.OnShot(bullet, partOfBody);
        }
    }
}
