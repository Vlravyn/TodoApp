using System.Windows;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace TodoApp.AttachedProperties
{
    public class CheckBoxSymbolProperty : CheckBox
    {
        public SymbolRegular Symbol
        {
            get => (SymbolRegular)GetValue(SymbolProperty);
            set => SetValue(SymbolProperty, value);
        }

        public static readonly DependencyProperty SymbolProperty = DependencyProperty.RegisterAttached(nameof(Symbol), typeof(SymbolRegular), typeof(CheckBoxSymbolProperty), new PropertyMetadata(SymbolRegular.Empty));

        public static void SetSymbol(DependencyObject d, DependencyProperty dp) => d.SetValue(dp, SymbolProperty);

        public static SymbolRegular? GetSymbol(DependencyObject d) => (SymbolRegular)d.GetValue(SymbolProperty);
    }
}