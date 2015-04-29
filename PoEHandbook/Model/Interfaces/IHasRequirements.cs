//  ------------------------------------------------------------------ 
//  PoEHandbook
//  IHasRequirements.cs by Tyrrrz
//  29/04/2015
//  ------------------------------------------------------------------ 

namespace PoEHandbook.Model.Interfaces
{
    /// <summary>
    /// Implemented by items that require characters to fulfill level or stat requirements
    /// </summary>
    public interface IHasRequirements
    {
        RequirementsHandler RequirementsHandler { get; }
    }
}