using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    //public TutorialGroup tutorialGroup;
    public TutorialGroup[] tutorialGroups;
    public TutorialGroup currentGroup;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public ProgressBar progressBar;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("KeyDown");
        }
    }
    void Start()
    {
        currentGroup.Initialize();
        UpdateContent();
    }

    public void LastTutorial()
    {
        currentGroup.GoBack();
        UpdateContent();
        Debug.Log("Back");
    }
    public void NextTutorial()
    {
        Debug.Log("Next");
        currentGroup.GoToNextTutorial();
        UpdateContent();
    
    }

    void UpdateContent()
    {
        TutorialObject current = currentGroup.CurrentTutorial;

        if(current != null)
        {
            titleText.text = current.title;
            descriptionText.text = current.description;
        }
        
        progressBar.UpdateProgressBar();
    }

    public void SwitchGroup(int index)
    {
        currentGroup = tutorialGroups[index];
        currentGroup.Initialize();

        UpdateContent();

        Debug.Log("Switched to group: " + index);
    }
}
