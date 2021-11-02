using System.ComponentModel;
using TCGManager.Stores;

public class ViewModelBase : INotifyPropertyChanged
{
    protected NavigationStore _navigationStore { get; set; }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(params string[] propertyName)
    {
        foreach (var property in propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}

