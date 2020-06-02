using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : Enemy
{

    public override void Attack()
    {
        if (Vector3.Distance(player.position, self.transform.position) > attackRange)
        {
            return;
        }
        Debug.Log("Action 1");

        base.Attack();

        Debug.Log("Action 2");
    }
    public void SlingShotAttack()
    {
        int critChance = Random.Range(0, 21);
        float critDamage = 0;
        if (critChance == 20)
        {
            critDamage = Random.Range(baseDamage / 2, baseDamage * difficulty);
        }
        Debug.Log("SlingShot");
        player.GetComponent<PlayerController>().DamagePlayer(baseDamage * difficulty + critDamage);
    }
}