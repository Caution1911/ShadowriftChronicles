using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Loyalty")]
    public Image loyaltyFill;
    public TextMeshProUGUI loyaltyText;

    [Header("Score")]
    public TextMeshProUGUI scoreText;

    [Header("Stance Icons")]
    public Image ravagerIcon;
    public Image sentinelIcon;
    public Image harmonistIcon;
    public Color activeColor = Color.white;
    public Color inactiveColor = new Color(1, 1, 1, 0.35f);

    [Header("Player Health")]
    public Image healthFill;
    public TextMeshProUGUI healthText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void UpdateLoyalty(float value)
    {
        if (loyaltyFill != null) loyaltyFill.fillAmount = value / 100f;
        if (loyaltyText != null) loyaltyText.text = $"Loyalty: {Mathf.RoundToInt(value)}";
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null) scoreText.text = $"Score: {score}";
    }

    public void UpdateStance(Stance stance)
    {
        ravagerIcon.color = stance == Stance.Ravager ? activeColor : inactiveColor;
        sentinelIcon.color = stance == Stance.Sentinel ? activeColor : inactiveColor;
        harmonistIcon.color = stance == Stance.Harmonist ? activeColor : inactiveColor;
    }

    public void UpdateHealth(float current, float max)
    {
        if (healthFill != null) healthFill.fillAmount = current / max;
        if (healthText != null) healthText.text = $"{Mathf.RoundToInt(current)} / {Mathf.RoundToInt(max)}";
    }
}
