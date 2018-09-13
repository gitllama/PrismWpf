using PrismAutofac.Behavior;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Animation;

namespace Behavior
{

    public class DynamicBehavior : BehaviorBase<Grid>
    {

        #region Property

        #endregion


        #region OnSetup/Cleanup

        protected override void OnSetup()
        {
            base.OnSetup();
            this.AssociatedObject.Loaded += AssociatedObject_Loaded;
        }

        protected override void OnCleanup()
        {
            this.AssociatedObject.Loaded -= AssociatedObject_Loaded;
            base.OnCleanup();
        }

        #endregion

        public class JSON
        {
            public string title;
            public string binding = "A.Value";
        }


        List<JSON> a = new List<JSON>();
        #region Event

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            a.Add(new JSON(){ title = "a" });
            a.Add(new JSON() { title = "b" });

            var win = sender as Grid;

            var stack = new StackPanel()
            {

            };
            foreach(var i in new string[] { "a", "b" })
            {
                var s = new SliderUserControl()
                {
                    TitleName = "JsonTest"
                };
                stack.Children.Add(s);
            }

            //foreach(var i in a)
            //{
            //    var dock = new DockPanel();

            //    var label = new Label()
            //    {
            //        Content = i.title,
            //        Width = 70
            //    };
            //    Binding binding = new Binding(i.binding);
            //    var textbox = new TextBox()
            //    {

            //    };
            //    textbox.SetBinding(TextBox.TextProperty, binding);

            //    dock.Children.Add(label);
            //    dock.Children.Add(textbox);

            //    stack.Children.Add(dock);
            //}
            win.Children.Add(stack);

        }

        #endregion





    }
}