using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    [SerializeField] Image healthbarImage;

    private Stats.StatsEntityFinal statsEntity;
    private Damageable damageable;

    void Start()
    {
        statsEntity = GetComponentInParent<Stats.StatsEntityFinal> ();
        damageable = GetComponentInParent<Damageable>();
    }

    void Update()
    {
        if (statsEntity == null || damageable == null)
        {
            return;
        }
        
        healthbarImage.fillAmount = (float)damageable.CurrentHealth / statsEntity.Body.MaxHP;
    }   


}