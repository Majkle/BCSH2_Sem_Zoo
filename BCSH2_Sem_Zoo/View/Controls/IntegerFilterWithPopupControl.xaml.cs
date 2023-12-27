using DataGridExtensions;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace BCSH2_Sem_Zoo.View.Controls
{
    /// <summary>
    /// Interaction logic for FilterWithPopupControl.xaml
    /// </summary>
    public partial class IntegerFilterWithPopupControl
    {
        public IntegerFilterWithPopupControl()
        {
            InitializeComponent();
        }

        public string Caption
        {
            get => (string)GetValue(CaptionProperty);
            set => SetValue(CaptionProperty, value);
        }
        /// <summary>
        /// Identifies the Minimum dependency property
        /// </summary>
        public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register(nameof(Caption), typeof(string), typeof(IntegerFilterWithPopupControl)
                , new FrameworkPropertyMetadata("Zadejte limity:", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }
        /// <summary>
        /// Identifies the Minimum dependency property
        /// </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof(Minimum), typeof(double), typeof(IntegerFilterWithPopupControl)
                , new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, _) => ((IntegerFilterWithPopupControl)sender).Range_Changed()));

        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }
        /// <summary>
        /// Identifies the Maximum dependency property
        /// </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(IntegerFilterWithPopupControl)
                , new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, _) => ((IntegerFilterWithPopupControl)sender).Range_Changed()));


        public bool IsPopupVisible
        {
            get => (bool)GetValue(IsPopupVisibleProperty);
            set => SetValue(IsPopupVisibleProperty, value);
        }
        /// <summary>
        /// Identifies the IsPopupVisible dependency property
        /// </summary>
        public static readonly DependencyProperty IsPopupVisibleProperty =
            DependencyProperty.Register(nameof(IsPopupVisible), typeof(bool), typeof(IntegerFilterWithPopupControl), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        private void Range_Changed()
        {
            Filter = Maximum > Minimum ? new IntegerContentFilter(Minimum, Maximum) : new IntegerContentFilter(Minimum, double.MaxValue);
        }

        public IContentFilter? Filter
        {
            get => (IContentFilter?)GetValue(FilterProperty);
            set => SetValue(FilterProperty, value);
        }
        /// <summary>
        /// Identifies the Filter dependency property
        /// </summary>
        public static readonly DependencyProperty FilterProperty =
            DependencyProperty.Register(nameof(Filter), typeof(IContentFilter), typeof(IntegerFilterWithPopupControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, _) => { }));

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }

    public class IntegerContentFilter : IContentFilter
    {
        public IntegerContentFilter(double min, double max)
        {
            Min = min;
            Max = max;
        }

        public double Min { get; }

        public double Max { get; }

        public bool IsMatch(object? value)
        {
            if (value == null)
                return false;

            if (!double.TryParse(value.ToString(), out var number))
            {
                return false;
            }

            return (number >= Min) && (number <= Max);
        }
    }
}
