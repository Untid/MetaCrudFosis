namespace MetaCrudFosis.App;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = new Window(new AppShell())
        {
            Title = "MetaCrudFosis"
        };

#if WINDOWS
        window.Width = 1024;
        window.Height = 768;
#endif
        return window;
    }
}