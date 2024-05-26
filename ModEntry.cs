﻿using JumpKing.Mods;
using JumpKing.PauseMenu.BT.Actions;
using JumpKing.PauseMenu;
using JumpKing.Player;
using EntityComponent;
using System.ComponentModel;
using System.IO;
using JumpKing_GamepadVibration.Model;
using System;
using System.Reflection;
using JumpKing_GamepadVibration.Menu;

namespace JumpKing_GamepadVibration
{
    [JumpKingMod("YutaGoto.JumpKing_GamepadVibration")]
    public static class ModEntry
    {

        public const string SETTINS_FILE = "YutaGoto.GamepadVibration.Settings.xml";
        private static string AssemblyPath { get; set; }

        [MainMenuItemSetting]
        [PauseMenuItemSetting]
        public static ITextToggle AddToggleEnabled(object factory, GuiFormat format)
        {
            return new ToggleEnabled();
        }

        [MainMenuItemSetting]
        [PauseMenuItemSetting]
        public static ITextToggle AddToggleIsLanded(object factory, GuiFormat format)
        {
            return new ToggleIsLanded();
        }


        [MainMenuItemSetting]
        [PauseMenuItemSetting]
        public static ITextToggle AddToggleGiantBootsPower(object factory, GuiFormat format)
        {
            return new ToggleGiantBootsPower();
        }

        [MainMenuItemSetting]
        [PauseMenuItemSetting]
        public static ITextToggle AddToggleIsKnocked(object factory, GuiFormat format)
        {
            return new ToggleIsKnocked();
        }

        /// <summary>
        /// Called by Jump King before the level loads
        /// </summary>
        [BeforeLevelLoad]
        public static void BeforeLevelLoad()
        {
            AssemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            try
            {
                Preference.Preferences = XmlSerializerHelper.Deserialize<Preferences>(AssemblyPath + "\\" + SETTINS_FILE);
            }
            catch (Exception)
            {
                Preference.Preferences = new Preferences();
                XmlSerializerHelper.Serialize(AssemblyPath + "\\" + SETTINS_FILE, Preference.Preferences);
            }

            Preference.Preferences.PropertyChanged += SaveSettingsOnFile;
        }

        /// <summary>
        /// Called by Jump King when the level unloads
        /// </summary>
        [OnLevelUnload]
        public static void OnLevelUnload() { }

        /// <summary>
        /// Called by Jump King when the Level Starts
        /// </summary>
        [OnLevelStart]
        public static void OnLevelStart()
        {
            PlayerEntity player = EntityManager.instance.Find<PlayerEntity>();

            if (player != null)
            {
                player.m_body.RegisterBehaviour(new Behaviour.Vibration());
            }
        }

        /// <summary>
        /// Called by Jump King when the Level Ends
        /// </summary>
        [OnLevelEnd]
        public static void OnLevelEnd() { }

        private static void SaveSettingsOnFile(object sender, PropertyChangedEventArgs args)
        {
            try
            {
                XmlSerializerHelper.Serialize(AssemblyPath + "\\YutaGoto.GamepadVibration.Settings.xml", Preference.Preferences);
            }
            catch (Exception)
            {

            }
        }
    }
}
