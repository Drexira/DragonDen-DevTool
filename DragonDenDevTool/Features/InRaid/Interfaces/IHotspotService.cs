using System.Collections.Generic;
using DragonDenDevTool.Utilities;

namespace DragonDenDevTool.Features.InRaid.Interfaces;

public interface IHotspotService
{
    IReadOnlyList<Hotspot> GlobalSpots { get; }
    IReadOnlyList<Hotspot> FavoriteSpots { get; }
    void LoadFavorites();
    void SaveFavorites();
    void AddFavorite(Hotspot spot);
    void RemoveFavoriteAt(int index);
}