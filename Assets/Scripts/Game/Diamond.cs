using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    [RequireComponent(typeof(SpriteRenderer), typeof(ParticleSystem), typeof(Collider2D))]
    public class Diamond: GameEntity
    {

        public readonly UnityEvent<DiamondEvent, Diamond> Events = new UnityEvent<DiamondEvent, Diamond>();

        private IEnumerator _damageAnimationRoutine;
        private IEnumerator _fadeAnimationRoutine;

        private SpriteRenderer _renderer;
        private ParticleSystem _fadeParticleSystem;

        private int _hp;
        
        public int Hp
        {
            set => _hp = value;
        }
        
        public Color Color
        {
            set => _renderer.color = value;
        }
        
        public int Index { get; set; }

        protected override void Awake()
        {
            base.Awake();
            _renderer = GetComponent<SpriteRenderer>();
            _fadeParticleSystem = GetComponent<ParticleSystem>();
        }

        public void TakeDamage(int damage)
        {
            if(_hp > 0)
            {
                _hp -= damage;

                if (_hp <= 0)
                {
                    Events.Invoke(DiamondEvent.Collected, this);
                    StartFadeAnimationRoutine();
                }
                else
                {
                    StartDamageAnimationRoutine();
                }
            }
        }
        
        public void StartMoving() => StartMoving(() => Events.Invoke(DiamondEvent.Finished, this));
        
        private void OnMouseDown() => Events.Invoke(DiamondEvent.Clicked, this);

        private void StartDamageAnimationRoutine()
        {
            if(_damageAnimationRoutine != null)
                StopCoroutine(_damageAnimationRoutine);

            _damageAnimationRoutine = DamageAnimationRoutine();
            StartCoroutine(_damageAnimationRoutine);
        }

        private IEnumerator DamageAnimationRoutine()
        {
            var startScale = new Vector3(Scale, Scale, 1);
            var endScale = startScale * 1.15f;
            endScale.z = 1;

            ThisTransform.localScale = startScale;

            yield return null;

            float t = 0;
            float speed = 10;

            while (t < 1)
            {
                t += Time.deltaTime * speed;
                ThisTransform.localScale = Vector3.Lerp(startScale, endScale, t);
                yield return null;
            }

            while (t > 0)
            {
                t -= Time.deltaTime * speed;
                ThisTransform.localScale = Vector3.Lerp(startScale, endScale, t);
                yield return null;
            }
            
            _damageAnimationRoutine = null;
        }

        private void StartFadeAnimationRoutine()
        {
            if (_fadeAnimationRoutine != null)
                StopCoroutine(_fadeAnimationRoutine);

            StartCoroutine(_fadeAnimationRoutine = FadeAnimationRoutine());
        }

        private IEnumerator FadeAnimationRoutine()
        {
            var color = _renderer.color;
            var main = _fadeParticleSystem.main;
            var halfGlobalDuration = (main.duration + main.startLifetime.constantMax) / 2;
            main.startColor = color;

            _fadeParticleSystem.Play();

            float da = 1 / halfGlobalDuration; 
            while (color.a > 0)
            {
                color.a -= Time.deltaTime * da;
                _renderer.color = color;
                yield return null;
            }
            
            yield return new WaitForSeconds(halfGlobalDuration);
            
            _fadeAnimationRoutine = null;
            Events.Invoke(DiamondEvent.Finished, this);
        }
    }
}