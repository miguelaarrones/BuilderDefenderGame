using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance { get; private set; }

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private RectTransform canvasRectTransform;
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private RectTransform backgroundRectTransform;

    private TooltipTimer tooltipTimer;

    private void Awake()
    {
        Instance = this;

        Hide();
    }

    private void Update()
    {
        HandleFollowMouse();

        if (tooltipTimer != null)
        {
            tooltipTimer.timer -= Time.deltaTime;
            if (tooltipTimer.timer <= 0)
            {
                Hide();
            }
        }
    }

    private void HandleFollowMouse()
    {
        Vector2 anchoredPosition = Input.mousePosition / canvasRectTransform.localScale.x;

        if ((anchoredPosition.x + backgroundRectTransform.rect.width) > canvasRectTransform.rect.width)
        {
            anchoredPosition.x = canvasRectTransform.rect.width - backgroundRectTransform.rect.width;
        }

        if ((anchoredPosition.y + backgroundRectTransform.rect.height) > canvasRectTransform.rect.height)
        {
            anchoredPosition.y = canvasRectTransform.rect.height - backgroundRectTransform.rect.height;
        }

        rectTransform.anchoredPosition = anchoredPosition;
    }

    private void SetText(string tooltipText)
    {
        textMeshPro.SetText(tooltipText);
        textMeshPro.ForceMeshUpdate();

        Vector2 textSize = textMeshPro.GetRenderedValues(false);
        Vector2 padding = new Vector2(8f, 8f);
        backgroundRectTransform.sizeDelta = textSize + padding;
    }

    public void Show(string tooltipText, TooltipTimer tooltipTimer = null)
    {
        this.tooltipTimer = tooltipTimer;

        SetText(tooltipText);
        gameObject.SetActive(true);
        HandleFollowMouse();
    }
    public void Hide() 
    {
        gameObject.SetActive(false);
    }

    public class TooltipTimer
    {
        public float timer;
    }
}
