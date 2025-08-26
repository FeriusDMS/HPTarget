using Dalamud.Plugin;
using Dalamud.Interface.Windowing;
using Dalamud.Bindings.ImGui;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;

namespace HPTarget;

public sealed class Plugin : IDalamudPlugin
{
    public string Name => "HPTarget";

    [PluginService] internal static ITargetManager TargetManager { get; private set; } = null!;
    [PluginService] internal static DalamudPluginInterface PluginInterface { get; private set; } = null!;

    private readonly WindowSystem windowSystem;
    private readonly HPTargetWindow hpWindow;

    public Plugin()
    {
        this.windowSystem = new WindowSystem("HPTarget");
        this.hpWindow = new HPTargetWindow();
        this.windowSystem.AddWindow(hpWindow);

        PluginInterface.UiBuilder.Draw += this.DrawUI;
    }

    private void DrawUI() => this.windowSystem.Draw();

    public void Dispose()
    {
        this.windowSystem.RemoveAllWindows();
        PluginInterface.UiBuilder.Draw -= this.DrawUI;
    }
}

public class HPTargetWindow : Window
{
    public HPTargetWindow() : base("HPTarget", ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoCollapse)
    {
        this.RespectCloseHotkey = false;
    }

    public override void Draw()
    {
        var target = Plugin.TargetManager.Target as BattleChara;
        if (target == null) return;

        ImGui.Text($"{target.CurrentHp:n0} / {target.MaxHp:n0}");
    }
}