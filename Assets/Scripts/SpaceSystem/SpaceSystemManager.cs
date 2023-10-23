using System;
using System.Linq;
using CargoManagement;
using SystemMap;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace SpaceSystem
{
    public class SpaceSystemManager : MonoBehaviour, ICargoRegistryLoader
    {
        private static SpaceSystemManager _instance;
        public static SpaceSystemManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<SpaceSystemManager>();
                }

                return _instance;
            }
        }
        
        public InputActionAsset ActionAsset;
        private InputAction NavMenu;
        public GameObject NavPanel;

        public TargetController _TargetController;
        public SystemRegistry SystemRegistry;
        public CargoRegistry CargoRegistry;
        public GameObject Player;
        
        private void Start()
        {
            NavMenu = ActionAsset.FindAction("OpenNavMenu");
            var lastTp = PlayerPrefs.GetString(SpaceTruckerConstants.TradingPostKey);
            var dockerIndex = PlayerPrefs.GetInt(SpaceTruckerConstants.DockingIndex, 0);
            if (!string.IsNullOrWhiteSpace(lastTp))
            {
                var tp = GetTradingPost(lastTp);
                if (tp != null)
                {
                    if (dockerIndex > tp.DockingPoints.Count - 1)
                    {
                        dockerIndex = 0;
                    }
                    var dockingPoint = tp.DockingPoints[dockerIndex].transform;
                    Player.transform.rotation = dockingPoint.rotation;
                    Player.transform.position = dockingPoint.position;
                }
            }
            
            SetTarget();
        }

        private void Update()
        {
            if (NavMenu.triggered)
            {
                NavPanel.SetActive(!NavPanel.activeSelf);
            }
        }

        public void SetTarget()
        {
            var targetId = PlayerPrefs.GetString(SpaceTruckerConstants.TargetTradingPost);
            Debug.Log(targetId);
            if (!string.IsNullOrWhiteSpace(targetId))
            {
                _TargetController.gameObject.SetActive(true);
                var target = GetTradingPost(targetId);
                if (target != null)
                {
                    _TargetController.Target = target.gameObject;
                }
            }
            else
            {
                _TargetController.gameObject.SetActive(false);
            }
        }

        private static TradingPostBehaviour GetTradingPost(string targetId)
        {
            var tps = FindObjectsOfType<TradingPostBehaviour>();
            var target = tps.First(x => x.Info.Id == targetId);
            return target;
        }

        public void LoadTradingPost(SystemMap.TradingPost tp)
        {
            PlayerPrefs.SetString(SpaceTruckerConstants.TradingPostKey, tp.Id);
            SceneManager.LoadScene(SpaceTruckerConstants.TradingPostScene);
        }


        public void PlayerDied()
        {
            SceneManager.LoadScene(SpaceTruckerConstants.SpaceScene);
        }

        private void OnDestroy()
        {
            if (this == _instance) _instance = null;
        }

        public CargoRegistry GetCargoRegistry()
        {
            return CargoRegistry;
        }
    }
}