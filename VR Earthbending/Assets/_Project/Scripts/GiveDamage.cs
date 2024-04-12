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
            // Debug.Log("AAAA IT GAVE DAMANGE");
            enemyAIscript = other.GetComponent<EnemyAI>();

            enemyAIscript.TakeDamange(damangeDelt);
            Destroy(gameObject);

        }
    }
}
