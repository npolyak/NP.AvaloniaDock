using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using NP.Avalonia.Visuals;
using NP.Avalonia.Visuals.Behaviors;
using NP.Avalonia.Visuals.Controls;
using NP.Avalonia.UniDock;
using NP.Avalonia.UniDock.Serialization;
using NP.Utilities;
using System;
using System.IO;

namespace DockWindowsSample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            CurrentScreenPointBehavior.CurrentScreenPoint.Subscribe(OnCurrentScreenPointChanged);


        }

        private void OnCurrentScreenPointChanged(Point2D screenPoint)
        {
            AttachedProperties.SetCurrentScreenPoint(this, screenPoint);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void SetButtonBounds()
        {
            Button button = this.FindControl<Button>("TheButton");

            ButtonBounds = button.GetScreenBounds();
        }

        const string SerializationFilePath = "../../../../SerializationResult.xml";

        #region ButtonBounds Styled Avalonia Property
        public Rect2D ButtonBounds
        {
            get { return GetValue(ButtonBoundsProperty); }
            private set { SetValue(ButtonBoundsProperty, value); }
        }

        public static readonly StyledProperty<Rect2D> ButtonBoundsProperty =
            AvaloniaProperty.Register<MainWindow, Rect2D>
            (
                nameof(ButtonBounds)
            );
        #endregion ButtonBounds Styled Avalonia Property

        public void Serialize()
        {
            DockManager dockManager = DockAttachedProperties.GetTheDockManager(this);

            var dockManagerParams = dockManager.ToParams();

            string serializationStr = 
                XmlSerializationUtils.Serialize(dockManagerParams);

            using StreamWriter writer = new StreamWriter(SerializationFilePath);

            writer.Write(serializationStr);

            writer.Flush();
        }


        public void Restore()
        {
            DockManager dockManager = DockAttachedProperties.GetTheDockManager(this);

            using StreamReader reader = new StreamReader(SerializationFilePath);

            string serializationStr = reader.ReadToEnd();

            DockManagerParams dmp = 
                XmlSerializationUtils.Deserialize<DockManagerParams>(serializationStr);

            dockManager.SetDockManagerFromParams(dmp);
        }

        public void ClearGroups()
        {
            DockManager dockManager = DockAttachedProperties.GetTheDockManager(this);

            dockManager.ClearGroups();
        }

    }
}
