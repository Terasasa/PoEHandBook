﻿//  ------------------------------------------------------------------ 
//  PoEHandbook
//  Equipment.cs by Tyrrrz
//  29/04/2015
//  ------------------------------------------------------------------ 

using System.Collections.Generic;
using System.Xml;
using PoEHandbook.Model.Interfaces;

namespace PoEHandbook.Model
{
    public class Equipment : Entity, IHasStats, IHasRequirements, IHasMods, IHasRarity
    {
        public Equipment()
        {
            StatsHandler = new StatsHandler(this);
            RequirementsHandler = new RequirementsHandler(this);
            ModsHandler = new ModsHandler(this);
            RarityHandler = new RarityHandler(this);
        }

        public StatsHandler StatsHandler { get; private set; }
        public RequirementsHandler RequirementsHandler { get; private set; }
        public ModsHandler ModsHandler { get; private set; }
        public RarityHandler RarityHandler { get; private set; }

        public override void Deserialize(XmlNode node)
        {
            base.Deserialize(node);

            StatsHandler.Deserialize(node);
            RequirementsHandler.Deserialize(node);
            ModsHandler.Deserialize(node);
            RarityHandler.Deserialize(node);
        }

        public override bool ContainsInProperties(string query, out List<string> properties)
        {
            bool result = base.ContainsInProperties(query, out properties);

            List<string> temp;
            if (StatsHandler.ContainsInProperties(query, out temp))
            {
                properties.AddRange(temp);
                result = true;
            }
            if (RequirementsHandler.ContainsInProperties(query, out temp))
            {
                properties.AddRange(temp);
                result = true;
            }
            if (ModsHandler.ContainsInProperties(query, out temp))
            {
                properties.AddRange(temp);
                result = true;
            }
            if (RarityHandler.ContainsInProperties(query, out temp))
            {
                properties.AddRange(temp);
                result = true;
            }

            return result;
        }
    }
}