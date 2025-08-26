using Dalamud.Plugin;
using Dalamud.Interface.Winddowing;
using Dalamud.Bindings.ImGui;
using Dalamud.Game.ClientState.Objects.SubKinds;
using Dalamud.Game.ClientState.Objects.Types;

namespace HPTarget
{
    public sealed class Plugin : IDalamudPlugin
    {
        public string Name => "HPTarget";

        private readonly DalamudPluginInterface pluginInterface;
        private readonly ITargetManager targetManager;
        private readonly WindowSystem windowSystem = new("HPTarget");
        private readonly HpWindow hpWindow;

        public Plugin(DalamudPluginInterface pluginInterface, ITargetManager targetManager)
        {
            this.pluginInterface = pluginInterface;
            this.targetManager = targetManager;

            this.hpWindow = new HpWindow(targetManager);
            this.windowSystem.AddWindow(this.hpWindow);

            this.pluginInterface.UiBuilder.Draw += DrawUI;
        }

        private void DrawUI() => this.windowSystem.Draw();

        public void Dispose()
        {
            this.windowSystem.RemoveAllWindows();
            this.pluginInterface.UiBuilder.Draw -= DrawUI;
        }
    }

    public class HpWindow : Window
    {
        private readonly ITargetManager targetManager;

        public HpWindow(ITargetManager targetManager)
            : base("HP Target Overlay", ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.AlwaysAutoResize | ImGuiWindowFlags.NoBackground)
        {
            this.targetManager = targetManager;
        }

        public override void Draw()
        {
            var target = this.targetManager.Target as IBattleChara;
            if (target == null || target.MaxHp <= 0)
                return;

            string hpText = $"{target.CurrentHp:N0} / {target.MaxHp:N0}";
            ImGui.Text(hpText);
        }
    }
}