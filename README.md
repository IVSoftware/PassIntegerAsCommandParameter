## Command Parameter

It could all be simpler perhaps, and as Julian observed the `CommandParameter` in this case _is_ a `string` giving you two basic choices to make the signature match.

**SetCountCommand = new Command(onIncrementCount)**

```csharp
public partial class MainPage : ContentPage
{
    public MainPage() => InitializeComponent();
}
class MainPageBindingContext : INotifyPropertyChanged
{
    public MainPageBindingContext()
    {
        IncrementCountCommand = new Command(onIncrementCount);
        SetCountCommand = new Command(onSetCount);
    }
    public ICommand SetCountCommand { get; private set; }
    private void onSetCount(object o)
    {
        Debug.WriteLine($"Object is {o.GetType().Name}");
        Count = Convert.ToInt32(o);
    }
    .
    .
    .
}
```

_OR_

**SetCountCommand = new Command&lt;string&gt;(onSetCount)**
```
    .
    .
    .
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
    .
    .
    .
```

___

_Note: `CommandParameter` **can be** an int e.g. `CommandParameter="{StaticResource Five}"`_
___

**Maui default redux**
[![demo][1]][1]

```xaml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:PassIntegerAsCommandParameter"
             x:Class="PassIntegerAsCommandParameter.MainPage">
    <ContentPage.BindingContext>
        <local:MainPageBindingContext/>
    </ContentPage.BindingContext>
    <ScrollView>
        <VerticalStackLayout
            Spacing="25"
            Padding="30,0"
            VerticalOptions="Center">

            <Image
                Source="dotnet_bot.png"
                SemanticProperties.Description="Cute dot net bot waving hi to you!"
                HeightRequest="200"
                HorizontalOptions="Center" />

            <Label
                Text="Hello, World!"
                SemanticProperties.HeadingLevel="Level1"
                FontSize="32"
                HorizontalOptions="Center" />

            <Button
                Text="Set to 5"
                SemanticProperties.Hint="Sets count to 5"
                Command="{Binding SetCountCommand}"
                CommandParameter="5"
                HorizontalOptions="Center" />

            <Button
                Text="{Binding ButtonText}"
                SemanticProperties.Hint="Counts the number of times you click"
                Command="{Binding IncrementCountCommand}"
                HorizontalOptions="Center" />

        </VerticalStackLayout>
    </ScrollView>
</ContentPage>
```

C#

```csharp
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
                switch (Count)
                {
                    case 0: ButtonText = "Click me"; break;
                    case 1: ButtonText = $"Clicked {Count} time"; break;
                    default: ButtonText = $"Clicked {Count} times"; break;
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
```


  [1]: https://i.stack.imgur.com/vaZJy.png