﻿// (c) Nick Polyak 2021 - http://awebpros.com/
// License: MIT License (https://opensource.org/licenses/MIT)
//
// short overview of copyright rules:
// 1. you can use this framework in any commercial or non-commercial 
//    product as long as you retain this copyright message
// 2. Do not blame the author of this software if something goes wrong. 
// 
// Also, please, mention this software in any documentation for the 
// products that use it.

using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

namespace NP.Avalonia.UniDock
{
    public class DragHeaderBehavior : DragItemBehavior<DockItemHeaderControl>
    {
        #region IsSet Attached Avalonia Property
        public static void SetIsSet(AvaloniaObject obj, bool value)
        {
            obj.SetValue(IsSetProperty, value);
        }
        #endregion IsSet Attached Avalonia Property

        private static DockItem? GetDockItem(DockItemHeaderControl headerControl) =>
            headerControl.DataContext as DockItem;

        protected override bool MoveItemWithinContainer(Control itemsContainer, PointerEventArgs e)
        {
            return false;
        }

        public DragHeaderBehavior() : base(GetDockItem!)
        {

        }

        static DragHeaderBehavior()
        {
            Instance = new DragHeaderBehavior();
        }
    }
}