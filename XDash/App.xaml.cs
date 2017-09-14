﻿using DryIoc;
using MVPathway.Builder.Abstractions;
using MVPathway.MVVM.Abstractions;
using MVPathway.Utils.Presenters;
using Xamarin.Forms;
using XDash.Framework.Builder;
using XDash.Services;
using XDash.Services.Contracts;

namespace XDash
{
    public partial class App
    {
        public App()
        {
            InitializeComponent();
            MainPage = new ContentPage();
        }

        public override void Configure(IPathwayBuilder builder)
        {
            base.Configure(builder);
            builder
                .UsePresenter<StackPresenter>()
                .UseAppStart<XDashAppStart>()
                .UseXDash();
        }

        public override void ConfigureServices(IDiContainer container)
        {
            base.ConfigureServices(container);
            container.Register<ILocalizer, Localizer>();
        }

        public override void ConfigureViewModels(IViewModelManager vmManager)
        {
            vmManager.AutoScanAndRegister(GetType().GetAssembly());
            //vmManager.RegisterPageForViewModel<DevicesViewModel, DevicesPage>();
            //vmManager.RegisterPageForViewModel<SettingsViewModel, SettingsPage>();
        }
    }
}
