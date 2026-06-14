namespace MetaCrudFosis.App;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        webView.Source = Config.WebUrl;
    }
}