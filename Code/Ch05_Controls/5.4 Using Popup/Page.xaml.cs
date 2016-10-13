using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Ch05_Controls.Recipe5_4
{
  public partial class Page : UserControl
  {
    //data for the top level menu
    internal List<MenuItemData> TopMenuData = null;
    //popups for the topmenu and the submenu
    Popup TopMenu, SubMenu;
    //Listboxes for the menu content
    ListBox lbxTopMenu, lbxSubMenu;

    public Page()
    {
      InitializeComponent();
      //initialize the menu data
      TopMenuData = new List<MenuItemData> 
      { 
        new MenuItemData{MenuItemCaption="Camera", ImageUri="Camera.png"}, 
        new MenuItemData{MenuItemCaption="CD Drive",ImageUri="CD_Drive.png"}, 
        new MenuItemData{MenuItemCaption="Computer",ImageUri="Computer.png"}, 
        new MenuItemData{MenuItemCaption="Dialup",ImageUri="Dialup.png"},
        new MenuItemData{MenuItemCaption="My Network",ImageUri="mynet.png"},
        new MenuItemData{MenuItemCaption="Mouse",ImageUri="Mouse.png"}
      };

      TopMenuData[4].Children = new List<MenuItemData> 
        {
            new MenuItemData{MenuItemCaption="Network Folder",
              ImageUri="Network_Folder.png",Parent = TopMenuData[4]}, 
            new MenuItemData{MenuItemCaption="Network Center",
              ImageUri="Network_Center.png",Parent = TopMenuData[4]}, 
            new MenuItemData{MenuItemCaption="Connect To",
              ImageUri="Network_ConnectTo.png",Parent = TopMenuData[4]}, 
            new MenuItemData{MenuItemCaption="Internet",
              ImageUri="Network_Internet.png",Parent = TopMenuData[4]}
            };

      //create and initialize the top menu popup
      TopMenu = new Popup();
      lbxTopMenu = new ListBox();
      //set the listbox style to apply the menu look templating
      lbxTopMenu.Style = this.Resources["styleMenu"] as Style;
      //bind the topmenu data
      lbxTopMenu.ItemsSource = TopMenuData;
      TopMenu.Child = lbxTopMenu;

      //create and initialize the submenu
      SubMenu = new Popup();
      lbxSubMenu = new ListBox();
      lbxSubMenu.MouseLeave += new MouseEventHandler(lbxSubMenu_MouseLeave);
      lbxSubMenu.Style = this.Resources["styleMenu"] as Style;
      SubMenu.Child = lbxSubMenu;
    }

    //set the top menu position
    private void SetTopMenuPosition(Popup Target,
      FrameworkElement CoordSpaceSource)
    {
      //get the transform to use
      GeneralTransform transform = this.TransformToVisual(CoordSpaceSource);
      //transform the left-bottom corner
      Point pt = transform.Transform(new Point(0.0,
        CoordSpaceSource.ActualHeight));
      //set offsets accordingly
      Target.HorizontalOffset = pt.X;
      Target.VerticalOffset = pt.Y;
    }
    //set the submenu position
    private void SetSubMenuPosition(Popup Target,
      FrameworkElement CoordSpaceSource, int ItemIndex, 
      FrameworkElement ParentMenuItem)
    {

      //get the transform to use
      GeneralTransform transform = this.TransformToVisual(CoordSpaceSource);
      //transform the right-top corner
      Point pt = transform.Transform(
        new Point(ParentMenuItem.ActualWidth,
          CoordSpaceSource.ActualHeight +
          (ParentMenuItem.ActualHeight * ItemIndex)));
      //set offsets accordingly
      Target.HorizontalOffset = pt.X;
      Target.VerticalOffset = pt.Y;
    }

    private void btnDropDown_Click(object sender, RoutedEventArgs e)
    {
      //position the top menu
      SetTopMenuPosition(TopMenu, LayoutRoot);
      //show or hide
      TopMenu.IsOpen = !TopMenu.IsOpen;
    }

    private void LbxItemRoot_MouseEnter(object sender, MouseEventArgs e)
    {
      //get the listboxitem for the selected top menu item
      ListBoxItem lbxItem = (sender as Grid).Parent as ListBoxItem;
      //get the bound MenuItemData
      MenuItemData midTop = (sender as Grid).DataContext as MenuItemData;
      //do we have children and are we on the top menu?
      if (midTop.Parent == null &&
        (midTop.Children == null || midTop.Children.Count == 0))
      {
        //do not show the submenu
        SubMenu.IsOpen = false;
      }
      else if (midTop.Children != null && midTop.Children.Count > 0)
      {
        //yes - position sub menu
        SetSubMenuPosition(SubMenu, LayoutRoot, TopMenuData.IndexOf(midTop),
          (sender as Grid));
        //bind to children MenuItemData collection
        lbxSubMenu.ItemsSource = midTop.Children;
        //show  submenu    
        SubMenu.IsOpen = true;
      }

    }
    //leaving submenu - close it
    void lbxSubMenu_MouseLeave(object sender, MouseEventArgs e)
    {
      SubMenu.IsOpen = false;
    }
  }
}
