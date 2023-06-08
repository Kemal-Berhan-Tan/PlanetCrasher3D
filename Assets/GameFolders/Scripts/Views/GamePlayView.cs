using System.Collections.Generic;
using Framework.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameFolders.Scripts.Views
{
    public class GamePlayView : BaseView
    {
        #region Public Variables

        public List<float> planetsMaskYPositions = new List<float>();
        public RectTransform Container, Content, RoadMapMask;

        public TextMeshProUGUI currentShowingLevelText;

        public RectTransform fuelBarMask;
        //public Button RoadmapCloseButtonHandler;

        #endregion

        #region Private Variables

        #endregion

        #region Implemented Methods

        protected override void Initialize()
        {
            //RoadmapCloseButtonHandler.onClick.AddListener((_presenter as GamePlayPresenter).CloseRoadMapButtonHandler);
            // settingsButton.onClick.AddListener((_presenter as GamePlayPresenter).OnSettingsButtonClickHandler);
            //restartLevelButton.onClick.AddListener((_presenter as GamePlayPresenter).OnRestartButtonClickHandler);
        }

        #endregion
    }
}