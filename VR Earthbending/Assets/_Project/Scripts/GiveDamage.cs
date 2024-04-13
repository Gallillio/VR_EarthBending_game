using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveDamage : MonoBehaviour
{
    [SerializeField] private int damangeDelt;
    private EnemyAI enemyAIscript;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            //when enemy is still alive
            enemyAIscript = other.GetComponent<EnemyAI>();
            if (enemyAIscript != null)
            {
                enemyAIscript.TakeDamange(damangeDelt, gameObject.transform.forward);
                Destroy(gameObject);
            }
            //when enemy is dead and need to be in ragdoll state
            else
            {
                enemyAIscript = other.transform.parent.parent.gameObject.GetComponent<EnemyAI>();

                enemyAIscript.TakeDamange(damangeDelt, gameObject.transform.forward);
                Destroy(gameObject);
            }


        }
    }
}
