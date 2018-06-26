using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace RateThisStuff_Client.CustomControls
{
    /// <summary>
    /// Interaction logic for Rating.xaml
    /// </summary>
    public partial class Rating
    {
        #region Ctor

        public Rating()
        {
            InitializeComponent();

            Loaded += Rating_Loaded;
        }

        static Rating()
        {
            ValueProperty = DependencyProperty.Register("Value", typeof(int), typeof(Rating), new FrameworkPropertyMetadata(0, OnValueChanged, CoerceValueValue));
            MaximumProperty = DependencyProperty.Register("Maximum", typeof(int), typeof(Rating), new FrameworkPropertyMetadata(5));
            MinimumProperty = DependencyProperty.Register("Minimum", typeof(int), typeof(Rating), new FrameworkPropertyMetadata(0));
            StarOnColorProperty = DependencyProperty.Register("StarOnColor", typeof(Brush), typeof(Rating), new FrameworkPropertyMetadata(Brushes.Yellow, OnStarOnColorChanged));
            StarOffColorProperty = DependencyProperty.Register("StarOffColor", typeof(Brush), typeof(Rating), new FrameworkPropertyMetadata(Brushes.White, OnStarOffColorChanged));
            StarStrokeColorProperty = DependencyProperty.Register("StarStrokeColor", typeof(Brush), typeof(Rating), new FrameworkPropertyMetadata(Brushes.Black, OnStarStrokeColorChanged));
        }

        #endregion

        #region Events

        /// <summary>
        /// Notifies when rating has been changed.
        /// </summary>
        public event EventHandler<RatingChangedEventArgs> RatingChanged;

        #endregion

        #region Dependency Properties

        /// <summary>
        /// Value dependency property.
        /// </summary>
        public static readonly DependencyProperty ValueProperty;

        /// <summary>
        /// Maximum dependency property.
        /// </summary>
        public static readonly DependencyProperty MaximumProperty;

        /// <summary>
        /// Minimum dependency property.
        /// </summary>
        public static readonly DependencyProperty MinimumProperty;

        /// <summary>
        /// Star on color dependency property.
        /// </summary>
        public static readonly DependencyProperty StarOnColorProperty;

        /// <summary>
        /// Star off color dependency property.
        /// </summary>
        public static readonly DependencyProperty StarOffColorProperty;

        /// <summary>
        /// Star off color dependency property.
        /// </summary>
        public static readonly DependencyProperty StarStrokeColorProperty;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum value.
        /// </summary>
        public int Maximum
        {
            get { return (int)GetValue(MaximumProperty); }
            set { SetValue(MaximumProperty, value); }
        }

        /// <summary>
        /// Gets or sets the minimum value.
        /// </summary>
        public int Minimum
        {
            get { return (int)GetValue(MinimumProperty); }
            set { SetValue(MinimumProperty, value); }
        }

        /// <summary>
        /// Gets or sets the star on color.
        /// </summary>
        public Brush StarOnColor
        {
            get { return (Brush)GetValue(StarOnColorProperty); }
            set { SetValue(StarOnColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the star off color.
        /// </summary>
        public Brush StarOffColor
        {
            get { return (Brush)GetValue(StarOffColorProperty); }
            set { SetValue(StarOffColorProperty, value); }
        }

        /// <summary>
        /// Gets or sets the star off color.
        /// </summary>
        public Brush StarStrokeColor
        {
            get { return (Brush)GetValue(StarStrokeColorProperty); }
            set { SetValue(StarStrokeColorProperty, value); }
        }

        #endregion

        #region Event Handlers

        private static void OnValueChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            Rating rating = obj as Rating;

            if (rating != null)
            {
                rating.OnRatingChanged();
            }
        }

        private static object CoerceValueValue(DependencyObject obj, object value)
        {
            Rating rating = (Rating)obj;

            int current = (int)value;

            if (current < rating.Minimum)
            {
                current = rating.Minimum;
            }

            if (current > rating.Maximum)
            {
                current = rating.Maximum;
            }

            return current;
        }

        private static void OnStarOffColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            Rating rating = obj as Rating;

            if (rating != null)
            {
                Brush offColor = (Brush)e.NewValue;

                foreach (Star star in rating.stackPanelStars.Children)
                {
                    star.OffColor = offColor;
                }
            }
        }

        private static void OnStarOnColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            Rating rating = obj as Rating;

            if (rating != null)
            {
                Brush onColor = (Brush)e.NewValue;

                foreach (Star star in rating.stackPanelStars.Children)
                {
                    star.OnColor = onColor;
                }
            }
        }

        private static void OnStarStrokeColorChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            Rating rating = obj as Rating;

            if (rating != null)
            {
                Brush strokeColor = (Brush)e.NewValue;

                foreach (Star star in rating.stackPanelStars.Children)
                {
                    star.StrokeColor = strokeColor;
                }
            }
        }

        private void Rating_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeStars();
        }

        private void star_MouseLeave(object sender, MouseEventArgs e)
        {
            Star star = (Star)sender;

            if (!star.IsOn)
            {
                int current = (int)star.Tag;
                int value;

                foreach (Star str in stackPanelStars.Children)
                {
                    DisableStateChange(str);

                    value = (int)str.Tag;

                    if (value < current && value > Value)
                    {
                        str.State = StarState.Off;
                    }

                    EnableStateChange(str);
                }
            }
        }

        private void star_MouseEnter(object sender, MouseEventArgs e)
        {
            Star star = (Star)sender;

            if (!star.IsOn)
            {
                int current = (int)star.Tag;
                int value;

                foreach (Star str in stackPanelStars.Children)
                {
                    DisableStateChange(str);

                    value = (int)str.Tag;

                    if (value < current)
                    {
                        str.State = StarState.On;
                    }

                    EnableStateChange(str);
                }
            }
        }

        private void star_StateChanged(object sender, StarStateChangedEventArgs e)
        {
            Star star = (Star)sender;

            int current = (int)star.Tag;

            bool reset = (current < Value);

            int value;

            foreach (Star str in stackPanelStars.Children)
            {
                value = (int)str.Tag;

                DisableStateChange(str);

                if (value < current)
                {
                    str.State = StarState.On;
                }
                else if (value > current)
                {
                    str.State = StarState.Off;
                }
                else if (value == current && reset)
                {
                    str.State = StarState.On;
                }

                EnableStateChange(str);
            }

            Value = current;
        }

        #endregion

        #region Methods

        private void InitializeStars()
        {
            stackPanelStars.Children.Clear();

            int value = 1;
            

            for (int i = 0; i < Maximum; i++)
            {
                Star star = new Star();
                star.OnColor = StarOnColor;
                star.OffColor = StarOffColor;
                star.StrokeColor = StarStrokeColor;
                star.Tag = value;
                star.StateChanged += star_StateChanged;
                star.MouseEnter += star_MouseEnter;
                star.MouseLeave += star_MouseLeave;
                
                value++;

                stackPanelStars.Children.Insert(i, star);
            }

            Star lastStar = (Star) stackPanelStars.Children[Maximum-1];
            star_MouseEnter(lastStar,null);
            star_MouseLeave(lastStar,null);
        }

        private void EnableStateChange(Star str)
        {
            str.StateChanged += star_StateChanged;
        }

        private void DisableStateChange(Star str)
        {
            str.StateChanged -= star_StateChanged;
        }

        private void OnRatingChanged()
        {
            if (RatingChanged != null)
            {
                RatingChanged(this, new RatingChangedEventArgs(Value));

            }
        }

        #endregion
    }

    /// <summary>
    /// Event arguments for the rating change.
    /// </summary>
    public class RatingChangedEventArgs
    {
        /// <summary>
        /// Gets the value of the rating.
        /// </summary>
        public int Value { get; set; }

        public RatingChangedEventArgs(int value)
        {
            Value = value;
        }
    }
}
