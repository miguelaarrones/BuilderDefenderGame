using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingRepairButton : MonoBehaviour
{
    [SerializeField] private Button repairButton;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private ResourceTypeSO goldResourceType;

    private void Awake()
    {
        repairButton.onClick.AddListener(() =>
        {
            int missingHealth = healthSystem.GetHealthAmountMax() - healthSystem.GetHealthAmount();
            int repairCost = missingHealth / 2;

            ResourceAmount[] resourceAmountCost = new ResourceAmount[] { new ResourceAmount { resourceTypeSO = goldResourceType, amount = repairCost } };
            if (ResourceManager.Instance.CanAfford(resourceAmountCost))
            {
                // Can afford the repairs.
                ResourceManager.Instance.SpendResources(resourceAmountCost);
                healthSystem.HealFull();
            } else
            {
                // Cannot afford the repairs;
                TooltipUI.Instance.Show("Cannot afford the repair cost!", new TooltipUI.TooltipTimer { timer = 2f });
            }
        });
    }
}
