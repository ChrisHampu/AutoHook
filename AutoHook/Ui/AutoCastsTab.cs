using ImGuiNET;
using AutoHook.Utils;
using AutoHook.Configurations;
using AutoHook.Data;

namespace AutoHook.Ui;

internal class AutoCastsTab : TabConfig
{
    public override bool Enabled => true;
    public override string TabName => "Auto Casts";

    private static AutoCastsConfig cfg = Service.Configuration.AutoCastsCfg;

    public override void DrawHeader()
    {
        ImGui.TextWrapped("The new Auto Cast/Mooch is a experimental feature and can be a little confusing at first. I'll be trying to find a more simple and intuitive solution later\nPlease report any issues you encounter.");

        // Disable all casts
        if (DrawUtil.Checkbox("Enable Auto Casts", ref cfg.EnableAll, "You can uncheck this to not use any actions below"))
        { }

        if (cfg.EnableAll) {
            ImGui.SameLine();
            if (DrawUtil.Checkbox("Don't Cancel Mooch", ref cfg.DontCancelMooch, "If mooch is available & Auto Mooch is enabled, actions that cancel mooch wont be used (e.g. Chum, Fish Eyes, Prize Catch etc.)"))
            { }
        }
    }

    public override void Draw()
    {
        if (cfg.EnableAll)
        {
            DrawAutoCast();
            DrawAutoMooch();
            DrawPatience();
            DrawMakeShiftBait();
            DrawThaliaksFavor();
            DrawPrizeCatch();
            DrawChum();
            DrawFishEyes();
            //DrawSurfaceSlapIdenticalCast();
            DrawCordials();
        }
    }

    private void DrawAutoCast()
    {
        if (DrawUtil.Checkbox("Global Auto Cast Line", ref cfg.EnableAutoCast, "Cast (FSH Action) will be used after a fish bite\n\nIMPORTANT!!!\nIf you have this option enabled and you don't have a Custom Auto Mooch or the Global Auto Mooch option enabled, the line will be casted normally and you'll lose your mooch oportunity (If available)."))
        {}

        if (cfg.EnableAutoCast)
        {
            ImGui.Indent();
            DrawExtraOptionsAutoCast();
            ImGui.Unindent();
        }
    }

    private void DrawExtraOptionsAutoCast()
    {

    }

    private void DrawAutoMooch()
    {
        if (DrawUtil.Checkbox("Global Auto Mooch", ref cfg.EnableMooch, "All fish will be mooched if available. This option have priority over Auto Cast Line\n\nIf you want to Auto Mooch only a especific fish and ignore others, disable this option and add the fish you want in the bait/mooch tab"))
        { }

        if (cfg.EnableMooch)
        {
            ImGui.Indent();
            DrawExtraOptionsAutoMooch();
            ImGui.Unindent();
        }
    }

    private void DrawExtraOptionsAutoMooch()
    {
        ImGui.Checkbox("Use Mooch II", ref cfg.EnableMooch2);
    }

    private void DrawPatience()
    {
        if (DrawUtil.Checkbox("Use Patience I/II", ref cfg.EnablePatience, "Patience I/II will be used when your current GP is equal (or higher) to the action cost +20 (Ex: 220 for I, 580 for II), this helps to avoid not having GP for the hooksets"))
        { }

        if (cfg.EnablePatience)
        {
            ImGui.Indent();
            DrawExtraOptionsPatience();
            ImGui.Unindent();
        }
    }

    private void DrawExtraOptionsPatience()
    {

        if (DrawUtil.Checkbox("Use when Makeshift Bait is active", ref cfg.EnableMakeshiftPatience))
        { }
        if (ImGui.RadioButton("Patience I###1", cfg.SelectedPatienceID == IDs.Actions.Patience))
        {
            cfg.SelectedPatienceID = IDs.Actions.Patience;
        }

        if (ImGui.RadioButton("Patience II###2", cfg.SelectedPatienceID == IDs.Actions.Patience2))
        {
            cfg.SelectedPatienceID = IDs.Actions.Patience2;
        }
    }

