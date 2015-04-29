//  ------------------------------------------------------------------ 
//  PoEHandbook
//  Recipe.cs by Tyrrrz
//  29/04/2015
//  ------------------------------------------------------------------ 

using PoEHandbook.Model.Interfaces;

namespace PoEHandbook.Model
{
    public class Recipe : Entity, IHasDescription
    {
        public Recipe()
        {
            DescriptionHandler = new DescriptionHandler(this);
        }

        public DescriptionHandler DescriptionHandler { get; private set; }
    }
}