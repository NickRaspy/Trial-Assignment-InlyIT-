using UnityEngine;

namespace TA_IT
{
    [RequireComponent(typeof(BoxCollider))]
    public class PlayerReturner : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            other.transform.position = Vector3.up;
        }
    }
}
