    using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingGhost : MonoBehaviour
{
    [SerializeField] private Transform spriteGameObject;
    [SerializeField] private ResourceNearbyOverlay resourceNearbyOverlay;

    private void Awake()
    {
        Hide();
    }

    private void Start()
    {
        BuildingManager.Instance.OnActiveBuildingTypeChange += BuildingManager_OnActiveBuildingTypeChange;
    }

    private void BuildingManager_OnActiveBuildingTypeChange(object sender, BuildingManager.OnActiveBuildingTypeChangeArgs args)
    {
        if (args.activeBuildingType == null)
        {
            Hide();
            resourceNearbyOverlay.Hide();
        } 
        else
        {
            Show(args.activeBuildingType.sprite);
            resourceNearbyOverlay.Show(args.activeBuildingType.resourceGeneratorData);
        }
    }

    private void Update()
    {
        transform.position = UtilsClass.GetMouseWorldPosition();
    }

    private void Show(Sprite ghostSprite)
    {
        spriteGameObject.gameObject.SetActive(true);
        spriteGameObject.GetComponent<SpriteRenderer>().sprite = ghostSprite;
    }

    private void Hide()
    {
        spriteGameObject.gameObject.SetActive(false);
    }
}
