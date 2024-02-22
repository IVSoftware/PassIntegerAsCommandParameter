using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace PassIntegerAsCommandParameter
{
    public partial class MainPage : ContentPage
    {
        public MainPage() => InitializeComponent();
    }
    class MainPageBindingContext : INotifyPropertyChanged
    {
        public MainPageBindingContext()
        {
            IncrementCountCommand = new Command(onIncrementCount);
            SetCountCommand = new Command<string>(onSetCount);
        }
        public ICommand SetCountCommand { get; private set; }
        private void onSetCount(string valueAsString)
        {
            Count = Convert.ToInt32(valueAsString);
        }
        public ICommand IncrementCountCommand { get; private set; }
        private void onIncrementCount(object o)
        {
            Count++;
        }
        public string ButtonText
        {
            get => _buttonText;
            set
            {
                if (!Equals(_buttonText, value))
                {
                    _buttonText = value;
                    OnPropertyChanged();
                }
            }
        }
        string _buttonText = "Click me";
        public int Count
        {
            get => _count;
            set
            {
                if (!Equals(_count, value))
                {
                    _count = value;
                    if (Count == 0)
                    {
                        ButtonText = "Click me";
                    }
                    else
                    {
                        ButtonText = $"Clicked {Count} times";
                    }
                    OnPropertyChanged();
                }
            }
        }
        int _count = default;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
