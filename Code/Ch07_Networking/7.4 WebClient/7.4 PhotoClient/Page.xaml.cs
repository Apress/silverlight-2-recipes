using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Xml.Linq;

namespace Ch07_Networking.Recipe7_4.PhotoClient
{
  public partial class Page : UserControl
  {

    private const string MetadataDownloadUri =
      "http://localhost:9494/MetaData.svc";
    private const string MetadataUploadUri =
      "http://localhost:9494/MetaDataUpload.aspx";
    private const string PhotoDownloadUri =
      "http://localhost:9494/PhotoDownload.svc";
    private const string PhotoUploadUri =
      "http://localhost:9494/PhotoUpload.aspx";


    ObservableCollection<WrappedImage> ImageSources =
      new ObservableCollection<WrappedImage>();
    WebClient wcThumbZip = new WebClient();


    public Page()
    {
      InitializeComponent();
      lbxThumbs.ItemsSource = ImageSources;
      contentctlLargeImage.Content = new WrappedImage();
      GetImageNames();
    }

    private void GetImageNames()
    {
      //create a WebClient
      WebClient wcImageNames = new WebClient();
      //attach a handler to the OpenReadCompleted event
      wcImageNames.OpenReadCompleted +=
        new OpenReadCompletedEventHandler(
          delegate(object sender, OpenReadCompletedEventArgs e)
          {
            //initialize a Json Serializer
            DataContractJsonSerializer jsonSer =
              new DataContractJsonSerializer(typeof(List<string>));
            //deserialize the returned Stream to a List<string>
            List<string> FileNames =
              jsonSer.ReadObject(e.Result) as List<string>;
            //start loading the thumbnails
            LoadThumbNails(FileNames);
          });
      //Start reading the remote resource as a stream
      wcImageNames.OpenReadAsync(
        new Uri(string.Format("{0}/GetPhotoFileNames", MetadataDownloadUri)));

    }

    private void LoadThumbNails(List<string> ImageFileNames)
    {
      wcThumbZip.OpenReadCompleted +=
        new OpenReadCompletedEventHandler(wcThumbZip_OpenReadCompleted);
      wcThumbZip.DownloadProgressChanged +=
        new DownloadProgressChangedEventHandler
          (
            delegate(object Sender, DownloadProgressChangedEventArgs e)
            {
              //set the progress bar value to the reported progress percentage
              pbarThumbZipDownload.Value = e.ProgressPercentage;
            }
          );
      //start reading the thumbnails zip file as a stream, 
      //pass in the ImageFileNames List<string> as user state
      wcThumbZip.OpenReadAsync(
        new Uri(
          string.Format("{0}/GetThumbs", PhotoDownloadUri)), ImageFileNames);
    }

    void wcThumbZip_OpenReadCompleted(object sender,
      OpenReadCompletedEventArgs e)
    {
      //if operation was cancelled, return.
      if (e.Cancelled) return;
      //grab the passed in user state from 
      //e.UserState, and cast it appropriately
      List<string> FileNames = e.UserState as List<string>;
      //create a StreamResourceInfo wrapping the returned stream, 
      //with content type set to .PNG
      StreamResourceInfo resInfo = new StreamResourceInfo(e.Result, "image/png");
      //for each file name
      for (int i = 0; i < FileNames.Count; i++)
      {
        //create and initialize a WrappedImage instance
        WrappedImage wi =
          new WrappedImage
          {
            Small = new BitmapImage(),
            Large = null,
            FileName = FileNames[i] + ".jpg",
            ThumbName = FileNames[i] + ".png"
          };
        try
        {
          //Read the thumb nail image from the returned stream (the zip file)
          Stream ThumbStream = Application.GetResourceStream(
            resInfo, new Uri(wi.ThumbName, UriKind.Relative)).Stream;
          //and save it in the WrappedImage instance
          wi.Small.SetSource(ThumbStream);
          //and bind it to the thumbnail listbox
          ImageSources.Add(wi);
        }
        catch
        {
        }
      }
      //hide the progress bar and show the ListBox
      visualThumbZipDownload.Visibility = Visibility.Collapsed;
      lbxThumbs.Visibility = Visibility.Visible;
    }
    private void btnZipDownloadCancel_Click(object sender, RoutedEventArgs e)
    {
      //if downloading thumbnail zip , issue an async request to cancel
      if (wcThumbZip != null && wcThumbZip.IsBusy)
        wcThumbZip.CancelAsync();
    }


