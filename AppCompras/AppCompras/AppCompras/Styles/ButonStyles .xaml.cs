﻿using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace AppCompras.Styles
{
    /// <summary>
    /// Class helps to reduce repetitive markup, and allows an apps appearance to be more easily changed.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ButonStyles
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Styles" /> class.
        /// </summary>
        public ButonStyles()
        {
            this.InitializeComponent();
        }
    }
}