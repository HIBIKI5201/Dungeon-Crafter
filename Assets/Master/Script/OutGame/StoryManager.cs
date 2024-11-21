using System.Collections;
using UnityEngine;

namespace DCFrameWork
{
    public class StoryManager : MonoBehaviour
    {
        private IEnumerator _enumerator;

        public void Initialize()
        {
            _enumerator = PlayStoryContext();
        }

        public void NextContext() => _enumerator.MoveNext();

        private IEnumerator PlayStoryContext()
        {
            yield return null;
        }
    }
}
