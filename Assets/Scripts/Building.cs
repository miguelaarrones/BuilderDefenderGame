using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    private HealthSystem healthSystem;
    private BuildingTypeSO buildingType;
    [SerializeField] private Transform buildingDemolishButton;
    [SerializeField] private Transform buildingRepairButton;

    private void Awake()
    {
        HideBuildingDemolishButton();
        HideBuildingRepairButton();
    }

    // Start is called before the first frame update
    void Start()
    {
        buildingType = GetComponent<BuildingTypeHolder>().buildingType;
        healthSystem = GetComponent<HealthSystem>();

        healthSystem.SetHealthAmountMax(buildingType.healthAmountMax, true);
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;

        healthSystem.OnDied += HealthSystem_OnDied;
    }

    private void HealthSystem_OnHealed(object sender, System.EventArgs e)
    {
        if (healthSystem.IsFullHealth())
        {
            HideBuildingRepairButton();
        }
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        ShowBuildingRepairButton();
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDamaged);

        CinemachineShake.Instance.ShakeCamera(7f, .15f);
        ChromaticAberrationScript.Instance.SetWeight(1f);
    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e)
    {
        Instantiate(Resources.Load<Transform>("BuildingDestroyedParticles"), transform.position, Quaternion.identity);
        Destroy(gameObject);
        SoundManager.Instance.PlaySound(SoundManager.Sound.BuildingDestroyed);

        CinemachineShake.Instance.ShakeCamera(10f, .2f);
        ChromaticAberrationScript.Instance.SetWeight(1f);
    }

    private void OnMouseEnter()
    {
        ShowBuildingDemolishButton();
    }

    private void OnMouseExit()
    {
        HideBuildingDemolishButton();
    }

    private void ShowBuildingDemolishButton()
    {
        if (buildingDemolishButton != null)
        {
            buildingDemolishButton.gameObject.SetActive(true);
        }
    }

    private void HideBuildingDemolishButton()
    {
        if (buildingDemolishButton != null)
        {
            buildingDemolishButton.gameObject.SetActive(false);
        }
    }

    private void ShowBuildingRepairButton()
    {
        if (buildingRepairButton != null)
        {
            buildingRepairButton.gameObject.SetActive(true);
        }
    }

    private void HideBuildingRepairButton()
    {
        if (buildingRepairButton != null)
        {
            buildingRepairButton.gameObject.SetActive(false);
        }
    }
}
