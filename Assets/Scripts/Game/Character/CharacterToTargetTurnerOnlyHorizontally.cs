using UnityEngine;
using System.Collections;

public class CharacterToTargetTurnerOnlyHorizontally : CharacterToTargetTurner {

   public override void OnUpdate() {
        
        if(target.transform.position.x > this.transform.position.x) {
            bodyControl.SetCurrentDirection(Direction.RIGHT);
        } else {
            bodyControl.SetCurrentDirection(Direction.LEFT);
        } 
    }
}
