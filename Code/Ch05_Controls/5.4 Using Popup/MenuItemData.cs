using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Ch05_Controls.Recipe5_4
{
  //data for a single menu item
  public class MenuItemData
  {
    //image URI string used to load the image
    internal string ImageUri
    {
      set
      {
        MenuItemImage = new BitmapImage();
        MenuItemImage.SetSource(this.GetType().Assembly.
          GetManifestResourceStream(this.GetType().Namespace + "." + value));
      }
    }
    //menu item image 
    public BitmapImage MenuItemImage { get; set; }
    //menu item caption
    public string MenuItemCaption { get; set; }
    //children items for submenus
    public List<MenuItemData> Children { get; set; }
    //parent menu item
    public MenuItemData Parent { get; set; }
    //toggle submenu arrow visibility based on presence of children items
    public Visibility SubMenuArrowVisibility
    {
      get
      {
        return (Children == null 
          || Children.Count == 0 ? 
          Visibility.Collapsed : Visibility.Visible);
      }
    }
  }
}