using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class BuildingConstruction : MonoBehaviour
{
    public static BuildingConstruction Create(Vector3 position, BuildingTypeSO buildingTypeSO)
    {
        Transform buildingConstructionPrefab = Resources.Load<Transform>("BuildingConstruction");
        Transform buildingConstructionTransform = Instantiate(buildingConstructionPrefab, position, Quaternion.identity);

        BuildingConstruction buildingConstruction = buildingConstructionTransform.GetComponent<BuildingConstruction>();
        buildingConstruction.SetBuildingType(buildingTypeSO);

        return buildingConstruction;
    }

    private float constructionTimer;
    private float constructionTimerMax;
    private BuildingTypeSO buildingTypeSO;
    private BoxCollider2D boxCollider2D;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private BuildingTypeHolder buildingTypeHolder;
    private Material constructionMaterial;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        buildingTypeHolder = GetComponent<BuildingTypeHolder>();
        constructionMaterial = spriteRenderer.material;
    }

    private void Update()
    {
        constructionTimer -= Time.deltaTime;

        constructionMaterial.SetFloat("_Progress", GetConstructionTimerNormalized());

        if (constructionTimer <= 0)
        {
            Debug.Log("DING!");
            Instantiate(buildingTypeSO.prefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void SetBuildingType(BuildingTypeSO buildingTypeSO)
    {
        this.buildingTypeSO = buildingTypeSO;
        
        constructionTimerMax = buildingTypeSO.constructionTimerMax;
        constructionTimer = constructionTimerMax;

        spriteRenderer.sprite = buildingTypeSO.sprite;

        boxCollider2D.offset = buildingTypeSO.prefab.GetComponent<BoxCollider2D>().offset;
        boxCollider2D.size = buildingTypeSO.prefab.GetComponent<BoxCollider2D>().size;

        buildingTypeHolder.buildingType = buildingTypeSO;
    }

    public float GetConstructionTimerNormalized() => 1 - constructionTimer / constructionTimerMax;
}