namespace DragonDenDevTool.Features.InRaid.Interfaces;

public interface IDevTab
{
    string Title { get; }
    void OnGUI();
    void Tick();
}