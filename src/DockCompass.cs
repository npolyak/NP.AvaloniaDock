﻿using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.VisualTree;
using NP.Avalonia.Visuals;
using NP.Avalonia.Visuals.Behaviors;
using NP.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NP.Avalonia.UniDock
{
    public class DockCompass : TemplatedControl
    {
        private IList<DockSideControl>? SideControls { get; set; }

        private IDisposable? _subscriptionDisposable = null;

        private bool IsSubscribed => _subscriptionDisposable != null;

        internal bool CanStartPointerDetection { get; set; } = true;

        protected void StartPointerDetection()
        {
            if (!CanStartPointerDetection || !IsEffectivelyVisible || !IsInitialized)
                return;

            UpdateSideControls();

            if (IsSubscribed)
            {
                return;
            }

            _subscriptionDisposable =
                CurrentScreenPointBehavior.CurrentScreenPoint.Subscribe(OnPointerMoved);
        }


        #region StartOrEndPointerDetection Styled Avalonia Property
        public bool StartOrEndPointerDetection
        {
            get { return GetValue(StartOrEndPointerDetectionProperty); }
            set { SetValue(StartOrEndPointerDetectionProperty, value); }
        }

        public static readonly StyledProperty<bool> StartOrEndPointerDetectionProperty =
            AvaloniaProperty.Register<DockCompass, bool>
            (
                nameof(StartOrEndPointerDetection)
            );
        #endregion StartOrEndPointerDetection Styled Avalonia Property


        static DockCompass()
        {
            StartOrEndPointerDetectionProperty
                .Changed
                .AddClassHandler<DockCompass>((x, e) => x.StartOrEndPointerDetectionChanged(e));
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            StartPointerDetection();
        }

        private void StartOrEndPointerDetectionChanged(AvaloniaPropertyChangedEventArgs flagChanged)
        {
            bool? flag = (bool?)flagChanged.NewValue;

            if (flag == true)
            {
                StartPointerDetection();
            }
            else
            {
                FinishPointerDetection();
            }
        }

        private void UpdateSideControls()
        {
            if (SideControls == null || SideControls.Count < 5)
            {
                SideControls =
                    this.GetVisualDescendants()
                        .OfType<DockSideControl>()
                        .ToList();
            }
        }

        private void OnPointerMoved(Point2D pointerScreenLocation)
        {
            UpdateSideControls();

            var currentSideControl =
                SideControls
                    ?.FirstOrDefault
                    (
                        c => PointHelper.GetScreenBounds(c).ContainsPoint(pointerScreenLocation));

            if (currentSideControl == null)
            {
                this.ClearValue(DockAttachedProperties.DockSideProperty);
                return;
            }

            var currentDockSide = DockAttachedProperties.GetDockSide(this);
            var dockSide = DockAttachedProperties.GetDockSide(currentSideControl);

            if (dockSide != currentDockSide)
            {
                DockAttachedProperties.SetDockSide(this, dockSide);
            }
        }

        public void FinishPointerDetection()
        {
            _subscriptionDisposable?.Dispose();
            _subscriptionDisposable = null;
            this.ClearValue(DockAttachedProperties.DockSideProperty);
        }
    }
}
