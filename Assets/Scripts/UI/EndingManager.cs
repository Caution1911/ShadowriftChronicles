using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class EndingManager : MonoBehaviour
{
    public GameObject endingPanel;
    public TextMeshProUGUI endingTitle;
    public TextMeshProUGUI endingDescription;

    public void ShowEnding(float finalLoyalty)
    {
        endingPanel.SetActive(true);

        if (finalLoyalty >= 80)
        {
            endingTitle.text = "TRUE BALANCE";
            endingDescription.text = "You walked the razor’s edge between realities and preserved both. The multiverse is stable. Luna remains by your side.";
        }
        else if (finalLoyalty >= 40)
        {
            endingTitle.text = "BITTERSWEET SEVERANCE";
            endingDescription.text = "You chose to cut the connections. The realms are safe… but forever separated. Luna’s light grows distant.";
        }
        else
        {
            endingTitle.text = "TRAGIC UNITY";
            endingDescription.text = "You forced the realities together. The Architect’s will is fulfilled. Luna is lost to the merge.";
        }
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
