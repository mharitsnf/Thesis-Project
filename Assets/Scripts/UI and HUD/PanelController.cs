using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    public static PanelController Instance { get; private set; }
    
    private TextMeshProUGUI _textbox;
    public CanvasGroup canvasGroup;
    public CanvasGroup indicatorCanvasGroup;

    public float alphaSmoothing;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        _textbox = GetComponentInChildren<TextMeshProUGUI>();
    }

    public IEnumerator ShowPanel(string newText, bool isUsingIndicator = false)
    {
        InteractionController.Instance.playerInput.OtherInteraction.NextPanel.Disable();
        
        _textbox.text = newText;

        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1, alphaSmoothing * Time.unscaledDeltaTime);

            if (canvasGroup.alpha > .95f) canvasGroup.alpha = 1;
            
            yield return null;
        }

        if (!isUsingIndicator) yield break;

        yield return new WaitForSecondsRealtime(2f);

        while (indicatorCanvasGroup.alpha < 1) 
        {
            indicatorCanvasGroup.alpha = Mathf.Lerp(indicatorCanvasGroup.alpha, 1, alphaSmoothing * Time.unscaledDeltaTime);

            if (indicatorCanvasGroup.alpha > .95f) indicatorCanvasGroup.alpha = 1;
            
            yield return null;
        }
        
        InteractionController.Instance.playerInput.OtherInteraction.NextPanel.Enable();
    }

    public IEnumerator HidePanel()
    {
        StartCoroutine(HideNextPanelIndicator());
        yield return StartCoroutine(HideMainPanel());
    }

    private IEnumerator HideMainPanel()
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0, alphaSmoothing * Time.unscaledDeltaTime);

            if (canvasGroup.alpha < .05f) canvasGroup.alpha = 0;
            
            yield return null;
        }
        
        _textbox.text = "";
    }

    private IEnumerator HideNextPanelIndicator()
    {
        while (indicatorCanvasGroup.alpha > 0)
        {
            indicatorCanvasGroup.alpha = Mathf.Lerp(indicatorCanvasGroup.alpha, 0, alphaSmoothing * Time.unscaledDeltaTime);

            if (indicatorCanvasGroup.alpha < .05f) indicatorCanvasGroup.alpha = 0;
            
            yield return null;
        }
    }
}
