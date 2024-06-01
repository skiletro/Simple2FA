using OtpNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTotpApp.Shared
{
    // TODO: EXPLAIN WHAT THE FUCK THIS EVEN DOES?????
    public class TotpData : INotifyPropertyChanged
    {
        private string code;
        private int remainingTime;
        private float remainingTimeNormalised;

        public Totp Totp { get; }

        public string Code
        {
            get => code;
            set
            {
                if (code != value)
                {
                    code = value;
                    OnPropertyChanged(nameof(Code));
                }
            }
        }

        public int RemainingTime
        {
            get => remainingTime;
            set
            {
                if (remainingTime != value)
                {
                    remainingTime = value;
                    OnPropertyChanged(nameof(RemainingTime));
                }
            }
        }
        public float RemainingTimeNormalised
        {
            get => remainingTimeNormalised;
            set
            {
                if (remainingTimeNormalised != value)
                {
                    remainingTimeNormalised = value;
                    OnPropertyChanged(nameof(RemainingTimeNormalised));
                }
            }
        }

        public string Name { get; set; }

        public TotpData(string name, Totp totp)
        {
            Name = name;
            Totp = totp;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
