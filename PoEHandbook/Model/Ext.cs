﻿//  ------------------------------------------------------------------ 
//  PoEHandbook
//  Ext.cs by Tyrrrz
//  27/04/2015
//  ------------------------------------------------------------------ 

using System.Windows.Media;

namespace PoEHandbook.Model
{
    public static class Ext
    {
        /// <summary>
        /// Get the colors that are applicable to the given rarity tier
        /// </summary>
        public static void GetRarityColor(this Equippable.RarityTier rarity, out Color fore, out Color back)
        {
            switch (rarity)
            {
                case Equippable.RarityTier.Unique:
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