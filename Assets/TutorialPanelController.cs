using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPanelController : MonoBehaviour
{
    public static TutorialPanelController Instance { get; private set; }
    
    public Image tutorialPanel;
    public TextMeshProUGUI textbox;

    public float maxAlpha;
    public float alphaSmoothing;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public IEnumerator ShowPanel(string newText)
    {
        textbox.text = newText;

        Color panelColor = tutorialPanel.color;
        Color textColor = textbox.color;
        
        while (panelColor.a < maxAlpha)
        {
            panelColor.a = Mathf.Lerp(panelColor.a, maxAlpha + 0.1f, alphaSmoothing * Time.unscaledDeltaTime);
            textColor.a = Mathf.Lerp(panelColor.a, 1.1f, alphaSmoothing * Time.unscaledDeltaTime);
            
            tutorialPanel.color = panelColor;
            textbox.color = textColor;

            yield return null;
        }
    }

    public void ChangeText(string newText)
    {
        textbox.text = newText;
    }

    public IEnumerator HidePanel()
    {
        Color panelColor = tutorialPanel.color;
        Color textColor = textbox.color;
        
        while (panelColor.a > 0)
        {
            panelColor.a = Mathf.Lerp(panelColor.a, -.1f, alphaSmoothing * Time.unscaledDeltaTime);
            textColor.a = Mathf.Lerp(panelColor.a, -.1f, alphaSmoothing * Time.unscaledDeltaTime);
            
            tutorialPanel.color = panelColor;
            textbox.color = textColor;

            yield return null;
        }
        
        textbox.text = "";
    }
}
