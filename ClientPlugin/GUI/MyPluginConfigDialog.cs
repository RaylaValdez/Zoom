using System;
using System.Text;
using Sandbox;
using Sandbox.Game.Gui;
using Sandbox.Graphics.GUI;
using VRage;
using VRage.Input;
using VRage.Utils;
using VRageMath;

namespace ClientPlugin.GUI
{

    public class MyPluginConfigDialog : MyGuiScreenBase
    {
        private const string Caption = "Zoom Configuration";
        public override string GetFriendlyName() => "MyPluginConfigDialog_Zoom";

        private MyLayoutTable layoutTable;

        private MyGuiControlLabel bindingKeyLabel;
        private MyGuiControlButton bindingKeyButton;

        private MyGuiControlMultilineText infoText;
        private MyGuiControlMultilineText infoTextToggleFunction;

        private MyGuiControlLabel toggleFeatureLabel;
        private MyGuiControlCheckbox toggleFeatureCheckbox;

        private MyGuiControlButton closeButton;

        public MyPluginConfigDialog() : base(new Vector2(0.5f, 0.5f), MyGuiConstants.SCREEN_BACKGROUND_COLOR, new Vector2(0.5f, 0.7f), false, null, MySandboxGame.Config.UIBkOpacity, MySandboxGame.Config.UIOpacity)
        {
            EnabledBackgroundFade = true;
            m_closeOnEsc = true;
            m_drawEvenWithoutFocus = true;
            CanHideOthers = true;
            CanBeHidden = true;
            CloseButtonEnabled = true;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            RecreateControls(true);
        }

        public override void RecreateControls(bool constructor)
        {
            base.RecreateControls(constructor);

            CreateControls();
            LayoutControls();
        }

        private void CreateControls()
        {
            AddCaption(Caption);

            var config = Plugin.Instance.Config;

            infoText = new MyGuiControlMultilineText
            {
                Name = "InfoText",
                OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP,
                TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
                TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
                Text = new StringBuilder("\nHold the Zoom keybind to, well, zoom.\n\nUse scrollwheel while zooming to adjust zoom!")
            };

            infoTextToggleFunction = new MyGuiControlMultilineText
            {
                Name = "InfoTextToggleFunction",
                OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP,
                TextAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
                TextBoxAlign = MyGuiDrawAlignEnum.HORISONTAL_LEFT_AND_VERTICAL_TOP,
                Text = new StringBuilder("\nBy default, Zooming can be toggled by pressing the keybind twice.\r\n This checkbox turns that feature on or off.")
            };

            closeButton = new MyGuiControlButton(originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER, text: MyTexts.Get(MyCommonTexts.ScreenMenuButtonSave), onButtonClick: OnOk);
            bindingKeyLabel = new MyGuiControlLabel(originAlign: MyGuiDrawAlignEnum.HORISONTAL_RIGHT_AND_VERTICAL_CENTER, text: "Zoom");
            StringBuilder boundButton = new StringBuilder();
            boundButton.Append(((MyKeys)config.BindingKey == MyKeys.None) ? "None" : MyInput.Static.GetKeyName((MyKeys)config.BindingKey));
            bindingKeyButton = new MyGuiControlButton(originAlign: MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_CENTER, text: boundButton, visualStyle: VRage.Game.MyGuiControlButtonStyleEnum.ControlSetting, onButtonClick: OnBindingKeyClick, onSecondaryButtonClick: OnBindingKeySecondaryClick, toolTip: "Click to edit.\nRight click to clear.");

            CreateCheckbox(out toggleFeatureLabel, out toggleFeatureCheckbox, config.ToggleFeature, value => config.ToggleFeature = value, "Zoom Toggle\r\n(Double Press Zoom Key)", "Allow toggle by double pressing keybind.");
        }

        private void OnBindingKeyClick(MyGuiControlButton button)
        {
            MyKeys key = (MyKeys)Plugin.Instance.Config.BindingKey;
            MyPluginBinderMessageBox myGuiControlAssignKeyMessageBox = new MyPluginBinderMessageBox(key, new StringBuilder("Press desired Zoom key."), new StringBuilder("Zoom Binding"));
            myGuiControlAssignKeyMessageBox.Closed += delegate (MyGuiScreenBase s, bool isUnloading)
            {
                Plugin.Instance.Config.BindingKey = (byte)myGuiControlAssignKeyMessageBox.OutKey;
                this.RecreateControls(false);
            };
            MyGuiSandbox.AddScreen(myGuiControlAssignKeyMessageBox);
        }

        private void OnBindingKeySecondaryClick(MyGuiControlButton button)
        {
            Plugin.Instance.Config.BindingKey = (byte)MyKeys.None;
            this.RecreateControls(false);
        }

        private void OnOk(MyGuiControlButton _) => CloseScreen();

        private void CreateCheckbox(out MyGuiControlLabel labelControl, out MyGuiControlCheckbox checkboxControl, bool value, Action<bool> store, string label, string tooltip, bool enabled = true)
        {
            labelControl = new MyGuiControlLabel
            {
                Text = label,
                OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP,
                Enabled = enabled,
            };

            checkboxControl = new MyGuiControlCheckbox(toolTip: tooltip)
            {
                OriginAlign = MyGuiDrawAlignEnum.HORISONTAL_CENTER_AND_VERTICAL_TOP,
                IsChecked = value,
                Enabled = enabled,
                CanHaveFocus = enabled
            };
            if (enabled)
            {
                checkboxControl.IsCheckedChanged += cb => store(cb.IsChecked);
            }
            else
            {
                checkboxControl.IsCheckedChanged += cb => { cb.IsChecked = value; };
            }
        }

        private void LayoutControls()
        {
            var size = Size ?? Vector2.One;
            layoutTable = new MyLayoutTable(this, -0.3f * size, 0.6f * size);
            layoutTable.SetColumnWidths(400f, 100f);
            // TODO: Add more row heights here as needed
            layoutTable.SetRowHeights(90f, 90f, 150f, 60f);

            layoutTable.Add(toggleFeatureLabel, MyAlignH.Left, MyAlignV.Center, 0, 0);
            layoutTable.Add(toggleFeatureCheckbox, MyAlignH.Left, MyAlignV.Center, 0, 1);
            
            var row = 1;

            layoutTable.Add(infoText, MyAlignH.Left, MyAlignV.Top, row, 0, colSpan: 2);
            row++;


            layoutTable.Add(bindingKeyLabel, MyAlignH.Left, MyAlignV.Center, row, 0);
            layoutTable.Add(bindingKeyButton, MyAlignH.Left, MyAlignV.Center, row, 1);


            //layoutTable.Add(infoTextToggleFunction, MyAlignH.Left,MyAlignV.Center, row, 0, colSpan: 2);
            

            // TODO: Layout your UI controls here

            row++;

            layoutTable.Add(closeButton, MyAlignH.Center, MyAlignV.Center, row, 0, colSpan: 2);
            // row++;
        }
    }
}