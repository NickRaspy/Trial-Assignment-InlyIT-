using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TA_IT
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        private Character playerCharacter;

        //Parameters
        [Header("Parameters")]
        [SerializeField] private float speed = 5;
        [SerializeField] private float sensitivity = 200;
        [SerializeField][Min(1)] private int health = 100;
        [SerializeField] private float interactionDistance = 1f;

        //Camera and some parameters
        [Header("Camera")]
        [SerializeField] private Transform followingCamera;
        [SerializeField] private float cameraAngle;
        [SerializeField] private float cameraOffsetZ;
        [SerializeField] private float cameraHeight;

        //UI element related to player
        [Header("UI")]
        [SerializeField] private Text healthText;
        [SerializeField] private Text interactionText;

        //Input
        private PlayerInputSettings input;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            input = new();
            input.Enable();

            playerCharacter = new();
            playerCharacter.AddModule(new HealthModule(health, ResetScene, HealthTextChange));
            playerCharacter.AddModule(new ControlledMovementModule(input, GetComponent<Rigidbody>(), speed, sensitivity));
            playerCharacter.AddModule(new InteractionModule(input, transform, interactionDistance, InteractionTextChange));

            if (followingCamera != null)
            {
                if (followingCamera.parent != transform) followingCamera.parent = transform;
                followingCamera.eulerAngles = new Vector3(cameraAngle, 0f, 0f);
                followingCamera.localPosition = new(0f, cameraHeight, -cameraOffsetZ);
            }
        }

        void Update() => playerCharacter?.Update();

        void ResetScene()
        {
            input.Disable();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        void HealthTextChange() => healthText.text = $"нг: {playerCharacter.GetModule<HealthModule>()?.Health}";
        void InteractionTextChange() => interactionText.text = playerCharacter.GetModule<InteractionModule>()?.InteractableText;

    }
}