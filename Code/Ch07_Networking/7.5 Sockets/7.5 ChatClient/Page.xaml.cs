using System.Windows;
using System.Windows.Controls;

namespace Ch07_Networking.Recipe7_5.ChatClient
{
  public partial class Page : UserControl
  {
    public ClientConnectionManager ConnManager { get; set; }

    public Page()
    {
      InitializeComponent();
      //initialize the ClientConnectionManager
      ConnManager = new ClientConnectionManager { ParentPage = this };
      //set the data context to the ClientConnetionManager
      LayoutRoot.DataContext = ConnManager;
    }

    private void btnJoin_Click(object sender, RoutedEventArgs e)
    {
      ConnManager.Join();
    }
    private void btnLogoff_Click(object sender, RoutedEventArgs e)
    {
      ConnManager.Disconnect();
    }

    private void btnTalk_Click(object sender, RoutedEventArgs e)
    {
      //get the participant name from the Button.Tag
      //which was bound to the name at data binding
      ConnManager.TalkingTo = (sender as Button).Tag as string;
      ShowChatView();
    }

    private void btnSend_Click(object sender, RoutedEventArgs e)
    {
      ConnManager.SendTextMessage();
    }

    private void btnEndChat_Click(object sender, RoutedEventArgs e)
    {
      ConnManager.SendChatEnd();
    }
    internal void ShowParticipantsView()
    {
      viewParticipants.Visibility = Visibility.Visible;
      viewLogin.Visibility = Visibility.Collapsed;
      viewChat.Visibility = Visibility.Collapsed;
    }
    internal void ShowChatView()
    {
      viewParticipants.Visibility = Visibility.Collapsed;
      viewLogin.Visibility = Visibility.Collapsed;
      viewChat.Visibility = Visibility.Visible;
    }
    internal void ShowLoginView()
    {
      viewParticipants.Visibility = Visibility.Collapsed;
      viewLogin.Visibility = Visibility.Visible;
      viewChat.Visibility = Visibility.Collapsed;
    }
  }
}




