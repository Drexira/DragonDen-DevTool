using System.Threading.Tasks;
using EFT;
using EFT.InventoryLogic;

namespace DragonDenDevTool.Features.InRaid.Interfaces;

public interface IItemService
{
    Task DropItemAsync(string templateId, Player player, int stack = 1);
    ItemTemplate[] SearchItems(string query, int max);
    int GetStackMax(string templateId);
}