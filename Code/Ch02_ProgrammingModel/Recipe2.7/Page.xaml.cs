using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Ch02_ProgrammingModel.Recipe2_7
{
  public partial class Page : UserControl
  {
    public Page()
    {
      InitializeComponent();
    }

    private void ButtonSelectFiles_Click(object sender, RoutedEventArgs e)
    {
      //Create dialog
      OpenFileDialog fileDlg = new OpenFileDialog();
      //Set file filter as desired
      fileDlg.Filter = "Tiff Files (*.tif)|*.tif|All Files (*.*)|*.*";
      fileDlg.FilterIndex = 1;
      //Allow multiple files to be selected (false by default)
      fileDlg.Multiselect = true;
      //Show Open File Dialog
      if (true == fileDlg.ShowDialog())
      {
        StatusLabel.Text = 
            fileDlg.Files.Count() + " file(s) selected";
        foreach (var file in fileDlg.Files)
        {
          FileList.Items.Add(file.Name);
        }
      }
    }
  }
}