using System.Windows.Media.Imaging;
using System.Reflection;
using System.Collections.Generic;

namespace Ch05_Controls.Recipe5_9
{
  public class CustomImageSource
  {
    public string ImageName { get; set; }
    private BitmapImage _bitmapImage;
    public BitmapImage ImageFromResource
    {
      get
      {
        if (_bitmapImage == null)
        {
          _bitmapImage = new BitmapImage();
          _bitmapImage.SetSource(
            this.GetType().Assembly.GetManifestResourceStream(ImageName));
        }

        return _bitmapImage;
      }
    }    
  }
  public class ImagesCollection : List<CustomImageSource>
  {
    public ImagesCollection()
    {
      Assembly thisAssembly = this.GetType().Assembly;
      List<string> ImageNames =
        new List<string>(thisAssembly.GetManifestResourceNames());

      foreach (string Name in ImageNames)
      {
        if (Name.Contains(".png"))
          this.Add(new CustomImageSource { ImageName = Name });
      }
    }
  }
}