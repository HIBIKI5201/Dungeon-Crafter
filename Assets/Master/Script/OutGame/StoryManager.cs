using DCFrameWork.MainSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

namespace DCFrameWork.SceneSystem {
    public class StoryManager : MonoBehaviour {
        private AudioSource _audioSource;
        private Image _backGround;

        private IEnumerator _enumerator;
        private List<StoryText> _storyTextList;

        private StoryUIManager _storyUIManager;

        [SerializeField]
        private GameObject _cullieth;
        private SpriteRenderer[] _culliethSprites;
        [SerializeField]
        private GameObject _rabbiliss;
        private SpriteRenderer[] _rabbilissSprites;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _backGround = GetComponentInChildren<Image>();

            _culliethSprites = _cullieth.GetComponentsInChildren<SpriteRenderer>();
            _rabbilissSprites = _rabbiliss.GetComponentsInChildren<SpriteRenderer>();
        }

        public void Initialize(StoryUIManager storyUIManager)
        {
            _enumerator = PlayStoryContext();
            _storyUIManager = storyUIManager;
        }

        public void SetStoryData(StoryData storyData)
        {
            _storyTextList = storyData.StoryText;
            SetBackGround(storyData.BackGround);
        }

        public void NextText() => _enumerator.MoveNext();

        private IEnumerator PlayStoryContext()
        {
            int count = 0;
            while (count < _storyTextList.Count) {
                StoryText storyText = _storyTextList[count];

                //サウンドとUIを更新
                _storyUIManager.TextBoxUpdate(storyText.Character, storyText.Text);
                _audioSource.PlayOneShot(storyText.AudioClip);

                //もしクリエトかラビリスならハイライト
                CharacterEnum character = storyText.Character switch {
                    "クリエト" => CharacterEnum.Culieth,
                    "ラビリス" => CharacterEnum.Rabbiliss,
                    _ => CharacterEnum.None,
                };
                CharacterHighlight(character);

                count++;

                Debug.Log($"{storyText.Character} {storyText.Animation}\n{storyText.Text}");
                yield return null;
            }
            EndStory();
        }
        private void SetBackGround(Sprite sprite)
        {
            _backGround.sprite = sprite;
        }

        private void CharacterHighlight(CharacterEnum character)
        {
            switch (character) {
                case CharacterEnum.Rabbiliss:
                    ColorChange(_rabbilissSprites, Color.white);
                    ColorChange(_culliethSprites, Color.gray);
                    break;
                
                case CharacterEnum.Culieth:
                    ColorChange(_culliethSprites, Color.white);
                    ColorChange(_rabbilissSprites, Color.gray);
                    break;
                
                case CharacterEnum.None:
                    ColorChange(_rabbilissSprites, Color.gray);
                    ColorChange(_culliethSprites, Color.gray);
                    break;
            }

            void ColorChange(SpriteRenderer[] renderers, Color color)
            {
                foreach (SpriteRenderer renderer in renderers) {
                    renderer.color = color;
                }
            }
        }

        private void EndStory()
        {
            var system = GameBaseSystem.sceneSystem as StorySystem;
            system.EndStory();
        }

        public bool SetAudioVolume(float volume)
        {
            if (volume < 0 || 1 < volume) {
                return false;
            }

            _audioSource.volume = volume;
            return true;
        }

        private enum CharacterEnum
        {
            None = 0,
            Culieth = 1 << 1,
            Rabbiliss = 1 << 2,
        }
    }
}
