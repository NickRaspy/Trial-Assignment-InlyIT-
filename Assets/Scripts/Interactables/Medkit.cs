using UnityEngine;

namespace TA_IT.Interactables
{
    public class Medkit : Interactable
    {
        [SerializeField] private int healAmount = 10;
        public override void Interact(Character character)
        {
            character.GetModule<HealthModule>().Health += healAmount;
            base.Interact(character);
        }
    }
}

