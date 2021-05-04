using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace PlatoVoiturage1.Services
{
    //This class is made to ensure that when entering phone numbers, the user can enter anything else than numbers.
    //This service was made because certain keyboards, even if they are set to "phone", puts "-" and spaces as available keys.
    class NumericValidationBehavior :Behavior<Entry>
    {
        //These two methodes below are here to bind our new event handler to the entry bloc
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

        //This method is the event handler that will check if our entry is valid. Each time the text changes, this event handler goes through th entire inut and checks for bad characters
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
