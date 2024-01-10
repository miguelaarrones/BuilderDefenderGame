using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingDemolishButton : MonoBehaviour
{
    [SerializeField] private Button demolishButton;
    [SerializeField] private Building building;

    private void Awake()
    {
        demolishButton.onClick.AddListener(() =>
        {
            BuildingTypeSO buildingTypeSO = building.GetComponent<BuildingTypeHolder>().buildingType;
            foreach(ResourceAmount resourceAmount in buildingTypeSO.constructionResourceCostArray)
            {
                ResourceManager.Instance.AddResource(resourceAmount.resourceTypeSO, Mathf.FloorToInt(resourceAmount.amount / .6f));
            }
            Destroy(building.gameObject);
        });
    }
}
