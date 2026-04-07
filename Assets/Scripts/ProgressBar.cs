using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    public TutorialManager tutorialManager; 
    public RectTransform progressBarFill; 
    public RectTransform background;


    public void UpdateProgressBar()
    {
        TutorialGroup group = tutorialManager.currentGroup;

        int currentIndex = group.tutorialIndex;
        int total = group.tutorialObjects.Count;

        float progress = (float)(currentIndex + 1) / total;
        float width = background.rect.width;

        progressBarFill.sizeDelta = new Vector2(width * progress, progressBarFill.sizeDelta.y);
    }
}

