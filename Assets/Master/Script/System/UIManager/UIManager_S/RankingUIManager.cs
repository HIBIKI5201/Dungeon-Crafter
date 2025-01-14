using DCFrameWork.UI;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace DCFrameWork.UI
{
    public class RankingUIManager : MonoBehaviour 
    {
        public static RankingUIManager instance;
        Label[] _labels = new Label[3];
        UIDocument _uiDocument;
        private int[] _scores = new int[4];
        private void Start()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            _uiDocument = GetComponent<UIDocument>();

            for(int i = 0; i < _labels.Length; i++)
            {
                _labels[i] = _uiDocument.rootVisualElement.Q<Label>($"Ranktext{i + 1}");
            }

            foreach(Label label in _labels)
            {
                label.text = "0";
            }
        }

        public void UpdateRanking(int score)
        {
            _scores[3] = score;
            Array.Sort( _scores );
            Array.Reverse( _scores );
            for(int i = 0; i < _scores.Length - 1; i++)
            {
                _labels[i].text = _scores[i].ToString();
            }
        }

    }
}
