using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarHelper : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerCountText;
    [SerializeField] TextMeshProUGUI[] textElements;
    
    Color lowHealthColour;
    Color highHealthColour;
    
    Gradient gradient;
    
    public Image progressBar;
    
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTextVisibility(bool isVisible)
    {
        foreach (var textElement in textElements)
        {
            textElement.alpha = isVisible ? 255f : 0f;
        }
    }

    public void SetPlayerCountText(int playerCount)
    {
        playerCountText.text = playerCount.ToString();
    }
    
    public void SetProgressBarFill(float progressBarFill)
    {
        if (progressBarFill == 0)
        {
            progressBar.enabled = false;
        }else if (progressBarFill < 1 && progressBarFill > 0)
        {
            progressBar.enabled = true;
            progressBar.fillAmount = progressBarFill;
            EvaluateColour();
        }
    }
    
    void EvaluateColour()
    {
        if (progressBar != null)
        {
            progressBar.color = gradient.Evaluate(progressBar.fillAmount);
        }
    }

}
