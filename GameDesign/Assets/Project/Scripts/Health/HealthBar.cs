using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField] Image healthbarImage;

    private StatsEntityFinal statsEntity;
    private Damageable damageable;

    void Start()
    {
        statsEntity = GetComponentInParent<StatsEntityFinal>();
        damageable = GetComponentInParent<Damageable>();
    }

    void Update()
    {
        if (statsEntity == null || damageable == null)
        {
            return;
        }

        Debug.Log(damageable.CurrentHealth);
        
        healthbarImage.fillAmount = damageable.CurrentHealth / statsEntity.MaxHealth;
    }


}