    //thumbnail selection changed
    private void lbxThumbs_SelectionChanged(object sender,
      SelectionChangedEventArgs e)
    {
      //get the WrappedImage bound to the selected item
      WrappedImage wi = (e.AddedItems[0] as WrappedImage);
      //bind it to the large image display, as well to the metadata display
      contentctlLargeImage.Content = wi;
      contentctlImageInfo.Content = wi;
      //if the large image has not been downloaded
      if (wi.Large == null)
      {
        //display the progress bar and hid the large image control
        wi.ProgressVisible = Visibility.Visible;
        wi.ImageVisible = Visibility.Collapsed;
        //initialize the BitmapImage for the large image
        wi.Large = new BitmapImage();
        //new web client
        WebClient wcLargePhoto = new WebClient();
        //progress change handler
        wcLargePhoto.DownloadProgressChanged +=
          new DownloadProgressChangedEventHandler(
            delegate(object Sender, DownloadProgressChangedEventArgs e1)
            {
              //update value bound to progress bar
              wi.PercentProgress = e1.ProgressPercentage;
            });
        //completion handler
        wcLargePhoto.DownloadStringCompleted +=
          new DownloadStringCompletedEventHandler(
            wcLargePhoto_DownloadStringCompleted);
        //dowmload image bytes as a string, pass 
        //in WrappedImage instance as user supplied state
        wcLargePhoto.DownloadStringAsync(
          new Uri(string.Format("{0}/Photos?Name={1}",
            PhotoDownloadUri, wi.FileName)), wi);
      }
    }
    //large image download completed
    void wcLargePhoto_DownloadStringCompleted(object sender,
      DownloadStringCompletedEventArgs e)
    {
      //get the WrappedImage instance from user supplied state
      WrappedImage wi = (e.UserState as WrappedImage);
      //parse XML formatted response string into an XDocument
      XDocument xDoc = XDocument.Parse(e.Result);
      //grab the root, and decode the deafult base64 
      //representation into the image bytes
      byte[] Buff = Convert.FromBase64String((string)xDoc.Root);
      //wrap in a memory stream,and 
      MemoryStream ms = new MemoryStream(Buff);
      wi.Large.SetSource(ms);
      wi.ProgressVisible = Visibility.Collapsed;
      wi.ImageVisible = Visibility.Visible;
      GetPhotoMetadata(wi);
    }

    private void btnPrev_Click(object sender, RoutedEventArgs e)
    {
      if (lbxThumbs.SelectedIndex == 0) return;
      lbxThumbs.SelectedIndex = lbxThumbs.SelectedIndex - 1;
    }

    private void btnNext_Click(object sender, RoutedEventArgs e)
    {
      if (lbxThumbs.SelectedIndex == lbxThumbs.Items.Count - 1) return;
      lbxThumbs.SelectedIndex = lbxThumbs.SelectedIndex + 1;
    }

    private void btnMeta_Checked(object sender, RoutedEventArgs e)
    {
      contentctlImageInfo.Visibility = Visibility.Visible;
    }

    private void btnMeta_Unchecked(object sender, RoutedEventArgs e)
    {
      contentctlImageInfo.Visibility = Visibility.Collapsed;
    }

