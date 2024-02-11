
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace MvvmExample;

public record class UserPrincipal(string UserName);

//[ObservableObject]
//[ObservableRecipient]
public partial class MainWindowViewModel : ObservableObject //ObservableRecipient, IRecipient<UserPrincipal>
{
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ClickCommand))]
    private string? _firstName = "John";

    public MainWindowViewModel()
    {
        //Messenger.Register<UserPrincipal>(this);

        ////... somewhere else in the app sends message like this:
        //Messenger.Send(new UserPrincipal("Bob"));
    }

    public void Receive(UserPrincipal message)
    {
        FirstName = message.UserName;
    }

    private bool CanClick() => FirstName == "John";

    partial void OnFirstNameChanging(string? value)
    {

    }

    [RelayCommand(CanExecute = nameof(CanClick))]
    private async Task Click()
    {
        await Task.Delay(1_000);
        FirstName = "Not John";
    }


}
