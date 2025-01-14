using DCFrameWork.MainSystem;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace DCFrameWork.SceneSystem {
    public class StoryManager : MonoBehaviour {
        private AudioSource _audioSource;
        private Image _backGround1;
        private Image _backGround2;

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
            Image[] images = GetComponentsInChildren<Image>();
            _backGround1 = images[0];
            _backGround2 = images[1];

            _culliethSprites = _cullieth.GetComponentsInChildren<SpriteRenderer>();
            _rabbilissSprites = _rabbiliss.GetComponentsInChildren<SpriteRenderer>();
        }

        public void Initialize(StoryUIManager storyUIManager)
        {
            _enumerator = PlayStoryContext();
            _storyUIManager = storyUIManager;
        }

        public async void SetStoryData(StoryData storyData)
        {
            _storyTextList = storyData.StoryText;
            await SetBackGround(storyData.BackGround[0], BackGroundSetType.None);
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
        private async Task SetBackGround(Sprite sprite, BackGroundSetType type)
        {
            switch (type) {
                case BackGroundSetType.None:
                    _backGround1.sprite = sprite;
                    break;

                case BackGroundSetType.Fade:
                    _backGround2.sprite = sprite;

                    //背景1をフェードアウトする
                    while (_backGround1.color.a > 0) {
                        Color newColor = _backGround1.color;
                        newColor.a -= 30 * Time.deltaTime;
                        _backGround1.color = newColor;
                        await Awaitable.NextFrameAsync();
                    }
                    break;
            }

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

        private enum CharacterEnum {
            None = 0,
            Culieth = 1 << 1,
            Rabbiliss = 1 << 2,
        }

        private enum BackGroundSetType {
            None = 0,
            Fade = 1,
        }
    }
}
