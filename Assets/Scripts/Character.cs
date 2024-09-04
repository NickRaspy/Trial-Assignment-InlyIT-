using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace TA_IT
{
    public class Character
    {
        private List<ICharacterModule> modules = new();

        public void AddModule(ICharacterModule module)
        {
            module.Initialize(this);
            modules.Add(module);
        }

        public void Update()
        {
            foreach (var module in modules)
            {
                module.UpdateModule();
            }
        }
        public T GetModule<T>() where T : ICharacterModule
        {
            foreach (var module in modules)
            {
                if (module is T t)
                {
                    return t;
                }
            }
            Debug.LogError($"Required module wasn't found");
            return default;
        }
    }
    public class HealthModule : CharacterModule
    {
        private int health;

        public int Health
        {
            get { return health; } 
            set 
            { 
                health = value;
                onHealthChange.Invoke();
            }
        }

        public Action onZeroHealth;
        public Action onHealthChange;

        public HealthModule(int startHealth, Action onZeroHealth, Action onHealthChange)
        {
            health = startHealth;
            this.onZeroHealth = onZeroHealth;
            this.onHealthChange = onHealthChange;
        }
        public override void Initialize(Character character)
        {
            base.Initialize(character);
        }

        public override void UpdateModule() 
        {
            if(health <= 0 && onZeroHealth != null)
            {
                onZeroHealth.Invoke();
                onZeroHealth = null;
                onHealthChange = null;
            }
        }
    }
    public class ControlledMovementModule : CharacterModule
    {
        private PlayerInputSettings input;

        private Rigidbody rb;
        private float speed;
        private float sensitivity;

        private float hor;
        private float vert;
        private float mouseX;

        private bool canMove = true;
        public bool CanMove
        {
            get { return canMove; }
            set 
            { 
                canMove = value; 
                rb.isKinematic = !canMove;
            }
        }

        public ControlledMovementModule(PlayerInputSettings input, Rigidbody rb, float speed, float sensitivity)
        {
            this.input = input;
            this.rb = rb;
            this.speed = speed;
            this.sensitivity = sensitivity;
        }

        public override void Initialize(Character character)
        {
            base.Initialize(character);

            //Set direction change on move input pressing and unpressing
            input.BasicControl.Move.performed += move =>
            {
                hor = move.ReadValue<Vector2>().x;
                vert = move.ReadValue<Vector2>().y;
            };
            input.BasicControl.Move.canceled += cancel => hor = vert = 0f;

            //Set rotation change on mouse move
            input.BasicControl.Rotate.performed += rotate => mouseX = rotate.ReadValue<float>();
            input.BasicControl.Rotate.canceled += cancel => mouseX = 0;

        }
        public override void UpdateModule()
        {
            if (canMove) rb.MovePosition(rb.position + speed * Time.deltaTime * (rb.transform.forward * vert + rb.transform.right * hor));
            rb.transform.Rotate(0, mouseX * sensitivity * Time.deltaTime, 0);
        }
        public void SetPosition(Vector3 position) => rb.transform.position = position;
    }

    public class InteractionModule : CharacterModule
    {
        private PlayerInputSettings input;
        private Transform transform;
        private float interactionDistance;

        private Interactable interactable;

        private string interactableText;
        public string InteractableText
        {
            get { return interactableText; }
            set 
            {
                interactableText = value;
                onInteractableChanged.Invoke();
            }
        }

        public Action onInteractableChanged;

        public InteractionModule(PlayerInputSettings input, Transform transform, float interactionDistance, Action onInteractableChanged)
        {
            this.input = input;
            this.transform = transform;
            this.interactionDistance = interactionDistance;
            this.onInteractableChanged = onInteractableChanged;
        }

        public override void Initialize(Character character)
        {
            base.Initialize(character);

            //Set interaction on button
            input.BasicControl.Interact.performed += interaction => Interact();
        }

        public override void UpdateModule() 
        {
            DetectInteractable();
        }

        private void DetectInteractable()
        {
            Ray ray = new(transform.position, transform.forward);
            Debug.DrawRay(transform.position, transform.forward * interactionDistance, Color.red);
            if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
            {
                if (hit.collider.GetComponent<Interactable>() != null)
                {
                    interactable = hit.collider.GetComponent<Interactable>();
                    InteractableText = $"[{input.BasicControl.Interact.bindings[0].path.Split('/').Last().ToUpper()}] " + interactable.actionText;
                }
            }
            else
            {
                interactable = null;
                InteractableText = "";
            }
        }

        public void Interact() => interactable?.Interact(character);

    }
    #region MODULE_BASE
    public abstract class CharacterModule : ICharacterModule
    {
        protected Character character;

        public virtual void Initialize(Character character)
        {
            this.character = character;
        }

        public abstract void UpdateModule();
    }
    public interface ICharacterModule
    {
        void Initialize(Character character);
        void UpdateModule();
    }
    #endregion
}