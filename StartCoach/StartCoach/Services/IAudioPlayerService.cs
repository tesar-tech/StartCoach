using System;
using System.Collections.Generic;
using System.Text;

namespace StartCoach.Services
{
    public interface IAudioPlayerService
    {
        void Play(string pathToAudioFile);
        void Play();

        //void PlayPripravit();
        //void PlayPozor();

        void Pause();
        Action OnFinishedPlaying { get; set; }
    }
}
