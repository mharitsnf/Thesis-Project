using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelController : MonoBehaviour
{
    public static PanelController Instance { get; private set; }
    
    private Image _tutorialPanel;
    private TextMeshProUGUI _textbox;
    public CanvasGroup canvasGroup;

    public float maxAlpha;
    public float alphaSmoothing;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        _tutorialPanel = GetComponent<Image>();
        _textbox = GetComponentInChildren<TextMeshProUGUI>();
    }

    public IEnumerator ShowPanel(string newText)
    {
        _textbox.text = newText;

        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1, alphaSmoothing * Time.unscaledDeltaTime);

            if (canvasGroup.alpha > .95f) canvasGroup.alpha = 1;
            
            yield return null;
        }
    }

    public void ChangeText(string newText)
    {
        _textbox.text = newText;
    }

    public IEnumerator HidePanel()
    {
        
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0, alphaSmoothing * Time.unscaledDeltaTime);

            if (canvasGroup.alpha < .05f) canvasGroup.alpha = 0;
            
            yield return null;
        }
        
        _textbox.text = "";
    }
}
