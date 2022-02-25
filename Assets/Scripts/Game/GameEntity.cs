using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D;
using Utils;

namespace Game
{
    public abstract class GameEntity: MonoBehaviour
    {

        [SerializeField] private float spawnOffset;
        
        protected Transform ThisTransform;
        private Bounds _spawnBounds;
        
        private Coroutine _movingRoutine;

        private float _scale;
        private float _moveSpeed;
        private float _rotateSpeed;

        public Bounds SpawnBounds
        {
            set => _spawnBounds = value;
        }
        
        public float Scale
        {
            get => _scale;
            set
            {
                _scale = value;
                transform.localScale = new Vector3(value, value);
            }
        }

        public float DurationMove
        {
            set
            {
                _moveSpeed = 1 / value;
                _rotateSpeed = RandomUtility.Sign() * 360 * _moveSpeed;
            }
        }
        

        protected virtual void Awake()
        {
            ThisTransform = GetComponent<Transform>();
        }

        protected void StartMoving(UnityAction onEndMoving)
        {
            StopMoving();
            _movingRoutine = StartCoroutine(MovingRoutine(onEndMoving));
        }

        protected void StopMoving()
        {
            if (_movingRoutine != null)
            {
                StopCoroutine(_movingRoutine);
            }
        }

        private IEnumerator MovingRoutine(UnityAction onEndMoving)
        {
            Vector3 startPosition = RandomUtility.PositionOnBounds(_spawnBounds);
            Vector3 startRightTangent = RandomUtility.PositionInsideBounds(_spawnBounds);
            Vector3 endLeftTangent = RandomUtility.PositionInsideBounds(_spawnBounds);
            Vector3 endPosition = RandomUtility.PositionOnBounds(_spawnBounds);
            
            ThisTransform.position = startPosition;
            ThisTransform.rotation = Quaternion.AngleAxis(Random.Range(-180, 180), Vector3.forward);
            
            yield return null;
            
            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime * _moveSpeed;
                ThisTransform.position = 
                    BezierUtility.BezierPoint(startPosition, startRightTangent, endLeftTangent, endPosition, t);
                ThisTransform.rotation *= Quaternion.AngleAxis(Time.deltaTime * _rotateSpeed, Vector3.forward);
                yield return null;
            }

            _movingRoutine = null;
            onEndMoving?.Invoke();

        }
    }
}