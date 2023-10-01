using System.Collections.Generic;
using System.Linq;
using CargoManagement;
using ItemDatabase;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace TradingPost
{
    public class ShipLoadingController : MonoBehaviour
    {
        //Input
        public InputActionAsset ActionAsset;

        private InputAction SwitchObject;
        private InputAction Move;
        private InputAction Rotate;
        private InputAction Confirm;
        private InputAction DropItem;
        private InputAction BuyMenu;
        private InputAction MissionBoard;
        
        //Component
        public float MoveSpeed = 5f;
        public float RotationSpeed = 180f;
        public Collider2D SellArea;
        public Collider2D DeliverArea;
        public Collider2D LoadingArea;
        [FormerlySerializedAs("LoadingContactFilter")] 
        public ContactFilter2D cargoContactFilter2D;
        private MovableCargo _target;
        public GameObject CanSellInstruction;
        public GameObject CanDeliverInstruction;
        public GameObject BuyMenuPanel;
        public GameObject MissionBoardPanel;
        public TextMeshProUGUI InfoText;
        public float infoSetAtTime;

        public List<Collider2D> NewItemSlots;

        private int _index = 0;
        private List<MovableCargo> _MovableObjects;
        
        void Start()
        {
            var ActionMap = ActionAsset.FindActionMap("LoadShip");
            ActionMap.Enable();
            SwitchObject = ActionMap.FindAction("Switch Object");
            Move = ActionMap.FindAction("Movement");
            Rotate = ActionMap.FindAction("Rotate");
            Confirm = ActionMap.FindAction("Confirm");
            DropItem = ActionMap.FindAction("DropItem");
            BuyMenu = ActionMap.FindAction("BuyMenu");
            MissionBoard = ActionMap.FindAction("MissionBoard");
            _MovableObjects = FindObjectsOfType<MovableCargo>().ToList();
            PickTarget();
            BuyMenuPanel.SetActive(false);
            ShowMessage("Hi");
        }

        public void ShowMessage(string message)
        {
            InfoText.GameObject().SetActive(true);
            InfoText.text = message;
            infoSetAtTime = Time.time;
        }
        
        public void PickTarget()
        {
            if (_MovableObjects.Any())
            {
                SelectMovable(_MovableObjects.First());
            }
        }
        
        public void SelectMovable(MovableCargo movableCargo)
        {
            if (_target != null) _target.Deselected();
            _target = movableCargo;
            _target.Selected();
            
            var activeMission = MissionManager.GetActiveMission();
            deliverable = IsTargetDeliverable(activeMission);
        }

        private string saved = "";

        private bool deliverable = false;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) SceneManager.LoadScene("MainMenu");
            
            if (InfoText.gameObject.activeSelf &&  Time.time - infoSetAtTime > 2f)
            {
                InfoText.gameObject.SetActive(false);
            }
            
            if (SwitchObject.triggered)
            {
                if (_index >= _MovableObjects.Count-1)
                {
                    _index = -1;
                }
                _index++;
                SelectMovable(_MovableObjects[_index]);
            }

            if (Confirm.triggered)
            {
                TradingPostGameManager.Instance.GotoSpace();
            }

            if (canSell && DropItem.triggered)
            {
                SellTarget();
            }

            if (canDeliver && DropItem.triggered)
            {
                DeliverTarget();
            }

            if (BuyMenu.triggered)
            {
                BuyMenuPanel.SetActive(!BuyMenuPanel.activeSelf);
            }
            
            if (MissionBoard.triggered)
            {
                TradingPostGameManager.Instance.MissionPanelManager.UpdateUI();
                MissionBoardPanel.SetActive(!MissionBoardPanel.activeSelf);
            }
            
            #if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //Save
                var load = new CargoLoad();
                var loadedCargo = inLoadingArea
                    .Select(loadedCollider => loadedCollider.GetComponent<MovableCargo>())
                    .Where(cargo => cargo != null).ToList();
                Debug.Log($"Detected {loadedCargo.Count} items");
                //TODO: add storage relative point
                load.LoadFromMovableCargo(transform.position, loadedCargo);
                saved = load.SaveToString();
                Debug.Log(saved);
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                MoneyManager.AddCredits(1000);
                TradingPostGameManager.Instance.UpdateMoneyLabel();
            }
            
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                PlayerPrefs.DeleteAll();
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                _MovableObjects.ForEach(mo => Destroy(mo.gameObject));
                _MovableObjects = new List<MovableCargo>();
                _target = null;
                Debug.Log("Cleared");
            }

            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                LoadCargo(saved);
            }
            #endif
        
        }

        private void DeliverTarget()
        {
            var activeMission = MissionManager.GetActiveMission();
            if (IsTargetDeliverable(activeMission))
            {
                var nowComplete = MissionManager.ProgressMission(activeMission.Id, 1);
                if (nowComplete)
                {
                    MoneyManager.AddCredits(activeMission.Info.Reward);
                    TradingPostGameManager.Instance.UpdateMoneyLabel();
                }
                ShowMessage(nowComplete ? "Mission Completed" : $"Delivery { activeMission.ItemsDelivered+1}/{activeMission.Info.Quantity} made");
                DestroyTarget();
                Debug.Log($"Completed? {nowComplete}");
            }
        }

        private bool IsTargetDeliverable(MissionProgress activeMission)
        {
            return activeMission != null && 
                   activeMission.Info.TradingPostId == TradingPostGameManager.Instance.TradingPostInfo.Id && 
                   activeMission.Info.RequestingItemId == _target.CargoInfo.Info.Id;
        }

        private void SellTarget()
        {
            TradingPostGameManager.Instance.SellCargo(_target.CargoInfo);
            DestroyTarget();
        }

        private void DestroyTarget()
        {
            _MovableObjects.Remove(_target);
            Destroy(_target.gameObject);
            PickTarget();
        }

        List<MovableCargo> inLoadingArea = new List<MovableCargo>();
        private bool canSell = false;
        private bool canDeliver = false;
        
        void FixedUpdate()
        {
            if (_target)
            {
                var moveVector = Move.ReadValue<Vector2>();
                var rotation = Rotate.ReadValue<float>();

                var delta = moveVector * (MoveSpeed * Time.fixedDeltaTime);
                _target.Rigidbody.MovePosition((Vector2)_target.transform.position + delta);
                var rotDelta = -rotation * RotationSpeed * Time.fixedDeltaTime;
                _target.Rigidbody.MoveRotation(_target.transform.rotation.eulerAngles.z + rotDelta);
                
                canSell = _target.InArea(SellArea);
                canDeliver = deliverable && _target.InArea(DeliverArea);
            }
            else
            {
                canSell = false;
                canDeliver = false;
            }
            CanSellInstruction.SetActive(canSell);
            CanDeliverInstruction.SetActive(canDeliver);

            var count = 0;
            inLoadingArea = new List<MovableCargo>(_MovableObjects.Count);
            for (var i = 0; i < _MovableObjects.Count; i++)
            {
                var currentCargo = _MovableObjects[i];
                var loaded = currentCargo.InArea(LoadingArea);
                currentCargo.SetInLoadingArea(loaded);
                if (!loaded) continue;
                count++;
                inLoadingArea.Add(currentCargo);
            }
            
            UpdateAvailableNewSlots();
        }

        public void LoadCargo(string currentCargo)
        {
            var load = CargoLoad.LoadFromString(currentCargo);
            if (!load.IsEmpty)
            {
                _MovableObjects = load.Instantiate(transform.position, TradingPostGameManager.Instance.CargoRegistry);
                _index = 0;
                SelectMovable(_MovableObjects[_index]);
            }
        }

        public string SaveCargoToJson()
        {
            var load = new CargoLoad();
            //TODO: add storage relative point
            load.LoadFromMovableCargo(transform.position, inLoadingArea);
            return load.SaveToString();
        }

        [HideInInspector]
        public List<Vector3> availableBuySlots;

        public void UpdateAvailableNewSlots()
        {
            availableBuySlots = NewItemSlots
                .Where(c =>
                {
                    var colls = new List<Collider2D>();
                    var amt = c.OverlapCollider(cargoContactFilter2D, colls);
                    if (amt > 0) Debug.Log(colls);
                    return amt == 0;
                })
                .Select(c => c.transform.position)
                .ToList();
        }

        public void SpawnBoughtCargo(CargoInfo newCargo)
        {
            if (availableBuySlots.Any())
            {
                var slot = availableBuySlots.First();
                var obj = TradingPostGameManager.Instance.CargoRegistry.FindCargoById(newCargo.Id);
                var go = Instantiate(obj.Prefab, slot, Quaternion.identity);
                var mc = go.GetComponent<MovableCargo>();
                mc.playerOwned = true;
                _MovableObjects.Add(mc);
                SelectMovable(mc);
            }
        }

        public void NotifyMissionChange()
        {
            var activeMission = MissionManager.GetActiveMission();
            deliverable = IsTargetDeliverable(activeMission);
        }
    }
}
