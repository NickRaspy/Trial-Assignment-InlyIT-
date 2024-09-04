using UnityEngine;

namespace TA_IT.Interactables
{
    public class Tent : Interactable
    {
        public float exitValue = 2f; //Set the position of exit out of Tent from Tent position
        [SerializeField]private bool inTent = false;
        public override void Interact(Character character)
        {
            character.GetModule<ControlledMovementModule>().CanMove = inTent;
            character.GetModule<ControlledMovementModule>().SetPosition(!inTent ? transform.position + Vector3.up : transform.position + Vector3.left*exitValue + Vector3.up);
            inTent = !inTent;
            base.Interact(character);
        }
    }
}
