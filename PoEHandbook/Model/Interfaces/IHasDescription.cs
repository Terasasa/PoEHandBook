//  ------------------------------------------------------------------ 
//  PoEHandbook
//  IHasDescription.cs by Tyrrrz
//  29/04/2015
//  ------------------------------------------------------------------ 

namespace PoEHandbook.Model.Interfaces
{
    /// <summary>
    /// Implemented by items that have a text description
    /// </summary>
    public interface IHasDescription
    {
        DescriptionHandler DescriptionHandler { get; }
    }
}