using System;
using System.Linq;
using SystemMap;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceSystem
{
    public class SpaceSystemManager : MonoBehaviour
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

        public TargetController _TargetController;
        public SystemRegistry SystemRegistry;
        public GameObject Player;
        private void Start()
        {
            var lastTp = PlayerPrefs.GetString(SpaceTruckerConstants.TradingPostKey);
            if (!string.IsNullOrWhiteSpace(lastTp))
            {
                var tp = GetTradingPost(lastTp);
                if (tp != null)
                {
                    Player.transform.position = tp.DockingPoints.First().transform.position;
                }
            }
            
            SetTarget();
        }

        public void SetTarget()
        {
            var targetId = PlayerPrefs.GetString(SpaceTruckerConstants.TargetTradingPost);
            if (!string.IsNullOrWhiteSpace(targetId))
            {
                var target = GetTradingPost(targetId);
                if (target != null)
                {
                    _TargetController.Target = target.gameObject;
                }
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
        

        private void OnDestroy()
        {
            if (this == _instance) _instance = null;
        }
    }
}