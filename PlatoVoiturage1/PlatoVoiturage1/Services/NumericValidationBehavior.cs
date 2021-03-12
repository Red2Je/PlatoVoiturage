using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PlatoVoiturage1.Services
{
    class NumericValidationBehavior :Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        private static void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {

            if (!string.IsNullOrWhiteSpace(args.NewTextValue))
            {
                bool isValid = true;
                foreach (char elem in args.NewTextValue.ToCharArray()) {
                    isValid = isValid && char.IsDigit(elem);
                }
                ((Entry)sender).Text = isValid ? args.NewTextValue : args.NewTextValue.Remove(args.NewTextValue.Length - 1);
            }
        }
    }
}
