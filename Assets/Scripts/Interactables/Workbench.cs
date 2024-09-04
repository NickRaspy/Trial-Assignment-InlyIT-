using UnityEngine;

namespace TA_IT.Interactables
{
    public class Workbench : Interactable
    {
        [SerializeField] private GameObject outcomeObject;
        public override void Interact(Character character)
        {
            outcomeObject.SetActive(true);
            base.Interact(character);
        }
    }
}
