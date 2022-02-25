using System;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class Bonus : GameEntity
    {

        public readonly UnityEvent<BonusEvent, Bonus> Events = new UnityEvent<BonusEvent, Bonus>();

        private bool _activated = false;

        public void Activate()
        {
            if(_activated) return;
            _activated = true;
            Activation();
        }

        protected abstract void Activation();
        
        public void StartMoving() => StartMoving(() => Events.Invoke(BonusEvent.Finished, this));

        private void OnMouseDown() => Events.Invoke(BonusEvent.Clicked, this);
        
    }
}