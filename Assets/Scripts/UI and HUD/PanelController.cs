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

        Color panelColor = _tutorialPanel.color;
        Color textColor = _textbox.color;
        
        while (panelColor.a < maxAlpha)
        {
            panelColor.a = Mathf.Lerp(panelColor.a, maxAlpha + 0.1f, alphaSmoothing * Time.unscaledDeltaTime);
            textColor.a = Mathf.Lerp(panelColor.a, 1.1f, alphaSmoothing * Time.unscaledDeltaTime);
            
            _tutorialPanel.color = panelColor;
            _textbox.color = textColor;

            yield return null;
        }
    }

    public void ChangeText(string newText)
    {
        _textbox.text = newText;
    }

    public IEnumerator HidePanel()
    {
        Color panelColor = _tutorialPanel.color;
        Color textColor = _textbox.color;
        
        while (panelColor.a > 0)
        {
            panelColor.a = Mathf.Lerp(panelColor.a, -.1f, alphaSmoothing * Time.unscaledDeltaTime);
            textColor.a = Mathf.Lerp(panelColor.a, -.1f, alphaSmoothing * Time.unscaledDeltaTime);
            
            _tutorialPanel.color = panelColor;
            _textbox.color = textColor;

            yield return null;
        }
        
        _textbox.text = "";
    }
}
