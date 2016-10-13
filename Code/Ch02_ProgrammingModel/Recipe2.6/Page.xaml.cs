using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Ch02_ProgrammingModel.Recipe2_6
{
  public partial class Page : UserControl
  {
    private IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
    private string setting = "MySettings";
    private string FormDataFileName = "FormFields.data";
    private string FormDataDirectory = "FormData";

    public Page()
    {
      InitializeComponent();
    }

    private void UserControl_Loaded(object sender, RoutedEventArgs e)
    {
      try
      {
        if (settings.Keys.Count != 0)
        {
          settingTextData.Text = settings[setting].ToString();
        }
      }
      catch (IsolatedStorageException ex)
      {
        settingTextData.Text = "Error saving setting: " + ex.Message;
      }
    }

    private void SaveFormData_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        using (var store = IsolatedStorageFile.GetUserStoreForApplication())
        {
          //Use to control loop for finding correct number of textboxes
          int TotalFields = 4;
          StringBuilder formData = new StringBuilder(50);
          for (int i = 1; i <= TotalFields; i++)
          {
            TextBox tb = FindName("Field" + i.ToString()) as TextBox;
            if (tb != null)
              formData.Append(tb.Text);
            //If on last TextBox value, don't add "|" character to end of data
            if (i != TotalFields)
              formData.Append("|");
          }
          store.CreateDirectory(FormDataDirectory);
          IsolatedStorageFileStream fileHandle = store.CreateFile(System.IO.Path.Combine(FormDataDirectory, FormDataFileName));

          using (StreamWriter sw = new StreamWriter(fileHandle))
          {
            sw.WriteLine(formData);
            sw.Flush();
            sw.Close();
          }
        }
      }
      catch (IsolatedStorageException ex)
      {
        settingTextData.Text = "Error saving data: " + ex.Message;
      }
    }

    private void ReadFormData_Click(object sender, RoutedEventArgs e)
    {
      using (var store = IsolatedStorageFile.GetUserStoreForApplication())
      {
        //Load form data using private string values for directory and filename
        string filePath = System.IO.Path.Combine(FormDataDirectory, FormDataFileName);
        //Check to see if file exists before proceeding
        if (store.FileExists(filePath))
        {
          using (StreamReader sr = new StreamReader(
              store.OpenFile(filePath, FileMode.Open, FileAccess.Read)))
          {
            string formData = sr.ReadLine();
            //Split string based on seperator used in SaveFormData method
            string[] fieldValues = formData.Split('|');
            for (int i = 1; i <= fieldValues.Count(); i++)
            {
              //Use the FindName method to loop through TextBoxes
              TextBox tb = FindName("Field" + i.ToString()) as TextBox;
              if (tb != null)
                tb.Text = fieldValues[i - 1];
            }
            sr.Close();
          }
        }
      }
    }

    private void ButtonUpdateSetting(object sender, RoutedEventArgs e)
    {
      try
      {
        settings[setting] = settingTextData.Text;
      }
      catch (IsolatedStorageException ex)
      {
        settingTextData.Text = "Error reading setting: " + ex.Message;
      }
    }
  }
}