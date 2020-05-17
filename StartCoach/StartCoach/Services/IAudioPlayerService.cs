using Android.Media;
using System;
using System.Collections.Generic;
using System.Text;
using static StartCoach.ViewModels.StrankaViewModel;

namespace StartCoach.Services
{
    public interface IAudioPlayerService
    {
        void Play(Sound sound);
        void Play();

        //void PlayPripravit();
        //void PlayPozor();

        void Pause();
        Action OnFinishedPlaying { get; set; }
    }
}
