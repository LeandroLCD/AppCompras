using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppCompras.Behaviors
{
    public class MaskMoneyBehavior : Behavior<Entry>
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

        private void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            var entry = sender as Entry;
            var text = entry.Text.Replace(".","").ToCharArray();
            string textModificado = string.Empty;

            if (text.Length <= 6)
            {

                textModificado = entry.Text.Replace(".", "");

                    switch (text.Length)
                    {
                        case 4:
                        textModificado = textModificado.Insert(1, ".");
                            break;
                        case 5:
                        textModificado = textModificado.Insert(2, ".");
                            break;
                        case 6:
                        textModificado = textModificado.Insert(3, ".");
                            break;
                        default:
                            break;

                     }
                
                entry.Text = textModificado;
                return;
            }
            if (text.Length > 6 )
            {
                textModificado = entry.Text.Replace(".", "");
                switch (text.Length)
                {
                    case 7:
                        textModificado = textModificado.Insert(1, ".").Insert(5,".");
                        break;
                    case 8:
                        textModificado = textModificado.Insert(2, ".").Insert(6, "."); ;
                        break;
                    case 9:
                        textModificado = textModificado.Insert(3, ".").Insert(7, "."); ;
                        break;
                    default:

                        break;

                }

                entry.Text = textModificado;
                return;

            }
            
            
        }
    }
}
