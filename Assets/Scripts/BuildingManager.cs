using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance {  get; private set; }

    public event EventHandler<OnActiveBuildingTypeChangeArgs> OnActiveBuildingTypeChange;
    public class OnActiveBuildingTypeChangeArgs : EventArgs
    {
        public BuildingTypeSO activeBuildingType;
    }

    [SerializeField] private Building hqBuilding;

    private BuildingTypeListSO buildingTypeList;
    private BuildingTypeSO activeBuildingType;

    private void Awake()
    {
        Instance = this;

        buildingTypeList = GameAssets.Instance.buildingTypeListSO;
    }

    private void Start()
    {
        hqBuilding.GetComponent<HealthSystem>().OnDied += HQ_OnDied;
    }

    private void HQ_OnDied(object sender, EventArgs e)
    {
        GameOverUI.Instance.Show();
        SoundManager.Instance.PlaySound(SoundManager.Sound.GameOver);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (activeBuildingType != null)
            {
                if (CanSpawnBuilding(activeBuildingType, UtilsClass.GetMouseWorldPosition(), out string errorMessage))
                {
                    if (ResourceManager.Instance.CanAfford(activeBuildingType.constructionResourceCostArray))
                    {
                        ResourceManager.Instance.SpendResources(activeBuildingType.constructionResourceCostArray);
                        //Instantiate(activeBuildingType.prefab, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
                        BuildingConstruction.Create(UtilsClass.GetMouseWorldPosition(), activeBuildingType);
                        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingPlaced);
                    } else
                    {
                        TooltipUI.Instance.Show($"Cannot afford {activeBuildingType.GetConstructionResourceCostString()}", new TooltipUI.TooltipTimer { timer = 2f });
                    }
                }
                else
                {
                    TooltipUI.Instance.Show(errorMessage, new TooltipUI.TooltipTimer { timer = 2f });
                }
            }
        }

        if (Input.GetMouseButtonDown(1) && activeBuildingType != null)
        {
            SetActiveBuildingType(null);
        }
    }

    public void SetActiveBuildingType(BuildingTypeSO buildingType)
    {
        activeBuildingType = buildingType;
        OnActiveBuildingTypeChange?.Invoke(this, new OnActiveBuildingTypeChangeArgs
        {
            activeBuildingType = activeBuildingType
        });
    }

    public BuildingTypeSO GetActiveBuildingType() => activeBuildingType;

    private bool CanSpawnBuilding(BuildingTypeSO buildingTypeSO, Vector3 position, out string errorMessage)
    {
        BoxCollider2D boxCollider2D = buildingTypeSO.prefab.GetComponent<BoxCollider2D>();

        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0);

        bool isAreaClear = collider2DArray.Length == 0;
        if (!isAreaClear)
        {
            errorMessage = "Area is not clear!";
            return false;
        }

        collider2DArray = Physics2D.OverlapCircleAll(position, buildingTypeSO.minConstructionRadius);

        foreach (Collider2D collider in collider2DArray)
        {
            // Colliders inside construction radius.
            BuildingTypeHolder buildingTypeHolder = collider.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder != null)
            {
                // Has a BuildingTypeHolder
                if (buildingTypeHolder.buildingType == buildingTypeSO)
                {
                    // There's already a building of this type within the construction radius.
                    errorMessage = "Too close to another building of the same type!";
                    return false;
                }
            }
        }

        if (buildingTypeSO.hasResourceGeneratorData)
        {
            ResourceGeneratorData resourceGeneratorData = buildingTypeSO.resourceGeneratorData;
            int nearbyResourceAmount = ResourceGenerator.GetNearbyResourceAmount(resourceGeneratorData, position);

            if (nearbyResourceAmount == 0)
            {
                errorMessage = "There are no nearby Resource Nodes";
                return false;
            }
        }

        float maxConstructionRadius = 25f;
        collider2DArray = Physics2D.OverlapCircleAll(position, maxConstructionRadius);

        foreach (Collider2D collider in collider2DArray)
        {
            // Colliders inside construction radius.
            BuildingTypeHolder buildingTypeHolder = collider.GetComponent<BuildingTypeHolder>();
            if (buildingTypeHolder != null)
            {
                // It's a building
                errorMessage = "";
                return true;
            }
        }

        errorMessage = "Too far from any other building!";
        return false;
    }

    public Building GetHQBuilding() => hqBuilding;
}
