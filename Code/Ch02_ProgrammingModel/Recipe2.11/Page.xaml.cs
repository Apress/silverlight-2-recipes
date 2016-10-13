using System;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Ch02_ProgrammingModel.Recipe2_11
{
  public partial class Page : UserControl
  {
    private int WorkLoops = 30;
    private BackgroundWorker worker = new BackgroundWorker();
    #region Recipe 2-6 Declarations
    private IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
    private string FormDataFileName = "FormFields.data";
    private string FormDataDirectory = "FormData";
    #endregion
    public Page()
    {
      InitializeComponent();

      //Configure BackgroundWorker thread
      worker.WorkerReportsProgress = true;
      worker.WorkerSupportsCancellation = true;
      worker.DoWork += new DoWorkEventHandler(worker_DoWork);
      worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
      worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
    }

    void worker_DoWork(object sender, DoWorkEventArgs e)
    {
      for (int i = 1; i <= WorkLoops; i++)
      {
        //Check to see if the work has been canceled
        if ((worker.CancellationPending == true))
        {
          e.Cancel = true;
          break;
        }
        else
        {
          // Perform a time consuming operation and report progress.
          System.Threading.Thread.Sleep(1000);
          worker.ReportProgress((int)
              System.Math.Floor((double)i / (double)WorkLoops * 100.0));
        }
      }
      e.Result = Environment.NewLine + "Completed: " + DateTime.Now.ToString();
    }

    void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    {
      AnnimateStatusEllipse.Stop();
      if (!e.Cancelled)
      {
        StatusEllipse.Fill = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
        WorkResultsTextData.Text = WorkResultsTextData.Text + e.Result.ToString();
        ToolTipService.SetToolTip(StatusEllipse, "Work Complete.");
      }
      else
      {
        StatusEllipse.Fill = new SolidColorBrush(Color.FromArgb(255, 255, 255, 0));
        WorkResultsTextData.Text = WorkResultsTextData.Text + Environment.NewLine +
          "Canceled @: " + DateTime.Now.ToString();
        ToolTipService.SetToolTip(StatusEllipse, "Operation canceled by user.");
      }

    }

    void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
    {
      if (PromptCancelCanvas.Visibility == Visibility.Collapsed)
        ToolTipService.SetToolTip(StatusEllipse, e.ProgressPercentage.ToString() +
          "% Complete.  Click to cancel...");
    }

    private void DoWorkButton_Click(object sender, RoutedEventArgs e)
    {
      if (worker.IsBusy != true)
      {
        WorkResultsTextData.Text = "Started: " + DateTime.Now.ToString();
        worker.RunWorkerAsync(WorkResultsTextData.Text);
        AnnimateStatusEllipse.RepeatBehavior = RepeatBehavior.Forever;
        AnnimateStatusEllipse.Begin();
      }
    }

    private void StatusEllipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      if (worker.IsBusy)
        PromptCancelCanvas.Visibility = Visibility.Visible;
    }

    private void ButtonConfirmCancelYes_Click(object sender, RoutedEventArgs e)
    {
      worker.CancelAsync();
      PromptCancelCanvas.Visibility = Visibility.Collapsed;
    }

    private void ButtonConfirmCancelNo_Click(object sender, RoutedEventArgs e)
    {
      PromptCancelCanvas.Visibility = Visibility.Collapsed;
    }

    #region Recipe 2-6 Event Handlers
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
        WorkResultsTextData.Text = "Error saving data: " + ex.Message;
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
    #endregion
  }
}
