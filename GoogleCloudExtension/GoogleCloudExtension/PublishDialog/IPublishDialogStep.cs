﻿using System;
using System.Windows;

namespace GoogleCloudExtension.PublishDialog
{
    public interface IPublishDialogStep
    {
        bool CanGoNext { get; }

        event EventHandler CanGoNextChanged;

        bool CanPublish { get; }

        event EventHandler CanPublishChanged;

        FrameworkElement Content { get; }

        IPublishDialogStep Next();

        void Publish();

        void OnPushedToDialog(IPublishDialog dialog);
    }
}
