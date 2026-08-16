using UnityEngine;
using UnityEngine.UI;

public class AbilityButtonUI : MonoBehaviour
{
    public AbilitySystem abilitySystem;
    public Button button;
    public Image cooldownOverlay;

    void Update()
    {
        if (abilitySystem == null || cooldownOverlay == null) return;

        float percent = abilitySystem.SpecialCooldownPercent;
        cooldownOverlay.fillAmount = percent;

        if (button != null)
            button.interactable = percent <= 0f;
    }

    public void OnAttackButtonPressed() => abilitySystem.TryBasicAttack();
    public void OnSpecialButtonPressed() => abilitySystem.TrySpecialAbility();
}
