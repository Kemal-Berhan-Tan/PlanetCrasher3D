using Framework.Core;
using GameFolders.Scripts.Managers.HighLevelManagers;
using GameFolders.Scripts.Models;
using UnityEngine;

namespace GameFolders.Scripts.AppInitializer
{
    public class AppInitializer : MonoBehaviour
    {
        [SerializeField] private AppManager appManager;
        [SerializeField] private GameManager gameManager;
        [SerializeField] private UIManager uiManager;

        private void Awake()
        {
            var baseMediator = new BaseMediator();
            appManager.InjectMediator(baseMediator);
            gameManager.InjectMediator(baseMediator);
            uiManager.InjectMediator(baseMediator);
        }

        private void Start()
        {
            var gameModel = new GameModel();
            appManager.InjectModel(gameModel);
            gameManager.InjectModel(gameModel);
            uiManager.InjectModel(gameModel);
        }
    }
}