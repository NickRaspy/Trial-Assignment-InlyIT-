using UnityEngine;

namespace TA_IT
{
    public abstract class Interactable : MonoBehaviour
    {
        public string actionText;
        public bool willBeDestroyedOnInteract;
        public virtual void Interact(Character character)
        {
            if (willBeDestroyedOnInteract) Destroy(gameObject);
        }
    }
}