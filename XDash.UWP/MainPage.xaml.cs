﻿using MVPathway.Builder;
using XDash.Framework.Emvy.UWP;

namespace XDash.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();

            LoadApplication(PathwayFactory.Create<XDash.App>(di =>
            {
                di.AddXDashPlatformDependencies();
            }));
        }
    }
}
