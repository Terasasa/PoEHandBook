//  ------------------------------------------------------------------ 
//  PoEHandbook
//  IHasMods.cs by Tyrrrz
//  29/04/2015
//  ------------------------------------------------------------------ 

namespace PoEHandbook.Model.Interfaces
{
    /// <summary>
    /// Implemented by items that can have mods (affixes)
    /// </summary>
    public interface IHasMods
    {
        ModsHandler ModsHandler { get; }
    }
}