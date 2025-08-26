using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Dalamud.Interface.Windowing;
using Dalamud.Bindings.ImGui;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;

namespace HPTarget;

public sealed class Plugin : IDalamudPlugin {
    public string Name => "HPTarget";

    [PluginService] internal static DalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] internal static ITargetManager TargetManager { get; private set; } = null!;

    private readonly WindowSystem ws = new("HPTarget");
    private readonly HPWindow window = new();

    public Plugin() {
        ws.AddWindow(window);
        PluginInterface.UiBuilder.Draw += Draw;
    }

    private void Draw() => ws.Draw();

    public void Dispose() {
        ws.RemoveAllWindows();
        PluginInterface.UiBuilder.Draw -= Draw;
    }
}

public class HPWindow : Window {
    public HPWindow() : base("HPTarget Overlay", ImGui.ImGuiWindowFlags.AlwaysAutoResize) {}

    public override void Draw() {
        var target = TargetManager.Target as BattleChara;
        if (target == null) return;
        ImGui.ImGui.Text($"{target.CurrentHp:n0} / {target.MaxHp:n0}");
    }
}