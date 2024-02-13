using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VRage.Input;

#if !TORCH

namespace ClientPlugin.Config
{
    public class PluginConfig: IPluginConfig
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void SetValue<T>(ref T field, T value, [CallerMemberName] string propName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return;

            field = value;

            OnPropertyChanged(propName);
        }

        private void OnPropertyChanged([CallerMemberName] string propName = "")
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged == null)
                return;

            propertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        private byte bindingKey = (byte)MyKeys.CapsLock;

        public byte BindingKey
        {
            get => bindingKey;
            set => SetValue(ref bindingKey, value);
        }
    }
}

#endif