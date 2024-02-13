using System.ComponentModel;

namespace ClientPlugin.Config
{
    public interface IPluginConfig: INotifyPropertyChanged
    {
        byte BindingKey { get; set; }
    }
}