using MvvmEssentials.Core.Commands;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TodoApp.CustomControls
{
    [TemplatePart(Name = "Part_ContentHost", Type = typeof(string))]
    public class CustomTextBox : TextBox
    {
        #region DependencyProperties

        public static readonly DependencyProperty RegexProperty = DependencyProperty.Register(nameof(Regex), typeof(Regex), typeof(CustomTextBox), new PropertyMetadata(null));
        public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register(nameof(PlaceholderText), typeof(string), typeof(CustomTextBox), new PropertyMetadata(string.Empty));
        public static readonly DependencyProperty DisableCutCopyPasteProperty = DependencyProperty.Register(nameof(DisableCutCopyPaste), typeof(bool), typeof(CustomTextBox), new PropertyMetadata(false, DisableCutCopyPasteCallback));
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(nameof(CornerRadius), typeof(CornerRadius), typeof(CustomTextBox), new PropertyMetadata(default));
        public static readonly DependencyProperty ClearTextButtonVisibilityProperty = DependencyProperty.Register(nameof(ClearTextButtonVisibility), typeof(Visibility), typeof(CustomTextBox), new PropertyMetadata(Visibility.Collapsed));

        #endregion

        #region Public Properties

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        /// <summary>
        /// the placeholder text for this textbox
        /// </summary>
        public string PlaceholderText
        {
            get => (string)GetValue(PlaceholderTextProperty);
            set => SetValue(PlaceholderTextProperty, value);
        }

        /// <summary>
        /// The regex that limits the type of text that can be input for the textbox
        /// </summary>
        public Regex? Regex
        {
            get => (Regex?)GetValue(RegexProperty);
            set => SetValue(RegexProperty, value);
        }

        /// <summary>
        /// Specifies whether the cut copy and paste commands on this text box should be enabled.
        /// </summary>
        public bool DisableCutCopyPaste
        {
            get => (bool)GetValue(DisableCutCopyPasteProperty);
            init => SetValue(DisableCutCopyPasteProperty, value);
        }

        /// <summary>
        /// The visibility of the button which is binded to the <see cref="ClearTextCommand"/>
        /// </summary>
        public Visibility ClearTextButtonVisibility
        {
            get => (Visibility)GetValue(ClearTextButtonVisibilityProperty);
            set => SetValue(ClearTextButtonVisibilityProperty, value);
        }

        #endregion

        #region RelayCommands

        public RelayCommand ClearTextCommand => new(() => Text = "");

        #endregion

        #region Constructors

        public CustomTextBox()
        {
        }
        #endregion

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            //Do not let the text be input if the final text is not matching the used regex.
            if (Regex is not null && !Regex.IsMatch(Text.Insert(CaretIndex, e.Text)))
                e.Handled = true;
        }

        #region Private Methods

        /// <summary>
        /// Disables the cut copy paste commands for the textbox the subscribes to this method.
        /// </summary>
        /// <param name="sender">the sender of the event</param>
        /// <param name="e">the event arguments</param>
        private static void CutCopyPasteHandler(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Cut || e.Command == ApplicationCommands.Copy || e.Command == ApplicationCommands.Paste)
                e.Handled = true;
        }

        /// <summary>
        /// Add an <see cref="EventHandler"/> to disable the Cut, Copy and Paste commands for the textbox.
        /// </summary>
        /// <param name="d">the <see cref="CustomTextBox"/>which executed this method.</param>
        /// <param name="e">the event arguments.</param>
        private static void DisableCutCopyPasteCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is CustomTextBox t && t.DisableCutCopyPaste)
                CommandManager.AddPreviewExecutedHandler(t, CutCopyPasteHandler);
        }

        #endregion

    }
}