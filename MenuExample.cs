﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GTA;
using NativeUI;

public class MenuExample : Script
{
    private UIMenu mainMenu;
    private UIMenuCheckboxItem ketchupCheckbox;
    private UIMenuListItem dishesListItem;
    private UIMenuItem cookItem;

    public MenuExample()
    {
        Tick += OnTick;
        KeyDown += OnKeyDown;

        mainMenu = new UIMenu("Native UI", "~b~NATIVEUI SHOWCASE", new Point(20, 10));
        mainMenu.AddItem(ketchupCheckbox = new UIMenuCheckboxItem("Add ketchup?", false, "Do you wish to add ketchup?"));
        var foods = new List<dynamic>
        {
            "Banana",
            "Apple",
            "Pizza",
            "Quartilicious",
            0xF00D, // Dynamic!
        };
        mainMenu.AddItem(dishesListItem = new UIMenuListItem("Food", foods, 0));
        mainMenu.AddItem(cookItem = new UIMenuItem("Cook!", "Cook the dish with the appropiate ingredients and ketchup."));
        cookItem.LeftBadge = UIMenuItem.BadgeStyle.Star;
        cookItem.RightBadge = UIMenuItem.BadgeStyle.Tick;
        mainMenu.RefreshIndex();

        mainMenu.OnItemSelect += OnItemSelect;
        mainMenu.OnListChange += OnListChange;
        mainMenu.OnCheckboxChange += OnCheckboxChange;
        mainMenu.OnIndexChange += OnItemChange;
    }

    public void OnItemChange(UIMenu sender, int index)
    {
        sender.MenuItems[index].LeftBadge = UIMenuItem.BadgeStyle.None;
    }

    public void OnCheckboxChange(UIMenu sender, UIMenuCheckboxItem checkbox, bool Checked)
    {
        if (sender != mainMenu || checkbox != ketchupCheckbox) return; // We only want to detect changes from our menu.
        UI.Notify("~r~Ketchup status: ~b~" + Checked);
    }

    public void OnListChange(UIMenu sender, UIMenuListItem list, int index)
    {
        if (sender != mainMenu || list != dishesListItem) return; // We only want to detect changes from our menu.
        string dish = list.IndexToItem(index).ToString();
        UI.Notify("Preparing ~b~" + dish +"~w~...");
    }
    
    public void OnItemSelect(UIMenu sender, UIMenuItem selectedItem, int index)
    {
        if (sender != mainMenu || selectedItem != cookItem) return; // We only want to detect changes from our menu and our button.
        // You can also detect the button by using index
        string dish = dishesListItem.IndexToItem(dishesListItem.Index).ToString();
        bool ketchup = ketchupCheckbox.Checked;

        string output = ketchup
            ? "You have ordered ~b~{0}~w~ ~r~with~w~ ketchup."
            : "You have ordered ~b~{0}~w~ ~r~without~w~ ketchup.";
        UI.ShowSubtitle(String.Format(output, dish));
    }

    public void OnTick(object o, EventArgs e)
    {
        mainMenu.Draw();
    }

    public void OnKeyDown(object o, KeyEventArgs e)
    {
        mainMenu.ProcessKey(e.KeyCode);
        if (e.KeyCode == Keys.F5) // Our menu on/off switch
        {
            mainMenu.Visible = !mainMenu.Visible;
        }
    }
}
