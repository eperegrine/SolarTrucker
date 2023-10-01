using System;
using CargoManagement;
using Random = UnityEngine.Random;

namespace ItemDatabase
{
    [Serializable]
    public class Mission
    {
        public string RequestingItemId;
        public string TradingPostId;
        public int Quantity;
        public string Title;
        public string Description;
        public int Reward;
    }

    public static class MissionBuilder
    {
        public const float MinRewardMult = 1.05f;
        public const float MaxRewardMult = 2f;

        public static Mission Generate(CargoRegistry allItems, SystemMap.TradingPost[] possiblePosts)
        {
            var intRand = new System.Random();
            
            var cargoIndex = intRand.Next(allItems.CargoOptions.Count);
            var item = allItems.CargoOptions[cargoIndex];
            
            var postIndex = intRand.Next(possiblePosts.Length);
            var tp = possiblePosts[postIndex];
            
            var qty = intRand.Next(1, 3); 
            return new Mission()
            {
                Title = $"[{tp.Name}] Need {qty} {item.Info.Name}",
                Description = "TODO: WRITE DESCRIPTIONS",
                Quantity = qty,
                RequestingItemId = item.Info.Id,
                Reward = (int)(item.Info.BuyValue * qty * Random.Range(MinRewardMult, MaxRewardMult)),
                TradingPostId = tp.Id
            };
        }
    }
}