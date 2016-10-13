using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Ch03_DevelopingUX.Recipe3_15
{
  public partial class Page : UserControl
  {
    double moveSpeed = 10;
    double leftPosition = 0;
    double topPosition = 0;

    public Page()
    {
      InitializeComponent();
      this.KeyUp += new KeyEventHandler(GameCanvas_KeyUp);

      leftPosition = (double)RadioactiveBall.GetValue(Canvas.LeftProperty);
      topPosition = (double)RadioactiveBall.GetValue(Canvas.TopProperty);
    }

    //Start Game
    private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      ClickToPlay.Visibility = Visibility.Collapsed ;
      WelcomeMessage.Visibility = Visibility.Collapsed ;
      RadioactiveBall.Visibility = Visibility.Visible;
      SpinGameBallStoryboard.Begin();
    }

    //Re-position ball
    private void Draw()
    {
      RadioactiveBall.SetValue(Canvas.LeftProperty, leftPosition);
      RadioactiveBall.SetValue(Canvas.TopProperty, topPosition);
    }

    //Calculate new position based on key pressed.
    //Perform basic edge collision detection
    private void GameCanvas_KeyUp(object sender, KeyEventArgs e)
    {
      switch (e.Key)
      {
        case Key.Right: if ((leftPosition) <= (this.Width - (RadioactiveBall.Width*1.25)))
                          leftPosition += moveSpeed;
                        break;
        case Key.Left: if (leftPosition >= (RadioactiveBall.Width * .25))
                          leftPosition -= moveSpeed;
                        break;
        case Key.Up: if (topPosition >= (RadioactiveBall.Height * .25))
                          topPosition -= moveSpeed;
                        break;
        case Key.Down: if (topPosition <= (this.Height - (RadioactiveBall.Height*1.25)))
                          topPosition += moveSpeed;
                        break;
      }
      Draw();
    }
  }
}