    private void DrawThaliaksFavor()
    {
        ImGui.PushID("ThaliaksFavor");
        if (DrawUtil.Checkbox("Use Thaliak's Favor", ref cfg.EnableThaliaksFavor, "This might conflict with Auto MakeShift Bait"))
        { }

        if (cfg.EnableThaliaksFavor)
        {
            ImGui.Indent();
            DrawExtraOptionsThaliaksFavor();
            ImGui.Unindent();
        }
        ImGui.PopID();
    }

    private void DrawExtraOptionsThaliaksFavor()
    {
        if (Utils.DrawUtil.EditNumberField("When Stacks =", ref cfg.ThaliaksFavorStacks))
        {
            if (cfg.ThaliaksFavorStacks < 3)
                cfg.ThaliaksFavorStacks = 3;

            if (cfg.ThaliaksFavorStacks > 10)
                cfg.ThaliaksFavorStacks = 10;
        }
    }

    private void DrawMakeShiftBait()
    {
        ImGui.PushID("MakeShiftBait");
        if (DrawUtil.Checkbox("Use Makeshift Bait", ref cfg.EnableMakeshiftBait, "This might conflict with Auto Thaliak's Favor"))
        {

        }

        if (cfg.EnableMakeshiftBait)
        {
            ImGui.Indent();
            DrawExtraOptionsMakeShiftBait();
            ImGui.Unindent();
        }
        ImGui.PopID();
    }

    private void DrawExtraOptionsMakeShiftBait()
    {
        if (Utils.DrawUtil.EditNumberField($"When Stacks = ", ref cfg.MakeshiftBaitStacks))
        {
            if (cfg.MakeshiftBaitStacks < 5)
                cfg.MakeshiftBaitStacks = 5;

            if (cfg.MakeshiftBaitStacks > 10)
                cfg.MakeshiftBaitStacks = 10;
        }
    }

    private void DrawPrizeCatch()
    {
        if (DrawUtil.Checkbox("Use Prize Catch", ref cfg.EnablePrizeCatch, "Cancels Current Mooch. Patience and Makeshift Bait will not be used when Prize Catch active"))
        { }

        if (cfg.EnablePrizeCatch)
        {
            ImGui.Indent();
            DrawExtraOptionsPrizeCatch();
            ImGui.Unindent();
        }
    }

    private void DrawExtraOptionsPrizeCatch()
    {
        if (DrawUtil.Checkbox("Use when Identical Catch is active", ref cfg.EnableIdenticalPrizeCatch))
        { }
    }

    private void DrawChum()
    {
        if (DrawUtil.Checkbox("Use Chum", ref cfg.EnableChum, "Cancels Current Mooch"))
        { }
    }

    private void DrawFishEyes()
    {
        if (DrawUtil.Checkbox("Use Fish Eyes", ref cfg.EnableFishEyes, "Cancels Current Mooch"))
        { }
    }

    private void DrawSurfaceSlapIdenticalCast()
    {
        if (DrawUtil.Checkbox("Use Surface Slap", ref cfg.EnableSurfaceSlap, "Overrides Identical Cast"))
        {
            cfg.EnableIdenticalCast = false;
        }

        if (DrawUtil.Checkbox("Use Identical Cast", ref cfg.EnableIdenticalCast, "Overrides Surface Slap"))
        {
            cfg.EnableSurfaceSlap = false;
        }
    }

    private void DrawCordials()
    {
        if (DrawUtil.Checkbox("Use Cordials (Hi-Cordial First)", ref cfg.EnableCordials, "If theres no Hi-Cordials, Cordials will be used instead"))
        { }

        if (cfg.EnableCordials)
        {
            ImGui.Indent();
            DrawExtraOptionsCordials();
            ImGui.Unindent();
        }
    }

    private void DrawExtraOptionsCordials()
    {
        if (DrawUtil.Checkbox("Change Priority: Cordial > HI-Cordials", ref cfg.EnableCordialFirst, "If theres no Cordials, Hi-Cordials will be used instead"))
        { }
    }
}
