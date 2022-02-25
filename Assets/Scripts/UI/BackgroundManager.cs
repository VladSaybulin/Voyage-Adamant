using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI
{
    public class BackgroundManager : MonoBehaviour
    {
        [SerializeField] private List<Sprite> images;
        private void Start()
        {
            Random.InitState(System.Environment.TickCount);
            var thisTransform = GetComponent<Transform>();
            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = images[Random.Range(0, images.Count)];

            var mainCamera = Camera.main;
            var cameraHeight = mainCamera.orthographicSize * 2;
            var cameraSize = new Vector2(mainCamera.aspect * cameraHeight, cameraHeight);
            var spriteSize = spriteRenderer.sprite.bounds.size;
        
            
            Vector2 scale = thisTransform.localScale;
            if (cameraSize.x >= cameraSize.y) { // Landscape (or equal)
                scale *= cameraSize.x / spriteSize.x;
            } else { // Portrait
                scale *= cameraSize.y / spriteSize.y;
            }
            
            thisTransform.localScale = scale;
        }
    }
    
    
}