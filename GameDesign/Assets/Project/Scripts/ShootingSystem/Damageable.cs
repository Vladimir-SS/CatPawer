using UnityEngine;
using UnityEngine.UI;
public class Damageable : MonoBehaviour, IDamageable
{

    [SerializeField] Image healthbarImage;

    private StatsEntity statsEntity;

    void Start()
    {
        statsEntity = GetComponent<StatsEntity>();
    }

    public void TakeDamage(float damage)
    {
        Debug.Log("Took damage amount: " + damage);

        // Modify MaxHealth using the public method
        statsEntity.ModifyMaxHealth(-damage);

        if (!ReferenceEquals(healthbarImage, null))
            healthbarImage.fillAmount = statsEntity.MaxHealth / 100;   // should be currentHealth/MaxHealth,  must be changed

        Debug.Log("Remaining Health: " + statsEntity.MaxHealth);

        if (statsEntity.MaxHealth <= 0)
            Die();
    }


    public void Die()
    {
        Destroy(transform.root.gameObject); // Destroy the root GameObject
        //statsEntity.RemoveStats()  ...
    }


}