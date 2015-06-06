//  ------------------------------------------------------------------ 
//  PoEHandbook
//  Ext.cs by Tyrrrz
//  27/04/2015
//  ------------------------------------------------------------------ 

using System.Windows.Media;
using PoEHandbook.Model.Interfaces;

namespace PoEHandbook.Model
{
    public static class Ext
    {
        /// <summary>
        /// Get the colors that are applicable to the given rarity tier
        /// </summary>
        public static void GetEntityColor(this Entity entity, out Color fore, out Color back)
        {
            // Currency
            if (entity is Currency)
            {
                fore = Color.FromRgb(135, 120, 80);
                back = Color.FromRgb(45, 40, 25);
                return;
            }

            // Recipe
            if (entity is Recipe)
            {
                fore = Color.FromRgb(200, 200, 200);
                back = Color.FromRgb(35, 55, 125);
                return;
            }

            // Misc items
            var entityWithRarity = entity as IHasRarity;
            if (entityWithRarity == null)
            {
                fore = Colors.White;
                back = Colors.LightSlateGray;
                return;
            }

            // Items with rarity
            switch (entityWithRarity.RarityHandler.Rarity)
            {
                case RarityHandler.RarityTier.Unique:
                    fore = Color.FromRgb(175, 95, 35);
                    back = Color.FromRgb(60, 30, 15);
                    return;
                default:
                    fore = Colors.White;
                    back = Colors.LightSlateGray;
                    return;
            }
        }
    }
}