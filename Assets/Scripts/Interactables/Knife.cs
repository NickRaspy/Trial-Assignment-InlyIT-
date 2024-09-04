using UnityEngine;

namespace TA_IT.Interactables
{
    public class Knife : Interactable
    {
        [SerializeField] private int damageAmount = 10;
        public override void Interact(Character character) 
        {
            character.GetModule<HealthModule>().Health -= damageAmount;
            base.Interact(character);
        }
    }
}

