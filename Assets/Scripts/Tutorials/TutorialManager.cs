using UnityEngine;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public TutorialGroup tutorialGroup;

    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("KeyDown");
        }
    }
    void Start()
    {
        tutorialGroup.Initialize();
        UpdateContent();
    }

    public void LastTutorial()
    {
        tutorialGroup.GoBack();
        UpdateContent();
        Debug.Log("Back");
    }
    public void NextTutorial()
    {
        Debug.Log("Next");
        tutorialGroup.GoToNextTutorial();
        UpdateContent();
    
    }

    void UpdateContent()
    {
        TutorialObject current = tutorialGroup.CurrentTutorial;

        if(current != null)
        {
            titleText.text = current.title;
            descriptionText.text = current.description;
        }
    }
}
