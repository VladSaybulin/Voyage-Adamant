using System.Collections;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Bomb: Bonus
    {

        private const int DiamondLayerMask = 1 << 6;
        private const int BonusLayerMask = 1 << 7;
        
        [SerializeField] private float radius;

        [SerializeField] private ParticleSystem explosionParticleSystem;

        private SpriteRenderer _spriteRenderer;
        private GameObject _gameObject;

        protected override void Awake()
        {
            base.Awake();
            _gameObject = gameObject;
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected override void Activation()
        {
            StopMoving();
            
            var position = ThisTransform.position;
            var diamondHits = Physics2D.CircleCastAll(position, radius, Vector2.zero, 100, DiamondLayerMask);
            var bonusHits = Physics2D.CircleCastAll(position, radius, Vector2.zero, 100, BonusLayerMask);

            foreach (var diamondHit in diamondHits)
            {
                diamondHit.collider.gameObject.GetComponent<Diamond>().TakeDamage(99);
            }

            foreach (var bonusHit in bonusHits)
            {
                var colliderGameObject = bonusHit.collider.gameObject;
                if (colliderGameObject == _gameObject) continue;
                colliderGameObject.GetComponent<Bonus>().Activate();
            }
            
            StartCoroutine(ExplosionAnimationRoutine());
        }

        private IEnumerator ExplosionAnimationRoutine()
        {
            var color = _spriteRenderer.color;
            var mainModule = explosionParticleSystem.main;
            var halfGlobalDuration = (mainModule.duration + mainModule.startLifetime.constantMax) / 2;
            explosionParticleSystem.Play();
            
            var da = 1 / halfGlobalDuration; 
            while (color.a > 0)
            {
                color.a -= Time.deltaTime * da;
                _spriteRenderer.color = color;
                yield return null;
            }

            yield return new WaitForSeconds(halfGlobalDuration);
            Events.Invoke(BonusEvent.Finished, this);
        }
    }
}