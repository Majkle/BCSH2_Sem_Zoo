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
    public partial class DateFilterWithPopupControl
    {
        public DateFilterWithPopupControl()
        {
            InitializeComponent();
        }

        private static DateTime initialMinDate = DateTime.Now.AddYears(-1);
        private static DateTime initialMaxDate = DateTime.Now;

        public string Caption
        {
            get => (string)GetValue(CaptionProperty);
            set => SetValue(CaptionProperty, value);
        }
        /// <summary>
        /// Identifies the Minimum dependency property
        /// </summary>
        public static readonly DependencyProperty CaptionProperty = DependencyProperty.Register(nameof(Caption), typeof(string), typeof(DateFilterWithPopupControl)
                , new FrameworkPropertyMetadata("Zadejte limity:", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public DateTime Minimum
        {
            get => (DateTime)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        /// <summary>
        /// Identifies the Minimum dependency property
        /// </summary>
        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof(Minimum), typeof(DateTime), typeof(DateFilterWithPopupControl)
                , new FrameworkPropertyMetadata(initialMinDate, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, _) => ((DateFilterWithPopupControl)sender).Range_Changed()));

        public DateTime Maximum
        {
            get => (DateTime)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }
        /// <summary>
        /// Identifies the Maximum dependency property
        /// </summary>
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof(Maximum), typeof(DateTime), typeof(DateFilterWithPopupControl)
                , new FrameworkPropertyMetadata(initialMaxDate, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, _) => ((DateFilterWithPopupControl)sender).Range_Changed()));


        public bool IsPopupVisible
        {
            get => (bool)GetValue(IsPopupVisibleProperty);
            set => SetValue(IsPopupVisibleProperty, value);
        }
        /// <summary>
        /// Identifies the IsPopupVisible dependency property
        /// </summary>
        public static readonly DependencyProperty IsPopupVisibleProperty =
            DependencyProperty.Register(nameof(IsPopupVisible), typeof(bool), typeof(DateFilterWithPopupControl), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        private void Range_Changed()
        {
            Filter = DateTime.Compare(Maximum, Minimum) > 0 ? new DateContentFilter(Minimum, Maximum) : new DateContentFilter(Minimum, DateTime.MaxValue);
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
            DependencyProperty.Register(nameof(Filter), typeof(IContentFilter), typeof(DateFilterWithPopupControl), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, (sender, _) => { }));
    }

    public class DateContentFilter : IContentFilter
    {
        public DateContentFilter(DateTime min, DateTime max)
        {
            Min = min;
            Max = max;
        }

        public DateTime Min { get; }

        public DateTime Max { get; }

        public bool IsMatch(object? value)
        {
            if (value == null)
                return false;

            if (!DateTime.TryParse(value.ToString(), out var date))
            {
                return false;
            }

            return DateTime.Compare(date, Min) >= 0 && DateTime.Compare(date, Max) <= 0;
        }
    }
}
