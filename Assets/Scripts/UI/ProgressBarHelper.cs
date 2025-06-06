using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarHelper : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI displayText;
    
    Color lowHealthColour;
    Color highHealthColour;
    
    Gradient gradient = null;
    
    public Image progressBar;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetGradient();
        SetTextVisibility(true);
    }

    public void SetTextVisibility(bool isVisible)
    {
        displayText.alpha = isVisible ? 255f : 0f;
    }

    public void SetProgressBarText(string text)
    {
        displayText.text = text;
    }
    
    public void SetProgressBarFill(float progressBarFill)
    {
        if (progressBarFill < 1 && progressBarFill > 0)
        {
            progressBar.fillAmount = progressBarFill;
            EvaluateColour();
        }
    }
    
    void EvaluateColour()
    {
        if (progressBar != null)
        {
            if (gradient == null)
            {
                SetGradient();
            }
            progressBar.color = gradient.Evaluate(progressBar.fillAmount);
        }
    }

    void SetGradient()
    {
        gradient = new Gradient();
        GradientColorKey[] colours =  new GradientColorKey[3];
        colours[0] = new GradientColorKey(Color.red, 0f);
        colours[1] = new GradientColorKey(Color.yellow, 0.5f);
        colours[2] = new GradientColorKey(Color.green, 1f);
        GradientAlphaKey[] alphas = new GradientAlphaKey[2];
        alphas[0] = new GradientAlphaKey(255, 0);
        alphas[1] = new GradientAlphaKey(255, 1);
        gradient.SetKeys(colours, alphas);
    }

}
