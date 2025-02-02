using UnityEngine;

public class EnemyHealthBar: MonoBehaviour
{
    private Transform bar;
    private Health enemyHealth;

    void Start()
    {
        bar = transform.Find("Bar");
        enemyHealth = GetComponentInParent<Health>();
    }

    void Update()
    {
        if(enemyHealth != null)
        {
            float healthPercentage = enemyHealth.GetHealthPercentage();
            bar.localScale = new Vector3(healthPercentage, 1f, 1f);
        }
    }
}
