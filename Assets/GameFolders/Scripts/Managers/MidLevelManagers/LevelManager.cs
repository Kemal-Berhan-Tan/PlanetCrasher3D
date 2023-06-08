using System.Collections.Generic;
using System.Linq;
using System;
using Framework.Core;
using GameFolders.Scripts.Enums;
using GameFolders.Scripts.Events;
using GameFolders.Scripts.Models;
using GameFolders.Scripts.Objects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameFolders.Scripts.Managers.MidLevelManagers
{
    public class LevelManager : BaseManager
    {
        #region Private Variables

        [SerializeField] private Transform levelItemsParrent;
        private List<LevelItem> levelItems = new List<LevelItem>();

        private LevelItem currentLevelItem;

        private GameModel _gameModel;

        #endregion

        #region Begining

        #endregion

        #region Implented Methods

        public override void Receive(BaseEventArgs baseEventArgs)
        {
            switch (baseEventArgs)
            {
                case ResetTheManagersEventArgs resetTheManagersEventArgs:
                    ResetTheLevelManager();
                    break;
                case StartThePlanetFightEventArgs startThePlanetFightEventArgs:
                    currentLevelItem.gameObject.SetActive(false);
                    break;
            }
        }

        #endregion

        #region Public Methods

        public void LoadLevel()
        {
            // LevelData levelData = null;
            // if (levelDatas.LevelDataList.Count > _gameModel.LevelID)
            //     levelData = levelDatas.LevelDataList[_gameModel.LevelID];

            levelItems = (from Transform child in levelItemsParrent.transform
                    select child.transform.GetComponent<LevelItem>())
                .ToList();

            currentLevelItem = levelItems[_gameModel.LevelID];
            currentLevelItem.gameObject.SetActive(true);
            if (!currentLevelItem.CurvySpline)
                currentLevelItem.GenerateTheLevel();

            RenderSettings.skybox = currentLevelItem.SkyboxMaterial;

            BaseEventArgs tempEvent = new OnLevelCreatedEventArgs(currentLevelItem.CurvySpline);
            Broadcast(tempEvent);
            BroadcastUpward(tempEvent);
        }

        public void InjectModel(GameModel gameModel)
        {
            _gameModel = gameModel;
        }

        #endregion

        #region Incoming Receive Events

        private void ResetTheLevelManager()
        {
        }

        private void LevelEndTransition()
        {
        }

        #endregion
    }
}