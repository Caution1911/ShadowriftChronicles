using UnityEngine;
using UnityEngine.UI;

public class LoyaltyManager : MonoBehaviour
{
    [Range(0, 100)]
    public float loyalty = 65f;
    public Image loyaltyBarFill;

    public void ModifyLoyalty(float amount)
    {
        loyalty = Mathf.Clamp(loyalty + amount, 0f, 100f);
        if (loyaltyBarFill != null)
            loyaltyBarFill.fillAmount = loyalty / 100f;
    }
}
