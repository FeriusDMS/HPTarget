using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using Dalamud.Interface.Windowing;
using Dalamud.Bindings.ImGui;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Game.ClientState.Objects;
using Dalamud.IoC;

namespace HPTarget;

public class Plugin : IDalamudPlugin {
    public string Name => "HPTarget";

    [PluginService] public static IDalamudPluginInterface PluginInterface { get; private set; } = null!;
    [PluginService] public static ITargetManager TargetManager { get; private set; } = null!;
    [PluginService] public static IPluginLog Log { get; private set; } = null!;

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
    public HPWindow() : base("HPTarget Overlay", ImGuiWindowFlags.AlwaysAutoResize) {}

    public override void Draw() {
        var target = Plugin.TargetManager.Target as IBattleChara;
        if (target == null) return;
        ImGui.Text($"{target.CurrentHp:n0} / {target.MaxHp:n0}");

        Plugin.Log.Info($"Target HP: {target.CurrentHp}");
        Plugin.Log.Info($"Target HP n0: {target.CurrentHp:n0}");
        Plugin.Log.Info($"Target Max HP: {target.MaxHp}");
        Plugin.Log.Info($"Target Max HP n0: {target.MaxHp:n0}");
    }
}