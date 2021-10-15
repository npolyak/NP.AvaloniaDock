﻿using Avalonia.Controls;
using NP.Avalonia.UniDock;
using NP.Avalonia.UniDock.Factories;

namespace NP.DockItemsInMenus
{
    public class MyCustomFloatingWindowFactory : IFloatingWindowFactory
    {
        public virtual FloatingWindow CreateFloatingWindow(object _)
        {
            // create the window

            FloatingWindow dockWindow = new FloatingWindow();

            dockWindow.Classes = new Classes("PlainFloatingWindow", "MyFloatingWindow");
            dockWindow.TitleClasses = "WindowTitle";

            return dockWindow;
        }
    }
}