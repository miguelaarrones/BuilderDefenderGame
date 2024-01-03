using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderKeywordFilter;
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

    private BuildingTypeListSO buildingTypeList;
    private BuildingTypeSO activeBuildingType;

    private void Awake()
    {
        Instance = this;

        buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (activeBuildingType != null && CanSpawnBuilding(activeBuildingType, UtilsClass.GetMouseWorldPosition()))
            {
                Instantiate(activeBuildingType.prefab, UtilsClass.GetMouseWorldPosition(), Quaternion.identity);
            }
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

    private bool CanSpawnBuilding(BuildingTypeSO buildingTypeSO, Vector3 position)
    {
        BoxCollider2D boxCollider2D = buildingTypeSO.prefab.GetComponent<BoxCollider2D>();

        Collider2D[] collider2DArray = Physics2D.OverlapBoxAll(position + (Vector3)boxCollider2D.offset, boxCollider2D.size, 0);

        bool isAreaClear = collider2DArray.Length == 0;
        if (!isAreaClear) return false;

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
                    return false;
                }
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
                return true;
            }
        }

        return false;
    }
}
