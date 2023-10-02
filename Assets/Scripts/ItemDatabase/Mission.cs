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

            var postIndex = intRand.Next(possiblePosts.Length);
            var tp = possiblePosts[postIndex];
            
            var cargoIndex = intRand.Next(tp.Buying.Count);
            var item =tp.Buying[cargoIndex];
            
            var qty = intRand.Next(1, 5);
            var reward = (int)(item.Info.BuyValue * qty * Random.Range(MinRewardMult, MaxRewardMult));
            return new Mission()
            {
                Title = $"Source & Deliver {item.Info.Name}",
                Description = $"[{tp.Name}] Need {qty} {item.Info.Name}, Reward {reward}",
                Quantity = qty,
                RequestingItemId = item.Info.Id,
                Reward = reward,
                TradingPostId = tp.Id
            };
        }
    }
}