    private void GetPhotoMetadata(WrappedImage wi)
    {

      WebClient wcMetadataDownload = new WebClient();
      wcMetadataDownload.DownloadStringCompleted +=
        new DownloadStringCompletedEventHandler(
          delegate(object sender, DownloadStringCompletedEventArgs e)
          {
            DataContractJsonSerializer JsonSer =
              new DataContractJsonSerializer(typeof(PhotoMetaData));
            //decode UTF8 string to byte[], wrap in a memory string and 
            //deserialize to PhotoMetadata using DatacontractJsonSerializer
            PhotoMetaData pmd = JsonSer.ReadObject(
              new MemoryStream(new UTF8Encoding().GetBytes(e.Result)))
              as PhotoMetaData;
            //data bind
            (e.UserState as WrappedImage).Info = pmd;
          });
      wcMetadataDownload.DownloadStringAsync(
        new Uri(string.Format("{0}/PhotoMetadata?Id={1}",
          MetadataDownloadUri,
          wi.FileName)), wi);
    }


    private void btnSaveMetaData_Click(object sender, RoutedEventArgs e)
    {
      SetPhotoMetadata(contentctlImageInfo.Content as WrappedImage);
    }
    //upload metadata
    private void SetPhotoMetadata(WrappedImage wi)
    {
      //new WebClient
      WebClient wcMetadataUpload = new WebClient();
      //serialize the metadata as JSON 
      DataContractJsonSerializer JsonSer =
        new DataContractJsonSerializer(typeof(PhotoMetaData));
      MemoryStream ms = new MemoryStream();
      JsonSer.WriteObject(ms, wi.Info);
      //convert serialized form to a string
      string SerOutput = new UTF8Encoding().
        GetString(ms.GetBuffer(), 0, (int)ms.Length);
      ms.Close();
      //upload string
      //wcMetadataUpload.UploadStringAsync(
      //  new Uri(string.Format("{0}/SetPhotoMetaData", MetadataUri)),
      //  SerOutput);

      wcMetadataUpload.UploadStringAsync(
        new Uri(MetadataUploadUri), "POST",
       SerOutput);
    }


    //upload local image file
    private void btnUpload_Click(object sender, RoutedEventArgs e)
    {
      //open a file dialog and allow the user to select local image files
      OpenFileDialog ofd = new OpenFileDialog();
      ofd.Filter = "JPEG Images|*.jpg;*.jpeg";
      ofd.Multiselect = true;
      if (ofd.ShowDialog() == false) return;
      //for each selected file
      foreach (FileInfo fdfi in ofd.Files)
      {
        //new web client
        WebClient wcPhotoUpload = new WebClient();
        //content type
        //wcPhotoUpload.Headers["Content-Type"] = "image/jpeg";
        //name of the file as a custom property in header
        wcPhotoUpload.Headers["Image-Name"] = fdfi.Name;
        wcPhotoUpload.OpenWriteCompleted +=
          new OpenWriteCompletedEventHandler(wcPhotoUpload_OpenWriteCompleted);
        //upload image file - pass in the image file stream as user supplied state
        //wcPhotoUpload.OpenWriteAsync(new Uri(string.Format("{0}/AddPhoto", UploadUri)),
        //  "POST", fdfi.OpenRead());

        wcPhotoUpload.OpenWriteAsync(new Uri(PhotoUploadUri),
          "POST", fdfi.OpenRead());
      }
    }

    void wcPhotoUpload_OpenWriteCompleted(object sender, OpenWriteCompletedEventArgs e)
    {
      //get the image file stream from the user supplied state
      Stream imageStream = e.UserState as Stream;
      //write the image file out to the upload stream avialable in e.Result
      int ChunkSize = 1024 * 1024;
      int ReadCount = 0;
      byte[] Buff = new byte[ChunkSize];
      do
      {
        ReadCount = imageStream.Read(Buff, 0, ChunkSize);
        e.Result.Write(Buff, 0, ReadCount);
      } while (ReadCount == ChunkSize);
      //close upload stream and return - framework will upload in the background
      e.Result.Close();
    }
  }
}
