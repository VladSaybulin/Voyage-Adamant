using System.Collections.Generic;
using Game;
using UnityEngine;
using Utils;

namespace UI
{
    public class NeedCollectDiamonds : MonoBehaviour
    {

        [SerializeField] private AmountDiamonds item;

        private Transform _transform;
        private AmountDiamonds[] _items;

        public void Init(int maxIndex, List<Pair<int, Color>> itemParams)
        {
            _items = new AmountDiamonds[maxIndex];
            _transform = GetComponent<Transform>();

            foreach (var p in itemParams)
            {
                var it = Instantiate(item, _transform);
                it.Color = p.Second;
                _items[p.First] = it;
            }
        }

        public void SetAmount(int index, int amount)
        {
            var it = _items[index];
            if(it != null)
                _items[index].Amount = amount;
        }

        public void Clear()
        {
            for (int i = 0; i < _transform.childCount; i++)
                Destroy(_transform.GetChild(i).gameObject);
        }

    }
}