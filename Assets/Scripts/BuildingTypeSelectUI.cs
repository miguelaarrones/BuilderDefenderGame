using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingTypeSelectUI : MonoBehaviour
{
    [SerializeField] private Transform buttonTemplate;
    [SerializeField] private Sprite arrowSprite;

    private Dictionary<BuildingTypeSO, Transform> buttonTransformDictionary;
    private Transform cursorButton;

    private void Awake()
    {
        buttonTemplate.gameObject.SetActive(false);

        BuildingTypeListSO buildingTypeList = Resources.Load<BuildingTypeListSO>(typeof(BuildingTypeListSO).Name);

        buttonTransformDictionary = new Dictionary<BuildingTypeSO, Transform>();

        int index = 0;
        
        cursorButton = Instantiate(buttonTemplate, transform);
        cursorButton.gameObject.SetActive(true);

        float offsetAmount = 130f;
        cursorButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);

        cursorButton.Find("Image").GetComponent<Image>().sprite = arrowSprite;
        cursorButton.Find("Image").GetComponent<RectTransform>().sizeDelta = new Vector2(0, -30);

        cursorButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            BuildingManager.Instance.SetActiveBuildingType(null);
        });

        index++;

        foreach (BuildingTypeSO buildingType in buildingTypeList.list)
        {
            Transform buttonTransform = Instantiate(buttonTemplate, transform);
            buttonTransform.gameObject.SetActive(true);

            offsetAmount = 130f;
            buttonTransform.GetComponent<RectTransform>().anchoredPosition = new Vector2(offsetAmount * index, 0);

            buttonTransform.Find("Image").GetComponent<Image>().sprite = buildingType.sprite;

            buttonTransform.GetComponent<Button>().onClick.AddListener(() =>
            {
                BuildingManager.Instance.SetActiveBuildingType(buildingType);
            });

            buttonTransformDictionary[buildingType] = buttonTransform;

            index++;
        }
    }

    private void Start()
    {
        BuildingManager.Instance.OnActiveBuildingTypeChange += BuildingManager_OnActiveBuildingTypeChange;
        UpdateActiveBuildingTypeButton();
    }

    private void BuildingManager_OnActiveBuildingTypeChange(object sender, BuildingManager.OnActiveBuildingTypeChangeArgs e)
    {
        UpdateActiveBuildingTypeButton();
    }

    private void UpdateActiveBuildingTypeButton()
    {
        cursorButton.Find("Selected").gameObject.SetActive(false);

        foreach (BuildingTypeSO buildingType in buttonTransformDictionary.Keys)
        {
            Transform buttonTransform = buttonTransformDictionary[buildingType];
            buttonTransform.Find("Selected").gameObject.SetActive(false);
        }

        BuildingTypeSO activeBuildingType = BuildingManager.Instance.GetActiveBuildingType();
        if (activeBuildingType == null)
        {
            cursorButton.Find("Selected").gameObject.SetActive(true);
        }
        else
        {
            buttonTransformDictionary[activeBuildingType].Find("Selected").gameObject.SetActive(true);
        }
    }
}
