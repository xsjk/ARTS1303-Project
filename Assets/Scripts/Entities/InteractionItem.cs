using Player;

namespace Entities
{
    public struct InteractableAction
    {
        // key binding
        public KeyBinding Key;

        // prompt text
        public string Prompt;

        // is the prompt available
        public bool Disabled;
    }

    public interface IInteractable
    {
        public InteractableAction[] AvailableInteractions(PlayerComponents playerComponents);

        public void Interact(KeyBinding key, PlayerComponents playerComponents);
    }
}