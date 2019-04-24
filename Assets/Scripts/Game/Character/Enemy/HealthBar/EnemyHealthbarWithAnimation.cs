using UnityEngine;
using System.Collections;

public class EnemyHealthbarWithAnimation : EnemyHealthbar {

    private Animation2D animation2D;

    public override void Awake() {
        base.Awake();
        animation2D = healthbarSprite.GetComponent<Animation2D>();
    }

    public override void UpdateHealthbar(float currentAmount, int maxAmount) {

        float calculatedAmount = ((currentAmount / (float)maxAmount) * 10);
        int newFrame = Mathf.FloorToInt(calculatedAmount);

        if(newFrame >= animation2D.frames.Length) {
            animation2D.SetCurrentFrame(animation2D.frames.Length - 1);
        } else if(newFrame > -1) {
            animation2D.SetCurrentFrame(newFrame);
        } else {
            animation2D.SetCurrentFrame(0);
        }

        healthbarSprite.color = GetColorInBetween(currentAmount, (float)maxAmount);
    }
}
