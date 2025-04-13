// using Microsoft.Maui.Controls;
// using Microsoft.Maui.Controls.PlatformConfiguration;
// using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
// using System.Windows.Input;

// namespace AvaTerminal.Maui.Behaviors;

// public class EntryCompletedBehavior : Behavior<Microsoft.Maui.Controls.Entry>
// {
//     public static readonly BindableProperty CommandProperty =
//         BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(EntryCompletedBehavior));

//     public ICommand Command
//     {
//         get => (ICommand)GetValue(CommandProperty);
//         set => SetValue(CommandProperty, value);
//     }

//     protected override void OnAttachedTo(Microsoft.Maui.Controls.Entry bindable)
//     {
//         base.OnAttachedTo(bindable);
//         bindable.Completed += OnEntryCompleted;
//     }

//     protected override void OnDetachingFrom(Microsoft.Maui.Controls.Entry bindable)
//     {
//         base.OnDetachingFrom(bindable);
//         bindable.Completed -= OnEntryCompleted;
//     }

//     private void OnEntryCompleted(object? sender, EventArgs e)
//     {
//         if (Command?.CanExecute(null) == true)
//             Command.Execute(null);
//     }
// }
