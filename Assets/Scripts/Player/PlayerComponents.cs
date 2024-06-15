using Items;
using UnityEngine;

namespace Player
{
    public class PlayerComponents
    {
        public Inventory Inventory { get; }

        public PlayerComponents(GameObject player)
        {
            Inventory = player.GetComponent<Inventory>();
        }
    }
}