﻿using RateThisStuff_Client.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RateThisStuff_Client.Framework
{
    public class ViewModelBase:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
