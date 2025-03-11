using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDirection : MonoBehaviour
{
    public GameObject enemySelected;
    Vector3 enemyDirection;

    private void FixedUpdate()
    {
        if (enemySelected != null)
        {
            enemyDirection = enemySelected.transform.position;
            enemyDirection.y += 3f;
            transform.LookAt(enemyDirection);
        }
    }
}
