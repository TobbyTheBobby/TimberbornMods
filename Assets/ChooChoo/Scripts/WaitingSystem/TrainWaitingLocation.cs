using Timberborn.BaseComponentSystem;
using UnityEngine;

namespace ChooChoo.WaitingSystem
{
    public class TrainWaitingLocation : BaseComponent
    {
        private GameObject Occupant { get; set; }
        public bool Occupied => Occupant != null;

        public void Occupy(GameObject occupant)
        {
            Occupant = occupant;
        }

        public void UnOccupy()
        {
            Occupant = null;
        }
    }
}