using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : MonoBehaviour
{
    [SerializeField] private Rigidbody crown;
    private Enemy enemy;

    private void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (enemy.IsDead())
        {
            crown.transform.parent = null;
            crown.isKinematic = false;
        }
    }
}
